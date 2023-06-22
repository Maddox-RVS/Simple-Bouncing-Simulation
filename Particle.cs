using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Object_Simulation_Playground
{
    internal class Particle
    {
        private Hitbox hitbox;
        private Rectangle screenBounds;
        private Texture2D texture;
        private Color color;
        private Vector2 velocity;

        public Particle(Rectangle bounds, Color color, Texture2D texture, Rectangle screenBounds)
        {
            this.screenBounds = screenBounds;
            this.color = color;
            this.texture = texture;

            hitbox = new Hitbox(
                new Vector2(bounds.X, bounds.Y), 
                new Vector2(bounds.Width, bounds.Height), 
                GlobalVariables.boundingBox, 
                false, false);
            hitbox.Origin("corner");
            GlobalVariables.hitboxes.Add(hitbox);
            //hitbox.Visible(true);
        }

        public void Update()
        {
            hitbox.Update();
            checkReachBounds();
            checkVelocity();
            applyGravity();
            applyFriction(0.1f, 0.1f, 0.3f);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                addVelocity(0, -3f);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                addVelocity(0, 3f);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                addVelocity(-1f, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                addVelocity(1f, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                velocity = Vector2.Zero;
        }

        public void applyGravity()
        { 
            if (!isAtBounds(true))
                velocity.Y += 1f;
        }

        public void checkVelocity()
        {
            hitbox.Position(new Vector2(hitbox.Position().X + velocity.X, hitbox.Position().Y + velocity.Y));
        }

        public void applyFriction(float horizontalFrictionStrength, float verticalFrictionStrength, float stopThreshold)
        {
            if (velocity.Y > -stopThreshold && velocity.Y < stopThreshold)
                velocity.Y = 0.0f;
            else if (velocity.Y < 0)
                velocity.Y += verticalFrictionStrength;
            else if (velocity.Y > 0)
                velocity.Y -= verticalFrictionStrength;
            if (velocity.X > -stopThreshold && velocity.X < stopThreshold)
                velocity.X = 0.0f;
            else if (velocity.X < 0)
                velocity.X += horizontalFrictionStrength;
            else if (velocity.X > 0)
                velocity.X -= horizontalFrictionStrength;
        }

        public void addVelocity(float x, float y)
        {
            velocity.X += x;
            velocity.Y += y;
        }

        public void checkReachBounds()
        {
            if (hitbox.Position().X < 0 || hitbox.Position().X > screenBounds.Width - hitbox.Bounds().Width)
            {
                if (!isAtBounds(false))
                    velocity.X -= 1f;
                velocity.X *= -1f;
            }
            if (hitbox.Position().Y < 0 || hitbox.Position().Y > screenBounds.Height - hitbox.Bounds().Height)
            {
                if (!isAtBounds(true))
                    velocity.Y -= 1f;
                velocity.Y *= -1f;
            }
        }

        public bool isAtBounds(bool justFloor)
        {
            if (!justFloor && (hitbox.Position().X < 0 || hitbox.Position().X > screenBounds.Width - hitbox.Bounds().Width))
                return true;
            if (!justFloor && (hitbox.Position().Y < 0 || hitbox.Position().Y > screenBounds.Height - hitbox.Bounds().Height))
                return true;
            if (justFloor && hitbox.Position().Y > screenBounds.Height - hitbox.Bounds().Height)
                return true;
            return false;
        }

        public void setPosition(float x, float y)
        {
            hitbox.Position(new Vector2(x, y));
        }

        public Vector2 getPosition() { return hitbox.Position(); }

        public Rectangle getBounds()
        {
            return hitbox.Bounds();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox.Bounds(), color);
            hitbox.Draw(spriteBatch, Color.White);
        }
    }
}
