using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Timers;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
    static class Grafica
    {
        static public RenderWindow Finestra;
        public static Pianta[] piante;
        public static Pianta[,] mappa_piante;
        public static List<Zombie>[] mappa_zombie;
        public static int[] lista_piante;

        #region SFML
        // FONT
        public static Font numero_sole = new Font(@"..\..\..\Font\ComixLoud.ttf");

        // immagine MAPPA
        public static IntRect map1 = new IntRect(0, 0, 4179, 2399);
        public static Texture Map1 = new Texture(@"..\..\..\Immagini\Mappa\Mappa1.jpg", map1);
        public static readonly Sprite MAP1 = new Sprite(Map1);

        // Immagine conta soli
        public static IntRect c_s = new IntRect(0, 0, 253, 101);
        public static Texture C_s = new Texture(@"..\..\..\Immagini\Mappa\Conta_soli.png", c_s);
        public static readonly Sprite C_S = new Sprite(C_s);

        // Immagine paletta
        public static IntRect pa = new IntRect(152, 7, 658, 707);
        public static Texture Pa = new Texture(@"..\..\..\Immagini\Mappa\Paletta.png", pa);
        public static readonly Sprite PA = new Sprite(Pa);

        // Immagine cerchio
        public static IntRect cerchio = new IntRect(0, 0, 603, 603);
        public static Texture Cerchio = new Texture(@"..\..\..\Immagini\Mappa\Cerchio.png", cerchio);
        public static readonly Sprite CERCHIO = new Sprite(Cerchio);
        #endregion

        static public void Disegna()
        {
            PA.Position = new Vector2f(351, 35);
            PA.Scale = new Vector2f(0.07f, 0.07f);
            PA.Origin = new Vector2f(329, 353);
            CERCHIO.Position = new Vector2f(351, 37);
            CERCHIO.Scale = new Vector2f(0.08f, 0.08f);
            CERCHIO.Origin = new Vector2f(301, 301);

            // Immagine conta soli
            {
                C_S.Origin = new Vector2f(50, 50);
                C_S.Scale = new Vector2f(0.65f, 0.65f);
                C_S.Position = new Vector2f(155 + C_S.Origin.X * C_S.Scale.X, 0 + C_S.Origin.Y * C_S.Scale.Y);
            }

            MAP1.Scale = new Vector2f(0.25f, 0.25f);

            Finestra.Draw(MAP1); // Mappa
            Finestra.Draw(C_S);  // Contatore soli
            Finestra.Draw(CERCHIO); //Cerchio
            Finestra.Draw(PA); //Cerchio

            {
                for (int  i = 0; i < 8; i++)
                {
                    piante[lista_piante[i]].DisegnaLista(i, Program.yLista == i);
                    if (!piante[lista_piante[i]].GetInstace().Disponibile())
                    {
                        RectangleShape rect = new RectangleShape(new Vector2f(115, 58));
                        rect.FillColor = new Color(100, 100, 100, 150);
                        rect.Position = new Vector2f(25, 65 * i + 10);
                        Finestra.Draw(rect);
                    }
                }
            } // Disegno delle immagini lista

            // Piante nella mappa
            {
                for (int y = 0; y < 5; y++)
                    for (int x = 0; x < 9; x++)
                        if (mappa_piante[x, y] != null)
                            Finestra.Draw(mappa_piante[x, y].sprite);
            }
            // Zombie nella mappa
            {
                lock (Program.LockZombie)
                {
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < mappa_zombie[i].Count; j++)
                            if (mappa_zombie[i][j] != null)
                                Finestra.Draw(mappa_zombie[i][j].sprite);
                }
            }
            // Soli e semi
            {
                lock (Program.LockSemi)
                {
                    for (int i = 0; i < Seme.semi.Count; i++)
                        if (Seme.semi != null)
                            Finestra.Draw(Seme.semi[i].circle);
                }
                lock (Program.LockSoli)
                {
                    for (int i = 0; i < Sole.soli.Count; i++)
                        if (Sole.soli != null)
                            Finestra.Draw(Sole.soli[i].sole);
                }
                lock (Program.LockSoli)
                {
                    for (int i = 0; i < Sole.soliPresi.Count; i++)
                        if (Sole.soliPresi != null)
                            Finestra.Draw(Sole.soliPresi[i].sole);
                }
            }
            // Font
            {
                Text n_sole = new Text(Convert.ToString(Program.n_soli), numero_sole, 13);
                n_sole.FillColor = Color.White;
                n_sole.Position = new Vector2f(235, 26);
                Finestra.Draw(n_sole);
            }
        }
    }
}