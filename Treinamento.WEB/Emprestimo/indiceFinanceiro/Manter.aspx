<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manter.aspx.cs" Inherits="Treinamento.WEB.Emprestimo.indiceFinanceiro.Manter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function mascaraInteiro() {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.returnValue = false;
                return false;
            }
        }
        function valor(z) {

            if (mascaraInteiro(z) == false) {
                return false;
            }

            if (z.value > 100) {
                z.value = 100;
                return z;
            }
        }
    </script>
    <div id="divFormulario">
        <asp:Panel ID="PanelManterUF" runat="server" GroupingText="Manutenção de Índice Financeiro">
            <asp:Label ID="Label1" runat="server" Text="Código: " CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtCodigo" runat="server" MaxLength="10"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Periodicidade:" CssClass="label"></asp:Label>
            <br />
            <asp:TextBox ID="TxtPeriodicidade" runat="server" MaxLength="1" Width="36px"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" CssClass="label" Text="Data Referência:"></asp:Label>
            <br />
            <asp:TextBox ID="TxtDataReferencia" runat="server" MaxLength="1" TextMode="Month"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" CssClass="label" Text="Valor (%):"></asp:Label>
            <br />
            <asp:TextBox ID="TxtValor" runat="server" MaxLength="3" Width="32px" onKeyPress="valor(this);"></asp:TextBox>
            <br />
            <br />
            <asp:Panel ID="PanelValores" runat="server" GroupingText="Valores do Índice" Visible="false">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridviewListagem"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                        <asp:BoundField DataField="DataReferencia.Date" HeaderText="Data de Referência" />
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <br />
            <br />
            <div id="divAreaBotao">
                <asp:Button ID="BtnSalvar" runat="server" Text="Salvar" CssClass="botao"
                    OnClick="BtnSalvar_Click" />
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="botao"
                    OnClick="BtnCancelar_Click" />
            </div>
            <br />

        </asp:Panel>
    </div>
</asp:Content>
