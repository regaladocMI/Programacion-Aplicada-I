namespace Conecta4
{
    // Encapsula el estado del tablero y las reglas básicas del juego.
    // Nadie fuera de esta clase accede directamente a la matriz interna:
    // todo se hace a través de métodos públicos (encapsulamiento).
    public class Tablero
    {
        public const int Filas = 6;
        public const int Columnas = 7;

        private readonly Ficha[,] celdas;

        public Tablero()
        {
            celdas = new Ficha[Filas, Columnas];
            Reiniciar();
        }

        public Ficha ObtenerFicha(int fila, int columna) => celdas[fila, columna];

        public void Reiniciar()
        {
            for (int f = 0; f < Filas; f++)
                for (int c = 0; c < Columnas; c++)
                    celdas[f, c] = Ficha.Vacio;
        }

        public bool ColumnaLlena(int columna) => celdas[0, columna] != Ficha.Vacio;

        // Fila libre más baja de una columna, o -1 si está llena.
        public int FilaLibre(int columna)
        {
            for (int f = Filas - 1; f >= 0; f--)
            {
                if (celdas[f, columna] == Ficha.Vacio) return f;
            }
            return -1;
        }

        // Intenta colocar una ficha en la columna indicada.
        // Devuelve true y la fila resultante si tuvo éxito; false si la columna está llena.
        public bool ColocarFicha(int columna, Ficha ficha, out int filaUsada)
        {
            filaUsada = FilaLibre(columna);
            if (filaUsada == -1) return false;

            celdas[filaUsada, columna] = ficha;
            return true;
        }

        // Deshace una jugada (usado por la IA para simular sin alterar el tablero real).
        public void QuitarFicha(int fila, int columna)
        {
            celdas[fila, columna] = Ficha.Vacio;
        }

        public bool TableroLleno()
        {
            for (int c = 0; c < Columnas; c++)
            {
                if (celdas[0, c] == Ficha.Vacio) return false;
            }
            return true;
        }

        // Revisa las 4 direcciones (horizontal, vertical, dos diagonales)
        // a partir de una celda específica.
        public bool HayConexionDeCuatro(int fila, int columna, Ficha ficha)
        {
            int[][] direcciones =
            {
                new[] { 0, 1 },   // horizontal
                new[] { 1, 0 },   // vertical
                new[] { 1, 1 },   // diagonal \
                new[] { 1, -1 }   // diagonal /
            };

            foreach (var dir in direcciones)
            {
                int conteo = 1;
                conteo += ContarEnDireccion(fila, columna, dir[0], dir[1], ficha);
                conteo += ContarEnDireccion(fila, columna, -dir[0], -dir[1], ficha);

                if (conteo >= 4) return true;
            }
            return false;
        }

        private int ContarEnDireccion(int fila, int columna, int deltaFila, int deltaColumna, Ficha ficha)
        {
            int contador = 0;
            int f = fila + deltaFila;
            int c = columna + deltaColumna;

            while (f >= 0 && f < Filas && c >= 0 && c < Columnas && celdas[f, c] == ficha)
            {
                contador++;
                f += deltaFila;
                c += deltaColumna;
            }
            return contador;
        }
    }
}