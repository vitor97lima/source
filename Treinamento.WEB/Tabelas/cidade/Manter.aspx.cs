using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.cidade
{
    public partial class Manter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Carregar as UF no DropDown

            
            List<UnidadeFederativa> lListaUf = UnidadeFederativaBLL.Instance.Listar();

            foreach (UnidadeFederativa lUf in lListaUf)
            {
                DropDownUf.Items.Add(new ListItem(lUf.Sigla, lUf.Id.ToString()));
                

            }
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    switch (Request.QueryString["acao"])
                    {
                        case "abrir":
                            BloquearComponentes();
                            break;
                        case "editar":
                            break;
                        case "excluir":
                            BloquearComponentes();
                            BtnExcluir.Visible = true;
                            break;
                    }

                    try
                    {
                        Cidade lCidade =
                            CidadeBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                        TxtNome.Text = lCidade.Nome;
                        DropDownUf.SelectedValue = lCidade.Uf.Id.ToString() ;

                    }
                    catch (BusinessException ex)
                    {
                        Web.ExibeAlerta(Page, ex.Message);
                    }
                }
            }
        }

        private void BloquearComponentes()
        {
            TxtNome.Enabled = DropDownUf.Enabled = BtnSalvar.Visible = false;
        }

        // FALTA IMPLEMENTAR
        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtNome.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Cidade", "Nome", true);
                }

                Cidade lCidade = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lCidade = CidadeBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Cidade alterada com sucesso.";
                }
                else
                {
                    lCidade = new Cidade();
                    mensagem = "Cidade cadastrada com sucesso.";
                }
                UnidadeFederativa lUf = new UnidadeFederativa();
                lUf = UnidadeFederativaBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownUf.SelectedValue));
                lCidade.Nome = TxtNome.Text.Trim();
                lCidade.Uf = lUf;
                CidadeBLL.Instance.Persistir(lCidade);
                Web.ExibeAlerta(Page, mensagem, "Listar.aspx");
            }
            catch (BusinessException ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }

        protected void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                Cidade lCidade = CidadeBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                CidadeBLL.Instance.Excluir(lCidade);
                Web.ExibeAlerta(Page, "Cidade excluída com sucesso!", "Listar.aspx");
            }
            catch (BusinessException ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Listar.aspx");
        }
    }
}