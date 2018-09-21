using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace PSC.Controller
{
    public class ClientV2
    {
        //--------------------------------------------------------------------------------------------------------->>>
        // Recursos de conexión
        private class Resources
        {
            public const int Port = 99;
            public static string Ip = "127.0.0.1";
            public static IPAddress localAddr { get; set; }
            public static IPEndPoint NetLink { get; set; }
            public static TcpClient clientTCP { get; set; }
            public static Socket client { get; set; }
            public static Socket fclient { get; set; }
        }

        //--------------------------------------------------------------------------------------------------------->>>
        // Inicia recursos de conexión
        private void InitResource()
        {
            Resources.localAddr = IPAddress.Parse(Resources.Ip);
            Resources.NetLink = new IPEndPoint(Resources.localAddr, Resources.Port);
            Resources.clientTCP = new TcpClient();
            Resources.client = Resources.clientTCP.Client;
        }

        //--------------------------------------------------------------------------------------------------------->>>
        // Inicia conexión
        private void InitConn()
        {
            InitResource();
            Resources.clientTCP.Connect(Resources.NetLink);
        }

        // Función para envio de datos (Peticiones)
        private void SendPetition(string petition)
        {
            byte[] dataToSend = Encoding.ASCII.GetBytes(petition);
            EndPoint remoteEP = Resources.client.RemoteEndPoint; // ERROR!!! NO ME DEJA UTILIZAR ESTE ELEMENTO YA QUE LO DESTRUÍ ANTES...BUSCAR LA MANERA DE USARLO
            // Enviamos datos
            Resources.client.SendTo(dataToSend, dataToSend.Length, SocketFlags.None, remoteEP);
        }

        //--------------------------------------------------------------------------------------------------------->>>
        // Función para recepción de datos
        private string ReceiveResponse()
        {
            byte[] dataToReceive = new byte[1500];
            string dataReceived = "";
            int receiveSize = 0;
            EndPoint remoteEP = Resources.client.RemoteEndPoint;
            //Resources.client.ReceiveTimeout = 3000;
            // Esperamos  a que llegue información
            Resources.client.Poll(5000000, SelectMode.SelectRead);
            // mientras haya data disponible 
            while (Resources.client.Available > 0)
            {
                receiveSize = Resources.client.Available;
                Resources.client.ReceiveFrom(dataToReceive, 0, receiveSize, SocketFlags.Partial, ref remoteEP);
                dataReceived = dataReceived + Encoding.ASCII.GetString(dataToReceive);
                Array.Clear(dataToReceive, 0, dataToReceive.Length);
                // Esperamos  a que llegue información nuevamente (esta vez menos tiempo)
                Resources.client.Poll(3000000, SelectMode.SelectRead);
            }
            return dataReceived;
        }
    }
}