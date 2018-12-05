using System;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.contratos.pagamento
{
    public partial class PagamentoConcessao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<Contrato> lContratos = ContratoBLL.Instance.Listar(Ordem<Contrato>.ASC(x => x.DataConcessao), Ordem<Contrato>.ASC(x => x.Empregado.Nome));
                IList<Contrato> lContratosLiberados = new List<Contrato>();

                foreach (Contrato lContrato in lContratos)
                {
                    if (lContrato.Situacao == ESituacaoContrato.Liberado)
                    {
                        lContratosLiberados.Add(lContrato);
                    }
                }
                GridSolicitados.DataSource = lContratosLiberados;
                GridSolicitados.DataBind();
                if (GridSolicitados.Rows.Count == 0)
                {
                    lblMsgSolicitados.Visible = true;
                }
            }
            if (Request.QueryString["acao"] != null)
            {
                if (Request.QueryString["acao"].Equals("confirmar"))
                {
                    try
                    {
                        Contrato lContrato = ContratoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        if (lContrato.Situacao == ESituacaoContrato.Liberado)
                        {
                            lContrato.Situacao = ESituacaoContrato.Ativo;
                            ContratoBLL.Instance.Persistir(lContrato);
                            Web.ExibeAlerta(Page, "Pagamento Confirmado. Contrato Ativo!", "PagamentoConcessao.aspx");
                        }
                        else
                        {
                            throw new OperacaoNaoRealizadaException();
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
}