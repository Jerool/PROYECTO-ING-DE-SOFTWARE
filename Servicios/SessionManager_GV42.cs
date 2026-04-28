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

        private Usuario _usuarioActual = null;

        public bool HaySesionActiva()
        {
            return _usuarioActual != null;
        }

        public void IniciarSesion(Usuario usuario)
        {
            _usuarioActual = usuario;
        }

        public void CerrarSesion()
        {
            _usuarioActual = null;
        }

        public Usuario ObtenerUsuarioActual()
        {
            return _usuarioActual;
        }
    }
}
