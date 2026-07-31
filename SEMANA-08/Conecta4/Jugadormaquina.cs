using System;
using System.Collections.Generic;

namespace Conecta4
{
    // La máquina decide su jugada sola, aplicando una estrategia simple:
    // 1) Ganar si puede.
    // 2) Bloquear al jugador si este puede ganar en su próximo turno.
    // 3) Si no, priorizar el centro con algo de aleatoriedad.
    public class JugadorMaquina : Jugador
    {
        private readonly Random rnd = new Random();
        private readonly int[] ordenPreferido = { 3, 2, 4, 1, 5, 0, 6 };

        public JugadorMaquina(Ficha ficha, string nombre) : base(ficha, nombre) { }

        public override int ElegirColumna(Tablero tablero)
        {
            int columna = BuscarJugadaGanadora(tablero, Ficha);
            if (columna != -1) return columna;

            Ficha fichaRival = Ficha == Ficha.Maquina ? Ficha.Jugador : Ficha.Maquina;
            columna = BuscarJugadaGanadora(tablero, fichaRival);
            if (columna != -1) return columna;

            return ElegirColumnaEstrategica(tablero);
        }

        private int BuscarJugadaGanadora(Tablero tablero, Ficha ficha)
        {
            for (int columna = 0; columna < Tablero.Columnas; columna++)
            {
                if (!tablero.ColocarFicha(columna, ficha, out int fila)) continue;

                bool gana = tablero.HayConexionDeCuatro(fila, columna, ficha);
                tablero.QuitarFicha(fila, columna); // deshacer la simulación

                if (gana) return columna;
            }
            return -1;
        }

        private int ElegirColumnaEstrategica(Tablero tablero)
        {
            List<int> disponibles = new List<int>();

            foreach (int col in ordenPreferido)
            {
                if (!tablero.ColumnaLlena(col))
                    disponibles.Add(col);
            }

            if (disponibles.Count == 0) return -1;

            int limite = Math.Min(2, disponibles.Count);
            return disponibles[rnd.Next(limite)];
        }
    }
}