using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Emprestimo;
using Treinamento.BLL.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.pagamento
{
    public partial class ConfirmarPagamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Page.Response.Redirect("ContratosAtivos.aspx");
                }
                try
                {
                    Prestacao lPrestacao = PrestacaoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    if (lPrestacao.DataPagamento != null)
                        Web.ExibeAlerta(Page, "Esta prestação já foi paga!", "Prestacoes.aspx?id=" + lPrestacao.Contrato.Id);
                    lblDataVencimento.Text = lPrestacao.DataVencimento.ToString();
                    lblValorParcela.Text = lPrestacao.ValorPrestacao.ToString();
                    lblTotalPagar.Text = lPrestacao.ValorPrestacao.ToString();
                    lblDataConcessao.Text = lPrestacao.Contrato.DataConcessao.ToString(@"dd/MM/yyyy");
                }
                catch (BusinessException ex)
                {
                    Web.ExibeAlerta(Page, ex.Message);
                }
            }

        }
        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtDataPagamento.Text.Trim().Equals(String.Empty))
                {
                    TxtDataPagamento.Text = "";
                    TxtDataPagamento.Focus();
                    throw new CampoNaoInformadoException("Prestação", "Data de Pagamento", true);
                }
                else
                {
                    Prestacao lPrestacao = PrestacaoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    if (lPrestacao.DataPagamento != null)
                    {
                        Web.ExibeAlerta(Page, "Prestação´já foi paga!");
                    }
                    else if (DateTime.Parse(TxtDataPagamento.Text).Date < lPrestacao.Contrato.DataConcessao)
                    {
                        Web.ExibeAlerta(Page, "A data de pagamento não pode ser anterior a data de concessão");
                    }
                    else
                    {
                        lPrestacao.DataPagamento = DateTime.Parse(TxtDataPagamento.Text);
                        PrestacaoBLL.Instance.Persistir(lPrestacao);
                        Web.ExibeAlerta(Page, "Parcela Paga!", "Prestacoes.aspx?id=" + lPrestacao.Contrato.Id);
                        if (!(ContratoBLL.Instance.ExistePrestacaoAberta(lPrestacao.Contrato)))
                        {
                            ContratoBLL.Instance.LiquidarContrato(lPrestacao.Contrato);
                            Web.ExibeAlerta(Page, "Todas as parcelas foram pagas! Contrato Liquidado", "../contratos/Situacao.aspx");
                        }

                    }
                }
            }
            catch (BusinessException ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }
    }
}