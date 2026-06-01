using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Usuario_GV42
    {

        private string _DNI;
        public string DNI
        {
            get { return _DNI; }
            set { _DNI = value; }
        }

        private string _Apellido;
        public string Apellido
        {
            get { return _Apellido; }
            set { _Apellido = value; }
        }

        private string _Nombre;
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set { _Login = value; }
        }

        private string _Contrasena;
        public string Contrasena
        {
            get { return _Contrasena; }
            set { _Contrasena = value; }
        }

        private Rol_GV42 _Rol;
        public Rol_GV42 Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
        }

        public string RolNombre
        {
            get { return _Rol != null ? _Rol.Nombre : string.Empty; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }


        private bool _Bloqueo;
        public bool Bloqueo
        {
            get { return _Bloqueo; }
            set { _Bloqueo = value; }
        }

        private bool _Activo;
        public bool Activo
        {
            get { return _Activo; }
            set { _Activo = value; }
        }

        public int IntentosFallidos { get; set; }
        public DateTime? UltimoIntentoFallido { get; set; }

        public bool DebeCambiarContrasena { get; set; }

        // Idioma preferido del usuario ("es" / "en"). Se carga al loguearse
        // para que la app arranque en el idioma que el usuario usó la última vez.
        public string Idioma { get; set; } = "es";

        public Usuario_GV42(string dni, string apellidos, string nombre, string login, string password, Rol_GV42 rol, string email)
        {
            DNI = dni;
            Apellido = apellidos;
            Nombre = nombre;
            Login = login;
            Contrasena = password;
            Rol = rol;
            Email = email;
            Bloqueo = false;
        }

        public Usuario_GV42()
        {

        }
    }
}
