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

        // Rol ahora es una entidad completa (Id + Nombre), no un string suelto.
        // La tabla Usuario referencia a Roles por IdRol (FK), por eso acá guardamos
        // el objeto Rol_GV42 entero.
        private Rol_GV42 _Rol;
        public Rol_GV42 Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
        }

        // Helper de solo lectura: devuelve el nombre del rol (o cadena vacía si no hay).
        // Útil para mostrar en la grilla sin tener que pegarle .Rol.Nombre en cada lado
        // y por si alguna parte vieja del código todavía espera un string.
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
