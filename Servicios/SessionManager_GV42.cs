using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{


    public class SessionManager_GV42
    {
        private static SessionManager_GV42 _instancia;


        private Usuario_GV42 _usuarioActual = null;

        private SessionManager_GV42() { }

        public static SessionManager_GV42 Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new SessionManager_GV42();
                return _instancia;
            }
        }


        public bool IniciarSesion(Usuario_GV42 usuario)
        {
            if (_usuarioActual != null)
                return false;

            _usuarioActual = usuario;
            return true;
        }

        public void CerrarSesion()
        {
            _usuarioActual = null;
        }

        public bool HaySesionActiva()
        {
            return _usuarioActual != null;
        }


        public Usuario_GV42 ObtenerUsuarioActual()
        {
            return _usuarioActual;
        }
    }
}
