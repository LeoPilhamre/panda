using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Utils.Console;


namespace Panda.Networking.Server
{
    
    public sealed class Server : Panda.Networking.Settings
    {

        private TcpListener listener;

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> packetHandlers;


        public Server()
        {
            
        }


        internal void Start()
        {
            WriteLine.Log("Starting server...");

            InitializeServerData();

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(Connected), null);

            WriteLine.Log($"Server started on PORT: {port}");
        }

        private void Connected(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);
            listener.BeginAcceptTcpClient(new AsyncCallback(Connected), null);

            WriteLine.Log($"Incoming connection from [{client.Client.RemoteEndPoint}]...");

            for (int i = 1; i <= maxClientConnections; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);

                    WriteLine.Log($"[{client.Client.RemoteEndPoint}] has established a connection!");
                    
                    return;
                }
            }

            WriteLine.LogWarning($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }


        private void InitializeServerData()
        {
            for (int i = 1; i <= maxClientConnections; i++)
                clients.Add(i, new Client(i));

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived }
            };
        }

    }

}