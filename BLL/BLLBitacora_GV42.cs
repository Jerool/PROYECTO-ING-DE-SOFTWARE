using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class BLLBitacora_GV42
    {
        private static BLLBitacora_GV42 _Instancia;
        private readonly DALBitacora_GV42 _DALBitacora;

        private BLLBitacora_GV42()
        {
            _DALBitacora = new DALBitacora_GV42();
        }

        public static BLLBitacora_GV42 Instancia
        {
            get
            {
                if (_Instancia == null)
                    _Instancia = new BLLBitacora_GV42();
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
        public List<Bitacora_GV42> Filtrar(string login, string modulo, string evento,
                                          string criticidad, DateTime fechaInicio, DateTime fechaFin)
        {
            return _DALBitacora.Filtrar(login, modulo, evento, criticidad, fechaInicio, fechaFin);
        }

        public List<string> ListarModulos() => _DALBitacora.ListarModulos();
        public List<string> ListarTiposEvento() => _DALBitacora.ListarTiposEvento();
        public List<string> ListarCriticidades()
        {
            return new List<string> { "Alta", "Media", "Baja" };
        }
    }
}
