using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{

    public class Bitacora_GV42
    {

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set { _Login = value; }
        }


        private string _Modulo;
        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }


        private string _Evento;
        public string Evento
        {
            get { return _Evento; }
            set { _Evento = value; }
        }

        private string _Criticidad;
        public string Criticidad
        {
            get { return _Criticidad; }
            set { _Criticidad = value; }
        }


        private DateTime _FechaHora;
        public DateTime FechaHora
        {
            get { return _FechaHora; }
            set { _FechaHora = value; }
        }


        public Bitacora_GV42(string login, string modulo, string evento, string criticidad, DateTime fechaHora)
        {
            Login = login;
            Modulo = modulo;
            Evento = evento;
            Criticidad = criticidad;
            FechaHora = fechaHora;
        }

        public Bitacora_GV42()
        {

        }
    }
}
