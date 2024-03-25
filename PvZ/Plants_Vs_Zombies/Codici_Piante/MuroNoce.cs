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
    class MuroNoce : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_mn = new IntRect(0, 0, 339, 170);
        public static Texture L_mn = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_MuroNoce.png", l_mn);
        public static readonly Sprite L_MN = new Sprite(L_mn);

        public static bool disponibile = true;
        public readonly int X, Y;
        
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

        public MuroNoce(int x, int y) : base(x, y) //mappa
        {
            vita = 1000;
            disponibile = false;
            attesa = new Timer(20000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[7]].prezzo;

            X = x; Y = y;

            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\MuroNoce.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(12, 25);
            sprite.Scale = new Vector2f(0.3f, 0.3f);
        }
        public MuroNoce()
        {
            prezzo = 50;
        } //lista

        public override void GetInstace(int x, int y)
        {
            new MuroNoce(x, y);
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_MN.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_MN.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_MN.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_MN);
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override MuroNoce GetInstace()
        {
            return new MuroNoce();
        }

        public override bool Disponibile() 
        { 
            return disponibile; 
        }
    }
}