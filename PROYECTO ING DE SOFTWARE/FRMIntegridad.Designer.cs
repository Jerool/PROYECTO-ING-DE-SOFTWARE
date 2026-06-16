namespace PROYECTO_ING_DE_SOFTWARE
{
    partial class FRMIntegridad
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Label lblTablas;
        private System.Windows.Forms.ListBox lstTablas;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel pnlIcono;
        private System.Windows.Forms.Label lblIcono;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.lblTablas = new System.Windows.Forms.Label();
            this.lstTablas = new System.Windows.Forms.ListBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.pnlIcono = new System.Windows.Forms.Panel();
            this.lblIcono = new System.Windows.Forms.Label();
            this.pnlIcono.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlIcono
            //
            this.pnlIcono.Controls.Add(this.lblIcono);
            this.pnlIcono.Location = new System.Drawing.Point(20, 20);
            this.pnlIcono.Name = "pnlIcono";
            this.pnlIcono.Size = new System.Drawing.Size(60, 60);
            this.pnlIcono.TabIndex = 0;
            //
            // lblIcono
            //
            this.lblIcono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIcono.Name = "lblIcono";
            this.lblIcono.Text = "!";
            this.lblIcono.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(95, 25);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Integridad comprometida";
            //
            // lblMensaje
            //
            this.lblMensaje.Location = new System.Drawing.Point(95, 55);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(450, 40);
            this.lblMensaje.Text = "Se detectaron alteraciones en la base de datos realizadas desde fuera de la aplicación.";
            //
            // lblTablas
            //
            this.lblTablas.AutoSize = true;
            this.lblTablas.Location = new System.Drawing.Point(20, 110);
            this.lblTablas.Name = "lblTablas";
            this.lblTablas.Text = "Tablas afectadas:";
            //
            // lstTablas
            //
            this.lstTablas.Location = new System.Drawing.Point(20, 130);
            this.lstTablas.Name = "lstTablas";
            this.lstTablas.Size = new System.Drawing.Size(530, 130);
            this.lstTablas.TabIndex = 1;
            //
            // btnRestore
            //
            this.btnRestore.Location = new System.Drawing.Point(20, 280);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(170, 50);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "Restore (recalcular)";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            //
            // btnBackup
            //
            this.btnBackup.Location = new System.Drawing.Point(200, 280);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(170, 50);
            this.btnBackup.TabIndex = 3;
            this.btnBackup.Text = "Backup (restaurar)";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(380, 280);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(170, 50);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // FRMIntegridad
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 360);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.lstTablas);
            this.Controls.Add(this.lblTablas);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.pnlIcono);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRMIntegridad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Control de Integridad";
            this.pnlIcono.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
