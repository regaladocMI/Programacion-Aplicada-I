using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Juego21
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        // Puntuaciones acumuladas
        private int puntosJugador1 = 0;
        private int puntosJugador2 = 0;

        // Control de estados de juego
        private bool jugador1Plantado = false;
        private bool jugador2Plantado = false;
        private bool juegoTerminado = false;

        // Lista de cartas ya salidas para evitar repetir en la misma partida
        private List<string> cartasUsadas = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            IniciarNuevoJuego();
        }

        private void BtnNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            IniciarNuevoJuego();
        }

        private void IniciarNuevoJuego()
        {
            puntosJugador1 = 0;
            puntosJugador2 = 0;
            jugador1Plantado = false;
            jugador2Plantado = false;
            juegoTerminado = false;

            cartasUsadas.Clear();

            pnlCartasJugador1.Children.Clear();
            pnlCartasJugador2.Children.Clear();

            lblPuntos1.Text = "Jugador 1 Puntos: 0";
            lblPuntos2.Text = "Jugador 2 Puntos: 0";
            lblMensaje.Text = "¡Partida iniciada! Turno de ambos jugadores.";

            // Habilitar botones de juego
            btnPedir1.IsEnabled = true;
            btnPlantarse1.IsEnabled = true;
            btnPedir2.IsEnabled = true;
            btnPlantarse2.IsEnabled = true;
        }

        private void BtnPedir1_Click(object sender, RoutedEventArgs e)
        {
            if (juegoTerminado || jugador1Plantado) return;

            DarCarta(1);

            // Si se pasa de 21 automáticamente pierde el turno / se planta
            if (puntosJugador1 > 21)
            {
                jugador1Plantado = true;
                btnPedir1.IsEnabled = false;
                btnPlantarse1.IsEnabled = false;
                lblMensaje.Text = "¡Jugador 1 se pasó de 21 puntos!";
                VerificarFinJuego();
            }
        }

        private void BtnPlantarse1_Click(object sender, RoutedEventArgs e)
        {
            if (juegoTerminado || jugador1Plantado) return;

            jugador1Plantado = true;
            btnPedir1.IsEnabled = false;
            btnPlantarse1.IsEnabled = false;
            lblMensaje.Text = "Jugador 1 se ha plantado con " + puntosJugador1 + " puntos.";
            VerificarFinJuego();
        }

        private void BtnPedir2_Click(object sender, RoutedEventArgs e)
        {
            if (juegoTerminado || jugador2Plantado) return;

            DarCarta(2);

            // Si se pasa de 21 automáticamente pierde
            if (puntosJugador2 > 21)
            {
                jugador2Plantado = true;
                btnPedir2.IsEnabled = false;
                btnPlantarse2.IsEnabled = false;
                lblMensaje.Text = "¡Jugador 2 se pasó de 21 puntos!";
                VerificarFinJuego();
            }
        }

        private void BtnPlantarse2_Click(object sender, RoutedEventArgs e)
        {
            if (juegoTerminado || jugador2Plantado) return;

            jugador2Plantado = true;
            btnPedir2.IsEnabled = false;
            btnPlantarse2.IsEnabled = false;
            lblMensaje.Text = "Jugador 2 se ha plantado con " + puntosJugador2 + " puntos.";
            VerificarFinJuego();
        }

        private void DarCarta(int jugador)
        {
            // Generar una carta aleatoria válida (Número del 1 al 13, Palos: C, D, E, T)
            string[] palos = { "C", "D", "E", "T" };
            string codigoCarta = "";

            do
            {
                int numero = random.Next(1, 14); // 1 al 13
                string palo = palos[random.Next(palos.Length)];
                codigoCarta = $"{numero}{palo}";
            }
            while (cartasUsadas.Contains(codigoCarta));

            cartasUsadas.Add(codigoCarta);

            // Calcular el valor numérico para el 21
            string[] partes = codigoCarta.Split(new char[] { 'C', 'D', 'E', 'T' });
            int valorNominal = int.Parse(partes[0]);
            int puntosCarta = valorNominal > 10 ? 10 : valorNominal;

            // Crear el control Image para mostrar la carta
            Image imgCarta = new Image();
            imgCarta.Width = 70;
            imgCarta.Height = 100;
            imgCarta.Margin = new Thickness(5);

            // IMPORTANTE: uriPath se declara ANTES del try para que el catch
            // también pueda usarla en el mensaje de error.
            string uriPath = $"/Images/{codigoCarta}.png";

            try
            {
                // URI relativo: WPF lo resuelve automáticamente como recurso
                // embebido del ensamblado actual. Más confiable que armar
                // el pack URI absoluto a mano.
                BitmapImage bitmap = new BitmapImage(new Uri(uriPath, UriKind.Relative));
                imgCarta.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + uriPath + "\n\n" + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error al cargar recurso: " + ex.Message);
            }

            // Agregar la carta al contenedor correspondiente y sumar puntos
            if (jugador == 1)
            {
                pnlCartasJugador1.Children.Add(imgCarta);
                puntosJugador1 += puntosCarta;
                lblPuntos1.Text = $"Jugador 1 Puntos: {puntosJugador1}";
            }
            else
            {
                pnlCartasJugador2.Children.Add(imgCarta);
                puntosJugador2 += puntosCarta;
                lblPuntos2.Text = $"Jugador 2 Puntos: {puntosJugador2}";
            }
        }

        private void VerificarFinJuego()
        {
            // El juego termina si ambos se plantaron o alguno superó los 21 y el otro ya terminó
            if ((jugador1Plantado && jugador2Plantado) ||
                (puntosJugador1 > 21 && jugador2Plantado) ||
                (puntosJugador2 > 21 && jugador1Plantado) ||
                (puntosJugador1 > 21 && puntosJugador2 > 21))
            {
                juegoTerminado = true;
                DeterminarGanador();
            }
        }

        private void DeterminarGanador()
        {
            // Deshabilitar todos los botones de acción por si acaso
            btnPedir1.IsEnabled = false;
            btnPlantarse1.IsEnabled = false;
            btnPedir2.IsEnabled = false;
            btnPlantarse2.IsEnabled = false;

            string resultado = "";

            bool j1Excedido = puntosJugador1 > 21;
            bool j2Excedido = puntosJugador2 > 21;

            if (j1Excedido && j2Excedido)
            {
                resultado = $"¡Empate crítico! Ambos jugadores se pasaron de 21 puntos (J1: {puntosJugador1}, J2: {puntosJugador2}).";
            }
            else if (j1Excedido)
            {
                resultado = $"¡GANADOR JUGADOR 2! J1 se pasó con {puntosJugador1} puntos y J2 obtuvo {puntosJugador2}.";
            }
            else if (j2Excedido)
            {
                resultado = $"¡GANADOR JUGADOR 1! J2 se pasó con {puntosJugador2} puntos y J1 obtuvo {puntosJugador1}.";
            }
            else
            {
                // Ninguno se pasó, gana el que tenga mayor puntaje o empate
                if (puntosJugador1 > puntosJugador2)
                {
                    resultado = $"¡GANADOR JUGADOR 1! Obtuvo {puntosJugador1} puntos frente a {puntosJugador2} del J2.";
                }
                else if (puntosJugador2 > puntosJugador1)
                {
                    resultado = $"¡GANADOR JUGADOR 2! Obtuvo {puntosJugador2} puntos frente a {puntosJugador1} del J1.";
                }
                else
                {
                    resultado = $"¡EMPATE! Ambos jugadores obtuvieron {puntosJugador1} puntos.";
                }
            }

            lblMensaje.Text = resultado;
        }
    }
}