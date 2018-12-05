using System;
using System.Collections.Generic;
using System.Web.UI;
using Treinamento.DTO;
using Treinamento.DTO.Beneficio;
using Treinamento.BLL.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Beneficio.contraCheque
{
    public partial class Listar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                IList<ContraCheque> lListaContraCheque = ContraChequeBLL.Instance.Listar(x => x.Empregado == lEmpregado, "Data");
                GridView1.DataSource = lListaContraCheque;
                GridView1.DataBind();
                lblEmpregadoNome.Text = lEmpregado.Nome;
                if (GridView1.Rows.Count == 0)
                    LabelMensagem.Visible = true;
            }
            else
            {
                Page.Response.Redirect("../folha/Listar.aspx");
            }
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    if (txtDataRef.Text != "")
                    {
                        Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        DateTime lDataReferencia = DateTime.Parse(txtDataRef.Text);
                        ContraCheque lContraCheque
                            = ContraChequeBLL.Instance.GerarContraCheque(lEmpregado, lDataReferencia);
                        lEmpregado.ContraCheques.Add(lContraCheque);
                        EmpregadoBLL.Instance.Persistir(lEmpregado);
                        GridView1.DataSource = lEmpregado.ContraCheques;
                        GridView1.DataBind();

                        Web.ExibeAlerta(Page, "Gerado Com sucesso", "Listar.aspx?id=" + lEmpregado.Id);
                    }
                    else
                    {
                        throw new CampoNaoInformadoException("Contra Cheque", "Data Referência", true);
                    }
                }
                catch (BusinessException ex)
                {
                    Web.ExibeAlerta(Page, ex.Message);
                }
            }
        }
    }
}