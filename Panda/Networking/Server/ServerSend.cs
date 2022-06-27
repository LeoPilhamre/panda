

namespace Panda.Networking.Server
{

    internal static class ServerSend
    {

        public static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        public static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxClientConnections; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        public static void SendTCPDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.maxClientConnections; i++)
            {
                if (i != exceptClient)
                    Server.clients[i].tcp.SendData(packet);
            }
        }

        #region Packets

        public static void Welcome(int toClient, string msg)
        {
            using (Packet packet = new Packet((int) ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }

        #endregion

    }

}