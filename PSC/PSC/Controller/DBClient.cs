using PSC.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace PSC.Controller
{
    public class DBClient
    {
        // Instancia de datos de conexión
        ConnResources resrc = new ConnResources();
        // Propiedad de buffer para enviar
        public static Byte[] bufferDBSend { get; set; }
        public static Byte[] bufferDBPetition { get; set; }
        // Socket para para las conexiones
        Socket client;

        // Contructor por default y para un nuevo cliente, también vícula la lógica con la vista
        public DBClient() { }
        public DBClient(LoggedSession context)
        {
            resrc.LoggedContext = context;
            client = resrc.clientSocket;
        }

        // Función para inicializar los recursos del cliente
        private void InitResource()
        {
            resrc.serverAddr = IPAddress.Parse(resrc.Ip);
            resrc.DBNetLink = new IPEndPoint(resrc.serverAddr, resrc.FilePort);
            TcpClient DBclientTCP = new TcpClient(resrc.DBNetLink);
            client = DBclientTCP.Client;
        }

        // Iniciamos el cliente
        public void Start()
        {
            try
            {
                // Creamos nuevo cliente y se comienza envío de datos
                CreateClient();
            }
            catch (Exception ex)
            {
                // Establecemos mensajes de error
                resrc.LoggedContext.scriptMessage(ex.Message);
            }
        }

        // Función para crear un nuevo cliente
        private void CreateClient()
        {
            // Inicializamos recursos del cliente
            InitResource();
            // 
            EndPoint remoteEP = resrc.DBNetLink;
            // Conectamos con el host remoto
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), null);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            EndPoint remoteEP = client.RemoteEndPoint;
            try
            {
                //client.Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}