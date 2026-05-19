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
using static BLL.BLLUsuario_GV42;        // Para acceder al enum ResultadoCambioContrasena

namespace PROYECTO_ING_DE_SOFTWARE
{

    public partial class FRMCambiarContrasenia : Form
    {
        private readonly BLLUsuario_GV42 _bll;

        public FRMCambiarContrasenia()
        {
            InitializeComponent();

 
            txtUsuario.Text = SessionManager_GV42.Instancia.ObtenerUsuarioActual().Login;
            _bll = new BLLUsuario_GV42();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string login = txtUsuario.Text.Trim();
            string contrasenaActual = txtContrasenia.Text.Trim();
            string nuevaContrasena = txtNuevaconstrasenia.Text.Trim();
            string confirmar = txtConfirmarContrasenia.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(contrasenaActual) ||
                string.IsNullOrEmpty(nuevaContrasena) || string.IsNullOrEmpty(confirmar))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validamos que la NUEVA contraseña cumpla los requisitos mínimos de seguridad
            // (al menos 6 caracteres, una letra y un número). La actual no la validamos
            // porque podría haber sido seteada bajo reglas viejas.
            if (!Validaciones_GV42.EsContrasenaValida(nuevaContrasena))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_CONTRASENA,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevaconstrasenia.Focus();
                return;
            }

            ResultadoCambioContrasena resultado =
                _bll.CambiarContrasena(login, contrasenaActual, nuevaContrasena, confirmar);

            switch (resultado)
            {
                case ResultadoCambioContrasena.Exitoso:
                    MessageBox.Show("Contraseña cambiada correctamente.", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case ResultadoCambioContrasena.ContrasenaActualIncorrecta:
                    MessageBox.Show("La contraseña actual es incorrecta.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoCambioContrasena.ContrasenasNoCoinciden:
                    MessageBox.Show("La nueva contraseña y la confirmación no coinciden.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ResultadoCambioContrasena.UsuarioInexistente:
                    MessageBox.Show("El usuario no existe.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
