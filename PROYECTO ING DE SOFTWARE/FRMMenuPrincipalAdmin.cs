using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_ING_DE_SOFTWARE
{


    public partial class FRMMenuPrincipalAdmin : Form
    {

        private Form _formularioActual = null;

        public FRMMenuPrincipalAdmin()
        {
            InitializeComponent();
        }

        private void FRMMenuPrincipalAdmin_Load(object sender, EventArgs e)
        {
            Usuario_GV42 actual = SessionManager_GV42.Instancia.ObtenerUsuarioActual();
            if (actual != null)
                lblUsuarioActual.Text = $"Sesión: {actual.Nombre} {actual.Apellido} ({actual.Login}) — Rol: {actual.RolNombre}";
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
                "¿Está seguro que desea cerrar sesión?",
                "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
    }
}
