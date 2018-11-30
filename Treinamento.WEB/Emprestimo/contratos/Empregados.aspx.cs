using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.BLL.Beneficio;
using Treinamento.DTO.Beneficio;

namespace Treinamento.WEB.Emprestimo.contratos
{
    public partial class Empregados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = EmpregadoBLL.Instance.Listar();
            GridView1.DataBind();

            if(GridView1.Rows.Count == 0)
            {
                LabelMensagem.Visible = true;              
            }
        }
    }
}