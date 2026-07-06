using BLL;
using Servicios;
using Servicios.Instalacion;
using System;
using System.Windows.Forms;

namespace PROYECTO_ING_DE_SOFTWARE
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IdiomaManager_GV42.Instancia.CambiarIdioma(IdiomaManager_GV42.IDIOMA_POR_DEFECTO);

            if (!ConfigurarConexionBD()) return;

            Application.Run(new FRMIniciarSesion());
        }

        private static bool ConfigurarConexionBD()
        {
            string instancia = ConfiguracionBD_GV42.LeerInstanciaGuardada();

            if (!string.IsNullOrEmpty(instancia))
            {
                try
                {
                    if (BLLInstalador_GV42.ExisteBaseDatos(instancia))
                    {
                        BLLInstalador_GV42.ConfigurarConexion(instancia);
                        return true;
                    }
                }
                catch
                {
                }
            }

            using (var frm = new FRMSeleccionInstancia())
            {
                DialogResult r = frm.ShowDialog();
                if (r != DialogResult.OK || string.IsNullOrEmpty(frm.InstanciaElegida))
                    return false;

                BLLInstalador_GV42.ConfigurarConexion(frm.InstanciaElegida);
                return true;
            }
        }
    }
}
