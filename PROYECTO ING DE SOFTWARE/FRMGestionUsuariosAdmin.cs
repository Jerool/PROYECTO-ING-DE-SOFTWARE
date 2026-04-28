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
using BLL;

namespace PROYECTO_ING_DE_SOFTWARE
{
    public partial class FRMGestionUsuariosAdmin : Form
    {
        private readonly BLLUsuario_GV42 _bll;
        private string _modo = "Consulta"; // Consulta | Crear | Modificar | Desbloquear | ActivarDesactivar
        private Usuario _usuarioSeleccionado = null;

        public FRMGestionUsuariosAdmin()
        {
            InitializeComponent();

            _bll = new BLLUsuario_GV42();

        }

        private void FRMPrincipalAdmin_Load(object sender, EventArgs e)
        {
            ModoConsulta();
            CargarGrilla(soloActivos: true);
            rbActivos.Checked = true;
        }

        private void CargarGrilla(bool soloActivos)
        {
            List<Usuario> lista = soloActivos ? _bll.ListarActivos() : _bll.ListarTodos();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = lista;

            // Pintar inactivos de rojo
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                if (!(bool)row.Cells["Activo"].Value)
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
            }
        }


        private void ModoConsulta()
        {
            _modo = "Consulta";
            lblMensaje.Text = "Modo Consulta";
            LimpiarCampos();
            HabilitarCampos(false);
            btnCrear.Enabled = true;
            btnModificar.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnActivarDesactivar.Enabled = true;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;
            rbActivos.Enabled = true;
            rbTodos.Enabled = true;
            dgvUsuarios.Enabled = true;
        }

        private void ModoOperacion(string modo)
        {
            _modo = modo;
            lblMensaje.Text = $"Modo {modo}";
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnActivarDesactivar.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            rbActivos.Enabled = false;
            rbTodos.Enabled = false;
        }



        private void HabilitarCampos(bool habilitar)
        {
            txtDni.Enabled = habilitar;
            txtApellido.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtRol.Enabled = habilitar;
            // UserName, Bloqueado, Activo nunca se editan manualmente
            txtUser.Enabled = false;
            txtBloqueado.Enabled = false;
            txtActivo.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtDni.Text = txtApellido.Text = txtNombre.Text =
            txtEmail.Text = txtRol.Text = txtUser.Text =
            txtBloqueado.Text = txtActivo.Text = "";
        }


        private void btnCrear_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            HabilitarCampos(true);
            dgvUsuarios.Enabled = false;
            ModoOperacion("Crear");
        }


        private void btnAplicar_Click(object sender, EventArgs e)
        {
            switch (_modo)
            {
                case "Crear":
                    Crear();
                    break;
                case "Modificar":
                    Modificar();
                    break;
                case "Desbloquear":
                    Desbloquear();
                    break;
                case "ActivarDesactivar":
                    ActivarDesactivar();
                    break;
            }
        }

        private void ActivarDesactivar()
        {
            bool nuevoEstado = !_usuarioSeleccionado.Activo;
            string accion = nuevoEstado ? "activado" : "desactivado";
            _bll.ActivarDesactivar(_usuarioSeleccionado.DNI, nuevoEstado);
            MessageBox.Show($"Usuario {_usuarioSeleccionado.Login} {accion}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);

        }

        private void Desbloquear()
        {
            _bll.Desbloquear(_usuarioSeleccionado.DNI);
            MessageBox.Show($"Usuario {_usuarioSeleccionado.Login} desbloqueado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);

        }

        private void Modificar()
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("El email no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _bll.ModificarEmail(_usuarioSeleccionado.DNI, txtEmail.Text.Trim());
            MessageBox.Show("Email modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void Crear()
        {
            if (string.IsNullOrEmpty(txtDni.Text) || string.IsNullOrEmpty(txtApellido.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtRol.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtRol.Text != "Admin" && txtRol.Text != "Usuario")
            {
                MessageBox.Show("El rol debe ser 'Admin' o 'Usuario'.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _bll.CrearUsuario(txtDni.Text.Trim(), txtApellido.Text.Trim(),
                                txtNombre.Text.Trim(), txtEmail.Text.Trim(), txtRol.Text.Trim());

            MessageBox.Show("Usuario creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;
            _usuarioSeleccionado = dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
            if (_usuarioSeleccionado == null) return;

            txtDni.Text = _usuarioSeleccionado.DNI;
            txtApellido.Text = _usuarioSeleccionado.Apellido;
            txtNombre.Text = _usuarioSeleccionado.Nombre;
            txtEmail.Text = _usuarioSeleccionado.Email;
            txtRol.Text = _usuarioSeleccionado.Rol;
            txtUser.Text = _usuarioSeleccionado.Login;
            txtBloqueado.Text = _usuarioSeleccionado.Bloqueo ? "Sí" : "No";
            txtActivo.Text = _usuarioSeleccionado.Activo ? "Sí" : "No";
        }

        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActivos.Checked) CargarGrilla(soloActivos: true);
        }

        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodos.Checked) CargarGrilla(soloActivos: false);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            SessionManager_GV42.CerrarSesion();
            this.Close();
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la grilla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HabilitarCampos(false);
            dgvUsuarios.Enabled = false;
            ModoOperacion("Desbloquear");
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la grilla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Solo Email es editable
            HabilitarCampos(false);
            txtEmail.Enabled = true;
            dgvUsuarios.Enabled = false;
            ModoOperacion("Modificar");
        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la grilla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HabilitarCampos(false);
            dgvUsuarios.Enabled = false;
            ModoOperacion("ActivarDesactivar");
        }


    }
}
