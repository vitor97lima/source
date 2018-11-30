using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using Treinamento.BLL.Beneficio;
using Treinamento.BLL.Emprestimo;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Emprestimo.contratos
{
    public partial class Liberar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<Contrato> lContratos = ContratoBLL.Instance.Listar();
                IList<Contrato> lContratosSolicitados = new List<Contrato>();

                foreach (Contrato lContrato in lContratos)
                {
                    if (lContrato.Situacao == ESituacaoContrato.Solicitado)
                    {
                        lContratosSolicitados.Add(lContrato);
                    }
                }
                GridSolicitados.DataSource = lContratosSolicitados;
                GridSolicitados.DataBind();
                if (GridSolicitados.Rows.Count == 0)
                {
                    lblMsgSolicitados.Visible = true;
                }
            }
            if (Request.QueryString["acao"] != null)
            {
                if (Request.QueryString["acao"].Equals("liberar"))
                {
                    try
                    {
                        Contrato lContrato = ContratoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        ContratoBLL.Instance.LiberarContrato(lContrato);
                        string lArquivo = ContratoBLL.Instance.GerarTxt(lContrato);
                        var fi = new FileInfo(lArquivo);
                        Response.Clear();
                        Response.ContentType = "text/txt";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                        Response.WriteFile(lArquivo);
                        
                        Web.ExibeAlerta(Page, "Contrato Liberado!");
                        Response.End();
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