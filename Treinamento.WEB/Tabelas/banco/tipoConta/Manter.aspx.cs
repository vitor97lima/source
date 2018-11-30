using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.banco.tipoConta
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
                        TipoContaBancaria lTipoContaBancaria =
                            TipoContaBancariaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                        TxtDescricao.Text = lTipoContaBancaria.Descricao;
                        TxtOperacao.Text = lTipoContaBancaria.Operacao;

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
            TxtDescricao.Enabled = TxtOperacao.Enabled = BtnSalvar.Visible = false;
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtDescricao.Text.Trim().Equals(String.Empty))
                {
                    TxtDescricao.Text = "";
                    TxtDescricao.Focus();
                    throw new CampoNaoInformadoException("Tipo de Conta Bancária", "Descrição", true);
                }

                TipoContaBancaria lTipoContaBancaria = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lTipoContaBancaria = TipoContaBancariaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Tipo de Conta Bancária alterado com sucesso.";
                }
                else
                {
                    lTipoContaBancaria = new TipoContaBancaria();
                    mensagem = "Tipo de Conta Bancária cadastrado com sucesso.";
                }
                lTipoContaBancaria.Descricao = TxtDescricao.Text.Trim();
                lTipoContaBancaria.Operacao = TxtOperacao.Text.Trim();
                TipoContaBancariaBLL.Instance.Persistir(lTipoContaBancaria);
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
                TipoContaBancaria lTipoContaBancaria = TipoContaBancariaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                TipoContaBancariaBLL.Instance.Excluir(lTipoContaBancaria);
                Web.ExibeAlerta(Page, "Tipo de Conta Bancária excluído com sucesso!", "Listar.aspx");
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