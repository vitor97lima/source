using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Treinamento.DTO.Beneficio;
using Treinamento.BLL.Beneficio;

namespace Treinamento.WEB.Beneficio.folha
{
    public partial class FolhaBeneficio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Carregar as UF no DropDown
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    Empregado lEmpregado = EmpregadoBLL.Instance.BuscarPorId(Convert.ToInt32(Request.QueryString["id"]));
                    lblNomeEmpregado.Text = lEmpregado.Nome;
                    lblCpfEmpregado.Text = lEmpregado.Cpf;
                    List<EventoFolha> lListaEnventoFolha = EventoFolhaBLL.Instance.Listar();
                    foreach (EventoFolha lEventoFolha in lListaEnventoFolha)
                    {
                        DropDownEventos.Items.Add(new ListItem(lEventoFolha.Descricao, lEventoFolha.Id.ToString()));
                    }
                }
                else
                {
                    Page.Response.Redirect("Listar.aspx");
                }
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {

            ListBoxEventosEmpregado.Items.Add( new ListItem(DropDownEventos.SelectedItem.Text, DropDownEventos.SelectedItem.Value));
        }

        protected void btnRemover_Click(object sender, EventArgs e)
        {
            ListBoxEventosEmpregado.Items.Remove(ListBoxEventosEmpregado.SelectedItem);
        }
    }
}