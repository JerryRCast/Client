using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSC.Modell
{
    public class ClientsModeling
    {
        // Propiedades para operaciones del cliente
        public class Client
        {
            public OperationType operation { get; set; }// Tipo de operacion a ralizar
            public string username { get; set; }// User de empresa
            public string password { get; set; }// Pass de empresa
            public string rfc { get; set; }// Identificador de empresa
            public int topRange { get; set; } // Cantidad de elementos a mostrar en los SELECT
            public int index { get; set; }// Desde Donde empezar a mostrar en TopRange
            public string[] indexFiles { get; set; } // Cantidad de archivos a Borrar
            public int mountFiles { get; set; } // Cantidad de archivos a Cargar
        }

        // Propiedades para respuesta a operaciones del cliente
        public class Data
        {
            public bool flag { get; set; }
            public List<Permissions.UserData> userData { get; set; }
            public List<Permissions.UserPrivileges> privilegeData { get; set; }
            public List<Permissions.UserView> userviewData { get; set; }
            public List<FileModel> filesData { get; set; }
        }

        // Propiedades para petiticion
        public class PetitionData
        {
            public Client clientData { get; set; }
            public FileModel fileToUpload { get; set; } // Archivo a cargar
            public string fileName { get; set; }
            public string fileFormat { get; set; }
            public string fileLoadDate { get; set; }
            public string fileSealDate { get; set; }
            public string fileArea { get; set; }
        }
    }
}