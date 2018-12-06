using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Global;
using Treinamento.BLL.Global;
using Treinamento.BLL.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.WEB.Tabelas.empregado
{
    public partial class Manter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDropDownUf();
                CarregarDropDownBanco();
                CarregarDropDownTipoConta();
                CarregarDropDownCidades();
                CarregarDropDownAgencias();
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
                        Empregado lEmpregado =
                            EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                        TxtNome.Text = lEmpregado.Nome;
                        txtDataAdmissao.Text = lEmpregado.DataAdmissao.ToString("yyyy-MM-dd");
                        TxtCPF.Text = lEmpregado.Cpf;
                        TxtSalarioBase.Text = lEmpregado.SalarioBase.ToString();

                        Endereco lEndereco = lEmpregado.Endereco;
                        TxtEndCep.Text = lEndereco.Cep.ToString();
                        TxtEndComplemento.Text = lEndereco.Complemento;
                        TxtEndLogadouro.Text = lEndereco.Logradouro;
                        TxtEndNumero.Text = lEndereco.Numero;
                        DropDownUf.SelectedValue = lEndereco.Cidade.Uf.Id.ToString();
                        DropDownCidade.SelectedValue = lEndereco.Cidade.Id.ToString();

                        ContaBancaria lContaBancaria = lEmpregado.ContaBancaria;
                        DropDownBanco.SelectedValue = lContaBancaria.Agencia.Banco.Id.ToString();
                        DropDownAgencia.SelectedValue = lContaBancaria.Agencia.Id.ToString();
                        DropDownTipoConta.SelectedValue = lContaBancaria.Tipo.Id.ToString();
                        TxtConta.Text = lContaBancaria.Numero;
                        TxtDigitoConta.Text = lContaBancaria.Digito;
                        CarregarDropDownCidades();
                        CarregarDropDownAgencias();
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
            TxtNome.Enabled
                = TxtCPF.Enabled
                = txtDataAdmissao.Enabled
                = TxtEndCep.Enabled
                = TxtEndComplemento.Enabled
                = TxtEndLogadouro.Enabled
                = TxtEndNumero.Enabled
                = TxtSalarioBase.Enabled
                = DropDownCidade.Enabled
                = DropDownUf.Enabled
                = DropDownBanco.Enabled
                = DropDownAgencia.Enabled
                = DropDownTipoConta.Enabled
                = TxtConta.Enabled
                = TxtDigitoConta.Enabled
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
                    throw new CampoNaoInformadoException("Empregado", "Nome", true);
                }
                if (TxtCPF.Text.Trim().Equals(String.Empty))
                {
                    TxtCPF.Text = "";
                    TxtCPF.Focus();
                    throw new CampoNaoInformadoException("Empregado", "CPF", true);
                }
                if (txtDataAdmissao.Text.Trim().Equals(String.Empty))
                {
                    txtDataAdmissao.Text = "";
                    txtDataAdmissao.Focus();
                    throw new CampoNaoInformadoException("Empregado", "Data de Admissão", true);
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

                Empregado lEmpregado = null;
                string mensagem = "";
                if (Request.QueryString["id"] != null)
                {
                    lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    mensagem = "Empregado alterada com sucesso.";
                }
                else
                {
                    lEmpregado = new Empregado();
                    mensagem = "Empregado cadastrada com sucesso.";
                }
                Cidade lCidade = new Cidade();
                lCidade = CidadeBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownCidade.SelectedValue));


                Endereco lEndereco = new Endereco();
                lEndereco.Logradouro = TxtEndLogadouro.Text.Trim();
                lEndereco.Numero = TxtEndNumero.Text.Trim();
                lEndereco.Complemento = TxtEndComplemento.Text.Trim();
                lEndereco.Cidade = lCidade;
                lEndereco.Cep = Convert.ToInt32(TxtEndCep.Text.Replace("-", ""));

                Banco lBanco = BancoBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownBanco.SelectedValue));
                Agencia lAgencia = AgenciaBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownAgencia.SelectedValue));
                TipoContaBancaria lTipoConta = TipoContaBancariaBLL.Instance.BuscarPorId(Convert.ToInt32(DropDownTipoConta.SelectedValue));
                ContaBancaria lContaBancaria = new ContaBancaria();
                lContaBancaria.Agencia = lAgencia;
                lContaBancaria.Tipo = lTipoConta;
                lContaBancaria.Numero = TxtConta.Text;
                lContaBancaria.Digito = TxtDigitoConta.Text;

                lEmpregado.Nome = TxtNome.Text.Trim();
                lEmpregado.Endereco = lEndereco;
                lEmpregado.Cpf = TxtCPF.Text.Trim();
                lEmpregado.DataAdmissao = DateTime.Parse(txtDataAdmissao.Text);
                lEmpregado.ContaBancaria = lContaBancaria;
                lEmpregado.SalarioBase = float.Parse(TxtSalarioBase.Text);

                EmpregadoBLL.Instance.Persistir(lEmpregado);

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
                Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                EmpregadoBLL.Instance.Excluir(lEmpregado);
                Web.ExibeAlerta(Page, "Empregado excluída com sucesso!", "Listar.aspx");
            }
            catch (Exception ex)
            {
                Web.ExibeAlerta(Page, "Não é possivel excluir! Há Registros dependentes desse funcionario");
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

        protected void DropDownBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //Carregar as Agencias no DropDown
                DropDownAgencia.Items.Clear();
                Int32 lBancoId = Convert.ToInt32(DropDownBanco.SelectedValue);
                Banco lBanco = BancoBLL.Instance.BuscarPorId(lBancoId);
                if (lBanco.Agencias != null)
                {
                    foreach (Agencia lAgencia in lBanco.Agencias)
                    {
                        DropDownAgencia.Items.Add(new ListItem(lAgencia.Codigo + " - " + lAgencia.Nome, lAgencia.Id.ToString()));
                    }
                }
            }
        }

        protected void CarregarDropDownUf()
        {
            DropDownUf.Items.Clear();
            List<UnidadeFederativa> lListaUf = UnidadeFederativaBLL.Instance.Listar();
            foreach (UnidadeFederativa lUf in lListaUf)
            {
                DropDownUf.Items.Add(new ListItem(lUf.Sigla, lUf.Id.ToString()));
            }
        }
        protected void CarregarDropDownBanco()
        {
            DropDownBanco.Items.Clear();
            List<Banco> lListaBanco = BancoBLL.Instance.Listar();
            foreach (Banco banco in lListaBanco)
            {
                DropDownBanco.Items.Add(new ListItem(banco.Codigo + " - " + banco.Nome, banco.Id.ToString()));
            }
        }
        protected void CarregarDropDownTipoConta()
        {
            DropDownTipoConta.Items.Clear();
            List<TipoContaBancaria> lListaTipoConta = TipoContaBancariaBLL.Instance.Listar();
            foreach (TipoContaBancaria lTipoConta in lListaTipoConta)
            {
                DropDownTipoConta.Items.Add(new ListItem(lTipoConta.Operacao + " - " + lTipoConta.Descricao, lTipoConta.Id.ToString()));
            }
        }
        protected void CarregarDropDownCidades()
        {
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
        protected void CarregarDropDownAgencias()
        {
            DropDownAgencia.Items.Clear();
            Int32 lBancoId = Convert.ToInt32(DropDownBanco.SelectedValue);
            Banco lBanco = BancoBLL.Instance.BuscarPorId(lBancoId);
            if (lBanco.Agencias != null)
            {
                foreach (Agencia lAgencia in lBanco.Agencias)
                {
                    DropDownAgencia.Items.Add(new ListItem(lAgencia.Codigo + " - " + lAgencia.Nome, lAgencia.Id.ToString()));
                }
            }
        }
    }
}