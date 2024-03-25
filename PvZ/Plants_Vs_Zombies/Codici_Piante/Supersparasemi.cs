using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Supersparasemi : Pianta
    {
        // Immagine lista di supersparasemi
        public static IntRect l_sup = new IntRect(0, 0, 339, 170);
        public static Texture L_sup = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Supersparasemi.png", l_sup);
        public static readonly Sprite L_SUP = new Sprite(L_sup);

        public static bool disponibile = true;
        private readonly int X, Y;

        public Timer Seme_On = new Timer(1500);
        public Timer Seme_On_2 = new Timer(400);
        public override int Vita
        {
            get
            {
                lock (LockVita)
                    return vita;
            }
            set
            {
                lock (LockVita)
                {
                    vita = value;
                    if (vita <= 0)
                    {
                        Program.mappa_piante[X, Y] = null;
                    }
                }
            }
        }

        public Supersparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[1]].prezzo;

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(106, 10, 203, 212);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Supersparasemi.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(12, 25);
            sprite.Scale = new Vector2f(0.31f, 0.31f);
        }

        public Supersparasemi() 
        { 
            prezzo = 200; 
        }

        public bool Spara()
        {
            bool spara = false;
            lock (Program.LockZombie)
            {
                for (int i = 0; i < Program.mappa_zombie[Y].Count; i++)
                {
                    Zombie z = Program.mappa_zombie[Y][i];
                    if (z != null)
                        if (z.sprite.Position.X >= sprite.Position.X)
                            spara = true;
                }
            }
            return spara;
        }

        private void Seme_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Spara())
            {
                lock (Program.LockSemi)
                {
                    SemeDefault s = new SemeDefault(new Vector2f(X, Y), 10);
                }
                Seme_On_2.Elapsed += Seme_On_2_Elapsed;
                Seme_On_2.Enabled = true;
            }
        }

        private void Seme_On_2_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Program.LockSemi)
            {
                SemeDefault s = new SemeDefault(new Vector2f(X, Y), 10);
            }
            Seme_On_2.Stop();
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_SUP.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_SUP.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_SUP.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_SUP);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void GetInstace(int x, int y)
        {
            new Supersparasemi(x, y);
        }

        public override Supersparasemi GetInstace()
        {
            return new Supersparasemi();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}