using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            AbrirPuertoSerial(); // Abrir el puerto serial al cargar el formulario
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
        }

        private void AbrirPuertoSerial()
        {
            try
            {
                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.Open();
                }
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

        private ValidacionResultado ValidarEntradas()
        {
            int idBomba = 0;
            string tipoAbastecimiento = string.Empty;
            string nombreCliente = string.Empty;
            double montoPrepago = 0;

            // Validar ID de bomba
            if (!int.TryParse(txtIdBomba.Text, out idBomba) || idBomba <= 0 || !panelCentral.Bombas.Any(b => b.ID == idBomba))
            {
                MessageBox.Show("Por favor, introduce un ID de bomba válido.");
                return new ValidacionResultado { EsValido = false };
            }

            // Validar tipo de abastecimiento
            if (cmbTipoAbastecimiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un tipo de abastecimiento.");
                return new ValidacionResultado { EsValido = false };
            }
            tipoAbastecimiento = cmbTipoAbastecimiento.SelectedItem.ToString();

            // Validar nombre del cliente
            nombreCliente = txtNombreCliente.Text;
            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                MessageBox.Show("Por favor, introduce un nombre de cliente.");
                return new ValidacionResultado { EsValido = false };
            }

            // Validar monto de prepago si aplica
            if (tipoAbastecimiento == "Prepago")
            {
                if (!double.TryParse(txtMontoPrepago.Text, out montoPrepago) || montoPrepago <= 0)
                {
                    MessageBox.Show("Por favor, introduce un monto de prepago válido.");
                    return new ValidacionResultado { EsValido = false };
                }
            }

            return new ValidacionResultado
            {
                EsValido = true,
                IdBomba = idBomba,
                TipoAbastecimiento = tipoAbastecimiento,
                NombreCliente = nombreCliente,
                MontoPrepago = montoPrepago
            };
        }

        private void EnviarTipoAbastecimiento(string tipoAbastecimiento)
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    if (tipoAbastecimiento == "Tanque Lleno")
                    {
                        mySerialPort.WriteLine("TANQUE_LLENO");
                    }
                    else
                    {
                        mySerialPort.WriteLine(tipoAbastecimiento);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el tipo de abastecimiento al Arduino: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Close();
            }

            // Guardar transacciones al cerrar la aplicación
            panelCentral.GuardarTransacciones();
        }

        private async void btnIniciarAbastecimiento_Click(object sender, EventArgs e)
        {
            // Asegurarse de que el puerto esté abierto antes de cualquier operación
            AbrirPuertoSerial();

            var resultadoValidacion = ValidarEntradas();
            if (!resultadoValidacion.EsValido)
            {
                return;
            }

            // Enviar tipo de abastecimiento al Arduino
            EnviarTipoAbastecimiento(resultadoValidacion.TipoAbastecimiento);

            var transaccion = new TransaccionAbastecimiento
            {
                Fecha = DateTime.Now,
                NombreCliente = resultadoValidacion.NombreCliente,
                TipoAbastecimiento = resultadoValidacion.TipoAbastecimiento,
                PrecioPorLitro = 10.0,
                Cantidad = resultadoValidacion.TipoAbastecimiento == "Prepago" ? resultadoValidacion.MontoPrepago / 10.0 : 0,
                MontoPagado = resultadoValidacion.MontoPrepago,
                IdBomba = resultadoValidacion.IdBomba
            };

            panelCentral.AgregarTransaccion(transaccion);
            await panelCentral.OperarBombaAsync(resultadoValidacion.IdBomba, true);

            if (resultadoValidacion.TipoAbastecimiento == "Prepago" || resultadoValidacion.TipoAbastecimiento == "Tanque Lleno")
            {
                int pin = resultadoValidacion.IdBomba == 1 ? 7 : (resultadoValidacion.IdBomba == 2 ? 8 : 0);
                if (pin != 0)
                {
                    int tiempoEncendido = resultadoValidacion.TipoAbastecimiento == "Prepago" ? (int)(resultadoValidacion.MontoPrepago * 2) : 100; // Cambiar a 100 segundos para "Tanque Lleno"
                    string comando = $"LED,{pin},{tiempoEncendido}";

                    if (mySerialPort.IsOpen)
                    {
                        mySerialPort.WriteLine(comando);
                        await Task.Delay(tiempoEncendido * 1000); // Esperar 100 segundos
                        await panelCentral.OperarBombaAsync(resultadoValidacion.IdBomba, false);
                        mySerialPort.WriteLine("LED,0,0");
                    }
                    else
                    {
                        MessageBox.Show("El puerto serial no está abierto.");
                    }
                }
                else
                {
                    MessageBox.Show("ID de bomba no válido para encender LED.");
                }
            }
            else
            {
                MessageBox.Show("Solo los tipos de abastecimiento 'Prepago' y 'Tanque Lleno' son compatibles para encender el LED.");
            }
        }

        private async void btnTerminarAbastecimiento_Click(object sender, EventArgs e)
        {
            // Asegurarse de que el puerto esté abierto antes de cualquier operación
            AbrirPuertoSerial();

            if (!int.TryParse(txtIdBomba.Text, out int idBomba))
            {
                MessageBox.Show("Por favor, introduce un ID de bomba válido.");
                return;
            }
            await panelCentral.OperarBombaAsync(idBomba, false);
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
                {"Total Transacciones", transaccionesFiltradas.Count},
                {"Total Litros Abastecidos", transaccionesFiltradas.Sum(t => t.Cantidad)},
                {"Total Ingresos", transaccionesFiltradas.Sum(t => t.MontoPagado)},
                {"Tota lPrepago", transaccionesFiltradas.Where(t => t.TipoAbastecimiento == "Prepago").Sum(t => t.MontoPagado)},
                {"Total TanqueLleno", transaccionesFiltradas.Where(t => t.TipoAbastecimiento == "Tanque Lleno").Sum(t => t.MontoPagado)}
            };

            MostrarEstadisticas(estadisticasFiltradas);
        }

        private void btnResetearDatos_Click(object sender, EventArgs e)
        {
            panelCentral.ResetearDatos();
            MessageBox.Show("Los datos han sido reseteados.");
        }

        private void btnCierreDiario_Click(object sender, EventArgs e)
        {
            var cierreDiario = Estadisticas.GenerarCierreDiario(panelCentral.Transacciones);
            MostrarEstadisticas(cierreDiario);
        }

        private void btnInformePrepago_Click(object sender, EventArgs e)
        {
            var informePrepago = Estadisticas.GenerarInformePrepago(panelCentral.Transacciones);
            MostrarEstadisticas(informePrepago);
        }

        private void btnInformeTanqueLleno_Click(object sender, EventArgs e)
        {
            var informeTanqueLleno = Estadisticas.GenerarInformeTanqueLleno(panelCentral.Transacciones);
            MostrarEstadisticas(informeTanqueLleno);
        }

        private void btnBombaMasUtilizada_Click(object sender, EventArgs e)
        {
            var bombaMasUtilizada = Estadisticas.ObtenerBombaMasUtilizada(panelCentral.Transacciones);
            MessageBox.Show($"Bomba más utilizada: {bombaMasUtilizada}");
        }

        private void btnBombaMenosUtilizada_Click(object sender, EventArgs e)
        {
            var bombaMenosUtilizada = Estadisticas.ObtenerBombaMenosUtilizada(panelCentral.Transacciones);
            MessageBox.Show($"Bomba menos utilizada: {bombaMenosUtilizada}");
        }

        private void txtIdBomba_TextChanged(object sender, EventArgs e) { }

        private void txtMontoPrepago_TextChanged(object sender, EventArgs e) { }

        private void cmbTipoAbastecimiento_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
