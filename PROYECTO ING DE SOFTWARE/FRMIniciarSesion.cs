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
using static BLL.BLLUsuario_GV42;

namespace PROYECTO_ING_DE_SOFTWARE
{
    public partial class FRMIniciarSesion : Form
    {
        private readonly BLLUsuario_GV42 _bllUsuario;
        public FRMIniciarSesion()
        {
            InitializeComponent();
            _bllUsuario = new BLLUsuario_GV42();

            string hash = Encriptador_GV42.Instancia.EncriptarContrasena("lautaro212");
            Console.WriteLine (hash);
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

            ResultadoLogin resultado = _bllUsuario.IntentarLogin(login, contrasena);

            switch (resultado)
            {
                case ResultadoLogin.Exitoso:
                    AbrirFormularioSegunRol();
                    break;
                case ResultadoLogin.UsuarioBloqueado:
                case ResultadoLogin.BloqueadoPorIntentos:
                    MessageBox.Show("Tu usuario está bloqueado. Contactá al administrador.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultadoLogin.UsuarioInactivo:
                    MessageBox.Show("Tu usuario está inactivo. Contactá al administrador.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultadoLogin.ContrasenaIncorrecta:
                    MessageBox.Show("Contraseña incorrecta. Verificá tus credenciales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoLogin.UsuarioInexistente:
                    MessageBox.Show("El usuario no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoLogin.SesionActiva:
                    MessageBox.Show("Ya hay una sesión activa.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void AbrirFormularioSegunRol()
        {
            string rol = SessionManager_GV42.Instancia.ObtenerUsuarioActual().Rol;

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

