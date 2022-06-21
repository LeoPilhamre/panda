using System;
using Panda.Input;
// using Panda.Networking;


namespace Panda
{

    public sealed class Game
    {

        readonly Window window;
        readonly Keyboard keyboard;
        /*readonly Server server;
        readonly Client client;*/


        public Game(Window window, Keyboard keyboard/*, Server server, Client client*/)
        {
            this.window = window;
            this.keyboard = keyboard;
            /*this.server = server;
            this.client = client;*/
        }


        public void Start() => window.Load(() =>
        {
            // onOpen
            // server.Start();
            // client.Connect();

        }, () =>
        {
            // onClose
            // server.Stop();
            // client.Stop();

            window.Dispose();

        }, dt => {
            // onUpdate
            // server.Tick();
            // client.Tick();

        }, () =>
        {
            // onRender


        }, keyboard.KeyDown, keyboard.KeyUp);

    }

}
