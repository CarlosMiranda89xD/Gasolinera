using System;
using System.Windows.Forms;
using System.IO.Ports;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Gasolinera
{
    public partial class Form1 : Form
    {
        private PanelCentral panelCentral;
        private SerialPort mySerialPort;

        public Form1()
        {
            InitializeComponent();
            panelCentral = new PanelCentral();
            ConfigurarPuertoSerial();
        }

        private void ConfigurarPuertoSerial()
        {
            mySerialPort = new SerialPort("COM3");
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += MySerialPort_DataReceived;
            try
            {
                mySerialPort.Open();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Acceso denegado al puerto COM3. Verifica los permisos y que el puerto no esté en uso.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el puerto serial: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = mySerialPort.ReadLine();
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show($"Tiempo LED encendido: {data} segundos.");
            });
        }

        private async void btnIniciarAbastecimiento_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdBomba.Text, out int idBomba))
            {
                MessageBox.Show("Por favor, introduce un ID de bomba válido.");
                return;
            }

            string tipoAbastecimiento = cmbTipoAbastecimiento.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(tipoAbastecimiento))
            {
                MessageBox.Show("Por favor, selecciona un tipo de abastecimiento.");
                return;
            }

            string nombreCliente = txtNombreCliente.Text;
            double montoPrepago = string.IsNullOrEmpty(txtMontoPrepago.Text) ? 0 : double.Parse(txtMontoPrepago.Text);

            var transaccion = new TransaccionAbastecimiento
            {
                Fecha = DateTime.Now,
                NombreCliente = nombreCliente,
                TipoAbastecimiento = tipoAbastecimiento,
                PrecioPorLitro = 10.0,
                Cantidad = tipoAbastecimiento == "Prepago" ? montoPrepago / 10.0 : 0,
                MontoPagado = montoPrepago,
                IdBomba = idBomba
            };

            panelCentral.AgregarTransaccion(transaccion);
            await panelCentral.OperarBombaAsync(idBomba, true);

            if (tipoAbastecimiento == "Prepago")
            {
                int pin = idBomba == 1 ? 7 : (idBomba == 2 ? 8 : 0);
                if (pin != 0)
                {
                    int montoPrepagoMultiplicado = (int)(montoPrepago * 4);

                    string comando = $"LED,{pin},{montoPrepagoMultiplicado}";
                    mySerialPort.WriteLine(comando);
                    await Task.Delay(montoPrepagoMultiplicado * 1000);
                    await panelCentral.OperarBombaAsync(idBomba, false);
                    mySerialPort.WriteLine("LED,0,0");
                }
                else
                {
                    MessageBox.Show("ID de bomba no válido para encender LED.");
                }
            }
            else
            {
                MessageBox.Show("Solo el tipo de abastecimiento 'Prepago' es compatible para encender el LED.");
            }
        }

        private async void btnTerminarAbastecimiento_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdBomba.Text, out int idBomba))
            {
                MessageBox.Show("Por favor, introduce un ID de bomba válido.");
                return;
            }
            await panelCentral.OperarBombaAsync(idBomba, false);

            // Cierra la aplicación
            Application.Exit();
        }

        private void btnMostrarEstadisticas_Click(object sender, EventArgs e)
        {
            var estadisticas = panelCentral.ObtenerEstadisticas();
            MostrarEstadisticas(estadisticas);
        }

        private void btnFiltrarEstadisticas_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dateTimePickerInicio.Value;
            DateTime fechaFin = dateTimePickerFin.Value;

            var transaccionesFiltradas = panelCentral.Transacciones
                .Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
                .ToList();

            var estadisticasFiltradas = new Dictionary<string, object>
            {
                {"TotalTransacciones", transaccionesFiltradas.Count},
                {"TotalLitrosAbastecidos", transaccionesFiltradas.Sum(t => t.Cantidad)},
                {"TotalIngresos", transaccionesFiltradas.Sum(t => t.MontoPagado)},
                {"TotalPrepago", transaccionesFiltradas.Where(t => t.TipoAbastecimiento == "Prepago").Sum(t => t.MontoPagado)},
                {"TotalTanqueLleno", transaccionesFiltradas.Where(t => t.TipoAbastecimiento == "Tanque Lleno").Sum(t => t.MontoPagado)}
            };

            MostrarEstadisticas(estadisticasFiltradas);
        }

        private void MostrarEstadisticas(Dictionary<string, object> estadisticas)
        {
            dataGridViewEstadisticas.Rows.Clear();
            dataGridViewEstadisticas.Columns.Clear();

            dataGridViewEstadisticas.Columns.Add("Key", "Estadística");
            dataGridViewEstadisticas.Columns.Add("Value", "Valor");

            foreach (var estadistica in estadisticas)
            {
                dataGridViewEstadisticas.Rows.Add(estadistica.Key, estadistica.Value);
            }
        }

        private void btnResetearDatos_Click(object sender, EventArgs e)
        {
            panelCentral.ResetearDatos();
            MessageBox.Show("Los datos han sido reseteados.");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Close();
            }
        }

        private void txtIdBomba_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMontoPrepago_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbTipoAbastecimiento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
