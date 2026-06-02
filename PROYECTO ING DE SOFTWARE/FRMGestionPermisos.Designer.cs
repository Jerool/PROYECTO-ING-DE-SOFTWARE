namespace PROYECTO_ING_DE_SOFTWARE
{
    partial class FRMGestionPermisos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPatentes;
        private System.Windows.Forms.TabPage tabFamilias;
        private System.Windows.Forms.TabPage tabRoles;

        // ── Tab Patentes ──
        private System.Windows.Forms.DataGridView dgvPatentes;
        private System.Windows.Forms.Label lblTitPatentes;

        // ── Tab Familias ──
        private System.Windows.Forms.Label lblTitFamilias;
        private System.Windows.Forms.DataGridView dgvFamilias;
        private System.Windows.Forms.Button btnEliminarFamilia;
        private System.Windows.Forms.GroupBox gbCrearFamilia;
        private System.Windows.Forms.Label lblNombreFamilia;
        private System.Windows.Forms.TextBox txtNombreFamilia;
        private System.Windows.Forms.Label lblPatentesFamilia;
        private System.Windows.Forms.CheckedListBox clbPatentesFamilia;
        private System.Windows.Forms.Label lblSubfamilias;
        private System.Windows.Forms.CheckedListBox clbSubfamilias;
        private System.Windows.Forms.Button btnGuardarFamilia;
        private System.Windows.Forms.Button btnLimpiarFamilia;

        // ── Tab Roles ──
        private System.Windows.Forms.Label lblTitRoles;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Button btnEliminarRol;
        private System.Windows.Forms.GroupBox gbCrearRol;
        private System.Windows.Forms.Label lblNombreRol;
        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Label lblPatentesRol;
        private System.Windows.Forms.CheckedListBox clbPatentesRol;
        private System.Windows.Forms.Label lblFamiliasRol;
        private System.Windows.Forms.CheckedListBox clbFamiliasRol;
        private System.Windows.Forms.Button btnGuardarRol;
        private System.Windows.Forms.Button btnLimpiarRol;

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPatentes = new System.Windows.Forms.TabPage();
            this.dgvPatentes = new System.Windows.Forms.DataGridView();
            this.lblTitPatentes = new System.Windows.Forms.Label();
            this.tabFamilias = new System.Windows.Forms.TabPage();
            this.gbCrearFamilia = new System.Windows.Forms.GroupBox();
            this.lblNombreFamilia = new System.Windows.Forms.Label();
            this.txtNombreFamilia = new System.Windows.Forms.TextBox();
            this.lblPatentesFamilia = new System.Windows.Forms.Label();
            this.clbPatentesFamilia = new System.Windows.Forms.CheckedListBox();
            this.lblSubfamilias = new System.Windows.Forms.Label();
            this.clbSubfamilias = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarFamilia = new System.Windows.Forms.Button();
            this.btnLimpiarFamilia = new System.Windows.Forms.Button();
            this.btnEliminarFamilia = new System.Windows.Forms.Button();
            this.dgvFamilias = new System.Windows.Forms.DataGridView();
            this.lblTitFamilias = new System.Windows.Forms.Label();
            this.tabRoles = new System.Windows.Forms.TabPage();
            this.gbCrearRol = new System.Windows.Forms.GroupBox();
            this.lblNombreRol = new System.Windows.Forms.Label();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.lblPatentesRol = new System.Windows.Forms.Label();
            this.clbPatentesRol = new System.Windows.Forms.CheckedListBox();
            this.lblFamiliasRol = new System.Windows.Forms.Label();
            this.clbFamiliasRol = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarRol = new System.Windows.Forms.Button();
            this.btnLimpiarRol = new System.Windows.Forms.Button();
            this.btnEliminarRol = new System.Windows.Forms.Button();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.lblTitRoles = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPatentes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatentes)).BeginInit();
            this.tabFamilias.SuspendLayout();
            this.gbCrearFamilia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFamilias)).BeginInit();
            this.tabRoles.SuspendLayout();
            this.gbCrearRol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPatentes);
            this.tabControl.Controls.Add(this.tabFamilias);
            this.tabControl.Controls.Add(this.tabRoles);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(823, 537);
            this.tabControl.TabIndex = 0;
            // 
            // tabPatentes
            // 
            this.tabPatentes.Controls.Add(this.dgvPatentes);
            this.tabPatentes.Controls.Add(this.lblTitPatentes);
            this.tabPatentes.Location = new System.Drawing.Point(4, 22);
            this.tabPatentes.Name = "tabPatentes";
            this.tabPatentes.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.tabPatentes.Size = new System.Drawing.Size(815, 511);
            this.tabPatentes.TabIndex = 0;
            this.tabPatentes.Text = "Patentes";
            // 
            // dgvPatentes
            // 
            this.dgvPatentes.AllowUserToAddRows = false;
            this.dgvPatentes.AllowUserToDeleteRows = false;
            this.dgvPatentes.AllowUserToResizeRows = false;
            this.dgvPatentes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPatentes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPatentes.Location = new System.Drawing.Point(13, 39);
            this.dgvPatentes.MultiSelect = false;
            this.dgvPatentes.Name = "dgvPatentes";
            this.dgvPatentes.ReadOnly = true;
            this.dgvPatentes.RowHeadersVisible = false;
            this.dgvPatentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatentes.Size = new System.Drawing.Size(1428, 901);
            this.dgvPatentes.TabIndex = 0;
            // 
            // lblTitPatentes
            // 
            this.lblTitPatentes.AutoSize = true;
            this.lblTitPatentes.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitPatentes.Location = new System.Drawing.Point(13, 10);
            this.lblTitPatentes.Name = "lblTitPatentes";
            this.lblTitPatentes.Size = new System.Drawing.Size(240, 20);
            this.lblTitPatentes.TabIndex = 1;
            this.lblTitPatentes.Text = "Catálogo de patentes del sistema";
            this.lblTitPatentes.Click += new System.EventHandler(this.lblTitPatentes_Click);
            // 
            // tabFamilias
            // 
            this.tabFamilias.Controls.Add(this.gbCrearFamilia);
            this.tabFamilias.Controls.Add(this.btnEliminarFamilia);
            this.tabFamilias.Controls.Add(this.dgvFamilias);
            this.tabFamilias.Controls.Add(this.lblTitFamilias);
            this.tabFamilias.Location = new System.Drawing.Point(4, 22);
            this.tabFamilias.Name = "tabFamilias";
            this.tabFamilias.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.tabFamilias.Size = new System.Drawing.Size(163, 61);
            this.tabFamilias.TabIndex = 1;
            this.tabFamilias.Text = "Familias";
            // 
            // gbCrearFamilia
            // 
            this.gbCrearFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrearFamilia.Controls.Add(this.lblNombreFamilia);
            this.gbCrearFamilia.Controls.Add(this.txtNombreFamilia);
            this.gbCrearFamilia.Controls.Add(this.lblPatentesFamilia);
            this.gbCrearFamilia.Controls.Add(this.clbPatentesFamilia);
            this.gbCrearFamilia.Controls.Add(this.lblSubfamilias);
            this.gbCrearFamilia.Controls.Add(this.clbSubfamilias);
            this.gbCrearFamilia.Controls.Add(this.btnGuardarFamilia);
            this.gbCrearFamilia.Controls.Add(this.btnLimpiarFamilia);
            this.gbCrearFamilia.Location = new System.Drawing.Point(373, 39);
            this.gbCrearFamilia.Name = "gbCrearFamilia";
            this.gbCrearFamilia.Size = new System.Drawing.Size(416, 451);
            this.gbCrearFamilia.TabIndex = 0;
            this.gbCrearFamilia.TabStop = false;
            this.gbCrearFamilia.Text = "Crear nueva familia";
            // 
            // lblNombreFamilia
            // 
            this.lblNombreFamilia.AutoSize = true;
            this.lblNombreFamilia.Location = new System.Drawing.Point(13, 26);
            this.lblNombreFamilia.Name = "lblNombreFamilia";
            this.lblNombreFamilia.Size = new System.Drawing.Size(47, 13);
            this.lblNombreFamilia.TabIndex = 0;
            this.lblNombreFamilia.Text = "Nombre:";
            // 
            // txtNombreFamilia
            // 
            this.txtNombreFamilia.Location = new System.Drawing.Point(69, 23);
            this.txtNombreFamilia.Name = "txtNombreFamilia";
            this.txtNombreFamilia.Size = new System.Drawing.Size(326, 20);
            this.txtNombreFamilia.TabIndex = 1;
            // 
            // lblPatentesFamilia
            // 
            this.lblPatentesFamilia.AutoSize = true;
            this.lblPatentesFamilia.Location = new System.Drawing.Point(13, 56);
            this.lblPatentesFamilia.Name = "lblPatentesFamilia";
            this.lblPatentesFamilia.Size = new System.Drawing.Size(91, 13);
            this.lblPatentesFamilia.TabIndex = 2;
            this.lblPatentesFamilia.Text = "Patentes a incluir:";
            // 
            // clbPatentesFamilia
            // 
            this.clbPatentesFamilia.CheckOnClick = true;
            this.clbPatentesFamilia.Location = new System.Drawing.Point(13, 74);
            this.clbPatentesFamilia.Name = "clbPatentesFamilia";
            this.clbPatentesFamilia.Size = new System.Drawing.Size(382, 139);
            this.clbPatentesFamilia.TabIndex = 3;
            // 
            // lblSubfamilias
            // 
            this.lblSubfamilias.AutoSize = true;
            this.lblSubfamilias.Location = new System.Drawing.Point(13, 230);
            this.lblSubfamilias.Name = "lblSubfamilias";
            this.lblSubfamilias.Size = new System.Drawing.Size(129, 13);
            this.lblSubfamilias.TabIndex = 4;
            this.lblSubfamilias.Text = "Subfamilias (anidamiento):";
            // 
            // clbSubfamilias
            // 
            this.clbSubfamilias.CheckOnClick = true;
            this.clbSubfamilias.Location = new System.Drawing.Point(13, 247);
            this.clbSubfamilias.Name = "clbSubfamilias";
            this.clbSubfamilias.Size = new System.Drawing.Size(382, 139);
            this.clbSubfamilias.TabIndex = 5;
            // 
            // btnGuardarFamilia
            // 
            this.btnGuardarFamilia.Location = new System.Drawing.Point(13, 412);
            this.btnGuardarFamilia.Name = "btnGuardarFamilia";
            this.btnGuardarFamilia.Size = new System.Drawing.Size(103, 26);
            this.btnGuardarFamilia.TabIndex = 6;
            this.btnGuardarFamilia.Text = "Guardar";
            this.btnGuardarFamilia.Click += new System.EventHandler(this.btnGuardarFamilia_Click);
            // 
            // btnLimpiarFamilia
            // 
            this.btnLimpiarFamilia.Location = new System.Drawing.Point(124, 412);
            this.btnLimpiarFamilia.Name = "btnLimpiarFamilia";
            this.btnLimpiarFamilia.Size = new System.Drawing.Size(103, 26);
            this.btnLimpiarFamilia.TabIndex = 7;
            this.btnLimpiarFamilia.Text = "Limpiar";
            this.btnLimpiarFamilia.Click += new System.EventHandler(this.btnLimpiarFamilia_Click);
            // 
            // btnEliminarFamilia
            // 
            this.btnEliminarFamilia.Location = new System.Drawing.Point(13, 256);
            this.btnEliminarFamilia.Name = "btnEliminarFamilia";
            this.btnEliminarFamilia.Size = new System.Drawing.Size(343, 26);
            this.btnEliminarFamilia.TabIndex = 1;
            this.btnEliminarFamilia.Text = "Eliminar familia seleccionada";
            this.btnEliminarFamilia.Click += new System.EventHandler(this.btnEliminarFamilia_Click);
            // 
            // dgvFamilias
            // 
            this.dgvFamilias.AllowUserToAddRows = false;
            this.dgvFamilias.AllowUserToDeleteRows = false;
            this.dgvFamilias.AllowUserToResizeRows = false;
            this.dgvFamilias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFamilias.Location = new System.Drawing.Point(13, 39);
            this.dgvFamilias.MultiSelect = false;
            this.dgvFamilias.Name = "dgvFamilias";
            this.dgvFamilias.ReadOnly = true;
            this.dgvFamilias.RowHeadersVisible = false;
            this.dgvFamilias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFamilias.Size = new System.Drawing.Size(343, 208);
            this.dgvFamilias.TabIndex = 2;
            // 
            // lblTitFamilias
            // 
            this.lblTitFamilias.AutoSize = true;
            this.lblTitFamilias.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitFamilias.Location = new System.Drawing.Point(13, 10);
            this.lblTitFamilias.Name = "lblTitFamilias";
            this.lblTitFamilias.Size = new System.Drawing.Size(141, 20);
            this.lblTitFamilias.TabIndex = 3;
            this.lblTitFamilias.Text = "Familias existentes";
            // 
            // tabRoles
            // 
            this.tabRoles.Controls.Add(this.gbCrearRol);
            this.tabRoles.Controls.Add(this.btnEliminarRol);
            this.tabRoles.Controls.Add(this.dgvRoles);
            this.tabRoles.Controls.Add(this.lblTitRoles);
            this.tabRoles.Location = new System.Drawing.Point(4, 22);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.tabRoles.Size = new System.Drawing.Size(163, 61);
            this.tabRoles.TabIndex = 2;
            this.tabRoles.Text = "Roles";
            // 
            // gbCrearRol
            // 
            this.gbCrearRol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrearRol.Controls.Add(this.lblNombreRol);
            this.gbCrearRol.Controls.Add(this.txtNombreRol);
            this.gbCrearRol.Controls.Add(this.lblPatentesRol);
            this.gbCrearRol.Controls.Add(this.clbPatentesRol);
            this.gbCrearRol.Controls.Add(this.lblFamiliasRol);
            this.gbCrearRol.Controls.Add(this.clbFamiliasRol);
            this.gbCrearRol.Controls.Add(this.btnGuardarRol);
            this.gbCrearRol.Controls.Add(this.btnLimpiarRol);
            this.gbCrearRol.Location = new System.Drawing.Point(373, 39);
            this.gbCrearRol.Name = "gbCrearRol";
            this.gbCrearRol.Size = new System.Drawing.Size(416, 451);
            this.gbCrearRol.TabIndex = 0;
            this.gbCrearRol.TabStop = false;
            this.gbCrearRol.Text = "Crear nuevo rol";
            // 
            // lblNombreRol
            // 
            this.lblNombreRol.AutoSize = true;
            this.lblNombreRol.Location = new System.Drawing.Point(13, 26);
            this.lblNombreRol.Name = "lblNombreRol";
            this.lblNombreRol.Size = new System.Drawing.Size(47, 13);
            this.lblNombreRol.TabIndex = 0;
            this.lblNombreRol.Text = "Nombre:";
            // 
            // txtNombreRol
            // 
            this.txtNombreRol.Location = new System.Drawing.Point(69, 23);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(326, 20);
            this.txtNombreRol.TabIndex = 1;
            // 
            // lblPatentesRol
            // 
            this.lblPatentesRol.AutoSize = true;
            this.lblPatentesRol.Location = new System.Drawing.Point(13, 56);
            this.lblPatentesRol.Name = "lblPatentesRol";
            this.lblPatentesRol.Size = new System.Drawing.Size(110, 13);
            this.lblPatentesRol.TabIndex = 2;
            this.lblPatentesRol.Text = "Patentes individuales:";
            // 
            // clbPatentesRol
            // 
            this.clbPatentesRol.CheckOnClick = true;
            this.clbPatentesRol.Location = new System.Drawing.Point(13, 74);
            this.clbPatentesRol.Name = "clbPatentesRol";
            this.clbPatentesRol.Size = new System.Drawing.Size(382, 139);
            this.clbPatentesRol.TabIndex = 3;
            // 
            // lblFamiliasRol
            // 
            this.lblFamiliasRol.AutoSize = true;
            this.lblFamiliasRol.Location = new System.Drawing.Point(13, 230);
            this.lblFamiliasRol.Name = "lblFamiliasRol";
            this.lblFamiliasRol.Size = new System.Drawing.Size(47, 13);
            this.lblFamiliasRol.TabIndex = 4;
            this.lblFamiliasRol.Text = "Familias:";
            // 
            // clbFamiliasRol
            // 
            this.clbFamiliasRol.CheckOnClick = true;
            this.clbFamiliasRol.Location = new System.Drawing.Point(13, 247);
            this.clbFamiliasRol.Name = "clbFamiliasRol";
            this.clbFamiliasRol.Size = new System.Drawing.Size(382, 139);
            this.clbFamiliasRol.TabIndex = 5;
            // 
            // btnGuardarRol
            // 
            this.btnGuardarRol.Location = new System.Drawing.Point(13, 412);
            this.btnGuardarRol.Name = "btnGuardarRol";
            this.btnGuardarRol.Size = new System.Drawing.Size(103, 26);
            this.btnGuardarRol.TabIndex = 6;
            this.btnGuardarRol.Text = "Guardar";
            this.btnGuardarRol.Click += new System.EventHandler(this.btnGuardarRol_Click);
            // 
            // btnLimpiarRol
            // 
            this.btnLimpiarRol.Location = new System.Drawing.Point(124, 412);
            this.btnLimpiarRol.Name = "btnLimpiarRol";
            this.btnLimpiarRol.Size = new System.Drawing.Size(103, 26);
            this.btnLimpiarRol.TabIndex = 7;
            this.btnLimpiarRol.Text = "Limpiar";
            this.btnLimpiarRol.Click += new System.EventHandler(this.btnLimpiarRol_Click);
            // 
            // btnEliminarRol
            // 
            this.btnEliminarRol.Location = new System.Drawing.Point(13, 256);
            this.btnEliminarRol.Name = "btnEliminarRol";
            this.btnEliminarRol.Size = new System.Drawing.Size(343, 26);
            this.btnEliminarRol.TabIndex = 1;
            this.btnEliminarRol.Text = "Eliminar rol seleccionado";
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);
            // 
            // dgvRoles
            // 
            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.AllowUserToResizeRows = false;
            this.dgvRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoles.Location = new System.Drawing.Point(13, 39);
            this.dgvRoles.MultiSelect = false;
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.RowHeadersVisible = false;
            this.dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoles.Size = new System.Drawing.Size(343, 208);
            this.dgvRoles.TabIndex = 2;
            // 
            // lblTitRoles
            // 
            this.lblTitRoles.AutoSize = true;
            this.lblTitRoles.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitRoles.Location = new System.Drawing.Point(13, 10);
            this.lblTitRoles.Name = "lblTitRoles";
            this.lblTitRoles.Size = new System.Drawing.Size(122, 20);
            this.lblTitRoles.TabIndex = 3;
            this.lblTitRoles.Text = "Roles existentes";
            // 
            // FRMGestionPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 537);
            this.Controls.Add(this.tabControl);
            this.Name = "FRMGestionPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Permisos";
            this.Load += new System.EventHandler(this.FRMGestionPermisos_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPatentes.ResumeLayout(false);
            this.tabPatentes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatentes)).EndInit();
            this.tabFamilias.ResumeLayout(false);
            this.tabFamilias.PerformLayout();
            this.gbCrearFamilia.ResumeLayout(false);
            this.gbCrearFamilia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFamilias)).EndInit();
            this.tabRoles.ResumeLayout(false);
            this.tabRoles.PerformLayout();
            this.gbCrearRol.ResumeLayout(false);
            this.gbCrearRol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
