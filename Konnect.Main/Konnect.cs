using Konnect.Framework;
using Konnect.Main.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Konnect.Main
{
    public class Konnect : Game
    {
        const int DOT_COUNT = 11;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Texture2D _backgroundTile;
        Texture2D _barrelTile;
        Texture2D _wallTile;

        const int Y_OFFSET = 40;
        const int X_OFFSET = 40;

        Board _board;
        
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
            _board = new Board(DOT_COUNT);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTile = Content.Load<Texture2D>("tile");
            _barrelTile = Content.Load<Texture2D>("barrel");
            _wallTile = Content.Load<Texture2D>("wall");
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsExitInput())
                Exit();

            MouseEvents.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            DrawBaseBackground();
            DrawBoard();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBoard()
        {
            _board.Draw(_spriteBatch, _barrelTile, _wallTile, new Vector2(X_OFFSET, Y_OFFSET));
        }

        private void DrawBaseBackground()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var position = new Vector2(X_OFFSET + i * _backgroundTile.Width, Y_OFFSET + j * _backgroundTile.Height);
                    _spriteBatch.Draw(_backgroundTile, position, null, Color.White);
                }
            }
        }

        private static bool IsExitInput()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        }
    }
}
