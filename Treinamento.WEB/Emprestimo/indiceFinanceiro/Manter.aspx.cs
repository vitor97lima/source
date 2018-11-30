using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Emprestimo;
using Treinamento.BLL.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.indiceFinanceiro
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
                        IndiceFinanceiro lIndiceFinanceiro =
                             IndiceFinanceiroBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        IndiceFinanceiroValor lIndiceFinanceiroValor = null;
                        TxtCodigo.Text = lIndiceFinanceiro.Codigo;
                        TxtPeriodicidade.Text = lIndiceFinanceiro.Periodicidade.ToString();

                        DateTime lDataRefMaisRecente = DateTime.MinValue;
                        lIndiceFinanceiroValor = lIndiceFinanceiro.ValorMaisRecente;
                        TxtValor.Text = lIndiceFinanceiroValor.Valor.ToString();
                        TxtDataReferencia.Text = lIndiceFinanceiroValor.DataReferencia.ToShortDateString();
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
            TxtCodigo.Enabled
                = TxtPeriodicidade.Enabled
                = BtnSalvar.Visible = false;
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtCodigo.Text.Trim().Equals(String.Empty))
                {
                    TxtCodigo.Text = "";
                    TxtCodigo.Focus();
                    throw new CampoNaoInformadoException("Índice Financeiro", "Código", true);
                }
                if (TxtPeriodicidade.Text.Trim().Equals(String.Empty))
                {
                    TxtPeriodicidade.Text = "";
                    TxtPeriodicidade.Focus();
                    throw new CampoNaoInformadoException("Índice Financeiro", "Periodicidade", true);
                }
                if (TxtValor.Text.Trim().Equals(String.Empty))
                {
                    TxtPeriodicidade.Text = "";
                    TxtPeriodicidade.Focus();
                    throw new CampoNaoInformadoException("Índice Financeiro", "Valor", true);
                }
                if (TxtDataReferencia.Text.Trim().Equals(String.Empty))
                {
                    TxtPeriodicidade.Text = "";
                    TxtPeriodicidade.Focus();
                    throw new CampoNaoInformadoException("Índice Financeiro", "Data de Referência", true);
                }

                IndiceFinanceiro lIndiceFinanceiro = null;
                IndiceFinanceiroValor lIndiceFinanceiroValor = new IndiceFinanceiroValor();
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lIndiceFinanceiro = IndiceFinanceiroBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Índice financeiro alterado com sucesso.";
                }
                else
                {
                    lIndiceFinanceiro = new IndiceFinanceiro();
                    mensagem = "Índice financeiro cadastrado com sucesso.";
                }

                lIndiceFinanceiro.Codigo = TxtCodigo.Text.Trim();
                lIndiceFinanceiro.Periodicidade = TxtPeriodicidade.Text.ToCharArray()[0];
                lIndiceFinanceiroValor.DataReferencia = DateTime.Parse(TxtDataReferencia.Text);
                lIndiceFinanceiroValor.Valor = float.Parse(TxtValor.Text);
                if (lIndiceFinanceiro.Valores == null)
                    lIndiceFinanceiro.Valores = new List<IndiceFinanceiroValor>();
                lIndiceFinanceiro.Valores.Add(lIndiceFinanceiroValor);
                IndiceFinanceiroBLL.Instance.Persistir(lIndiceFinanceiro);
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
                IndiceFinanceiro lIndiceFinanceiro = IndiceFinanceiroBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                IndiceFinanceiroBLL.Instance.Excluir(lIndiceFinanceiro);
                Web.ExibeAlerta(Page, "Índice Financeiro excluído com sucesso!", "Listar.aspx");
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