using Microsoft.Xna.Framework.Input;

namespace MiniRogue
{
    public static class InputHandler
    {
        private static KeyboardState keyboardState;
        private static KeyboardState old_keyboardState;
        
      

        public static void Update()
        {
            old_keyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return old_keyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
        }

        public static  bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
