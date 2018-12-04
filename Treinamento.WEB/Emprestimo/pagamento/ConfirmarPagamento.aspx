<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmarPagamento.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.pagamento.ConfirmarPagamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Baixa de Pestação">
            <asp:Label ID="Label5" runat="server" CssClass="label" Text="Data da Concessão: "></asp:Label>
            <br />
            <asp:Label ID="lblDataConcessao" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" CssClass="label" Text="Data de Pagamento: "></asp:Label>
            <br />
            <asp:TextBox ID="TxtDataPagamento" runat="server" MaxLength="100" TextMode="Date"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Data de Vencimento: "></asp:Label>
            <br />
            <asp:Label ID="lblDataVencimento" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" CssClass="label" Text="Valor da Parcela"></asp:Label>
            :
            <br />
            <asp:Label ID="lblValorParcela" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" CssClass="label" Text="Valor da Correção"></asp:Label>
            :
            <br />
            <asp:Label ID="lblValorCorrecao" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" CssClass="label" Text="Total a pagar"></asp:Label>
            :
            <br />
            <asp:Label ID="lblTotalPagar" runat="server"></asp:Label>
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao" 
                    onclick="BtnSalvar_Click"/>
            </div>
            <br />
        </asp:Panel> 
    </div>
</asp:Content>
