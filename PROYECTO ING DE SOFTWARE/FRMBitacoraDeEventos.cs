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
    public partial class FRMBitacoraDeEventos : Form
    {
        private readonly BLLBitacora_GV42 _bllBitacora;

        private readonly BLLUsuario_GV42 _bllUsuario;

        private const string SIN_FILTRO = "(Todos)";

        public FRMBitacoraDeEventos()
        {
            InitializeComponent();
            _bllBitacora = BLLBitacora_GV42.Instancia;
            _bllUsuario = new BLLUsuario_GV42();
        }


        private void FRMBitacoraDeEventos_Load(object sender, EventArgs e)
        {
            ConfigurarGrillaSoloLectura();
            CargarCombos();
            EstablecerFechasPorDefecto();
            CargarGrillaPorDefecto();
        }

        private void ConfigurarGrillaSoloLectura()
        {
            dgvBitacora.ReadOnly = true;
            dgvBitacora.AllowUserToAddRows = false;
            dgvBitacora.AllowUserToDeleteRows = false;
            dgvBitacora.AllowUserToResizeRows = false;
            dgvBitacora.AllowUserToResizeColumns = false;
            dgvBitacora.AllowUserToOrderColumns = false;
            dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBitacora.MultiSelect = false;
            dgvBitacora.RowHeadersVisible = false;
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBitacora.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }

        private void EstablecerFechasPorDefecto()
        {

            DateTime hoy = DateTime.Now.Date;
            DateTime hace3Dias = hoy.AddDays(-3);

            dtpFechaInicio.MinDate = hace3Dias;
            dtpFechaInicio.MaxDate = hoy;
            dtpFechaInicio.Value = hace3Dias;
            dtpFechaFin.MinDate = hace3Dias;
            dtpFechaFin.MaxDate = hoy;
            dtpFechaFin.Value = hoy;
        }

        private void CargarCombos()
        {
            List<string> modulos = new List<string> { SIN_FILTRO };
            modulos.AddRange(_bllBitacora.ListarModulos());
            cboModulo.DataSource = modulos;

            List<string> eventos = new List<string> { SIN_FILTRO };
            eventos.AddRange(_bllBitacora.ListarTiposEvento());
            cboEvento.DataSource = eventos;

            List<string> criticidades = new List<string> { SIN_FILTRO };
            criticidades.AddRange(_bllBitacora.ListarCriticidades());
            cboCriticidad.DataSource = criticidades;
        }

        private void CargarGrillaPorDefecto()
        {
            DateTime fechaFinReal = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);
            CargarGrilla(_bllBitacora.Filtrar(
                login: null,
                modulo: null,
                tipoEvento: null,
                criticidad: null,
                fechaInicio: dtpFechaInicio.Value.Date,
                fechaFin: fechaFinReal));
        }

        private void CargarGrilla(List<Bitacora_GV42> registros)
        {
            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = registros;
            ConfigurarColumnas();
            if (dgvBitacora.Rows.Count > 0)
            {
                dgvBitacora.ClearSelection();
                dgvBitacora.Rows[0].Selected = true;
                dgvBitacora.CurrentCell = dgvBitacora.Rows[0].Cells[0];
            }
            else
            {
                txtNombreUsuario.Text = "";
                txtApellidoUsuario.Text = "";
            }
        }
        private void ConfigurarColumnas()
        {
            if (dgvBitacora.Columns.Count == 0) return;
            string[] aOcultar = { "Modulo", "TipoEvento" };
            foreach (string c in aOcultar)
            if (dgvBitacora.Columns.Contains(c)) dgvBitacora.Columns[c].Visible = false;

            if (dgvBitacora.Columns.Contains("Login")) dgvBitacora.Columns["Login"].HeaderText = "Usuario";
            if (dgvBitacora.Columns.Contains("ModuloNombre")) dgvBitacora.Columns["ModuloNombre"].HeaderText = "Módulo";
            if (dgvBitacora.Columns.Contains("TipoEventoNombre")) dgvBitacora.Columns["TipoEventoNombre"].HeaderText = "Tipo evento";
            if (dgvBitacora.Columns.Contains("Detalle")) dgvBitacora.Columns["Detalle"].HeaderText = "Detalle";
            if (dgvBitacora.Columns.Contains("Evento")) dgvBitacora.Columns["Evento"].Visible = false;
            if (dgvBitacora.Columns.Contains("Criticidad")) dgvBitacora.Columns["Criticidad"].HeaderText = "Criticidad";
            if (dgvBitacora.Columns.Contains("FechaHora"))
            {
                dgvBitacora.Columns["FechaHora"].HeaderText = "Fecha y hora";
                dgvBitacora.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (dtpFechaFin.Value < dtpFechaInicio.Value)
            {
                MessageBox.Show("La fecha fin no puede ser anterior a la fecha inicio.","Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string login = txtLogin.Text.Trim();
            string modulo = ValorCombo(cboModulo);
            string evento = ValorCombo(cboEvento);
            string criticidad = ValorCombo(cboCriticidad);
            DateTime fechaFinReal = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);
            List<Bitacora_GV42> resultados = _bllBitacora.Filtrar(
            login, modulo, evento, criticidad,
            dtpFechaInicio.Value.Date, fechaFinReal);

            CargarGrilla(resultados);
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtLogin.Text = "";
            cboModulo.SelectedIndex = 0;
            cboEvento.SelectedIndex = 0;
            cboCriticidad.SelectedIndex = 0;
            EstablecerFechasPorDefecto();
            CargarGrillaPorDefecto();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvBitacora.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros para exportar.",
                                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Archivo PDF (*.pdf)|*.pdf";
                sfd.Title = "Guardar bitácora como PDF";
                sfd.FileName = $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string[] headers = { "Usuario", "Módulo", "Tipo evento", "Detalle", "Criticidad", "Fecha y hora" };

                    float[] proporciones = { 0.12f, 0.12f, 0.22f, 0.22f, 0.10f, 0.22f };

                    List<string[]> filas = new List<string[]>();
                    foreach (DataGridViewRow row in dgvBitacora.Rows)
                    {
                        if (row.IsNewRow) continue;
                        filas.Add(new string[]
                        {
                            ValorCelda(row, "Login"),
                            ValorCelda(row, "ModuloNombre"),
                            ValorCelda(row, "TipoEventoNombre"),
                            ValorCelda(row, "Detalle"),
                            ValorCelda(row, "Criticidad"),
                            ValorCelda(row, "FechaHora")
                        });
                    }

                    string subtitulo = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss} - " +
                                       $"Total de registros: {filas.Count}";

                    GeneradorPdf_GV42 generador = new GeneradorPdf_GV42();
                    generador.Generar(sfd.FileName, "Bitacora de Eventos", subtitulo,
                                      headers, proporciones, filas);

                    DialogResult abrir = MessageBox.Show($"PDF generado correctamente en:\n{sfd.FileName}\n\n¿Querés abrirlo ahora?","Éxito", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (abrir == DialogResult.Yes)
                        System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo generar el PDF.\n\nDetalle: " + ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ValorCelda(DataGridViewRow fila, string columna)
        {
            if (!dgvBitacora.Columns.Contains(columna)) return "";
            object val = fila.Cells[columna].Value;
            if (val == null) return "";
            if (val is DateTime dt) return dt.ToString("dd/MM/yyyy HH:mm:ss");
            return val.ToString();
        }

        private void dgvBitacora_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBitacora.CurrentRow == null) return;
            Bitacora_GV42 registro = dgvBitacora.CurrentRow.DataBoundItem as Bitacora_GV42;
            if (registro == null) return;

            try
            {
                Usuario_GV42 usuario = _bllUsuario.BuscarPorLogin(registro.Login);
                if (usuario != null)
                {
                    txtNombreUsuario.Text = usuario.Nombre;
                    txtApellidoUsuario.Text = usuario.Apellido;
                }
                else
                {
                    txtNombreUsuario.Text = "(Usuario inexistente)";
                    txtApellidoUsuario.Text = "";
                }
            }
            catch
            {
                txtNombreUsuario.Text = "";
                txtApellidoUsuario.Text = "";
            }
        }

        private string ValorCombo(ComboBox cbo)
        {
            string seleccion = cbo.SelectedItem?.ToString();
            return seleccion == SIN_FILTRO ? null : seleccion;
        }
    }
}
