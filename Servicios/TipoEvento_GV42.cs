using System;

namespace Servicios
{
    // Entidad TipoEvento_GV42
    // -----------------------
    // Refleja la tabla "TipoEvento" de la base. Sirve para que la columna
    // EVENTOS.IdTipoEvento apunte a un tipo válido (FK).
    //
    // Importante: el "tipo" es genérico ("Login exitoso", "Contraseña incorrecta",
    // "Usuario creado", etc.). La parte específica con detalle dinámico
    // ("Intento 2/3", "Usuario creado: jeremias747") va en la columna Detalle
    // de EVENTOS, no acá.
    public class TipoEvento_GV42
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public TipoEvento_GV42() { }
        public TipoEvento_GV42(int id, string nombre) { Id = id; Nombre = nombre; }

        public override string ToString() => Nombre ?? string.Empty;
    }
}
