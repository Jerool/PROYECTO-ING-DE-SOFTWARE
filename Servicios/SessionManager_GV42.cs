using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    // SessionManager: maneja la sesión activa en la app (un único usuario logueado a la vez).
    // Es un Singleton: solo existe UNA instancia en toda la ejecución del programa,
    // y se accede por SessionManager_GV42.Instancia.
    //
    // ¿Por qué Singleton? Porque la "sesión actual" es un estado global de la app,
    // y queremos que CUALQUIER lugar del código pueda preguntarle quién está logueado
    // sin tener que pasarse referencias por todos lados.
    public class SessionManager_GV42
    {
        // nstancia única (privada y estática) que se inicializa cuando se accede por primera vez.
        private static SessionManager_GV42 _instancia;

        // Campo que guarda al usuario actualmente logueado. Si es null, no hay sesión.
        private Usuario_GV42 _usuarioActual = null;

        private SessionManager_GV42() { }

        //Punto de acceso global a la única instancia.
        public static SessionManager_GV42 Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new SessionManager_GV42();
                return _instancia;
            }
        }

        // Inicia sesión con un usuario. Devuelve false si ya había alguien logueado
        // (no permitimos sesiones simultáneas en la misma instancia).
        public bool IniciarSesion(Usuario_GV42 usuario)
        {
            if (_usuarioActual != null)
                return false;

            _usuarioActual = usuario;
            return true;
        }

        //Cierra la sesión: deja a _usuarioActual en null.
        public void CerrarSesion()
        {
            _usuarioActual = null;
        }

        // La BLL lo usa para validar
        //    al intentar un nuevo login.
        public bool HaySesionActiva()
        {
            return _usuarioActual != null;
        }

        // Devuelve el usuario actual (o null si no hay sesión).
        //    Sirve, por ejemplo, para que la bitácora sepa quién hizo cada acción.
        public Usuario_GV42 ObtenerUsuarioActual()
        {
            return _usuarioActual;
        }
    }
}
