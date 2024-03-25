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
    class InfiNoce : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_in = new IntRect(0, 0, 339, 170);
        public static Texture L_in = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_InfiNoce.png", l_in);
        public static readonly Sprite L_IN = new Sprite(L_in);

        public static bool disponibile = true;
        public readonly int X, Y;
        public int vitaAtt;
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

        public InfiNoce(int x, int y) : base(x, y) //mappa
        {
            vita = 750; vitaAtt = vita;
            disponibile = false;
            attesa = new Timer(20000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[7]].prezzo;

            X = x; Y = y;

            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Infi_Noce.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(2, 20);
            sprite.Scale = new Vector2f(0.33f, 0.33f);

            Timer Recupero_vita = new Timer(500);
            Recupero_vita.Elapsed += Recupero_vita_Elapsed;
            Recupero_vita.Enabled = true;
        }

        public InfiNoce()
        {
            prezzo = 75;
        } //lista

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_IN.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_IN.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_IN.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_IN);
        }

        void Recupero_vita_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (vitaAtt <= vita - 1)
                vitaAtt += 1;
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void GetInstace(int x, int y)
        {
            new InfiNoce(x, y);
        }

        public override InfiNoce GetInstace()
        {
            return new InfiNoce();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}

