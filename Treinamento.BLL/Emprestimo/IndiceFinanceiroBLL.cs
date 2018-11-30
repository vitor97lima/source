using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.Exceptions;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.BLL.Emprestimo
{
    public class IndiceFinanceiroBLL : ABLL<IndiceFinanceiro, IndiceFinanceiroBLL>
    {
        public override void Validar(IndiceFinanceiro pIndiceFinanceiro, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pIndiceFinanceiro.Codigo == null)
                ex = new CampoNaoInformadoException("Índice Financeiro", "Código", true, ex);
            if(pIndiceFinanceiro.Periodicidade.ToString() == "")
                ex = new CampoNaoInformadoException("Índice Financeiro", "Periodicidade", true, ex);
            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
