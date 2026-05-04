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
            if (string.IsNullOrWhiteSpace(evento.Modulo)) 
                throw new Exception("Módulo inválido. No puede estar vacío.");
            if (string.IsNullOrWhiteSpace(evento.Evento))
                throw new Exception("Evento inválido. No puede estar vacío."); 
            return evento;
        }
    }
}
