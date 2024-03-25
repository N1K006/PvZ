using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Timers;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
    static class Program
    {
        public const int LARGHEZZA = 1045;
        public const int ALTEZZA = 600;

        static VideoMode Schermo = new VideoMode(LARGHEZZA, ALTEZZA);
        static RenderWindow Finestra = new RenderWindow(Schermo, "Plants VS Zombies");

        public static Pianta[] piante = { new Sparasemi(),
                                          new Supersparasemi(),
                                          new Triplasparasemi(),
                                          new Sparabrina(),
                                          new Girasole(),
                                          new GirasoleGemello(),
                                          new Rovo(),
                                          new MuroNoce(),
                                          new InfiNoce() };

        public static Zombie[] zombie = { new ZombieOrdinario(0f), 
                                          new ZombieSegnaletico(0f) };

        public static int[] lista_piante = new int[8];
        public static Pianta[,] mappa_piante = new Pianta[9, 5];
        public static List<Zombie>[] mappa_zombie = new List<Zombie>[5];

        public static int yLista = 8;
        public static int x, y;
        public static int contatore = 5, n_soli = 5000;

        static Timer Sun_On_Map = new Timer(4000);

        static Timer Vel_Zombie = new Timer(5000);
        static Timer Zombie_On = new Timer(10010);

        static Timer Diff = new Timer(30000);
        static int cDiff = 2;
        public static int difficolta = 0;

        public static int fase = 0;

        public static int Difficolta
        {
            get => difficolta;
            set
            {
                if (value > 0)
                {
                    difficolta = value;
                    for (int i = 0; i < zombie.Length; i++)
                    {
                        if (zombie[i].Probabilita == 0)
                        {
                            zombie[i].Probabilita = 100;
                            break;
                        }
                        else
                            zombie[i].Probabilita -= zombie[i].Probabilita / 10;
                    }
                    switch (value)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                    }
                }
            }
        }

        static public object LockZombie = new object();
        static public object LockSemi = new object();
        static public object LockSoli = new object();

        static void Main()
        {
            Logger.Grade = Logger.Grades.Trace;

            {
                for (int i = 0; i < 5; i++)
                    mappa_zombie[i] = new List<Zombie>();
                {
                    Grafica.piante = piante;
                    Grafica.Finestra = Finestra;
                    Grafica.mappa_piante = mappa_piante;
                    Grafica.mappa_zombie = mappa_zombie;
                    Grafica.lista_piante = lista_piante;

                    Home.Finestra = Finestra;
                } //grafica e home
                {
                    lista_piante[0] = 0;
                    lista_piante[1] = 1;
                    lista_piante[2] = 2;
                    lista_piante[3] = 3;
                    lista_piante[4] = 4;
                    lista_piante[5] = 5;
                    lista_piante[6] = 7;
                    lista_piante[7] = 8;
                } //lista
            } // inizializzazioni variabili
            
            while (Finestra.IsOpen)
            {
                if (fase == 0)
                {
                    Home.home();
                    Finestra.SetVerticalSyncEnabled(true);
                    Finestra.Closed += (sender, args) => Finestra.Close();
                    Finestra.MouseButtonPressed += MouseClick;
                    Finestra.Clear();
                    AvvioGame();
                }
                Grafica.Disegna();
                Finestra.DispatchEvents();
                Finestra.Display();
            }
        }
         
        static void AvvioGame()
        {
            Zombie_On.Elapsed += Zombie_On_Elapsed;
            Zombie_On.Enabled = true;

            Sun_On_Map.Elapsed += Sun_On_Map_Elapsed;
            Sun_On_Map.Enabled = true;

            Vel_Zombie.Elapsed += Vel_Zombie_Elapsed;
            Vel_Zombie.Enabled = true;

            Diff.Elapsed += Diff_Elapsed;
            Diff.Enabled = true;
        }

        static void MouseClick(object sender, MouseButtonEventArgs e)
        {
            x = e.X;
            y = e.Y;

            Logger.WriteLine(x + " " + y, 6);
            bool s;
            lock (LockSoli)
            {
                s = ClickSole();
            }

            if (x > 24 && x < 140) //lista selezionata
            {
                int Y = y - 10;
                int aux = Y % 65;
                Y /= 65;
                if (aux > 57 || y - 10 < 0 || !piante[lista_piante[Y]].GetInstace().Disponibile())
                    Y = 8;

                yLista = Y;
            }
            else if (x > 253 && y > 71) //casella selezionata
            {
                int X, Y;
                X = (x - 254) / 81;
                Y = (y - 72) / 100;
                Logger.WriteLine(X + " " + Y, 6);
                if (!s && X < 9)
                    PosizionaPianta(X, Y);
            }
            else if (x > 351 && y > 37) // paletta
            {
                
            }
            else
                yLista = 8;
        }

        static void Sun_On_Map_Elapsed(object sender, ElapsedEventArgs e)
        {
            contatore++;
            if (contatore == 6)
            {
                contatore = 0;
                lock (LockSoli)
                {
                    Sole s = new Sole();
                    s.value_sun = 25;
                }
            }
        }

        static void Vel_Zombie_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(Zombie_On.Interval > 1000)
                Zombie_On.Interval -= 10;
        }

        static void Diff_Elapsed(object sender, ElapsedEventArgs e)
        {
            cDiff++;
            if ( cDiff == 3)
            {
                Difficolta++;
                cDiff = 0;
            }
        }

        static void Zombie_On_Elapsed(object sender, ElapsedEventArgs e)
        {
            Random r = new Random();
            bool creato = false;
            for (int i = 0; i < zombie.Length && !creato; i++)
                if (r.Next(0, 100) < zombie[i].GetInstance().Probabilita || i == zombie.Length - 1)
                {
                    lock (LockZombie)
                    {
                        Zombie aux = zombie[i].GetInstance(new Random().Next(0, 5));
                    }
                    creato = true;
                }
        }

        static bool ClickSole()
        {
            lock (LockSoli)
                for (int i = 0; i < Sole.soli.Count; i++)
                    if (Sole.soli[i] != null)
                        if (x >= Sole.soli[i].pos.X - 32 && x <= Sole.soli[i].pos.X + 32 && y >= Sole.soli[i].pos.Y - 32 && y <= Sole.soli[i].pos.Y + 32)
                        {
                            Sole.soli[i].Preso();
                            return true;
                        }
            return false;
        }

        static void PosizionaPianta(int x, int y)
        {
            if (mappa_piante[x, y] == null && yLista != 8)
                if (piante[lista_piante[yLista]].GetInstace().Disponibile() && piante[lista_piante[yLista]].GetInstace().prezzo <= n_soli)
                    piante[lista_piante[yLista]].GetInstace(x, y);
            yLista = 8;
        }
    }
}