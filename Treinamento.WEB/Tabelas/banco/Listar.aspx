<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listar.aspx.cs" Inherits="Treinamento.WEB.Tabelas.banco.Listar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="LabelMensagem" runat="server" Text="Não foram encontrados Bancos cadastrados" Visible="false" CssClass="label"></asp:Label>
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
        CellPadding="4" ForeColor="#333333" GridLines="None" >
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />

        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" />
            <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
            <asp:HyperLinkField DataNavigateUrlFields="Id" 
                DataNavigateUrlFormatString="~/Tabelas/banco/Manter.aspx?id={0}&amp;acao=abrir" 
                NavigateUrl="~/Tabelas/banco/Manter.aspx" Text="Abrir" />
            <asp:HyperLinkField DataNavigateUrlFields="Id" 
                DataNavigateUrlFormatString="~/Tabelas/banco/Manter.aspx?id={0}&amp;acao=editar" 
                NavigateUrl="~/Tabelas/banco/Manter.aspx" Text="Editar" />
            <asp:HyperLinkField DataNavigateUrlFields="Id" 
                DataNavigateUrlFormatString="~/Tabelas/banco/Manter.aspx?id={0}&amp;acao=excluir" 
                NavigateUrl="~/Tabelas/banco/Manter.aspx" Text="Excluir" />
        </Columns>

        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</asp:Content>
