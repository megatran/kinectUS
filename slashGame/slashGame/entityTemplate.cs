using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace slashGame {

    public class living : entity {
        protected double health, attack;

        public living(int x, int y, int width, int height, string spriteName, double health, double attack) : 
                 base(x, y, width, height, spriteName) {
            this.health = health;
            this.attack = attack;
        }

        public double getAttack() {
            return attack;
        }

        public Boolean Attack(double damage) {//return true if entity is dead
            health -= damage;
            Console.WriteLine("Attempted Attack, Heath at {0}! Damage Take: {1}", health, damage);
            if(health <= 0) return true;
            return false;
        }
    }

    public class entity {
        protected Rectangle dims;
        protected Image sprite;
        protected string spriteName;
        protected Int16 rotTimes;
        protected float rotAngle;
        public Point parentSize;

        public enum direction {
            up, right, down, left
        }

        protected direction dir;

        public entity(int x, int y, int width, int height, string spriteName) {
            this.dims = new Rectangle(x, y, width, height);
            this.spriteName = spriteName;
            this.rotTimes = 0; this.rotAngle = 0; this.dir = direction.up;//set default counter values
            loadImg();
            parentSize = new Point();
        }

        public Point getImgDims() {
            return new Point(sprite.Width, sprite.Height);
        }

        public void refreshFrame() {
            loadImg();
            switch(dir) {
                case direction.up:
                    //do nothing
                    break;
                case direction.right:
                    sprite.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case direction.down:
                    sprite.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case direction.left:
                    sprite.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
        }

        public void setDir(direction d) {
            this.dir = d;
        }

        public void setPos(int x, int y) {
            this.dims.X = x;
            this.dims.Y = y;
        }

        public void setDims(int width, int height) {
            this.dims.Width = width;
            this.dims.Height = height;
        }

        public void move(int xoff, int yoff) {
            if(dims.X + xoff > 10 && dims.X + xoff + dims.Width < parentSize.X) this.dims.X += xoff;
            if(dims.Y + yoff > 10 && dims.Y + yoff + dims.Height < parentSize.Y)this.dims.Y += yoff;
        }

        public void rotateSprite(float angle) {//contains slight blur prevention
            this.rotAngle += angle;
            if(this.rotAngle >= 360) this.rotAngle %= 360;
            if(rotTimes > 2) {
                rotTimes = 0;
                loadImg();
                rotate(rotAngle);
            } else {
                rotate(angle);
                rotTimes++;
            }
            //Console.WriteLine("rotTimes {0}, rotAngle {1}", rotTimes, rotAngle);
        }

        public void rotate(float angle) {//no blur prevention
            int d = sprite.Width > sprite.Height ? sprite.Width : sprite.Height;
            Bitmap b = new Bitmap(d, d);
            using (Graphics g = Graphics.FromImage(b)) {
                g.TranslateTransform(d / 2, d / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-d / 2, -d / 2);
                g.DrawImage(sprite, 0, 0);
            }
            sprite = b;
        }

        public void loadImg() { 
            if (System.IO.File.Exists(spriteName)) sprite = Image.FromFile(spriteName);
            else {
                setDims(100, 50);
                sprite = new Bitmap(this.dims.Width, this.dims.Height);
                Graphics g = Graphics.FromImage(sprite);
                Font f = new Font("Arial", 13);
                SolidBrush b = new SolidBrush(Color.Red);
                PointF p = new PointF(0.0f, 0.0f);
                g.DrawString("Err Loading..\nSprite!", f, b, p);
            }
        }

        public int getY() {
            return dims.Y;
        }

        public int getX() {
            return dims.X;
        }

        public int getWidth() {
            return dims.Width;
        }
        public int getHeight() {
            return dims.Height;
        }

        public void draw(ref Graphics g) {
            g.DrawImage(sprite, dims);
        }
    }

    
}
