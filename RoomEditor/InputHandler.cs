using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RoomEditor
{
    public static class InputHandler
    {
        private static KeyboardState keyboardState;
        private static KeyboardState old_keyboardState;
        private static MouseState mouseState;
        private static MouseState old_mouseState;

        public static Point MousePosition { get { return mouseState.Position; } }

        public static void Update()
        {
            old_keyboardState = keyboardState;
            old_mouseState = mouseState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return old_keyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
        }

        public static  bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        internal static bool WasRMousePressed()
        {
            return (old_mouseState.RightButton == ButtonState.Pressed &&
                    mouseState.RightButton == ButtonState.Released);
        }
        internal static bool IsRMousePressed()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }
    }
}
