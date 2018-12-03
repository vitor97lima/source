<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.indiceFinanceiro.Manter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Índice Financeiro">
            <asp:Label ID="Label1" runat="server" Text="Código: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtCodigo" runat="server" MaxLength="10"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Periodicidade:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtPeriodicidade" runat="server" MaxLength="1" Width="36px"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" CssClass="label" Text="Data Referência:"></asp:Label>
            <br />
            <asp:TextBox ID="TxtDataReferencia" runat="server" MaxLength="1" TextMode="Month"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" CssClass="label" Text="Valor:"></asp:Label>
            <br />
            <asp:TextBox ID="TxtValor" runat="server" MaxLength="5" ></asp:TextBox>
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao" 
                    onclick="BtnSalvar_Click"/>
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao" 
                    onclick="BtnCancelar_Click"/>
            </div>
            <br />
        </asp:Panel> 
    </div>
</asp:Content>
