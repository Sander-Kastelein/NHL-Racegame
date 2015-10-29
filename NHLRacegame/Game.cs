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
            //BackgroundImage = roadImage;



     
            

            Paint += new PaintEventHandler(PaintHandler);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(Tick);
            GameTimer.Start();

            Init();
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



        public void Tick(Object sender, EventArgs e)
        {
            Loop();
            Invalidate();
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
        }
  

    }
}
