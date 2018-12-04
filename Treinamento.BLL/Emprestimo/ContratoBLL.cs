using System;
using System.IO;
using Treinamento.Exceptions;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.BLL.Emprestimo
{
    public class ContratoBLL : ABLL<Contrato, ContratoBLL>
    {
        public override void Validar(Contrato pContrato, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pContrato.Empregado == null)
                ex = new CampoNaoInformadoException("Contrato", "Empregado", true, ex);
            if (pContrato.DataConcessao == null)
                ex = new CampoNaoInformadoException("Contrato", "Data de Concessão", true, ex);
            //if (pContrato.IndiceCorrecao == null)
            //    ex = new CampoNaoInformadoException("Contrato", "Indice de Correcão", true, ex);
            if (pContrato.ValorEmprestimo <= 0)
                ex = new CampoNaoInformadoException("Contrato", "Valor do Empréstimo", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
        public void LiberarContrato(Contrato pContrato)
        {
            if (pContrato.Situacao == ESituacaoContrato.Solicitado)
            {
                pContrato.Prestacoes = PrestacaoBLL.Instance.GerarPestacoes(pContrato);
                pContrato.InicioAmortizacao = pContrato.Prestacoes[0].DataVencimento;
                pContrato.Situacao = ESituacaoContrato.Liberado;
                ContratoBLL.Instance.Persistir(pContrato);
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }

        public void ExportarContrato(Contrato pContrato)
        {
            if (pContrato.Situacao == ESituacaoContrato.Ativo)
            {
                pContrato.Situacao = ESituacaoContrato.Exportado;
                ContratoBLL.Instance.Persistir(pContrato);
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }

        public bool ExistePrestacaoAberta(Contrato pContrato)
        {
            bool lLiquidado = false;
            foreach (Prestacao prestacao in pContrato.Prestacoes)
            {
                if (prestacao.DataPagamento == null)
                {
                    lLiquidado = true;
                }
            }
            return lLiquidado;
        }

        public void LiquidarContrato(Contrato pContrato)
        {
            if (pContrato.Situacao == ESituacaoContrato.Ativo)
            {
                pContrato.Situacao = ESituacaoContrato.Liquidado;
                ContratoBLL.Instance.Persistir(pContrato);
            }
            else
            {
                throw new OperacaoNaoRealizadaException();
            }
        }
        public string GerarTxt(Contrato pContrato)
        {
            string lCondigo = pContrato.Codigo.ToString();
            string lDataConcessao = pContrato.DataConcessao.ToString(@"dd/MM/yyyy");
            string lNomeEmpregado = pContrato.Empregado.Nome;
            string lPrazo = pContrato.Prazo.ToString();
            string lValorEmprestimo = pContrato.ValorEmprestimo.ToString();
            string[] lLinhas = { lCondigo, lDataConcessao, lNomeEmpregado, lPrazo, lValorEmprestimo };
            string lNomeArquivo = pContrato.Id + "-" + pContrato.DataConcessao.ToString(@"dd-MM-yyyy") + lNomeEmpregado + ".txt";
            string lDiretorio = @"C:\Treinamento\source\Arquivos\Contratos\";
            try
            {
                if (!(Directory.Exists(lDiretorio)))
                    Directory.CreateDirectory(lDiretorio);
                if (!(File.Exists(lDiretorio + lNomeArquivo)))
                {
                    File.WriteAllLines(lDiretorio + lNomeArquivo, lLinhas);
                    return lDiretorio + lNomeArquivo;

                }
                else
                {
                    return lDiretorio + lNomeArquivo;
                    throw new ErrorException("Ja existe um Arquivo salvo para este documento!");
                }
            }
            catch (Exception ex)
            {
                throw new OperacaoNaoRealizadaException();
            }
        }
    }
}
