using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;

namespace Treinamento.WEB.Tabelas.cidade
{
    public partial class Listar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            GridView1.DataSource = CidadeBLL.Instance.Listar(Ordem<Cidade>.ASC(x => x.Uf),Ordem<Cidade>.ASC(x => x.Nome));
            GridView1.DataBind();

            if (GridView1.Rows.Count == 0)
                LabelMensagem.Visible = true;
        }
    }
}