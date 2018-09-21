using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSC.Controller;
using PSC.Modell;

namespace PSC.View
{
    public partial class HomeLogin : System.Web.UI.Page
    {
        AuthenticationOps op = new AuthenticationOps();
        AuthenticationOps authenticationObject = null;
        SendReceiveStructures serializeJsonObject = null;
        Client clientObject = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Clear();
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            authenticationObject = new AuthenticationOps(this);
            serializeJsonObject = new SendReceiveStructures(this);
            clientObject = new Client(this);
        }

        protected void loginB_Click(object sender, EventArgs e)
        {
            try
            {
                if (rfcT.Text.Length > 1 && (rfcT.Text.Length < 12 || rfcT.Text.Length > 13))
                {
                    errorSesion.Text = "";
                    errorLengthRFC.Text = "Verifique que el RFC contenga 12 o 13 caracteres";
                }
                else if (userNameT.Text.Length == 0 || passwordT.Text.Length == 0 || rfcT.Text.Length == 0)
                {
                    errorLengthRFC.Text = "";
                    errorSesion.Text = "Verifique que todos los campos hayan sido llenados";
                }
                else
                {
                    errorLengthRFC.Text = "";
                    errorSesion.Text = "";
                    ClientsModeling.Client dataLogin = new ClientsModeling.Client()
                    {
                        operation = OperationType.requestAuthentication,
                        rfc = rfcT.Text,
                        username = userNameT.Text,
                        password = passwordT.Text
                    };
                    // Ejecutamos autenticación
                    ClientsModeling.Data response = op.RequestAuthentication(dataLogin);

                    //Evaluamos
                    if (response == null) errorSesion.Text = "Parece que hubo un error con la conexión";
                    else
                    {
                        if (response.flag)
                        {
                            // Seteamos información para conexión al sistema
                            Session["RFC"] = rfcT.Text;
                            Session["userName"] = userNameT.Text;
                            Session["userData"] = response.userData;
                            Session["privilegeData"] = response.privilegeData;
                            Session["userviewData"] = response.userviewData;
                            // Redirigimos al Gestor
                            //Response.Redirect("https://www.google.com.mx");
                            Response.Redirect("~/View/LoggedSession.aspx");
                        }
                        else errorSesion.Text = "Verifique que sus datos sean correctos";
                    } 
                }
            }
            catch (Exception ex)
            {
                scriptMessage("ERROR: " + ex.Message + ", " + ex.StackTrace);
            }
        }

        public void scriptMessage(string message)
        {
            string script = "alert(\'" + message + "\');";
            ScriptManager.RegisterStartupScript(this, GetType(),
                              "ServerControlScript", script, true);
        }

        public void Logger(string msg)
        {
            ClientErrors.Text = msg;
        }
    }
}