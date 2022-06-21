using System;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;


namespace Panda.Input
{

    public sealed class Keyboard
    {

        public event Action<Key> OnKeyDown;

        public event Action<Key> OnKeyUp;


        public Keyboard()
        {

        }


        internal void KeyDown(Key key)
        {
            OnKeyDown?.Invoke(key);
        }

        internal void KeyUp(Key key)
        {
            OnKeyUp?.Invoke(key);
        }

    }

}
