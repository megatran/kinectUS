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
    public partial class Form1 : Form {
        private living p1, p2;
        private Boolean[] keyToggles;
        private int p1Ticker, p2Ticker;

        public Form1() {
            InitializeComponent();
            keyToggles = new Boolean[6];
            for(int i = 0; i < 6; i++) keyToggles[i] = false;
            p1Ticker = 0; p2Ticker = 0;
            this.renderTimer.Interval = 20;
            this.renderTimer.Start();
            this.inputTimer.Interval = 20;
            this.inputTimer.Start();
            this.KeyDown += keyDownListener;
            this.KeyUp += keyUpListener;
            this.ResizeEnd += notifyEnts;
            keyToggles = new Boolean[6];
        }

        private void notifyEnts(object sender, EventArgs e) {
            p1.parentSize.X = this.Width; p1.parentSize.Y = this.Height;
            p2.parentSize.X = this.Width; p2.parentSize.Y = this.Height;
        }

        private void keyUpListener(object sender, KeyEventArgs e) {
            switch((int) e.KeyCode) {
                case 87://w
                    if(keyToggles[0]) keyToggles[0] = false;
                    break;
                case 83://s
                    if(keyToggles[1]) keyToggles[1] = false;
                    break;
                case 32://space
                    if(keyToggles[2]) keyToggles[2] = false;
                    break;
                case 38://up
                    if(keyToggles[3]) keyToggles[3] = false;
                    break;
                case 40://down
                    if(keyToggles[4]) keyToggles[4] = false;
                    break;
                case 13://enter
                    if(keyToggles[5]) keyToggles[5] = false;
                    break;
            }
        }

        private void keyDownListener(object sender, KeyEventArgs e) {
            switch((int) e.KeyCode) {
                case 87://w
                    if(!keyToggles[0]) keyToggles[0] = true;
                    break;
                case 83://s
                    if(!keyToggles[1]) keyToggles[1] = true;
                    break;
                case 32://space
                    if(!keyToggles[2]) keyToggles[2] = true;
                    break;
                case 38://up
                    if(!keyToggles[3]) keyToggles[3] = true;
                    break;
                case 40://down
                    if(!keyToggles[4]) keyToggles[4] = true;
                    break;
                case 13://enter
                    if(!keyToggles[5]) keyToggles[5] = true;
                    break;
            }
            
        }

        private void inputTimer_Tick(object sender, EventArgs e) {
            if(keyToggles[0]) p1.move(0, -10);
            if(keyToggles[1]) p1.move(0, 10);
            if(keyToggles[2]) 
                if(p1Ticker <= 0) {
                    if(p2.Attack(p1.getAttack() * getDamageDelt())) { Console.WriteLine("Player 1 Won!!"); Application.Exit(); }
                    p1Ticker = 5;
                }
            if(keyToggles[3]) p2.move(0, -10);
            if(keyToggles[4]) p2.move(0, 10);
            if(keyToggles[5]) 
                if(p2Ticker <= 0) {
                    if(p1.Attack(p2.getAttack() * getDamageDelt())) { Console.WriteLine("Player 2 Won!!"); Application.Exit(); }
                    p2Ticker = 5;
                }
            if(p1Ticker > 0) p1Ticker--;
            if(p2Ticker > 0) p2Ticker--;
        }

        private void renderTimer_Tick(object sender, EventArgs e) {
            Bitmap b = new Bitmap(frame.Width, frame.Height);
            Graphics g = Graphics.FromImage(b);
            //Call draw method of entities, pass g
            p1.draw(ref g);
            p2.draw(ref g);
            this.frame.Image = b;
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e) {
            p1 = new living(0, 0, 0, 0, "p1.png", 500.0, 50.0);
            p2 = new living(0, 0, 0, 0, "p2.png", 500.0, 50.0);
            Point p1_2 = p1.getImgDims();
            Point p2_2 = p2.getImgDims();
            p1.setDims(p1_2.X / 4, p1_2.Y / 4);
            p2.setDims(p2_2.X / 4, p2_2.Y / 4);
            p1.setPos(50, 50);
            p2.setPos(50+p1_2.X/4, (this.Height - p2_2.Y/4) - 50);
            this.Width = 100 + p1_2.X/4 + p2_2.X/4;
            p1.parentSize.X = this.Width; p1.parentSize.Y = this.Height;
            p2.parentSize.X = this.Width; p2.parentSize.Y = this.Height;
            p1.setDir(entity.direction.right);
            p2.setDir(entity.direction.left);
            p1.refreshFrame();
            p2.refreshFrame();
        }

        private float getDamageDelt() {
            int dist = Math.Abs(p1.getY() - p2.getY());
            float retval;
            if(dist < p1.getHeight()) { retval = ((float) (p1.getHeight() - dist)) / ((float) p1.getHeight()); Console.Write("dist < height! "); }
            else retval =  0.0f;
            Console.WriteLine("Damage Value Determined To Be {0}", retval);
            return retval;
        }
    }
}
