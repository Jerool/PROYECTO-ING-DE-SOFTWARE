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
    public partial class FRMIniciarSesion : Form
    {
        private readonly BLLUsuario_GV42 _bllUsuario;
        public FRMIniciarSesion()
        {
            InitializeComponent();
            _bllUsuario = new BLLUsuario_GV42();
        }
        
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string login = txtLogIn.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool exitoso = _bllUsuario.IntentarLogin(login, contrasena);
            if (exitoso)
            {
                AbrirFormularioSegunRol();
            }
            else
            {
                MessageBox.Show("Login fallido. Verificá tus credenciales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirFormularioSegunRol()
        {
            string rol = SessionManager_GV42.ObtenerUsuarioActual().Rol;

            Form formulario;

            if (rol == "Admin")
                formulario = new FRMMenuPrincipalAdmin();
            else
                formulario = new FRMMenuPrincipalAdmin();

            formulario.Show();
            this.Hide();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
 }

