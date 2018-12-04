using System;
using System.Collections.Generic;
using Treinamento.DTO.Emprestimo;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Emprestimo
{
    public class SaldoEmprestimoBLL : ABLL<SaldoEmprestimo, SaldoEmprestimoBLL>
    {
        public override void Validar(SaldoEmprestimo pSaldoEmprestimo, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pSaldoEmprestimo.Contrato == null)
            {
                throw new CampoNaoInformadoException("Saldo Emprestimo", "Contrato", true, ex);
            }
            if (pSaldoEmprestimo.Prestacao == null)
            {
                throw new CampoNaoInformadoException("Saldo Emprestimo", "Prestação", true, ex);
            }

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }

        public SaldoEmprestimo GerarSaldo(Prestacao pPrestacao)
        {
            if (pPrestacao.Id < 1)
            {
                throw new OperacaoNaoRealizadaException();
            }
            if (pPrestacao.Contrato == null)
            {
                throw new CampoNaoInformadoException("Saldo", "Contrato", true);
            }

            SaldoEmprestimo lSaldo = new SaldoEmprestimo();
            lSaldo.Contrato = pPrestacao.Contrato;
            lSaldo.Correcao = pPrestacao.ValorCorrecao;
            lSaldo.DataReferencia = pPrestacao.DataVencimento.Value;
            lSaldo.Prestacao = pPrestacao;

            Contrato lContrato = pPrestacao.Contrato;
            foreach (Prestacao lPrestacao in lContrato.Prestacoes)
            {
                if (lPrestacao.NumeroPrestacao <= pPrestacao.NumeroPrestacao)
                {
                    lSaldo.SaldoPrincipal += lPrestacao.ValorPrestacao;
                }
                if (lPrestacao.DataPagamento == null)
                {
                    lSaldo.SaldoDevedorTotal += lPrestacao.ValorPrestacao;
                }
            }
            lSaldo.SaldoDevedorTotal -= lSaldo.Prestacao.ValorPrestacao;
            return lSaldo;
        }

    }
}
