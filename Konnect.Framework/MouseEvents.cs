using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Konnect.Framework
{
    public static class MouseEvents
    {
        public delegate void OnJustPressed(Point position);

        public static event OnJustPressed? JustPressed;

        static ButtonState _previousState = ButtonState.Released;

        static ButtonState LeftButtonState => Mouse.GetState().LeftButton;

        public static void Update(GameTime gameTime)
        {
            if (LeftButtonState == ButtonState.Pressed && _previousState == ButtonState.Released)
            {
                JustPressed?.Invoke(Mouse.GetState().Position);
            }

            _previousState = LeftButtonState;
        }
    }
}
