using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



public class ThreadPoolTcpSrvr
{
    private TcpListener listener;

    public List<TcpConnection> Connections;
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
        listener = new TcpListener(endpoint);
        listener.Start();

        Console.WriteLine("The local End point is  :" +
                              listener.LocalEndpoint);

        Console.WriteLine("Waiting for clients...");
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            var newconnection = new TcpConnection();
            newconnection.ThreadListener = this.listener;
            Connections.Add(newconnection);
            ThreadPool.QueueUserWorkItem(newconnection.HandleConnection, client);

            //while (!listener.Pending())
            //{
            //    Thread.Sleep(1000);
            //}
            //var newconnection = new TcpConnection();
            //newconnection.ThreadListener = this.listener;
            //Connections.Add(newconnection);
            //ThreadPool.QueueUserWorkItem(new
            //           WaitCallback(newconnection.HandleConnection));
        }
    }
    
}

public class TcpConnection
{
    public TcpListener ThreadListener;
    public string Message = "";

    public void HandleConnection(object clientOb)
    {
        StringBuilder RecvMessage;
        TcpClient client = (TcpClient) clientOb;
        int recv;
        byte[] data = new byte[1024];

        NetworkStream ns = client.GetStream();

        Console.WriteLine("New client accepted"); //": {0} active connections");

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        ns.Write(data, 0, data.Length);
        RecvMessage = new StringBuilder();
        int iMsgEnd = 0;

        while (ns.CanRead)
        {
            recv = ns.Read(data, 0, data.Length);
            ns.Write(data, 0, recv);
            iMsgEnd = RecvMessage.Length;
            RecvMessage.AppendFormat("{0}", Encoding.ASCII.GetString(data, 0, recv));
            for (; iMsgEnd < RecvMessage.Length; iMsgEnd++)
            {
                if (RecvMessage[iMsgEnd] == ';')
                {
                    Message = RecvMessage.ToString(0, iMsgEnd);
                    RecvMessage.Remove(0, iMsgEnd+1);
                }
            }
        }

        ns.Close();
        client.Close();
        // TODO remove this connection from the list: _connections;
        Console.WriteLine("Client disconnected"); // {0} active connections",_connections);
    }
}

