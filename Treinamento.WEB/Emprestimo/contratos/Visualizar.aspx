<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visualizar.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.contratos.Visualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .rightColumn {
            margin-left: 464px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Visualizar Contrato">
        <asp:Label ID="Label1" runat="server" Text="Nome: " CssClass="label"></asp:Label>
        <asp:Label ID="lblNomeEmpregado" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label14" runat="server" CssClass="label" Text="Valor do Emprestimo: "></asp:Label>
        <br />
        <asp:Label ID="Label15" runat="server" CssClass="label" Text="R$"></asp:Label>
        <asp:TextBox ID="TxtValorEmprestimo" runat="server" MaxLength="14" TextMode="Number" Width="60px" onkeyup="txtOnKeyPress();" Enabled="False"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" runat="server" CssClass="label" Text="Prazo (meses): "></asp:Label><br />
        <asp:TextBox ID="TxtPrazo" runat="server" MaxLength="14" TextMode="Number" Width="60px" onkeyup="txtOnKeyPress();" Enabled="False"></asp:TextBox>
        <br />
        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Codigo:"></asp:Label><br />
        <asp:TextBox ID="TxtCodigo" runat="server" MaxLength="14" TextMode="Number" Width="60px" Enabled="False"></asp:TextBox>
        <br />
        <br />

        <asp:Label ID="Label18" runat="server" CssClass="label" Text="Situação:  "></asp:Label>
        <asp:Label ID="lblSituacao" runat="server" CssClass="label"></asp:Label>
        <br />

        <br />
        <asp:Label ID="Label4" runat="server" CssClass="label" Text="Valor da Prestação: R$"></asp:Label>
        <asp:Label ID="lblValorPrestacao" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label16" runat="server" CssClass="label" Text="Quantidade de Prestações: "></asp:Label>
        <asp:Label ID="lblQntPrestacao" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label17" runat="server" CssClass="label" Text="Prestações em aberto: "></asp:Label>
        <asp:Label ID="lblPrestacoesAbertas" runat="server" CssClass="label"></asp:Label>
        <br />
        <br />
    </asp:Panel>
</asp:Content>
