namespace slashGame
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox frame;
        private System.Windows.Forms.Timer renderTimer, inputTimer;
        private System.Drawing.Point defaultWinSize;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.defaultWinSize = new System.Drawing.Point(1400, 740);
            this.components = new System.ComponentModel.Container();
            this.frame = new System.Windows.Forms.PictureBox();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.inputTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.frame)).BeginInit();
            this.SuspendLayout();

            this.frame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frame.Location = new System.Drawing.Point(0, 0);
            this.frame.Name = "Draw Frame";
            this.frame.Size = new System.Drawing.Size(this.defaultWinSize);
            this.frame.TabIndex = 0;
            this.frame.TabStop = false;

            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            this.inputTimer.Tick += new System.EventHandler(this.inputTimer_Tick);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(this.defaultWinSize);
            this.Controls.Add(this.frame);
            this.Name = "slashGame";
            this.Text = "slashGame";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.frame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}

