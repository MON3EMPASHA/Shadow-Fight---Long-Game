using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management.Instrumentation;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;


class HealthBar
{
    public int X, Y, Width, Height;
    public int MaxHealth;
    public float CurrentHealth;

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
class HeroActor
{
    public int x = 0, y = 0, Widht = 0, Heigth = 0, iFrame = 0, maxFrames = 0, dir = 0, jumbFlag = 0, health = 60, healthBar = 60;
    public List<Bitmap> Frames, walk, jumb, die;
    public string imageFileName = "";
    public Bitmap image;
    public bool eating = false;
    public HeroActor(int x, int y, int width, int height, string imageFileName, bool centeralized, bool bg)
    {

        this.x = x;
        this.y = y;
        this.Widht = width;
        this.Heigth = height;
        this.image = new Bitmap(imageFileName);
        if (this.Widht > 0 && this.Heigth > 0)
            this.image = new Bitmap(image, width, height);
        if (centeralized)
        {
            this.x -= this.image.Width / 2;
            this.y -= this.image.Height / 2;
        }
        if (bg)
            this.image.MakeTransparent(this.image.GetPixel(0, 0));


        // Custom Frames
        //Frames = walk = jumb = die = new List<Bitmap>();
        Frames = new List<Bitmap>();
        walk = new List<Bitmap>();
        jumb = new List<Bitmap>();
        die = new List<Bitmap>();

        if (width > 0 && height > 0)
        {
            for (int i = 1; i < 3; i++)
                walk.Add(new Bitmap(new Bitmap("Man/walk/" + i + ".png"), width, height));

            for (int i = 1; i < 9; i++)
                jumb.Add(new Bitmap(new Bitmap("Man/jumb/" + i + ".png"), width, height));

            for (int i = 1; i < 7; i++)
                die.Add(new Bitmap(new Bitmap("Man/Die/" + i + ".png"), width, height));
        }
        else
        {
            for (int i = 1; i < 3; i++)
                walk.Add(new Bitmap("Man/walk/" + i + ".png"));

            for (int i = 1; i < 9; i++)
                jumb.Add(new Bitmap("Man/jumb/" + i + ".png"));

            for (int i = 1; i < 7; i++)
                die.Add(new Bitmap("Man/Die/" + i + ".png"));
        }
        Frames = walk;
        iFrame = 0;
        maxFrames = walk.Count;
    }

    public void setImage(string imageFileName, int width, int height)
    {
        this.imageFileName = imageFileName;
        if (width > 0 && height > 0)
            image = new Bitmap(new Bitmap(imageFileName), width, height);
        else
            image = new Bitmap(imageFileName);

    }

    public void addFrame(string frameName, int width, int height)
    {
        maxFrames++;
        if (width > 0 && height > 0)
            Frames.Add(new Bitmap(new Bitmap(frameName), width, height));


        else
            Frames.Add(new Bitmap(frameName));
    }
    public void Next()
    {
        if (iFrame == maxFrames)
            return;


        if (iFrame == 0 && maxFrames > 1)
            dir = 1;
        if (iFrame == maxFrames - 1)
            dir = -1;
        if (dir != 0)
        {
            iFrame += dir;
            image = Frames[iFrame];
        }
    }
    public void setFrames(List<Bitmap> newFrames)
    {
        iFrame = 0;
        maxFrames = newFrames.Count;
        Frames = newFrames;
    }
    public void reSet()
    {
        Frames = new List<Bitmap>();
        iFrame = 0;
        maxFrames = 0;
    }
}
class Hero
{
    public int X, Y, iframe, speed, flagjump, flagkill;
    public List<Bitmap> ImgsR, ImgsL, ImgsJ, ImgsK1;
    public HealthBar HealthBarr;
    public bool show = true;

    public Hero(int x, int y, bool show)
    {
        this.show = show;
        X = 100; Y = 490;
        iframe = 0;
        speed = 30;
        flagjump = 0;
        flagkill = 0;
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
    public System.Drawing.Rectangle Rects, Rectd;
    public Bitmap Img;
    public AdvancedImage(int w, int h, System.Drawing.Rectangle rects, System.Drawing.Rectangle rectd, Bitmap img)
    {
        W = w;
        H = h;
        Rects = rects;
        Rectd = rectd;
        Img = img;
    }
}
class Enemy
{
    public int X, Y, W, H, iframe;
    public List<Bitmap> Idle = new List<Bitmap>(), k1 = new List<Bitmap>();
    public string flaglist;
    public HealthBar HealthBarr;

