<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prestacoes.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.pagamento.Prestacoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Contratos Ativos">

        <asp:Panel ID="Panel1" runat="server" GroupingText="Contratos">
            <asp:Label ID="lblMsgSolicitados" runat="server" Text="Não foi encontrado nenhum contrato nesta situação" Visible="False" CssClass="label"></asp:Label>
            <br />
            <asp:GridView ID="GridSolicitados" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
                CellPadding="4" ForeColor="#333333" GridLines="None">
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <Columns>
                    <asp:BoundField DataField="NumeroPrestacao" HeaderText="" />
                    <asp:BoundField DataField="ValorPrestacao" HeaderText="Valor da Prestação" />
                    <asp:BoundField DataField="DataVencimento" HeaderText="Vencimento" />
                    <asp:BoundField DataField="DataPagamento" HeaderText="Pagamento" />
                    <asp:HyperLinkField  DataNavigateUrlFields="Id"
                        DataNavigateUrlFormatString="~/Emprestimo/pagamento/ConfirmarPagamento.aspx?id={0}&amp;acao=abrir"
                        NavigateUrl="~/Emprestimo/pagamento/ConfirmarPagamento.aspx" Text="Confirmar Pagamento" />
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
