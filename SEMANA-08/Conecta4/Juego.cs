using System;

namespace Conecta4
{
    // Motor del juego: coordina Tablero + Jugadores y no sabe nada de WPF.
    // MainWindow solo consume esta clase y reacciona a sus eventos.
    public class Juego
    {
        public Tablero Tablero { get; }
        public JugadorHumano Jugador1 { get; }
        public JugadorMaquina Jugador2 { get; }
        public Jugador JugadorActual { get; private set; }
        public bool Terminado { get; private set; }

        // Notifica a la UI que se colocó una ficha, para pintar la celda correspondiente.
        public event Action<int, int, Ficha> FichaColocada;

        // Notifica el fin de la partida con un mensaje ya listo para mostrar.
        public event Action<string> JuegoFinalizado;

        // Notifica el cambio de turno.
        public event Action<Jugador> TurnoCambiado;

        public Juego()
        {
            Tablero = new Tablero();
            Jugador1 = new JugadorHumano(Ficha.Jugador, "Jugador");
            Jugador2 = new JugadorMaquina(Ficha.Maquina, "Máquina");
            Reiniciar();
        }

        public void Reiniciar()
        {
            Tablero.Reiniciar();
            Terminado = false;
            JugadorActual = Jugador1;
        }

        // Punto de entrada llamado desde la UI al hacer clic en una columna.
        public void JugarColumnaHumano(int columna)
        {
            if (Terminado || JugadorActual != Jugador1) return;

            Jugador1.SeleccionarColumna(columna);
            if (!EjecutarTurno(Jugador1)) return; // columna llena, no pasa nada

            if (Terminado) return;

            JugadorActual = Jugador2;
            TurnoCambiado?.Invoke(JugadorActual);

            EjecutarTurno(Jugador2);

            if (!Terminado)
            {
                JugadorActual = Jugador1;
                TurnoCambiado?.Invoke(JugadorActual);
            }
        }

        // Ejecuta la jugada de "jugador" (humano o máquina) usando polimorfismo:
        // no importa cuál sea, ambos exponen ElegirColumna().
        private bool EjecutarTurno(Jugador jugador)
        {
            int columna = jugador.ElegirColumna(Tablero);
            if (columna < 0 || columna >= Tablero.Columnas) return false;

            if (!Tablero.ColocarFicha(columna, jugador.Ficha, out int fila)) return false;

            FichaColocada?.Invoke(fila, columna, jugador.Ficha);

            if (Tablero.HayConexionDeCuatro(fila, columna, jugador.Ficha))
            {
                Terminado = true;
                JuegoFinalizado?.Invoke($"¡Ganó {jugador.Nombre}!");
            }
            else if (Tablero.TableroLleno())
            {
                Terminado = true;
                JuegoFinalizado?.Invoke("Empate");
            }

            return true;
        }
    }
}