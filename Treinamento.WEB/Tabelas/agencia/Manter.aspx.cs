using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.agencia
{
    public partial class Manter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Carregar as UF no DropDown
            if (!IsPostBack)
            {
                List<UnidadeFederativa> lListaUf = UnidadeFederativaBLL.Instance.Listar();

                foreach (UnidadeFederativa lUf in lListaUf)
                {
                    DropDownUf.Items.Add(new ListItem(lUf.Sigla, lUf.Id.ToString()));


                }
                Int32 lUfID = Convert.ToInt32(DropDownUf.SelectedValue);
                UnidadeFederativa lUF = UnidadeFederativaBLL.Instance.BuscarPorId(lUfID);
                if (lUF.Cidades != null)
                {
                    foreach (Cidade cidade in lUF.Cidades)
                    {
                        DropDownCidade.Items.Add(new ListItem(cidade.Nome, cidade.Id.ToString()));
                    }
                }
                // CARREGA OS BANCOS
                List<Banco> lListaBanco = BancoBLL.Instance.Listar();

                foreach (Banco lBanco in lListaBanco)
                {
                    DropDownBanco.Items.Add(new ListItem(lBanco.Nome, lBanco.Id.ToString()));


                }

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
                        Agencia lAgencia =
                            AgenciaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));

                        TxtNome.Text = lAgencia.Nome;
                        TxtCodigo.Text = lAgencia.Codigo;
                        TxtDigito.Text = lAgencia.Digito;
                        DropDownBanco.SelectedValue = lAgencia.Banco.Id.ToString();

                        TxtEndLogadouro.Text = lAgencia.Endereco.Logradouro;
                        TxtEndNumero.Text = lAgencia.Endereco.Numero;
                        TxtEndComplemento.Text = lAgencia.Endereco.Complemento;
                        TxtEndCep.Text = lAgencia.Endereco.Cep.ToString();
                        DropDownCidade.SelectedValue = lAgencia.Endereco.Cidade.Id.ToString();
                        DropDownUf.SelectedValue = lAgencia.Endereco.Cidade.Uf.Id.ToString();

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
            TxtNome.Enabled = DropDownBanco.Enabled
                = TxtCodigo.Enabled
                = TxtDigito.Enabled
                = TxtEndCep.Enabled
                = TxtEndComplemento.Enabled
                = TxtEndLogadouro.Enabled
                = TxtEndNumero.Enabled
                = DropDownCidade.Enabled
                = DropDownUf.Enabled
                = BtnSalvar.Visible = false;
        }
        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtNome.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Agência", "Nome", true);
                }
                if (TxtCodigo.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Agência", "Código", true);
                }
                if (TxtDigito.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Agência", "Dígito", true);
                }
                if (TxtEndLogadouro.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Endereço", "Logadouro", true);
                }
                if (TxtEndNumero.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Endereço", "Numero", true);
                }
                if (TxtEndCep.Text.Trim().Equals(String.Empty))
                {
                    TxtNome.Text = "";
                    TxtNome.Focus();
                    throw new CampoNaoInformadoException("Endereço", "CEP", true);
                }

                Agencia lAgencia = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lAgencia = AgenciaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Agência alterada com sucesso.";
                }
                else
                {
                    lAgencia = new Agencia();
                    mensagem = "Agência cadastrada com sucesso.";
                }
                Cidade lCidade = new Cidade();
                lCidade = CidadeBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownCidade.SelectedValue));

                Banco lBanco = new Banco();
                lBanco = BancoBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownBanco.SelectedValue));

                Endereco lEndereco = new Endereco();
                lEndereco.Logradouro = TxtEndLogadouro.Text.Trim();
                lEndereco.Numero = TxtEndNumero.Text.Trim();
                lEndereco.Complemento = TxtEndComplemento.Text.Trim();
                lEndereco.Cidade = lCidade;
                lEndereco.Cep = Convert.ToInt32(TxtEndCep.Text);


                lAgencia.Nome = TxtNome.Text.Trim();
                lAgencia.Codigo = TxtCodigo.Text.Trim();
                lAgencia.Digito = TxtDigito.Text.Trim();
                lAgencia.Banco = lBanco;
                lAgencia.Endereco = lEndereco;
                AgenciaBLL.Instance.Persistir(lAgencia);
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
                Agencia lAgencia = AgenciaBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                AgenciaBLL.Instance.Excluir(lAgencia);
                Web.ExibeAlerta(Page, "Agência excluída com sucesso!", "Listar.aspx");
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

        protected void DropDownUf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //Carregar as Cidades no DropDown
                DropDownCidade.Items.Clear();
                Int32 lUfID = Convert.ToInt32(DropDownUf.SelectedValue);
                UnidadeFederativa lUF = UnidadeFederativaBLL.Instance.BuscarPorId(lUfID);
                if (lUF.Cidades != null)
                {
                    foreach (Cidade cidade in lUF.Cidades)
                    {
                        DropDownCidade.Items.Add(new ListItem(cidade.Nome, cidade.Id.ToString()));
                    }
                }
            }
        }
    }
}