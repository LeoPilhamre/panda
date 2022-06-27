using Utils.Console;


namespace Panda.Networking.Client
{

    internal static class ClientHandle
    {

        public static void Welcome(Packet packet)
        {
            string msg = packet.ReadString();
            int id = packet.ReadInt();

            WriteLine.Log($"Message from server: {msg}");
            Client.instance.id = id;

            ClientSend.WelcomeReceived();
        }

    }

}