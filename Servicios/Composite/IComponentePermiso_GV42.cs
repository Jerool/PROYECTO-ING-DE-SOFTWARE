using System.Collections.Generic;

namespace Servicios
{

    public interface IComponentePermiso_GV42
    {
        int Id { get; }
        string Nombre { get; }

    
        IEnumerable<Patente_GV42> ObtenerPatentes();
    }
}
