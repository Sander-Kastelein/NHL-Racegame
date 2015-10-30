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
