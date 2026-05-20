using System;

namespace Servicios
{

    public class Rol_GV42
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

        public Rol_GV42() { }

        public Rol_GV42(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre ?? string.Empty;
        }

    }
}
