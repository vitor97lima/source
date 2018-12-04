using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Emprestimo
{
    public class IndiceFinanceiro
    {
        private int _id;
        private string _codigo;
        private char _periodicidade;
        private IList<IndiceFinanceiroValor> _valores;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public virtual char Periodicidade
        {
            get { return _periodicidade; }
            set { _periodicidade = value; }
        }
        public virtual IList<IndiceFinanceiroValor> Valores
        {
            get { return _valores; }
            set { _valores = value; }
        }
        public virtual IndiceFinanceiroValor ValorMaisRecente
        {
            get
            {
                IndiceFinanceiroValor lIndiceFinanceiroValor = null;
                if (_valores != null)
                {
                    DateTime lDataRefMaisRecente = DateTime.MinValue;
                    foreach (IndiceFinanceiroValor lValor in this.Valores)
                    {
                        if (lValor.DataReferencia > lDataRefMaisRecente)
                        {
                            lDataRefMaisRecente = lValor.DataReferencia;
                            lIndiceFinanceiroValor = lValor;
                        }
                    }
                }
                return lIndiceFinanceiroValor;
            }
        }
        public virtual IndiceFinanceiroValor ValorMaisAntigo
        {
            get
            {
                IndiceFinanceiroValor lIndiceFinanceiroValor = null;
                if (_valores != null)
                {
                    DateTime lDataRefMaisAntigo = DateTime.MaxValue;
                    foreach (IndiceFinanceiroValor lValor in this.Valores)
                    {
                        if (lValor.DataReferencia < lDataRefMaisAntigo)
                        {
                            lDataRefMaisAntigo = lValor.DataReferencia;
                            lIndiceFinanceiroValor = lValor;
                        }
                    }
                }
                return lIndiceFinanceiroValor;
            }
        }
    }
}
