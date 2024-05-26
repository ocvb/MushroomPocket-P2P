using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using Newtonsoft.Json;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public enum Message
    {
        FAILURE,
        CLIENT_CONNECT,
        SERVER_CONNECT
    }
    public static class Server
    {

        public static int ServerPort = 8080;
        public static int ConnectingToServer_port = 8081;
        static List<IPEndPoint> peers = new List<IPEndPoint>();

        public static IPAddress getIp()
        {
            IPAddress ipv4W;
            IPAddress.TryParse(GetAllLocalIPv4(NetworkInterfaceType.Wireless80211).FirstOrDefault(), out ipv4W);

            IPAddress ipv4E;
            IPAddress.TryParse(GetAllLocalIPv4(NetworkInterfaceType.Ethernet).FirstOrDefault(), out ipv4E);

            return ipv4W ?? ipv4E;
        }

        // public static TcpListener StartServer(bool testip = false)
        // {
        //     IPAddress ip;
        //     if (testip)
        //     {
        //         ip = IPAddress.Parse("127.0.0.1");
        //         Console.WriteLine("Test IP: " + ip);
        //     }
        //     else
        //     {
        //         ip = getIp();
        //     }
        //     TcpListener server = new TcpListener(ip, default_PORT);
        //     server.Start();
        //     Console.WriteLine("Server started...");

        //     TcpClient client = server.AcceptTcpClient();
        //     HandleClient(client);
        //     return server;
        // }

        public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string message = Encoding.ASCII.GetString(buffer, 0, data);

            Console.WriteLine("Received: " + message.ToString());
            if (message.StartsWith("register"))
            {
                peers.Add((IPEndPoint)client.Client.RemoteEndPoint);
                // Console.WriteLine($"Peer connected! {client.Client.RemoteEndPoint}");
            }
        }



        public static TcpListener ConnectToPeer(string ip, int port, Message message)
        {
            using (TcpClient c = new TcpClient(ip, port))
            using (NetworkStream ns = c.GetStream())
            {
                byte[] data = Encoding.ASCII.GetBytes(message.ToString());
                ns.WriteAsync(data, 0, data.Length);
            }

            IPAddress ipAddr = IPAddress.Parse(ip);

            return new TcpListener(ipAddr, port);
        }

        public static void SendToPeer(string ip, int port, object message)
        {
            using (TcpClient client = new TcpClient(ip, port))
            using (NetworkStream ns = client.GetStream())
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(message, settings);
                byte[] data = Encoding.ASCII.GetBytes(json);
                ns.WriteAsync(data, 0, data.Length);
            }
        }

        public static string PeerResponse(this TcpListener _)
        {
            var task = Task.Run(() =>
            {

                string data;
                using (TcpClient client = _.AcceptTcpClient())
                using (NetworkStream ns = client.GetStream())
                {

                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    byte[] response = new byte[1024];
                    int bytesRead = ns.Read(response, 0, response.Length);
                    data = Encoding.ASCII.GetString(response, 0, bytesRead);
                }
                return data;
            });

            if (task.Wait(50000))
            {
                return task.Result;
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }

    }


    public static class TcpListenerExtensions
    {
        public static Message ListenForMessage(this TcpListener _)
        {
            Message retVal;
            bool success = false;

            // Accept a new TCP request and
            // reference it's network stream
            using (TcpClient client = _.AcceptTcpClient())
            using (NetworkStream ns = client.GetStream())
            {
                // Read bytes from the network stream
                byte[] response = new byte[1024];
                int bytesRead = ns.Read(response, 0, response.Length);

                // The use of the "out" keyword passes retVal by reference
                // which assigns it to the Message type the response parses out to
                // if the parse is successful
                success = Enum.TryParse<Message>(Encoding.ASCII.GetString(response, 0, bytesRead), out retVal);
            }

            if (success)
                return retVal;
            else
                return Message.FAILURE;
        }
    }

}