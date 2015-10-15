using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHLRacegame
{
    public partial class StartScreen : Form
    {
        public StartScreen()
        {
            InitializeComponent();
            BackColor = Color.Aqua;

        }

        private void btn_start_game_Click(object sender, EventArgs e)
        {
            (new Game()).ShowDialog();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
