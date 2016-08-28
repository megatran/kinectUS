namespace slashGame {
    partial class EndgameDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.frame = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();

            this.frame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frame.Location = new System.Drawing.Point(0, 24);
            this.frame.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(1050, 577);
            this.frame.TabIndex = 0;
            this.frame.TabStop = false;

            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.frame);
            this.ClientSize = new System.Drawing.Size(379, 299);
            this.Name = "endgame";
            this.Text = "Game Over!";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox frame;
    }
}