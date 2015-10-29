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

        public CarSelect()
        {
            InitializeComponent();
            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "titelbackground.bmp"));

            (new Game()).Show();

        }
    }
}
