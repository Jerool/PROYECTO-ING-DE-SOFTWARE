using Servicios;                        // Para la entidad Usuario_GV42 y el SessionManager
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
            ConfigurarGrillaSoloLectura();
            CargarRoles();
            ModoConsulta();
            CargarGrilla(soloActivos: true);
            rbActivos.Checked = true;
        }

        // Deja la grilla en modo "consulta": no se puede editar ninguna celda,
        // no se pueden agregar/borrar filas, no se puede redimensionar nada.
        // Toda configuración va acá, en código, para que sobreviva a cualquier
        // regeneración del Designer.
        private void ConfigurarGrillaSoloLectura()
        {
            dgvUsuarios.ReadOnly = true;                  // ninguna celda editable
            dgvUsuarios.AllowUserToAddRows = false;       // sin fila vacía al final
            dgvUsuarios.AllowUserToDeleteRows = false;    // no se borran filas
            dgvUsuarios.AllowUserToResizeRows = false;    // alto fijo
            dgvUsuarios.AllowUserToResizeColumns = false; // ancho fijo
            dgvUsuarios.AllowUserToOrderColumns = false;  // sin drag de columnas
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.RowHeadersVisible = false;        // estética: ocultamos la columna de selección
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Headers tampoco se pueden estirar:
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }

        // Llena el ComboBox de Rol con las entidades Rol_GV42 que devuelve la BLL.
        // Usamos DisplayMember = "Nombre" para que el combo muestre el texto,
        // pero detrás cada item es un Rol_GV42 con su Id (la FK).
        private void CargarRoles()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = _bll.ListarRoles();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
        }

        private void CargarGrilla(bool soloActivos)
        {
            List<Usuario_GV42> lista = soloActivos ? _bll.ListarActivos() : _bll.ListarTodos();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = lista;

            // Ocultamos la columna Contrasena: al ser un hash SHA256 no aporta
            // información útil al administrador y ensucia visualmente la grilla.
            if (dgvUsuarios.Columns.Contains("Contrasena"))
                dgvUsuarios.Columns["Contrasena"].Visible = false;

            // Como Rol ahora es Rol_GV42 (entidad), por defecto el DataGridView
            // mostraría "Servicios.Rol_GV42" en esa columna. La ocultamos y
            // mostramos la propiedad RolNombre, que devuelve solo el texto.
            if (dgvUsuarios.Columns.Contains("Rol"))
                dgvUsuarios.Columns["Rol"].Visible = false;

            if (dgvUsuarios.Columns.Contains("RolNombre"))
                dgvUsuarios.Columns["RolNombre"].HeaderText = "Rol";

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
            txtEmail.Text = txtUser.Text =
            txtBloqueado.Text = txtActivo.Text = "";
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
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
            // El SelectedItem del combo es un Rol_GV42 (entidad), no un string.
            Rol_GV42 rol = comboBox1.SelectedItem as Rol_GV42;

            if (!Validaciones_GV42.EsEmailValido(email))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_EMAIL,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            if (rol == null)
            {
                MessageBox.Show("Seleccioná un rol.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (email != _usuarioSeleccionado.Email)
                _bll.ModificarEmail(_usuarioSeleccionado.DNI, email);

            // Comparamos por Id (la FK) en vez de por nombre.
            if (_usuarioSeleccionado.Rol == null || rol.Id != _usuarioSeleccionado.Rol.Id)
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
            Rol_GV42 rol = comboBox1.SelectedItem as Rol_GV42;

            if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) ||
                rol == null)
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Todas las validaciones de formato pasan por Validaciones_GV42 (regex centralizadas).
            if (!Validaciones_GV42.EsDniValido(dni))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_DNI,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (!Validaciones_GV42.EsApellidoValido(apellido))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_APELLIDO,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }
            if (!Validaciones_GV42.EsNombreValido(nombre))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_NOMBRE,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (!Validaciones_GV42.EsEmailValido(email))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_EMAIL,
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Ya no validamos el rol contra strings hardcodeados: si vino del
            // combo, por construcción es uno de los roles que existen en la base.

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

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            _usuarioSeleccionado = dgvUsuarios.CurrentRow.DataBoundItem as Usuario_GV42;
            if (_usuarioSeleccionado == null) return;

            txtDni.Text = _usuarioSeleccionado.DNI;
            txtApellido.Text = _usuarioSeleccionado.Apellido;
            txtNombre.Text = _usuarioSeleccionado.Nombre;
            txtEmail.Text = _usuarioSeleccionado.Email;
            // Posicionamos el combo en el rol del usuario buscando por Id.
            SeleccionarRolEnCombo(_usuarioSeleccionado.Rol);
            txtUser.Text = _usuarioSeleccionado.Login;
            txtBloqueado.Text = _usuarioSeleccionado.Bloqueo ? "Sí" : "No";
            txtActivo.Text = _usuarioSeleccionado.Activo ? "Sí" : "No";
        }

        // Busca dentro del combo el Rol con el mismo Id que el del usuario y lo selecciona.
        // Es más robusto que comparar por nombre porque la FK es el Id.
        private void SeleccionarRolEnCombo(Rol_GV42 rol)
        {
            if (rol == null) { comboBox1.SelectedIndex = -1; return; }

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                Rol_GV42 r = comboBox1.Items[i] as Rol_GV42;
                if (r != null && r.Id == rol.Id)
                {
                    comboBox1.SelectedIndex = i;
                    return;
                }
            }
            comboBox1.SelectedIndex = -1;
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
