using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSC.Modell
{
    public enum OperationType
    {
        DefaultOperation = 0,
        requestAuthentication = 1,
        requestUploadFile = 2,
        GetFiles = 3,
        GetFilesx1 = 4,
        GetFilesxFormat = 5,
        GetFilesxArea = 6,
        GetFilesxDate = 7,
        GetFilesxAD = 8,
        GetFilesxFD = 9,
        GetFilesxFA = 10,
        GetFilesxFAD = 11,
    }
}