using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PlatformerGame
{
    internal class Hitbox
    {
        Vector2 position;
        Vector2 size;
        Rectangle boundsRect;
        Texture2D boundingBox;
        bool boundingBox_Visible;
        bool collisions;
        bool colliding;
        bool left;
        bool right;
        bool top;
        bool bottom;
        String origin = "corner";

        public Hitbox(Vector2 position, Vector2 size, Texture2D boundingBox, bool boundingBox_Visible, bool collisions)
        {
            this.position = position;
            this.size = size;
            this.boundingBox_Visible = boundingBox_Visible;
            this.boundingBox = boundingBox;

            //WARNING - Doesn't work well between two moving hitboxes if both hitboxes have collision enabled
            this.collisions = collisions;
        }

        public void Update()
        {
            colliding = Check_Colliding();

            if (collisions)
            {
                foreach (Hitbox hitbox in GlobalVariables.hitboxes)
                {
                    if (hitbox != this)
                    {
                        float left_difference = Math.Abs(hitbox.boundsRect.Right - boundsRect.Left);
                        float right_difference = Math.Abs(boundsRect.Right - hitbox.boundsRect.Left);
                        float top_difference = Math.Abs(hitbox.boundsRect.Bottom - boundsRect.Top);
                        float bottom_difference = Math.Abs(boundsRect.Bottom - hitbox.boundsRect.Top);

                        if (boundsRect.Intersects(hitbox.boundsRect))
                        {
                            if (left_difference < right_difference && left_difference < top_difference && left_difference < bottom_difference)
                            {
                                position.X += left_difference - 1;

                                left = true;
                            }
                            else if (right_difference < left_difference && right_difference < top_difference && right_difference < bottom_difference)
                            {
                                position.X -= right_difference - 1;

                                right = true;
                            }
                            else if (top_difference < bottom_difference && top_difference < right_difference && top_difference < left_difference)
                            {
                                position.Y += top_difference - 1;

                                top = true;
                            }
                            else if (bottom_difference < top_difference && bottom_difference < right_difference && bottom_difference < left_difference)
                            {
                                position.Y -= bottom_difference - 1;

                                bottom = true;
                            }
                        }
                    }
                }

                if (!colliding)
                {
                    left = false;
                    right = false;
                    top = false; 
                    bottom = false;
                }
            }
        }

        public bool Left()
        {
            return left;
        }

        public bool Right()
        {
            return right;
        }

        public bool Top()
        {
            return top;
        }

        public bool Bottom()
        {
            return bottom;
        }

        public Rectangle Bounds()
        {
            return boundsRect;
        }

        public void Origin(String origin)
        {
            this.origin = origin;
        }

        public String Origin()
        {
            return origin;
        }

        public bool Check_Colliding()
        {
            float objects_collided_with = 0;

            foreach (Hitbox obj in GlobalVariables.hitboxes)
            {

                if (!boundsRect.Intersects(obj.boundsRect))
                {
                    objects_collided_with++;
                }

            }

            if (objects_collided_with == GlobalVariables.hitboxes.Count - 1)
                return false;
            return true;
        }

        public bool Colliding()
        {
            return colliding;
        }

        public void Collisions(bool status)
        {
            collisions = status;
        }

        public bool Collisions()
        {
            return collisions;
        }

        public void Size(Vector2 size)
        {
            this.size = size;
        }

        public Vector2 Size()
        {
            return size;
        }

        public void Position(Vector2 position)
        {
            float previous_X = this.position.X;
            float previous_Y = this.position.Y;

            bool moving_left = false;
            bool moving_right = false;
            bool moving_up = false;
            bool moving_down = false;

            if (position.X < previous_X)
                moving_left = true;
            else if (position.X > previous_X)
                moving_right = true;

            if (position.Y < previous_Y)
                moving_up = true;
            else if (position.Y > previous_Y)
                moving_down = true;

            if (!left && moving_left)
                this.position.X = position.X;
            else if (!right && moving_right)
                this.position.X = position.X;

            if (!top && moving_up)
                this.position.Y = position.Y;
            else if (!bottom && moving_down)
                this.position.Y = position.Y;
        }

        public Vector2 Position()
        {
            return position;
        }

        public void Visible(bool status)
        {
            boundingBox_Visible = status;
        }

        public bool Visible()
        {
            return boundingBox_Visible;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (origin == "center")
                boundsRect = new Rectangle((int) position.X - ((int) size.X / 2), (int) position.Y - ((int)size.Y / 2), (int) size.X, (int) size.Y);
            else
                boundsRect = new Rectangle((int) position.X, (int) position.Y, (int)size.X, (int)size.Y);

            if (boundingBox_Visible)
                spriteBatch.Draw(boundingBox, boundsRect, color);
        }
    }
}
