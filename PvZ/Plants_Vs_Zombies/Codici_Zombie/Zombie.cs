using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using SFML.Graphics;
using SFML.System;

namespace Plants_Vs_Zombies
{
    abstract class Zombie
    {
        public object LockVel = new object(), LockVita = new object();
        public static IntRect rect;
        public static Texture texture;
        public Sprite sprite = new Sprite();

        public int fila;
        protected readonly float Vel = 30;
        public List<Rallentamento> rallentamenti = new List<Rallentamento>();

        protected int vita;
        public int danno;

        abstract public int Vita { get; set; }
        
        public Zombie(int y, float vel)
        {
            Vel = vel;
            int Y;
            switch (y)
            {
                case 0:
                    Y = 90;
                    break;
                case 1:
                    Y = 180;
                    break;
                case 2:
                    Y = 280;
                    break;
                case 3:
                    Y = 380;
                    break;
                default:
                    Y = 480;
                    break;
            }
            sprite.Position = new Vector2f(1093, Y);
        }
        public Zombie() { }
        abstract public float Probabilita { get; set; }
        abstract public Zombie GetInstance();
        abstract public Zombie GetInstance(int y);
    }
}