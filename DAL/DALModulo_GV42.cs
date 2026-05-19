using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // DAL de la tabla Modulo. Es un catálogo cerrado y chiquito (Login,
    // Gestión Usuario, Contraseña, Usuario). Lo usamos para:
    //  - Llenar el combo de filtros en FRMBitacoraDeEventos.
    //  - Resolver el Id por nombre cuando vamos a insertar en EVENTOS.
    public class DALModulo_GV42
    {
        private readonly Acceso _acceso;

        public DALModulo_GV42()
        {
            _acceso = Acceso.Instancia;
        }

        public List<Modulo_GV42> ListarTodos()
        {
            string query = "SELECT Id, Nombre FROM Modulo ORDER BY Nombre";
            DataTable dt = _acceso.leer(query, null);

            List<Modulo_GV42> lista = new List<Modulo_GV42>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Modulo_GV42
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                });
            }
            return lista;
        }

        // Devuelve el Modulo (con su Id) buscando por nombre. null si no existe.
        // Útil para los lugares del código que solo conocen el módulo por su nombre.
        public Modulo_GV42 BuscarPorNombre(string nombre)
        {
            string query = "SELECT Id, Nombre FROM Modulo WHERE Nombre = @Nombre";
            SqlParameter[] p = { new SqlParameter("@Nombre", nombre) };
            DataTable dt = _acceso.leer(query, p);

            if (dt.Rows.Count == 0) return null;

            return new Modulo_GV42
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()
            };
        }
    }
}
