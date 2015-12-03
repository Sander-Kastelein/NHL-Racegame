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

namespace NHLRacegame
{
    public partial class CarSelect : Form
    {

        Game game;

        public CarSelect()
        {
            InitializeComponent();
            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "selectscreen.png"));
            game = new Game();

            this.Size = new Size(1024, 768);
            this.Width = 1024;
            this.Height = 768;
            this.Left = 0;
            this.Top = 0;


            this.KeyDown += OnKeyDown;

        }

        public void OnKeyDown(Object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Space)
            {
                game.Show();
                game.startGame();
                this.Hide();
            }
        }
    }
}
