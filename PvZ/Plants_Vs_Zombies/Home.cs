using System;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;

namespace Plants_Vs_Zombies
{
    static  class Home
    {
        static public RenderWindow Finestra;

        // Immagine cerchio
        public static IntRect _home = new IntRect(0, 0, 1279, 792);
        public static Texture _Home = new Texture(@"..\..\..\Immagini\Mappa\Home.png", _home);
        public static readonly Sprite HOME = new Sprite(_Home);
        public static Font font = new Font(@"..\..\..\Font\ComixLoud.ttf");

        public static IntRect indietro = new IntRect(0, 0, 166, 166);
        public static Texture Indietro = new Texture(@"..\..\..\Immagini\Mappa\Indietro.png", indietro);
        public static readonly Sprite INDIETRO = new Sprite(Indietro);

        static Text impostazioni;
        static Text gioca;
        static bool _impostazioni = false;

        public static void Disegna()
        {
            HOME.Scale = new Vector2f((float)1045 / (float)HOME.Texture.Size.X, (float)600 / (float)HOME.Texture.Size.Y);
            Finestra.Draw(HOME);

            if (!_impostazioni)
            {
                gioca = new Text("GIOCA", font, 40);
                gioca.FillColor = new Color(255, 255, 255);
                gioca.Position = new Vector2f(355, 190);
                Finestra.Draw(gioca);

                impostazioni = new Text("IMPOSTAZIONI", font, 17);
                impostazioni.FillColor = new Color(0, 0, 0);
                impostazioni.Position = new Vector2f(350, 270);
                Finestra.Draw(impostazioni);
            }
            else if (true)
            {
                RectangleShape rect_impostazioni = new RectangleShape(new Vector2f(915, 400));
                rect_impostazioni.FillColor = new Color(100, 100, 100, 220);
                rect_impostazioni.Position = new Vector2f(65, 154);
                Finestra.Draw(rect_impostazioni);

                INDIETRO.Position = new Vector2f(15, 15);
                INDIETRO.Scale = new Vector2f(0.5f, 0.5f);
                Finestra.Draw(INDIETRO);
            }
        }

        public static void home()
        {
            Finestra.SetVerticalSyncEnabled(true);
            Finestra.Closed += (sender, args) => Finestra.Close();
            Finestra.MouseButtonPressed += MouseClick;

            while (Finestra.IsOpen)
            {
                Finestra.Clear();
                Disegna();
                Finestra.DispatchEvents();
                Finestra.Display();
                if (Program.fase == 1)
                    break;
            }
        }

        private static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            Logger.WriteLine(x.ToString() + " " + y, 6);

            if (x > 356 && x < 556 && y > 182 && y < 243) // tasto gioca
                Program.fase = 1;
            else if (x > 351 && x < 564 && y > 267 && y < 290) //tasto impostazioni
                _impostazioni = true;
            else if (_impostazioni && x > 15 && x < 98 && y > 15 && y < 98) // tasto indietro nelle impostazioni
                _impostazioni = false;
        }
    }
}