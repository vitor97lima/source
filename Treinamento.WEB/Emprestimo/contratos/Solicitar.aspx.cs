using System;
using System.Web.UI;
using Treinamento.BLL.Beneficio;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.contratos
{
    public partial class Solicitar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                Page.Response.Redirect("Empregados.aspx");
            }
            Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
            lblNomeEmpregado.Text = lEmpregado.Nome;
            DateTime lVencimento = DateTime.Today;
            lVencimento = lVencimento.AddMonths(1);
            lVencimento = new DateTime(lVencimento.Year, lVencimento.Month, DateTime.DaysInMonth(lVencimento.Year, lVencimento.Month));
            lblPrimeiroVencimento.Text = lVencimento.Date.ToShortDateString();
        }
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void BtnSolicitar_Click(object sender, EventArgs e)
        {
            try
            {
                Contrato lContrato = new Contrato();
                lContrato.Codigo = Convert.ToInt32(TxtCodigo.Text);
                lContrato.DataConcessao = DateTime.Today;
                lContrato.Empregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                lContrato.Prazo = Convert.ToInt32(TxtPrazo.Text);
                lContrato.Situacao = ESituacaoContrato.Solicitado;
                lContrato.ValorEmprestimo = float.Parse(TxtValorEmprestimo.Text);

                lContrato.ValorPrestacao = lContrato.ValorEmprestimo / lContrato.Prazo;

                ContratoBLL.Instance.Persistir(lContrato);
                Web.ExibeAlerta(Page, "Solicitado com Sucesso!", "Situacao.aspx");
            }
            catch (BusinessException ex)
            {
                Web.ExibeAlerta(Page, ex.Message);
            }
        }

    }
}