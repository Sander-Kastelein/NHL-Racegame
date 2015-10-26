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
        public bool[][] mapBounds;

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

            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Racemap.bmp"));

            ImageConverter converter = new ImageConverter();
            byte[] imageByteArray = (byte[])converter.ConvertTo(BackgroundImage, typeof(byte[]));
            int colCounter = 0;
            int y = 0;

            mapBounds = new bool[BackgroundImage.Height+1][];

            for (int i = 0; i < mapBounds.Length; i++)
            {
                mapBounds[i] = new bool[BackgroundImage.Width];
            }

            Console.WriteLine(imageByteArray.Length.ToString() + " != "  + (3 * BackgroundImage.Height * BackgroundImage.Width).ToString());


            for (int i = 0; i < imageByteArray.Length; i+=3)
            {
                if (BackgroundImage.Width == colCounter)
                {
                    colCounter = 0;
                    y++;
                }

                mapBounds[y][colCounter] = (imageByteArray[i] == 255); // Als red == 255, dan true, anders false
                colCounter++;
            }
            

            Paint += new PaintEventHandler(PaintHandler);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(Tick);
            GameTimer.Start();

            Init();
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
            p.rotation = -90;
            p.posX = 970;
            p.posY = 680;
            drawList.Add(p);

            Player p2 = new Player(this);
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
