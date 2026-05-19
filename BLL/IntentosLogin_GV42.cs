using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IntentosLogin_GV42
    {
        public const int MAX_INTENTOS = 3;


        private static IntentosLogin_GV42 _instancia;


        private readonly Dictionary<string, int> _intentosPorLogin;


        private readonly object _candado = new object();

        private IntentosLogin_GV42()
        {
            _intentosPorLogin = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);  
        }

        public static IntentosLogin_GV42 Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new IntentosLogin_GV42();
                return _instancia;
            }
        }

        public int RegistrarIntentoFallido(string login)
        {
            lock (_candado)
            {
                if (!_intentosPorLogin.ContainsKey(login))
                    _intentosPorLogin[login] = 0;

                _intentosPorLogin[login]++;
                return _intentosPorLogin[login];
            }
        }

        public int Obtener(string login)
        {
            lock (_candado)
            {
                return _intentosPorLogin.TryGetValue(login, out int v) ? v : 0;
            }
        }

        // Borra el contador del login.
        public void Resetear(string login)
        {
            lock (_candado)
            {
                if (_intentosPorLogin.ContainsKey(login))
                    _intentosPorLogin.Remove(login);
            }
        }
    }
}
