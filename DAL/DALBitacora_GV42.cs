using Servicios;                        
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    
    
    public class DALBitacora_GV42 : IbitacoraDAL_GV42
    {
        private readonly Acceso _acceso;

        public DALBitacora_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        
        public void Guardar(Bitacora_GV42 registro)
        {
            string query = "INSERT INTO EVENTOS (UserName, Modulo, Evento, Criticidad, FechaHora) " +
                           "VALUES (@UserName, @Modulo, @Evento, @Criticidad, @FechaHora)";

            SqlParameter[] parametros = {
                new SqlParameter("@UserName",   registro.Login),
                new SqlParameter("@Modulo",     registro.Modulo),
                new SqlParameter("@Evento",     registro.Evento),
                new SqlParameter("@Criticidad", registro.Criticidad),
                new SqlParameter("@FechaHora",  registro.FechaHora)
            };

            _acceso.escribir(query, parametros);
        }

        public List<Bitacora_GV42> Listar()
        {
            string query = "SELECT UserName, Modulo, Evento, Criticidad, FechaHora FROM EVENTOS ORDER BY FechaHora DESC";
            return MapearLista(_acceso.leer(query, null));
        }


        //Todos los filtros son opcionales y se combinan entre sí mediante el operador lógico AND, lo que significa que el sistema retornará únicamente los registros que cumplan simultáneamente con todos los criterios que hayan sido completados.
        public List<Bitacora_GV42> Filtrar(string login, string modulo, string evento,string criticidad, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder sb = new StringBuilder(
            "SELECT UserName, Modulo, Evento, Criticidad, FechaHora " +
            "FROM EVENTOS WHERE FechaHora BETWEEN @FechaInicio AND @FechaFin");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaFin", fechaFin)
            };

            // Login: si vino texto, filtramos por LIKE para permitir búsqueda parcial.  
            if (!string.IsNullOrWhiteSpace(login))
            {
                sb.Append(" AND UserName LIKE @Login");
                parametros.Add(new SqlParameter("@Login", "%" + login + "%"));
            }

            // Módulo: comparación exacta (los valores vienen del combo).
            if (!string.IsNullOrWhiteSpace(modulo))
            {
                sb.Append(" AND Modulo = @Modulo");
                parametros.Add(new SqlParameter("@Modulo", modulo));
            }

            // Evento: usamos LIKE con prefijo porque algunos eventos se guardan con
            // detalle dinámico al final. 
            if (!string.IsNullOrWhiteSpace(evento))
            {
                sb.Append(" AND Evento LIKE @Evento");
                parametros.Add(new SqlParameter("@Evento", evento + "%"));
            }

            if (!string.IsNullOrWhiteSpace(criticidad))
            {
                sb.Append(" AND Criticidad = @Criticidad");
                parametros.Add(new SqlParameter("@Criticidad", criticidad));
            }

            sb.Append(" ORDER BY FechaHora DESC");

            return MapearLista(_acceso.leer(sb.ToString(), parametros.ToArray()));
        }

        private List<Bitacora_GV42> MapearLista(DataTable dt)
        {
            List<Bitacora_GV42> lista = new List<Bitacora_GV42>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Bitacora_GV42
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

        public List<string> ListarTiposEvento()
        {
            string query = "SELECT Nombre FROM TipoEvento ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<string> tipos = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                tipos.Add(row["Nombre"].ToString());
            }
            return tipos;
        }

        public List<string> ListarModulos()
        {
            string query = "SELECT Nombre FROM Modulo ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<string> modulos = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                modulos.Add(row["Nombre"].ToString());
            }
            return modulos;
        }

    }
}
