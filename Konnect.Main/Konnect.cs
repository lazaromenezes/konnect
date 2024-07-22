using Konnect.Framework;
using Konnect.Main.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Konnect.Main
{
    public class Konnect : Game
    {
        SpriteBatch _batch;

        public Konnect()
        {
            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            RegisterServices();
            RegisterComponents();

            base.Initialize();
        }

        private void RegisterServices()
        {
            _batch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _batch);
        }

        private void RegisterComponents()
        {
            Components.Add(new Board(this));
            Components.Add(new MouseEvents(this));
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsExitInput())
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _batch.Begin();
            base.Draw(gameTime);
            _batch.End();
        }

        private static bool IsExitInput()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        }
    }
}
