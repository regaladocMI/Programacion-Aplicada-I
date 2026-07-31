namespace Conecta4
{
    // Clase base abstracta: define el contrato común que deben cumplir
    // tanto el jugador humano como la máquina (abstracción).
    public abstract class Jugador
    {
        public Ficha Ficha { get; }
        public string Nombre { get; }

        protected Jugador(Ficha ficha, string nombre)
        {
            Ficha = ficha;
            Nombre = nombre;
        }

        // Cada tipo de jugador decide "a su manera" en qué columna jugar
        // (polimorfismo: el comportamiento real depende de la clase concreta).
        public abstract int ElegirColumna(Tablero tablero);
    }
}