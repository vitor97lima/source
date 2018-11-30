<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Tabelas.banco.Manter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Banco">
            <asp:Label ID="Label1" runat="server" Text="Nome: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtNome" runat="server" MaxLength="100" Width="200"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Codigo:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtCodigo" runat="server" MaxLength="10" Width="200"></asp:TextBox>
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao" 
                    onclick="BtnSalvar_Click"/>
                <asp:Button ID="BtnExcluir" runat="server" Text="Excluir" CssClass="botao" 
                    Visible="false" onclick="BtnExcluir_Click"/>
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao" 
                    onclick="BtnCancelar_Click"/>
            </div>
            <br />
        </asp:Panel> 
    </div>
</asp:Content>
