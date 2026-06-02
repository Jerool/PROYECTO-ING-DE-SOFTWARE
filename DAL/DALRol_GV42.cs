using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    // DAL del Rol — entendido como un Composite que agrupa patentes y/o familias.
    // Maneja las tablas Roles, RolPatente y RolFamilia.
    //
    // Notar que algunos métodos (Listar / BuscarPorNombre) se mantienen en
    // DALUsuario_GV42 por compatibilidad con el código existente. Acá agregamos
    // los métodos NUEVOS específicos del Composite.
    public class DALRol_GV42
    {
        private readonly Acceso _acceso;
        private readonly DALFamilia_GV42 _dalFamilia;
        private readonly DALPatente_GV42 _dalPatente;

        public DALRol_GV42()
        {
            _acceso = Acceso.Instancia;
            _dalFamilia = new DALFamilia_GV42();
            _dalPatente = new DALPatente_GV42();
        }

        public List<Rol_GV42> ListarTodos()
        {
            string q = "SELECT Id, Nombre FROM Roles ORDER BY Nombre";
            DataTable dt = _acceso.leer(q, null);

            var lista = new List<Rol_GV42>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Rol_GV42
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                });
            }
            return lista;
        }

        // Carga el rol con TODOS sus componentes (patentes directas + familias
        // expandidas como árbol). Esto es lo que la BLL usa para evaluar permisos.
        public Rol_GV42 ObtenerArbol(int idRol)
        {
            // Cabecera
            string qRol = "SELECT Id, Nombre FROM Roles WHERE Id = @Id";
            DataTable dtRol = _acceso.leer(qRol, new[] { new SqlParameter("@Id", idRol) });
            if (dtRol.Rows.Count == 0) return null;

            var rol = new Rol_GV42
            {
                Id = Convert.ToInt32(dtRol.Rows[0]["Id"]),
                Nombre = dtRol.Rows[0]["Nombre"].ToString()
            };

            // Patentes directas del rol
            string qPat =
                "SELECT P.Id, P.Nombre, P.DataKey " +
                "FROM RolPatente RP " +
                "INNER JOIN Patente P ON P.Id = RP.IdPatente " +
                "WHERE RP.IdRol = @Id";
            DataTable dtPat = _acceso.leer(qPat, new[] { new SqlParameter("@Id", idRol) });
            foreach (DataRow row in dtPat.Rows)
            {
                rol.Hijos.Add(new Patente_GV42
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    DataKey = row["DataKey"].ToString()
                });
            }

            // Familias asociadas (cargadas como árbol completo, recursivo)
            string qFam = "SELECT IdFamilia FROM RolFamilia WHERE IdRol = @Id";
            DataTable dtFam = _acceso.leer(qFam, new[] { new SqlParameter("@Id", idRol) });
            foreach (DataRow row in dtFam.Rows)
            {
                int idFam = Convert.ToInt32(row["IdFamilia"]);
                Familia_GV42 fam = _dalFamilia.ObtenerArbol(idFam);
                if (fam != null) rol.Hijos.Add(fam);
            }

            return rol;
        }

        // Crea un rol con sus hijos directos. La validación de duplicados o de
        // que las familias existan se hace en la BLL.
        public int Crear(string nombre, List<int> idsPatentes, List<int> idsFamilias)
        {
            // Insertamos el rol y traemos su Id con SCOPE_IDENTITY().
            // Usamos un único batch así el SCOPE_IDENTITY queda en el mismo "scope"
            // que el INSERT y nos devuelve el Id correcto.
            string qIns = "INSERT INTO Roles (Nombre) VALUES (@Nombre); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            object res = _acceso.leerEscalar(qIns, new[] { new SqlParameter("@Nombre", nombre) });

            // Defensa: si por algún motivo no obtuvimos el Id, cortamos acá con
            // un mensaje claro en vez de intentar insertar con IdRol = 0 y reventar
            // la FK de RolPatente/RolFamilia.
            if (res == null || res == DBNull.Value)
                throw new Exception("No se pudo obtener el Id del rol recién creado.");

            int idRol = Convert.ToInt32(res);

            foreach (int idPat in idsPatentes ?? new List<int>())
            {
                _acceso.escribir(
                    "INSERT INTO RolPatente (IdRol, IdPatente) VALUES (@R, @P)",
                    new[] { new SqlParameter("@R", idRol), new SqlParameter("@P", idPat) });
            }

            foreach (int idFam in idsFamilias ?? new List<int>())
            {
                _acceso.escribir(
                    "INSERT INTO RolFamilia (IdRol, IdFamilia) VALUES (@R, @F)",
                    new[] { new SqlParameter("@R", idRol), new SqlParameter("@F", idFam) });
            }

            return idRol;
        }

        // ¿Hay algún usuario con este rol asignado? Lo usa la BLL para impedir
        // la eliminación de un rol en uso (regla de negocio del enunciado).
        public bool EstaEnUso(int idRol)
        {
            string q = "SELECT COUNT(1) FROM Usuario WHERE IdRol = @Id";
            object res = _acceso.leerEscalar(q, new[] { new SqlParameter("@Id", idRol) });
            return Convert.ToInt32(res) > 0;
        }

        // Cuántos usuarios tienen este rol — útil para mensajes del tipo
        // "no podés eliminarlo, hay 3 usuarios con este rol".
        public int CantidadUsuariosConRol(int idRol)
        {
            string q = "SELECT COUNT(1) FROM Usuario WHERE IdRol = @Id";
            object res = _acceso.leerEscalar(q, new[] { new SqlParameter("@Id", idRol) });
            return Convert.ToInt32(res);
        }

        public void Eliminar(int idRol)
        {
            // Las relaciones RolPatente y RolFamilia tienen ON DELETE CASCADE,
            // así que se limpian solas al borrar el rol.
            _acceso.escribir("DELETE FROM Roles WHERE Id = @Id",
                new[] { new SqlParameter("@Id", idRol) });
        }

        // Devuelve los Ids directos del rol — análogo al de familia. Sirve para
        // futuras validaciones de duplicados de roles (por composición), aunque
        // el enunciado solo lo pide para familias.
        public List<int> IdsPatentesDirectas(int idRol)
        {
            string q = "SELECT IdPatente FROM RolPatente WHERE IdRol = @Id ORDER BY IdPatente";
            DataTable dt = _acceso.leer(q, new[] { new SqlParameter("@Id", idRol) });
            return dt.Rows.Cast<DataRow>().Select(r => Convert.ToInt32(r["IdPatente"])).ToList();
        }

        public List<int> IdsFamiliasDirectas(int idRol)
        {
            string q = "SELECT IdFamilia FROM RolFamilia WHERE IdRol = @Id ORDER BY IdFamilia";
            DataTable dt = _acceso.leer(q, new[] { new SqlParameter("@Id", idRol) });
            return dt.Rows.Cast<DataRow>().Select(r => Convert.ToInt32(r["IdFamilia"])).ToList();
        }
    }
}
