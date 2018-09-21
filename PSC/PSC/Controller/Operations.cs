using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSC.Modell;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace PSC.Controller
{
    public class Operations
    {
        //--------------------------------------------------------------------------------------------------------->>>
        // Función para ejecutar Select por 1era vez
        public static List<FileModel> GetFiles(string userName, string rfc)
        {
            // Instancia del cliente y de la lista a retornar
            Client cli = new Client();
            List<FileModel> files = new List<FileModel>();
            // Creamos la petición por Default (1er ingreso)
            ClientsModeling.Client clientData = new ClientsModeling.Client()
            {
                operation = OperationType.GetFiles,
                username = userName,
                rfc = rfc,
                topRange = 25,
                index = 1,
            };
            try
            {
                // Se serializa Información
                string dataSend = SendReceiveStructures.JsonSerialize(clientData, null, null, null, null, null);
                // Inicia Cliente y proceso de envío y recepción de datos
                string dataReceive = cli.MakePetition(dataSend);
                // Deserializamos información
                ClientsModeling.Data result = SendReceiveStructures.JsonDeserialize(dataReceive);
                // Evaluación de la bandera recibida
                if (result.flag) files = result.filesData; else files = null;
            }
            catch (Exception ex)
            {
            }
            return files;
        }
        //--------------------------------------------------------------------------------------------------------->>>
        // Sobrecarga de GetFiles para ejecutar todas las operaciones SELECT (siempre se ejecuta después del 1ero)
        public static List<FileModel> GetFiles(ClientsModeling.Client client, string fname, string fformat, string farea, string fdate)
        {
            // Instancia del cliente y de la lista a retornar
            Client cli = new Client();
            List<FileModel> files = new List<FileModel>();
            try
            {
                // Serializamos información a enviar
                string dataSend = SendReceiveStructures.JsonSerialize(client, null, fname, fformat, farea, fdate);
                // Inicia Cliente y proceso de envío y recepción de datos
                string dataReceive = cli.MakePetition(dataSend);
                // Deserializamos información
                ClientsModeling.Data result = SendReceiveStructures.JsonDeserialize(dataReceive);
                // Evaluación de la bandera recibida
                if (result.flag) files = result.filesData; else files = null;
            }
            catch (Exception ex)
            { 
            }
            return files;
        }
        //--------------------------------------------------------------------------------------------------------->>>[!!!! MODIFIED !!!!]
        // Lineas que tengan '***', son nuevas
        // Función para peticion de carga (Cada petición de carga llevará un fragmento del archivo)
        public static void LoadPetition(ClientsModeling.Client client, string areaDestiny, HttpPostedFile file)
        {
            // Instanciamos CLiente
            Client cli = new Client();
            // Generamos fragmentos de archivo
            List<FileModel> fragments = FileSplitter(file); // ***
            foreach (FileModel fragment in fragments) // ***
            { // ***
                // Serializamos información a enviar
                string dataSend = SendReceiveStructures.JsonSerialize(client, fragment, null, null, areaDestiny, null);
                // Inicia proceso de envío y recepción de datos
                cli.MakePetition(dataSend);
            } // ***      
        }
        //--------------------------------------------------------------------------------------------------------->>>[!!!! NEW !!!!!]
        // Función para partir el archivo en diversos fragmentos y generar su info a mandar 
        private static List<FileModel> FileSplitter(HttpPostedFile file)
        { 
            // Lista a retornar con todos los fragmentos de un archivo
            List<FileModel> fileFragmentList = new List<FileModel>();
            // Array para copiar fragmentos de archivo
            byte[] fragment = new byte[1500];
            // Convertimos a bytes info de cada Archivo
            byte[] allFile = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
            // Expresión regular contra la que se validaran los strings
            Regex reg = new Regex("[^a-zA-Z0-9_. ]");
            // Contadores
            int partCount = 1;
            int count = 0;
            int fsize = allFile.Length;
            // Tratamos cada nombre de archivo
            string fName = Path.GetFileNameWithoutExtension(file.FileName.Normalize(NormalizationForm.FormD));
            // Comenzamos a partir el archivo en fragmentos de 1500 bytes
            while (fsize > 0)
            {
                // Copiamos parte del archivo a un nuevo array para enviar
                Array.Copy(allFile,count,fragment,0,1500);
                // Generamos objeto con info del archivo
                fileFragmentList.Add(new FileModel()
                {
                    fileName = reg.Replace(fName, ""),
                    fileFormat = Path.GetExtension(file.FileName),
                    fileSize = file.ContentLength.ToString(),
                    filePart = partCount,
                    fileFragment = fragment
                });
                Array.Clear(fragment, 0, fragment.Length);
                count += 1500;
                fsize -= 1500;
                partCount += 1;
            }
            return fileFragmentList;
        }
        //--------------------------------------------------------------------------------------------------------->>>
        // Cargamos en la función la Lista de archivos
        public static List<FileModel> LoadFiles(IList<HttpPostedFile> files)
        {
            // Instancia del cliente para hacer las peticiones y del objeto FileModel para enviar. Lista a devolver
            Client cli = new Client();
            FileModel fileInfo;
            List<FileModel> filesUpdt = new List<FileModel>();
            // String a mandar con info del archivo, string para tratar cada nombre de archivo, string a recibir con info actualizada
            string strnData = "";
            string fName = "";
            string updatedData = "";
            // Expresión regular contra la que se validaran los strings
            Regex reg = new Regex("[^a-zA-Z0-9_. ]");
            // Buffer de operaciones
            byte[] allFile;
            foreach (var file in files)
            {
                // Convertimos a bytes info de cada Archivo
                allFile = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                // Tratamos cada nombre de archivo
                fName = Path.GetFileNameWithoutExtension(file.FileName.Normalize(NormalizationForm.FormD));
                // Generamos objeto con info del archivo
                fileInfo = new FileModel()
                {
                    fileName = reg.Replace(fName, ""),
                    fileFormat = Path.GetExtension(file.FileName),
                    fileSize = file.ContentLength.ToString()
                };
                // Serializamos directamente la info del archivo
                strnData = JsonConvert.SerializeObject(fileInfo);
                // Se envía info del Archivo y el Archivo
                updatedData = cli.SendFiles(strnData, allFile);
                // Deserializamos información
                ClientsModeling.Data result = new ClientsModeling.Data();

                if (updatedData.Length > 0)
                {
                    result = SendReceiveStructures.JsonDeserialize(updatedData);
                    // Evaluación de la bandera recibida
                    if (result.flag) filesUpdt = result.filesData;
                    else filesUpdt.DefaultIfEmpty(new FileModel());
                }
                else
                {
                    filesUpdt.DefaultIfEmpty(new FileModel());
                }
                // Se limpian los Arrays
                Array.Clear(allFile, 0, allFile.Length);
            }
            return filesUpdt;
        }
        //--------------------------------------------------------------------------------------------------------->>>
        // Función para eleiminar 'N' cantidad de elementos
        public static void DeleteFiles()
        {

        }
    }
}