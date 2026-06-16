using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ING_DE_SOFTWARE
{
    public partial class FRMIntegridad : Form, IObservadorIdioma_GV42
    {
        private readonly BLLIntegridad_GV42 _bll;
        private readonly ResultadoIntegridad _resultado;

        public bool SeRestauroBackup { get; private set; }
        public bool SeRecalcularon { get; private set; }

        public FRMIntegridad(ResultadoIntegridad resultado)
        {
            InitializeComponent();
            _bll = new BLLIntegridad_GV42();
            _resultado = resultado;

            IdiomaManager_GV42.Instancia.Suscribir(this);
            this.FormClosed += (s, e) => IdiomaManager_GV42.Instancia.Desuscribir(this);

            AplicarEstilos();
            CargarTablas();
            ActualizarIdioma();
        }

        private void AplicarEstilos()
        {
            Color azulOscuro    = Color.FromArgb(13, 71, 161);
            Color azulClaro     = Color.FromArgb(227, 242, 253);
            Color rojo          = Color.FromArgb(198, 40, 40);
            Color blanco        = Color.White;
            Font  fuenteBase    = new Font("Segoe UI", 10F);
            Font  fuenteTitulo  = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            Font  fuenteIcono   = new Font("Segoe UI Semibold", 32F, FontStyle.Bold);
            Font  fuenteBtn     = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

            this.BackColor = azulClaro;
            this.Font = fuenteBase;

            pnlIcono.BackColor = rojo;
            lblIcono.ForeColor = blanco;
            lblIcono.Font = fuenteIcono;

            lblTitulo.ForeColor = rojo;
            lblTitulo.Font = fuenteTitulo;
            lblMensaje.ForeColor = azulOscuro;
            lblMensaje.Font = fuenteBase;
            lblTablas.ForeColor = azulOscuro;
            lblTablas.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            lstTablas.BackColor = blanco;
            lstTablas.BorderStyle = BorderStyle.FixedSingle;
            lstTablas.Font = new Font("Consolas", 10F);
            lstTablas.ForeColor = azulOscuro;

            EstilarBotonPrimario(btnRestore, azulOscuro, blanco, fuenteBtn);
            EstilarBotonPrimario(btnBackup,  azulOscuro, blanco, fuenteBtn);
            EstilarBotonSecundario(btnCancelar, azulOscuro, blanco, fuenteBtn);
        }

        private static void EstilarBotonPrimario(Button btn, Color fondo, Color texto, Font fuente)
        {
            btn.BackColor = fondo;
            btn.ForeColor = texto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = fuente;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private static void EstilarBotonSecundario(Button btn, Color borde, Color blanco, Font fuente)
        {
            btn.BackColor = blanco;
            btn.ForeColor = borde;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = borde;
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = fuente;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private void CargarTablas()
        {
            lstTablas.Items.Clear();
            foreach (var t in _resultado.TablasComprometidas)
                lstTablas.Items.Add("• " + t);
        }

        public void ActualizarIdioma()
        {
            this.Text = IdiomaManager_GV42.T("integridad.titulo");
            if (lblTitulo != null) lblTitulo.Text = IdiomaManager_GV42.T("integridad.tituloAlerta");
            if (lblMensaje != null) lblMensaje.Text = IdiomaManager_GV42.T("integridad.mensaje");
            if (lblTablas != null) lblTablas.Text = IdiomaManager_GV42.T("integridad.tablasAfectadas");
            if (btnRestore != null) btnRestore.Text = IdiomaManager_GV42.T("integridad.botonRestore");
            if (btnBackup != null) btnBackup.Text = IdiomaManager_GV42.T("integridad.botonBackup");
            if (btnCancelar != null) btnCancelar.Text = IdiomaManager_GV42.T("general.cancelar");
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
                IdiomaManager_GV42.T("integridad.confirmRestore"),
                IdiomaManager_GV42.T("integridad.titulo"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                _bll.Recalcular();
                SeRecalcularon = true;
                MessageBox.Show(IdiomaManager_GV42.T("integridad.restoreOk"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, IdiomaManager_GV42.T("general.error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string ultimo = _bll.ObtenerUltimoBackup();
            if (string.IsNullOrEmpty(ultimo))
            {
                MessageBox.Show(IdiomaManager_GV42.T("integridad.sinBackups"),
                                IdiomaManager_GV42.T("general.advertencia"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mensaje = IdiomaManager_GV42.T("integridad.confirmBackup") +
                             "\n\n" + IdiomaManager_GV42.T("integridad.archivoARestaurar") + "\n" + ultimo;

            DialogResult r = MessageBox.Show(mensaje,
                                             IdiomaManager_GV42.T("integridad.titulo"),
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r != DialogResult.Yes) return;

            try
            {
                _bll.RestaurarUltimoBackup();
                SeRestauroBackup = true;
                MessageBox.Show(IdiomaManager_GV42.T("integridad.backupOk"),
                                IdiomaManager_GV42.T("general.exito"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(IdiomaManager_GV42.T("integridad.backupError") + "\n\n" + ex.Message,
                                IdiomaManager_GV42.T("general.error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
