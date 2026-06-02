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
            this.tabFamilias = new System.Windows.Forms.TabPage();
            this.tabRoles = new System.Windows.Forms.TabPage();

            this.lblTitPatentes = new System.Windows.Forms.Label();
            this.dgvPatentes = new System.Windows.Forms.DataGridView();

            this.lblTitFamilias = new System.Windows.Forms.Label();
            this.dgvFamilias = new System.Windows.Forms.DataGridView();
            this.btnEliminarFamilia = new System.Windows.Forms.Button();
            this.gbCrearFamilia = new System.Windows.Forms.GroupBox();
            this.lblNombreFamilia = new System.Windows.Forms.Label();
            this.txtNombreFamilia = new System.Windows.Forms.TextBox();
            this.lblPatentesFamilia = new System.Windows.Forms.Label();
            this.clbPatentesFamilia = new System.Windows.Forms.CheckedListBox();
            this.lblSubfamilias = new System.Windows.Forms.Label();
            this.clbSubfamilias = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarFamilia = new System.Windows.Forms.Button();
            this.btnLimpiarFamilia = new System.Windows.Forms.Button();

            this.lblTitRoles = new System.Windows.Forms.Label();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.btnEliminarRol = new System.Windows.Forms.Button();
            this.gbCrearRol = new System.Windows.Forms.GroupBox();
            this.lblNombreRol = new System.Windows.Forms.Label();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.lblPatentesRol = new System.Windows.Forms.Label();
            this.clbPatentesRol = new System.Windows.Forms.CheckedListBox();
            this.lblFamiliasRol = new System.Windows.Forms.Label();
            this.clbFamiliasRol = new System.Windows.Forms.CheckedListBox();
            this.btnGuardarRol = new System.Windows.Forms.Button();
            this.btnLimpiarRol = new System.Windows.Forms.Button();

            // ── Form ──
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 620);
            this.Text = "Gestión de Permisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // ── tabControl ──
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabPatentes);
            this.tabControl.Controls.Add(this.tabFamilias);
            this.tabControl.Controls.Add(this.tabRoles);

            // ── tabPatentes ──
            this.tabPatentes.Text = "Patentes";
            this.tabPatentes.Padding = new System.Windows.Forms.Padding(10);
            this.tabPatentes.Controls.Add(this.dgvPatentes);
            this.tabPatentes.Controls.Add(this.lblTitPatentes);

            this.lblTitPatentes.Text = "Catálogo de patentes del sistema (solo lectura)";
            this.lblTitPatentes.Location = new System.Drawing.Point(15, 12);
            this.lblTitPatentes.AutoSize = true;
            this.lblTitPatentes.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            this.dgvPatentes.Location = new System.Drawing.Point(15, 45);
            this.dgvPatentes.Size = new System.Drawing.Size(905, 520);
            this.dgvPatentes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.dgvPatentes.ReadOnly = true;
            this.dgvPatentes.AllowUserToAddRows = false;
            this.dgvPatentes.AllowUserToDeleteRows = false;
            this.dgvPatentes.AllowUserToResizeRows = false;
            this.dgvPatentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatentes.MultiSelect = false;
            this.dgvPatentes.RowHeadersVisible = false;
            this.dgvPatentes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // ── tabFamilias ──
            this.tabFamilias.Text = "Familias";
            this.tabFamilias.Padding = new System.Windows.Forms.Padding(10);
            this.tabFamilias.Controls.Add(this.gbCrearFamilia);
            this.tabFamilias.Controls.Add(this.btnEliminarFamilia);
            this.tabFamilias.Controls.Add(this.dgvFamilias);
            this.tabFamilias.Controls.Add(this.lblTitFamilias);

            this.lblTitFamilias.Text = "Familias existentes";
            this.lblTitFamilias.Location = new System.Drawing.Point(15, 12);
            this.lblTitFamilias.AutoSize = true;
            this.lblTitFamilias.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            this.dgvFamilias.Location = new System.Drawing.Point(15, 45);
            this.dgvFamilias.Size = new System.Drawing.Size(400, 240);
            this.dgvFamilias.ReadOnly = true;
            this.dgvFamilias.AllowUserToAddRows = false;
            this.dgvFamilias.AllowUserToDeleteRows = false;
            this.dgvFamilias.AllowUserToResizeRows = false;
            this.dgvFamilias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFamilias.MultiSelect = false;
            this.dgvFamilias.RowHeadersVisible = false;
            this.dgvFamilias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnEliminarFamilia.Text = "Eliminar familia seleccionada";
            this.btnEliminarFamilia.Location = new System.Drawing.Point(15, 295);
            this.btnEliminarFamilia.Size = new System.Drawing.Size(400, 30);
            this.btnEliminarFamilia.Click += new System.EventHandler(this.btnEliminarFamilia_Click);

            this.gbCrearFamilia.Text = "Crear nueva familia";
            this.gbCrearFamilia.Location = new System.Drawing.Point(435, 45);
            this.gbCrearFamilia.Size = new System.Drawing.Size(485, 520);
            this.gbCrearFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));

            this.lblNombreFamilia.Text = "Nombre:";
            this.lblNombreFamilia.Location = new System.Drawing.Point(15, 30);
            this.lblNombreFamilia.AutoSize = true;
            this.gbCrearFamilia.Controls.Add(this.lblNombreFamilia);

            this.txtNombreFamilia.Location = new System.Drawing.Point(80, 27);
            this.txtNombreFamilia.Size = new System.Drawing.Size(380, 23);
            this.gbCrearFamilia.Controls.Add(this.txtNombreFamilia);

            this.lblPatentesFamilia.Text = "Patentes a incluir:";
            this.lblPatentesFamilia.Location = new System.Drawing.Point(15, 65);
            this.lblPatentesFamilia.AutoSize = true;
            this.gbCrearFamilia.Controls.Add(this.lblPatentesFamilia);

            this.clbPatentesFamilia.Location = new System.Drawing.Point(15, 85);
            this.clbPatentesFamilia.Size = new System.Drawing.Size(445, 170);
            this.clbPatentesFamilia.CheckOnClick = true;
            this.gbCrearFamilia.Controls.Add(this.clbPatentesFamilia);

            this.lblSubfamilias.Text = "Subfamilias (anidamiento):";
            this.lblSubfamilias.Location = new System.Drawing.Point(15, 265);
            this.lblSubfamilias.AutoSize = true;
            this.gbCrearFamilia.Controls.Add(this.lblSubfamilias);

            this.clbSubfamilias.Location = new System.Drawing.Point(15, 285);
            this.clbSubfamilias.Size = new System.Drawing.Size(445, 170);
            this.clbSubfamilias.CheckOnClick = true;
            this.gbCrearFamilia.Controls.Add(this.clbSubfamilias);

            this.btnGuardarFamilia.Text = "Guardar";
            this.btnGuardarFamilia.Location = new System.Drawing.Point(15, 475);
            this.btnGuardarFamilia.Size = new System.Drawing.Size(120, 30);
            this.btnGuardarFamilia.Click += new System.EventHandler(this.btnGuardarFamilia_Click);
            this.gbCrearFamilia.Controls.Add(this.btnGuardarFamilia);

            this.btnLimpiarFamilia.Text = "Limpiar";
            this.btnLimpiarFamilia.Location = new System.Drawing.Point(145, 475);
            this.btnLimpiarFamilia.Size = new System.Drawing.Size(120, 30);
            this.btnLimpiarFamilia.Click += new System.EventHandler(this.btnLimpiarFamilia_Click);
            this.gbCrearFamilia.Controls.Add(this.btnLimpiarFamilia);

            // ── tabRoles ──
            this.tabRoles.Text = "Roles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(10);
            this.tabRoles.Controls.Add(this.gbCrearRol);
            this.tabRoles.Controls.Add(this.btnEliminarRol);
            this.tabRoles.Controls.Add(this.dgvRoles);
            this.tabRoles.Controls.Add(this.lblTitRoles);

            this.lblTitRoles.Text = "Roles existentes";
            this.lblTitRoles.Location = new System.Drawing.Point(15, 12);
            this.lblTitRoles.AutoSize = true;
            this.lblTitRoles.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            this.dgvRoles.Location = new System.Drawing.Point(15, 45);
            this.dgvRoles.Size = new System.Drawing.Size(400, 240);
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.AllowUserToResizeRows = false;
            this.dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoles.MultiSelect = false;
            this.dgvRoles.RowHeadersVisible = false;
            this.dgvRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnEliminarRol.Text = "Eliminar rol seleccionado";
            this.btnEliminarRol.Location = new System.Drawing.Point(15, 295);
            this.btnEliminarRol.Size = new System.Drawing.Size(400, 30);
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);

            this.gbCrearRol.Text = "Crear nuevo rol";
            this.gbCrearRol.Location = new System.Drawing.Point(435, 45);
            this.gbCrearRol.Size = new System.Drawing.Size(485, 520);
            this.gbCrearRol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));

            this.lblNombreRol.Text = "Nombre:";
            this.lblNombreRol.Location = new System.Drawing.Point(15, 30);
            this.lblNombreRol.AutoSize = true;
            this.gbCrearRol.Controls.Add(this.lblNombreRol);

            this.txtNombreRol.Location = new System.Drawing.Point(80, 27);
            this.txtNombreRol.Size = new System.Drawing.Size(380, 23);
            this.gbCrearRol.Controls.Add(this.txtNombreRol);

            this.lblPatentesRol.Text = "Patentes individuales:";
            this.lblPatentesRol.Location = new System.Drawing.Point(15, 65);
            this.lblPatentesRol.AutoSize = true;
            this.gbCrearRol.Controls.Add(this.lblPatentesRol);

            this.clbPatentesRol.Location = new System.Drawing.Point(15, 85);
            this.clbPatentesRol.Size = new System.Drawing.Size(445, 170);
            this.clbPatentesRol.CheckOnClick = true;
            this.gbCrearRol.Controls.Add(this.clbPatentesRol);

            this.lblFamiliasRol.Text = "Familias:";
            this.lblFamiliasRol.Location = new System.Drawing.Point(15, 265);
            this.lblFamiliasRol.AutoSize = true;
            this.gbCrearRol.Controls.Add(this.lblFamiliasRol);

            this.clbFamiliasRol.Location = new System.Drawing.Point(15, 285);
            this.clbFamiliasRol.Size = new System.Drawing.Size(445, 170);
            this.clbFamiliasRol.CheckOnClick = true;
            this.gbCrearRol.Controls.Add(this.clbFamiliasRol);

            this.btnGuardarRol.Text = "Guardar";
            this.btnGuardarRol.Location = new System.Drawing.Point(15, 475);
            this.btnGuardarRol.Size = new System.Drawing.Size(120, 30);
            this.btnGuardarRol.Click += new System.EventHandler(this.btnGuardarRol_Click);
            this.gbCrearRol.Controls.Add(this.btnGuardarRol);

            this.btnLimpiarRol.Text = "Limpiar";
            this.btnLimpiarRol.Location = new System.Drawing.Point(145, 475);
            this.btnLimpiarRol.Size = new System.Drawing.Size(120, 30);
            this.btnLimpiarRol.Click += new System.EventHandler(this.btnLimpiarRol_Click);
            this.gbCrearRol.Controls.Add(this.btnLimpiarRol);

            // ── Agregamos el tabControl al form ──
            this.Controls.Add(this.tabControl);
            this.Load += new System.EventHandler(this.FRMGestionPermisos_Load);

            this.ResumeLayout(false);
        }

        #endregion
    }
}
