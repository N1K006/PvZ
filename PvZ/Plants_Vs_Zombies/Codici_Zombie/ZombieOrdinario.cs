using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;
using System.Reflection.Emit;

namespace Plants_Vs_Zombies
{
    class ZombieOrdinario : Zombie
    {
        Pianta p;
        Timer mangia = new Timer(500);

        static private float prob;
        override public float Probabilita
        {
            get => prob; 
            set =>  prob = 
                value <= 100 && value >= 0 ? //condizione
                value : //se true
                throw new ArgumentException("è possibile assegnare solo valori compresi tra 0 e 100"); //se false
        }
        public ZombieOrdinario(int y) : base(y, 1)
        {
            lock (LockVita)
            {
                lock (Program.LockZombie)
                {
                    Program.mappa_zombie[y].Add(this);
                }

                fila = y;
                vita = 60;
            }
            danno = 10;

            rect = new IntRect(30, 15, 422, 658);
            texture = new Texture(@"..\..\..\Immagini\Zombie\Zombie_ordinario\Zombie_ordinario.png", rect);
            sprite.Texture = texture;
            sprite.Scale = new Vector2f(-0.15f, 0.15f);
            {
                Timer Mov_Zombie = new Timer(100);
                Mov_Zombie.Elapsed += Mov_Zombie_Elapsed;
                Mov_Zombie.Enabled = true;

                mangia.Elapsed += mangia_Elapsed;
                mangia.Enabled = true;
            } //Timer
        }
        public ZombieOrdinario(float prob)
        {
            Probabilita = prob;
        }
        public ZombieOrdinario() { }

        public override int Vita
        {
            get => vita;    
            set
            {
                lock (LockVita)
                {
                    vita = value;
                    if (value <= 0)
                        lock (Program.LockZombie)
                        {
                            Program.mappa_zombie[fila].Remove(this);
                        }
                }
            }
        }

        private void Mov_Zombie_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sprite.Position.X >= 20)
            {
                lock (LockVel)
                {
                    sprite.Position -= new Vector2f(rallentamenti.Count > 0 ? Vel / 100 * (100 - RallMax()) : Vel, 0);
                }
            }
            else
            {
                lock (Program.LockZombie)
                {
                    Program.mappa_zombie[fila].Remove(this);
                }
                ((Timer)sender).Stop();
            }
            int RallMax()
            {
                int r;
                lock (LockVel)
                {
                    r = rallentamenti[0].ValRall;
                    for (int i = 0; i < rallentamenti.Count; i++)
                        if (rallentamenti[i].ValRall > r)
                            r = rallentamenti[i].ValRall;
                }
                return r;
            }
        }
        private void mangia_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Mangia())
                lock (p.LockVita)
                    p.Vita -= danno;
        }
        private bool Mangia()
        {
            bool mangia = false;
            for (int i = 8; i >= 0; i--)
            {
                Pianta p = Program.mappa_piante[i, fila];
                if (p == null)
                    continue;
                Logger.WriteLine((sprite.Position.X).ToString(), 6);
                Logger.WriteLine((sprite.Position.X < p.sprite.Position.X + p.sprite.Texture.Size.X * Math.Abs(p.sprite.Scale.X)).ToString(), 6);
                if (sprite.Position.X > p.sprite.Position.X && sprite.Position.X < p.sprite.Position.X + p.sprite.Texture.Size.X * Math.Abs(p.sprite.Scale.X))
                {
                    this.p = p; 
                    mangia = true; 
                    break;
                }
            }
            return mangia;
        }

        public override ZombieOrdinario GetInstance()
        {
            return new ZombieOrdinario();
        }
        public override ZombieOrdinario GetInstance(int y)
        {
            return new ZombieOrdinario(y);
        }
    }
}