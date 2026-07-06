using System;
using System.IO;

namespace Servicios.Instalacion
{
    public static class ConfiguracionBD_GV42
    {
        private const string NOMBRE_ARCHIVO = "conexion.cfg";

        private static string RutaArchivo
        {
            get
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(dir, NOMBRE_ARCHIVO);
            }
        }

        public static string LeerInstanciaGuardada()
        {
            try
            {
                if (!File.Exists(RutaArchivo)) return null;
                string contenido = File.ReadAllText(RutaArchivo).Trim();
                return string.IsNullOrEmpty(contenido) ? null : contenido;
            }
            catch { return null; }
        }

        public static void GuardarInstancia(string instancia)
        {
            try
            {
                File.WriteAllText(RutaArchivo, instancia ?? string.Empty);
            }
            catch { }
        }

        public static string ArmarConnectionString(string instancia, string nombreBd)
        {
            return $"Data Source={instancia};Initial Catalog=\"{nombreBd}\";Integrated Security=True";
        }

        public static string ArmarConnectionStringMaster(string instancia)
        {
            return $"Data Source={instancia};Initial Catalog=master;Integrated Security=True";
        }
    }
}
