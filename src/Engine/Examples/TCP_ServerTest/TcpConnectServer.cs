using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

public class ThreadPoolTcpSrvr
{
    private TcpListener client;

    public static string IpList()
    {
        IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in Host.AddressList)
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

        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(IpList()), 9050);
        client = new TcpListener(endpoint);
        client.Start();

        Console.WriteLine("The local End point is  :" +
                              client.LocalEndpoint);

        Console.WriteLine("Waiting for clients...");
        while (true)
        {
            while (!client.Pending())
            {
                Thread.Sleep(1000);
            }
            TcpConnection newconnection = new TcpConnection();
            newconnection.threadListener = this.client;
            ThreadPool.QueueUserWorkItem(new
                       WaitCallback(newconnection.HandleConnection));
        }
    }
    
}

class TcpConnection
{
    public StringBuilder recvMessage;
    public TcpListener threadListener;
    private static int connections = 0;
    public bool MsgOut;

    public string teststring = "";

    public void HandleConnection(object state)
    {
        int recv;
        byte[] data = new byte[1024];

        TcpClient client = threadListener.AcceptTcpClient();
        NetworkStream ns = client.GetStream();
        connections++;
        Console.WriteLine("New client accepted: {0} active connections",
                           connections);

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        ns.Write(data, 0, data.Length);

        while (true)
        {
            data = new byte[1024];

            recv = ns.Read(data, 0, data.Length);
            recvMessage = new StringBuilder();
            recvMessage.AppendFormat("{0}", Encoding.ASCII.GetString(data, 0, recv));
            Console.WriteLine("You received the following message : " + recvMessage);
            
            if (recv == 0 || !ns.CanRead)
                break;
            ns.Write(data, 0, recv);
            teststring = recvMessage.ToString();
        }
        ns.Close();
        client.Close();
        connections--;
        Console.WriteLine("Client disconnected: {0} active connections",
                           connections);
    }

    public string NewCoord()
    {
        return teststring;
     
    }
}

