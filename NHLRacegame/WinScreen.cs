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
    public partial class WinScreen : Form
    {

        public WinScreen()
        {
            InitializeComponent();

            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = Path.Combine(Environment.CurrentDirectory, "win.wav");
            player.Play();
            
            this.KeyDown += new KeyEventHandler(OnKeyDown);
        }

        public void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Space)
            {
                this.Hide();
            }
        }
    }
}
