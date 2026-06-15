using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Servicios;

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

            Application.Run(new FRMIniciarSesion());
        }
    }
}
