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
    class Rovo : Pianta
    {
        // Immagine lista di Rovo
        public static IntRect l_ro = new IntRect(0, 0, 339, 170);
        public static Texture L_ro = new Texture(@"..\..\..\Immagini\Piante\Lista_di_Piante\Lista_Rovo.png", l_ro);
        public static readonly Sprite L_RO = new Sprite(L_ro);

        public static bool disponibile = true;
        public const int Num_Lista_Piante = 3;
        public int danno = 5;
        private readonly int X, Y;
        private Timer Attack_On = new Timer(100);
        public override int Vita { get; set; }

        public Rovo(int x, int y) : base(x, y)
        {
            disponibile = false;
            attesa = new Timer(5000);
            attesa.Start();
            attesa.Elapsed += attesa_Elapsed;
            Program.n_soli -= Program.piante[Program.lista_piante[6]].prezzo;

            X = x; Y = y;

            rect = new IntRect(52, 52, 308, 128);
            texture = new Texture(@"..\..\..\Immagini\Piante\Piante\Rovo.png", rect);
            sprite.Texture = texture;
            sprite.Position += new Vector2f(2, 65);
            sprite.Scale = new Vector2f(0.26f, 0.26f);

            Attack_On.Elapsed += Attack_On_Elapsed;
            Attack_On.Enabled = true;
        }

        public Rovo() 
        {
            prezzo = 100;
        }
        //308 422
        private void Attack_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Program.LockZombie)
                for (int i = 0; i < Program.mappa_zombie[Y].Count; i++)
                    if (Program.mappa_zombie[Y][i].sprite.Position.X <= sprite.Position.X + 308 * sprite.Scale.X && Program.mappa_zombie[Y][i].sprite.Position.X + 422 * Program.mappa_zombie[Y][i].sprite.Scale.X >= sprite.Position.X)
                        lock (Program.mappa_zombie[Y][i].LockVita)
                            Program.mappa_zombie[Y][i].Vita -= danno;
        }

        public override void GetInstace(int x, int y)
        {
            new Rovo(x, y);
        }

        public override void DisegnaLista(int posizione, bool selezionato)
        {
            L_RO.Position = new Vector2f(25, 65 * posizione + 10);
            if (selezionato)
                L_RO.Scale = new Vector2f(0.36f, 0.36f);
            else
                L_RO.Scale = new Vector2f(0.34f, 0.34f);
            Grafica.Finestra.Draw(L_RO);
        }
        void attesa_Elapsed(object sender, ElapsedEventArgs e)
        {
            disponibile = true;
            attesa.Stop();
        }

        public override Rovo GetInstace()
        {
            return new Rovo();
        }

        public override bool Disponibile()
        {
            return disponibile;
        }
    }
}