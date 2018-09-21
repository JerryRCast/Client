using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSC.Modell
{
    public class Permissions
    {
        // Definición de usuario
        public class UserData
        {
            public int id_perfil { get; set; }
            public int id_area { get; set; }
            public string desc_perfil { get; set; }
            public string desc_area { get; set; }
        }

        // Propiedades para los permisos del usuario
        public class UserPrivileges
        {
            public int id_item { get; set; }
            public string desc1 { get; set; }
            public string type { get; set; }
            public string method { get; set; }
        }

        // Propiedades para crear la vista
        public class UserView
        {
            public int id_Page { get; set; }
            public string name_Page { get; set; }
        }
    }
}