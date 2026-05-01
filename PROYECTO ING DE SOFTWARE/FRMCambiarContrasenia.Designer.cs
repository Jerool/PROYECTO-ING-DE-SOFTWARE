namespace PROYECTO_ING_DE_SOFTWARE
{
    partial class FRMCambiarContrasenia
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContrasenia = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNuevaconstrasenia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConfirmarContrasenia = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlCard — panel que envuelve todo el formulario y centra los controles
            //
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.btnAceptar);
            this.pnlCard.Controls.Add(this.txtConfirmarContrasenia);
            this.pnlCard.Controls.Add(this.label4);
            this.pnlCard.Controls.Add(this.txtNuevaconstrasenia);
            this.pnlCard.Controls.Add(this.label3);
            this.pnlCard.Controls.Add(this.txtContrasenia);
            this.pnlCard.Controls.Add(this.label2);
            this.pnlCard.Controls.Add(this.txtUsuario);
            this.pnlCard.Controls.Add(this.label1);
            this.pnlCard.Controls.Add(this.lblTitulo);
            this.pnlCard.Location = new System.Drawing.Point(150, 30);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(500, 390);
            this.pnlCard.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.lblTitulo.Location = new System.Drawing.Point(125, 22);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(245, 30);
            this.lblTitulo.Text = "Cambiar Contraseña";
            //
            // label1 (Usuario)
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label1.Location = new System.Drawing.Point(45, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.Text = "Usuario";
            //
            // txtUsuario (read-only, se carga con el login del usuario actual)
            //
            this.txtUsuario.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtUsuario.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.txtUsuario.Location = new System.Drawing.Point(45, 100);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(400, 22);
            this.txtUsuario.TabIndex = 0;
            //
            // label2 (Contraseña actual)
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label2.Location = new System.Drawing.Point(45, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.Text = "Contraseña actual";
            //
            // txtContrasenia
            //
            this.txtContrasenia.BackColor = System.Drawing.Color.White;
            this.txtContrasenia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContrasenia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtContrasenia.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.txtContrasenia.Location = new System.Drawing.Point(45, 160);
            this.txtContrasenia.Name = "txtContrasenia";
            this.txtContrasenia.PasswordChar = '*';
            this.txtContrasenia.Size = new System.Drawing.Size(400, 22);
            this.txtContrasenia.TabIndex = 1;
            //
            // label3 (Nueva contraseña)
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label3.Location = new System.Drawing.Point(45, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 17);
            this.label3.Text = "Nueva contraseña";
            //
            // txtNuevaconstrasenia
            //
            this.txtNuevaconstrasenia.BackColor = System.Drawing.Color.White;
            this.txtNuevaconstrasenia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNuevaconstrasenia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNuevaconstrasenia.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.txtNuevaconstrasenia.Location = new System.Drawing.Point(45, 220);
            this.txtNuevaconstrasenia.Name = "txtNuevaconstrasenia";
            this.txtNuevaconstrasenia.PasswordChar = '*';
            this.txtNuevaconstrasenia.Size = new System.Drawing.Size(400, 22);
            this.txtNuevaconstrasenia.TabIndex = 2;
            //
            // label4 (Confirmar contraseña)
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label4.Location = new System.Drawing.Point(45, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 17);
            this.label4.Text = "Confirmar contraseña";
            //
            // txtConfirmarContrasenia
            //
            this.txtConfirmarContrasenia.BackColor = System.Drawing.Color.White;
            this.txtConfirmarContrasenia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmarContrasenia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtConfirmarContrasenia.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.txtConfirmarContrasenia.Location = new System.Drawing.Point(45, 280);
            this.txtConfirmarContrasenia.Name = "txtConfirmarContrasenia";
            this.txtConfirmarContrasenia.PasswordChar = '*';
            this.txtConfirmarContrasenia.Size = new System.Drawing.Size(400, 22);
            this.txtConfirmarContrasenia.TabIndex = 3;
            //
            // btnAceptar
            //
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(25, 118, 210);
            this.btnAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.btnAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(165, 325);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(160, 38);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            //
            // FRMCambiarContrasenia
            //
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlCard);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FRMCambiarContrasenia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambiar Contraseña";
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContrasenia;
        private System.Windows.Forms.TextBox txtNuevaconstrasenia;
        private System.Windows.Forms.TextBox txtConfirmarContrasenia;
        private System.Windows.Forms.Button btnAceptar;
    }
}
