using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerGame;
using System;

namespace Object_Simulation_Playground
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Random rnd = new Random();
        private Rectangle screenBounds;
        private Particle[] particles;
        private Color[] colors;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            screenBounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GlobalVariables.boundingBox = Content.Load<Texture2D>("boundingBox");
            particles = new Particle[3];

            colors = new Color[]
            {
                Color.Yellow,
                Color.Orange,
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Magenta,
                Color.DarkBlue,
                Color.DarkCyan,
                Color.Gold,
                Color.Pink,
                Color.DeepPink,
                Color.SkyBlue,
                Color.DeepSkyBlue,
                Color.LightBlue,
                Color.LightGreen,
                Color.DarkGreen,
                Color.Purple
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(
                    new Rectangle(rnd.Next(0, screenBounds.Width), rnd.Next(0, screenBounds.Height), 30, 30),
                    getRandomColor(),
                    Content.Load<Texture2D>("particle"),
                    screenBounds);
                particles[i].addVelocity(rnd.Next(-50, 50), rnd.Next(-50, 50));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Particle particle in particles)
            {
                particle.Update();
            }

            base.Update(gameTime);
        }

        public Color getRandomColor()
        {
            return colors[rnd.Next(0, colors.Length - 1)];
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            
            foreach (Particle particle in particles)
                particle.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}