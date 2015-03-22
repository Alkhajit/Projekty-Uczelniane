using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    class Plansza
    {
        public int width;
        int height;
        int rozmiar;
        Graphics graf;
        bool[,] Punkty;
        bool[,] temp;
        int time =0;
        public int speed = 100;
        public delegate bool Zasada(bool[,] a, int x, int y);
        List<Bitmap> przebieg = new List<Bitmap>();
        Zasada Check;
        //Konstruktory
        public Plansza(Graphics g)
        {
            width = 100;
            height = 100;
            rozmiar = 10;
            graf = g;
            Punkty = new bool[width, height];
        }
        public Plansza(Panel a, Graphics g)
        {
            width = a.Width / 10;
            height = a.Height / 10;
            rozmiar = 10;
            graf = g;
            Check = Game_Of_Life;
            Punkty = new bool[width + 1, height + 1];

        }
        public Plansza(Panel a, Graphics g, int Time)
        {
            width = a.Width / 10;
            height = a.Height / 10;
            rozmiar = 10;
            graf = g;
            time = Time;
            Punkty = new bool[width + 1, height + 1];

        }
        public Plansza(Panel a, Graphics g, Zasada zasada)
        {
            width = a.Width / 10;
            height = a.Height / 10;
            rozmiar = 10;
            graf = g;
            Check = zasada;
            Punkty = new bool[width + 1, height + 1];

        }

        public Plansza(Panel a, int rozmiar, int Time, Graphics g)
        {
            this.rozmiar = rozmiar;
            width = a.Width / rozmiar;
            height = a.Height / rozmiar;
            graf = g;
            time = Time;
            Punkty = new bool[width + 1, height + 1];
        }

        public void Start()
        {


            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    graf.DrawRectangle(Pens.DarkSlateGray, i * rozmiar, j * rozmiar, rozmiar, rozmiar);
                }
            }
        }

        public void DodajPunkt(Point p)
        {
            int x = (p.X / rozmiar);
            int y = (p.Y / rozmiar);
            if (Punkty[x, y] == false)
            {
                graf.FillEllipse(Brushes.Cyan, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                Punkty[x, y] = true;
            }
            else
            {
                graf.FillEllipse(Brushes.Black, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                Punkty[x, y] = false;


            }
        }
        public void DrawPlansza()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //graf.DrawRectangle(Pens.White, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                    graf.FillEllipse(Brushes.Cyan, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                }
            }
        }
        private bool Draw(bool[,] a)
        {
            Bitmap xx = new Bitmap(width * rozmiar, height * rozmiar);
            Graphics graf = Graphics.FromImage(xx);
            graf.Clear(Color.Black);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (a[x, y] == true)
                    {
                        graf.DrawRectangle(Pens.DarkSlateGray, width * rozmiar, height * rozmiar, rozmiar, rozmiar);
                        graf.FillEllipse(Brushes.Cyan, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                    }
                    else
                    {
                        graf.DrawRectangle(Pens.DarkSlateGray, width * rozmiar, height * rozmiar, rozmiar, rozmiar);
                        graf.FillEllipse(Brushes.Black, x * rozmiar, y * rozmiar, rozmiar, rozmiar);
                    }
                }
            }
            if (przebieg.Count > 0)
            {
                if ((xx == przebieg.Last())) return false;
                else { przebieg.Add(xx); return true; }
            }
            else { przebieg.Add(xx); return true; }
        }
        public void Draw()
        {
            time++;
            
            graf.DrawImageUnscaled(przebieg[time], new Point(0, 0));
            Thread.Sleep(200);
            if (time < przebieg.Count())
                Draw();
            else time = 0;
        }
        private void KolejnyRuch(bool[,] a, bool[,] temp)
        {
            Przepisz(a, out temp);
            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    temp[x, y] = Check(a, x, y);
                }
            }
            if(Draw(temp)==true&przebieg.Count<10000)
            {
                a = temp;
                KolejnyRuch(a, temp);
            }
            
        }
        private bool Game_Of_Life(bool[,] a, int x, int y)
        {
            int c = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j < a.GetLength(1) & i < a.GetLength(0))
                    {
                        if (i >= 0 & j >= 0)
                        {
                            if (i == x & j == y) { }
                            else
                                if (a[i, j] == true) c++;
                        }
                    }
                }
            }
            if (a[x, y] == true & (c == 2 | c == 3)) return true;
            else if (a[x, y] == false & c == 3) return true;
            else return false;
        }
        private void Przepisz(bool[,] a, out bool[,] temp)
        {
            temp = new bool[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    temp[i, j] = (bool)a[i, j];
                }
            }
        }
        public void Change_Check(int i)
        {

        }
        public void Initialize()
        {
            KolejnyRuch(Punkty, temp);
        }
    }
}
