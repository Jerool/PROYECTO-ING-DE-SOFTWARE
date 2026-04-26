using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Usuario
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

        private string _Rol;

        public string Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
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
            set { _Activo  = value; }
        }

        private int _Intentos;
        public int Intentos
        {
            get { return _Intentos; }
            set { _Intentos = value; }
        }

        public Usuario(string dni, string apellidos, string nombre, string login, string password, string rol, string email, int intentos)
        {
            DNI = dni;
            Apellido = apellidos;
            Nombre = nombre;
            Login = login;
            Contrasena = password;
            Rol = rol;
            Email = email;
            Bloqueo = false;
            Activo = true;
            Intentos = intentos;

        }

        public Usuario()
        {
            
        }

    }

    public class Bitacora 
    {
        private string _Login;

        public string Login
        {
            get { return _Login; }
            set { _Login = value; }
        }

        private string _Modulo;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }

        private string _Evento;

        public string Evento
        {
            get { return _Evento; }
            set { _Evento = value; }
        }
        private string _Criticidad;

        public string Criticidad
        {
            get { return _Criticidad; }
            set { _Criticidad = value; }
        }

        private DateTime _FechaHora;

        public DateTime FechaHora
        {
            get { return _FechaHora; }
            set { _FechaHora = value; }
        }

        public Bitacora(string login, string modulo, string evento, string criticidad, DateTime fechaHora)
        {
            Login = login;
            Modulo = modulo;
            Evento = evento;
            Criticidad = criticidad;
            FechaHora = fechaHora;
        }

        public Bitacora()
        {
            
        }
    }
}
