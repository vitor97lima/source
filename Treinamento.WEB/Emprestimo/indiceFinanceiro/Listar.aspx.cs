using System;
using System.Linq;
using Treinamento.DTO.Global;
using Treinamento.DTO.Emprestimo;
using Treinamento.BLL.Emprestimo;

namespace Treinamento.WEB.Emprestimo.indiceFinanceiro
{
    public partial class Listar : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			GridView1.DataSource = IndiceFinanceiroBLL.Instance.Listar(Ordem<IndiceFinanceiro>.ASC(x=>x.Codigo));
			GridView1.DataBind();

			if (GridView1.Rows.Count == 0)
				LabelMensagem.Visible = true;
		}
	}
}