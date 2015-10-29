using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;


namespace NHLRacegame
{
    public partial class Game : Form
    {
        List<ISprite> drawList = new List<ISprite>();
        public Bitmap roadBitmap;
        public bool isPaused = true;
        public DateTime start;
        public bool isBegin = true;
        public bool isEnd = false;
        public DateTime disableNyanAt = DateTime.Now;
        public bool isNyanPlaying = false;

        public System.Media.SoundPlayer player;
        public System.Media.SoundPlayer nyanPlayer;

        public List<PictureBox> nyans = new List<PictureBox>();

        public Game()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint,
                true
            );

            Image roadImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Racemap.bmp"));
            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Racemapv3.bmp"));
            roadBitmap = new Bitmap(roadImage);
            //BackgroundImage = roadImage; //DEBUG

            player = new System.Media.SoundPlayer();
            player.SoundLocation = Path.Combine(Environment.CurrentDirectory, "music.wav");
            player.Play();


            nyanPlayer = new System.Media.SoundPlayer();
            nyanPlayer.SoundLocation = Path.Combine(Environment.CurrentDirectory, "nyan.wav");

     
            Paint += new PaintEventHandler(PaintHandler);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(Tick);
            GameTimer.Start();

            int fps = 25; // Sanders arme laptop trekt geen 60FPS :( verander dit gerust naar 40-60 als je een i5 of i7 hebt

            Timer FrameTimer = new Timer();
            FrameTimer.Interval = 1000 / fps;
            FrameTimer.Tick += new EventHandler(FrameTick);
            FrameTimer.Start();


            Init();
        }

        public void goNyan()
        {
            if (DateTime.Now.Subtract(disableNyanAt).Seconds >= 0 && isNyanPlaying)
            {
                nyanPlayer.Stop();
                isNyanPlaying = false;
                player.Play();

            }

            foreach(PictureBox nyan in nyans)
            {
                nyan.Left += 2;
            }
        }

        public void addNyan()
        {
            if (isNyanPlaying) return;
            Image nyanImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "nyan.gif"));

            player.Stop();
            nyanPlayer.Play();
            isNyanPlaying = true;

            Random rnd = new Random();
 
                PictureBox nyan = new PictureBox();
                nyan.Image = nyanImage;
                nyan.Height = 28;
                nyan.Width = 72;
                nyan.BackColor = Color.FromArgb(230,226,253);
                nyan.Left = -nyan.Width - (rnd.Next(0, 200)) ;
                nyan.Top = (rnd.Next(0, 740));
                nyans.Add(nyan);
            
            foreach(PictureBox nyana in nyans)
            {
                this.Controls.Add(nyana);
            }

            disableNyanAt = DateTime.Now.AddSeconds(16);

        }

        public bool isPositionOnRoad(double x, double y)
        {
            try
            {
                int ix = (int)Math.Round(x);
                int iy = (int)Math.Round(y);
                if (iy < 0 || ix < 0)
                {
                    return false;
                }
                if (iy >= roadBitmap.Height) return false;
                if (ix >= roadBitmap.Width) return false;

                Color pixel = roadBitmap.GetPixel((int)Math.Round(x), (int)Math.Round(y));
                return pixel.R > 0 || pixel.B > 0 || pixel.G > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public void FrameTick(Object sender, EventArgs e)
        {
            if (isEnd) return;
            Invalidate();
        }

        public void Tick(Object sender, EventArgs e)
        {
            if (isEnd) return;
            if (isBegin)
            {
                isBegin = false;
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = Path.Combine(Environment.CurrentDirectory, "start.wav");
                player.Play();
                start = DateTime.Now;
            }

            if ((DateTime.Now).Subtract(start).Seconds == 1)
            {
                startLabel.Text = "2";
            }
            if ((DateTime.Now).Subtract(start).Seconds == 2)
            {
                startLabel.Text = "1";
            }
            if ((DateTime.Now).Subtract(start).Seconds == 3)
            {
                startLabel.Text = "0";
                isPaused = false;
            }
            if ((DateTime.Now).Subtract(start).Seconds == 4)
            {
                startLabel.Hide();
                player.PlayLooping();
            }
            if (!isPaused) Loop();
        }

        public void PaintHandler(Object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        public void Draw(Graphics g)
        {
            foreach(ISprite drawItem in drawList)
            {
                drawItem.Draw(g);
            }
        }

        public void stopGame()
        {
            isEnd = true;
            player.Stop();
        }

        public void Init()
        {
            Player p = new Player(this);
            p.name = "Player 1";
            p.rotation = -90;
            p.posX = 970;
            p.posY = 680;
            drawList.Add(p);
            
            Player p2 = new Player(this);
            p2.name = "Player 2";
            p2.interfaceOffset = 270;
            p2.rotation = -90;
            p2.posX = 990;
            p2.posY = 680;
            p2.upKey = Key.W;
            p2.downKey = Key.S;
            p2.leftKey = Key.A;
            p2.rightKey = Key.D;
            drawList.Add(p2);
            

        }


        public void Loop()
        {
            foreach (ISprite drawItem in drawList)
            {
                drawItem.Loop();
            }
            goNyan();
            powerUp();
        }

        public void powerUp()
        {
            Random rand = new Random();
            if (rand.Next(0, 100 * 60) == 0)
            {
                addNyan(); // Niet echt een power-up maar wel leuk :)
            }
        }


    }
}
