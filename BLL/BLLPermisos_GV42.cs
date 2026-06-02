using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Lógica de negocio del módulo de permisos (Composite).
    // -----------------------------------------------------
    // Se encarga de:
    //   - Crear Familias validando que no haya otra con la MISMA composición.
    //   - Crear Roles asociando patentes y/o familias.
    //   - Eliminar Roles validando que no estén asignados a ningún Usuario.
    //   - Listar elementos para llenar combos / grillas en la UI.
    //
    // "Composición duplicada" significa: misma lista de patentes directas Y
    // misma lista de subfamilias directas. Si una familia A contiene exactamente
    // las mismas patentes y subfamilias que una familia B existente, A ya es
    // un duplicado de B y el sistema lo rechaza.
    public class BLLPermisos_GV42
    {
        private readonly DALPatente_GV42 _dalPatente;
        private readonly DALFamilia_GV42 _dalFamilia;
        private readonly DALRol_GV42 _dalRol;

        public BLLPermisos_GV42()
        {
            _dalPatente = new DALPatente_GV42();
            _dalFamilia = new DALFamilia_GV42();
            _dalRol = new DALRol_GV42();
        }

        // ─── Lecturas ──────────────────────────────────────────────────────

        public List<Patente_GV42> ListarPatentes() => _dalPatente.ListarTodas();
        public List<Familia_GV42> ListarFamilias() => _dalFamilia.ListarTodasPlanas();
        public List<Rol_GV42> ListarRoles()      => _dalRol.ListarTodos();

        public Familia_GV42 ObtenerArbolFamilia(int idFamilia) => _dalFamilia.ObtenerArbol(idFamilia);
        public Rol_GV42     ObtenerArbolRol(int idRol)         => _dalRol.ObtenerArbol(idRol);

        // ─── Creación de Familias ──────────────────────────────────────────

        // Crea una familia con sus patentes y subfamilias. Antes de crearla,
        // valida que no exista otra familia con la MISMA composición directa.
        //
        // Si la composición coincide con una familia ya existente, lanza una
        // excepción que la UI debe atrapar y mostrar al usuario.
        public int CrearFamilia(string nombre, List<int> idsPatentes, List<int> idsSubfamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre de la familia es obligatorio.");

            // Validación: nombre único (la tabla tiene UNIQUE, pero damos un mensaje claro).
            if (_dalFamilia.ListarTodasPlanas().Any(f => f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe una familia con el nombre '{nombre}'. Elegí otro nombre.");

            idsPatentes    = (idsPatentes ?? new List<int>()).Distinct().OrderBy(i => i).ToList();
            idsSubfamilias = (idsSubfamilias ?? new List<int>()).Distinct().OrderBy(i => i).ToList();

            if (idsPatentes.Count == 0 && idsSubfamilias.Count == 0)
                throw new Exception("La familia debe contener al menos una patente o una subfamilia.");

            // ─── Validación 1: redundancia entre patentes directas y subfamilias ───
            // Si una patente directa ya está incluida (recursivamente) en alguna de
            // las subfamilias seleccionadas, la patente directa es redundante.
            // Ej.: Familia A = {Crear, Modificar}. Intento crear B = {A + Crear} → Crear ya está en A.
            ValidarRedundanciaPatentesYSubfamilias(idsPatentes, idsSubfamilias);

            // ─── Validación 2: composición EFECTIVA duplicada ───
            // Comparamos el set de patentes EFECTIVAS (planas, después de expandir todas las
            // subfamilias) de la nueva familia contra el de cada familia existente. Si alguna
            // coincide exactamente, la nueva sería equivalente y la rechazamos.
            HashSet<int> patentesEfectivasNueva = ConstruirPatentesEfectivas(idsPatentes, idsSubfamilias);
            Familia_GV42 equivalente = BuscarFamiliaConMismasPatentesEfectivas(patentesEfectivasNueva);
            if (equivalente != null)
                throw new Exception(
                    $"Ya existe una familia con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten familias duplicadas.");

            return _dalFamilia.Crear(nombre, idsPatentes, idsSubfamilias);
        }

        // Chequea que ninguna patente directa esté también incluida en alguna de las
        // subfamilias seleccionadas. Si encuentra una, lanza Exception con el detalle
        // de qué patente y en qué subfamilia está, para que el usuario sepa qué quitar.
        private void ValidarRedundanciaPatentesYSubfamilias(List<int> idsPatentes, List<int> idsSubfamilias)
        {
            if (idsPatentes.Count == 0 || idsSubfamilias.Count == 0) return;

            // Para cada subfamilia, expandimos su árbol y guardamos qué patentes aporta.
            foreach (int idSub in idsSubfamilias)
            {
                Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idSub);
                if (arbol == null) continue;

                HashSet<int> patentesDeLaSub = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));

                foreach (int idPat in idsPatentes)
                {
                    if (patentesDeLaSub.Contains(idPat))
                    {
                        // Buscamos el nombre de la patente para el mensaje.
                        Patente_GV42 pat = _dalPatente.BuscarPorId(idPat);
                        string nombrePat = pat != null ? pat.Nombre : $"Id {idPat}";
                        throw new Exception(
                            $"La patente '{nombrePat}' ya está incluida en la subfamilia '{arbol.Nombre}'. " +
                            "Es redundante agregarla directamente. Quitala de las patentes individuales " +
                            "o quitá la subfamilia.");
                    }
                }
            }
        }

        // Construye el set completo de patentes efectivas (planas) que tendría una
        // familia con la composición indicada. Une las patentes directas con todas
        // las que aportan las subfamilias recursivamente.
        private HashSet<int> ConstruirPatentesEfectivas(List<int> idsPatentes, List<int> idsSubfamilias)
        {
            HashSet<int> efectivas = new HashSet<int>(idsPatentes);
            foreach (int idSub in idsSubfamilias)
            {
                Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idSub);
                if (arbol == null) continue;
                foreach (var p in arbol.ObtenerPatentes())
                    efectivas.Add(p.Id);
            }
            return efectivas;
        }

        // Recorre las familias existentes y devuelve la primera cuyo set de patentes
        // EFECTIVAS coincida exactamente con el indicado. Esto detecta duplicados
        // aunque la composición directa sea distinta (ej. una familia se arma con
        // patentes sueltas y otra con una subfamilia equivalente).
        private Familia_GV42 BuscarFamiliaConMismasPatentesEfectivas(HashSet<int> patentesEfectivas)
        {
            foreach (var fam in _dalFamilia.ListarTodasPlanas())
            {
                Familia_GV42 arbol = _dalFamilia.ObtenerArbol(fam.Id);
                if (arbol == null) continue;
                HashSet<int> efectivas = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                if (efectivas.SetEquals(patentesEfectivas))
                    return fam;
            }
            return null;
        }

        // (Método antiguo `BuscarFamiliaConMismaComposicion` reemplazado por
        // `BuscarFamiliaConMismasPatentesEfectivas`, que detecta duplicados
        // a nivel del conjunto efectivo de patentes y no solo de la composición
        // directa.)
        private Familia_GV42 _Obsoleto_BuscarFamiliaConMismaComposicion_NoUsar(
            List<int> idsPatentesNuevas, List<int> idsSubfamiliasNuevas)
        {
            return null;
        }

        public void EliminarFamilia(int idFamilia)
        {
            _dalFamilia.Eliminar(idFamilia);
        }

        // ─── Creación de Roles ─────────────────────────────────────────────

        public int CrearRol(string nombre, List<int> idsPatentes, List<int> idsFamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del rol es obligatorio.");

            // Validación: nombre único. La tabla Roles tiene UNIQUE en Nombre, pero
            // hacemos el chequeo acá para devolver un mensaje claro al usuario en
            // vez de tragarse la excepción del SQL.
            if (_dalRol.ListarTodos().Any(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe un rol con el nombre '{nombre}'. Elegí otro nombre.");

            idsPatentes = (idsPatentes ?? new List<int>()).Distinct().ToList();
            idsFamilias = (idsFamilias ?? new List<int>()).Distinct().ToList();

            if (idsPatentes.Count == 0 && idsFamilias.Count == 0)
                throw new Exception("El rol debe contener al menos una patente o una familia.");

            // ─── Validación 1: redundancia entre patentes directas y familias ───
            // Si una patente directa del rol ya está incluida (recursivamente) en
            // alguna de las familias seleccionadas, es redundante.
            ValidarRedundanciaPatentesYFamilias(idsPatentes, idsFamilias);

            // ─── Validación 2: composición EFECTIVA duplicada con otro rol ───
            // Si otro rol tiene exactamente el mismo set efectivo de patentes (planas,
            // después de expandir las familias), la composición es equivalente.
            HashSet<int> efectivasNuevo = ConstruirPatentesEfectivas(idsPatentes, idsFamilias);
            Rol_GV42 equivalente = BuscarRolConMismasPatentesEfectivas(efectivasNuevo);
            if (equivalente != null)
                throw new Exception(
                    $"Ya existe un rol con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten roles duplicados.");

            return _dalRol.Crear(nombre, idsPatentes, idsFamilias);
        }

        // Mismo criterio que ValidarRedundanciaPatentesYSubfamilias, pero para roles:
        // ninguna patente directa del rol puede estar también incluida en alguna de
        // las familias seleccionadas.
        private void ValidarRedundanciaPatentesYFamilias(List<int> idsPatentes, List<int> idsFamilias)
        {
            if (idsPatentes.Count == 0 || idsFamilias.Count == 0) return;

            foreach (int idFam in idsFamilias)
            {
                Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idFam);
                if (arbol == null) continue;

                HashSet<int> patentesDeLaFam = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));

                foreach (int idPat in idsPatentes)
                {
                    if (patentesDeLaFam.Contains(idPat))
                    {
                        Patente_GV42 pat = _dalPatente.BuscarPorId(idPat);
                        string nombrePat = pat != null ? pat.Nombre : $"Id {idPat}";
                        throw new Exception(
                            $"La patente '{nombrePat}' ya está incluida en la familia '{arbol.Nombre}'. " +
                            "Es redundante agregarla directamente. Quitala de las patentes individuales " +
                            "o quitá la familia.");
                    }
                }
            }
        }

        // Recorre todos los roles existentes y devuelve el primero cuyo set efectivo
        // de patentes coincida con el indicado. Detecta roles duplicados aunque hayan
        // sido armados con composiciones directas distintas (ej. uno con patentes
        // sueltas y otro con una familia que las contiene todas).
        private Rol_GV42 BuscarRolConMismasPatentesEfectivas(HashSet<int> patentesEfectivas)
        {
            foreach (var rol in _dalRol.ListarTodos())
            {
                Rol_GV42 arbol = _dalRol.ObtenerArbol(rol.Id);
                if (arbol == null) continue;
                HashSet<int> efectivas = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                if (efectivas.SetEquals(patentesEfectivas))
                    return rol;
            }
            return null;
        }

        // Elimina un rol si NO está siendo utilizado por ningún usuario.
        // Si está en uso, lanza una excepción con un mensaje claro indicando
        // cuántos usuarios lo tienen asignado.
        //
        // Esta es la regla de negocio del enunciado:
        //   "No se puede eliminar un rol que esté asignado a uno o más usuarios."
        public void EliminarRol(int idRol)
        {
            int cantUsuarios = _dalRol.CantidadUsuariosConRol(idRol);
            if (cantUsuarios > 0)
            {
                throw new Exception(
                    $"No se puede eliminar el rol porque está asignado a {cantUsuarios} usuario(s). " +
                    "Reasigná esos usuarios a otro rol antes de eliminarlo.");
            }

            _dalRol.Eliminar(idRol);
        }

        // ─── Verificación de permisos (uso típico desde la UI) ─────────────

        // ¿El rol con este Id tiene permiso para esta DataKey?
        // Carga el árbol completo y delega a Rol.TienePermiso, que recorre el
        // Composite recursivamente.
        public bool TienePermiso(int idRol, string dataKey)
        {
            Rol_GV42 rol = _dalRol.ObtenerArbol(idRol);
            return rol != null && rol.TienePermiso(dataKey);
        }
    }
}
