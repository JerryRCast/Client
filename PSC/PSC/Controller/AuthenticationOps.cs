using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSC.Modell;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using PSC.View;

namespace PSC.Controller
{
    public class AuthenticationOps
    {
        Client client = new Client();
        // Instancia de la vista
        public static HomeLogin Context = null;

        // Constructor para ligar lógica y vista
        public AuthenticationOps() { }
        public AuthenticationOps(HomeLogin context)
        {
            Context = context;
        }

        // Función para petición de autenticación
        public ClientsModeling.Data RequestAuthentication(ClientsModeling.Client dataLogin)
        {
            ClientsModeling.Data petitionResponse = new ClientsModeling.Data();
            try
            {
                // Serializamos y convertimos en bytes
                string dataSend = SendReceiveStructures.JsonSerialize(dataLogin,null,null,null,null,null);
                // Inicia Cliente y proceso de envío y recepción de datos
                string dataReceive = client.MakePetition(dataSend);
                // Deserializamos información
                petitionResponse =  SendReceiveStructures.JsonDeserialize(dataReceive); 
            }
            catch (Exception ex)
            {
                //Context.scriptMessage("Error al intentar conectar...\r\n" + ex.Message);
                Context.Logger("Error: " + ex.Message + ",\r\n" + ex.HResult + ",\r\n" + ex.StackTrace);
            }
            return petitionResponse;
        }
    }
}