    public Enemy(int type)
    {
        //Health bar
        HealthBarr = new HealthBar(1200, 30, 270, 20, 100);
        //
        iframe = 0;
        //Create Enemy 2
        switch (type)
        {
            case 2:
                flaglist = "Idle";
                X = 700; Y = 500;
                //Stand state
                for (int i = 1; i <= 9; i++)
                {
                    Bitmap ssw = new Bitmap($"Enmy2/idle/{i}.png");
                    ssw.MakeTransparent(ssw.GetPixel(0, 0));
                    Idle.Add(ssw);
                }
                //kill 1  movements
                for (int i = 1; i <= 27; i++)
                {
                    Bitmap ssw = new Bitmap($"Enmy2/k1/{i}.png");
                    ssw.MakeTransparent(ssw.GetPixel(0, 0));
                    k1.Add(ssw);
                }
                break;
        }
    }
}
class Rectangle
{
    public int x = 0, y = 0, Widht = 0, Heigth = 0;
    public Color cl = Color.Red;
    public Rectangle(int x, int y, int widht, int heigth, Color cl)
    {
        this.x = x;
        this.y = y;
        this.Widht = widht;
        this.Heigth = heigth;
        this.cl = cl;
    }
}
namespace Shadow_Fight___Long_Game
{
    public partial class Form1 : Form
    {
        //Bitmaps
        Bitmap off;

        //Lists
        List<Hero> heroList = new List<Hero>();
        List<AdvancedImage> background = new List<AdvancedImage>();
        List<Enemy> enemylist = new List<Enemy>();
        List<HeroActor> Bullets = new List<HeroActor>();
        List<HeroActor> Zombies = new List<HeroActor>();
        //List<int> trackZombies = new List<int>();
        List<HeroActor> trackZombies2 = new List<HeroActor>();


        //Flags
        int flagheroleft = 0; bool flag_moving = false;
        int zombieBackground = 3, bossBackground = 4, heroIndex = 1;
        bool flagtale3selm = false; bool flagfall = false;
        bool leaveHeroAlone = false; bool tl3asanser = false;

        //Timer
        Timer tt = new Timer();

        //counts
        int ctenmy2tick = 0;
        int timerCounter = -1, timerLimit = 50;
        int ctDie = 0;
        //backfround 
        int backGroundIndex = 3; // it could be 1 or 2 or 3 or 4 or 5
        int bcgrondiframe = 0; // used in the fifth background only


        // Actors
        HeroActor Hero;
        Rectangle laser;
        HeroActor leadder;
        HeroActor Elevator;
        int bulletsLimit = 50;
        int heroHealth = 270, zombieEatings = 0, zombieDead = 0, zombieDeadLimit = 1;
        bool finishRound = false;
        int enemyDamage = 3;

        public Form1()
        {
            WindowState = FormWindowState.Maximized;
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
            laser = null;

            if (flagtale3selm)
            {
                flagtale3selm = false;
                flagfall = true;
            }
            // DrawDubb(CreateGraphics());
        }

