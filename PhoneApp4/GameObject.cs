using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
//using Microsoft.Xna.Framework;
namespace PhoneApp4
{
    [Serializable()]
    public class GameObject
    {
        public PointCollection extremePoints { get; set; }
        public PointCollection edges { get; set; }
        public Rectangle hitbox { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Trigger { get; set; } //0 = no trigger, 1 = Goal

        public GameObject()
        {

        }
        // extremePoints = x1y1, x2y1, x1y2, x2y2
        //              leftup rightup leftdown rightdown
        public GameObject(PointCollection extremePoints, PointCollection edges)
        {
            this.extremePoints = extremePoints;
            this.edges = edges;
        }

        public GameObject(int top, int left, int width, int height, PointCollection edges)
        {
            this.edges = edges;
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;
        }

        public GameObject(int top, int left, int width, int height)
        {
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;

            PointCollection pc = new PointCollection();
            Point p;
            int i;
            for (i=top; i < top + height; i++)
            {
                p = new Point(i, left);
                pc.Add(p);
                p = new Point(i, left + width);
                pc.Add(p);
            }
            for (i = left; i < left + width; i++)
            {
                p = new Point(top, i);
                pc.Add(p);
                p = new Point(top + height, i);
                pc.Add(p);
            }
            this.edges = pc;
        }

        public bool Collision(GameObject go)
        {
            bool horizontalCollision = false;
            bool verticalCollision = false;
            if (go.Left < this.Left && this.Left < go.Left + go.Width)
            {
                horizontalCollision = true;
            }

            if (go.Left < this.Left + this.Width && this.Left + this.Width < go.Left + go.Width)
            {
                horizontalCollision = true;
            }

            if (go.Top < this.Top && this.Top < go.Top + go.Height)
            {
                verticalCollision = true;
            }
            if (go.Top < this.Top + this.Height && this.Top + this.Height < go.Top + go.Height)
            {
                verticalCollision = true;
            }

            if (horizontalCollision && verticalCollision)
            {
                return CollisionPixel(go);
            }

            if (!horizontalCollision)
            {
                if (this.Left < go.Left && go.Left < this.Left + this.Width) horizontalCollision = true;
            }
            if (!verticalCollision)
            {
                if (this.Top < go.Top && go.Top < this.Top + this.Height) verticalCollision = true;
            }
            if (horizontalCollision && verticalCollision)
            {
                return CollisionPixel(go);
            }
            if (!horizontalCollision && !verticalCollision)
            {
                if ((this.Left < go.Left && go.Left < this.Left + this.Width) && (this.Top < go.Top && go.Top < this.Top + this.Height)) // wholly inside
                {
                    return true;
                } 
            }
            return false;
        }

        // UNOPTIMIZED!
        public bool CollisionPixel(GameObject go)
        {
            return true;
            foreach (Point p in this.edges)
            {
                
                foreach (Point po in go.edges)
                {
                    p.Equals(po);
                }
                //if (go.edges.Contains(p)) return true;
            }
            return false;
        }

    }

class Actor : GameObject
{
    public int HP;
    public int Damage;
    public int ReloadTime;
    //public Ammo ammo

    public Actor() : base ()
    {
        this.HP = 10;
        this.Damage = 1;
        this.ReloadTime = 1000;
    }

    public Actor(int top, int left, int width, int height, int health, int damage, int reloadtime) : base (top,left,width,height)
    {
        this.HP = health;
        this.Damage = damage;
        this.ReloadTime = reloadtime;

        hitbox = new Rectangle();
        hitbox.Width = width;
        hitbox.Height = height;
        hitbox.Fill = new SolidColorBrush(Colors.Red);
    }
}

    //if (Rectangle.Intersect(this.hitbox,go.hitbox))
    //if (this.top < go.this.top + this.hitbox.Width)

