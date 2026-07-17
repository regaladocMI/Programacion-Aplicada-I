using System;
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

namespace CentroOdontologico
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            calCita.SelectedDate = DateTime.Today; // Selecciona la fecha actual por defecto al abrir
        }

        private void btnCronograma_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar que el campo del paciente no esté vacío o en blanco
            if (string.IsNullOrWhiteSpace(txtPaciente.Text))
            {
                MessageBox.Show("Ingrese un paciente válido", "Validar Paciente", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPaciente.Focus();
                return;
            }
            string paciente = txtPaciente.Text.Trim();

            // 2. Validar que se haya seleccionado un tratamiento
            if (cbxTratamiento.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un tratamiento", "Tratamiento Faltante", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbxTratamiento.Focus();
                return;
            }

            // 3. Validar que se haya seleccionado una pieza dental
            if (cbxPiezaDental.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una pieza dental", "Pieza Dental Faltante", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbxPiezaDental.Focus();
                return;
            }

            // Extracción segura del contenido de los ComboBox
            string tratamiento = ((ComboBoxItem)cbxTratamiento.SelectedItem).Content.ToString();
            string pieza = ((ComboBoxItem)cbxPiezaDental.SelectedItem).Content.ToString();

            // 4. Validar que el calendario tenga una fecha seleccionada
            if (!calCita.SelectedDate.HasValue)
            {
                MessageBox.Show("Debe seleccionar una fecha en el calendario", "Fecha Faltante", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime fechaCita = calCita.SelectedDate.Value;
            DateTime citaProxima = fechaCita.AddDays(15);

            // 5. Construcción del reporte formateado
            string reporte = $"REPORTE DE CITA ODONTOLÓGICA\n" +
                             $"========================================\n" +
                             $"• Paciente: {paciente}\n" +
                             $"• Tratamiento: {tratamiento}\n" +
                             $"• Pieza Dental: {pieza}\n" +
                             $"• Fecha de Cita: {fechaCita:dd/MM/yyyy}\n" +
                             $"----------------------------------------\n" +
                             $"• Próxima Cita Sugerida: {citaProxima:dd/MM/yyyy}";

            // Mostrar el resultado en el TextBox de reporte
            txtReporte.Text = reporte;
        }
    }
}