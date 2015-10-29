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

        public WinScreen(string winnerName)
        {
            InitializeComponent();
            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "titelbackground.bmp"));
            winner.Text = winnerName;

        }
    }
}
