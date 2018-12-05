using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Global;
using Treinamento.BLL.Beneficio;

namespace Treinamento.WEB.Beneficio.folha
{
    public partial class Listar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<Empregado> lEmpregados = EmpregadoBLL.Instance.Listar(Ordem<Empregado>.ASC(x => x.Nome));
            GridView1.DataSource = lEmpregados;
            GridView1.DataBind();

            if (GridView1.Rows.Count == 0)
                LabelMensagem.Visible = true;
        }

    }
}