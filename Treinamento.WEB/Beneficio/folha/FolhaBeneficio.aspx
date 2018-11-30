<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FolhaBeneficio.aspx.cs" Inherits="Treinamento.WEB.Beneficio.folha.FolhaBeneficio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Folha Beneficio">
            <br />
            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Nome: "></asp:Label>
                <asp:Label ID="lblNomeEmpregado" runat="server" Font-Size="Medium"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="CPF: "></asp:Label>
                <asp:Label ID="lblCpfEmpregado" runat="server" Font-Size="Medium"></asp:Label>
                <br />
                <br />
                <asp:DropDownList ID="DropDownEventos" runat="server" Height="23px" Width="145px">
                </asp:DropDownList>
                <br />
                <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" OnClick="btnAdicionar_Click" />
                <asp:Button ID="btnRemover" runat="server" Text="Remover" OnClick="btnRemover_Click" />
                <br />
                <br />
                <asp:ListBox ID="ListBoxEventosEmpregado" runat="server" Height="112px" Width="134px"></asp:ListBox>
                <br />
                <br />
                <asp:Button ID="BtnSalvar" runat="server" CssClass="botao" Text="Salvar" />
                <asp:Button ID="BtnCancelar" runat="server" CssClass="botao" Text="Cancelar" />
            </asp:Panel>
            <br />
            <div id="divAreaBotao">
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
