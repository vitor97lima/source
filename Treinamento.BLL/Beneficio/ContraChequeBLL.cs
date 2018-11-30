using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Beneficio
{
    public class ContraChequeBLL : ABLL<ContraCheque, ContraChequeBLL>
    {
        public override void Validar(ContraCheque pContraCheque, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pContraCheque.Data == null)
                ex = new CampoNaoInformadoException("Contra Cheque", "Data", true, ex);
            if (pContraCheque.Empregado == null)
                ex = new CampoNaoInformadoException("Contra Cheque", "Empregado", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
        private double CalcularIR(ContraCheque pContraCheque)
        {
            Empregado lEmpregado = pContraCheque.Empregado;
            double lSalarioBase = lEmpregado.SalarioBase;
            double lIR = 0;

            if (lSalarioBase < 1500)
                lIR = (lSalarioBase * 0.1);
            else if (lSalarioBase >= 1500 && lSalarioBase < 2500)
            {
                lIR = (1500 * 0.1) + ((lSalarioBase - 1500) * 0.15);
            }
            else
            {
                lIR = (1500 * 0.1);
                if ((lSalarioBase - 1500) > 2500)
                {
                    lIR += 2500 * 0.15;
                    lIR += ((lSalarioBase - 2500) * 0.2);

                }
                else
                {
                    lIR += (lSalarioBase - 1500) * 0.15;
                }
            }
            return lIR;
        }

        public void CalcularFolhaEvento(ContraCheque pContraCheque)
        {
            Empregado lEmpregado = pContraCheque.Empregado;
            double lSalarioBase = lEmpregado.SalarioBase;
            double lValorLiquido = lSalarioBase;
            IList<EventoFolha> lListaEventos = EventoFolhaBLL.Instance.Listar();
            Dictionary<EventoFolha, float> lEventos = new Dictionary<EventoFolha, float>();
            foreach (EventoFolha lEventoFolha in lListaEventos)
            {
                if (lEventoFolha.Descricao != "Triênio")
                    lEventos.Add(lEventoFolha, 0);
                else if (((pContraCheque.Data - lEmpregado.DataAdmissao).TotalDays) > 1095)
                {
                    lEventos.Add(lEventoFolha, 0);
                }
            }

            EventoFolha lEF = new EventoFolha();
            lEF.Descricao = "IR";
            lEF.Percentual = 0;
            lEF.Id = 6;
            lEF.TipoEvento = ETipoEvento.Desconto;
            lEventos.Add(lEF, 0);

            for (int index = 0; index < lEventos.Count; index++)
            {
                EventoFolha lEventoFolha = lEventos.ElementAt(index).Key;
                if (lEventoFolha.Descricao.ToUpper().Trim().Equals("IR"))
                {
                    lEventos[lEventoFolha] = (float)-CalcularIR(pContraCheque);
                }
                else if (lEventoFolha.TipoEvento == ETipoEvento.Desconto)
                {
                    lEventos[lEventoFolha] = (float)-((lSalarioBase * lEventoFolha.Percentual) / 100);

                }
                else if (lEventoFolha.TipoEvento == ETipoEvento.Provento)
                {
                    lEventos[lEventoFolha] = (float)((lSalarioBase * lEventoFolha.Percentual) / 100);
                }
            }
            pContraCheque.Eventos = lEventos;
        }

        public void CalcularValorLiquido(ContraCheque pContraCheque)
        {
            float lTotalEventoFolha = 0;
            foreach (float lValor in pContraCheque.Eventos.Values)
            {
                lTotalEventoFolha += lValor;
            }
            pContraCheque.ValorLiquido = pContraCheque.Empregado.SalarioBase + lTotalEventoFolha;
        }

        public string GerarTxt(ContraCheque pContraCheque)
        {

            Empregado lEmpregado = pContraCheque.Empregado;
            string lBanco = lEmpregado.ContaBancaria.Agencia.Banco.Codigo;
            string lAgencia = lEmpregado.ContaBancaria.Agencia.Codigo;
            string lConta = lEmpregado.ContaBancaria.Numero + "-" + lEmpregado.ContaBancaria.Digito;
            string lCpf = lEmpregado.Cpf;
            string lNome = lEmpregado.Nome;
            string lValor = pContraCheque.ValorLiquido.ToString();
            string lTipo = "P";
            string[] lLinhas = { lBanco, lAgencia, lConta, lCpf, lNome, lValor, lTipo };
            string lNomeArquivo = pContraCheque.Id + "-" + pContraCheque.Data.ToString(@"dd-MM-yyyy") + lEmpregado.Nome + ".txt";
            string lDiretorio = @"C:\Treinamento\source\Arquivos\ContraCheque\";

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
