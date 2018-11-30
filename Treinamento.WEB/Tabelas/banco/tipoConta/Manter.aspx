<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Tabelas.banco.tipoConta.Manter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Tipo de Conta Bancária">
            <asp:Label ID="Label1" runat="server" Text="Descrição: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtDescricao" runat="server" MaxLength="100" Width="200"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Operação:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtOperacao" runat="server" MaxLength="3" Width="62px"></asp:TextBox>
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
