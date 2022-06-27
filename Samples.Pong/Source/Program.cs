using Panda;
using Panda.Input;
using Panda.Networking.Server;
using Panda.Networking.Client;


namespace Samples.Dino;

public class Program
{

    public static void Main(string[] argv)
    {
        Window window = new Window("cum <3", 1280, 720);

        Server server = new Server();

        Panda.Networking.Client.Client client = new Panda.Networking.Client.Client();
        client.Connect();

        List<Entity> entities = new List<Entity>();

        new Game(window, server, entities).Start();

        Console.ReadKey();
    }

}
