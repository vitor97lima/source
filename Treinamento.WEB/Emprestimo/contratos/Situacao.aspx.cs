using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.WEB.Emprestimo.contratos
{
    public partial class Situacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<Contrato> lContratos = ContratoBLL.Instance.Listar();
            IList<Contrato> lContratosSolicitados = new List<Contrato>();
            IList<Contrato> lContratosLiberados = new List<Contrato>();
            IList<Contrato> lContratosExportados = new List<Contrato>();
            IList<Contrato> lContratosAtivos = new List<Contrato>();
            IList<Contrato> lContratosLiquidados = new List<Contrato>();

            foreach (Contrato lContrato in lContratos)
            {
                switch (lContrato.Situacao)
                {
                    case ESituacaoContrato.Solicitado:
                        lContratosSolicitados.Add(lContrato);
                        break;
                    case ESituacaoContrato.Liberado:
                        lContratosLiberados.Add(lContrato);
                        break;
                    case ESituacaoContrato.Exportado:
                        lContratosExportados.Add(lContrato);
                        break;
                    case ESituacaoContrato.Ativo:
                        lContratosAtivos.Add(lContrato);
                        break;
                    case ESituacaoContrato.Liquidado:
                        lContratosLiquidados.Add(lContrato);
                        break;
                }
            }
            GridSolicitados.DataSource = lContratosSolicitados;
            GridSolicitados.DataBind();

            GridLiquidados.DataSource = lContratosLiquidados;
            GridLiquidados.DataBind();

            GridLiberados.DataSource = lContratosLiberados;
            GridLiberados.DataBind();

            GridAtivos.DataSource = lContratosAtivos;
            GridAtivos.DataBind();

            GridExportados.DataSource = lContratosExportados;
            GridExportados.DataBind();

            if (GridSolicitados.Rows.Count == 0)
            {
                lblMsgSolicitados.Visible = true;
            }
            if (GridLiquidados.Rows.Count == 0)
            {
                lblMsgLiquidados.Visible = true;
            }
            if (GridAtivos.Rows.Count == 0)
            {
                lblMsgAtivos.Visible = true;
            }
            if (GridExportados.Rows.Count == 0)
            {
                lblMsgExportados.Visible = true;
            }
            if (GridLiberados.Rows.Count == 0)
            {
                lblMsgLiberados.Visible = true;
            }
        }
    }
}