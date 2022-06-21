using System;
using System.Net;
using System.Net.Sockets;
using Utils.Console;


namespace Panda.Networking
{
    
    public sealed class Client
    {

        public static int dataBufferSize = 4096;


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

                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
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

                    // TODO: HANDLE DATA
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    WriteLine.LogError($"Error recieving packet: {e}");
                    // TODO: DISCONNECT
                }
            }

        }

    }

}