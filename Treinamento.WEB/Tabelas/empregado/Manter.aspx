﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Tabelas.empregado.Manter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function MascaraCPF() {
            if (mascaraInteiro(document.getElementById('<%=TxtCPF.ClientID%>')) == false) {
                event.returnValue = false;
            }
            return formataCampo(document.getElementById('<%=TxtCPF.ClientID%>'), '000.000.000-00', event);
        }
          function Moeda(z) {
            if (mascaraInteiro(z) == false) {
                event.returnValue = false;
            }
             v = z.value;
             v = v.replace(/\D/g, "")
             v = v.replace(/(\d{1})(\d{1,2})$/, "$1,$2")
             z.value = v;
         }
        function mascaraInteiro() {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.returnValue = false;
                return false;
            }
            return true;
        }
        function MascaraCep() {
            if (mascaraInteiro(document.getElementById('<%=TxtEndCep.ClientID%>')) == false) {
                event.returnValue = false;
            }
            return formataCampo(document.getElementById('<%=TxtEndCep.ClientID%>'), '00000-000', event);
        }
        //formata de forma generica os campos
        function formataCampo(campo, Mascara, evento) {
            var boleanoMascara;

            var Digitato = evento.keyCode;
            exp = /\-|\.|\/|\(|\)| /g
            campoSoNumeros = campo.value.toString().replace(exp, "");

            var posicaoCampo = 0;
            var NovoValorCampo = "";
            var TamanhoMascara = campoSoNumeros.length;;

            if (Digitato != 8) { // backspace 
                for (i = 0; i <= TamanhoMascara; i++) {
                    boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".")
                                                            || (Mascara.charAt(i) == "/"))
                    boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(")
                                                            || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
                    if (boleanoMascara) {
                        NovoValorCampo += Mascara.charAt(i);
                        TamanhoMascara++;
                    } else {
                        NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                        posicaoCampo++;
                    }
                }
                campo.value = NovoValorCampo;
                return true;
            } else {
                return true;
            }
        }
    </script>
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Empregado">
            <asp:Label ID="Label1" runat="server" Text="Nome: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtNome" runat="server" MaxLength="100" Width="200"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="CPF: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtCPF" runat="server" MaxLength="14" Width="125px" onKeyPress="MascaraCPF()"></asp:TextBox>
            <br />
            <asp:Label ID="Label11" runat="server" CssClass="label" Text="Data de Admissão: "></asp:Label>
            <br />
            <asp:TextBox ID="txtDataAdmissao" runat="server" TextMode="Date"></asp:TextBox>
            <br />
            <asp:Label ID="Label14" runat="server" CssClass="label" Text="Salário Base: "></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" CssClass="label" Text="R$"></asp:Label>
            <asp:TextBox ID="TxtSalarioBase" runat="server" MaxLength="8" onKeyPress="Moeda(this);"></asp:TextBox>
            <br />
            <asp:Panel ID="Panel1" runat="server" GroupingText="Endereço">
                <br />
                <asp:Label ID="Label5" runat="server" Text="Logadouro: " CssClass="label"></asp:Label>
                <br />
                <asp:TextBox ID="TxtEndLogadouro" runat="server" MaxLength="255" Width="200"></asp:TextBox>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Complemento: " CssClass="label"></asp:Label>
                <br />
                <asp:TextBox ID="TxtEndComplemento" runat="server" MaxLength="255" Width="200"></asp:TextBox>
                <br />
                <asp:Label ID="Label6" runat="server" Text="Numero: " CssClass="label"></asp:Label>
                <br />
                <asp:TextBox ID="TxtEndNumero" runat="server" MaxLength="10" Width="100"></asp:TextBox>
                <br />
                <asp:Label ID="Label8" runat="server" Text="CEP: " CssClass="label"></asp:Label>
                <br />
                <asp:TextBox ID="TxtEndCep" runat="server" MaxLength="9" Width="100" onKeyPress="MascaraCep();"></asp:TextBox>
                <br />
                <asp:Label ID="Label9" runat="server" Text="UF: " CssClass="label"></asp:Label>
                <br />
                <asp:DropDownList ID="DropDownUf" runat="server" OnLoad="DropDownUf_SelectedIndexChanged" OnSelectedIndexChanged="DropDownUf_SelectedIndexChanged" Style="height: 22px" AutoPostBack="True">
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label10" runat="server" Text="Cidade: " CssClass="label"></asp:Label>
                <br />
                <asp:DropDownList ID="DropDownCidade" runat="server">
                </asp:DropDownList>
                <br />
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" GroupingText="Dados Bancários">
                <br />
                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Banco: "></asp:Label>
                <br />
                <asp:DropDownList ID="DropDownBanco" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownBanco_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label4" runat="server" CssClass="label" Text="Agencia: "></asp:Label>
                <br />
                <asp:DropDownList ID="DropDownAgencia" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label12" runat="server" CssClass="label" Text="Tipo de Conta: "></asp:Label>
                <br />
                <asp:DropDownList ID="DropDownTipoConta" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label13" runat="server" CssClass="label" Text="Conta: "></asp:Label>
                <br />
                <asp:TextBox ID="TxtConta" runat="server" MaxLength="10" TextMode="Number" Width="100"></asp:TextBox>
                <b>-</b>
                <asp:TextBox ID="TxtDigitoConta" runat="server" MaxLength="1" Width="17px"></asp:TextBox>
                <br />
            </asp:Panel>
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao"
                    OnClick="BtnSalvar_Click" />
                <asp:Button ID="BtnExcluir" runat="server" Text="Excluir" CssClass="botao"
                    Visible="false" OnClick="BtnExcluir_Click" />
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao"
                    OnClick="BtnCancelar_Click" />
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
