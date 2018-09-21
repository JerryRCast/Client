using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using PSC.View;
using System.Text;
using PSC.Modell;
using System.Threading;

namespace PSC.Controller
{
    public class Client
    {
        HomeLogin Context = null;

        public Client()
        { }
        public Client(HomeLogin context)
        {
            Context = context;
        }
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

        //--------------------------------------------------------------------------------------------------------->>>
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
        //--------------------------------------------------------------------------------------------------------->>>
        // Función para enviar Archivos
        public string SendFiles(string infoFile, byte[] fileData) // GENERAR CONEXION PARA ESTE NUEVO CLIENTE A TRÁVES DE OTRO PUERTO (100)
        {
            string updatedData = "";
            try
            {
                // Iniciamos otra conexión
                InitConn();
                EndPoint remoteEP = Resources.client.RemoteEndPoint;
                // revisamos si hay o no conexión
                bool conn = Resources.client.Connected;
                if (conn)
                {
                    // Enviamos cada objeto con info del archivo
                    SendPetition(infoFile);
                    // Aqui cortamos y enviamos el archivo...
                    byte[] dataToSend = new byte[1500];
                    int FileSize = fileData.Length;
                    // Esperamos antes de empezar a enviar el archivo
                    Thread.Sleep(3000);
                    // Enviamos el archivo
                    if (FileSize <= 1500)
                    {
                        dataToSend = fileData;
                        Resources.client.SendTo(dataToSend, dataToSend.Length, SocketFlags.None, remoteEP);
                        Array.Clear(dataToSend, 0, dataToSend.Length);
                    }
                    else
                    {
                        int count = 0;
                        while (FileSize > 1500)
                        {
                            Array.Copy(fileData, count, dataToSend, 0, 1500);
                            Resources.client.SendTo(fileData, fileData.Length, SocketFlags.Partial, remoteEP);
                            Array.Clear(dataToSend, 0, dataToSend.Length);
                            count = count + 1500;
                            FileSize = FileSize - 1500;
                        }
                    }
                    // Esperamos a que haya Info
                    Thread.Sleep(3000);
                    // Recibimos actualización de datos
                    updatedData = ReceiveResponse();
                }
                Resources.client.Close();
            }
            catch (Exception)
            {
            }
            return updatedData;
        }
        //--------------------------------------------------------------------------------------------------------->>>
        // Se hace la petición
        public string MakePetition(string petition)
        {
            string response = "";
            try
            {
                // Iniciamos conexión
                InitConn();
                // revisamos si hay o no conexión
                bool conn = Resources.client.Connected;
                if (conn)
                {
                    SendPetition(petition);
                    Resources.client.Poll(300000,SelectMode.SelectRead); // ***
                    if (Resources.client.Available > 0) // ***
                    { // ***
                        response = ReceiveResponse();
                    } // ***             
                }
                Resources.client.Close(); 
            }
            catch (Exception ex)
            {
                Context.scriptMessage("Error de Servidor: " + ex.Message);
                Resources.client.Close();
            }
            return response;
        }
        

        /*// Se envian datos al servidor y esperamos respuesta de este
        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.BeginSend(bufferSend, 0, bufferSend.Length, SocketFlags.Partial, new AsyncCallback(SendCallback), client);
            }
            catch (Exception ex)
            {
                LoginContext.scriptMessage("Ocurrió un error durante la petición...\n" + ex.Message);
                //resrc.Context.Logger("Error en ConnectCallback: " + ex.Message + ", " + ex.HResult + ", " + ex.StackTrace);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.BeginReceive(resrc.bufferReceive, 0, resrc.bufferReceive.Length, SocketFlags.None,new AsyncCallback(ReceiveCallback), client);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // Instancia para serializar y desearializar
            SendReceiveStructures data = new SendReceiveStructures();
            // Terminamos de recibir información 
            Socket server = (Socket)ar.AsyncState;
            // Esperamos a terminar de recibir la información
            bool flag = ar.AsyncWaitHandle.WaitOne(-1,false);
            try
            {
                int size = server.EndReceive(ar);//......................................................ERROR DE SOCKET EXCEPTION
                                                 // Tratamos información recibida
                resrc.bufferInternal = new byte[size];
                Array.Copy(resrc.bufferReceive, resrc.bufferInternal, size);

                String responseData = Encoding.ASCII.GetString(resrc.bufferInternal);

                // Deserializamos info del Servidor
                ClientsModeling.Data receiveData = SendReceiveStructures.JsonDeserialize(responseData);
                // Seteamos la info recibida del servidor para su posterior uso
                AuthenticationOps.receiveData = receiveData;
                // Cerramos el envío y transmisión de datos
                server.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), server);
            }
            catch (Exception ex)
            {
                resrc.LoginContext.scriptMessage("Error: " + ex.Message);
            }
            finally
            {
                server.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), server);
            }
        }

        private static void DisconnectCallback(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;
            server.Close();
        }*/
    }
}