        void handleHeroEvents()
        {
            // -1. zombie damage hero
            heroHealth -= zombieEatings * 1;


            // 0. moveZombies
            for (int i = 0; i < Zombies.Count; i++)
            {
                HeroActor zombie = Zombies[i];
                if (zombie.health > 0 && zombie.eating == false)
                    zombie.x -= 5;
                if (Hero != null)
                {
                    if (zombie.x - 20 < Hero.x + Hero.image.Width - 50 && zombie.x - 20 > Hero.x && zombie.eating == false)
                    {
                        zombie.eating = true;
                        zombie.setFrames(zombie.jumb);
                        zombieEatings += 1;

                    }
                    if (zombie.eating && !(zombie.x - 20 < Hero.x + Hero.image.Width - 50 && zombie.x - 20 > Hero.x))
                    {
                        zombie.setFrames(zombie.walk);
                        zombie.eating = false;
                        zombieEatings -= 1;
                    }

                }
                zombie.Next();
            }

            // 1. Jumb
            int speed = 5, mid = 4, bulletSpeed = 30;
            if (Hero != null && Hero.jumbFlag != 0)
            {
                if (Hero.jumbFlag <= mid)
                {
                    Hero.x += speed * 3;
                    Hero.y -= speed * 3;
                    Hero.jumbFlag += 1;
                }
                if (Hero.jumbFlag > mid)
                {
                    Hero.x += speed * 3;
                    Hero.y += speed * 3;
                    Hero.jumbFlag += 1;
                }
                if (Hero.jumbFlag == 9)
                {
                    Hero.jumbFlag = 0;
                    Hero.setFrames(Hero.walk);
                    Hero.iFrame = -1;
                }
                Hero.Next();
            }

            // 2. Bullets
            for (int i = 0; i < Bullets.Count; i++)
            {
                HeroActor bullet = Bullets[i];
                bullet.x += bulletSpeed;
                if (bullet.x > ClientSize.Width)
                {
                    Bullets.RemoveAt(i);
                    i--;
                }
                int range = 10;
                for (int j = 0; j < Zombies.Count; j++)
                {
                    HeroActor zombie = Zombies[j];
                    int point = bullet.x + bullet.image.Width + range;
                    if (point > zombie.x && point < zombie.x + zombie.image.Width && bullet.y + 20 > zombie.y && zombie.health > 0)
                    {
                        zombie.health -= 20;
                        Bullets.RemoveAt(i);
                        if (zombie.health <= 0)
                        {
                            //trackZombies.Add(j);
                            trackZombies2.Add(zombie);
                            zombie.setFrames(zombie.die);
                            // zombieDead += 1;
                        }
                    }

                }
                if (backGroundIndex == bossBackground)
                {
                    for (int k = 0; k < enemylist.Count; k++)
                    {
                        if (Bullets[i].x > enemylist[k].X && Bullets[i].x < enemylist[k].X + enemylist[k].Idle[0].Width)
                        {
                            enemylist[k].HealthBarr.CurrentHealth -= 5;
                            Bullets.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // 3. Track Dead Zombies with indexes
            for (int i = 0; i < trackZombies2.Count; i++)
            {
                if (Zombies.Count > 0)
                {
                    //int index = trackZombies[i];
                    HeroActor zombie = trackZombies2[i];
                    if (zombie.iFrame == zombie.maxFrames - 1 && zombie.health <= 0)
                    {
                        for (int j = 0; j < Zombies.Count; j++)
                        {
                            if (zombie == Zombies[j])
                            {
                                trackZombies2.RemoveAt(i);
                                Zombies.RemoveAt(j);
                            }
                        }
                        zombieDead += 1;
                    }
                }
            }

        }
        void createZombie()
        {

            HeroActor zombie = new HeroActor(ClientSize.Width, 625, 0, 0, "Zombies/Zombie1/zombiewalk (1).png", false, false);
            zombie.Frames = new List<Bitmap>();
            zombie.walk = new List<Bitmap>();
            zombie.jumb = new List<Bitmap>();
            zombie.die = new List<Bitmap>();
            zombie.setFrames(zombie.walk);

            for (int i = 1; i <= 93; i++)
                zombie.walk.Add(new Bitmap($"Zombies/Zombie1/zombiewalk ({i}).png"));

            for (int i = 1; i <= 29; i++)
                zombie.die.Add(new Bitmap($"Zombies/Zombie3/zombiedeath ({i}).png"));

            for (int i = 1; i <= 40; i++)
                zombie.jumb.Add(new Bitmap($"Zombies/Zombie2/zombieeat ({i}).png"));

            zombie.setFrames(zombie.walk);
            Zombies.Add(zombie);
        }
        private void Tt_Tick(object sender, EventArgs e)
        {
            Tick();
            DrawDubb(CreateGraphics());
        }
        void Tick()
        {
            if (enemylist.Count > 0 && enemylist[0].flaglist == "k1")
            {
                Enemy boss = enemylist[0];
                int range = 50;
                int condition = boss.X - Hero.x + Hero.image.Width;
                if (Hero != null && heroHealth > 0 && condition < range && condition >= 0)
                    heroHealth -= 1;
            }
            finishRound = zombieDead >= zombieDeadLimit;
            if (timerCounter == timerLimit || timerCounter == -1)
            {
                timerCounter = 0;
                if (backGroundIndex == zombieBackground && !finishRound)
                    createZombie();
            }
            if (timerCounter == -1 && backGroundIndex == zombieBackground && zombieDead < zombieDeadLimit)
                createZombie();
            timerCounter++;
            handleHeroEvents();

            if (finishRound)
            {
                if (leadder == null && Zombies.Count == 0)
                {
                    leadder = new HeroActor(ClientSize.Width - 200, 200, 0, 0, "ladder.png", false, false);
                    Elevator = new HeroActor(ClientSize.Width - 430, 450, 0, 0, "elevator.png", false, false);
                }
            }

            if (heroList.Count > 0)
            {
                herojump();
                herokill();
            }
            MoveBackground();
            ManageAndMoveEnemies();
            if (flagfall)
            {
                if (Hero.y < 630)
                    Hero.y += 20;
                else flagfall = false;
            }
            etl3asanser();
            if (backGroundIndex == bossBackground && enemylist.Count > 0)
            {
                int range = 0;
                if (enemylist[0].X - range < Hero.x + Hero.Widht && enemylist[0].flaglist == "k1")
                {
                    // kill hero
                    heroHealth -= enemyDamage;

                }
            }
            if (Hero.health <= 0 && ctDie==0)
            {
                MessageBox.Show("Game Over");
                ctDie++;
            }
        }
        void etl3lader(int index)
        {
            Hero.y -= 5;
            if (Hero.y < 300)
            {
                backGroundIndex = 4;
                Hero.x = 50;
                leaveHeroAlone = true;


                InitializeGame();
            }
        }
        void etl3asanser()
        {
            if (tl3asanser)
            {
                if (Hero.y > 250)
                {
                    Hero.y -= 5;
                    Elevator.y -= 5;
                }
                else
                {
                    backGroundIndex = 4;
                    Hero.x = 50;
                    leaveHeroAlone = true;
                    InitializeGame();
                    tl3asanser = false;
                }
            }
        }
        void ManageAndMoveEnemies()
        {
            for (int i = 0; i < enemylist.Count; i++)
            {
                Enemy enemy = enemylist[i];

                if (backGroundIndex == bossBackground && enemy.flaglist == "Idle")
                {
                    if (enemylist[0].X + 5 > Hero.x + Hero.Widht)
                        enemylist[0].X -= 5;

                    ctenmy2tick++;
                    if (ctenmy2tick >= 25)
                    {
                        enemy.flaglist = "k1";
                        enemy.iframe = 0;
                        ctenmy2tick = 0;
                    }
                }

                enemy.iframe++;
                List<Bitmap> temp = new List<Bitmap>();

                if (enemy.flaglist == "Idle")
                    temp = enemy.Idle;

                else if (enemy.flaglist == "k1")
                    temp = enemy.k1;

                if (enemy.iframe == temp.Count)
                {
                    if (enemy.flaglist == "k1")
                    {
                        enemy.flaglist = "Idle";
                        enemy.iframe = 0;
                    }
                    else
                        enemy.iframe = 0;
                }
            }
        }
        void herojump()
        {

            if (heroList.Count > 0 && heroList[0].flagjump == 1)
            {
                if (heroList[0].iframe < 9) { heroList[0].Y -= heroList[0].speed; }
                else if (heroList[0].iframe >= 9) { heroList[0].Y += heroList[0].speed; }
                if (heroList[0].iframe == 16) { heroList[0].Y += heroList[0].speed * 3; }
                if (heroList[0].iframe == 18) { heroList[0].iframe = 0; heroList[0].flagjump = 0; flag_moving = false; heroList[0].Y -= heroList[0].speed * 3; }
                heroList[0].iframe++;

            }
        }
        void herokill()
        {
            if (heroList.Count > 0 && heroList[0].flagkill == 1 && heroList[0].flagjump == 0)
            {
                if (heroList[0].iframe == 5) { heroList[0].iframe = 0; heroList[0].flagkill = 0; heroList[0].Y = 490; }
                heroList[0].iframe++;
                heroList[0].Y = 525;
                if (heroList[0].iframe >= 3) { heroList[0].Y = 560; }
            }
        }
        void MoveBackground()
        {
            if (flag_moving)
            {

                if ((heroIndex == 1 && Hero != null && Hero.x > 399 && (zombieDead < zombieDeadLimit) || backGroundIndex == 4) || (heroIndex == 2 && heroList[0].X > 399))
                {
                    for (int i = 0; i < background.Count; i++)
                    {
                        int s = 0;
                        switch (backGroundIndex)
                        {
                            case 2:
                                s = 0;
                                if (background.Count > 5)
                                {
                                    if (i != 1)
                                    {
                                        if (i == 0) { s = 1 / 2; }
                                        if (i == 2) { s = 2; }
                                        if (i == 3) { s = 3; }
                                        if (i == 4) { s = 4; }
                                        if (i == 5) { s = 5; }
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
                                break;
                            case 3:
                                s = 0;
                                if (background.Count > 2)
                                {
                                    if (i == 0) { s = 1; }
                                    if (i == 1) { s = 2; }
                                    if (i == 2) { s = 5; }

                                    if (background[i].Rects.X + background[i].Rects.Width < background[i].Img.Width - 5)
                                    {
                                        background[i].Rects.X += s;
                                    }
                                    else
                                    {
                                        background[i].Rects.X = 0;
                                    }
                                }
                                break;
                            case 4:
                                s = 0;
                                if (background.Count > 4)
                                {
                                    if (i == 0) { s = 1; }
                                    if (i == 1) { s = 2; }
                                    if (i == 2) { s = 3; }
                                    if (i == 3) { s = 4; }
                                    if (i == 4) { s = 5; }


                                    if (background[i].Rects.X + background[i].Rects.Width < background[i].Img.Width - 5)
                                    {
                                        background[i].Rects.X += s;
                                    }
                                    else
                                    {
                                        background[i].Rects.X = 0;
                                    }
                                }
                                break;
                        }
                    }

                }

            }
        }

        int firstZombie()
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                HeroActor zombie = Zombies[i];
                if (zombie.health > 0)
                    return i;
            }
            return -1;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int speed = 10;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    flag_moving = true;

                    switch (heroIndex)
                    {
                        case 2:
                            if (heroList[0] != null && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                            {
                                heroList[0].iframe++;
                                heroList[0].X -= heroList[0].speed;
                                if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                                flagheroleft = 1;
                                flag_moving = true;
                                heroList[0].Y = 540;
                            }
                            break;
                        case 1:

                            if (!flagtale3selm && !flagfall && !tl3asanser)
                            {
                                if (Hero.Frames[0] != Hero.walk[0])
                                    Hero.setFrames(Hero.walk);
                                Hero.x -= speed;
                                Hero.Next();
                            }
                            break;
                    }


                    break;
                case Keys.Right:
                    flag_moving = true;
                    switch (heroIndex)
                    {
                        case 2:
                            if (heroList[0] != null && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                            {
                                heroList[0].iframe++;
                                if (backGroundIndex != 1 && backGroundIndex != 5)
                                {
                                    if ((heroIndex == 2 && heroList[0].X < 400) || (heroIndex == 1 && Hero.x < 400))
                                        heroList[0].X += heroList[0].speed;
                                }
                                else { heroList[0].X += heroList[0].speed; }
                                if (heroList[0].iframe == 15) { heroList[0].iframe = 2; }
                                flagheroleft = 0;
                                flag_moving = true;
                                heroList[0].Y = 540;
                            }
                            break;
                        case 1:

                            if (!flagtale3selm && !flagfall && !tl3asanser)
                            {
                                if (Hero.Frames[0] != Hero.walk[0])
                                    Hero.setFrames(Hero.walk);
                                if ((heroIndex == 1 && Hero.x < 400) || zombieDead == zombieDeadLimit || finishRound)
                                    Hero.x += speed;

                                Hero.Next();
                            }
                            break;
                    }
                    break;
                case Keys.Up:
                    switch (heroIndex)
                    {
                        case 2:
                            if (heroList[0] != null && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                            {
                                heroList[0].flagjump = 1;
                                heroList[0].iframe = 0;
                            }
                            break;
                        case 1:
                            if (leadder != null && Elevator != null && Hero.x + Hero.image.Width / 2 > leadder.x && Hero.x + Hero.image.Width / 2 < leadder.x + leadder.image.Width)
                            {
                                flagtale3selm = true;
                                etl3lader(1);
                            }
                            else if (leadder != null && Elevator != null && Hero.x + Hero.image.Width / 2 > Elevator.x && Hero.x + Hero.image.Width / 2 < Elevator.x + Elevator.image.Width)
                            {
                                if (!tl3asanser)
                                {
                                    Hero.y -= 100;
                                    tl3asanser = true;
                                }
                            }
                            else
                            {
                                if (Hero.jumbFlag == 0)
                                {
                                    Hero.setFrames(Hero.jumb);
                                    Hero.jumbFlag = 1;
                                }
                            }
                            break;
                    }

                    break;
                case Keys.Space:
                    switch (heroIndex)
                    {
                        case 2:
                            if (heroList[0] != null && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                            {
                                heroList[0].flagkill = 1;
                                heroList[0].iframe = 0;
                            }
                            break;
                        case 1:
                            if (bulletsLimit > 0 && !tl3asanser)
                            {

                                Bullets.Add(new HeroActor(Hero.x + Hero.image.Width - 30, Hero.y + 25, 25, 10, "Man/bullet3.png", false, false));
                                bulletsLimit -= 1;
                            }
                            break;
                    }
                    break;
                case Keys.Enter:
                    // laser
                    int laserWidth = ClientSize.Width - Hero.x + Hero.image.Width - 30, zombieIndex = firstZombie();
                    HeroActor selectedZombie = null;
                    //MessageBox.Show("" + zombieIndex);
                    if (zombieIndex >= 0)
                    {
                        selectedZombie = Zombies[zombieIndex];
                        laserWidth = selectedZombie.x - Hero.x - Hero.image.Width + 20;

                    }

                    laser = new Rectangle(Hero.x + Hero.image.Width - 10, Hero.y + 30, laserWidth, 10, Color.Red);
                    if (zombieIndex >= 0)
                    {
                        selectedZombie.health -= 1;
                        if (selectedZombie.health <= 0)
                        {
                            //trackZombies.Add(0);
                            trackZombies2.Add(selectedZombie);
                            selectedZombie.setFrames(selectedZombie.die);
                            zombieDead += 1;
                        }
                    }


                    break;
                case Keys.R:
                    InitializeGame();
                    break;
                case Keys.Escape:

                    if (backGroundIndex < 5)
                        backGroundIndex++;
                    else
                        backGroundIndex = 1;
                    InitializeGame();
                    break;
                case Keys.Down:
                    if (heroIndex == 1)
                    {
                        //  heroIndex = 2;
                    }
                    else
                    {
                        // heroIndex = 1;
                    }
                    InitializeGame();
                    break;

            }

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            InitializeGame();
        }
        void InitializeGame()
        {
            //Initializing when Pressing R
            heroList.Clear(); background.Clear(); bcgrondiframe = 0; Bullets.Clear(); Zombies.Clear();
            enemylist.Clear();
            zombieEatings = 0;
            timerCounter = -1;
            Zombies.Clear();
            Hero = null;
            heroHealth = 270;
            bulletsLimit = 50;
            leadder = null;
            Elevator = null;
            trackZombies2.Clear();
            // Creating Hero
            int y;
            if (backGroundIndex != 5)
                y = 1000;
            else
                y = 490;

            bool show = false;
            if (heroIndex == 2)
                show = true;
            Hero monem = new Hero(100, y, show);
            heroList.Add(monem);
            if (heroIndex == 1)
                Hero = new HeroActor(100, 630, 150, 150, "Man/walk/1.png", false, false);
            if (backGroundIndex == bossBackground)
            {
                Enemy monemm = new Enemy(2);
                enemylist.Add(monemm);
            }
            //Background
            {
                switch (backGroundIndex)
                {
                    case 1:
                        for (int i = 1; i < 2; i++)
                        {
                            string filePath = $"backgrounds/background/{i}.jpg";
                            Bitmap bitmap = new Bitmap(filePath);
                            System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                            background.Add(pnn);
                        }
                        break;
                    case 2:
                        for (int i = 1; i <= 7; i++)
                        {
                            string filePath = $"backgrounds/background2/L{i}.png";
                            Bitmap bitmap = new Bitmap(filePath);
                            System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, bitmap.Width / (i == 2 || i == 7 ? 1 : 2), bitmap.Height);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                            background.Add(pnn);
                        }
                        break;
                    case 3:
                        for (int i = 1; i <= 3; i++)
                        {
                            string filePath = $"backgrounds/background3/{i}.png";
                            Bitmap bitmap = new Bitmap(filePath);
                            System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, bitmap.Width / 2, bitmap.Height);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                            background.Add(pnn);
                        }
                        break;
                    case 4:
                        for (int i = 1; i <= 5; i++)
                        {
                            string filePath = $"backgrounds/background4/{i}.png";
                            Bitmap bitmap = new Bitmap(filePath);
                            System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, bitmap.Width / 2, bitmap.Height);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                            background.Add(pnn);
                        }
                        break;
                    case 5:
                        for (int i = 1; i <= 16; i++)
                        {
                            string filePath = $"backgrounds/background5/{i}.png";
                            Bitmap bitmap = new Bitmap(filePath);
                            System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                            AdvancedImage pnn = new AdvancedImage(this.ClientSize.Width, this.ClientSize.Height, sourceRect, destRect, bitmap);
                            background.Add(pnn);
                        }

                        break;
                }

            }
        }
        void DrawListImages(List<HeroActor> images, Graphics g)
        {
            for (int i = 0; i < images.Count; i++)
            {
                HeroActor element = images[i];
                Bitmap currentImage = element.image;
                g.DrawImage(currentImage, element.x, element.y);
            }
        }
        void drawBackGround(Graphics g)
        {
            //background
            if (backGroundIndex != 5)
            {
                for (int i = 0; i < background.Count; i++)
                    g.DrawImage(background[i].Img, background[i].Rectd, background[i].Rects, GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(background[bcgrondiframe].Img, background[bcgrondiframe].Rectd, background[bcgrondiframe].Rects, GraphicsUnit.Pixel);
                bcgrondiframe++;
                if (bcgrondiframe == 16) { bcgrondiframe = 0; }
            }
        }
        void drawEnemy(Graphics g)
        {
            //Enmmy
            for (int i = 0; i < enemylist.Count; i++)
            {
                if (enemylist[i].HealthBarr.CurrentHealth > 0)
                {
                    Enemy e = enemylist[i];
                    List<Bitmap> temp = new List<Bitmap>();
                    if (e.flaglist == "Idle") { temp = e.Idle; }
                    else if (e.flaglist == "k1") { temp = e.k1; }
                    g.DrawImage(temp[e.iframe], e.X, e.Y, 385, 317);

                    //Health bar
                    HealthBar hb = e.HealthBarr;
                    float healthPercentage = (float)hb.CurrentHealth / hb.MaxHealth;
                    int healthBarWidth = (int)(hb.Width * healthPercentage);
                    g.FillRectangle(Brushes.Gray, hb.X, hb.Y, hb.Width, hb.Height);
                    g.FillRectangle(Brushes.Red, hb.X + (hb.Width - healthBarWidth), hb.Y, healthBarWidth, hb.Height);
                    g.DrawRectangle(Pens.Black, hb.X, hb.Y, hb.Width, hb.Height);
                }
            }
        }
        void drawHero1(Graphics g)
        {
            //Hero
            for (int i = 0; i < heroList.Count; i++)
            {
                //MessageBox.Show("Hero Show Flag=" + heroList[0].show);
                if (heroList[i].show)
                {
                    int nm = 0;
                    if (backGroundIndex == 4) { nm = 3; }
                    if (backGroundIndex == 3) { nm = 2; }

                    //Move Left
                    if (flagheroleft == 1 && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                        g.DrawImage(heroList[i].ImgsL[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsL[heroList[i].iframe].Width * nm, heroList[i].ImgsL[heroList[i].iframe].Height * nm);


                    //Move Right 
                    else if (flagheroleft == 0 && heroList[0].flagjump == 0 && heroList[0].flagkill == 0)
                        g.DrawImage(heroList[i].ImgsR[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsR[heroList[i].iframe].Width * nm, heroList[i].ImgsR[heroList[i].iframe].Height * nm);


                    //jump
                    else if (heroList[0].flagjump == 1 && heroList[0].flagkill == 0)
                        g.DrawImage(heroList[i].ImgsJ[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsJ[heroList[i].iframe].Width * nm * 2, heroList[i].ImgsJ[heroList[i].iframe].Height * nm * 2);


                    //Kill
                    else if (heroList[0].flagkill == 1 && heroList[0].flagjump == 0)
                        g.DrawImage(heroList[i].ImgsK1[heroList[i].iframe], heroList[i].X, heroList[i].Y, heroList[i].ImgsK1[heroList[i].iframe].Width * nm * 2, heroList[i].ImgsK1[heroList[i].iframe].Height * nm * 2);


                    //Hero Health bar
                    g.FillRectangle(Brushes.Gray, heroList[0].HealthBarr.X, heroList[0].HealthBarr.Y, heroList[0].HealthBarr.Width, heroList[0].HealthBarr.Height);
                    int currentHealthWidth = (int)((heroList[0].HealthBarr.CurrentHealth / (float)heroList[0].HealthBarr.MaxHealth) * heroList[0].HealthBarr.Width);
                    g.FillRectangle(Brushes.Red, heroList[0].HealthBarr.X, heroList[0].HealthBarr.Y, currentHealthWidth, heroList[0].HealthBarr.Height);

                }
            }
        }
        void drawSingleImage(HeroActor image, Graphics g)
        {
            int nm = 0;
            if (backGroundIndex == 4) { nm = 1; }
            if (backGroundIndex == 3) { nm = 1; }
            if (image != null)
                g.DrawImage(image.image, image.x, image.y);
        }
        void drawHealth(List<HeroActor> Players, Graphics g)
        {
            Color backColorBar = Color.Orange, active = Color.Blue;
            for (int i = 0; i < Players.Count; i++)
            {
                HeroActor c = Players[i];

                int height = 10, startX = c.x + 10, startY = c.y - 5;

                // draw Back Bar
                if (c.health > 0)
                {
                    g.FillRectangle(new SolidBrush(backColorBar), startX, startY, c.healthBar, height);
                    g.DrawRectangle(new Pen(Color.Black, 3), startX, startY, c.healthBar, height);

                    // draw Health Bar
                    g.FillRectangle(new SolidBrush(active), startX, startY, c.health, height);
                    g.DrawRectangle(new Pen(Color.Black, 3), startX, startY, c.health, height);

                }
            }

        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            drawBackGround(g);
            drawHero1(g);
            drawEnemy(g);
            if (backGroundIndex == zombieBackground && leadder != null && Elevator != null)
            {
                drawSingleImage(leadder, g);
                drawSingleImage(Elevator, g);
            }
            drawSingleImage(Hero, g);
            DrawListImages(Bullets, g);
            DrawListImages(Zombies, g);

            if (heroIndex == 1)
            {
                // bullets
                g.DrawString("" + bulletsLimit, new Font("Arial", 16, FontStyle.Bold), Brushes.White, 160, 80);
                g.DrawImage(new Bitmap(new Bitmap("Man/bullet3.png"), 30, 15), 210, 85);


                g.DrawString("" + zombieDead, new Font("Arial", 16, FontStyle.Bold), Brushes.White, 250, 80);
                g.DrawImage(new Bitmap(new Bitmap("Zombies/Zombie1/zombiewalk (1).png"), 50, 50), 270, 80);
            }
            drawHealth(Zombies, g);


            if (laser != null)
            {
                g.FillRectangle(new SolidBrush(laser.cl), laser.x, laser.y, laser.Widht, laser.Heigth);
                g.DrawRectangle(new Pen(Color.Black, 3), laser.x, laser.y, laser.Widht, laser.Heigth);
            }
            g.FillRectangle(new SolidBrush(Color.Red), 120, 30, heroHealth, 20);
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