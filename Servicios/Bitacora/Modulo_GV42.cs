using System;

namespace Servicios
{

    public class Modulo_GV42
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Modulo_GV42() { }
        public Modulo_GV42(int id, string nombre) { Id = id; Nombre = nombre; }

        public override string ToString() => Nombre ?? string.Empty;
    }
}
