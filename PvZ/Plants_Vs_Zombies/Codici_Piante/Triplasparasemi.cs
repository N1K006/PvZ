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
    class Triplasparasemi : Pianta
    {
        // Immagine lista di Triplasparasemi
        public static IntRect l_ts = new IntRect(0, 0, 339, 170);
        public static Texture L_ts = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Triplasparasemi.png", l_ts);
        public static readonly Sprite L_TS = new Sprite(L_ts);

        public static bool disponibile = true;
        public const int Num_Lista_Piante = 7;
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

        public Triplasparasemi(int x, int y) : base(x, y)
        {
            vita = 50;
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[2]].prezzo;

            X = x; Y = y;

            Seme_On.Elapsed += Seme_On_Elapsed;
            Seme_On.Enabled = true;

            rect = new IntRect(102, 14, 309, 264);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Triplasparasemi.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(0, 18);
            sprite.Scale = new Vector2f(0.29f, 0.29f);
        }
        public Triplasparasemi() 
        {
            prezzo = 300;
        }

        public bool Spara()
        {
            bool spara = false;
            lock (Program.LockZombie)
            {
                if (Y > 0)
                    for (int i = 0; i < Program.mappa_zombie[Y - 1].Count; i++)
                    {
                        Zombie z = Program.mappa_zombie[Y - 1][i];
                        if (z != null)
                            if (z.sprite.Position.X >= sprite.Position.X)
                                spara = true;
                    }
            }
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
            lock (Program.LockZombie)
            {
                if (Y < 4)
                    for (int i = 0; i < Program.mappa_zombie[Y + 1].Count; i++)
                    {
                        Zombie z = Program.mappa_zombie[Y + 1][i];
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
                if (Y > 0)
                    lock (Program.LockSemi)
                    {
                        SemeDefault s1 = new SemeDefault(new Vector2f(X, Y - 1), 10);
                    }
                lock (Program.LockSemi)
                {
                    SemeDefault s2 = new SemeDefault(new Vector2f(X, Y), 10);
                }
                if (Y < 5)
                    lock (Program.LockSemi)
                    {
                        SemeDefault s3 = new SemeDefault(new Vector2f(X, Y + 1), 10);
                    }
            }
        }

        public override void GetInstace(int x, int y)
        {
            new Triplasparasemi(x, y);
        }
        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_TS.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_TS.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_TS.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_TS);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Triplasparasemi GetInstace()
        {
            return new Triplasparasemi();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}
