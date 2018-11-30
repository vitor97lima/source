<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visualizar.aspx.cs" Inherits="Treinamento.WEB.Beneficio.contraCheque.Visualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="LabelMensagem" runat="server" Text="Não foram encontradas Contra cheques para este funcionário" Visible="False" CssClass="label"></asp:Label>
    <br />
    <asp:Label ID="lblEmpregadoNome" runat="server" Font-Size="Medium"></asp:Label>
    <br />
    <br />
    Salário Base:
    <asp:Label ID="lblSalarioBase" runat="server" Font-Size="Medium"></asp:Label>
    <br />
    Salário Líquido:
    <asp:Label ID="lblSalarioLiquido" runat="server" Font-Size="Medium"></asp:Label>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="Key.Descricao" HeaderText="Evento" />
            <asp:BoundField DataField="Key.TipoEvento" HeaderText="Tipo" />
            <asp:BoundField DataField="Value" HeaderText="Valor" />
        </Columns>


        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <br />
    <div id="divAreaBotao">
        <asp:Button ID="BtnGerarTxt" runat="server" Text="Salvar em TXT" CssClass="botao" OnClick="BtnGerarTxt_Click"/>
    </div>
</asp:Content>
