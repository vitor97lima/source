<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GerarFolha.aspx.cs" Inherits="Treinamento.WEB.Beneficio.folha.GerarFolha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">

        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Folha de Pagamento">
            <asp:Label ID="Label1" runat="server" Text="Mês de Referência: "></asp:Label>
            <asp:TextBox ID="txtMesRef" runat="server" TextMode="Month"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Empregados:"></asp:Label>
            <br />
            <asp:CheckBoxList ID="cblEmpregados" runat="server" DataTextField="Nome" DataValueField="Id">
            </asp:CheckBoxList>

            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnGerar" runat="server" CssClass="botao" Text="Gerar" OnClick="BtnGerar_Click" />
                <asp:Button ID="BtnCancelar" runat="server" CssClass="botao" Text="Cancelar" />
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
