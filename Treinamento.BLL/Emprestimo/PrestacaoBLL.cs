using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.Exceptions;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.BLL.Emprestimo
{
    public class PrestacaoBLL : ABLL<Prestacao, PrestacaoBLL>
    {
        public override void Validar(Prestacao pPrestacao, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pPrestacao.Contrato == null)
            {
                throw new CampoNaoInformadoException("Prestação", "Contrato", true, ex);
            }
            if (pPrestacao.DataVencimento == null)
            {
                throw new CampoNaoInformadoException("Prestação", "Data Vencimento", true, ex);
            }

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }

        public void CorrigirPrestacoes(Contrato pContrato)
        {
            if (pContrato.Prestacoes != null && pContrato.IndiceCorrecao != null)
            {
                IList<Prestacao> lPrestacoes = pContrato.Prestacoes;
                foreach (Prestacao lPrestacao in lPrestacoes)
                {
                    float lValorPrestacaoAnterior = lPrestacao.ValorPrestacao;
                    lPrestacao.ValorPrestacao = (float)(lPrestacao.ValorPrestacao * Math.Pow((1 + pContrato.IndiceCorrecao.ValorMaisRecente.Valor), (lPrestacao.NumeroPrestacao)));
                    lPrestacao.ValorCorrecao = lValorPrestacaoAnterior - lValorPrestacaoAnterior;
                }
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }
        public void CorrigirPrestacao(Prestacao pPrestacao)
        {
            if (pPrestacao != null && pPrestacao.Contrato.IndiceCorrecao != null)
            {
                float lValorPrestacaoAnterior = pPrestacao.ValorPrestacao;
                pPrestacao.ValorPrestacao = (float)(pPrestacao.ValorPrestacao * Math.Pow((1 + pPrestacao.Contrato.IndiceCorrecao.ValorMaisRecente.Valor), (pPrestacao.NumeroPrestacao)));
                pPrestacao.ValorCorrecao = lValorPrestacaoAnterior - lValorPrestacaoAnterior;
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }
        public IList<Prestacao> GerarPestacoes(Contrato pContrato)
        {
            if (pContrato != null)
            {
                int lQtdPrestacoes = pContrato.Prazo;
                if (lQtdPrestacoes < 1)
                {
                    lQtdPrestacoes = 1;
                }
                IList<Prestacao> lListaPrestacoes = new List<Prestacao>();
                int lMesConcessao = pContrato.DataConcessao.Month;
                int lAnoConcessao = pContrato.DataConcessao.Year;
                DateTime lVencimento = new DateTime(lAnoConcessao, lMesConcessao, 1);
                lVencimento = lVencimento.AddMonths(1);
                lVencimento = lVencimento.AddDays(-1);
                float lValorEmprestimo = pContrato.ValorEmprestimo;
                float lValorPrestacao = lValorEmprestimo / lQtdPrestacoes;


                for (int i = 0; i < lQtdPrestacoes; i++)
                {
                    Prestacao lPrestacao = new Prestacao();
                    lPrestacao.ValorPrincipal = lValorEmprestimo;
                    lPrestacao.ValorPrestacao = lValorPrestacao;
                    lPrestacao.NumeroPrestacao = i + 1;
                    lPrestacao.Contrato = pContrato;
                    lVencimento = lVencimento.AddMonths(1);
                    lVencimento = new DateTime(lVencimento.Year, lVencimento.Month, DateTime.DaysInMonth(lVencimento.Year, lVencimento.Month));
                    lPrestacao.DataVencimento = lVencimento;
                    lListaPrestacoes.Add(lPrestacao);
                }
                return lListaPrestacoes;
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }

        public IList<Prestacao> GerarPestacoes(Contrato pContrato, uint pQtdPrestacoes)
        {
            if (pContrato != null)
            {
                if (pQtdPrestacoes < 1)
                {
                    pQtdPrestacoes = 1;
                }
                IList<Prestacao> lListaPrestacoes = new List<Prestacao>();
                int lMesConcessao = pContrato.DataConcessao.Month;
                int lAnoConcessao = pContrato.DataConcessao.Year;
                DateTime lVencimento = new DateTime(lAnoConcessao, lMesConcessao, 1);
                lVencimento.AddMonths(2).AddDays(-1);
                float lValorEmprestimo = pContrato.ValorEmprestimo;
                float lValorPrestacao = lValorEmprestimo / pQtdPrestacoes;

                for (int i = 0; i < pQtdPrestacoes; i++)
                {
                    Prestacao lPrestacao = new Prestacao();
                    lPrestacao.ValorPrincipal = lValorEmprestimo;
                    lPrestacao.ValorPrestacao = lValorPrestacao;
                    lPrestacao.NumeroPrestacao = i + 1;
                    lPrestacao.Contrato = pContrato;
                    lVencimento.AddMonths(i);
                    lVencimento = new DateTime(lVencimento.Year, lVencimento.Month, DateTime.DaysInMonth(lVencimento.Year, lVencimento.Month));
                    lPrestacao.DataVencimento = lVencimento;
                    lListaPrestacoes.Add(lPrestacao);
                }
                return lListaPrestacoes;
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }
    }
}
