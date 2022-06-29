using System;
using System.Collections.Generic;
using Panda.Input;
using Panda.Networking.Server;
using Panda.Rendering;


namespace Panda
{

    public sealed class Game
    {

        readonly Window window;
        readonly Server server;
        readonly List<Entity> entities;
        readonly Keyboard keyboard;
        readonly Renderer renderer;


        public Game(Window window, Server server, List<Entity> entities)
        {
            this.window = window;
            this.server = server;
            this.entities = entities;

            keyboard = new Keyboard();
            renderer = new Renderer();
        }


        public void Start() => window.Load(() =>
        {
            // onOpen
            server.Start();
            renderer.OnOpen(Window.window);

            foreach (Entity entity in entities)
                entity.Start();

        }, () =>
        {
            // onClose
            window.Dispose();
            renderer.Dispose();

            foreach (Entity entity in entities)
                entity.Close();

        }, dt => {
            // onUpdate
            ThreadManager.UpdateMain();

            foreach (Entity entity in entities)
                entity.Update(dt);

        }, () =>
        {
            // onRender
            renderer.OnRender();

        }, keyboard.KeyDown, keyboard.KeyUp);

    }

}
