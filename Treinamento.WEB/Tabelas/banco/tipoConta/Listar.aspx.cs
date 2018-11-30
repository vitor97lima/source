using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;

namespace Treinamento.WEB.Tabelas.banco.tipoConta
{
	public partial class Listar : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			GridView1.DataSource = TipoContaBancariaBLL.Instance.Listar();
			GridView1.DataBind();

			if (GridView1.Rows.Count == 0)
				LabelMensagem.Visible = true;
		}
	}
}