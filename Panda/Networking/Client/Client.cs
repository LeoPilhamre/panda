using System;
using System.Net.Sockets;
using Utils.Console;


namespace Panda.Networking.Client
{
    
    public sealed class Client
    {

        public static Client instance;

        public int id = 0;

        public TCP tcp;

        private delegate void PacketHandler(Packet packet);
        private static Dictionary<int, PacketHandler> packetHandlers;


        public Client()
        {
            if (instance == null)
                instance = this;

            tcp = new TCP();
        }

        public void Connect()
        {
            InitializeClientData();

            tcp.Connect();
        }


        public class TCP : Panda.Networking.Settings
        {

            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedPacket;
            private byte[] receiveBuffer;


            public TCP()
            {
                
            }


            internal void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];

                socket.BeginConnect(ip, port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);

                if (!socket.Connected)
                    return;

                stream = socket.GetStream();

                receivedPacket = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                } catch (Exception e)
                {
                    WriteLine.LogError($"Error sending data to server via TCP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        // TODO: DISCONNECT
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    receivedPacket.Reset(HandleData(data));
                    
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    WriteLine.LogError($"Error recieving packet: {e}");
                    // TODO: DISCONNECT
                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLength = 0;

                receivedPacket.SetBytes(data);

                if (receivedPacket.UnreadLength() >= 4)
                {
                    packetLength = receivedPacket.ReadInt();
                    if (packetLength <= 0)
                        return true;
                }

                while (packetLength > 0 && packetLength <= receivedPacket.UnreadLength())
                {
                    byte[] packetBytes = receivedPacket.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() => 
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            packetHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;
                    if (receivedPacket.UnreadLength() >= 4)
                    {
                        packetLength = receivedPacket.ReadInt();
                        if (packetLength <= 0)
                            return true;
                    }
                }

                if (packetLength <= 1)
                    return true;

                return false;
            }

        }


        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int) ServerPackets.welcome, ClientHandle.Welcome }
            };
        }

    }

}