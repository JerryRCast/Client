using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSC.Controller;
using PSC.Modell;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;

namespace PSC.View
{
    public partial class LoggedSession : System.Web.UI.Page
    {
        //------------------------------------------------------------------>>>
        // Mostramos elementos en la tabla
        protected void ShowDocs(bool FirstTime, List<FileModel> files)
        {
            if (FirstTime)
            {
                List<FileModel> filesData = Operations.GetFiles((string)Session["UserName"], (string)Session["RFC"]);
                if (filesData != null)
                {
                    grd.DataSource = filesData;
                    grd.DataBind();
                }
                else
                {
                    string notif = "alert(\"No hay archivos disponibles...\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", notif, true);
                }
            }
            else
            {
                grd.DataSource = files;
                grd.DataBind();
            }
        }
        //------------------------------------------------------------------>>>
        // Cargamos items extraídos de la BD
        protected void LoadItems()
        {
            // Inicialización de objetos
            List<Permissions.UserData> userData = (List<Permissions.UserData>)Session["userData"];
            List<Permissions.UserPrivileges> privilegeData = (List<Permissions.UserPrivileges>)Session["privilegeData"];
            List<Permissions.UserView> userviewData = (List<Permissions.UserView>)Session["userviewData"];

            // Creación de interfaz
            Label sesionAs = (Label)Master.FindControl("sesionAs");
            sesionAs.Text = (string)Session["UserName"] + "\n" + userData[0].desc_perfil;

            ContentPlaceHolder tabs = Page.Master.FindControl("headerMenu") as ContentPlaceHolder;
            // For para PageNames
            for (int i = 0; i < userviewData.Count; i++)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes.Add("Class", "liClase");
                tabs.Controls.Add(li);
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("Class", "aClase");
                anchor.Attributes.Add("href", "#");

                li.Controls.Add(anchor);
                var header = new Label
                {
                    Text = userviewData[i].name_Page + ""
                };
                anchor.Controls.Add(header);
            }
            // For para AreaNames
            for (int i = 0; i < userData.Count; i++)
            {
                areaFilter.Items.Add(new ListItem(userData[i].desc_area, Convert.ToString(userData[i].id_area)));
                areaLPanel.Items.Add(new ListItem(userData[i].desc_area, Convert.ToString(userData[i].id_area)));
            }
            // For para items y funciones
            for (int i = 0; i < privilegeData.Count; i++)
            {
                if (privilegeData[i].method == "loadFile")
                {
                    LoadFilesBtn.Visible = true;
                }
                else
                {
                    var btnMenu = new Button
                    {
                        ID = "creatingB" + i,
                        CommandArgument = (string)privilegeData[i].method,
                        Text = privilegeData[i].desc1 + "",
                        CssClass = "btnPag1"
                    };
                    btnMenu.Command += Load_Items;
                    bMenu.Controls.Add(btnMenu);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Nos muestra los archivos
                ShowDocs(true, null);
                //DateTxt.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            LoadItems();
        }
        //------------------------------------------------------------------>>>
        // Carga de items para la interfaz
        private void Load_Items(object sender, CommandEventArgs e)
        {
            if ((string)e.CommandArgument == "sealFile")
            {
                showMenuTab2();
            }
            else if ((string)e.CommandArgument == "getCertificate")
            {
                showMenuTab2();
            }
            else if ((string)e.CommandArgument == "deleteFile")
            {
                showMenuTab2();
            }
        }

        private void showMenuTab2()
        {
            //Response.Redirect("~/View/HomeLogin.aspx");
        }
        //------------------------------------------------------------------>>>
        // Función para mostrar alertas al usuario
        public void scriptMessage(string message)
        {
            string script = "alert(\'" + message + "\');";
            ScriptManager.RegisterStartupScript(this, GetType(),
                              "ServerControlScript", script, true);
        }
        //------------------------------------------------------------------>>>
        // Comenzamos proceso de petición de carga
        protected void panelAccept_Click(object sender, EventArgs e)
        {
            // Declaramos lista a mostrar en la tabla
            List<FileModel> files = new List<FileModel>();
            // Hay archivos cargados???
            if (FileUpload1.HasFiles)
            {
                if (!areaLPanel.SelectedValue.Equals(""))
                {
                    // Creamos data del cliente a enviar
                    ClientsModeling.Client client = new ClientsModeling.Client()
                    {
                        operation = OperationType.requestUploadFile,
                        username = (string)Session["userName"],
                        rfc = (string)Session["RFC"],
                        topRange = 25,
                        index = 1,
                        mountFiles = FileUpload1.PostedFiles.Count
                    };
                    foreach (HttpPostedFile file in FileUpload1.PostedFiles)
                    {
                        // Hacemos petición de carga con un fragmento de cada archivo y para todos los archivos de uno en uno
                        Operations.LoadPetition(client, areaLPanel.SelectedValue, file);
                    }
                    
                    // Proceso de envio para archivos
                    files = Operations.LoadFiles(FileUpload1.PostedFiles);//------------------------------->>>Asincronía??
                    // Mostramos datos
                    if (files.Count > 0)
                    {
                        ShowDocs(false, files);
                    }
                    else
                    {
                        files = Operations.GetFiles((string)Session["userName"], (string)Session["RFC"]);
                        ShowDocs(false, files);
                    }
                }
                else scriptMessage("No se seleccionó un área!!!");
            }
            else scriptMessage("No hay archivos que cargar!!!");
        }
        //------------------------------------------------------------------>>>
        // Botón para buscar documentos por coincidencia de caracteres
        protected void FinderBtn_Click(object sender, EventArgs e)
        {
            // Creamos una onstancia de operacion de cliente
            ClientsModeling.Client client;
            // Declaramos lista a mostrar en la tabla
            List<FileModel> files = new List<FileModel>();
            string Fname = WordFilterTxt.Text;
            if (Fname.Equals(""))
            {
                scriptMessage("No puedes hacer una búsqueda sin un parametro a buscar...");// ---------------------------------------------->>> JERRY DEL FUTURO, TERMINA ESTA FUNCIÓN POR MI...
            }
            else
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesx1,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, Fname, null, null, null);
            }
            ShowDocs(false, files);
        }
        //------------------------------------------------------------------>>>
        // Botón para aplicar filtros de búsqueda
        protected void ApplyFilter_Click(object sender, EventArgs e)
        {
            // Declaramos lista a mostrar en la tabla
            List<FileModel> files = new List<FileModel>();
            // Declaramos y asignamos valor a variables 
            string Farea = areaFilter.SelectedValue, Fformat = formatFilter.SelectedValue, Fdate = DateTxt.Text;
            // Creamos una onstancia de operacion de cliente
            ClientsModeling.Client client;
            // Evaluar que operación realizar
            if (Farea.Equals("") && Fformat.Equals("") && Fdate.Equals(""))// Búsqueda sin ningún filtro (*)
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFiles,
                    topRange = 25,
                    index = 1
                };
                Operations.GetFiles(client, null, null, null, null);
            }
            else if (!Farea.Equals("") && !Fformat.Equals("") && !Fdate.Equals(""))// Búsqueda con los 3 filtros activos
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxFAD,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, Fformat, Farea, Fdate);
            }
            else if (!Fformat.Equals("") && Farea.Equals("") && Fdate.Equals(""))// Búsqueda por Formato
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxFormat,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, Fformat, null, null);
            }
            else if (Fformat.Equals("") && !Farea.Equals("") && Fdate.Equals(""))// Búsqueda por Área
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxArea,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, null, Farea, null);
            }
            else if (Fformat.Equals("") && Farea.Equals("") && !Fdate.Equals(""))// Búsqueda por Fecha
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxDate,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, null, null, Fdate);
            }
            else if (Fformat.Equals("") && !Farea.Equals("") && !Fdate.Equals(""))// Búsqueda por Área y Fecha
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxAD,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, null, Farea, Fdate);
            }
            else if (!Fformat.Equals("") && Farea.Equals("") && !Fdate.Equals(""))// Búsqueda por Formato y Fecha
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxFD,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, Fformat, null, Fdate);
            }
            else if (!Fformat.Equals("") && !Farea.Equals("") && Fdate.Equals(""))// Búsqueda por Formato y Área
            {
                client = new ClientsModeling.Client()
                {
                    rfc = (string)Session["RFC"],
                    username = (string)Session["UserName"],
                    operation = OperationType.GetFilesxFA,
                    topRange = 25,
                    index = 1
                };
                files = Operations.GetFiles(client, null, Fformat, Farea, null);
            }
            ShowDocs(false, files);
        }
    }
}