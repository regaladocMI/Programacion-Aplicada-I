using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Conecta4
{
    // MainWindow ya NO contiene lógica de juego: solo traduce eventos de "Juego"
    // a cambios visuales, y clics de botones a llamadas al motor del juego.
    public partial class MainWindow : Window
    {
        private readonly Juego juego = new Juego();
        private readonly Ellipse[,] celdas = new Ellipse[Tablero.Filas, Tablero.Columnas];

        public MainWindow()
        {
            InitializeComponent();
            MapearCeldas();
            SuscribirEventosDelJuego();
            ActualizarVistaCompleta();
        }

        // Recorre los hijos de gridTablero (definidos en el XAML) y los guarda
        // en la matriz "celdas" según su Grid.Row / Grid.Column.
        private void MapearCeldas()
        {
            foreach (UIElement elemento in gridTablero.Children)
            {
                if (elemento is Ellipse elipse)
                {
                    int fila = Grid.GetRow(elipse);
                    int columna = Grid.GetColumn(elipse);
                    celdas[fila, columna] = elipse;
                }
            }
        }

        private void SuscribirEventosDelJuego()
        {
            juego.FichaColocada += (fila, columna, ficha) =>
            {
                celdas[fila, columna].Fill = ObtenerColor(ficha);
            };

            juego.TurnoCambiado += jugadorActual =>
            {
                txtEstado.Text = $"Turno: {jugadorActual.Nombre} "
                    + (jugadorActual.Ficha == Ficha.Jugador ? "(Rojo)" : "(Amarillo)");
            };

            juego.JuegoFinalizado += mensaje =>
            {
                txtEstado.Text = mensaje;
            };
        }

        private Brush ObtenerColor(Ficha ficha)
        {
            return ficha switch
            {
                Ficha.Jugador => Brushes.Red,
                Ficha.Maquina => Brushes.Yellow,
                _ => Brushes.White
            };
        }

        private void ActualizarVistaCompleta()
        {
            for (int f = 0; f < Tablero.Filas; f++)
                for (int c = 0; c < Tablero.Columnas; c++)
                    celdas[f, c].Fill = ObtenerColor(juego.Tablero.ObtenerFicha(f, c));

            txtEstado.Text = "Turno: Jugador (Rojo)";
        }

        private void BtnColumna_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;
            int columna = int.Parse(boton.Tag.ToString());
            juego.JugarColumnaHumano(columna);
        }

        private void BtnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            juego.Reiniciar();
            ActualizarVistaCompleta();
        }
    }
}