using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.WEB.Emprestimo.baixaPrestacao
{
    public partial class Baixa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<Contrato> lContratos = ContratoBLL.Instance.Listar();
            IList<Contrato> lContratosLiberados = new List<Contrato>();
            IList<Contrato> lContratosAtivos = new List<Contrato>();

            foreach (Contrato lContrato in lContratos)
            {
                switch (lContrato.Situacao)
                {
                    case ESituacaoContrato.Liberado:
                        lContratosLiberados.Add(lContrato);
                        break;
                    case ESituacaoContrato.Ativo:
                        lContratosAtivos.Add(lContrato);
                        break;
                }
            }

            GridLiberados.DataSource = lContratosLiberados;
            GridLiberados.DataBind();

            GridAtivos.DataSource = lContratosAtivos;
            GridAtivos.DataBind();

            if (GridAtivos.Rows.Count == 0)
            {
                lblMsgAtivos.Visible = true;
            }
            if (GridLiberados.Rows.Count == 0)
            {
                lblMsgLiberados.Visible = true;
            }
        }
    }
}