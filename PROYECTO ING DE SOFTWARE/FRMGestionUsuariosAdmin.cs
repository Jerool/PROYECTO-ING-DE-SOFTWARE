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

    public partial class FRMGestionUsuariosAdmin : Form, IObservadorIdioma_GV42
    {
        private readonly BLLUsuario_GV42 _bll;

        private string _modo = "Consulta";

        private Usuario_GV42 _usuarioSeleccionado = null;

        public FRMGestionUsuariosAdmin()
        {
            InitializeComponent();
            _bll = new BLLUsuario_GV42();

            IdiomaManager_GV42.Instancia.Suscribir(this);
            this.FormClosed += (s, e) => IdiomaManager_GV42.Instancia.Desuscribir(this);

            ActualizarIdioma();
        }

        // Refresca TODOS los textos del form cuando cambia el idioma.
        public void ActualizarIdioma()
        {
            this.Text = IdiomaManager_GV42.T("usuarios.titulo");

            if (label1 != null) label1.Text = IdiomaManager_GV42.T("usuarios.dni");
            if (label2 != null) label2.Text = IdiomaManager_GV42.T("usuarios.apellido");
            if (label3 != null) label3.Text = IdiomaManager_GV42.T("usuarios.nombre");
            if (label4 != null) label4.Text = IdiomaManager_GV42.T("usuarios.email");
            if (label5 != null) label5.Text = IdiomaManager_GV42.T("usuarios.rol");
            if (label6 != null) label6.Text = IdiomaManager_GV42.T("usuarios.userName");
            if (label7 != null) label7.Text = IdiomaManager_GV42.T("usuarios.bloqueado");
            if (label8 != null) label8.Text = IdiomaManager_GV42.T("usuarios.activo");

            if (btnCrear != null) btnCrear.Text = IdiomaManager_GV42.T("usuarios.crear");
            if (btnModificar != null) btnModificar.Text = IdiomaManager_GV42.T("usuarios.modificar");
            if (btnDesbloquear != null) btnDesbloquear.Text = IdiomaManager_GV42.T("usuarios.desbloquear");
            if (btnActivarDesactivar != null) btnActivarDesactivar.Text = IdiomaManager_GV42.T("usuarios.activarDesactivar");
            if (btnAplicar != null) btnAplicar.Text = IdiomaManager_GV42.T("usuarios.aplicar");
            if (btnCancelar != null) btnCancelar.Text = IdiomaManager_GV42.T("usuarios.cancelar");
            if (btnSalir != null) btnSalir.Text = IdiomaManager_GV42.T("usuarios.salir");

            if (rbActivos != null) rbActivos.Text = IdiomaManager_GV42.T("usuarios.activos");
            if (rbTodos != null) rbTodos.Text = IdiomaManager_GV42.T("usuarios.todos");

            // El label de modo se setea según el modo actual.
            if (lblMensaje != null) lblMensaje.Text = TraducirModo(_modo);

            // Refrescamos también los headers de la grilla, si ya está cargada.
            if (dgvUsuarios != null && dgvUsuarios.Columns.Contains("RolNombre"))
                dgvUsuarios.Columns["RolNombre"].HeaderText = IdiomaManager_GV42.T("usuarios.rol");
        }

        // Helper para traducir el modo actual a texto amigable en el idioma activo.
        private string TraducirModo(string modo)
        {
            string etiquetaModo = IdiomaManager_GV42.T("usuarios.modo");
            return etiquetaModo + " " + modo;
        }

        private void FRMPrincipalAdmin_Load(object sender, EventArgs e)
        {
            ConfigurarGrillaSoloLectura();
            CargarRoles();
            ModoConsulta();
            CargarGrilla(soloActivos: true);
            rbActivos.Checked = true;
        }

        private void ConfigurarGrillaSoloLectura()
        {
            dgvUsuarios.ReadOnly = true;                  
            dgvUsuarios.AllowUserToAddRows = false;       
            dgvUsuarios.AllowUserToDeleteRows = false;    
            dgvUsuarios.AllowUserToResizeRows = false;    
            dgvUsuarios.AllowUserToResizeColumns = false; 
            dgvUsuarios.AllowUserToOrderColumns = false;  
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.RowHeadersVisible = false;      
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }

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

   
            if (dgvUsuarios.Columns.Contains("Contrasena"))
                dgvUsuarios.Columns["Contrasena"].Visible = false;

            if (dgvUsuarios.Columns.Contains("IntentosFallidos"))
                dgvUsuarios.Columns["IntentosFallidos"].Visible = false;

            if (dgvUsuarios.Columns.Contains("UltimoIntentoFallido"))
                dgvUsuarios.Columns["UltimoIntentoFallido"].Visible = false;

  
            if (dgvUsuarios.Columns.Contains("Rol"))
                dgvUsuarios.Columns["Rol"].Visible = false;

            if (dgvUsuarios.Columns.Contains("RolNombre"))
                dgvUsuarios.Columns["RolNombre"].HeaderText = "Rol";

            if (dgvUsuarios.Columns.Contains("DebeCambiarContrasena"))
                dgvUsuarios.Columns["DebeCambiarContrasena"].Visible = false;

            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                if (!(bool)row.Cells["Activo"].Value)
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
            }
        }

        private void ModoConsulta()
        {
            _modo = "Consulta";
            lblMensaje.Text = TraducirModo(_modo);
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
            lblMensaje.Text = TraducirModo(modo);
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

            Usuario_GV42 actual = SessionManager_GV42.Instancia.ObtenerUsuarioActual();
            if (actual != null && _usuarioSeleccionado.Login == actual.Login)
            {
                MessageBox.Show("No podés activar/desactivar tu propio usuario. Pedile a otro administrador que lo haga.","Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ModoConsulta();
                CargarGrilla(rbActivos.Checked);
                return;
            }

            string accion = nuevoEstado ? "activado" : "desactivado";
            _bll.ActivarDesactivar(_usuarioSeleccionado.DNI, nuevoEstado);
            MessageBox.Show($"Usuario {_usuarioSeleccionado.Login} {accion}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void Desbloquear()
        {
            if (_usuarioSeleccionado.Bloqueo == false)
            {
                MessageBox.Show($"Usuario ya desbloqueado", "Error", MessageBoxButtons.OK);
                return;
            }

            
            _bll.Desbloquear(_usuarioSeleccionado.DNI, _usuarioSeleccionado.Login);
            MessageBox.Show($"Usuario {_usuarioSeleccionado.Login} desbloqueado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModoConsulta();
            CargarGrilla(rbActivos.Checked);
        }

        private void Modificar()
        {
            string email = txtEmail.Text.Trim();
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

            if (!Validaciones_GV42.EsDniValido(dni))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_DNI,"Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (!Validaciones_GV42.EsApellidoValido(apellido))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_APELLIDO,"Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return;
            }
            if (!Validaciones_GV42.EsNombreValido(nombre))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_NOMBRE,"Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (!Validaciones_GV42.EsEmailValido(email))
            {
                MessageBox.Show(Validaciones_GV42.MENSAJE_EMAIL,"Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }


            if (_bll.ExisteDNI(dni))
            {
                MessageBox.Show($"Ya existe un usuario con el DNI '{dni}'.","Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("No se pudo crear el usuario.\n\nDetalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SeleccionarRolEnCombo(_usuarioSeleccionado.Rol);
            txtUser.Text = _usuarioSeleccionado.Login;
            txtBloqueado.Text = _usuarioSeleccionado.Bloqueo ? "Sí" : "No";
            txtActivo.Text = _usuarioSeleccionado.Activo ? "Sí" : "No";
        }

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
