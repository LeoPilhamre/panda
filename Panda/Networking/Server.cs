using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Utils.Console;


namespace Panda.Networking
{
    
    public sealed class Server
    {

        readonly ushort port;
        readonly int maxClientConnections;


        private TcpListener listener;

        private Dictionary<int, Client> clients = new Dictionary<int, Client>();


        public Server(ushort port, int maxClientConnections)
        {
            this.port = port;
            this.maxClientConnections = maxClientConnections;
        }


        internal void Start()
        {
            WriteLine.Log("Starting server...");

            InitializeClients();

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(Connected), null);

            WriteLine.Log($"Server started on PORT: {port}");
        }

        private void Connected(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);
            listener.BeginAcceptTcpClient(new AsyncCallback(Connected), null);

            WriteLine.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxClientConnections; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    
                    return;
                }
            }

            WriteLine.LogWarning($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }


        private void InitializeClients()
        {
            for (int i = 1; i <= maxClientConnections; i++)
                clients.Add(i, new Client(i));
        }

    }

}