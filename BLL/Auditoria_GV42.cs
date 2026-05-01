using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Auditoria_GV42
    {
        private static Auditoria_GV42 _Instancia;
        private readonly DALBitacora_GV42 _DALBitacora;
        private Auditoria_GV42()
        {
            _DALBitacora = new DALBitacora_GV42();
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
            var registro = new Bitacora_GV42
            {
                Login = login,
                Modulo = modulo,
                Evento = evento,
                Criticidad = criticidad,
                FechaHora = DateTime.Now
            };
            _DALBitacora.Guardar(registro);
        }
        public List<Bitacora_GV42> Listar()
        {
            return _DALBitacora.Listar();
        }
    }
}
