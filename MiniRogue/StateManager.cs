using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MiniRogue
{
    public class State
    {
        internal StateManager Manager;

        public State(StateManager Manager)
        {
            this.Manager = Manager;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
    public class StateManager
    {
        private Stack<State> states;
        internal Game game;

        private bool StackIsEmpty { get { return states.Count == 0; } }
        public StateManager(Game game)
        {
            this.game = game;
            states = new Stack<State>();
        }

        public void Push(State newState)
        {
            newState.OnEnter();
            states.Push(newState);
        }

        public State Pop()
        {
            if (!StackIsEmpty) { states.Peek().OnEnter(); return states.Pop(); }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            if (!StackIsEmpty) { states.Peek().Update(gameTime); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!StackIsEmpty) { states.Peek().Draw(spriteBatch); }
        }
    }
}
