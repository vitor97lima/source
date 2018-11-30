using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.BLL.Beneficio;

namespace Treinamento.WEB.Beneficio.contraCheque
{
    public partial class Visualizar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ContraCheque lContraCheque = ContraChequeBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                GridView1.DataSource = lContraCheque.Eventos;
                GridView1.DataBind();
                lblEmpregadoNome.Text = lContraCheque.Empregado.Nome;
                lblSalarioBase.Text = lContraCheque.Empregado.SalarioBase.ToString();
                lblSalarioLiquido.Text = lContraCheque.ValorLiquido.ToString();
                if (GridView1.Rows.Count == 0)
                    LabelMensagem.Visible = true;
            }
            else
            {
                Page.Response.Redirect("../folha/Listar.aspx");
            }
        }

        protected void BtnGerarTxt_Click(object sender, EventArgs e)
        {
            try
            {
                string lNomeArquivo = ContraChequeBLL.Instance.GerarTxt(ContraChequeBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"])));
                Web.ExibeAlerta(Page, "Salvo com sucesso!");
                var lArquivo = new FileInfo(lNomeArquivo);
                Response.Clear();
                Response.ContentType = "text/txt";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + lArquivo.Name);
                Response.WriteFile(lNomeArquivo);
                Response.End();
            }
            catch (Exception ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }
    }
}