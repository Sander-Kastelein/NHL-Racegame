namespace NHLRacegame
{
    partial class StartScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_start = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.AutoSize = true;
            this.btn_start.BackColor = System.Drawing.Color.Transparent;
            this.btn_start.Font = new System.Drawing.Font("Lucida Sans", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_start.ForeColor = System.Drawing.Color.Gold;
            this.btn_start.Location = new System.Drawing.Point(0, 200);
            this.btn_start.MinimumSize = new System.Drawing.Size(1024, 0);
            this.btn_start.Name = "btn_start";
            this.btn_start.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_start.Size = new System.Drawing.Size(1024, 42);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "START";
            this.btn_start.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_exit
            // 
            this.btn_exit.AutoSize = true;
            this.btn_exit.BackColor = System.Drawing.Color.Transparent;
            this.btn_exit.Font = new System.Drawing.Font("Lucida Sans", 27.75F, System.Drawing.FontStyle.Bold);
            this.btn_exit.ForeColor = System.Drawing.Color.Gold;
            this.btn_exit.Location = new System.Drawing.Point(0, 300);
            this.btn_exit.MinimumSize = new System.Drawing.Size(1024, 0);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(1024, 42);
            this.btn_exit.TabIndex = 1;
            this.btn_exit.Text = "EXIT";
            this.btn_exit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label btn_start;
        private System.Windows.Forms.Label btn_exit;

    }
}

