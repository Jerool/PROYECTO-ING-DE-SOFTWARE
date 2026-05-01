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
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo FROM Usuario WHERE UserName = @UserName";

            SqlParameter[] parametros = { new SqlParameter("@UserName", login) };

            DataTable dt = _acceso.leer(query, parametros);

            if (dt.Rows.Count == 0)
                return null;

            return MapearFila(dt.Rows[0]);
        }

        public List<Usuario_GV42> ListarActivos()
        {
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo FROM Usuario WHERE Activo = 1";
            return MapearLista(_acceso.leer(query, null));
        }

        public List<Usuario_GV42> ListarTodos()
        {
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo FROM Usuario";
            return MapearLista(_acceso.leer(query, null));
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
            string query = "UPDATE Usuario SET Email = @Email WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@Email", email),
                new SqlParameter("@DNI",   dni)
            };
            _acceso.escribir(query, p);
        }

        public void ModificarRol(string dni, string rol)
        {
            string query = "UPDATE Usuario SET Rol = @Rol WHERE DNI = @DNI";
            SqlParameter[] p = {
                new SqlParameter("@Rol", rol),
                new SqlParameter("@DNI", dni)
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
            string query = "INSERT INTO Usuario (DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo) " +
                  "VALUES (@DNI, @Ape, @Nom, @Login, @Clave, @Rol, @Email, 0, 1)";
            SqlParameter[] p = {
                new SqlParameter("@DNI",   usuario.DNI),
                new SqlParameter("@Ape",   usuario.Apellido),
                new SqlParameter("@Nom",   usuario.Nombre),
                new SqlParameter("@Login", usuario.Login),
                new SqlParameter("@Clave", usuario.Contrasena),
                new SqlParameter("@Rol",   usuario.Rol),
                new SqlParameter("@Email", usuario.Email)
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
                Rol = row["Rol"].ToString(),
                Email = row["Email"].ToString(),
                Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public List<string> Listar()
        {
            string query = "SELECT Nombre FROM Roles ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<string> roles = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                roles.Add(row["Nombre"].ToString());
            }
            return roles;
        }
    }
}
