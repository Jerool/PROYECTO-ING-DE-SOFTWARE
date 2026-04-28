using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLUsuario_GV42
    {
        private readonly GestorUsuario_GV42 _gestorUsuario;

        public BLLUsuario_GV42()
        {
            _gestorUsuario = new GestorUsuario_GV42();
        }

        public enum ResultadoLogin
        {
            Exitoso,
            SesionActiva,
            UsuarioInexistente,
            UsuarioBloqueado,
            UsuarioInactivo,
            ContrasenaIncorrecta,
            BloqueadoPorIntentos
        }

        public ResultadoLogin IntentarLogin(string login, string contrasena)
        {
           
            if (SessionManager_GV42.Instancia.HaySesionActiva())
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Intento de login con sesión ya activa", "Media");
                return ResultadoLogin.SesionActiva;
            }

            Usuario usuario = _gestorUsuario.BuscarPorLogin(login);
            if (usuario == null)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inexistente", "Alta");
                return ResultadoLogin.UsuarioInexistente;
            }

            if (usuario.Bloqueo)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado", "Alta");
                return ResultadoLogin.UsuarioBloqueado;
            }

            if (!usuario.Activo)
            {
                Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inactivo", "Alta");
                return ResultadoLogin.UsuarioInactivo;
            }

            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasena);
            if (usuario.Contrasena != contrasenaCifrada)
            {
                usuario.Intentos++;
                if (usuario.Intentos >= 3)
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: true);
                    Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado por 3 intentos fallidos", "Alta");
                    return ResultadoLogin.BloqueadoPorIntentos;
                }
                else
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: false);
                    Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", $"Contraseña incorrecta. Intento {usuario.Intentos}/3", "Media");
                    return ResultadoLogin.ContrasenaIncorrecta;
                }
            }

            _gestorUsuario.ResetearIntentos(login);
            SessionManager_GV42.Instancia.IniciarSesion(usuario);
            Bitacora_GV42.Instancia.RegistrarEvento(login, "Login", "Login exitoso", "Baja");
            return ResultadoLogin.Exitoso;
        }

        public List<Usuario> ListarActivos() => _gestorUsuario.ListarActivos();

        public List<Usuario> ListarTodos() => _gestorUsuario.ListarTodos();

        public void Desbloquear(string dni)
        {
            _gestorUsuario.Desbloquear(dni);
            Bitacora_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Usuario Desbloqueado", "Media");
        }
        public void ActivarDesactivar(string dni, bool activo)
        {
            _gestorUsuario.ActivarDesactivar(dni, activo);
            Bitacora_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Usuario Activo", "Media");
        }
        public void ModificarEmail(string dni, string email)
        {
            _gestorUsuario.ModificarEmail(dni, email);
            Bitacora_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Email de usuario modificado", "Media");
        }

        public void CrearUsuario(string dni, string apellido, string nombre, string email, string rol)
        {

            // Contraseña automática: nombre + últimos 3 dígitos del DNI
            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);

            // Login automático: primeras  nombre + ultimos3 
            string login = nombre.ToLower() + ultimos3;

            Usuario u = new Usuario
            {
                DNI = dni,
                Apellido = apellido,
                Nombre = nombre,
                Login = login,
                Contrasena = contrasenaCifrada,
                Rol = rol,
                Email = email
            };

            _gestorUsuario.AgregarUsuario(u);
            Bitacora_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, " Gestion Usuario", $"Usuario creado: {login}", "Baja");
        }

        
    }
}
