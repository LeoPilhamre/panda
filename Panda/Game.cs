using System;
using Panda.Input;
using Panda.Networking.Server;
using Panda.Rendering;


namespace Panda
{

    public sealed class Game
    {

        readonly Window window;
        readonly Keyboard keyboard;
        readonly Renderer renderer;
        readonly Server server;


        public Game(Window window, Server server)
        {
            this.window = window;
            this.server = server;

            keyboard = new Keyboard();
            renderer = new Renderer();
        }


        public void Start() => window.Load(() =>
        {
            // onOpen
            server.Start();
            renderer.OnOpen(Window.window);

        }, () =>
        {
            // onClose
            window.Dispose();
            renderer.Dispose();

        }, dt => {
            // onUpdate
            ThreadManager.UpdateMain();

        }, () =>
        {
            // onRender
            renderer.OnRender();

        }, keyboard.KeyDown, keyboard.KeyUp);

    }

}
