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
    class KiwiBestiale : Pianta
    {
        // Immagine lista di muro noce
        public static IntRect l_kb = new IntRect(0, 0, 0, 0);
        public static Texture L_kb = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_KiwiBestiale.png", l_kb);
        public static readonly Sprite L_KB = new Sprite(L_kb);

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

        public KiwiBestiale(int x, int y) : base(x, y)
        {
            vita = 150;
            disponibile = false;
            attesa = new Timer(15000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            rect = new IntRect(116, 5, 183, 225);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\KiwiBestiale.png", rect);
            sprite.Texture = texture;
        }

        public KiwiBestiale()
        {
            prezzo = 175;
        }

        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_KB.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_KB.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_KB.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_KB);
        }

        public override bool Disponibile()
        {
            return disponibile;
        }

        public override void GetInstace(int x, int y)
        {
            new KiwiBestiale(x, y);
        }

        public override KiwiBestiale GetInstace()
        {
            return new KiwiBestiale();
        }
    }
}