using System;
using System.Linq;
using Treinamento.DTO.Global;
using System.Web.UI;
using System.Collections.Generic;
using Treinamento.BLL.Beneficio;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;
using System.Web.UI.WebControls;

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
            List<IndiceFinanceiro> lListaIndiceFinanceiro = IndiceFinanceiroBLL.Instance.Listar();

            if(lListaIndiceFinanceiro.Count == 0 || lListaIndiceFinanceiro == null)
            {
                Web.ExibeAlerta(Page, "Nenhum Indice Financeiro Cadastrado! Por favor, cadastre um.", "../indiceFinanceiro/Manter.aspx");
            }

            Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
            lblNomeEmpregado.Text = lEmpregado.Nome;
            DateTime lVencimento = DateTime.Today;
            lVencimento = lVencimento.AddMonths(1);
            lVencimento = new DateTime(lVencimento.Year, lVencimento.Month, DateTime.DaysInMonth(lVencimento.Year, lVencimento.Month));
            lblPrimeiroVencimento.Text = lVencimento.Date.ToShortDateString();

            foreach (IndiceFinanceiro lIndiceFinanceiro in lListaIndiceFinanceiro)
            {
                DropDownIndiceCorrecao.Items.Add(new ListItem(lIndiceFinanceiro.Codigo, lIndiceFinanceiro.Id.ToString()));
            }
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
                lContrato.IndiceCorrecao = IndiceFinanceiroBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownIndiceCorrecao.SelectedValue));
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