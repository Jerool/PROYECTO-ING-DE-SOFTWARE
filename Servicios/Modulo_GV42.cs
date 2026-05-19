using System;

namespace Servicios
{
    // Entidad Modulo_GV42
    // -------------------
    // Refleja la tabla "Modulo" de la base. Sirve para que la columna
    // EVENTOS.IdModulo apunte a un módulo válido (FK).
    // Ejemplos: "Login", "Gestión Usuario", "Contraseña", "Usuario".
    public class Modulo_GV42
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Modulo_GV42() { }
        public Modulo_GV42(int id, string nombre) { Id = id; Nombre = nombre; }

        public override string ToString() => Nombre ?? string.Empty;
    }
}
