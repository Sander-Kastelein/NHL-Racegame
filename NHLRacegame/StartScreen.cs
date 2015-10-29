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
    public partial class StartScreen : Form
    {
        int selectedButton = 0;
        Font defaultFont;
        Label[] buttons = new Label[2];

        public StartScreen()
        {
            InitializeComponent();
            defaultFont = btn_start.Font;
            buttons[0] = btn_start;
            buttons[1] = btn_exit;

            BackgroundImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "titelbackground.bmp"));

            SetSelectedButton(0);

            KeyUp += OnKeyUp;

        }

        public void SetSelectedButton(int i)
        {
            buttons[selectedButton].Font = defaultFont;
            selectedButton = i;
            buttons[selectedButton].Font = new Font(buttons[selectedButton].Font.FontFamily, buttons[selectedButton].Font.Size + 8, buttons[selectedButton].Font.Style);      
        }

        public void OnKeyUp(Object sender, KeyEventArgs e)
        {
            int s = selectedButton;
            if (e.KeyCode.ToString() == "Down")
            {
                s--;
            }
            if (e.KeyCode.ToString() == "Up")
            {
                s++;
            }

            if (s == -1)
            {
                s = buttons.Length - 1;
            }

            if (s == buttons.Length)
            {
                s = 0;
            }
            if (s != selectedButton)
            {
                SetSelectedButton(s);
            }

            if (e.KeyCode.ToString() == "Return" || e.KeyCode.ToString() == "Space")
            {
                if (selectedButton == 0)
                {
                    // Start Game
                    Hide();
                    (new CarSelect()).Show();
                }
                if (selectedButton == 1)
                {
                    Application.Exit();
                    // Exit
                }
            }
        }



       
        
    }
}
