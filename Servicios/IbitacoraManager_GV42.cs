using System;

namespace Servicios
{
    // Contrato del manager de bitácora.
    // Mantiene la posibilidad de inyectar/mockear el comportamiento de
    // RegistrarEvento (validaciones + fecha por defecto) desde tests u otras capas.
    public interface IbitacoraManager_GV42
    {
        Bitacora_GV42 RegistrarEvento(Bitacora_GV42 evento);
    }
}
