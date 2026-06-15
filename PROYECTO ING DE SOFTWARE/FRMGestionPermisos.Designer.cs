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

        private System.Windows.Forms.DataGridView dgvPatentes;
        private System.Windows.Forms.Label lblTitPatentes;

        private System.Windows.Forms.Label lblTitFamilias;
        private System.Windows.Forms.DataGridView dgvFamilias;
        private System.Windows.Forms.Button btnEliminarFamilia;
        private System.Windows.Forms.Button btnModificarFamilia;
        private System.Windows.Forms.GroupBox gbCrearFamilia;
        private System.Windows.Forms.Label lblNombreFamilia;
        private System.Windows.Forms.TextBox txtNombreFamilia;
        private System.Windows.Forms.Label lblPatentesFamilia;
        private System.Windows.Forms.CheckedListBox clbPatentesFamilia;
        private System.Windows.Forms.Label lblSubfamilias;
        private System.Windows.Forms.CheckedListBox clbSubfamilias;
        private System.Windows.Forms.Button btnGuardarFamilia;
        private System.Windows.Forms.Button btnLimpiarFamilia;

        private System.Windows.Forms.Label lblTitRoles;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Button btnEliminarRol;
        private System.Windows.Forms.Button btnModificarRol;
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
            // ─── Paleta Material Azul (igual que los demás forms) ───
            System.Drawing.Color azulOscuro    = System.Drawing.Color.FromArgb(13, 71, 161);
            System.Drawing.Color azulClaro     = System.Drawing.Color.FromArgb(227, 242, 253);
            System.Drawing.Color blanco        = System.Drawing.Color.White;
            System.Drawing.Font  fuenteBase    = new System.Drawing.Font("Segoe UI", 9F);
            System.Drawing.Font  fuenteTitulo  = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            System.Drawing.Font  fuenteGroup   = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            System.Drawing.Font  fuenteBtn     = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);

            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPatentes = new System.Windows.Forms.TabPage();
            this.dgvPatentes = new System.Windows.Forms.DataGridView();
            this.lblTitPatentes = new System.Windows.Forms.Label();
            this.tabFamilias = new System.Windows.Forms.TabPage();
            this.btnModificarFamilia = new System.Windows.Forms.Button();
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
            this.btnModificarRol = new System.Windows.Forms.Button();
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
            this.tabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(900, 600);
            this.tabControl.TabIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.ItemSize = new System.Drawing.Size(140, 34);
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Normal;
            this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControl_DrawItem);
            //
            // tabPatentes
            //
            this.tabPatentes.BackColor = azulClaro;
            this.tabPatentes.Controls.Add(this.dgvPatentes);
            this.tabPatentes.Controls.Add(this.lblTitPatentes);
            this.tabPatentes.Location = new System.Drawing.Point(4, 22);
            this.tabPatentes.Name = "tabPatentes";
            this.tabPatentes.Padding = new System.Windows.Forms.Padding(12);
            this.tabPatentes.Size = new System.Drawing.Size(892, 574);
            this.tabPatentes.TabIndex = 0;
            this.tabPatentes.Text = "Patentes";
            //
            // lblTitPatentes
            //
            this.lblTitPatentes.AutoSize = true;
            this.lblTitPatentes.Font = fuenteTitulo;
            this.lblTitPatentes.ForeColor = azulOscuro;
            this.lblTitPatentes.Location = new System.Drawing.Point(15, 15);
            this.lblTitPatentes.Name = "lblTitPatentes";
            this.lblTitPatentes.Text = "Catálogo de patentes del sistema";
            //
            // dgvPatentes
            //
            EstilarGrilla(this.dgvPatentes, azulOscuro, azulClaro, blanco, fuenteBase);
            this.dgvPatentes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPatentes.Location = new System.Drawing.Point(15, 50);
            this.dgvPatentes.Name = "dgvPatentes";
            this.dgvPatentes.Size = new System.Drawing.Size(862, 510);
            this.dgvPatentes.TabIndex = 0;
            //
            // tabFamilias
            //
            this.tabFamilias.BackColor = azulClaro;
            this.tabFamilias.Controls.Add(this.btnModificarFamilia);
            this.tabFamilias.Controls.Add(this.gbCrearFamilia);
            this.tabFamilias.Controls.Add(this.btnEliminarFamilia);
            this.tabFamilias.Controls.Add(this.dgvFamilias);
            this.tabFamilias.Controls.Add(this.lblTitFamilias);
            this.tabFamilias.Location = new System.Drawing.Point(4, 22);
            this.tabFamilias.Name = "tabFamilias";
            this.tabFamilias.Padding = new System.Windows.Forms.Padding(12);
            this.tabFamilias.Size = new System.Drawing.Size(892, 574);
            this.tabFamilias.TabIndex = 1;
            this.tabFamilias.Text = "Familias";
            //
            // lblTitFamilias
            //
            this.lblTitFamilias.AutoSize = true;
            this.lblTitFamilias.Font = fuenteTitulo;
            this.lblTitFamilias.ForeColor = azulOscuro;
            this.lblTitFamilias.Location = new System.Drawing.Point(15, 15);
            this.lblTitFamilias.Name = "lblTitFamilias";
            this.lblTitFamilias.Text = "Familias existentes";
            //
            // dgvFamilias
            //
            EstilarGrilla(this.dgvFamilias, azulOscuro, azulClaro, blanco, fuenteBase);
            this.dgvFamilias.Location = new System.Drawing.Point(15, 50);
            this.dgvFamilias.Name = "dgvFamilias";
            this.dgvFamilias.Size = new System.Drawing.Size(380, 240);
            this.dgvFamilias.TabIndex = 1;
            //
            // btnModificarFamilia
            //
            EstilarBotonPrimario(this.btnModificarFamilia, azulOscuro, blanco, fuenteBtn);
            this.btnModificarFamilia.Location = new System.Drawing.Point(15, 300);
            this.btnModificarFamilia.Name = "btnModificarFamilia";
            this.btnModificarFamilia.Size = new System.Drawing.Size(380, 34);
            this.btnModificarFamilia.TabIndex = 2;
            this.btnModificarFamilia.Text = "Modificar familia seleccionada";
            this.btnModificarFamilia.Click += new System.EventHandler(this.btnModificarFamilia_Click);
            //
            // btnEliminarFamilia
            //
            EstilarBotonSecundario(this.btnEliminarFamilia, azulOscuro, blanco, fuenteBtn);
            this.btnEliminarFamilia.Location = new System.Drawing.Point(15, 340);
            this.btnEliminarFamilia.Name = "btnEliminarFamilia";
            this.btnEliminarFamilia.Size = new System.Drawing.Size(380, 34);
            this.btnEliminarFamilia.TabIndex = 3;
            this.btnEliminarFamilia.Text = "Eliminar familia seleccionada";
            this.btnEliminarFamilia.Click += new System.EventHandler(this.btnEliminarFamilia_Click);
            //
            // gbCrearFamilia
            //
            this.gbCrearFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrearFamilia.Controls.Add(this.lblNombreFamilia);
            this.gbCrearFamilia.Controls.Add(this.txtNombreFamilia);
            this.gbCrearFamilia.Controls.Add(this.lblPatentesFamilia);
            this.gbCrearFamilia.Controls.Add(this.clbPatentesFamilia);
            this.gbCrearFamilia.Controls.Add(this.lblSubfamilias);
            this.gbCrearFamilia.Controls.Add(this.clbSubfamilias);
            this.gbCrearFamilia.Controls.Add(this.btnGuardarFamilia);
            this.gbCrearFamilia.Controls.Add(this.btnLimpiarFamilia);
            this.gbCrearFamilia.Font = fuenteGroup;
            this.gbCrearFamilia.ForeColor = azulOscuro;
            this.gbCrearFamilia.Location = new System.Drawing.Point(410, 50);
            this.gbCrearFamilia.Name = "gbCrearFamilia";
            this.gbCrearFamilia.Size = new System.Drawing.Size(467, 510);
            this.gbCrearFamilia.TabIndex = 4;
            this.gbCrearFamilia.TabStop = false;
            this.gbCrearFamilia.Text = "Crear nueva familia";
            //
            // lblNombreFamilia
            //
            this.lblNombreFamilia.AutoSize = true;
            this.lblNombreFamilia.Font = fuenteBase;
            this.lblNombreFamilia.ForeColor = azulOscuro;
            this.lblNombreFamilia.Location = new System.Drawing.Point(20, 35);
            this.lblNombreFamilia.Text = "Nombre:";
            //
            // txtNombreFamilia
            //
            this.txtNombreFamilia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombreFamilia.Font = fuenteBase;
            this.txtNombreFamilia.Location = new System.Drawing.Point(85, 32);
            this.txtNombreFamilia.Name = "txtNombreFamilia";
            this.txtNombreFamilia.Size = new System.Drawing.Size(362, 23);
            this.txtNombreFamilia.TabIndex = 0;
            //
            // lblPatentesFamilia
            //
            this.lblPatentesFamilia.AutoSize = true;
            this.lblPatentesFamilia.Font = fuenteBase;
            this.lblPatentesFamilia.ForeColor = azulOscuro;
            this.lblPatentesFamilia.Location = new System.Drawing.Point(20, 70);
            this.lblPatentesFamilia.Text = "Patentes a incluir:";
            //
            // clbPatentesFamilia
            //
            this.clbPatentesFamilia.BackColor = blanco;
            this.clbPatentesFamilia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbPatentesFamilia.CheckOnClick = true;
            this.clbPatentesFamilia.Font = fuenteBase;
            this.clbPatentesFamilia.Location = new System.Drawing.Point(20, 90);
            this.clbPatentesFamilia.Name = "clbPatentesFamilia";
            this.clbPatentesFamilia.Size = new System.Drawing.Size(427, 156);
            this.clbPatentesFamilia.TabIndex = 1;
            //
            // lblSubfamilias
            //
            this.lblSubfamilias.AutoSize = true;
            this.lblSubfamilias.Font = fuenteBase;
            this.lblSubfamilias.ForeColor = azulOscuro;
            this.lblSubfamilias.Location = new System.Drawing.Point(20, 258);
            this.lblSubfamilias.Text = "Subfamilias (anidamiento):";
            //
            // clbSubfamilias
            //
            this.clbSubfamilias.BackColor = blanco;
            this.clbSubfamilias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbSubfamilias.CheckOnClick = true;
            this.clbSubfamilias.Font = fuenteBase;
            this.clbSubfamilias.Location = new System.Drawing.Point(20, 278);
            this.clbSubfamilias.Name = "clbSubfamilias";
            this.clbSubfamilias.Size = new System.Drawing.Size(427, 156);
            this.clbSubfamilias.TabIndex = 2;
            //
            // btnGuardarFamilia
            //
            EstilarBotonPrimario(this.btnGuardarFamilia, azulOscuro, blanco, fuenteBtn);
            this.btnGuardarFamilia.Location = new System.Drawing.Point(20, 455);
            this.btnGuardarFamilia.Name = "btnGuardarFamilia";
            this.btnGuardarFamilia.Size = new System.Drawing.Size(140, 36);
            this.btnGuardarFamilia.TabIndex = 3;
            this.btnGuardarFamilia.Text = "Guardar";
            this.btnGuardarFamilia.Click += new System.EventHandler(this.btnGuardarFamilia_Click);
            //
            // btnLimpiarFamilia
            //
            EstilarBotonSecundario(this.btnLimpiarFamilia, azulOscuro, blanco, fuenteBtn);
            this.btnLimpiarFamilia.Location = new System.Drawing.Point(175, 455);
            this.btnLimpiarFamilia.Name = "btnLimpiarFamilia";
            this.btnLimpiarFamilia.Size = new System.Drawing.Size(140, 36);
            this.btnLimpiarFamilia.TabIndex = 4;
            this.btnLimpiarFamilia.Text = "Limpiar";
            this.btnLimpiarFamilia.Click += new System.EventHandler(this.btnLimpiarFamilia_Click);
            //
            // tabRoles
            //
            this.tabRoles.BackColor = azulClaro;
            this.tabRoles.Controls.Add(this.btnModificarRol);
            this.tabRoles.Controls.Add(this.gbCrearRol);
            this.tabRoles.Controls.Add(this.btnEliminarRol);
            this.tabRoles.Controls.Add(this.dgvRoles);
            this.tabRoles.Controls.Add(this.lblTitRoles);
            this.tabRoles.Location = new System.Drawing.Point(4, 22);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(12);
            this.tabRoles.Size = new System.Drawing.Size(892, 574);
            this.tabRoles.TabIndex = 2;
            this.tabRoles.Text = "Roles";
            //
            // lblTitRoles
            //
            this.lblTitRoles.AutoSize = true;
            this.lblTitRoles.Font = fuenteTitulo;
            this.lblTitRoles.ForeColor = azulOscuro;
            this.lblTitRoles.Location = new System.Drawing.Point(15, 15);
            this.lblTitRoles.Name = "lblTitRoles";
            this.lblTitRoles.Text = "Roles existentes";
            //
            // dgvRoles
            //
            EstilarGrilla(this.dgvRoles, azulOscuro, azulClaro, blanco, fuenteBase);
            this.dgvRoles.Location = new System.Drawing.Point(15, 50);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.Size = new System.Drawing.Size(380, 240);
            this.dgvRoles.TabIndex = 1;
            //
            // btnModificarRol
            //
            EstilarBotonPrimario(this.btnModificarRol, azulOscuro, blanco, fuenteBtn);
            this.btnModificarRol.Location = new System.Drawing.Point(15, 300);
            this.btnModificarRol.Name = "btnModificarRol";
            this.btnModificarRol.Size = new System.Drawing.Size(380, 34);
            this.btnModificarRol.TabIndex = 2;
            this.btnModificarRol.Text = "Modificar rol seleccionado";
     //       this.btnModificarRol.Click += new System.EventHandler(this.btnModificarRol_Click);
            //
            // btnEliminarRol
            //
            EstilarBotonSecundario(this.btnEliminarRol, azulOscuro, blanco, fuenteBtn);
            this.btnEliminarRol.Location = new System.Drawing.Point(15, 340);
            this.btnEliminarRol.Name = "btnEliminarRol";
            this.btnEliminarRol.Size = new System.Drawing.Size(380, 34);
            this.btnEliminarRol.TabIndex = 3;
            this.btnEliminarRol.Text = "Eliminar rol seleccionado";
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);
            //
            // gbCrearRol
            //
            this.gbCrearRol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrearRol.Controls.Add(this.lblNombreRol);
            this.gbCrearRol.Controls.Add(this.txtNombreRol);
            this.gbCrearRol.Controls.Add(this.lblPatentesRol);
            this.gbCrearRol.Controls.Add(this.clbPatentesRol);
            this.gbCrearRol.Controls.Add(this.lblFamiliasRol);
            this.gbCrearRol.Controls.Add(this.clbFamiliasRol);
            this.gbCrearRol.Controls.Add(this.btnGuardarRol);
            this.gbCrearRol.Controls.Add(this.btnLimpiarRol);
            this.gbCrearRol.Font = fuenteGroup;
            this.gbCrearRol.ForeColor = azulOscuro;
            this.gbCrearRol.Location = new System.Drawing.Point(410, 50);
            this.gbCrearRol.Name = "gbCrearRol";
            this.gbCrearRol.Size = new System.Drawing.Size(467, 510);
            this.gbCrearRol.TabIndex = 4;
            this.gbCrearRol.TabStop = false;
            this.gbCrearRol.Text = "Crear nuevo rol";
            //
            // lblNombreRol
            //
            this.lblNombreRol.AutoSize = true;
            this.lblNombreRol.Font = fuenteBase;
            this.lblNombreRol.ForeColor = azulOscuro;
            this.lblNombreRol.Location = new System.Drawing.Point(20, 35);
            this.lblNombreRol.Text = "Nombre:";
            //
            // txtNombreRol
            //
            this.txtNombreRol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombreRol.Font = fuenteBase;
            this.txtNombreRol.Location = new System.Drawing.Point(85, 32);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(362, 23);
            this.txtNombreRol.TabIndex = 0;
            //
            // lblPatentesRol
            //
            this.lblPatentesRol.AutoSize = true;
            this.lblPatentesRol.Font = fuenteBase;
            this.lblPatentesRol.ForeColor = azulOscuro;
            this.lblPatentesRol.Location = new System.Drawing.Point(20, 70);
            this.lblPatentesRol.Text = "Patentes individuales:";
            //
            // clbPatentesRol
            //
            this.clbPatentesRol.BackColor = blanco;
            this.clbPatentesRol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbPatentesRol.CheckOnClick = true;
            this.clbPatentesRol.Font = fuenteBase;
            this.clbPatentesRol.Location = new System.Drawing.Point(20, 90);
            this.clbPatentesRol.Name = "clbPatentesRol";
            this.clbPatentesRol.Size = new System.Drawing.Size(427, 156);
            this.clbPatentesRol.TabIndex = 1;
            //
            // lblFamiliasRol
            //
            this.lblFamiliasRol.AutoSize = true;
            this.lblFamiliasRol.Font = fuenteBase;
            this.lblFamiliasRol.ForeColor = azulOscuro;
            this.lblFamiliasRol.Location = new System.Drawing.Point(20, 258);
            this.lblFamiliasRol.Text = "Familias:";
            //
            // clbFamiliasRol
            //
            this.clbFamiliasRol.BackColor = blanco;
            this.clbFamiliasRol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbFamiliasRol.CheckOnClick = true;
            this.clbFamiliasRol.Font = fuenteBase;
            this.clbFamiliasRol.Location = new System.Drawing.Point(20, 278);
            this.clbFamiliasRol.Name = "clbFamiliasRol";
            this.clbFamiliasRol.Size = new System.Drawing.Size(427, 156);
            this.clbFamiliasRol.TabIndex = 2;
            //
            // btnGuardarRol
            //
            EstilarBotonPrimario(this.btnGuardarRol, azulOscuro, blanco, fuenteBtn);
            this.btnGuardarRol.Location = new System.Drawing.Point(20, 455);
            this.btnGuardarRol.Name = "btnGuardarRol";
            this.btnGuardarRol.Size = new System.Drawing.Size(140, 36);
            this.btnGuardarRol.TabIndex = 3;
            this.btnGuardarRol.Text = "Guardar";
            this.btnGuardarRol.Click += new System.EventHandler(this.btnGuardarRol_Click);
            //
            // btnLimpiarRol
            //
            EstilarBotonSecundario(this.btnLimpiarRol, azulOscuro, blanco, fuenteBtn);
            this.btnLimpiarRol.Location = new System.Drawing.Point(175, 455);
            this.btnLimpiarRol.Name = "btnLimpiarRol";
            this.btnLimpiarRol.Size = new System.Drawing.Size(140, 36);
            this.btnLimpiarRol.TabIndex = 4;
            this.btnLimpiarRol.Text = "Limpiar";
            this.btnLimpiarRol.Click += new System.EventHandler(this.btnLimpiarRol_Click);
            //
            // FRMGestionPermisos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = azulClaro;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabControl);
            this.Font = fuenteBase;
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

        // ─── Helpers de estilo (Material Design) ───
        private static void EstilarGrilla(System.Windows.Forms.DataGridView dgv,
            System.Drawing.Color azulOscuro, System.Drawing.Color azulClaro,
            System.Drawing.Color blanco, System.Drawing.Font fuente)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = blanco;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = azulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = blanco;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = azulOscuro;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(4);
            dgv.ColumnHeadersHeight = 32;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.BackColor = blanco;
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgv.DefaultCellStyle.Font = fuente;
            dgv.DefaultCellStyle.SelectionBackColor = azulOscuro;
            dgv.DefaultCellStyle.SelectionForeColor = blanco;
            dgv.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = azulClaro;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = azulClaro;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 26;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        }

        private static void EstilarBotonPrimario(System.Windows.Forms.Button btn,
            System.Drawing.Color azulOscuro, System.Drawing.Color blanco, System.Drawing.Font fuente)
        {
            btn.BackColor = azulOscuro;
            btn.ForeColor = blanco;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = fuente;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private static void EstilarBotonSecundario(System.Windows.Forms.Button btn,
            System.Drawing.Color azulOscuro, System.Drawing.Color blanco, System.Drawing.Font fuente)
        {
            btn.BackColor = blanco;
            btn.ForeColor = azulOscuro;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = azulOscuro;
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = fuente;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        #endregion
    }
}
