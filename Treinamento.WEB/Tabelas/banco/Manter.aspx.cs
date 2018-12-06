using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.banco
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
                        Banco lBanco =
                            BancoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                        TxtNome.Text = lBanco.Nome;
                        TxtCodigo.Text = lBanco.Codigo;

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
            TxtNome.Enabled = TxtCodigo.Enabled = BtnSalvar.Visible = false;
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtNome.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Banco", "Nome", true);
                }

                Banco lBanco = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lBanco = BancoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Banco alterada com sucesso.";
                }
                else
                {
                    lBanco = new Banco();
                    mensagem = "Banco cadastrado com sucesso.";
                }
                lBanco.Nome = TxtNome.Text.Trim();
                lBanco.Codigo = TxtCodigo.Text.Trim();
                BancoBLL.Instance.Persistir(lBanco);
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
                Banco lBanco = BancoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                BancoBLL.Instance.Excluir(lBanco);
                Web.ExibeAlerta(Page, "Banco excluído com sucesso!", "Listar.aspx");
            }
            catch (Exception ex)
            {
                Web.ExibeAlerta(Page, "Não é possivel excluir!");
            }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Listar.aspx");
        }
    }
}