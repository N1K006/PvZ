﻿using System;
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
	class SemeBrina : Seme
	{
		Timer Mov_Seme = new Timer(5);
		Zombie z;
		int DurataRall;
		int ValoreRall;

		public SemeBrina(Vector2f p, int danno, int ValoreRall, int DurataRall)
		{
			this.ValoreRall = ValoreRall;
			this.DurataRall = DurataRall;

			this.danno = danno;
			velocita = 4;

			circle = new CircleShape(10, 10)
			{
				FillColor = Color.Cyan,
				Origin = new Vector2f(5, 5),
				Position = new Vector2f(254 + p.X * 81 + 60, 100 + p.Y * 100 + 10)
			};

			fila = (int)p.Y;

			lock (Program.LockSemi)
				semi.Add(this);
			Mov_Seme.Elapsed += Mov_Seme_Elapsed;
			Mov_Seme.Enabled = true;
		}

		protected override void Mov_Seme_Elapsed(object sender, ElapsedEventArgs e)
		{
			circle.Position = new Vector2f(circle.Position.X + velocita, circle.Position.Y);
			if (circle.Position.X >= 1060)
			{
				lock (Program.LockSemi)
					semi.Remove(this);
				return;
			}
			z = ZombieColpito();
			if (z != null)
			{
				lock (z.LockVita)
					z.Vita -= danno;
				lock (z.LockVel)
					z.rallentamenti.Add(new Rallentamento(ValoreRall, DurataRall, z));
				lock (Program.LockSemi)
					semi.Remove(this);
				Mov_Seme = null;
				fila = 5;
			}
		}

		Zombie ZombieColpito()
		{
			Zombie Zaux = null;

			lock (Program.LockZombie)
			{
				if (fila != 5)
                {
					for (int i = 0; i < Program.mappa_zombie[fila].Count; i++)
						if (Zaux == null && Tocca(Program.mappa_zombie[fila][i]))
							try
							{
								lock (Program.LockZombie)
								{
									if (Program.mappa_zombie[fila][i] != null)
										Zaux = Program.mappa_zombie[fila][i];
								}
							}
							catch (Exception) { }
						else if (Zaux != null)
							if (Zaux.sprite.Position.X > Program.mappa_zombie[fila][i].sprite.Position.X && Tocca(Program.mappa_zombie[fila][i]))
								try
								{
									if (Program.mappa_zombie[fila][i] != null)
										Zaux = Program.mappa_zombie[fila][i];

								}
								catch (Exception) { }
				}
			}
			return Zaux;

			bool Tocca(Zombie z)
			{
				try
				{
					if (z != null && z.sprite.Texture != null)
						if (circle.Position.X - 5 < z.sprite.Position.X + (z.sprite.Texture.Size.X * Math.Abs(z.sprite.Scale.X) - 59) &&
						circle.Position.X + 5 > z.sprite.Position.X - 50)
							return true;
				}
				catch (Exception) { }
				return false;
			}
		}
	}
}
