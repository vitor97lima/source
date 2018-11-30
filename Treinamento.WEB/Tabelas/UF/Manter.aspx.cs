using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.uf
{
    public partial class Manter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                        UnidadeFederativa lUf = 
                            UnidadeFederativaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                        TxtNome.Text = lUf.Nome;
                        TxtSigla.Text = lUf.Sigla;
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
            TxtNome.Enabled = TxtSigla.Enabled = BtnSalvar.Visible = false;
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtNome.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Unidade Federativa", "Nome", true);
                }
                if (TxtSigla.Text.Trim().Equals(String.Empty))
                {
                    TxtSigla.Text = "";
                    TxtSigla.Focus();
                    throw new CampoNaoInformadoException("Unidade Federativa", "Sigla", true);
                }
                
                UnidadeFederativa lUf = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
					lUf = UnidadeFederativaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Unidade Federativa alterada com sucesso.";
                }
                else
                {
                    lUf = new UnidadeFederativa();
                    mensagem = "Unidade Federativa cadastrada com sucesso.";
                }

                lUf.Nome = TxtNome.Text.Trim();
                lUf.Sigla = TxtSigla.Text.Trim();
				UnidadeFederativaBLL.Instance.Persistir(lUf);
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
				UnidadeFederativa lUf = UnidadeFederativaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
				UnidadeFederativaBLL.Instance.Excluir(lUf);
                Web.ExibeAlerta(Page, "Unidade Federativa excluída com sucesso!", "Listar.aspx");
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