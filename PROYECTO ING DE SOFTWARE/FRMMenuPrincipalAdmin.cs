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
        public Form formularioactual = null;
        public FRMMenuPrincipalAdmin()
        {
            InitializeComponent();
        }

        public void AbrirFormularioHijo(Form f)
        {
            if (formularioactual == null)
            {
                f.MdiParent = this;
                f.Show();
                f.Enabled = true;
                formularioactual = f;
                f.Dock = DockStyle.Fill;
            }
            else if (formularioactual.GetType() == f.GetType())
            {
                formularioactual.Close();
                formularioactual = null;
            }
            else
            {
                formularioactual.Close();
                formularioactual = null;
                AbrirFormularioHijo(f);
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMGestionUsuariosAdmin frm = new FRMGestionUsuariosAdmin();
            AbrirFormularioHijo(frm);
        }

        private void reLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMIniciarSesion frm = new FRMIniciarSesion();
            //frm.Show();
            //this.Close();
            AbrirFormularioHijo(frm);
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                BLLUsuario_GV42.CerrarSesión();  
                FRMIniciarSesion frm = new FRMIniciarSesion();
                frm.Show();
                this.Close();
            }
            else { return;}
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FRMCambiarContrasenia frm = new FRMCambiarContrasenia();
            AbrirFormularioHijo(frm);
        }
    }
}
