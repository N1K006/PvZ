using System.Timers;
using SFML.Graphics;
using SFML.System;

namespace Plants_Vs_Zombies
{
    abstract class Pianta
    {
        public object LockVita = new object();
        protected int vita;
        public int prezzo;
        public static IntRect rect;
        public static Texture texture;
        public Sprite sprite = new Sprite();
        public Timer attesa;

        protected Pianta(int x, int y)
        {
            Program.mappa_piante[x, y] = this;
            sprite.Position = new Vector2f(254 + x * 81, 72 + y * 100);
        }
        protected Pianta() { }
        abstract public int Vita { get; set; }
        abstract public void GetInstace(int x, int y);
        abstract public Pianta GetInstace();
        abstract public bool Disponibile();
        abstract public void DisegnaLista(int posizione, bool selezionato);
    }
}
