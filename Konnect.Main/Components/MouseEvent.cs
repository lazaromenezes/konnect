using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Konnect.Main.Components
{
    internal static class MouseEvent
    {
        public delegate void OnJustPressed(Point position);

        public static event OnJustPressed JustPressed;

        static ButtonState _previousState = ButtonState.Released;

        static ButtonState LeftButtonState => Mouse.GetState().LeftButton;

        public static void Update(GameTime gameTime) {
            if (LeftButtonState == ButtonState.Pressed && _previousState == ButtonState.Released)
            {
                JustPressed.Invoke(Mouse.GetState().Position);
                Console.WriteLine("JUST PRESSED");
            }

            _previousState = LeftButtonState;
        }
    }
}
