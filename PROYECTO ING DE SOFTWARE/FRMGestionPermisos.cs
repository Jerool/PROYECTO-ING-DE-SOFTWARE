using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ING_DE_SOFTWARE
{
    // Form principal del módulo de permisos (Composite en la UI).
    // Pestañas: Patentes (solo lectura), Familias y Roles (alta/baja).
    // Implementa IObservadorIdioma_GV42: se actualiza cuando cambia el idioma.
    public partial class FRMGestionPermisos : Form, IObservadorIdioma_GV42
    {
        private readonly BLLPermisos_GV42 _bll;

        public FRMGestionPermisos()
        {
            InitializeComponent();
            _bll = new BLLPermisos_GV42();

            IdiomaManager_GV42.Instancia.Suscribir(this);
            this.FormClosed += (s, e) => IdiomaManager_GV42.Instancia.Desuscribir(this);
        }

        private void FRMGestionPermisos_Load(object sender, EventArgs e)
        {
            RecargarTodo();
            ActualizarIdioma();
        }

        private void RecargarTodo()
        {
            CargarPatentes();
            CargarFamilias();
            CargarRoles();
        }

        // ═══════════════════════════════════════════════════════════════════
        // PATENTES (solo lectura)
        // ═══════════════════════════════════════════════════════════════════

        private void CargarPatentes()
        {
            List<Patente_GV42> patentes = _bll.ListarPatentes();

            // Sobrescribimos el Nombre de cada patente con su traducción según el
            // DataKey. Así la grilla y los CheckedListBox muestran los textos en
            // el idioma actual sin tener que tocar la base de datos.
            foreach (var p in patentes)
                p.Nombre = TraducirNombrePatente(p);

            dgvPatentes.DataSource = null;
            dgvPatentes.DataSource = patentes;
            if (dgvPatentes.Columns.Contains("Id"))
                dgvPatentes.Columns["Id"].Visible = false;
            AplicarHeadersPatentes();

            clbPatentesFamilia.Items.Clear();
            clbPatentesRol.Items.Clear();
            foreach (var p in patentes)
            {
                clbPatentesFamilia.Items.Add(p, false);
                clbPatentesRol.Items.Add(p, false);
            }
        }

        // Devuelve el nombre traducido a partir de la DataKey. Si no hay clave
        // configurada en el archivo de idioma, deja el nombre original (fallback).
        private string TraducirNombrePatente(Patente_GV42 p)
        {
            if (p == null || string.IsNullOrEmpty(p.DataKey)) return p?.Nombre ?? string.Empty;
            string clave = "patente." + p.DataKey;
            string traducido = IdiomaManager_GV42.T(clave);
            // T() devuelve la clave si no encuentra traducción → en ese caso usamos el original.
            return traducido == clave ? p.Nombre : traducido;
        }

        private void AplicarHeadersPatentes()
        {
            if (dgvPatentes.Columns.Contains("Nombre"))
                dgvPatentes.Columns["Nombre"].HeaderText = IdiomaManager_GV42.T("permisos.colNombre");
            if (dgvPatentes.Columns.Contains("DataKey"))
                dgvPatentes.Columns["DataKey"].HeaderText = IdiomaManager_GV42.T("permisos.colDataKey");
        }

        private void AplicarHeadersFamilias()
        {
            if (dgvFamilias.Columns.Contains("Nombre"))
                dgvFamilias.Columns["Nombre"].HeaderText = IdiomaManager_GV42.T("permisos.colNombre");
        }

        private void AplicarHeadersRoles()
        {
            if (dgvRoles.Columns.Contains("Nombre"))
                dgvRoles.Columns["Nombre"].HeaderText = IdiomaManager_GV42.T("permisos.colNombre");
        }

        // ═══════════════════════════════════════════════════════════════════
        // FAMILIAS
        // ═══════════════════════════════════════════════════════════════════

        private void CargarFamilias()
        {
            List<Familia_GV42> familias = _bll.ListarFamilias();

            dgvFamilias.DataSource = null;
            dgvFamilias.DataSource = familias;
            if (dgvFamilias.Columns.Contains("Id"))
                dgvFamilias.Columns["Id"].Visible = false;
            if (dgvFamilias.Columns.Contains("Hijos"))
                dgvFamilias.Columns["Hijos"].Visible = false;
            AplicarHeadersFamilias();

            clbSubfamilias.Items.Clear();
            clbFamiliasRol.Items.Clear();
            foreach (var f in familias)
            {
                clbSubfamilias.Items.Add(f, false);
                clbFamiliasRol.Items.Add(f, false);
            }
        }

        private void btnGuardarFamilia_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreFamilia.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show(IdiomaManager_GV42.T("permisos.ingresaNombreFamilia"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreFamilia.Focus();
                return;
            }

            List<int> idsPatentes = clbPatentesFamilia.CheckedItems
                .Cast<Patente_GV42>()
                .Select(p => p.Id)
                .ToList();

            List<int> idsSubfamilias = clbSubfamilias.CheckedItems
                .Cast<Familia_GV42>()
                .Select(f => f.Id)
                .ToList();

            try
            {
                _bll.CrearFamilia(nombre, idsPatentes, idsSubfamilias);
                MessageBox.Show(IdiomaManager_GV42.T("permisos.familiaCreada"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormularioFamilia();
                RecargarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                IdiomaManager_GV42.T("general.error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarFamilia_Click(object sender, EventArgs e)
        {
            LimpiarFormularioFamilia();
        }

        private void LimpiarFormularioFamilia()
        {
            txtNombreFamilia.Clear();
            for (int i = 0; i < clbPatentesFamilia.Items.Count; i++) clbPatentesFamilia.SetItemChecked(i, false);
            for (int i = 0; i < clbSubfamilias.Items.Count; i++) clbSubfamilias.SetItemChecked(i, false);
        }

        private void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            if (dgvFamilias.CurrentRow == null)
            {
                MessageBox.Show(IdiomaManager_GV42.T("permisos.seleccioneFamilia"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Familia_GV42 fam = dgvFamilias.CurrentRow.DataBoundItem as Familia_GV42;
            if (fam == null) return;

            DialogResult r = MessageBox.Show(
                $"{IdiomaManager_GV42.T("permisos.confirmEliminarFamilia")} '{fam.Nombre}'?",
                IdiomaManager_GV42.T("permisos.tituloEliminar"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                _bll.EliminarFamilia(fam.Id);
                MessageBox.Show(IdiomaManager_GV42.T("permisos.familiaEliminada"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                RecargarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                IdiomaManager_GV42.T("general.error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // ROLES
        // ═══════════════════════════════════════════════════════════════════

        private void CargarRoles()
        {
            List<Rol_GV42> roles = _bll.ListarRoles();
            dgvRoles.DataSource = null;
            dgvRoles.DataSource = roles;
            if (dgvRoles.Columns.Contains("Id"))
                dgvRoles.Columns["Id"].Visible = false;
            if (dgvRoles.Columns.Contains("Hijos"))
                dgvRoles.Columns["Hijos"].Visible = false;
            AplicarHeadersRoles();
        }

        private void btnGuardarRol_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreRol.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show(IdiomaManager_GV42.T("permisos.ingresaNombreRol"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreRol.Focus();
                return;
            }

            List<int> idsPatentes = clbPatentesRol.CheckedItems
                .Cast<Patente_GV42>()
                .Select(p => p.Id)
                .ToList();

            List<int> idsFamilias = clbFamiliasRol.CheckedItems
                .Cast<Familia_GV42>()
                .Select(f => f.Id)
                .ToList();

            try
            {
                _bll.CrearRol(nombre, idsPatentes, idsFamilias);
                MessageBox.Show(IdiomaManager_GV42.T("permisos.rolCreado"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormularioRol();
                RecargarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                IdiomaManager_GV42.T("general.error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarRol_Click(object sender, EventArgs e)
        {
            LimpiarFormularioRol();
        }

        private void LimpiarFormularioRol()
        {
            txtNombreRol.Clear();
            for (int i = 0; i < clbPatentesRol.Items.Count; i++) clbPatentesRol.SetItemChecked(i, false);
            for (int i = 0; i < clbFamiliasRol.Items.Count; i++) clbFamiliasRol.SetItemChecked(i, false);
        }

        private void btnEliminarRol_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show(IdiomaManager_GV42.T("permisos.seleccioneRol"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Rol_GV42 rol = dgvRoles.CurrentRow.DataBoundItem as Rol_GV42;
            if (rol == null) return;

            DialogResult r = MessageBox.Show(
                $"{IdiomaManager_GV42.T("permisos.confirmEliminarRol")} '{rol.Nombre}'?",
                IdiomaManager_GV42.T("permisos.tituloEliminar"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                // El BLL lanza Exception si el rol está asignado a algún usuario.
                _bll.EliminarRol(rol.Id);
                MessageBox.Show(IdiomaManager_GV42.T("permisos.rolEliminado"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                RecargarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                IdiomaManager_GV42.T("permisos.noSePuedeEliminar"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // OBSERVER DE IDIOMA
        // ═══════════════════════════════════════════════════════════════════

        public void ActualizarIdioma()
        {
            this.Text = IdiomaManager_GV42.T("permisos.titulo");

            // Tabs
            if (tabPatentes != null) tabPatentes.Text = IdiomaManager_GV42.T("permisos.tabPatentes");
            if (tabFamilias != null) tabFamilias.Text = IdiomaManager_GV42.T("permisos.tabFamilias");
            if (tabRoles != null) tabRoles.Text = IdiomaManager_GV42.T("permisos.tabRoles");

            // Tab Patentes
            if (lblTitPatentes != null) lblTitPatentes.Text = IdiomaManager_GV42.T("permisos.titPatentes");

            // Tab Familias
            if (lblTitFamilias != null) lblTitFamilias.Text = IdiomaManager_GV42.T("permisos.titFamilias");
            if (btnEliminarFamilia != null) btnEliminarFamilia.Text = IdiomaManager_GV42.T("permisos.eliminarFamilia");
            if (gbCrearFamilia != null) gbCrearFamilia.Text = IdiomaManager_GV42.T("permisos.crearFamilia");
            if (lblNombreFamilia != null) lblNombreFamilia.Text = IdiomaManager_GV42.T("permisos.nombre");
            if (lblPatentesFamilia != null) lblPatentesFamilia.Text = IdiomaManager_GV42.T("permisos.patentesAIncluir");
            if (lblSubfamilias != null) lblSubfamilias.Text = IdiomaManager_GV42.T("permisos.subfamilias");
            if (btnGuardarFamilia != null) btnGuardarFamilia.Text = IdiomaManager_GV42.T("permisos.guardar");
            if (btnLimpiarFamilia != null) btnLimpiarFamilia.Text = IdiomaManager_GV42.T("permisos.limpiar");

            // Tab Roles
            if (lblTitRoles != null) lblTitRoles.Text = IdiomaManager_GV42.T("permisos.titRoles");
            if (btnEliminarRol != null) btnEliminarRol.Text = IdiomaManager_GV42.T("permisos.eliminarRol");
            if (gbCrearRol != null) gbCrearRol.Text = IdiomaManager_GV42.T("permisos.crearRol");
            if (lblNombreRol != null) lblNombreRol.Text = IdiomaManager_GV42.T("permisos.nombre");
            if (lblPatentesRol != null) lblPatentesRol.Text = IdiomaManager_GV42.T("permisos.patentesIndividuales");
            if (lblFamiliasRol != null) lblFamiliasRol.Text = IdiomaManager_GV42.T("permisos.familias");
            if (btnGuardarRol != null) btnGuardarRol.Text = IdiomaManager_GV42.T("permisos.guardar");
            if (btnLimpiarRol != null) btnLimpiarRol.Text = IdiomaManager_GV42.T("permisos.limpiar");

            // Si las grillas ya tienen datos cargados, las recargamos para que los
            // nombres de las patentes aparezcan traducidos al nuevo idioma. Si el
            // form todavía no terminó de cargarse, omitimos: el Load va a cargar
            // todo en el idioma actual.
            if (dgvPatentes != null && dgvPatentes.Columns.Count > 0)
                RecargarTodo();
        }
    }
}
