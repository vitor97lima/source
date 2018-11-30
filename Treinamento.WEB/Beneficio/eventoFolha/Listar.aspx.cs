using System;
using Treinamento.BLL.Beneficio;

namespace Treinamento.WEB.Beneficio.eventoFolha
{
    public partial class Listar : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			GridView1.DataSource = EventoFolhaBLL.Instance.Listar();
			GridView1.DataBind();

			if (GridView1.Rows.Count == 0)
				LabelMensagem.Visible = true;
		}
	}
}