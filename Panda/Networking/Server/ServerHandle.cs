using Utils.Console;


namespace Panda.Networking.Server
{

    internal static class ServerHandle
    {

        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            string username = packet.ReadString();

            WriteLine.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
            
            if (fromClient != id)
                WriteLine.LogWarning($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({id})!");
            
            
        }

    }

}