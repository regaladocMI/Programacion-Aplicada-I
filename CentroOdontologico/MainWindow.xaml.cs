using System.Windows;

namespace CentroOdontologico
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCronagrenar_Click(object sender, RoutedEventArgs e)
        {
            // Validar si hay fecha seleccionada
            if (calCita.SelectedDate.HasValue)
            {
                string paciente = txtPaciente.Text;
                string tratamiento = cmbTratamiento.Text;
                string fecha = calCita.SelectedDate.Value.ToShortDateString();

                // Agregar al listado
                string registro = $"Paciente: {paciente} | Tratamiento: {tratamiento} | Fecha: {fecha}";
                lstCitas.Items.Add(registro);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fecha en el calendario.");
            }
        }
    }
}