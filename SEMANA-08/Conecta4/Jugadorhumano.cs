namespace Conecta4
{
    // El jugador humano no decide "solo": recibe la columna elegida
    // desde la interfaz (clic en un botón) y la guarda hasta que se le pide jugar.
    public class JugadorHumano : Jugador
    {
        private int columnaSeleccionada = -1;

        public JugadorHumano(Ficha ficha, string nombre) : base(ficha, nombre) { }

        public void SeleccionarColumna(int columna)
        {
            columnaSeleccionada = columna;
        }

        public override int ElegirColumna(Tablero tablero)
        {
            return columnaSeleccionada;
        }
    }
}