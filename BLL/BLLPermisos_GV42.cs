using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{

    public class BLLPermisos_GV42
    {
        private readonly DALPatente_GV42 _dalPatente;
        private readonly DALFamilia_GV42 _dalFamilia;
        private readonly DALRol_GV42 _dalRol;
        private readonly BLLIntegridad_GV42 _bllIntegridad;

        public BLLPermisos_GV42()
        {
            _dalPatente = new DALPatente_GV42();
            _dalFamilia = new DALFamilia_GV42();
            _dalRol = new DALRol_GV42();
            _bllIntegridad = new BLLIntegridad_GV42();
        }

        private void RecalcularFamilias()
        {
            try
            {
                _bllIntegridad.RecalcularTabla("Familia");
                _bllIntegridad.RecalcularTabla("FamiliaPatente");
                _bllIntegridad.RecalcularTabla("FamiliaIntegrada");
            } catch { }
        }

        private void RecalcularRoles()
        {
            try
            {
                _bllIntegridad.RecalcularTabla("Roles");
                _bllIntegridad.RecalcularTabla("RolPatente");
                _bllIntegridad.RecalcularTabla("RolFamilia");
            } catch { }
        }

        public List<Patente_GV42> ListarPatentes() => _dalPatente.ListarTodas();
        public List<Familia_GV42> ListarFamilias() => _dalFamilia.ListarTodasPlanas();
        public List<Rol_GV42> ListarRoles()      => _dalRol.ListarTodos();

        public Familia_GV42 ObtenerArbolFamilia(int idFamilia) => _dalFamilia.ObtenerArbol(idFamilia);
        public Rol_GV42     ObtenerArbolRol(int idRol)         => _dalRol.ObtenerArbol(idRol);

        public int CrearFamilia(string nombre, List<int> idsPatentes, List<int> idsSubfamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre de la familia es obligatorio.");

            if (_dalFamilia.ListarTodasPlanas().Any(f => f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe una familia con el nombre '{nombre}'. Elegí otro nombre.");

            idsPatentes    = (idsPatentes ?? new List<int>()).Distinct().OrderBy(i => i).ToList();
            idsSubfamilias = (idsSubfamilias ?? new List<int>()).Distinct().OrderBy(i => i).ToList();

            if (idsPatentes.Count == 0 && idsSubfamilias.Count == 0)
                throw new Exception("La familia debe contener al menos una patente o una subfamilia.");

            ValidarRedundanciaPatentesYSubfamilias(idsPatentes, idsSubfamilias);

            HashSet<int> patentesEfectivasNueva = ConstruirPatentesEfectivas(idsPatentes, idsSubfamilias);
            Familia_GV42 equivalente = BuscarFamiliaConMismasPatentesEfectivas(patentesEfectivasNueva);
            if (equivalente != null)
                throw new Exception($"Ya existe una familia con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten familias duplicadas.");

            int idCreada = _dalFamilia.Crear(nombre, idsPatentes, idsSubfamilias);
            RecalcularFamilias();
            return idCreada;
        }

        private void ValidarRedundanciaPatentesYSubfamilias(List<int> idsPatentes, List<int> idsSubfamilias)
        {
            // ── (a) Patente directa que ya viene por alguna subfamilia ──
            if (idsPatentes.Count > 0 && idsSubfamilias.Count > 0)
            {
                foreach (int idSub in idsSubfamilias)
                {
                    Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idSub);
                    if (arbol == null) continue;

                    HashSet<int> patentesDeLaSub = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));

                    foreach (int idPat in idsPatentes)
                    {
                        if (patentesDeLaSub.Contains(idPat))
                        {
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

            // ── (b) Dos subfamilias hermanas comparten alguna patente ──
            if (idsSubfamilias.Count >= 2)
            {
                var efectivasPorSub = new Dictionary<int, HashSet<int>>();
                var nombresPorSub = new Dictionary<int, string>();
                foreach (int idSub in idsSubfamilias)
                {
                    Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idSub);
                    if (arbol == null) continue;
                    efectivasPorSub[idSub] = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                    nombresPorSub[idSub] = arbol.Nombre;
                }

                var subs = efectivasPorSub.Keys.ToList();
                for (int i = 0; i < subs.Count; i++)
                {
                    for (int j = i + 1; j < subs.Count; j++)
                    {
                        var solapadas = efectivasPorSub[subs[i]].Intersect(efectivasPorSub[subs[j]]).ToList();
                        if (solapadas.Any())
                        {
                            Patente_GV42 pat = _dalPatente.BuscarPorId(solapadas.First());
                            string nombrePat = pat != null ? pat.Nombre : $"Id {solapadas.First()}";
                            throw new Exception(
                                $"La patente '{nombrePat}' está incluida tanto en la subfamilia '{nombresPorSub[subs[i]]}' " +
                                $"como en '{nombresPorSub[subs[j]]}'. Es redundante incluir ambas subfamilias.");
                        }
                    }
                }
            }
        }

        private void ValidarRedundanciaEnAscendentesFamilia(int idFamiliaModificada, HashSet<int> nuevasEfectivas)
        {
            foreach (var famAncestro in _dalFamilia.ListarTodasPlanas())
            {
                if (famAncestro.Id == idFamiliaModificada) continue;

                Familia_GV42 arbolAncestro = _dalFamilia.ObtenerArbol(famAncestro.Id);
                if (arbolAncestro == null) continue;

                if (!ContieneFamiliaRec(arbolAncestro, idFamiliaModificada)) continue;

                // (a) Patentes directas del ancestro que se solapan con las nuevas efectivas.
                var patentesDirectas = _dalFamilia.IdsPatentesDirectas(famAncestro.Id);
                var solapDirectas = patentesDirectas.Intersect(nuevasEfectivas).ToList();
                if (solapDirectas.Any())
                {
                    Patente_GV42 pat = _dalPatente.BuscarPorId(solapDirectas.First());
                    string nombrePat = pat != null ? pat.Nombre : $"Id {solapDirectas.First()}";
                    throw new Exception(
                        $"No se puede aplicar la modificación: la patente '{nombrePat}' quedaría duplicada " +
                        $"en la familia ancestro '{famAncestro.Nombre}' (ya está allí como patente directa " +
                        $"y aparecería también por esta familia). Quitala del ancestro primero.");
                }

                // (b) Patentes que aporta otra subfamilia hermana del ancestro, solapadas con las nuevas efectivas.
                var subsDelAncestro = _dalFamilia.IdsSubfamiliasDirectas(famAncestro.Id);
                foreach (int idHermana in subsDelAncestro)
                {
                    if (idHermana == idFamiliaModificada) continue;
                    Familia_GV42 arbolHermana = _dalFamilia.ObtenerArbol(idHermana);
                    if (arbolHermana == null) continue;
                    if (ContieneFamiliaRec(arbolHermana, idFamiliaModificada)) continue;

                    HashSet<int> efectivasHermana = new HashSet<int>(arbolHermana.ObtenerPatentes().Select(p => p.Id));
                    var solapHermana = efectivasHermana.Intersect(nuevasEfectivas).ToList();
                    if (solapHermana.Any())
                    {
                        Patente_GV42 pat = _dalPatente.BuscarPorId(solapHermana.First());
                        string nombrePat = pat != null ? pat.Nombre : $"Id {solapHermana.First()}";
                        throw new Exception(
                            $"No se puede aplicar la modificación: la patente '{nombrePat}' quedaría duplicada " +
                            $"en el ancestro '{famAncestro.Nombre}' (ya está incluida vía la subfamilia hermana " +
                            $"'{arbolHermana.Nombre}'). Quitala de la hermana o no la agregues acá.");
                    }
                }
            }

            // ── Mismas dos validaciones pero para roles que contengan la familia ──
            foreach (var rol in _dalRol.ListarTodos())
            {
                Rol_GV42 arbolRol = _dalRol.ObtenerArbol(rol.Id);
                if (arbolRol == null) continue;
                if (!RolContieneFamiliaRec(arbolRol, idFamiliaModificada)) continue;

                var patentesDirectasRol = _dalRol.IdsPatentesDirectas(rol.Id);
                var solapDirectas = patentesDirectasRol.Intersect(nuevasEfectivas).ToList();
                if (solapDirectas.Any())
                {
                    Patente_GV42 pat = _dalPatente.BuscarPorId(solapDirectas.First());
                    string nombrePat = pat != null ? pat.Nombre : $"Id {solapDirectas.First()}";
                    throw new Exception(
                        $"No se puede aplicar la modificación: la patente '{nombrePat}' quedaría duplicada " +
                        $"en el rol '{rol.Nombre}'. Quitala del rol primero.");
                }

                var familiasDelRol = _dalRol.IdsFamiliasDirectas(rol.Id);
                foreach (int idFamHermana in familiasDelRol)
                {
                    if (idFamHermana == idFamiliaModificada) continue;
                    Familia_GV42 arbolHermana = _dalFamilia.ObtenerArbol(idFamHermana);
                    if (arbolHermana == null) continue;
                    if (ContieneFamiliaRec(arbolHermana, idFamiliaModificada)) continue;

                    HashSet<int> efectivasHermana = new HashSet<int>(arbolHermana.ObtenerPatentes().Select(p => p.Id));
                    var solapHermana = efectivasHermana.Intersect(nuevasEfectivas).ToList();
                    if (solapHermana.Any())
                    {
                        Patente_GV42 pat = _dalPatente.BuscarPorId(solapHermana.First());
                        string nombrePat = pat != null ? pat.Nombre : $"Id {solapHermana.First()}";
                        throw new Exception(
                            $"No se puede aplicar la modificación: la patente '{nombrePat}' quedaría duplicada " +
                            $"en el rol '{rol.Nombre}' (ya está incluida vía la familia '{arbolHermana.Nombre}').");
                    }
                }
            }
        }

        private bool RolContieneFamiliaRec(Rol_GV42 rol, int idFamiliaBuscada)
        {
            if (rol == null) return false;
            foreach (var hijo in rol.Hijos)
            {
                if (hijo is Familia_GV42 fam)
                {
                    if (fam.Id == idFamiliaBuscada) return true;
                    if (ContieneFamiliaRec(fam, idFamiliaBuscada)) return true;
                }
            }
            return false;
        }

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

        private Familia_GV42 BuscarFamiliaConMismasPatentesEfectivas(HashSet<int> patentesEfectivas, int excluirId = 0)
        {
            foreach (var fam in _dalFamilia.ListarTodasPlanas())
            {
                if (fam.Id == excluirId) continue;
                Familia_GV42 arbol = _dalFamilia.ObtenerArbol(fam.Id);
                if (arbol == null) continue;
                HashSet<int> efectivas = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                if (efectivas.SetEquals(patentesEfectivas))
                    return fam;
            }
            return null;
        }

        public void EliminarFamilia(int idFamilia)
        {
            List<string> rolesQueLaUsan = _dalFamilia.NombresRolesQueUsan(idFamilia);
            if (rolesQueLaUsan.Count > 0)
            {
                throw new Exception(
                    $"No se puede eliminar la familia porque está asignada a {rolesQueLaUsan.Count} rol(es): " +
                    $"{string.Join(", ", rolesQueLaUsan)}. " +
                    "Modificá esos roles para quitarles la familia antes de eliminarla.");
            }

            List<string> familiasQueLaContienen = _dalFamilia.NombresFamiliasQueLaContienen(idFamilia);
            if (familiasQueLaContienen.Count > 0)
            {
                throw new Exception(
                    $"No se puede eliminar la familia porque es subfamilia de {familiasQueLaContienen.Count} familia(s): " +
                    $"{string.Join(", ", familiasQueLaContienen)}. " +
                    "Modificá esas familias para quitarles esta subfamilia antes de eliminarla.");
            }

            _dalFamilia.Eliminar(idFamilia);
            RecalcularFamilias();
        }

        public int CrearRol(string nombre, List<int> idsPatentes, List<int> idsFamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del rol es obligatorio.");

            if (_dalRol.ListarTodos().Any(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe un rol con el nombre '{nombre}'. Elegí otro nombre.");

            idsPatentes = (idsPatentes ?? new List<int>()).Distinct().ToList();
            idsFamilias = (idsFamilias ?? new List<int>()).Distinct().ToList();

            if (idsPatentes.Count == 0 && idsFamilias.Count == 0)
                throw new Exception("El rol debe contener al menos una patente o una familia.");

            ValidarRedundanciaPatentesYFamilias(idsPatentes, idsFamilias);

            HashSet<int> efectivasNuevo = ConstruirPatentesEfectivas(idsPatentes, idsFamilias);
            Rol_GV42 equivalente = BuscarRolConMismasPatentesEfectivas(efectivasNuevo);
            if (equivalente != null)
                throw new Exception(
                    $"Ya existe un rol con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten roles duplicados.");

            int idCreado = _dalRol.Crear(nombre, idsPatentes, idsFamilias);
            RecalcularRoles();
            return idCreado;
        }

        private void ValidarRedundanciaPatentesYFamilias(List<int> idsPatentes, List<int> idsFamilias)
        {
            // ── (a) Patente directa que ya viene por alguna familia ──
            if (idsPatentes.Count > 0 && idsFamilias.Count > 0)
            {
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

            // ── (b) Dos familias hermanas comparten alguna patente ──
            if (idsFamilias.Count >= 2)
            {
                var efectivasPorFam = new Dictionary<int, HashSet<int>>();
                var nombresPorFam = new Dictionary<int, string>();
                foreach (int idFam in idsFamilias)
                {
                    Familia_GV42 arbol = _dalFamilia.ObtenerArbol(idFam);
                    if (arbol == null) continue;
                    efectivasPorFam[idFam] = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                    nombresPorFam[idFam] = arbol.Nombre;
                }

                var fams = efectivasPorFam.Keys.ToList();
                for (int i = 0; i < fams.Count; i++)
                {
                    for (int j = i + 1; j < fams.Count; j++)
                    {
                        var solapadas = efectivasPorFam[fams[i]].Intersect(efectivasPorFam[fams[j]]).ToList();
                        if (solapadas.Any())
                        {
                            Patente_GV42 pat = _dalPatente.BuscarPorId(solapadas.First());
                            string nombrePat = pat != null ? pat.Nombre : $"Id {solapadas.First()}";
                            throw new Exception(
                                $"La patente '{nombrePat}' está incluida tanto en la familia '{nombresPorFam[fams[i]]}' " +
                                $"como en '{nombresPorFam[fams[j]]}'. Es redundante incluir ambas familias.");
                        }
                    }
                }
            }
        }

        private Rol_GV42 BuscarRolConMismasPatentesEfectivas(HashSet<int> patentesEfectivas, int excluirId = 0)
        {
            foreach (var rol in _dalRol.ListarTodos())
            {
                if (rol.Id == excluirId) continue;
                Rol_GV42 arbol = _dalRol.ObtenerArbol(rol.Id);
                if (arbol == null) continue;
                HashSet<int> efectivas = new HashSet<int>(arbol.ObtenerPatentes().Select(p => p.Id));
                if (efectivas.SetEquals(patentesEfectivas))
                    return rol;
            }
            return null;
        }

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
            RecalcularRoles();
        }

        public bool TienePermiso(int idRol, string dataKey)
        {
            Rol_GV42 rol = _dalRol.ObtenerArbol(idRol);
            return rol != null && rol.TienePermiso(dataKey);
        }

        public void ModificarFamilia(int idFamilia, string nombre, List<int> idsPatentes, List<int> idsSubfamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre de la familia es obligatorio.");

            if (_dalFamilia.ListarTodasPlanas()
                .Any(f => f.Id != idFamilia && f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe otra familia con el nombre '{nombre}'.");

            idsPatentes    = (idsPatentes ?? new List<int>()).Distinct().OrderBy(i => i).ToList();
            idsSubfamilias = (idsSubfamilias ?? new List<int>()).Distinct().OrderBy(i => i).ToList();

            if (idsPatentes.Count == 0 && idsSubfamilias.Count == 0)
                throw new Exception("La familia debe contener al menos una patente o una subfamilia.");

            if (idsSubfamilias.Contains(idFamilia))
                throw new Exception("Una familia no puede contenerse a sí misma como subfamilia.");

            foreach (int idSub in idsSubfamilias)
            {
                Familia_GV42 arbolSub = _dalFamilia.ObtenerArbol(idSub);
                if (arbolSub == null) continue;
                if (ContieneFamiliaRec(arbolSub, idFamilia))
                    throw new Exception(
                        $"No se puede agregar la subfamilia '{arbolSub.Nombre}' porque ya contiene a esta familia. " +
                        "Se generaría un ciclo.");
            }

            ValidarRedundanciaPatentesYSubfamilias(idsPatentes, idsSubfamilias);

            HashSet<int> patentesEfectivasNueva = ConstruirPatentesEfectivas(idsPatentes, idsSubfamilias);

            ValidarRedundanciaEnAscendentesFamilia(idFamilia, patentesEfectivasNueva);

            Familia_GV42 equivalente = BuscarFamiliaConMismasPatentesEfectivas(patentesEfectivasNueva, excluirId: idFamilia);
            if (equivalente != null)
                throw new Exception($"Ya existe otra familia con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten familias duplicadas.");


            if (equivalente != null)
                throw new Exception(
                    $"Ya existe otra familia con la misma composición efectiva de patentes: '{equivalente.Nombre}'.");

            _dalFamilia.Modificar(idFamilia, nombre, idsPatentes, idsSubfamilias);
            RecalcularFamilias();
        }

        public void ModificarRol(int idRol, string nombre, List<int> idsPatentes, List<int> idsFamilias)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del rol es obligatorio.");

            if (_dalRol.ListarTodos()
                .Any(r => r.Id != idRol && r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Ya existe otro rol con el nombre '{nombre}'.");

            idsPatentes = (idsPatentes ?? new List<int>()).Distinct().ToList();
            idsFamilias = (idsFamilias ?? new List<int>()).Distinct().ToList();

            if (idsPatentes.Count == 0 && idsFamilias.Count == 0)
                throw new Exception("El rol debe contener al menos una patente o una familia.");

            ValidarRedundanciaPatentesYFamilias(idsPatentes, idsFamilias);

            HashSet<int> efectivasNuevo = ConstruirPatentesEfectivas(idsPatentes, idsFamilias);
            Rol_GV42 equivalente = BuscarRolConMismasPatentesEfectivas(efectivasNuevo, excluirId: idRol);
            if (equivalente != null)
                throw new Exception(
                    $"Ya existe otro rol con la misma composición efectiva de patentes: '{equivalente.Nombre}'.");

            _dalRol.Modificar(idRol, nombre, idsPatentes, idsFamilias);
            RecalcularRoles();
        }

        private bool ContieneFamiliaRec(Familia_GV42 nodo, int idBuscado)
        {
            if (nodo == null) return false;
            foreach (var hijo in nodo.Hijos)
            {
                if (hijo is Familia_GV42 sub)
                {
                    if (sub.Id == idBuscado) return true;
                    if (ContieneFamiliaRec(sub, idBuscado)) return true;
                }
            }
            return false;
        }
    }
}
