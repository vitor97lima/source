using System;
using System.Web.UI;
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
                GridView1.DataSource = lEmpregado.ContraCheques;
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
                    Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    ContraCheque lContraCheque 
                        =  ContraChequeBLL.Instance.GerarContraCheque(lEmpregado, DateTime.Parse(txtDataRef.Text));
                    lEmpregado.ContraCheques.Add(lContraCheque);
                    EmpregadoBLL.Instance.Persistir(lEmpregado);
                    GridView1.DataSource = lEmpregado.ContraCheques ;
                    GridView1.DataBind();

                    Web.ExibeAlerta(Page, "Gerado Com sucesso", "Listar.aspx?id=" + lEmpregado.Id);

                }
                catch (BusinessException ex)
                {
                    Web.ExibeAlerta(Page, ex.Message);
                }

            }
        }
    }
}