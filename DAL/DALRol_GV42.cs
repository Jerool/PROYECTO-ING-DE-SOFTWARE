using Servicios;                        // Para la entidad Rol_GV42
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    
    public class DALRol_GV42
    {
        private readonly Acceso _acceso;

        public DALRol_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public List<Rol_GV42> ListarTodos()
        {
            string query = "SELECT Id, Nombre FROM Roles ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<Rol_GV42> lista = new List<Rol_GV42>();
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

        // Busca un rol por su nombre exacto. Devuelve null si no existe.
        public Rol_GV42 BuscarPorNombre(string nombre)
        {
            string query = "SELECT Id, Nombre FROM Roles WHERE Nombre = @Nombre";
            SqlParameter[] p = { new SqlParameter("@Nombre", nombre) };
            DataTable dt = _acceso.leer(query, p);

            if (dt.Rows.Count == 0) return null;

            return new Rol_GV42
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()
            };
        }
    }
}
