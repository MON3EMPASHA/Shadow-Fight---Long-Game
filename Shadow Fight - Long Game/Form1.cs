using Shadow_Fight___Long_Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;


class HealthBar
{
    public int X, Y, Width, Height;
    public int MaxHealth;
    public int CurrentHealth;

    public HealthBar(int x, int y, int width, int height, int maxHealth)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
}

class Hero
{
    public int X, Y,iframe,speed,flagjump,flagkill;
    public List<Bitmap> ImgsR,ImgsL,ImgsJ,ImgsK1;
    public HealthBar HealthBarr;

    public Hero()
    {
        X = 100; Y=490; iframe = 0;speed = 30; flagjump = 0; flagkill = 0;
        //Health bar
        HealthBarr = new HealthBar(120, 30, 270, 20, 100);
        //Right Movimg
        {
            ImgsR = new List<Bitmap>();
            Bitmap monem = new Bitmap("HeroR.png");
            ImgsR.Add(monem);
            for (int i = 1; i < 15; i++)
            {
                Bitmap monem2 = new Bitmap("herorunR/" + i + ".png");
                ImgsR.Add(monem2);
            }
        }
        //Left Moving
        {
            ImgsL = new List<Bitmap>();
            Bitmap monem = new Bitmap("HeroL.png");
            ImgsL.Add(monem);
            for (int i = 1; i < 15; i++)
            {
                Bitmap monem2 = new Bitmap("herorunL/" + i + ".png");
                ImgsL.Add(monem2);
            }
        }
        //jump
        {
            ImgsJ = new List<Bitmap>();
            for (int i = 1; i < 20; i++)
            {
                Bitmap monem2 = new Bitmap("jump/" + i + ".png");
                ImgsJ.Add(monem2);
            }
        }
        //Kill1
        {
            ImgsK1 = new List<Bitmap>();
            for (int i = 2; i < 12; i++)
            {
                Bitmap monem2 = new Bitmap("kill1/k" + i + ".png");
                ImgsK1.Add(monem2);
            }
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
            tt.Tick += Tt_Tick; tt.Interval = 50; tt.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            flag_moving = false;
            heroList[0].iframe = 0;
            heroList[0].Y = 490;
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
                    if (heroList[0].iframe < 9) { heroList[0].Y -= heroList[0].speed; }
                    else if (heroList[0].iframe >= 9) { heroList[0].Y += heroList[0].speed; }
                    if (heroList[0].iframe == 16) { heroList[0].Y+= heroList[0].speed*3 ; }
                    if (heroList[0].iframe == 18) { heroList[0].iframe = 0; heroList[0].flagjump = 0; flag_moving = false; heroList[0].Y -= heroList[0].speed * 3; }
                    ctjumptick = 0;
                    heroList[0].iframe++;
                }
                ctjumptick++;
            }
            else if( heroList[0].flagkill == 1)
            {
                if (heroList[0].iframe == 5) { heroList[0].iframe = 0; heroList[0].flagkill = 0; heroList[0].Y = 490;}
                heroList[0].iframe++;
                heroList[0].Y = 525;
                if (heroList[0].iframe >=3) { heroList[0].Y = 560; }
            }

            if (flag_moving && heroList[0].X>399)
            {
                for (int i = 0; i < background.Count; i++)
                {
                    int s = 0;
                    if (background.Count > 5)
                    {
                        if (i != 1)
                        {
                            if (i == 0) { s = 1/2; }
                            if (i == 2) { s = 2; }
                            if (i == 3) { s = 3; }
                            if (i == 4) { s = 4; }
                            if (i == 5) {  s = 5; }
                            if (background[i].Rects.X + background[i].Rects.Width < background[i].Img.Width - 5)
                            {
                                background[i].Rects.X += s;
                            }
                            else
                            {
                                background[i].Rects.X = 0;
                            }
                        }
                    }
                }
            }
            DrawDubb(CreateGraphics());
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                    {
                        heroList[0].iframe++;
                      //  if (heroList[0].X > 400)
                        heroList[0].X -= heroList[0].speed;
                        if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                        flagheroleft = 1;
                        flag_moving = true;
                        heroList[0].Y = 540;
                        Tick();
                    }
                    break;
                case Keys.Right:
                    if (heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                    {
                        heroList[0].iframe++;
                        if(heroList[0].X < 400)
                        heroList[0].X += heroList[0].speed;
                        if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                        flagheroleft = 0;
                        flag_moving = true;
                        heroList[0].Y = 540;
                        Tick();
                    }
                    break;
                case Keys.Up:
                    if (heroList[0].flagjump == 0 && heroList[0].flagkill==0)
                    {
                        heroList[0].flagjump = 1;
                        heroList[0].iframe = 0;
                        Tick();
                    }
                    break;
                case Keys.Space:
                    if (heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                    {
                        heroList[0].flagkill = 1;
                        heroList[0].iframe = 0;
                        Tick();
                    }
                    break;
            }
            
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
            for (int i = 1; i <= 7; i++)
            {
                string filePath = $"background2/L{i}.png";
                Bitmap bitmap = new Bitmap(filePath);
                Rectangle sourceRect = new Rectangle(0, 0, bitmap.Width / (i == 2 || i == 7 ? 1 : 2), bitmap.Height);
                Rectangle destRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                background.Add(pnn);
            }
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
                //Move Left
                if (flagheroleft == 1 && heroList[0].flagjump==0 && heroList[0].flagkill == 0)
                {
                    g.DrawImage(heroList[i].ImgsL[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsL[heroList[i].iframe].Width*2, heroList[i].ImgsL[heroList[i].iframe].Height * 2);
                }
                //Move Right 
                else if (flagheroleft == 0 && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                {
                    g.DrawImage(heroList[i].ImgsR[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsR[heroList[i].iframe].Width * 2, heroList[i].ImgsR [heroList[i].iframe].Height * 2);
                }
                //jump
                else if(heroList[0].flagjump == 1 && heroList[0].flagkill == 0)
                {
                    g.DrawImage(heroList[i].ImgsJ[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsJ[heroList[i].iframe].Width*4, heroList[i].ImgsJ[heroList[i].iframe].Height*4);
                }
                //Kill
                else if (heroList[0].flagkill == 1 && heroList[0].flagjump == 0)
                {
                    g.DrawImage(heroList[i].ImgsK1[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsK1[heroList[i].iframe].Width * 4, heroList[i].ImgsK1[heroList[i].iframe].Height * 4);
                }
                //Hero Health bar
                g.FillRectangle(Brushes.Gray, heroList[0].HealthBarr.X, heroList[0].HealthBarr.Y, heroList[0].HealthBarr.Width, heroList[0].HealthBarr.Height);
                int currentHealthWidth = (int)((heroList[0].HealthBarr.CurrentHealth / (float)heroList[0].HealthBarr.MaxHealth) * heroList[0].HealthBarr.Width);
                g.FillRectangle(Brushes.Red, heroList[0].HealthBarr.X, heroList[0].HealthBarr.Y, currentHealthWidth, heroList[0].HealthBarr.Height);
            }
            //Health Bar
            g.DrawImage(new Bitmap("heroHealthBar.png"), 1, 0);
        
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

    }
}
