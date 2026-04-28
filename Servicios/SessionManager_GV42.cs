using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SessionManager_GV42
    {
        // 1. Instancia única, privada y estática
        private static SessionManager_GV42 _instancia;

        // 2. El usuario logueado actualmente
        private Usuario _usuarioActual = null;

        // 3. Constructor privado: nadie puede hacer "new SessionManager_GV42()"
        private SessionManager_GV42() { }

        // 4. Punto de acceso global a la única instancia
        public static SessionManager_GV42 Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new SessionManager_GV42();
                return _instancia;
            }
        }

        // 5. Iniciar sesión: solo si no hay nadie logueado
        public bool IniciarSesion(Usuario usuario)
        {
            if (_usuarioActual != null)
                return false; // ya hay alguien logueado

            _usuarioActual = usuario;
            return true;
        }

        // 6. Cerrar sesión
        public void CerrarSesion()
        {
            _usuarioActual = null;
        }

        // 7. Verificar si hay sesión activa
        public bool HaySesionActiva()
        {
            return _usuarioActual != null;
        }

        // 8. Obtener el usuario actual
        public Usuario ObtenerUsuarioActual()
        {
            return _usuarioActual;
        }
    }
}
