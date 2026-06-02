using System.Collections.Generic;
using System.Linq;

namespace Servicios
{

    public class Rol_GV42 : IComponentePermiso_GV42
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Nombre;
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        public List<IComponentePermiso_GV42> Hijos { get; set; } = new List<IComponentePermiso_GV42>();

        public Rol_GV42() { }

        public Rol_GV42(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public IEnumerable<Patente_GV42> ObtenerPatentes()
        {
            var vistas = new HashSet<int>();
            foreach (var hijo in Hijos)
            {
                foreach (var p in hijo.ObtenerPatentes())
                {
                    if (vistas.Add(p.Id))
                        yield return p;
                }
            }
        }

        public const string ROL_SUPER_ADMIN = "Admin";

        public bool TienePermiso(string dataKey)
        {
            if (string.IsNullOrEmpty(dataKey)) return false;
            if (string.Equals(Nombre, ROL_SUPER_ADMIN, System.StringComparison.OrdinalIgnoreCase))
                return true;
            return ObtenerPatentes().Any(p => p.DataKey == dataKey);
        }
        public override string ToString()
        {
            return Nombre ?? string.Empty;
        }
    }
}
