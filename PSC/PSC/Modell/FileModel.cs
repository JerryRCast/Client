using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSC.Modell
{
    public class FileModel
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public string strnHash { get; set; }
        public string OID { get; set; }
        public string fileFormat { get; set; }
        public string fileSize { get; set; }
        public string fileLoadDate { get; set; }
        public string fileSealDate { get; set; }
        public int sealFlag { get; set; }
        public string filePath { get; set; }
        public int fileArea { get; set; }
        public int filePart { get; set; }
        public byte[] fileFragment { get; set; }
    }
}