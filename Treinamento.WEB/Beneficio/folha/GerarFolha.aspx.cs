using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.BLL.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Beneficio.folha
{
    public partial class GerarFolha : System.Web.UI.Page
    {
        private IList<ListItem> _selecionados;
        protected void Page_Load(object sender, EventArgs e)
        {
            cblEmpregados.DataSource = EmpregadoBLL.Instance.Listar();
            cblEmpregados.DataBind();
            _selecionados = new List<ListItem>();
        }

        protected void BtnGerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMesRef.Text.Trim().Equals(String.Empty))
                {
                    txtMesRef.Text = "";
                    txtMesRef.Focus();
                    throw new CampoNaoInformadoException("Gerar Folha", "Mês de Referência", true);
                }

                IList<Empregado> lSelecionados = new List<Empregado>();
                foreach (ListItem item in cblEmpregados.Items)
                {
                    if (!(item.Selected))
                    {
                        _selecionados.Add(item);
                    }
                }
                foreach (ListItem item in _selecionados)
                {
                    lSelecionados.Add(EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(item.Value)));
                }
                Session["MesReferencia"] = DateTime.Parse(txtMesRef.Text);
                Session["EmpregadosSelecionados"] = lSelecionados;
                Page.Response.Redirect("Listar.aspx");
            }
            catch (BusinessException ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }
    }
}