using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IntentosLogin_GV42
    {
        // Cantidad máxima de intentos fallidos antes de bloquear.
        public const int MAX_INTENTOS = 3;

        // Singleton: solo existe UNA instancia en toda la app.
        private static IntentosLogin_GV42 _instancia;

        // Diccionario login → cantidad de intentos fallidos.
        // Usamos OrdinalIgnoreCase para que "Pepe" y "pepe" cuenten como el mismo usuario.
        private readonly Dictionary<string, int> _intentosPorLogin;

        // Lock para que dos hilos no rompan el diccionario al mismo tiempo
        // (defensa: el form puede disparar varios login en paralelo).
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

        // Suma 1 al contador del login y devuelve el valor actualizado.
        // Si es la primera vez que falla, lo creamos en 1.
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

        // Devuelve el contador actual (0 si nunca falló).
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
