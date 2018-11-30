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
    public partial class ContratosAtivos : System.Web.UI.Page
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
        }
    }
}