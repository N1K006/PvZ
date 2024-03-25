using System;
using SFML.Graphics;
using SFML.System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Sparasemi : Pianta
    {
        // Immagine lista di sparasemi
        public static IntRect l_ss = new IntRect(0, 0, 339, 170);
        public static Texture L_ss = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Sparasemi.png", l_ss);
        public static readonly Sprite L_SS = new Sprite(L_ss);

        public static bool disponibile = true;
        public const int Num_Lista_Piante = 5;
        public readonly int X, Y;
        public Timer Seme_On = new Timer(1500);
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

        public Sparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[0]].prezzo;

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(174, 46, 202, 210);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Sparasemi.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(12, 25);
            sprite.Scale = new Vector2f(0.29f, 0.29f);
        }

        public Sparasemi()
        {
            prezzo = 100;
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
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Sparasemi(x, y);
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_SS.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_SS.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_SS.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_SS);

        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Sparasemi GetInstace()
        {
            return new Sparasemi();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}