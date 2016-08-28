using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slashGame {
    public partial class EndgameDialog : Form {
        public EndgameDialog() {
            InitializeComponent();
            this.FormClosed += EndgameDialog_FormClosed;
        }

        private void EndgameDialog_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }

        public void announce(string who) {
            Bitmap b = new Bitmap(frame.Width, frame.Height);
            Graphics g = Graphics.FromImage(b);
            Font f = new Font("Arial", 13);
            SolidBrush B = new SolidBrush(Color.Black);
            PointF p = new PointF(5.0f, 0.0f);
            g.DrawString("Player " + who + " WON!!!!!", f, B, p);
            this.frame.Image = b;
            this.Invalidate();
        }

    }
}