    /*
    class GameObject
    {
        public int top;
        public int left;
        public bool isTrigger;
        // Hitbox
        public Rectangle hitbox;
        // Sprite
        private SolidColorBrush color;

        public GameObject()
        {
            this.top = 0;
            this.left = 0;
            this.hitbox = new Rectangle();
            hitbox.Height = 10;
            hitbox.Width = 10;
            hitbox.Fill = new SolidColorBrush(Colors.White);

            this.IsTrigger = false;
            color = new SolidColorBrush(Colors.White);
        }

        public GameObject(int top, int left, int width, int height)
        {
            this.top = top;
            this.left = left;
            hitbox = new Rectangle();
            hitbox.Width = width;
            hitbox.Height = height;
            hitbox.Fill = new SolidColorBrush(Colors.White);
            this.isTrigger = false;
            this.color = new SolidColorBrush(Colors.White);
        }

        public SolidColorBrush COLOR
        {
            get { return color; }
            set { color = value; }
        }

        public bool IsTrigger
        {
            get { return isTrigger; }
            set { isTrigger = value; }
        }


        public void SetLocation(int top, int left)
        {
            this.top = top;
            this.left = left;
        }

        public void Move(int dTop, int dLeft)
        {
            this.top += dTop;
            this.left += dLeft;
        }

        // TARKISTAAAAAAAAAAAAAA!!!!!!!!!!!!!
        public bool Collision(GameObject go)
        {
            bool horizontalCollision = false;
            bool verticalCollision = false;
            Rectangle r1 = go.hitbox;
            Rectangle r2 = this.hitbox;
            //  r1.left <   r2.left  &&   r2.left     <     r1.right            ||    r1.left      <     r2.right           &&         r2.right            <      r1.right
            if (go.left < this.left && this.left < go.left + go.hitbox.Width)
            {
                horizontalCollision = true;
            }
            if (go.left < this.left + this.hitbox.Width && this.left + this.hitbox.Width < go.left + go.hitbox.Width)
            {
                horizontalCollision = true;
            }

            if (go.top < this.top && this.top < go.top + go.hitbox.Height)
            {
                verticalCollision = true;
            }
            if (go.top < this.top + this.hitbox.Height && this.top + this.hitbox.Height < go.top + go.hitbox.Height)
            {
                verticalCollision = true;
            }

            if (horizontalCollision && verticalCollision)
            {
                return true;
            }


            if (!horizontalCollision)
            {
                if (this.left < go.left && go.left < this.left + this.hitbox.Width) horizontalCollision = true;
            }
            if (!verticalCollision)
            {
                if (this.top < go.top && go.top < this.top + this.hitbox.Height) verticalCollision = true;
            }
            if (horizontalCollision && verticalCollision)
            {
                return true;
            }
            if (!horizontalCollision && !verticalCollision)
            {
                if ( (this.left < go.left && go.left < this.left+this.hitbox.Width) && ( this.top < go.top && go.top < this.top+this.hitbox.Height ) ) return true;
            }
            return false;
            //if (Rectangle.Intersect(this.hitbox,go.hitbox))
            //if (this.top < go.this.top + this.hitbox.Width)
        }
    }*/
    /*
    class Actor : GameObject
    {
        public int HP;
        public int Damage;
        public int ReloadTime;
        //public Ammo ammo

        public Actor() : base ()
        {
            this.HP = 10;
            this.Damage = 1;
            this.ReloadTime = 1000;
        }

        public Actor(int top, int left, int width, int height, int health, int damage, int reloadtime) : base (top,left,width,height)
        {
            this.HP = health;
            this.Damage = damage;
            this.ReloadTime = reloadtime;
        }
    }
    */
    /*
  class Enemy : Actor
  {
      public bool Stationary;
      public bool Gravity;
      public bool HorizontalShootingOnly;
      public int MovementSpeed;
      public bool StayOnPlatform;
      public bool OnPlatform;

      public Enemy()
          : base()
      {
          this.Stationary = false;
          this.Gravity = true;
          this.HorizontalShootingOnly = true;
          this.MovementSpeed = 2;
          this.StayOnPlatform = true;
          this.OnPlatform = false;
      }

      public Enemy(int top, int left, int width, int height, int health, int damage, int reloadtime, bool stationary, bool gravity)
          : base(top, left,width,height, health, damage, reloadtime)
      {
          this.Stationary = stationary;
          this.Gravity = gravity;
          this.MovementSpeed = 2;
          this.StayOnPlatform = true;
          this.OnPlatform = false;
      }
  }*/
    /*
    class Platform : GameObject
    {
        public bool JumpThrough;
        public bool Friction;
        public bool Knockback;
        public int Damage;

        public Platform()
            : base()
        {
            this.JumpThrough = false;
            this.Friction = true;
            this.Knockback = false;
            this.Damage = 0;
        }

        public Platform(int top, int left, int width, int height, bool jump, bool friction)
            : base(top, left,width,height)
        {
            this.JumpThrough = jump;
            this.Friction = friction;
            this.Knockback = false;
            this.Damage = 0;
        }
    }*/
}
