using System;
using System.Windows;

namespace Semana06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Este es el botón "Cantdad" (que realiza el cálculo)
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validamos que los datos sean correctos
                if (double.TryParse(txtMontoPagarEntrada.Text, out double montoDeuda) &&
                    dpVencimiento.SelectedDate.HasValue && dpPago.SelectedDate.HasValue)
                {
                    // Cálculo de días de mora
                    int diasMora = (dpPago.SelectedDate.Value - dpVencimiento.SelectedDate.Value).Days;

                    // Si la fecha de pago es antes que la de vencimiento, no hay mora
                    if (diasMora < 0) diasMora = 0;

                    // Cálculos financieros
                    double porcentajeTotal = diasMora * 0.5; // 0.5% por día
                    double moraSoles = montoDeuda * (porcentajeTotal / 100);
                    double totalPagar = montoDeuda + moraSoles;

                    // Asignación de resultados a los TextBoxes
                    txtDiasMora.Text = diasMora.ToString();
                    txtMoraPorcentaje.Text = porcentajeTotal.ToString("F2") + "%";
                    txtMoraSoles.Text = moraSoles.ToString("F2");
                    txtTotalPagar.Text = totalPagar.ToString("F2");
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un monto válido y selecciona ambas fechas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Este es el botón "Nuevo" (limpia todo el formulario)
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            txtCliente.Clear();
            txtMontoPagarEntrada.Clear();
            txtDiasMora.Clear();
            txtMoraPorcentaje.Clear();
            txtMoraSoles.Clear();
            txtTotalPagar.Clear();
            dpVencimiento.SelectedDate = null;
            dpPago.SelectedDate = null;
            txtCliente.Focus(); // Pone el cursor en Cliente para empezar de nuevo
        }

        // Este es el botón "Finalizar" (cierra la aplicación)
        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}