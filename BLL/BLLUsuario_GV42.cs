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
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Intento de login con sesión ya activa", "Media");
                return ResultadoLogin.SesionActiva;
            }

            Usuario usuario = _gestorUsuario.BuscarPorLogin(login);
            if (usuario == null)
            {
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inexistente", "Alta");
                return ResultadoLogin.UsuarioInexistente;
            }

            if (usuario.Bloqueo)
            {
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado", "Alta");
                return ResultadoLogin.UsuarioBloqueado;
            }

            if (!usuario.Activo)
            {
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario inactivo", "Alta");
                return ResultadoLogin.UsuarioInactivo;
            }

            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasena);
            if (usuario.Contrasena != contrasenaCifrada)
            {
                usuario.Intentos++;
                if (usuario.Intentos >= 3)
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: true);
                    Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado por 3 intentos fallidos", "Alta");
                    return ResultadoLogin.BloqueadoPorIntentos;
                }
                else
                {
                    _gestorUsuario.ActualizarIntentos(login, usuario.Intentos, bloqueo: false);
                    Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", $"Contraseña incorrecta. Intento {usuario.Intentos}/3", "Media");
                    return ResultadoLogin.ContrasenaIncorrecta;
                }
            }

            _gestorUsuario.ResetearIntentos(login);

            bool sesionIniciada = SessionManager_GV42.Instancia.IniciarSesion(usuario);
            if (!sesionIniciada)
            {
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Intento de login con sesión ya activa", "Alta");
                return ResultadoLogin.SesionActiva;
            }
            Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Login exitoso", "Baja");
            return ResultadoLogin.Exitoso;
        }

        public List<Usuario> ListarActivos() => _gestorUsuario.ListarActivos();

        public List<Usuario> ListarTodos() => _gestorUsuario.ListarTodos();

        public void Desbloquear(string dni, string login)
        {
            // Usar el login para buscar el usuario (ya está implementado)
            Usuario usuario = _gestorUsuario.BuscarPorLogin(login);

            // Regenerar la contraseña: nombre + últimos 3 del DNI
            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = usuario.Nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);

            // Desbloquear y resetear contraseña
            _gestorUsuario.Desbloquear(dni, contrasenaCifrada);

            Auditoria_GV42.Instancia.RegistrarEvento(
                SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login,
                "Gestión Usuario", $"Usuario {login} desbloqueado y contraseña reseteada", "Media");
        }

        public void ActivarDesactivar(string dni, bool activo)
        {
            _gestorUsuario.ActivarDesactivar(dni, activo);
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Usuario Activo", "Media");
        }
        public void ModificarEmail(string dni, string email)
        {
            _gestorUsuario.ModificarEmail(dni, email);
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Email de usuario modificado", "Media");
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
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, " Gestion Usuario", $"Usuario creado: {login}", "Baja");
        }

        public enum ResultadoCambioContrasena
        {
            Exitoso,
            ContrasenaActualIncorrecta,
            ContrasenasNoCoinciden,
            UsuarioInexistente
        }

        public ResultadoCambioContrasena CambiarContrasena(string login, string contrasenaActual, string nuevaContrasena, string confirmarContrasena)
        {
            // 1. Verificar que las nuevas contraseñas coincidan
            if (nuevaContrasena != confirmarContrasena)
                return ResultadoCambioContrasena.ContrasenasNoCoinciden;

            // 2. Buscar el usuario
            Usuario usuario = _gestorUsuario.BuscarPorLogin(login);
            if (usuario == null)
                return ResultadoCambioContrasena.UsuarioInexistente;

            // 3. Verificar que la contraseña actual sea correcta
            string contrasenaActualCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaActual);
            if (usuario.Contrasena != contrasenaActualCifrada)
                return ResultadoCambioContrasena.ContrasenaActualIncorrecta;

            // 4. Cifrar y guardar la nueva contraseña
            string nuevaContrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(nuevaContrasena);
            _gestorUsuario.CambiarContrasena(login, nuevaContrasenaCifrada);

            Auditoria_GV42.Instancia.RegistrarEvento(login, "Contraseña", "Contraseña cambiada exitosamente", "Baja");
            return ResultadoCambioContrasena.Exitoso;
        }

        public static void CerrarSesión()
        {
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Usuario", "Logout realizado", "Alta");
            SessionManager_GV42.Instancia.CerrarSesion();
        }
    }
}
