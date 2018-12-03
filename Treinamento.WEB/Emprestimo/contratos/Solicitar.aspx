<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Solicitar.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.contratos.Solicitar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .rightColumn {
            margin-left: 464px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function txtOnKeyPress() {
            var lPrazo = Number(document.getElementById('<%=TxtPrazo.ClientID%>').value);
            var lValorEmprestimo = Number(document.getElementById('<%=TxtValorEmprestimo.ClientID%>').value);
            if (lValorEmprestimo > 0 && lPrazo > 0)
                document.getElementById('<%=lblValorPrestacao.ClientID%>').innerHTML = (lValorEmprestimo / lPrazo);
            else
                document.getElementById('<%=lblValorPrestacao.ClientID%>').innerHTML = 0;
        }

    </script>
    <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Solicitar Empréstimo">
        <asp:Label ID="Label1" runat="server" Text="Nome: " CssClass="label"></asp:Label>
        <asp:Label ID="lblNomeEmpregado" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label14" runat="server" CssClass="label" Text="Valor do Emprestimo: "></asp:Label>
        <br />
        <asp:Label ID="Label15" runat="server" CssClass="label" Text="R$"></asp:Label>
        <asp:TextBox ID="TxtValorEmprestimo" runat="server" MaxLength="14" TextMode="Number" Width="60px" onkeyup="txtOnKeyPress();"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" runat="server" CssClass="label" Text="Prazo (meses): "></asp:Label><br />
        <asp:TextBox ID="TxtPrazo" runat="server" MaxLength="14" TextMode="Number" Width="60px" onkeyup="txtOnKeyPress();"></asp:TextBox>
        <br />
        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Codigo:"></asp:Label><br />
        <asp:TextBox ID="TxtCodigo" runat="server" MaxLength="14" TextMode="Number" Width="60px"></asp:TextBox>
        <br />
        <asp:Label ID="Label18" runat="server" CssClass="label" Text="Índice de Correção"></asp:Label>
        <br />
        <asp:DropDownList ID="DropDownIndiceCorrecao" runat="server">
        </asp:DropDownList>
        <br />

        <br />
        <asp:Label ID="Label16" runat="server" CssClass="label" Text="Primeiro Vencimento: "></asp:Label>
        <asp:Label ID="lblPrimeiroVencimento" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label17" runat="server" CssClass="label" Text="Valor da Prestação: R$"></asp:Label>
        <asp:Label ID="lblValorPrestacao" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <div id="divAreaBotao">
            <asp:Button ID="BtnSolicitar" runat="server" Text="Solicitar" CssClass="botao" OnClick="BtnSolicitar_Click" />
            <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao"
                OnClick="BtnCancelar_Click" />
        </div>
        <br />
    </asp:Panel>
</asp:Content>
