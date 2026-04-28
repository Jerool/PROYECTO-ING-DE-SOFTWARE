using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class GestorUsuario_GV42
    {
        private readonly Acceso _acceso;

        public GestorUsuario_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public Usuario BuscarPorLogin(string login)
        {
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo, Intentos FROM Usuario WHERE UserName = @UserName";

            SqlParameter[] parametros = { new SqlParameter("@UserName", login) };

            DataTable dt = _acceso.leer(query, parametros);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new Usuario
            {
                DNI = row["DNI"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Nombre = row["Nombre"].ToString(),
                Login = row["UserName"].ToString(),
                Contrasena = row["Contrasena"].ToString(),
                Rol = row["Rol"].ToString(),
                Email = row["Email"].ToString(),
                Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                Activo = Convert.ToBoolean(row["Activo"]),
                Intentos = Convert.ToInt32(row["Intentos"])
            };
        }

        public void ActualizarIntentos(string login, int intentos, bool bloqueo)
        {
            string query = "UPDATE Usuario SET Intentos = @Intentos, Bloqueo = @Bloqueo WHERE UserName = @UserName";

            SqlParameter[] parametros = {
                new SqlParameter("@Intentos", intentos),
                new SqlParameter("@Bloqueo",  bloqueo),
                new SqlParameter("@UserName",    login)
            };

            _acceso.escribir(query, parametros);
        }

        public void ResetearIntentos(string login)
        {
            string query = "UPDATE Usuario SET Intentos = 0 WHERE UserName = @UserName";

            SqlParameter[] parametros = {
                new SqlParameter("@UserName", login)
            };

            _acceso.escribir(query, parametros);
        }

        public void AgregarUsuario(Usuario usuario)
        {
            string query = "INSERT INTO Usuario (DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo, Intentos) " +
                  "VALUES (@DNI, @Ape, @Nom, @Login, @Clave, @Rol, @Email, 1, 0, 0)";
            SqlParameter[] p = {
                new SqlParameter("@DNI",   usuario.DNI),
                new SqlParameter("@Ape",   usuario.Apellido),
                new SqlParameter("@Nom",   usuario.Nombre),
                new SqlParameter("@Login", usuario.Login),
                new SqlParameter("@Clave", usuario.Contrasena),
                new SqlParameter("@Rol",   usuario.Rol),
                new SqlParameter("@Email", usuario.Email)
            };
            _acceso.escribir(query, p);
        }

        public List<Usuario> ListarActivos()
        {
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo, Intentos FROM Usuario WHERE Activo = 1";
            return MapearLista(_acceso.leer(query, null));
        }

        public List<Usuario> ListarTodos()
        {
            string query = "SELECT DNI, Apellido, Nombre, UserName, Contrasena, Rol, Email, Bloqueo, Activo, Intentos FROM Usuario";
            return MapearLista(_acceso.leer(query, null));
        }

        public void Desbloquear(string dni, string contrasenaCifrada)
        {
            string query = "UPDATE Usuario SET Bloqueo = 0, Intentos = 0, Contrasena = @Contrasena WHERE DNI = @DNI";
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

        private List<Usuario> MapearLista(DataTable dt)
        {
            List<Usuario> lista = new List<Usuario>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Usuario
                {
                    DNI = row["DNI"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    Nombre = row["Nombre"].ToString(),
                    Login = row["UserName"].ToString(),
                    Contrasena = row["Contrasena"].ToString(),
                    Rol = row["Rol"].ToString(),
                    Email = row["Email"].ToString(),
                    Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    Intentos = Convert.ToInt32(row["Intentos"])
                });
            }
            return lista;
        }

        public void CambiarContrasena(string login, string contrasenaCifrada)
        {
            string query = "UPDATE Usuario SET Contrasena = @Contrasena WHERE UserName = @Login";
        SqlParameter[] p = { new SqlParameter("@Contrasena", contrasenaCifrada), new SqlParameter("@Login", login)
    };
            _acceso.escribir(query, p);
        }
    }
}
