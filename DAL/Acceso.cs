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
        private static Acceso _instancia;
        protected SqlConnection conexion = null;
        

        private Acceso()
        {
            conexion = new SqlConnection();
        }

        public static Acceso Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Acceso();
                }
                return _instancia;
            }
        }

        public void conectar()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.ConnectionString = @"Data Source=DESKTOP-L5HB13;Initial Catalog=""Gestion Usuario"";Integrated Security=True"; /*(localdb)\MSSQLLocalDB JERE*/
                    conexion.Open();
                    Console.WriteLine("Conexión exitosa");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de conexión" + ex.Message);
            }
        }

        public void desconectar()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                    Console.WriteLine("Desconexión exitosa.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al desconectar: " + ex.Message);
            }
        }

        public SqlTransaction IniciarTransaccion()
        {
            conectar();
            return conexion.BeginTransaction();
        }

        public void ConfirmarTransaccion(SqlTransaction tx)
        {
            try
            {
                if (tx != null)
                {
                    tx.Commit();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al confirmar la transacción: " + ex.Message);
            }
            finally
            {
                desconectar();
            }
        }

        public void CancelarTransaccion(SqlTransaction tx)
        {
            try
            {
                if (tx != null)
                {
                    tx.Rollback();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al cancelar la transacción: " + ex.Message);
            }
        }

        public int escribir(string query, SqlParameter[] parametro)
        {
            SqlTransaction tx = null;
            int filasAfectadas = 0;
            SqlCommand comando = new SqlCommand();
            try
            {
                comando.Parameters.Clear();
                tx = IniciarTransaccion();
                comando.Connection = tx.Connection; // Asignar la conexión de la transacción al comando
                comando.Transaction = tx; // Asignar la transacción al comando
                comando.CommandText = query;
                if (parametro != null)
                {
                    comando.Parameters.AddRange(parametro);
                }
                filasAfectadas = comando.ExecuteNonQuery();
                ConfirmarTransaccion(tx);
                return filasAfectadas;
            }
            catch (Exception ex)
            {
                CancelarTransaccion(tx);
                Console.WriteLine("error", ex.Message);
                return 0;
            }
        }

        public DataTable leer(string query, SqlParameter[] parametro)
        {
            SqlCommand comando = new SqlCommand();
            DataTable dt = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            try
            {
                conectar();
                comando.Connection = conexion;
                comando.CommandText = query;
                if (parametro != null)
                    comando.Parameters.AddRange(parametro);

                adaptador.SelectCommand = comando;
                adaptador.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en Leer: " + ex.Message);
            }
            finally
            {
                desconectar();
            }
            return dt;
        }

        public object leerEscalar(string query, SqlParameter[] parametro)
        {
            SqlCommand comando = new SqlCommand();
            object resultado = null;
            try
            {
                conectar();
                comando.Connection = conexion;
                comando.CommandText = query;

                if (parametro != null)
                {
                    comando.Parameters.Clear();
                    comando.Parameters.AddRange(parametro);
                }
                resultado = comando.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al ejecutar ExecuteScalar: " + ex.Message);
            }
            finally
            {
                comando.Parameters.Clear();
                desconectar();
            }
            return resultado;
        }
    }
}
