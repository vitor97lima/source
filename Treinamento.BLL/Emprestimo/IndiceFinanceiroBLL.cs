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
            if (pIndiceFinanceiro.Periodicidade.ToString() == "")
                ex = new CampoNaoInformadoException("Índice Financeiro", "Periodicidade", true, ex);
            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
        public IndiceFinanceiroValor BuscarIndicePorVencimentoPrestacao(IndiceFinanceiro pIndiceFinanceiro, Prestacao pPrestacao)
        {
            if (pIndiceFinanceiro.Id < 1)
                throw new OperacaoNaoRealizadaException();
            if (pPrestacao.DataVencimento == null)
                throw new OperacaoNaoRealizadaException();

            IndiceFinanceiroValor lIndiceValorReferencia = pIndiceFinanceiro.ValorMaisAntigo;
            foreach (IndiceFinanceiroValor lIndiceValor in pIndiceFinanceiro.Valores)
            {
                if (lIndiceValor.DataReferencia < pPrestacao.DataVencimento)
                    if (lIndiceValorReferencia.DataReferencia < lIndiceValor.DataReferencia)
                        lIndiceValorReferencia = lIndiceValor;
            }
            return lIndiceValorReferencia;
        }
        public IndiceFinanceiroValor BuscarValorIndiceRegente(IndiceFinanceiro pIndiceFinanceiro)
        {
            if (pIndiceFinanceiro.Id < 1)
                throw new OperacaoNaoRealizadaException();

            IndiceFinanceiroValor lIndiceValorReferencia = pIndiceFinanceiro.ValorMaisAntigo;
            foreach (IndiceFinanceiroValor lIndiceValor in pIndiceFinanceiro.Valores)
            {
                if (lIndiceValor.DataReferencia < DateTime.Today)
                    if (lIndiceValorReferencia.DataReferencia < lIndiceValor.DataReferencia)
                        lIndiceValorReferencia = lIndiceValor;
            }
            return lIndiceValorReferencia;
        }
    }
}
