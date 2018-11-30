using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.Exceptions;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.BLL.Emprestimo
{
    public class IndiceFinanceiroValorBLL : ABLL<IndiceFinanceiroValor, IndiceFinanceiroValorBLL>
    {
        public override void Validar(IndiceFinanceiroValor pIndiceFinanceiroValor, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pIndiceFinanceiroValor.DataReferencia == null)
                ex = new CampoNaoInformadoException("Índice Financeiro Valor", "Data de Referência", true, ex);
            if (pIndiceFinanceiroValor.IndiceFinanceiro == null)
                ex = new CampoNaoInformadoException("Índice Financeiro Valor", "Índice Financeiro", true, ex);
            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
