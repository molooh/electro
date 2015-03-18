using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Examples.TCP_ServerTest
{
    public class ThreadPoolTcpSrvr
    {
        private TcpListener _listener;

        private List<TcpConnection> Connections;
        
        //Make List accessable in other class
        public List<TcpConnection> GetConnections()
        {
            return Connections;
            
        }

        //Get IP Address
        public static string IpList()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public ThreadPoolTcpSrvr()
        {
            Connections = new List<TcpConnection>();
        }

        public void StartListening()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(IpList()), 9050);
            _listener = new TcpListener(endpoint);
            _listener.Start();

            Console.WriteLine("The local End point is  :" +
                              _listener.LocalEndpoint);

            Console.WriteLine("Waiting for clients...");
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();

                var newconnection = new TcpConnection();
                newconnection.ThreadListener = _listener;
                Connections.Add(newconnection);
                ThreadPool.QueueUserWorkItem(newconnection.HandleConnection, client);
            }
        }
    
    }

    public class TcpConnection
    {
        public TcpListener ThreadListener;
        public string Message = "";
        private ThreadPoolTcpSrvr _tpts;

        public void HandleConnection(object clientOb)
        {
            StringBuilder RecvMessage;
            var client = (TcpClient) clientOb;
            int recv;
            byte[] data = new byte[1024];

            NetworkStream ns = client.GetStream();

            Console.WriteLine("New client accepted"); //": {0} active connections");

            const string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);
            RecvMessage = new StringBuilder();
            int iMsgEnd = 0;

            while (ns.CanRead)
            {
                recv = ns.Read(data, 0, data.Length); //TODO: if client disconnects --> IOExeption, fix it (client.Close() in the Android App!
                ns.Write(data, 0, recv);
                iMsgEnd = RecvMessage.Length;
                RecvMessage.AppendFormat("{0}", Encoding.ASCII.GetString(data, 0, recv));
                for (; iMsgEnd < RecvMessage.Length; iMsgEnd++)
                {
                    if (RecvMessage[iMsgEnd] == ';') //Protocol; in case server receives incomplete data
                    {
                        Message = RecvMessage.ToString(0, iMsgEnd);
                        RecvMessage.Remove(0, iMsgEnd + 1);
                    }
                }
            }

            ns.Close();
            
            // TODO remove this connection from the list: Connections
            
            //_tpts.GetConnections().Remove(RemoveFromList());
            client.Close();
            Console.WriteLine("Client disconnected"); // {0} active connections",_connections);
        }

        //public TcpConnection RemoveFromList()
        //{
        //    return _tpts.GetConnections().Single(x => x.ThreadListener;)
        //}
    }
}