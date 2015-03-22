using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Graphics g;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = panel1.CreateGraphics();

        }
        Plansza x = null;
        private void panel1_Click(object sender, EventArgs e)
        {
            Point point = panel1.PointToClient(Cursor.Position);
            x.DodajPunkt(point);



        }


        private void button1_Click(object sender, EventArgs e)
        {
            x = new Plansza(panel1, g);
            x.Start();
            x.speed = 10000 / (x.width * x.width);
            ti = new Thread(new ThreadStart(x.Initialize));
            tD = new Thread(new ThreadStart(x.Draw));
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel1.Size = new System.Drawing.Size(ClientSize.Width - 24, ClientSize.Height - 24);
            x = new Plansza(panel1, g);
        }
        bool started = false;
        Thread ti ;
        Thread tD ;

        private void button2_Click(object sender, EventArgs e)
        {

            if (started == false)
            {
                started = true;
                button2.Text = "Stop";
                ti.Start();
                Thread.Sleep(200);
                tD.Start();

            }
            else
            {
                button2.Text = "Start";
                started = false;
                ti.Abort();
                tD.Abort();
                ti = new Thread(new ThreadStart(x.Initialize));
                tD = new Thread(new ThreadStart(x.Draw));
            }
        }

    }
}
