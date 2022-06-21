using System;
using Panda.Input;
using Panda.Networking;


namespace Panda
{

    public sealed class Game
    {

        readonly Window window;
        readonly Keyboard keyboard;
        readonly Server server;


        public Game(Window window, Keyboard keyboard, Server server)
        {
            this.window = window;
            this.keyboard = keyboard;
            this.server = server;
        }


        public void Start() => window.Load(() =>
        {
            // onOpen
            server.Start();

        }, () =>
        {
            // onClose
            window.Dispose();

        }, dt => {
            // onUpdate

        }, () =>
        {
            // onRender


        }, keyboard.KeyDown, keyboard.KeyUp);

    }

}
