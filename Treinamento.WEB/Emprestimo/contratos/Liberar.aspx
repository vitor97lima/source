<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Liberar.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.contratos.Liberar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Liberar de Contratos">

        <asp:Panel ID="Panel1" runat="server" GroupingText="Solicitados">
            <asp:Label ID="lblMsgSolicitados" runat="server" Text="Não foi encontrado nenhum contrato nesta situação" Visible="False" CssClass="label"></asp:Label>
            <br />
            <asp:GridView ID="GridSolicitados" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
                CellPadding="4" ForeColor="#333333" GridLines="None">
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                    <asp:BoundField DataField="Empregado.Nome" HeaderText="NomeEmpregado" />
                    <asp:BoundField DataField="ValorEmprestimo" HeaderText="ValorEmprestimo" />
                     <asp:BoundField DataField="DataConcessao" HeaderText="Data de Concessão" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id"
                        DataNavigateUrlFormatString="~/Emprestimo/contratos/Visualizar.aspx?id={0}"
                        NavigateUrl="~/Emprestimo/contratos/Liberar.aspx" Text="Abrir" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id"
                        DataNavigateUrlFormatString="~/Emprestimo/contratos/Liberar.aspx?id={0}&amp;acao=liberar"
                        NavigateUrl="~/Emprestimo/contratos/Liberar.aspx" Text="Liberar" />
                </Columns>
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
