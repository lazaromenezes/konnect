using Konnect.Main.Components.Dot;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Konnect.Main
{
    public class Konnect : Game
    {
        const int DOT_COUNT = 11;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _backgroundTile;
        private Texture2D _barrelTile;

        const int Y_OFFSET = 40;
        const int X_OFFSET = 40;

        private readonly List<List<Dot>> dots = new(DOT_COUNT * DOT_COUNT);
        
        public Konnect()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            for (var i = 0; i < DOT_COUNT; i++)
            {
                dots.Add([]);
                for (var j = 0; j < DOT_COUNT; j++)
                {
                    dots[i].Add(new Dot(i * DOT_COUNT + j));
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTile = Content.Load<Texture2D>("tile");
            _barrelTile = Content.Load<Texture2D>("barrel");
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsExitInput())
                Exit();

            HandleDotClick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    var position = new Vector2(X_OFFSET + i * _backgroundTile.Width, Y_OFFSET + j * _backgroundTile.Height);
                    _spriteBatch.Draw(_backgroundTile, position, null, Color.White);
                }
            }

            foreach (var dot in dots.SelectMany(d => d))
            {
                var x = X_OFFSET + (dot.Index / DOT_COUNT) * Dot.Size.X - Dot.Size.X / 2;
                var y = Y_OFFSET + (dot.Index % DOT_COUNT) * Dot.Size.Y - Dot.Size.Y / 2;

                dot.Position = new Point(x, y);

                _spriteBatch.Draw(_barrelTile, dot.Rectangle, dot.Color);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private static bool IsExitInput()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        private void HandleDotClick()
        {
            var clickedDot = dots.SelectMany(d => d).SingleOrDefault(d => d.Rectangle.Contains(Mouse.GetState().Position));

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && clickedDot != null)
            {
                clickedDot.Color = Color.PowderBlue;
            }
        }
    }
}
