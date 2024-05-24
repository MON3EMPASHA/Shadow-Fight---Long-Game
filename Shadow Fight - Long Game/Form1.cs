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
    public int X, Y,iframe,speed,flagjump;
    public List<Bitmap> ImgsR,ImgsL,ImgsJ;
    public Hero()
    {
        X = 100; Y=490; iframe = 0;speed = 10; flagjump = 0;
        //Right Movimg
        ImgsR = new List<Bitmap>();
        Bitmap monem = new Bitmap("HeroR.png");
        ImgsR.Add(monem);
        for (int i = 1; i < 15; i++)
        {
            Bitmap monem2 = new Bitmap("herorunR/"+i+".png");
            ImgsR.Add(monem2);
        }
        //Left Moving
        ImgsL = new List<Bitmap>();
        monem = new Bitmap("HeroL.png");
        ImgsL.Add(monem);
        for (int i = 1; i < 15; i++)
        {
            Bitmap monem2 = new Bitmap("herorunL/" + i + ".png");
            ImgsL.Add(monem2);
        }
        //jump
        ImgsJ = new List<Bitmap>();
        for (int i = 1; i < 15; i++)
        {
            Bitmap monem2 = new Bitmap("jump/"+i+".png");
            ImgsJ.Add(monem2);
        }
    }
}
class AdvancedImage
{
    public int W, H;
    public Rectangle Rects, Rectd;
    public Bitmap Img;
    public AdvancedImage(int w, int h, Rectangle rects, Rectangle rectd, Bitmap img)
    {
        W = w;
        H = h;
        Rects = rects;
        Rectd = rectd;
        Img = img;
    }
}
namespace Shadow_Fight___Long_Game
{
    public partial class Form1 : Form
    {
        //Bitmaps
        Bitmap off;
        //Lists
        List<Hero> heroList=new List<Hero>(); List<AdvancedImage> background = new List<AdvancedImage>();
        //Flags
        int flagheroleft = 0; Boolean flag_moving = false;
        //Timer
        Timer tt = new Timer();
        //counts
        int ctjumptick = 0;
        public Form1()
        {
            WindowState= FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            tt.Tick += Tt_Tick; tt.Interval = 100; tt.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            flag_moving = false;
            heroList[0].iframe = 0;
            DrawDubb(CreateGraphics());

        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            Tick();
            DrawDubb(CreateGraphics());
        }
        void Tick()
        {
            if (heroList[0].flagjump == 1)
            {
                if (ctjumptick == 1)
                {
                    if (heroList[0].iframe < 7) { heroList[0].Y -= heroList[0].speed; }
                    else if (heroList[0].iframe >= 7) { heroList[0].Y += heroList[0].speed; }
                    heroList[0].iframe++;
                    if (heroList[0].iframe == 14) { heroList[0].iframe = 0; heroList[0].flagjump = 0; }
                    ctjumptick = 0;
                }
                ctjumptick++;
            }
            DrawDubb(CreateGraphics());
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (heroList[0].flagjump == 0)
                    {
                        heroList[0].iframe++;
                        heroList[0].X -= heroList[0].speed;
                        if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                        flagheroleft = 1;
                        flag_moving = true;
                    }
                    break;
                case Keys.Right:
                    if (heroList[0].flagjump == 0)
                    {
                        heroList[0].iframe++;
                        heroList[0].X += heroList[0].speed;
                        if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                        flagheroleft = 0;
                        flag_moving = true;
                    }
                    break;
                case Keys.Up:
                    if (heroList[0].flagjump == 0)
                    {
                        heroList[0].flagjump = 1;
                        heroList[0].iframe = 0;
                    }
                    break;
            
            }
            Tick();
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
            //Background
            Rectangle s = new Rectangle(0,0, new Bitmap("background/1.jpg").Width, new Bitmap("background/1.jpg").Height); Rectangle d = new Rectangle(0,0, this.ClientSize.Width, this.ClientSize.Height);
            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height,s,d,new Bitmap ("background/1.jpg"));
            background.Add(pnn);
            
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            //background
            for(int i = 0; i < background.Count; i++)
            {
                g.DrawImage(background[i].Img, background[i].Rectd, background[i].Rects, GraphicsUnit.Pixel);
            }
            //Hero
            for (int i = 0; i < heroList.Count; i++)
            {
                if (flagheroleft == 1 && heroList[0].flagjump==0)
                {
                    g.DrawImage(heroList[i].ImgsL[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsL[heroList[i].iframe].Width*2, heroList[i].ImgsL[heroList[i].iframe].Height * 2);
                }
                else if (flagheroleft == 0 && heroList[0].flagjump == 0)
                {
                    g.DrawImage(heroList[i].ImgsR[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsR[heroList[i].iframe].Width * 2, heroList[i].ImgsR [heroList[i].iframe].Height * 2);
                }
                else if(heroList[0].flagjump == 1)
                {
            
                    g.DrawImage(heroList[i].ImgsJ[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsJ[heroList[i].iframe].Width*4, heroList[i].ImgsJ[heroList[i].iframe].Height*4);

                }

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
