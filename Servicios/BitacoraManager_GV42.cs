using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{

    public class BitacoraManager_GV42 : IbitacoraManager_GV42
    {
        public Bitacora_GV42 RegistrarEvento(Bitacora_GV42 evento)
        {
            if (evento.FechaHora == default(DateTime))
                evento.FechaHora = DateTime.Now;
            if (evento.Criticidad != "Alta" && evento.Criticidad != "Media" && evento.Criticidad != "Baja")
                throw new Exception("Criticidad inválida. Debe ser 'Alta', 'Media' o 'Baja'.");
            if (string.IsNullOrWhiteSpace(evento.Login))
                throw new Exception("Login inválido. No puede estar vacío.");
            // Validamos por ModuloNombre (helper) en vez de por Modulo (entidad)
            // porque en muchos llamadores el módulo se arma desde un string.
            if (string.IsNullOrWhiteSpace(evento.ModuloNombre))
                throw new Exception("Módulo inválido. No puede estar vacío.");
            // Mismo criterio con TipoEvento: validamos que tenga al menos el nombre.
            if (string.IsNullOrWhiteSpace(evento.TipoEventoNombre))
                throw new Exception("Tipo de evento inválido. No puede estar vacío.");
            return evento;
        }
    }
}
