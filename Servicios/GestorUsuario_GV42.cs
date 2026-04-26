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

            SqlParameter[] parametros = {new SqlParameter("@UserName", login)};

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
            string query = "UPDATE Usuario SET Intentos = @Intentos, Bloqueo = @Bloqueo WHERE Login = @Login";

            SqlParameter[] parametros = {
                new SqlParameter("@Intentos", intentos),
                new SqlParameter("@Bloqueo",  bloqueo),
                new SqlParameter("@Login",    login)
            };

            _acceso.escribir(query, parametros);
        }

        public void ResetearIntentos(string login)
        {
            string query = "UPDATE Usuario SET Intentos = 0 WHERE Login = @Login";

            SqlParameter[] parametros = {
                new SqlParameter("@Login", login)
            };

            _acceso.escribir(query, parametros);
        }
    }
}
