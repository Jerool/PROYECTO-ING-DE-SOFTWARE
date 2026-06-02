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

        public BLLPermisos_GV42()
        {
            _dalPatente = new DALPatente_GV42();
            _dalFamilia = new DALFamilia_GV42();
            _dalRol = new DALRol_GV42();
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
                throw new Exception(
                    $"Ya existe una familia con la misma composición efectiva de patentes: '{equivalente.Nombre}'. " +
                    "No se permiten familias duplicadas.");

            return _dalFamilia.Crear(nombre, idsPatentes, idsSubfamilias);
        }

        private void ValidarRedundanciaPatentesYSubfamilias(List<int> idsPatentes, List<int> idsSubfamilias)
        {
            if (idsPatentes.Count == 0 || idsSubfamilias.Count == 0) return;

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

        public void EliminarFamilia(int idFamilia)
        {
            _dalFamilia.Eliminar(idFamilia);
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

            return _dalRol.Crear(nombre, idsPatentes, idsFamilias);
        }

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


        public bool TienePermiso(int idRol, string dataKey)
        {
            Rol_GV42 rol = _dalRol.ObtenerArbol(idRol);
            return rol != null && rol.TienePermiso(dataKey);
        }
    }
}
