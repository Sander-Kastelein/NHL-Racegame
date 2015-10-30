namespace NHLRacegame
{
    partial class Game
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
            this.nyan = new System.Windows.Forms.PictureBox();
            this.startLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nyan)).BeginInit();
            this.SuspendLayout();
            // 
            // nyan
            // 
            this.nyan.Location = new System.Drawing.Point(-50, 0);
            this.nyan.Name = "nyan";
            this.nyan.Size = new System.Drawing.Size(50, 50);
            this.nyan.TabIndex = 0;
            this.nyan.TabStop = false;
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.BackColor = System.Drawing.Color.Transparent;
            this.startLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.startLabel.Location = new System.Drawing.Point(443, 323);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(111, 118);
            this.startLabel.TabIndex = 1;
            this.startLabel.Text = "3";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.startLabel);
            this.Controls.Add(this.nyan);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game";
            ((System.ComponentModel.ISupportInitialize)(this.nyan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox nyan;
        private System.Windows.Forms.Label startLabel;
    }
}