using DAL;
using Servicios.Instalacion;

namespace BLL
{
    public static class BLLInstalador_GV42
    {
        public static string NombreBD
        {
            get { return InstaladorBD_GV42.NOMBRE_BD; }
        }

        public static bool ExisteBaseDatos(string instancia)
        {
            return InstaladorBD_GV42.ExisteBaseDatos(instancia);
        }

        public static void InstalarBaseDatos(string instancia)
        {
            InstaladorBD_GV42.InstalarBaseDatos(instancia);
        }

        public static void ConfigurarConexion(string instancia)
        {
            Acceso.ConnectionString = ConfiguracionBD_GV42.ArmarConnectionString(
                instancia, InstaladorBD_GV42.NOMBRE_BD);
        }
    }
}
