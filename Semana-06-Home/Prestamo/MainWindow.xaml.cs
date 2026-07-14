using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prestamo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            //VALIDAR NOMBRE DEL CLIENTE
            if (string.IsNullOrEmpty(txtCliente.Text))
            {
                MessageBox.Show("Ingrese el nombre del cliente", "Cliente Invalido", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCliente.Focus();
                return;
            }
            string cliente = txtCliente.Text;

            //VALIDAR MONTO A PAGAR 
            if (!double.TryParse(txtMontoPagar.Text, out double montoPagar) || montoPagar <= 0)
            {
                MessageBox.Show("Ingrese un Monto válido mayou a 0" , "Monto Inválido" , MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMontoPagar.Focus();
                return;
            }

            //VALIDAR LAS FECHAS PARA EVITAR QUE LA APP CAIGA
            if (!dtpFechaVen.SelectedDate.HasValue || !dtpFechaPago.SelectedDate.HasValue)
            {
                MessageBox.Show("Selecione el rango de fechas", "Fechas faltantes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            DateTime fechaVencimento = dtpFechaVen.SelectedDate.Value;
            DateTime fechaPago = dtpFechaPago.SelectedDate.Value;

            //CALCULAR DIAS DE MORA
            int diasMora = 0;
            if(fechaPago > fechaVencimento)
            {
                TimeSpan diferencia = fechaPago - fechaVencimento;
                diasMora = (int)diferencia.TotalDays;
            }

            //CÁLCULO DE MORA
            double moraPorcentaje = diasMora * 0.5;
            double moraSoles = montoPagar * moraPorcentaje / 100;
            double totalPagar = montoPagar + moraSoles;

            //MOSTRAR RESULTADOS CON FORMATO DE DECIMALES
            txtDiasMora.Text = diasMora.ToString();
            txtMoraPorcentaje.Text = moraPorcentaje.ToString();
            txtMoraSoles.Text = moraSoles.ToString();
            txtMontoTotal.Text = totalPagar.ToString();



        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            //LIMPIAR CONTROLES DE ENTRADA Y SALIDA ORDENADAMENTE 
            txtCliente.Clear();
            txtMontoPagar.Clear();
            dtpFechaPago.SelectedDate = null;
            dtpFechaVen.SelectedDate = null;

            txtDiasMora.Clear();
            txtMoraPorcentaje.Clear();
            txtMoraSoles.Clear();
            txtMontoTotal.Clear();

            txtCliente.Focus();
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}