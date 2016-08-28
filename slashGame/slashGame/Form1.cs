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

        public Form1() {
            InitializeComponent();
            this.time1.Interval = 50;
            this.time1.Start();
            this.KeyDown += keyDownListener;
            this.KeyUp += keyUpListener;
            this.ResizeEnd += notifyEnts;
        }

        private void notifyEnts(object sender, EventArgs e) {
            p1.parentSize.X = this.Width; p1.parentSize.Y = this.Height;
            p2.parentSize.X = this.Width; p2.parentSize.Y = this.Height;
        }

        private void keyUpListener(object sender, KeyEventArgs e) {
            
        }

        private void keyDownListener(object sender, KeyEventArgs e) {
            switch((int) e.KeyCode) {
                case 87://w
                    p1.move(0, -10);
                    break;
                case 83://s
                    p1.move(0, 10);
                    break;
                case 32://space
                    if(p2.Attack(p1.getAttack() * getDamageDelt())) { Console.WriteLine("Player 1 Won!!"); Application.Exit(); }
                    break;
                case 38://up
                    p2.move(0, -10);
                    break;
                case 40://down
                    p2.move(0, 10);
                    break;
                case 13://enter
                    if(p1.Attack(p2.getAttack() * getDamageDelt())) { Console.WriteLine("Player 2 Won!!"); Application.Exit(); }
                    break;
            }
            
        }

        private void time1_Tick(object sender, EventArgs e) {
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
