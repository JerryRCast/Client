using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSC.Modell;
using PSC.View;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace PSC.Controller
{
    public class SendReceiveStructures
    {
        public static HomeLogin Context = null;

        public SendReceiveStructures(HomeLogin context)
        {
            Context = context;
        }
        public SendReceiveStructures() { }

        // Serialización de un objecto ClientsModeling.Client
        public static string JsonSerialize(ClientsModeling.Client client, FileModel fileFragment, string fname, string fformat, string farea, string fdate)
        {
            //convert data to JSON string
            try
            {
                ClientsModeling.PetitionData package = new ClientsModeling.PetitionData();
                String data = null;
                switch (client.operation)
                {
                    case OperationType.requestAuthentication:
                    case OperationType.GetFiles:
                        {
                            package = new ClientsModeling.PetitionData()
                            {
                                clientData = client,
                                fileArea = "%%",
                                fileFormat = "%%",
                                fileLoadDate = "%%",
                                fileName = "%%"
                            };
                        }
                        break;
                    case OperationType.GetFilesx1:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileName = "%"+fname+"%",
                            fileArea = "%%",
                            fileFormat = "%%",
                            fileLoadDate = "%%"
                        };
                        break;
                    case OperationType.GetFilesxFormat:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileFormat = fformat,
                            fileArea = "%%",
                            fileLoadDate = "%%",
                            fileName = "%%"
                        };
                        break;                    
                    case OperationType.GetFilesxArea:                        
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileArea = farea,
                            fileFormat = "%%",
                            fileLoadDate = "%%",
                            fileName = "%%"
                        };
                        break;
                    case OperationType.GetFilesxDate:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileLoadDate = fdate,
                            fileFormat = "%%",
                            fileArea = "%%",
                            fileName = "%%"
                        };
                        break;
                    case OperationType.GetFilesxAD:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileArea = farea,
                            fileLoadDate = fdate,
                            fileFormat = "%%",
                            fileName ="%%"
                        };
                        break;
                    case OperationType.GetFilesxFD:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileFormat = fformat,
                            fileLoadDate = fdate,
                            fileArea = "%%",
                            fileName = "%%"
                        };
                        break;
                    case OperationType.GetFilesxFA:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileFormat = fformat,
                            fileArea = farea,
                            fileLoadDate = "%%",
                            fileName = "%%"
                        };
                        break;
                    case OperationType.GetFilesxFAD:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileFormat = fformat,
                            fileArea = farea,
                            fileLoadDate = fdate,
                            fileName = "%%"
                        };
                        break;
                    case OperationType.requestUploadFile:
                        package = new ClientsModeling.PetitionData()
                        {
                            clientData = client,
                            fileToUpload = fileFragment,
                            fileArea = farea,
                            fileFormat = "%%",
                            fileLoadDate = "%%",
                            fileName = "%%"
                        };
                        break;
                }
                data = JsonConvert.SerializeObject(package);
                return data;
            }
            catch (Exception ex)
            {
                Context.scriptMessage("ERROR AL SERIALIZAR: " + ex.Message);
            }
            return null;
        }

        // Deserialización---------------------------------------------------------------------------------->>>
        public static ClientsModeling.Data JsonDeserialize(string serverDataPkg)
        {
            try
            {
                ClientsModeling.Data serverdata = JsonConvert.DeserializeObject<ClientsModeling.Data>(serverDataPkg);
                return serverdata;
            }
            catch (Exception ex)
            {
                Context.scriptMessage("ERROR AL DESERIALIZAR: " + ex.Message);
            }
            return null;
        }
    }
}