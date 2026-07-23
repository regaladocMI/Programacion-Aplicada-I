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
using System.Windows.Threading;
using System.IO;

namespace TragamonedasV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _clockTimer;
        private DispatcherTimer _gameTimer;
        private int _secondsElapsed;
        private int _score;
        private readonly Random _rand = new Random();
        private readonly string[] _imageFiles = { "1.png", "2.png", "3.png", "4.png", "5.png"};
        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
        }

        //RELOJ SUPERIOR 
        private void InitializeClock()
        {
            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (s, e) => UpdateClockText();
            UpdateClockText();
            _clockTimer.Start();
        }

        private void UpdateClockText()
        {
            TimerText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        // ---------- INICIO DEL JUEGO ----------
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            StartGame();
        }

        private void StartGame()
        {
            _secondsElapsed = 0;
            _score = 0;
            UpdateScoreText();
            ResultText.Text = string.Empty;

            if (_gameTimer == null)
            {
                _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _gameTimer.Tick += (s, e) => DoGameTick();
            }

            DoGameTick(); // primera tirada inmediata
            _gameTimer.Start();
        }

        private void StopGame()
        {
            if (_gameTimer != null && _gameTimer.IsEnabled)
                _gameTimer.Stop();
        }

        // ---------- LÓGICA POR SEGUNDO ----------
        private void DoGameTick()
        {
            _secondsElapsed++;

            int a = _rand.Next(_imageFiles.Length);
            int b = _rand.Next(_imageFiles.Length);
            int c = _rand.Next(_imageFiles.Length);

            SetImageSource(Img1, _imageFiles[a]);
            SetImageSource(Img2, _imageFiles[b]);
            SetImageSource(Img3, _imageFiles[c]);

            int puntosGanados = 0;
            if (a == b && b == c)
                puntosGanados = 20; // 3 iguales
            else if (a == b || a == c || b == c)
                puntosGanados = 10; // 2 iguales

            _score += puntosGanados;
            UpdateScoreText();

            if (_score >= 60)
            {
                EndGame(true);
                return;
            }

            if (_secondsElapsed >= 8)
            {
                EndGame(false);
                return;
            }
        }

        // ---------- FIN DEL JUEGO ----------
        private void EndGame(bool gano)
        {
            StopGame();

            ResultText.Text = gano
                ? $"GANASTE, puntaje obtenido: {_score}"
                : $"PERDISTE, puntaje obtenido: {_score}";

            string mensaje = gano
                ? $"¡GANASTE con {_score} puntos! ¿Deseas jugar otra vez?"
                : $"PERDISTE con {_score} puntos. ¿Deseas jugar otra vez?";

            var respuesta = MessageBox.Show(mensaje, "Fin del juego", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.Yes)
            {
                StartGame();
            }
            else
            {
                StartButton.IsEnabled = true;
            }
        }

        // ---------- UTILIDADES ----------
        private void UpdateScoreText()
        {
            ScoreText.Text = _score.ToString();
        }

        private void SetImageSource(Image imgControl, string fileName)
        {
            // Intent: first try load as resource inside the assembly (pack URI).
            // If the image files were added as Content and copied to output, try loading from the executable folder.
            BitmapImage? bmp = null;
            try
            {
                var uri = new Uri($"pack://application:,,,/Imagenes/{fileName}", UriKind.Absolute);
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                imgControl.Source = bmp;
                return;
            }
            catch
            {
                // ignore and try next strategy
            }

            try
            {
                var folder = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = System.IO.Path.Combine(folder, "Imagenes", fileName);
                if (File.Exists(filePath))
                {
                    bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(filePath, UriKind.Absolute);
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    imgControl.Source = bmp;
                    return;
                }
            }
            catch
            {
                // ignore
            }

            // Si no se encuentra la imagen, limpiar la fuente para evitar mostrar nada
            imgControl.Source = null;
        }
    }
}