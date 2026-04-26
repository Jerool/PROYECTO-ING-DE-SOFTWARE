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
    public class GestorBitacora_GV42
    {
        private readonly Acceso _acceso;

        public GestorBitacora_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public void Guardar(Bitacora registro)
        {
            string query = "INSERT INTO EVENTOS (UserName, Modulo, Evento, Criticidad, FechaHora) " +
                           "VALUES (@UserName, @Modulo, @Evento, @Criticidad, @FechaHora)";

            SqlParameter[] parametros = {
                new SqlParameter("@UserName",registro.Login),
                new SqlParameter("@Modulo",registro.Modulo),
                new SqlParameter("@Evento",registro.Evento),
                new SqlParameter("@Criticidad",registro.Criticidad),
                new SqlParameter("@FechaHora", registro.FechaHora)
            };

            _acceso.escribir(query, parametros);
        }

        public List<Bitacora> Listar()
        {
            string query = "SELECT UserName, Modulo, Evento, Criticidad, FechaHora FROM EVENTOS ORDER BY FechaHora DESC";

            DataTable dt = _acceso.leer(query, null);

            List<Bitacora> lista = new List<Bitacora>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Bitacora
                {
                    Login = row["UserName"].ToString(),
                    Modulo = row["Modulo"].ToString(),
                    Evento = row["Evento"].ToString(),
                    Criticidad = row["Criticidad"].ToString(),
                    FechaHora = Convert.ToDateTime(row["FechaHora"])
                });
            }

            return lista;
        }
    }
}
