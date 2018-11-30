using System;
using System.Web.UI;
using System.Collections.Generic;
using Treinamento.BLL.Beneficio;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.contratos
{
    public partial class Exportar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<Contrato> lContratos = ContratoBLL.Instance.Listar();
                IList<Contrato> lContratosAtivos = new List<Contrato>();

                foreach (Contrato lContrato in lContratos)
                {
                    if (lContrato.Situacao == ESituacaoContrato.Ativo)
                    {
                        lContratosAtivos.Add(lContrato);
                    }
                }
                GridSolicitados.DataSource = lContratosAtivos;
                GridSolicitados.DataBind();
                if (GridSolicitados.Rows.Count == 0)
                {
                    lblMsgSolicitados.Visible = true;
                }
            }
            if (Request.QueryString["acao"] != null)
            {
                if (Request.QueryString["acao"].Equals("exportar"))
                {
                    try
                    {
                        Contrato lContrato = ContratoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        ContratoBLL.Instance.ExportarContrato(lContrato);
                        Web.ExibeAlerta(Page, "Cadastro Exportado!", "Exportar.aspx");
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