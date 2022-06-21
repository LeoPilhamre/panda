using Panda;
using Panda.Input;
using Panda.Networking;


namespace Samples.Dino;

public class Program
{

    public static void Main(string[] argv)
    {
        Window window = new Window("cum <3", 1280, 720);
        Keyboard keyboard = new Keyboard();

        Server server = new Server(7777, 10);

        new Game(window, keyboard, server).Start();
    }

}
