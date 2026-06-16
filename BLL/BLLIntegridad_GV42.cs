using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BLL
{
    public class BLLIntegridad_GV42
    {
        private readonly DALIntegridad_GV42 _dal;

        private const string NOMBRE_BD = "Gestion Usuario";
        private const string CONN_MASTER =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
        private const string CARPETA_BACKUPS = @"C:\Backups\GestionUsuario";
        private const int CANTIDAD_BACKUPS_A_CONSERVAR = 5;
        public const int INTERVALO_BACKUP_HORAS = 3;

        private static Timer _timerBackup;
        private static readonly object _lockTimer = new object();

        public BLLIntegridad_GV42()
        {
            _dal = new DALIntegridad_GV42();
        }

        public ResultadoIntegridad Verificar()
        {
            var resultado = new ResultadoIntegridad();

            if (!_dal.ExisteAlgunDVV())
            {
                Recalcular();
                resultado.EsBootstrap = true;
                resultado.EsIntegra = true;
                return resultado;
            }

            foreach (var tabla in DALIntegridad_GV42.TABLAS_PROTEGIDAS)
            {
                Dictionary<string, string> dvhsAhora = _dal.CalcularDVHsTabla(tabla);
                Dictionary<string, string> dvhsBd    = _dal.ObtenerDVHsAlmacenados(tabla);

                bool comprometida = false;

                if (dvhsAhora.Count != dvhsBd.Count) comprometida = true;
                else
                {
                    foreach (var kv in dvhsAhora)
                    {
                        if (!dvhsBd.TryGetValue(kv.Key, out string bd) || bd != kv.Value)
                        {
                            comprometida = true;
                            break;
                        }
                    }
                }

                if (!comprometida)
                {
                    string dvvAhora = CalculadorIntegridad_GV42.CalcularDVV(dvhsAhora.Values);
                    string dvvBd    = _dal.ObtenerDVVAlmacenado(tabla);
                    if (dvvAhora != dvvBd) comprometida = true;
                }

                if (comprometida) resultado.TablasComprometidas.Add(tabla);
            }

            resultado.EsIntegra = resultado.TablasComprometidas.Count == 0;
            return resultado;
        }

        public void Recalcular()
        {
            foreach (var tabla in DALIntegridad_GV42.TABLAS_PROTEGIDAS)
                RecalcularTabla(tabla);
        }

        public void RecalcularTabla(string nombreTabla)
        {
            Dictionary<string, string> dvhs = _dal.CalcularDVHsTabla(nombreTabla);
            _dal.GuardarDVHs(nombreTabla, dvhs);
            string dvv = CalculadorIntegridad_GV42.CalcularDVV(dvhs.Values);
            _dal.GuardarDVV(nombreTabla, dvv);
        }

        public string HacerBackupAutomatico()
        {
            if (!Directory.Exists(CARPETA_BACKUPS))
                Directory.CreateDirectory(CARPETA_BACKUPS);

            string nombreArchivo = $"GestionUsuario_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string rutaCompleta = Path.Combine(CARPETA_BACKUPS, nombreArchivo);

            _dal.HacerBackupSQL(CONN_MASTER, NOMBRE_BD, rutaCompleta);

            LimpiarBackupsViejos();
            return rutaCompleta;
        }

        public string ObtenerUltimoBackup()
        {
            if (!Directory.Exists(CARPETA_BACKUPS)) return null;

            var archivos = Directory.GetFiles(CARPETA_BACKUPS, "*.bak");
            if (archivos.Length == 0) return null;

            return archivos
                .Select(f => new FileInfo(f))
                .OrderByDescending(fi => fi.CreationTime)
                .First()
                .FullName;
        }

        private void LimpiarBackupsViejos()
        {
            if (!Directory.Exists(CARPETA_BACKUPS)) return;

            var archivos = Directory.GetFiles(CARPETA_BACKUPS, "*.bak")
                .Select(f => new FileInfo(f))
                .OrderByDescending(fi => fi.CreationTime)
                .ToList();

            for (int i = CANTIDAD_BACKUPS_A_CONSERVAR; i < archivos.Count; i++)
            {
                try { archivos[i].Delete(); } catch { }
            }
        }

        public void RestaurarUltimoBackup()
        {
            string ruta = ObtenerUltimoBackup();
            if (string.IsNullOrEmpty(ruta))
                throw new Exception("No hay backups disponibles para restaurar. " +
                                    "Iniciá sesión como admin al menos una vez para generar el primero.");
            RestaurarBackupDesdeRuta(ruta);
        }

        public void RestaurarBackupDesdeRuta(string rutaArchivoBak)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivoBak))
                throw new Exception("La ruta del backup es obligatoria.");
            if (!File.Exists(rutaArchivoBak))
                throw new Exception($"El archivo de backup no existe: {rutaArchivoBak}");

            _dal.RestaurarBackupSQL(CONN_MASTER, NOMBRE_BD, rutaArchivoBak);
        }

        public void IniciarBackupsProgramados()
        {
            lock (_lockTimer)
            {
                if (DebeHacerBackupAhora())
                    EjecutarBackupProgramado(null);

                if (_timerBackup != null) return;

                TimeSpan intervalo = TimeSpan.FromHours(INTERVALO_BACKUP_HORAS);
                _timerBackup = new Timer(EjecutarBackupProgramado, null, intervalo, intervalo);
            }
        }

        public void DetenerBackupsProgramados()
        {
            lock (_lockTimer)
            {
                _timerBackup?.Dispose();
                _timerBackup = null;
            }
        }

        private bool DebeHacerBackupAhora()
        {
            string ultimo = ObtenerUltimoBackup();
            if (string.IsNullOrEmpty(ultimo)) return true;

            try
            {
                FileInfo fi = new FileInfo(ultimo);
                TimeSpan transcurrido = DateTime.Now - fi.CreationTime;
                return transcurrido.TotalHours >= INTERVALO_BACKUP_HORAS;
            }
            catch
            {
                return true;
            }
        }

        private void EjecutarBackupProgramado(object state)
        {
            try
            {
                ResultadoIntegridad res = Verificar();
                if (!res.EsIntegra) return;

                string ruta = HacerBackupAutomatico();

                BLLBitacora_GV42.Instancia.RegistrarEvento(
                    "SISTEMA", "Admin", "Backup automatico generado",
                    $"Ruta: {ruta}", "Baja");
            }
            catch { }
        }
    }

    public class ResultadoIntegridad
    {
        public bool EsIntegra { get; set; }
        public bool EsBootstrap { get; set; }
        public List<string> TablasComprometidas { get; set; } = new List<string>();
    }
}
