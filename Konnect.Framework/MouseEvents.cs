using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Konnect.Framework
{
    public class MouseEvents(Game game) : GameComponent(game)
    {
        public delegate void OnJustPressed(Point position);

        public static event OnJustPressed? JustPressed;

        static ButtonState _previousState = ButtonState.Released;

        static ButtonState LeftButtonState => Mouse.GetState().LeftButton;

        public override void Update(GameTime gameTime)
        {
            if (LeftButtonState == ButtonState.Pressed && _previousState == ButtonState.Released)
            {
                JustPressed?.Invoke(Mouse.GetState().Position);
            }

            _previousState = LeftButtonState;
        }
    }
}
