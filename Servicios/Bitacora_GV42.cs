using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    // Bitacora_GV42
    // -------------
    // Representa una fila de la tabla EVENTOS.
    //
    // Cambios respecto a la versión vieja:
    //  - Modulo dejó de ser string suelto y pasó a ser Modulo_GV42 (entidad, FK).
    //  - El antiguo campo "Evento" se separó en dos:
    //        TipoEvento (catálogo, FK)         -> qué tipo de evento es
    //        Detalle    (texto libre, opcional)-> info dinámica del evento
    //    Ej.: TipoEvento = "Contraseña incorrecta", Detalle = "Intento 2/3".
    public class Bitacora_GV42
    {
        public string Login { get; set; }

        // FK a la tabla Modulo
        public Modulo_GV42 Modulo { get; set; }

        // FK a la tabla TipoEvento
        public TipoEvento_GV42 TipoEvento { get; set; }

        // Texto libre con el detalle dinámico (puede ser null/"")
        public string Detalle { get; set; }

        public string Criticidad { get; set; }
        public DateTime FechaHora { get; set; }

        // Helpers de solo lectura para mostrar fácil en grillas / PDF / labels.
        public string ModuloNombre => Modulo != null ? Modulo.Nombre : string.Empty;
        public string TipoEventoNombre => TipoEvento != null ? TipoEvento.Nombre : string.Empty;

        // Combinación legible "Tipo - Detalle". Si no hay detalle, devuelve solo el tipo.
        public string Evento
        {
            get
            {
                string tipo = TipoEventoNombre;
                if (string.IsNullOrWhiteSpace(Detalle)) return tipo;
                return tipo + " — " + Detalle;
            }
        }

        public Bitacora_GV42() { }

        // Constructor pensado para los lugares del código que ya conocen los
        // nombres como strings ("Login", "Gestión Usuario", etc.). Internamente
        // se guarda el string en una propiedad auxiliar, y el DAL/Manager se
        // encarga de resolver los nombres a Ids antes de persistir.
        public Bitacora_GV42(string login, string modulo, string tipoEvento, string detalle, string criticidad, DateTime fechaHora)
        {
            Login = login;
            // Guardamos el nombre como "shell" — quien persista esto va a tener
            // que resolver el Id contra la tabla Modulo.
            Modulo = string.IsNullOrEmpty(modulo) ? null : new Modulo_GV42 { Nombre = modulo };
            TipoEvento = string.IsNullOrEmpty(tipoEvento) ? null : new TipoEvento_GV42 { Nombre = tipoEvento };
            Detalle = detalle;
            Criticidad = criticidad;
            FechaHora = fechaHora;
        }
    }
}
