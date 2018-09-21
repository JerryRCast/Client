using PSC.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace PSC.Controller
{
    public class ConnResources
    {
        public HomeLogin LoginContext = null;
        public LoggedSession LoggedContext = null;
        public int Port = 99, FilePort = 100;
        public string Ip = "127.0.0.1";
        public Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Byte[] bufferReceive = new Byte[1024];
        public Byte[] bufferDBReceive = new Byte[1024];

        public byte[] bufferInternal { get; set; }
        public IPAddress serverAddr { get; set; }
        public IPEndPoint NetLink { get; set; }
        public IPEndPoint DBNetLink { get; set; }
    }
}