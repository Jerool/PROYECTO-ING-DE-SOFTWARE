using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SessionManager_GV42
    {
        private static Usuario _usuarioActual = null;

        // Verificar si hay una sesión activa
        public static bool HaySesionActiva()
        {
            return _usuarioActual != null;
        }

        // Iniciar sesión (guardar usuario)
        public static void IniciarSesion(Usuario usuario)
        {
            _usuarioActual = usuario;
        }

        // Cerrar sesión
        public static void CerrarSesion()
        {
            _usuarioActual = null;
        }

        // Obtener el usuario actual
        public static Usuario ObtenerUsuarioActual()
        {
            return _usuarioActual;
        } 
    }
}
