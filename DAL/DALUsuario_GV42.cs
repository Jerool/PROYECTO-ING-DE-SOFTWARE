using Servicios;                        // Para usar la entidad Usuario_GV42
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public class DALUsuario_GV42
    {

        private readonly Acceso _acceso;

        // SELECT base con el JOIN a Roles. Lo definimos una sola vez porque lo
        // usamos en todos los métodos de lectura. Trae Id y Nombre del rol así
        // armamos el Rol_GV42 directo en MapearFila.
        private const string SELECT_BASE =
            "SELECT U.DNI, U.Apellido, U.Nombre, U.UserName, U.Contrasena, " +
            "       U.IdRol, R.Nombre AS RolNombre, " +
            "       U.Email, U.Bloqueo, U.Activo " +
            "FROM Usuario U " +
            "INNER JOIN Roles R ON R.Id = U.IdRol";

        public DALUsuario_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public bool ExisteDNI(string dni)
        {
            string query = "SELECT COUNT(1) FROM Usuario WHERE DNI = @DNI";
            SqlParameter[] parametros = { new SqlParameter("@DNI", dni) };
            object resultado = _acceso.leerEscalar(query, parametros);
            return resultado != null && Convert.ToInt32(resultado) > 0;
        }

        public Usuario_GV42 BuscarPorLogin(string login)
        {
            string query = SELECT_BASE + " WHERE U.UserName = @UserName";

            SqlParameter[] parametros = { new SqlParameter("@UserName", login) };

            DataTable dt = _acceso.leer(query, parametros);

            if (dt.Rows.Count == 0)
                return null;

            return MapearFila(dt.Rows[0]);
        }

        public List<Usuario_GV42> ListarActivos()
        {
            string query = SELECT_BASE + " WHERE U.Activo = 1";
            return MapearLista(_acceso.leer(query, null));
        }

        public List<Usuario_GV42> ListarTodos()
        {
            return MapearLista(_acceso.leer(SELECT_BASE, null));
        }

        public void Bloquear(string login)
        {
            string query = "UPDATE Usuario SET Bloqueo = 1 WHERE UserName = @UserName";
            SqlParameter[] parametros = {
                new SqlParameter("@UserName", login)
            };
            _acceso.escribir(query, parametros);
        }

        public void Desbloquear(string dni, string contrasenaCifrada)
        {
            string query = "UPDATE Usuario SET Bloqueo = 0, Contrasena = @Contrasena WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@Contrasena", contrasenaCifrada),
                new SqlParameter("@DNI", dni)
            };
            _acceso.escribir(query, p);
        }

        public void ActivarDesactivar(string dni, bool activo)
        {
            string query = "UPDATE Usuario SET Activo = @Activo WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@Activo", activo),
                new SqlParameter("@DNI",    dni)
            };
            _acceso.escribir(query, p);
        }

        public void ModificarEmail(string dni, string email)
        {
            // Email se guarda CIFRADO en la base. La desencriptación se hace en MapearFila.
            string emailCifrado = EncriptadorReversible_GV42.Instancia.Encriptar(email);
            string query = "UPDATE Usuario SET Email = @Email WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@Email", emailCifrado),
                new SqlParameter("@DNI",   dni)
            };
            _acceso.escribir(query, p);
        }

        // Ahora se modifica el rol por Id (FK), no por nombre.
        public void ModificarRol(string dni, int idRol)
        {
            string query = "UPDATE Usuario SET IdRol = @IdRol WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@IdRol", idRol),
                new SqlParameter("@DNI",   dni)
            };
            _acceso.escribir(query, p);
        }

        public void CambiarContrasena(string login, string contrasenaCifrada)
        {
            string query = "UPDATE Usuario SET Contrasena = @Contrasena WHERE UserName = @Login";
            SqlParameter[] p = {
                new SqlParameter("@Contrasena", contrasenaCifrada),
                new SqlParameter("@Login", login)
            };
            _acceso.escribir(query, p);
        }

        public int AgregarUsuario(Usuario_GV42 usuario)
        {
            // Defensivo: si llegó sin rol, reventamos acá con un mensaje claro
            // antes de mandar un NULL a una columna NOT NULL.
            if (usuario.Rol == null)
                throw new Exception("El usuario no tiene rol asignado.");

            string query = "INSERT INTO Usuario (DNI, Apellido, Nombre, UserName, Contrasena, IdRol, Email, Bloqueo, Activo) " +
                  "VALUES (@DNI, @Ape, @Nom, @Login, @Clave, @IdRol, @Email, 0, 1)";
            SqlParameter[] p = {
                new SqlParameter("@DNI",   usuario.DNI),
                new SqlParameter("@Ape",   usuario.Apellido),
                new SqlParameter("@Nom",   usuario.Nombre),
                new SqlParameter("@Login", usuario.Login),
                new SqlParameter("@Clave", usuario.Contrasena),
                new SqlParameter("@IdRol", usuario.Rol.Id),
                // Email se guarda cifrado con AES (encriptación reversible).
                new SqlParameter("@Email", EncriptadorReversible_GV42.Instancia.Encriptar(usuario.Email))
            };
            return _acceso.escribir(query, p);
        }

        private List<Usuario_GV42> MapearLista(DataTable dt)
        {
            List<Usuario_GV42> lista = new List<Usuario_GV42>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearFila(row));
            }
            return lista;
        }

        private Usuario_GV42 MapearFila(DataRow row)
        {
            return new Usuario_GV42
            {
                DNI = row["DNI"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Nombre = row["Nombre"].ToString(),
                Login = row["UserName"].ToString(),
                Contrasena = row["Contrasena"].ToString(),
                // Reconstruimos la entidad Rol con el Id (FK) y el Nombre que
                // trajimos del JOIN. De acá en más, toda la app trabaja con
                // Rol_GV42 en lugar de un string suelto.
                Rol = new Rol_GV42
                {
                    Id = Convert.ToInt32(row["IdRol"]),
                    Nombre = row["RolNombre"].ToString()
                },
                // Email viene cifrado de la base — lo desencriptamos acá para que
                // todo el resto del sistema (BLL, UI) trabaje con el valor en claro.
                Email = EncriptadorReversible_GV42.Instancia.Desencriptar(row["Email"].ToString()),
                Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }
    }
}
