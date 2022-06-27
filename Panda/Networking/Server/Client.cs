using System;
using System.Net;
using System.Net.Sockets;
using Utils.Console;


namespace Panda.Networking.Server
{
    
    public sealed class Client : Panda.Networking.Settings
    {


        readonly int id;


        public TCP tcp;

        
        public Client(int id)
        {
            this.id = id;
            tcp = new TCP(id);
        }


        public class TCP
        {

            public TcpClient socket;

            readonly int id;

            private NetworkStream stream;
            private Packet receivedPacket;
            private byte[] receiveBuffer;


            public TCP(int id)
            {
                this.id = id;
            }


            internal void Connect(TcpClient socket)
            {
                this.socket = socket;

                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedPacket = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "Welcome to the server!");
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    WriteLine.LogError($"Error sending data to player {id} via TCP: {e}");
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
                            Server.packetHandlers[packetId](id, packet);
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

    }

}