namespace PROYECTO_ING_DE_SOFTWARE
{
    partial class FRMBitacoraDeEventos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblModulo = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.cboModulo = new System.Windows.Forms.ComboBox();
            this.cboEvento = new System.Windows.Forms.ComboBox();
            this.lblEvento = new System.Windows.Forms.Label();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.cboCriticidad = new System.Windows.Forms.ComboBox();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.lblCriticidad = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtApellidoUsuario = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBitacora
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.dgvBitacora.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBitacora.BackgroundColor = System.Drawing.Color.White;
            this.dgvBitacora.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBitacora.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBitacora.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBitacora.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBitacora.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBitacora.EnableHeadersVisualStyles = false;
            this.dgvBitacora.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            this.dgvBitacora.Location = new System.Drawing.Point(48, 12);
            this.dgvBitacora.MultiSelect = false;
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.ReadOnly = true;
            this.dgvBitacora.RowHeadersVisible = false;
            this.dgvBitacora.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBitacora.Size = new System.Drawing.Size(644, 213);
            this.dgvBitacora.TabIndex = 0;
            this.dgvBitacora.SelectionChanged += new System.EventHandler(this.dgvBitacora_SelectionChanged);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnLimpiar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnLimpiar.Location = new System.Drawing.Point(32, 415);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(85, 30);
            this.btnLimpiar.TabIndex = 1;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnAplicar
            // 
            this.btnAplicar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnAplicar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAplicar.FlatAppearance.BorderSize = 0;
            this.btnAplicar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnAplicar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnAplicar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAplicar.ForeColor = System.Drawing.Color.White;
            this.btnAplicar.Location = new System.Drawing.Point(235, 415);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(85, 30);
            this.btnAplicar.TabIndex = 2;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = false;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.btnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImprimir.FlatAppearance.BorderSize = 0;
            this.btnImprimir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnImprimir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(463, 415);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(85, 30);
            this.btnImprimir.TabIndex = 3;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblLogin.Location = new System.Drawing.Point(94, 307);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(41, 17);
            this.lblLogin.TabIndex = 4;
            this.lblLogin.Text = "Login";
            // 
            // lblModulo
            // 
            this.lblModulo.AutoSize = true;
            this.lblModulo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblModulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblModulo.Location = new System.Drawing.Point(94, 366);
            this.lblModulo.Name = "lblModulo";
            this.lblModulo.Size = new System.Drawing.Size(55, 17);
            this.lblModulo.TabIndex = 5;
            this.lblModulo.Text = "Modulo";
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.White;
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogin.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.txtLogin.Location = new System.Drawing.Point(135, 300);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(100, 24);
            this.txtLogin.TabIndex = 6;
            // 
            // cboModulo
            // 
            this.cboModulo.BackColor = System.Drawing.Color.White;
            this.cboModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModulo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboModulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboModulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.cboModulo.FormattingEnabled = true;
            this.cboModulo.Location = new System.Drawing.Point(153, 363);
            this.cboModulo.Name = "cboModulo";
            this.cboModulo.Size = new System.Drawing.Size(100, 25);
            this.cboModulo.TabIndex = 7;
            // 
            // cboEvento
            // 
            this.cboEvento.BackColor = System.Drawing.Color.White;
            this.cboEvento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEvento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboEvento.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboEvento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.cboEvento.FormattingEnabled = true;
            this.cboEvento.Location = new System.Drawing.Point(592, 303);
            this.cboEvento.Name = "cboEvento";
            this.cboEvento.Size = new System.Drawing.Size(120, 25);
            this.cboEvento.TabIndex = 11;
            // 
            // lblEvento
            // 
            this.lblEvento.AutoSize = true;
            this.lblEvento.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblEvento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblEvento.Location = new System.Drawing.Point(540, 306);
            this.lblEvento.Name = "lblEvento";
            this.lblEvento.Size = new System.Drawing.Size(50, 17);
            this.lblEvento.TabIndex = 9;
            this.lblEvento.Text = "Evento";
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFechaInicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblFechaInicio.Location = new System.Drawing.Point(259, 303);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(79, 17);
            this.lblFechaInicio.TabIndex = 8;
            this.lblFechaInicio.Text = "Fecha Inicio";
            // 
            // cboCriticidad
            // 
            this.cboCriticidad.BackColor = System.Drawing.Color.White;
            this.cboCriticidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCriticidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCriticidad.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboCriticidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.cboCriticidad.FormattingEnabled = true;
            this.cboCriticidad.Location = new System.Drawing.Point(606, 367);
            this.cboCriticidad.Name = "cboCriticidad";
            this.cboCriticidad.Size = new System.Drawing.Size(100, 25);
            this.cboCriticidad.TabIndex = 14;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFechaFin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblFechaFin.Location = new System.Drawing.Point(259, 370);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(65, 17);
            this.lblFechaFin.TabIndex = 13;
            this.lblFechaFin.Text = "Fecha Fin";
            // 
            // lblCriticidad
            // 
            this.lblCriticidad.AutoSize = true;
            this.lblCriticidad.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCriticidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblCriticidad.Location = new System.Drawing.Point(536, 373);
            this.lblCriticidad.Name = "lblCriticidad";
            this.lblCriticidad.Size = new System.Drawing.Size(64, 17);
            this.lblCriticidad.TabIndex = 12;
            this.lblCriticidad.Text = "Criticidad";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.btnCancelar.Location = new System.Drawing.Point(702, 415);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 30);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Salir";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaInicio.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.dtpFechaInicio.CalendarMonthBackground = System.Drawing.Color.White;
            this.dtpFechaInicio.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.dtpFechaInicio.CalendarTitleForeColor = System.Drawing.Color.White;
            this.dtpFechaInicio.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            this.dtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(334, 301);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(200, 24);
            this.dtpFechaInicio.TabIndex = 17;
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaFin.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.dtpFechaFin.CalendarMonthBackground = System.Drawing.Color.White;
            this.dtpFechaFin.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.dtpFechaFin.CalendarTitleForeColor = System.Drawing.Color.White;
            this.dtpFechaFin.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(222)))), ((int)(((byte)(251)))));
            this.dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(330, 368);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(200, 24);
            this.dtpFechaFin.TabIndex = 18;
            // 
            // txtNombreUsuario
            // 
            this.txtNombreUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.txtNombreUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombreUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNombreUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.txtNombreUsuario.Location = new System.Drawing.Point(247, 250);
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.ReadOnly = true;
            this.txtNombreUsuario.Size = new System.Drawing.Size(150, 24);
            this.txtNombreUsuario.TabIndex = 20;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblNombre.Location = new System.Drawing.Point(166, 253);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(58, 17);
            this.lblNombre.TabIndex = 19;
            this.lblNombre.Text = "Nombre";
            // 
            // txtApellidoUsuario
            // 
            this.txtApellidoUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.txtApellidoUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApellidoUsuario.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtApellidoUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.txtApellidoUsuario.Location = new System.Drawing.Point(503, 250);
            this.txtApellidoUsuario.Name = "txtApellidoUsuario";
            this.txtApellidoUsuario.ReadOnly = true;
            this.txtApellidoUsuario.Size = new System.Drawing.Size(150, 24);
            this.txtApellidoUsuario.TabIndex = 22;
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblApellido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblApellido.Location = new System.Drawing.Point(429, 252);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(57, 17);
            this.lblApellido.TabIndex = 21;
            this.lblApellido.Text = "Apellido";
            // 
            // FRMBitacoraDeEventos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 470);
            this.Controls.Add(this.txtApellidoUsuario);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtNombreUsuario);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.dtpFechaInicio);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.cboCriticidad);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.lblCriticidad);
            this.Controls.Add(this.cboEvento);
            this.Controls.Add(this.lblEvento);
            this.Controls.Add(this.lblFechaInicio);
            this.Controls.Add(this.cboModulo);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblModulo);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.dgvBitacora);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FRMBitacoraDeEventos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitácora de Eventos";
            this.Load += new System.EventHandler(this.FRMBitacoraDeEventos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBitacora;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.ComboBox cboModulo;
        private System.Windows.Forms.ComboBox cboEvento;
        private System.Windows.Forms.Label lblEvento;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.ComboBox cboCriticidad;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.Label lblCriticidad;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtApellidoUsuario;
        private System.Windows.Forms.Label lblApellido;
    }
}
