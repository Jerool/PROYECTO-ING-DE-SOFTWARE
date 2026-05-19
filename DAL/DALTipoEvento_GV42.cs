using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{

    public class DALTipoEvento_GV42
    {
        private readonly Acceso _acceso;

        public DALTipoEvento_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public List<TipoEvento_GV42> ListarTodos()
        {
            string query = "SELECT Id, Nombre FROM TipoEvento ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<TipoEvento_GV42> lista = new List<TipoEvento_GV42>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new TipoEvento_GV42
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                });
            }
            return lista;
        }

        public TipoEvento_GV42 BuscarPorNombre(string nombre)
        {
            string query = "SELECT Id, Nombre FROM TipoEvento WHERE Nombre = @Nombre";
            SqlParameter[] p = { new SqlParameter("@Nombre", nombre) };
            DataTable dt = _acceso.leer(query, p);

            if (dt.Rows.Count == 0) return null;

            return new TipoEvento_GV42
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()
            };
        }
    }
}
