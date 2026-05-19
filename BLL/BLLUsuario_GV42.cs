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
        private readonly DALRol_GV42 _DALRol;
        private readonly IbitacoraManager_GV42 _IbitacoraManager;
        private readonly DALBitacora_GV42 _DALBitacora;

        public BLLUsuario_GV42()
        {
            _DALUsuario = new DALUsuario_GV42();
            _DALRol = new DALRol_GV42();
            _DALBitacora = new DALBitacora_GV42();
            _IbitacoraManager = new BitacoraManager_GV42();
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

        // Helper interno: arma una Bitacora y la guarda. Se encarga de separar
        // el "tipo de evento" (catálogo, FK) del "detalle" (texto libre).
        // Así no repetimos new Bitacora_GV42(...) + _DALBitacora.Guardar(...) en cada llamada.
        private void Auditar(string login, string modulo, string tipoEvento, string detalle, string criticidad)
        {
            Bitacora_GV42 b = _IbitacoraManager.RegistrarEvento(
                new Bitacora_GV42(login, modulo, tipoEvento, detalle, criticidad, DateTime.Now));
            _DALBitacora.Guardar(b);
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
                Auditar(login, "Login", "Usuario inexistente", null, "Alta");
                return ResultadoLogin.UsuarioInexistente;
            }


            if (usuario.Bloqueo)
            {
                Auditar(login, "Login", "Usuario bloqueado", null, "Alta");
                return ResultadoLogin.UsuarioBloqueado;
            }

            if (!usuario.Activo)
            {
                Auditar(login, "Login", "Usuario inactivo", null, "Alta");
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
                    Auditar(login, "Login", "Usuario bloqueado por intentos fallidos",
                        $"{IntentosLogin_GV42.MAX_INTENTOS} intentos fallidos consecutivos", "Alta");
                    return ResultadoLogin.BloqueadoPorIntentos;
                }
                else
                {
                    Auditar(login, "Login", "Contraseña incorrecta",
                        $"Intento {intentos}/{IntentosLogin_GV42.MAX_INTENTOS}", "Media");
                    return ResultadoLogin.ContrasenaIncorrecta;
                }
            }

            IntentosLogin_GV42.Instancia.Resetear(login);
            bool sesionIniciada = SessionManager_GV42.Instancia.IniciarSesion(usuario);
            if (!sesionIniciada)
            {
                Auditar(login, "Login", "Intento de login con sesión ya activa", null, "Alta");
                return ResultadoLogin.SesionActiva;
            }
            Auditar(login, "Login", "Login exitoso", null, "Baja");
            return ResultadoLogin.Exitoso;
        }

        public List<Usuario_GV42> ListarActivos() => _DALUsuario.ListarActivos();
        public List<Usuario_GV42> ListarTodos() => _DALUsuario.ListarTodos();

        // Devuelve la lista completa de roles como entidades (Id + Nombre).
        // La UI la usa para llenar el combo y luego pasar la entidad seleccionada.
        public List<Rol_GV42> ListarRoles() => _DALRol.ListarTodos();

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
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario",
                "Usuario desbloqueado", $"Usuario {login} desbloqueado y contraseña reseteada", "Media");
        }

        public void ActivarDesactivar(string dni, bool activo)
        {
            _DALUsuario.ActivarDesactivar(dni, activo);
            string accion = activo ? "Usuario activado" : "Usuario desactivado";
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario",
                accion, $"DNI: {dni}", "Media");
        }

        public void ModificarEmail(string dni, string email)
        {
            _DALUsuario.ModificarEmail(dni, email);
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario",
                "Email modificado", $"DNI: {dni}", "Media");
        }

        // Recibe el Rol_GV42 ya resuelto (con Id + Nombre) — la UI lo manda
        // directo desde el ComboBox.
        public void ModificarRol(string dni, Rol_GV42 rol)
        {
            if (rol == null) throw new Exception("Debe seleccionar un rol válido.");
            _DALUsuario.ModificarRol(dni, rol.Id);
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario",
                "Rol modificado", $"DNI {dni} -> rol {rol.Nombre}", "Media");
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
            Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login, "Gestión Usuario",
                "Usuario creado", $"Login: {login}", "Baja");
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

            Auditar(login, "Contraseña", "Contraseña cambiada exitosamente", null, "Baja");
            return ResultadoCambioContrasena.Exitoso;
        }

        public static void CerrarSesión()
        {
            BLLUsuario_GV42 bll = new BLLUsuario_GV42();
            bll.Auditar(SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login,
                "Usuario", "Logout realizado", null, "Alta");

            SessionManager_GV42.Instancia.CerrarSesion();
        }
    }
}
