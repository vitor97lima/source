<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listar.aspx.cs" Inherits="Treinamento.WEB.Beneficio.contraCheque.Listar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="LabelMensagem" runat="server" Text="Não foram encontradas Contra cheques para este funcionário" Visible="False" CssClass="label"></asp:Label>
    <br />
    <asp:Label ID="lblEmpregadoNome" runat="server" Font-Size="Medium"></asp:Label>
    <br />
    <br />
    <asp:TextBox ID="txtDataRef" runat="server" TextMode="Month"></asp:TextBox>
    <asp:Button ID="btnGerar" runat="server" OnClick="btnGerar_Click" Text="Gerar" />
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />

        <Columns>
            <asp:BoundField DataField="Data.Year" HeaderText="Ano" />
            <asp:BoundField DataField="Data.Month" HeaderText="Mês" />
            <asp:BoundField DataField="Empregado.SalarioBase" HeaderText="Salario Base" />
            <asp:BoundField DataField="ValorLiquido" HeaderText="Salario Liquido" />
            <asp:HyperLinkField DataNavigateUrlFields="Id"
                DataNavigateUrlFormatString="~/Beneficio/contraCheque/Visualizar.aspx?id={0}&amp;acao=abrir"
                NavigateUrl="~/Beneficio/contraCheque/Visualizar.aspx" Text="Visualizar" />
        </Columns>

        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</asp:Content>
