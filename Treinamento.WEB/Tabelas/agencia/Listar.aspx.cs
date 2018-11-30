﻿using System;
using Treinamento.BLL.Global;
using Treinamento.DTO.Global;

namespace Treinamento.WEB.Tabelas.agencia
{
    public partial class Listar : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			GridView1.DataSource = AgenciaBLL.Instance.Listar();
			GridView1.DataBind();
			if (GridView1.Rows.Count == 0)
				LabelMensagem.Visible = true;
		}
	}
}