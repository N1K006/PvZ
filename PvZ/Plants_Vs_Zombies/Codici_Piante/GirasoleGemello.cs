using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class GirasoleGemello : Pianta
    {
        // Immagine lista di girasole gemello
        public static IntRect l_gg = new IntRect(0, 0, 339, 170);
        public static Texture L_gg = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_GirasoleGemello.png", l_gg);
        public static readonly Sprite L_GG = new Sprite(L_gg);

        public static bool disponibile = true;
        public const int Num_Lista_Piante = 1;
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

        public GirasoleGemello(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(10000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[5]].prezzo;

            X = x; Y = y;

            Sun_On.Elapsed += Sun_On_Elapsed;
            Sun_On.Enabled = true;

            rect = new IntRect(76, 0, 259, 231);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\GirasoleGemello.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(5, 25);
            sprite.Scale = new Vector2f(0.3f, 0.3f);
        }
        public GirasoleGemello() 
        {
            prezzo = 150;
        }

        private void Sun_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Program.LockSoli)
            {
                Sole s1 = new Sole(sprite.Position, 2);
                s1.value_sun = 25;
            }
            lock (Program.LockSoli)
            {
                Sole s2 = new Sole(sprite.Position, 2);
                s2.value_sun = 25;
            }
        }

        public override void GetInstace(int x, int y)
        {
            new GirasoleGemello(x, y);
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_GG.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_GG.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_GG.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_GG);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override GirasoleGemello GetInstace()
        {
            return new GirasoleGemello();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}
