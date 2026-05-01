using Servicios;                        // Para la entidad Usuario_GV42 y el SessionManager
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;                  
using System.Text;
using System.Text.RegularExpressions;   
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;                              

namespace PROYECTO_ING_DE_SOFTWARE
{

    public partial class FRMGestionUsuariosAdmin : Form
    {
        private readonly BLLUsuario_GV42 _bll;

        // Modo actual del form: define qué hacen los botones y qué campos están habilitados.
        private string _modo = "Consulta";

        private Usuario_GV42 _usuarioSeleccionado = null;

        public FRMGestionUsuariosAdmin()
        {
            InitializeComponent();
            _bll = new BLLUsuario_GV42();
        }

        // Carga el combo de roles, pone el form en modo Consulta y trae la grilla.
        private void FRMPrincipalAdmin_Load(object sender, EventArgs e)
        {
            CargarRoles();
            ModoConsulta();
            CargarGrilla(soloActivos: true);
            rbActivos.Checked = true;
        }

        // Llena el ComboBox de Rol con los nombres que hay en la tabla Roles de la base.
        private void CargarRoles()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = _bll.ListarRoles();
        }

        private void CargarGrilla(bool soloActivos)
        {
            List<Usuario_GV42> lista = soloActivos ? _bll.ListarActivos() : _bll.ListarTodos();
            dgvUsuarios.DataSource = null;       
            dgvUsuarios.DataSource = lista;

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
            comboBox1.Enabled = habilitar;
            txtUser.Enabled = false;
            txtBloqueado.Enabled = false;
            txtActivo.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtDni.Text = txtApellido.Text = txtNombre.Text =
            txtEmail.Text = comboBox1.Text = txtUser.Text =
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

        // Desbloquea el usuario seleccionado y le resetea la contraseña al patrón por defecto.
        private void Desbloquear()
        {
            _bll.Desbloquear(_usuarioSeleccionado.DNI, _usuarioSeleccionado.Login);
            MessageBox.Show($"Usuario {_usuarioSeleccionado.Login} desbloqueado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        // Modifica el email y/o el rol del usuario seleccionado.
        private void Modificar()
        {
            string email = txtEmail.Text.Trim();
            string rol = comboBox1.SelectedItem?.ToString() ?? comboBox1.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("El email no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(rol))
            {
                MessageBox.Show("Seleccioná un rol.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (email != _usuarioSeleccionado.Email)
                _bll.ModificarEmail(_usuarioSeleccionado.DNI, email);

            if (rol != _usuarioSeleccionado.Rol)
                _bll.ModificarRol(_usuarioSeleccionado.DNI, rol);

            MessageBox.Show("Usuario modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void Crear()
        {
            string dni = txtDni.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string email = txtEmail.Text.Trim();
            string rol = comboBox1.Text.Trim();

            if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(rol))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(dni, @"^\d{7,8}$"))
            {
                MessageBox.Show("El DNI debe contener solo números y tener entre 7 y 8 dígitos.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                MessageBox.Show("El apellido solo puede contener letras.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }
            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                MessageBox.Show("El nombre solo puede contener letras.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (!EsEmailValido(email))
            {
                MessageBox.Show("El formato del email no es válido.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (rol != "Admin" && rol != "Usuario")
            {
                MessageBox.Show("El rol seleccionado no es válido.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_bll.ExisteDNI(dni))
            {
                MessageBox.Show($"Ya existe un usuario con el DNI '{dni}'.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            try
            {
                _bll.CrearUsuario(dni, apellido, nombre, email, rol);

                MessageBox.Show("Usuario creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ModoConsulta();
                CargarGrilla(rbActivos.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo crear el usuario.\n\nDetalle: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            _usuarioSeleccionado = dgvUsuarios.CurrentRow.DataBoundItem as Usuario_GV42;
            if (_usuarioSeleccionado == null) return;

            txtDni.Text = _usuarioSeleccionado.DNI;
            txtApellido.Text = _usuarioSeleccionado.Apellido;
            txtNombre.Text = _usuarioSeleccionado.Nombre;
            txtEmail.Text = _usuarioSeleccionado.Email;
            comboBox1.Text = _usuarioSeleccionado.Rol;
            txtUser.Text = _usuarioSeleccionado.Login;
            txtBloqueado.Text = _usuarioSeleccionado.Bloqueo ? "Sí" : "No";
            txtActivo.Text = _usuarioSeleccionado.Activo ? "Sí" : "No";
        }

        // Radio button "Solo Activos": refresca la grilla con el filtro Activo = 1.
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
 
            HabilitarCampos(false);
            txtEmail.Enabled = true;
            comboBox1.Enabled = true;
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
