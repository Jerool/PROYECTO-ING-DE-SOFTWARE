using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Auditoria_GV42
    {
        private static Auditoria_GV42 _Instancia;
        private readonly GestorBitacora_GV42 _GestorBitacora;
        private Auditoria_GV42()
        {
            _GestorBitacora = new GestorBitacora_GV42();
        }

        public static Auditoria_GV42 Instancia
        {
            get
            {
                if (_Instancia == null)
                    _Instancia = new Auditoria_GV42();
                return _Instancia;
            }
        }
        public void RegistrarEvento(string login, string modulo, string evento, string criticidad)
        {
            var registro = new Bitacora
            {
                Login = login,
                Modulo = modulo,
                Evento = evento,
                Criticidad = criticidad,
                FechaHora = DateTime.Now
            };
            _GestorBitacora.Guardar(registro);
        }

        public List<Bitacora> Listar()
        {
            return _GestorBitacora.Listar();
        }
    }
}
