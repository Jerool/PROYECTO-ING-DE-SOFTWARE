using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{

    public class Encriptador_GV42
    {
        private static Encriptador_GV42 _instancia;

        private Encriptador_GV42() { }

        public static Encriptador_GV42 Instancia
        {
            get
            {
                if (_instancia == null)
                _instancia = new Encriptador_GV42();
                return _instancia;
            }
        }

        public string EncriptarContrasena(string contrasenaPlana)
        {
            if (string.IsNullOrEmpty(contrasenaPlana))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasenaPlana));

                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
