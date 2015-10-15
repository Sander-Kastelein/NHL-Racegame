using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Player p = new Player();
            drawList.Add(p);

            Player p2 = new Player();
            

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
