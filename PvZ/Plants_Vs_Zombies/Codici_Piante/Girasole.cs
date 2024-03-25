using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Girasole : Pianta
    {
        // Immagine lista di girasole
        public static IntRect l_g = new IntRect(0, 0, 339, 170);
        public static Texture L_g = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Girasole.png", l_g);
        public static readonly Sprite L_G = new Sprite(L_g);

        public static bool disponibile = true;
        public const int Num_Lista_Piante = 0;
        public readonly int X, Y;
        public Timer Sun_On = new Timer(12000);

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

        public Girasole(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[4]].prezzo;

            X = x; Y = y;

            Sun_On.Elapsed += Sun_On_Elapsed;
            Sun_On.Enabled = true;

            rect = new IntRect(117, 9, 187, 212);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Girasole.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(15, 25);
            sprite.Scale = new Vector2f(0.3f, 0.3f);
        }

        public Girasole()
        {
            prezzo = 50;
        }

        // Fa comparire un nuovo sole
        private void Sun_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Program.LockSoli)
            {
                Sole s = new Sole(sprite.Position, 1)
                {
                    value_sun = 25
                };
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Girasole(x, y);
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_G.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_G.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_G.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_G);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Girasole GetInstace()
        {
            return new Girasole();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}
