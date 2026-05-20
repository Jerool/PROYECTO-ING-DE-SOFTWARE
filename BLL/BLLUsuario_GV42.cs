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
        public const int MAX_INTENTOS = 3;
        private static readonly TimeSpan VENTANA_INTENTOS = TimeSpan.FromHours(1);

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


        private void Auditar(string login, string modulo, string tipoEvento, string detalle, string criticidad)
        {
           
            BLLBitacora_GV42.Instancia.RegistrarEvento(login, modulo, tipoEvento, detalle, criticidad);
           
        }

        public ResultadoLogin IntentarLogin(string login, string contrasena)
        {

            if (SessionManager_GV42.Instancia.HaySesionActiva())
            {
                Auditar(login, "Login", "Intento de login con sesión ya activa", null, "Media");
                return ResultadoLogin.SesionActiva;
            }

            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);
            if (usuario == null)
            {
                try
                {
                 Auditar(login, "Login", "Usuario inexistente", "el usuario no existe", "Alta");
                }
                catch 
                {
                    return ResultadoLogin.UsuarioInexistente;
                }
            }


            if (usuario.Bloqueo)
            {
                Auditar(login, "Login", "Usuario bloqueado", "Usuario bloqueado correctamente" , "Alta");
                return ResultadoLogin.UsuarioBloqueado;
            }

            if (!usuario.Activo)
            {
                Auditar(login, "Login", "Usuario inactivo", "el usuario esta inactivo", "Alta");
                return ResultadoLogin.UsuarioInactivo;
            }

            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasena);
            if (usuario.Contrasena != contrasenaCifrada)
            {

                int nuevosIntentos = CalcularNuevosIntentos(usuario);
                DateTime ahora = DateTime.Now;

                if (nuevosIntentos >= MAX_INTENTOS)
                {

                    _DALUsuario.ActualizarIntentosFallidos(login, nuevosIntentos, ahora);
                    _DALUsuario.Bloquear(login);
                    Auditar(login, "Login", "Usuario bloqueado por intentos fallidos",$"{MAX_INTENTOS} intentos fallidos consecutivos dentro de {VENTANA_INTENTOS.TotalMinutes:0} min", "Alta");
                    return ResultadoLogin.BloqueadoPorIntentos;
                }
                else
                {
                    _DALUsuario.ActualizarIntentosFallidos(login, nuevosIntentos, ahora);
                    Auditar(login, "Login", "Contraseña incorrecta", $"Intento {nuevosIntentos}/{MAX_INTENTOS}", "Media");
                    return ResultadoLogin.ContrasenaIncorrecta;
                }
            }


            _DALUsuario.ResetearIntentosFallidos(login);

            bool sesionIniciada = SessionManager_GV42.Instancia.IniciarSesion(usuario);
            if (!sesionIniciada)
            {
                Auditar(login, "Login", "Intento de login con sesión ya activa", "Intento de login con sesión ya activa", "Alta");
                return ResultadoLogin.SesionActiva;
            }
            Auditar(login, "Login", "Login exitoso", "Login correcto", "Baja");
            return ResultadoLogin.Exitoso;
        }

        private int CalcularNuevosIntentos(Usuario_GV42 usuario)
        {
            if (usuario.UltimoIntentoFallido == null)
                return 1;

            DateTime ultimo = usuario.UltimoIntentoFallido.Value;
            bool dentroDeLaVentana = (DateTime.Now - ultimo) <= VENTANA_INTENTOS;

            if (dentroDeLaVentana)
                return usuario.IntentosFallidos + 1;
            else
                return 1;
        }

        public List<Usuario_GV42> ListarActivos() => _DALUsuario.ListarActivos();
        public List<Usuario_GV42> ListarTodos() => _DALUsuario.ListarTodos();


        public List<Rol_GV42> ListarRoles() => _DALUsuario.ListarRoles();

        public Usuario_GV42 BuscarPorLogin(string login) => _DALUsuario.BuscarPorLogin(login);

        public bool ExisteDNI(string dni) => _DALUsuario.ExisteDNI(dni);

        public void Desbloquear(string dni, string login)
        {
            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);

            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = usuario.Nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);
            _DALUsuario.Desbloquear(dni, contrasenaCifrada);
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Usuario desbloqueado", $"Usuario {login} desbloqueado y contraseña reseteada", "Media");
        }

        public void ActivarDesactivar(string dni, bool activo)
        {
            _DALUsuario.ActivarDesactivar(dni, activo);
            string accion = activo ? "Usuario activado" : "Usuario desactivado";
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", accion, $"DNI: {dni}", "Media");
        }

        public void ModificarEmail(string dni, string email)
        {
            _DALUsuario.ModificarEmail(dni, email);
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario", "Email modificado", $"DNI: {dni}", "Media");
        }


        public void ModificarRol(string dni, Rol_GV42 rol)
        {
            if (rol == null) throw new Exception("Debe seleccionar un rol válido.");
            _DALUsuario.ModificarRol(dni, rol.Id);
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario","Rol modificado", $"DNI {dni} -> rol {rol.Nombre}", "Media");
        }
        public void CrearUsuario(string dni, string apellido, string nombre, string email, Rol_GV42 rol)
        {
            if (rol == null)
            throw new Exception("Debe seleccionar un rol.");

            if (_DALUsuario.ExisteDNI(dni))
            throw new Exception($"Ya existe un usuario con el DNI '{dni}'.");
            string ultimos3 = dni.Length >= 3 ? dni.Substring(dni.Length - 3) : dni;
            string contrasenaPlana = nombre.ToLower() + ultimos3;
            string contrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaPlana);
            string login = nombre.ToLower() + ultimos3;

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
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario","Usuario creado", $"Login: {login}", "Baja");
        }

        public enum ResultadoCambioContrasena
        {
            Exitoso,
            ContrasenaActualIncorrecta,
            ContrasenasNoCoinciden,
            UsuarioInexistente,
            NuevaIgualActual
        }

        public ResultadoCambioContrasena CambiarContrasena(string login, string contrasenaActual, string nuevaContrasena, string confirmarContrasena)
        {
            if (nuevaContrasena != confirmarContrasena)
            return ResultadoCambioContrasena.ContrasenasNoCoinciden;
            if (nuevaContrasena == contrasenaActual)
            return ResultadoCambioContrasena.NuevaIgualActual;

            Usuario_GV42 usuario = _DALUsuario.BuscarPorLogin(login);
            if (usuario == null)
            return ResultadoCambioContrasena.UsuarioInexistente;

            string contrasenaActualCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(contrasenaActual);
            if (usuario.Contrasena != contrasenaActualCifrada)
            return ResultadoCambioContrasena.ContrasenaActualIncorrecta;

            string nuevaContrasenaCifrada = Encriptador_GV42.Instancia.EncriptarContrasena(nuevaContrasena);
            if (usuario.Contrasena == nuevaContrasenaCifrada)
            return ResultadoCambioContrasena.NuevaIgualActual;

            _DALUsuario.CambiarContrasena(login, nuevaContrasenaCifrada);

            Auditar(login, "Contraseña", "Contraseña cambiada exitosamente", "Combio contrasenia", "Baja");
            return ResultadoCambioContrasena.Exitoso;
        }

        public static void CerrarSesión()
        {
            BLLUsuario_GV42 bll = new BLLUsuario_GV42();
            bll.Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login,"Usuario", "Logout realizado", "LogOut", "Alta");
            SessionManager_GV42.Instancia.CerrarSesion();
        }
    }
}
