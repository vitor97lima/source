using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.BLL.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Beneficio.eventoFolha
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
                        EventoFolha lEventoFolha = EventoFolhaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        TxtDescricao.Text = lEventoFolha.Descricao;
                        TxtPercentual.Text = lEventoFolha.Percentual.ToString();
                        if (lEventoFolha.TipoEvento.Equals(ETipoEvento.Desconto))
                        {
                            RbtnDesconto.Checked = true;
                        }
                        else
                        {
                            RbtnProvento.Checked = true;
                        }
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
            TxtDescricao.Enabled
                = TxtPercentual.Enabled
                = RbtnDesconto.Enabled
                = RbtnProvento.Enabled
                = BtnSalvar.Visible = false;
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtDescricao.Text.Trim().Equals(String.Empty))
                {
                    TxtDescricao.Text = "";
                    TxtDescricao.Focus();
                    throw new CampoNaoInformadoException("Evento Folha", "Descrição", true);
                }
                if (TxtPercentual.Text.Trim().Equals(String.Empty))
                {
                    TxtPercentual.Text = "";
                    TxtPercentual.Focus();
                    throw new CampoNaoInformadoException("Evento Folha", "Descrição", true);
                }
                EventoFolha lEventoFolha = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lEventoFolha = EventoFolhaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Evento Folha alterado com sucesso.";
                }
                else
                {
                    lEventoFolha = new EventoFolha();
                    mensagem = "Evento Folha cadastrado com sucesso.";
                }
                lEventoFolha.Descricao = TxtDescricao.Text;
                lEventoFolha.Percentual = float.Parse(TxtPercentual.Text);
                if (RbtnDesconto.Checked)
                {
                    lEventoFolha.TipoEvento = ETipoEvento.Desconto;
                }
                else
                {
                    lEventoFolha.TipoEvento = ETipoEvento.Provento;
                }
                EventoFolhaBLL.Instance.Persistir(lEventoFolha);
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
                EventoFolha lEventoFolha = EventoFolhaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                EventoFolhaBLL.Instance.Excluir(lEventoFolha);
                Web.ExibeAlerta(Page, "Evento Folha excluído com sucesso!", "Listar.aspx");
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