using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace BLL
{
    public class BLLUsuario_GV42
    {
        private readonly GestorUsuario_GV42 _gestorUsuario;

        public BLLUsuario_GV42()
        {
            _gestorUsuario = new GestorUsuario_GV42();
        }

        public bool IntentarLogin(string login, string contrasena)
        {
            // 1. ¿Ya hay una sesión activa?
            if (SessionManager_GV42.HaySesionActiva())
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Intento de login con sesión ya activa", "Media");
                return false;
            }

            // 2. ¿Existe el usuario?
            Usuario usuario = _gestorUsuario.BuscarPorLogin(login);
            if (usuario == null)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inexistente", "Alta");
                return false;
            }

            // 3. ¿Está bloqueado o inactivo?
            if (usuario.Bloqueo)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado", "Alta");
                return false;
            }
            if (!usuario.Activo)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inactivo", "Alta");
                return false;
            }

            // 4. ¿Contraseña correcta?
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasena);
            if (usuario.Contrasena != contrasenaCifrada)
            {
                usuario.Intentos++;

                if (usuario.Intentos >= 3)
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: true);
                    Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado por 3 intentos fallidos", "Alta");
                }
                else
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: false);
                    Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", $"Contraseña incorrecta. Intento {usuario.Intentos}/3", "Media");
                }
                return false;
            }

            // 5. Login exitoso
            _gestorUsuario.ResetearIntentos(login);
            SessionManager_GV42.IniciarSesion(usuario);
            Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Login exitoso", "Baja");
            return true;
        }
    }
}
