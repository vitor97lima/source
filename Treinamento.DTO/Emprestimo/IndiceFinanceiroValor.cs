using System;

namespace Treinamento.DTO.Emprestimo
{
    public class IndiceFinanceiroValor
    {
        private int _id;
        private IndiceFinanceiro _indiceFinanceiro;
        private float _valor;
        private DateTime _dataReferencia;
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual IndiceFinanceiro IndiceFinanceiro
        {
            get { return _indiceFinanceiro; }
            set { _indiceFinanceiro = value; }
        }

        public virtual float Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        public virtual DateTime DataReferencia
        {
            get { return _dataReferencia; }
            set { _dataReferencia = value; }
        }
    }
}
