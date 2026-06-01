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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cargamos el idioma por defecto ANTES de abrir el primer form,
            // así cuando los formularios se construyen ya tienen las traducciones
            // disponibles en el IdiomaManager.
            IdiomaManager_GV42.Instancia.CambiarIdioma(IdiomaManager_GV42.IDIOMA_POR_DEFECTO);

            Application.Run(new FRMIniciarSesion());
        }
    }
}
