namespace Gasolinera
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPagePresentacion;
        private System.Windows.Forms.TabPage tabPageAbastecimiento;
        private System.Windows.Forms.TabPage tabPageEstadisticas;
        private System.Windows.Forms.Button btnIniciarAbastecimiento;
        private System.Windows.Forms.Button btnTerminarAbastecimiento;
        private System.Windows.Forms.Button btnResetearDatos;
        private System.Windows.Forms.TextBox txtIdBomba;
        private System.Windows.Forms.TextBox txtNombreCliente;
        private System.Windows.Forms.TextBox txtMontoPrepago;
        private System.Windows.Forms.ComboBox cmbTipoAbastecimiento;
        private System.Windows.Forms.DateTimePicker dateTimePickerInicio;
        private System.Windows.Forms.DateTimePicker dateTimePickerFin;
        private System.Windows.Forms.Button btnFiltrarEstadisticas;
        private System.Windows.Forms.DataGridView dataGridViewEstadisticas;
        private System.Windows.Forms.Label lblPresentacion;
        private System.Windows.Forms.Button btnMostrarEstadisticas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabPagePresentacion = new TabPage();
            lblPresentacion = new Label();
            tabPageAbastecimiento = new TabPage();
            btnIniciarAbastecimiento = new Button();
            btnTerminarAbastecimiento = new Button();
            btnResetearDatos = new Button();
            txtIdBomba = new TextBox();
            txtNombreCliente = new TextBox();
            txtMontoPrepago = new TextBox();
            cmbTipoAbastecimiento = new ComboBox();
            tabPageEstadisticas = new TabPage();
            dateTimePickerInicio = new DateTimePicker();
            dateTimePickerFin = new DateTimePicker();
            btnFiltrarEstadisticas = new Button();
            dataGridViewEstadisticas = new DataGridView();
            btnMostrarEstadisticas = new Button();
            tabControl.SuspendLayout();
            tabPagePresentacion.SuspendLayout();
            tabPageAbastecimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEstadisticas).BeginInit();
            tabPageEstadisticas.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPagePresentacion);
            tabControl.Controls.Add(tabPageAbastecimiento);
            tabControl.Controls.Add(tabPageEstadisticas);
            tabControl.Location = new Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(460, 337);
            tabControl.TabIndex = 0;
            // 
            // tabPagePresentacion
            // 
            tabPagePresentacion.Controls.Add(lblPresentacion);
            tabPagePresentacion.Location = new Point(4, 22);
            tabPagePresentacion.Name = "tabPagePresentacion";
            tabPagePresentacion.Padding = new Padding(3);
            tabPagePresentacion.Size = new Size(452, 311);
            tabPagePresentacion.TabIndex = 0;
            tabPagePresentacion.Text = "Presentación";
            tabPagePresentacion.UseVisualStyleBackColor = true;
            // 
            // lblPresentacion
            // 
            lblPresentacion.AutoSize = true;
            lblPresentacion.Location = new Point(50, 50);
            lblPresentacion.Name = "lblPresentacion";
            lblPresentacion.Size = new Size(350, 150);
            lblPresentacion.TabIndex = 0;
            lblPresentacion.Text = "Bienvenido a la aplicación Gasolinera.\n\nUse las pestañas para navegar por la aplicación.\n\nEn la segunda pestaña, puede realizar operaciones de abastecimiento.\n\nEn la tercera pestaña, puede visualizar las estadísticas.";
            // 
            // tabPageAbastecimiento
            // 
            tabPageAbastecimiento.Controls.Add(btnIniciarAbastecimiento);
            tabPageAbastecimiento.Controls.Add(btnTerminarAbastecimiento);
            tabPageAbastecimiento.Controls.Add(btnResetearDatos);
            tabPageAbastecimiento.Controls.Add(txtIdBomba);
            tabPageAbastecimiento.Controls.Add(txtNombreCliente);
            tabPageAbastecimiento.Controls.Add(txtMontoPrepago);
            tabPageAbastecimiento.Controls.Add(cmbTipoAbastecimiento);
            tabPageAbastecimiento.Location = new Point(4, 22);
            tabPageAbastecimiento.Name = "tabPageAbastecimiento";
            tabPageAbastecimiento.Padding = new Padding(3);
            tabPageAbastecimiento.Size = new Size(452, 311);
            tabPageAbastecimiento.TabIndex = 1;
            tabPageAbastecimiento.Text = "Abastecimiento";
            tabPageAbastecimiento.UseVisualStyleBackColor = true;
            // 
            // btnIniciarAbastecimiento
            // 
            btnIniciarAbastecimiento.Location = new Point(50, 250);
            btnIniciarAbastecimiento.Size = new Size(150, 40);
            btnIniciarAbastecimiento.Text = "Iniciar Abastecimiento";
            btnIniciarAbastecimiento.Click += btnIniciarAbastecimiento_Click;
            // 
            // btnTerminarAbastecimiento
            // 
            btnTerminarAbastecimiento.Location = new Point(250, 250);
            btnTerminarAbastecimiento.Size = new Size(150, 40);
            btnTerminarAbastecimiento.Text = "Terminar Abastecimiento";
            btnTerminarAbastecimiento.Click += btnTerminarAbastecimiento_Click;
            // 
            // btnResetearDatos
            // 
            btnResetearDatos.Location = new Point(250, 200);
            btnResetearDatos.Size = new Size(150, 40);
            btnResetearDatos.Text = "Resetear Datos";
            btnResetearDatos.Click += btnResetearDatos_Click;
            // 
            // txtIdBomba
            // 
            txtIdBomba.Location = new Point(50, 50);
            txtIdBomba.Size = new Size(100, 20);
            txtIdBomba.PlaceholderText = "ID Bomba";
            // 
            // txtNombreCliente
            // 
            txtNombreCliente.Location = new Point(50, 100);
            txtNombreCliente.Size = new Size(200, 20);
            txtNombreCliente.PlaceholderText = "Nombre del Cliente";
            // 
            // txtMontoPrepago
            // 
            txtMontoPrepago.Location = new Point(50, 150);
            txtMontoPrepago.Size = new Size(100, 20);
            txtMontoPrepago.PlaceholderText = "Monto Prepago";
            // 
            // cmbTipoAbastecimiento
            // 
            cmbTipoAbastecimiento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoAbastecimiento.Items.AddRange(new object[] { "Prepago", "Tanque Lleno" });
            cmbTipoAbastecimiento.Location = new Point(50, 200);
            cmbTipoAbastecimiento.Size = new Size(121, 21);
            // 
            // tabPageEstadisticas
            // 
            tabPageEstadisticas.Controls.Add(dateTimePickerInicio);
            tabPageEstadisticas.Controls.Add(dateTimePickerFin);
            tabPageEstadisticas.Controls.Add(btnFiltrarEstadisticas);
            tabPageEstadisticas.Controls.Add(dataGridViewEstadisticas);
            tabPageEstadisticas.Controls.Add(btnMostrarEstadisticas);
            tabPageEstadisticas.Location = new Point(4, 22);
            tabPageEstadisticas.Name = "tabPageEstadisticas";
            tabPageEstadisticas.Padding = new Padding(3);
            tabPageEstadisticas.Size = new Size(452, 311);
            tabPageEstadisticas.TabIndex = 2;
            tabPageEstadisticas.Text = "Estadísticas";
            tabPageEstadisticas.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerInicio
            // 
            dateTimePickerInicio.Location = new Point(50, 20);
            dateTimePickerInicio.Name = "dateTimePickerInicio";
            dateTimePickerInicio.Size = new Size(200, 20);
            dateTimePickerInicio.TabIndex = 1;
            // 
            // dateTimePickerFin
            // 
            dateTimePickerFin.Location = new Point(50, 60);
            dateTimePickerFin.Name = "dateTimePickerFin";
            dateTimePickerFin.Size = new Size(200, 20);
            dateTimePickerFin.TabIndex = 2;
            // 
            // btnFiltrarEstadisticas
            // 
            btnFiltrarEstadisticas.Location = new Point(300, 40);
            btnFiltrarEstadisticas.Name = "btnFiltrarEstadisticas";
            btnFiltrarEstadisticas.Size = new Size(100, 30);
            btnFiltrarEstadisticas.TabIndex = 3;
            btnFiltrarEstadisticas.Text = "Filtrar Estadísticas";
            btnFiltrarEstadisticas.UseVisualStyleBackColor = true;
            btnFiltrarEstadisticas.Click += btnFiltrarEstadisticas_Click;
            // 
            // dataGridViewEstadisticas
            // 
            dataGridViewEstadisticas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEstadisticas.Location = new Point(50, 100);
            dataGridViewEstadisticas.Name = "dataGridViewEstadisticas";
            dataGridViewEstadisticas.Size = new Size(350, 160);
            dataGridViewEstadisticas.TabIndex = 4;
            // 
            // btnMostrarEstadisticas
            // 
            btnMostrarEstadisticas.Location = new Point(50, 270);
            btnMostrarEstadisticas.Size = new Size(150, 40);
            btnMostrarEstadisticas.Text = "Mostrar Estadísticas";
            btnMostrarEstadisticas.Click += btnMostrarEstadisticas_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 361);
            Controls.Add(tabControl);
            Name = "Form1";
            Text = "Gasolinera";
            tabControl.ResumeLayout(false);
            tabPagePresentacion.ResumeLayout(false);
            tabPagePresentacion.PerformLayout();
            tabPageAbastecimiento.ResumeLayout(false);
            tabPageAbastecimiento.PerformLayout();
            tabPageEstadisticas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewEstadisticas).EndInit();
            ResumeLayout(false);
        }
    }
}
