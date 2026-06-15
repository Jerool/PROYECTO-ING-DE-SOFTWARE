using BLL;
using Servicios;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ING_DE_SOFTWARE
{
    public partial class FRMMenuPrincipalAdmin : Form, IObservadorIdioma_GV42
    {
        private Form _formularioActual = null;
        private readonly BLLUsuario_GV42 _bllUsuario;

        private ToolStripMenuItem _menuIdioma;
        private ToolStripMenuItem _itemEspanol;
        private ToolStripMenuItem _itemIngles;

        public FRMMenuPrincipalAdmin()
        {
            InitializeComponent();
            _bllUsuario = new BLLUsuario_GV42();

            IdiomaManager_GV42.Instancia.Suscribir(this);
            this.FormClosed += (s, e) => IdiomaManager_GV42.Instancia.Desuscribir(this);

            ConstruirMenuIdioma();
            ActualizarIdioma();
        }

        private void ConstruirMenuIdioma()
        {
            if (menuStrip1 == null) return;

            _itemEspanol = new ToolStripMenuItem("Español");
            _itemEspanol.Click += (s, e) => _bllUsuario.CambiarIdioma(IdiomaManager_GV42.ES);

            _itemIngles = new ToolStripMenuItem("English");
            _itemIngles.Click += (s, e) => _bllUsuario.CambiarIdioma(IdiomaManager_GV42.EN);

            _menuIdioma = new ToolStripMenuItem("Idioma");
            _menuIdioma.DropDownItems.Add(_itemEspanol);
            _menuIdioma.DropDownItems.Add(_itemIngles);

            menuStrip1.Items.Add(_menuIdioma);
        }

        public void ActualizarIdioma()
        {
            if (adminToolStripMenuItem != null) adminToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.admin");
            if (usuariosToolStripMenuItem != null) usuariosToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.usuarios");
            if (bitacoraToolStripMenuItem != null) bitacoraToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.bitacora");
            if (gestionDePermisosToolStripMenuItem != null) gestionDePermisosToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.gestionPermisos");
            if (usuarioToolStripMenuItem != null) usuarioToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.usuario");
            if (reLoginToolStripMenuItem != null) reLoginToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.relogin");
            if (cambiarClaveToolStripMenuItem != null) cambiarClaveToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.cambiarClave");
            if (logOutToolStripMenuItem != null) logOutToolStripMenuItem.Text = IdiomaManager_GV42.T("menu.logout");

            if (_menuIdioma != null) _menuIdioma.Text = IdiomaManager_GV42.T("menu.idioma");
            if (_itemEspanol != null) _itemEspanol.Text = IdiomaManager_GV42.T("general.espanol");
            if (_itemIngles != null) _itemIngles.Text = IdiomaManager_GV42.T("general.ingles");
        }

        private void FRMMenuPrincipalAdmin_Load(object sender, EventArgs e)
        {
            Usuario_GV42 actual = SessionManager_GV42.Instancia.ObtenerUsuarioActual();
            if (actual != null)
                lblUsuarioActual.Text = $"Sesión: {actual.Nombre} {actual.Apellido} ({actual.Login}) — Rol: {actual.RolNombre}";

            AplicarPermisosMenu();
        }

        private void AplicarPermisosMenu()
        {
            Usuario_GV42 actual = SessionManager_GV42.Instancia.ObtenerUsuarioActual();
            if (actual == null || actual.Rol == null) return;

            if (string.Equals(actual.RolNombre, Rol_GV42.ROL_SUPER_ADMIN,
                              StringComparison.OrdinalIgnoreCase))
                return;

            var bllPermisos = new BLLPermisos_GV42();
            Rol_GV42 rolCompleto = bllPermisos.ObtenerArbolRol(actual.Rol.Id);
            if (rolCompleto == null) return;

            var dataKeys = rolCompleto.ObtenerPatentes()
                .Select(p => p.DataKey ?? string.Empty)
                .ToList();

            if (usuariosToolStripMenuItem != null)
                usuariosToolStripMenuItem.Visible = dataKeys.Any(k => k.StartsWith("Usuarios."));

            if (bitacoraToolStripMenuItem != null)
                bitacoraToolStripMenuItem.Visible = dataKeys.Any(k => k.StartsWith("Bitacora."));

            if (gestionDePermisosToolStripMenuItem != null)
                gestionDePermisosToolStripMenuItem.Visible = dataKeys.Any(k => k.StartsWith("Permisos."));
        }

        public void AbrirFormularioHijo(Form f)
        {
            if (_formularioActual != null && _formularioActual.GetType() == f.GetType())
            {
                _formularioActual.Close();
                _formularioActual = null;
                return;
            }

            if (_formularioActual != null)
            {
                _formularioActual.Close();
                _formularioActual = null;
            }

            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;

            pnlContenido.Controls.Clear();
            pnlContenido.Controls.Add(f);
            f.Show();
            _formularioActual = f;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FRMGestionUsuariosAdmin());
        }

        private void reLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FRMIniciarSesion());
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                IdiomaManager_GV42.T("menu.confirmarLogout"),
                IdiomaManager_GV42.T("menu.tituloLogout"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                BLLUsuario_GV42.CerrarSesión();
                FRMIniciarSesion frm = new FRMIniciarSesion();
                frm.Show();
                this.Close();
            }
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FRMCambiarContrasenia());
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FRMBitacoraDeEventos());
        }

        private void gestionDePermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioHijo(new FRMGestionPermisos());
        }

    }
}
