using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class Hero
{
    public int X, Y;
    public Bitmap Img;
    public Hero()
    {
        X = 100; Y=500;
        Img = new Bitmap("Hero.png");
    }
}

namespace Shadow_Fight___Long_Game
{
    public partial class Form1 : Form
    {
        //Bitmaps
        Bitmap off;
        //Lists
        List<Hero> heroList=new List<Hero>();
        public Form1()
        {
            WindowState= FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           DrawDubb(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off=new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            InitializeGame();
        }
        void InitializeGame()
        {
            //Initializing when Pressing R
            heroList.Clear();
            // Creating Hero
            Hero monem = new Hero(); heroList.Add(monem);

        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            //Hero
            for(int i = 0; i < heroList.Count; i++)
            {
                g.DrawImage(heroList[i].Img, heroList[i].X, heroList[i].Y);
            }
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

    }
}
