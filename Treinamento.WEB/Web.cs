using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace Treinamento.WEB
{
    public class Web
    {
        public static void ExibeAlerta(Page pagina, String mensagem)
        {
            pagina.ClientScript.RegisterClientScriptBlock(pagina.GetType(),
                                                        "erro",
                                                        "alert('" + RetirarCaracteresInvalidos(mensagem) + "');",
                                                        true);
        }

        public static void ExibeAlerta(Page pagina, String mensagem, String redirecionamento)
        {

            ExibeAlerta(pagina, mensagem);

            pagina.ClientScript.RegisterClientScriptBlock(pagina.GetType(), 
                                                        "redireciona", 
                                                        "document.location = '" + RetirarCaracteresInvalidos(redirecionamento) + "';", 
                                                        true);
        }
        
        private static string RetirarCaracteresInvalidos(string mensagem)
        {
            return mensagem.Replace("'", "\\'").Replace("\r\n", " ");
        }
    }

}