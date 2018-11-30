<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Beneficio.eventoFolha.Manter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Evento Folha">
            <asp:Label ID="Label1" runat="server" Text="Descrição:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtDescricao" runat="server" MaxLength="100" Width="200"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Percentual:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtPercentual" runat="server" MaxLength="3" Width="67px"></asp:TextBox>
            <br />
            <asp:RadioButton ID="RbtnProvento" runat="server" Checked="True" GroupName="RbtnTipoEvento" Text="Provento" />
            <asp:RadioButton ID="RbtnDesconto" runat="server" GroupName="RbtnTipoEvento" Text="Desconto" />
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao"
                    OnClick="BtnSalvar_Click" />
                <asp:Button ID="BtnExcluir" runat="server" Text="Excluir" CssClass="botao"
                    Visible="false" OnClick="BtnExcluir_Click" />
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao"
                    OnClick="BtnCancelar_Click" />
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
