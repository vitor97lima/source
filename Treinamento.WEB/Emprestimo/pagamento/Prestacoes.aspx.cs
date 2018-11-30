using System;
using System.Web.UI;
using System.Collections.Generic;
using Treinamento.BLL.Beneficio;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.pagamento
{
    public partial class Prestacoes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    try
                    {
                        Contrato lContrato = ContratoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        if (lContrato.Prestacoes == null || lContrato.Situacao != ESituacaoContrato.Ativo)
                        {
                            throw new OperacaoNaoRealizadaException();
                        }
                        GridSolicitados.DataSource = lContrato.Prestacoes;

                        GridSolicitados.DataBind();
                    }
                    catch (BusinessException ex)
                    {
                        Web.ExibeAlerta(Page, ex.Message, "ContratosAtivos.aspx");
                    }
                }
                else
                {
                    Page.Response.Redirect("ContratosAtivos.aspx");
                }
            }
        }
    }
}