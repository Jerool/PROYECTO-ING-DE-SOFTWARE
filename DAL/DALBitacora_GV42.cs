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
        private readonly DALModulo_GV42 _DALModulo;
        private readonly DALTipoEvento_GV42 _DALTipoEvento;

        // SELECT base con JOINs a Modulo y TipoEvento. Lo definimos una sola vez
        // porque lo usamos en Listar y en Filtrar.
        private const string SELECT_BASE =
            "SELECT E.UserName, " +
            "       E.IdModulo,     M.Nombre AS ModuloNombre, " +
            "       E.IdTipoEvento, T.Nombre AS TipoEventoNombre, " +
            "       E.Detalle, E.Criticidad, E.FechaHora " +
            "FROM EVENTOS E " +
            "INNER JOIN Modulo M     ON M.Id = E.IdModulo " +
            "INNER JOIN TipoEvento T ON T.Id = E.IdTipoEvento";

        public DALBitacora_GV42()
        {
            _acceso = Acceso.Instancia;
            _DALModulo = new DALModulo_GV42();
            _DALTipoEvento = new DALTipoEvento_GV42();
        }

        // Inserta una fila en EVENTOS. Si el registro vino con Modulo/TipoEvento
        // como "shell" (solo Nombre, sin Id) — caso típico desde BLLUsuario donde
        // se usan strings hardcodeados — los resolvemos contra las tablas catálogo.
        public void Guardar(Bitacora_GV42 registro)
        {
            int idModulo = ResolverIdModulo(registro.Modulo);
            int idTipoEvento = ResolverIdTipoEvento(registro.TipoEvento);

            string query =
                "INSERT INTO EVENTOS (UserName, IdModulo, IdTipoEvento, Detalle, Criticidad, FechaHora) " +
                "VALUES (@UserName, @IdModulo, @IdTipoEvento, @Detalle, @Criticidad, @FechaHora)";

            SqlParameter[] parametros = {
                new SqlParameter("@UserName",     registro.Login),
                new SqlParameter("@IdModulo",     idModulo),
                new SqlParameter("@IdTipoEvento", idTipoEvento),
                // Detalle puede ser null/"" — usamos DBNull.Value para no romper.
                new SqlParameter("@Detalle",      (object)registro.Detalle ?? DBNull.Value),
                new SqlParameter("@Criticidad",   registro.Criticidad),
                new SqlParameter("@FechaHora",    registro.FechaHora)
            };

            _acceso.escribir(query, parametros);
        }

        public List<Bitacora_GV42> Listar()
        {
            string query = SELECT_BASE + " ORDER BY E.FechaHora DESC";
            return MapearLista(_acceso.leer(query, null));
        }


        // Todos los filtros son opcionales y se combinan con AND.
        // Login: LIKE parcial. Modulo y Criticidad: exacto. Evento: LIKE prefijo
        // sobre el nombre del TipoEvento (igual que antes, pero ahora viene del JOIN).
        public List<Bitacora_GV42> Filtrar(string login, string modulo, string tipoEvento, string criticidad, DateTime fechaInicio, DateTime fechaFin)
        {
            StringBuilder sb = new StringBuilder(SELECT_BASE +
                " WHERE E.FechaHora BETWEEN @FechaInicio AND @FechaFin");

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaFin", fechaFin)
            };

            if (!string.IsNullOrWhiteSpace(login))
            {
                sb.Append(" AND E.UserName LIKE @Login");
                parametros.Add(new SqlParameter("@Login", "%" + login + "%"));
            }

            if (!string.IsNullOrWhiteSpace(modulo))
            {
                sb.Append(" AND M.Nombre = @Modulo");
                parametros.Add(new SqlParameter("@Modulo", modulo));
            }

            if (!string.IsNullOrWhiteSpace(tipoEvento))
            {
                sb.Append(" AND T.Nombre LIKE @TipoEvento");
                parametros.Add(new SqlParameter("@TipoEvento", tipoEvento + "%"));
            }

            if (!string.IsNullOrWhiteSpace(criticidad))
            {
                sb.Append(" AND E.Criticidad = @Criticidad");
                parametros.Add(new SqlParameter("@Criticidad", criticidad));
            }

            sb.Append(" ORDER BY E.FechaHora DESC");

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
                    Modulo = new Modulo_GV42
                    {
                        Id = Convert.ToInt32(row["IdModulo"]),
                        Nombre = row["ModuloNombre"].ToString()
                    },
                    TipoEvento = new TipoEvento_GV42
                    {
                        Id = Convert.ToInt32(row["IdTipoEvento"]),
                        Nombre = row["TipoEventoNombre"].ToString()
                    },
                    Detalle = row["Detalle"] == DBNull.Value ? "" : row["Detalle"].ToString(),
                    Criticidad = row["Criticidad"].ToString(),
                    FechaHora = Convert.ToDateTime(row["FechaHora"])
                });
            }
            return lista;
        }

        public List<string> ListarTiposEvento()
        {
            // Mantiene la firma anterior (lista de strings) para no romper la UI,
            // pero por debajo lee de TipoEvento (catálogo con FK).
            List<TipoEvento_GV42> entidades = _DALTipoEvento.ListarTodos();
            List<string> tipos = new List<string>();
            foreach (TipoEvento_GV42 t in entidades) tipos.Add(t.Nombre);
            return tipos;
        }

        public List<string> ListarModulos()
        {
            List<Modulo_GV42> entidades = _DALModulo.ListarTodos();
            List<string> modulos = new List<string>();
            foreach (Modulo_GV42 m in entidades) modulos.Add(m.Nombre);
            return modulos;
        }

        // Resuelve el Id del módulo a partir de la entidad que llegó.
        // Si la entidad ya tiene Id > 0, lo usamos. Si solo tiene Nombre,
        // lo buscamos en la tabla Modulo. Si no existe, reventamos con mensaje claro.
        private int ResolverIdModulo(Modulo_GV42 m)
        {
            if (m == null)
                throw new Exception("La bitácora no tiene módulo asignado.");

            if (m.Id > 0) return m.Id;

            Modulo_GV42 enBase = _DALModulo.BuscarPorNombre(m.Nombre);
            if (enBase == null)
                throw new Exception($"El módulo '{m.Nombre}' no existe en la tabla Modulo. " +
                                    "Agregalo al catálogo antes de registrar el evento.");
            return enBase.Id;
        }

        private int ResolverIdTipoEvento(TipoEvento_GV42 t)
        {
            if (t == null)
                throw new Exception("La bitácora no tiene tipo de evento asignado.");

            if (t.Id > 0) return t.Id;

            TipoEvento_GV42 enBase = _DALTipoEvento.BuscarPorNombre(t.Nombre);
            if (enBase == null)
                throw new Exception($"El tipo de evento '{t.Nombre}' no existe en la tabla TipoEvento. " +
                                    "Agregalo al catálogo antes de registrar el evento.");
            return enBase.Id;
        }
    }
}
