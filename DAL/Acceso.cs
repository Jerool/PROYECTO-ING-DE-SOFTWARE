using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    public class Acceso
    {
        // 1. Instancia privada estática
        private static Acceso _instancia;
        private SqlConnection _conexion;

        // 2. Constructor privado para evitar instanciación externa
        private Acceso()
        {
            try
            {
                _conexion.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=""Gestion Usuario"";Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
                _conexion.Open();
                Console.WriteLine("Conexión exitosa");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de conexión" + ex.Message);
            }
        }

        // 3. Propiedad pública para obtener la instancia única
        public static Acceso ObtenerInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new Acceso();
            }
            return _instancia;
        }

        // Método para ejecutar comandos de lectura (SELECT)
        public DataTable Leer(string consulta, SqlParameter[] parametros = null)
        {
            DataTable tabla = new DataTable();
            using (SqlCommand comando = new SqlCommand(consulta, _conexion))
            {
                if (parametros != null) comando.Parameters.AddRange(parametros);

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                try
                {
                    adaptador.Fill(tabla);
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error en la lectura de datos: " + ex.Message);
                }
            }
            return tabla;
        }

        // Método para ejecutar comandos de escritura (INSERT, UPDATE, DELETE)
        public int Escribir(string consulta, SqlParameter[] parametros = null)
        {
            int filasAfectadas = 0;
            using (SqlCommand comando = new SqlCommand(consulta, _conexion))
            {
                if (parametros != null) comando.Parameters.AddRange(parametros);

                try
                {
                    _conexion.Open();
                    filasAfectadas = comando.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error en la escritura de datos: " + ex.Message);
                }
                finally
                {
                    _conexion.Close();
                }
            }
            return filasAfectadas;
        }
    }
}
