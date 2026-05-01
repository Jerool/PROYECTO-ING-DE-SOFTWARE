using DAL;                              
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
        private readonly DALUsuario_GV42 _DALUsuario;

        public BLLUsuario_GV42()
        {
            _DALUsuario = new DALUsuario_GV42();
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

   
            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);
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
                int intentos = IntentosLogin_GV42.Instancia.RegistrarIntentoFallido(login);
                if (intentos >= IntentosLogin_GV42.MAX_INTENTOS)
                {
                    _DALUsuario.Bloquear(login);
                    IntentosLogin_GV42.Instancia.Resetear(login);
                    Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Usuario bloqueado por 3 intentos fallidos", "Alta");
                    return ResultadoLogin.BloqueadoPorIntentos;
                }
                else
                {
                    Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", $"Contraseña incorrecta. Intento {intentos}/{IntentosLogin_GV42.MAX_INTENTOS}", "Media");
                    return ResultadoLogin.ContrasenaIncorrecta;
                }
            }

            IntentosLogin_GV42.Instancia.Resetear(login);
            bool sesionIniciada = SessionManager_GV42.Instancia.IniciarSesion(usuario);
            if (!sesionIniciada)
            {
                Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Intento de login con sesión ya activa", "Alta");
                return ResultadoLogin.SesionActiva;
            }
            Auditoria_GV42.Instancia.RegistrarEvento(login, "Login", "Login exitoso", "Baja");
            return ResultadoLogin.Exitoso;
        }


        public List<Usuario_GV42> ListarActivos() => _DALUsuario.ListarActivos();
        public List<Usuario_GV42> ListarTodos() => _DALUsuario.ListarTodos();
        public List<string> ListarRoles() => _DALUsuario.Listar();
        public Usuario_GV42 BuscarPorLogin(string login) => _DALUsuario.BuscarPorLogin(login);

        public bool ExisteDNI(string dni) => _DALUsuario.ExisteDNI(dni);

        public void Desbloquear(string dni, string login)
        {
            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);

            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = usuario.Nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);
            _DALUsuario.Desbloquear(dni, contrasenaCifrada);
            IntentosLogin_GV42.Instancia.Resetear(login);
            Auditoria_GV42.Instancia.RegistrarEvento(
            SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login,"Gestión Usuario", $"Usuario {login} desbloqueado y contraseña reseteada", "Media");
        }

        public void ActivarDesactivar(string dni, bool activo)
        {
            _DALUsuario.ActivarDesactivar(dni, activo);
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Usuario Activo", "Media");
        }

        public void ModificarEmail(string dni, string email)
        {
            _DALUsuario.ModificarEmail(dni, email);
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Email de usuario modificado", "Media");
        }

        public void ModificarRol(string dni, string rol)
        {
            _DALUsuario.ModificarRol(dni, rol);
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", $"Rol modificado a {rol}", "Media");
        }
        public void CrearUsuario(string dni, string apellido, string nombre, string email, string rol)
        {
            if (_DALUsuario.ExisteDNI(dni))
            throw new Exception($"Ya existe un usuario con el DNI '{dni}'.");
            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);
            string login = nombre.ToLower() + ultimos3;

            // Validación previa: usuario ya existe con ese login.
            if (_DALUsuario.BuscarPorLogin(login) != null)
            throw new Exception($"Ya existe un usuario con el login '{login}'.");

            Usuario_GV42 u = new Usuario_GV42
            {
                DNI = dni,
                Apellido = apellido,
                Nombre = nombre,
                Login = login,
                Contrasena = contrasenaCifrada,
                Rol = rol,
                Email = email
            };
    
            int filas = _DALUsuario.AgregarUsuario(u);
            if (filas == 0)
                throw new Exception("El INSERT no afectó ninguna fila. Verificá la base de datos.");

            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", $"Usuario creado: {login}", "Baja");
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
            if (nuevaContrasena != confirmarContrasena)
                return ResultadoCambioContrasena.ContrasenasNoCoinciden;

            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);
            if (usuario == null)
                return ResultadoCambioContrasena.UsuarioInexistente;

            string contrasenaActualCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaActual);
            if (usuario.Contrasena != contrasenaActualCifrada)
                return ResultadoCambioContrasena.ContrasenaActualIncorrecta;

            string nuevaContrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(nuevaContrasena);
            _DALUsuario.CambiarContrasena(login, nuevaContrasenaCifrada);

            Auditoria_GV42.Instancia.RegistrarEvento(login, "Contraseña", "Contraseña cambiada exitosamente", "Baja");
            return ResultadoCambioContrasena.Exitoso;
        }

        // Es estático porque no necesita instancia: solo registra el evento
        // y limpia el SessionManager.
        public static void CerrarSesión()
        {
            Auditoria_GV42.Instancia.RegistrarEvento(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Usuario", "Logout realizado", "Alta");
            SessionManager_GV42.Instancia.CerrarSesion();
        }
    }
}
