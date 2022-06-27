

namespace Panda.Networking.Client
{

    internal static class ClientSend
    {

        public static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }

        #region Packets

        public static void WelcomeReceived()
        {
            using (Packet packet = new Packet((int) ClientPackets.welcomeReceived))
            {
                packet.Write(Client.instance.id);
                packet.Write("cum"); // USERNAME

                SendTCPData(packet);
            }
        }

        #endregion

    }

}