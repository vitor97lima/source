using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Emprestimo
{
    public class SaldoEmprestimo
    {
        private int _id;
        private float _saldoPrincipal;
        private Contrato _contrato;
        private DateTime _dataReferencia;
        private Prestacao _prestacao;
        private float _correcao;
        private float _saldoDevedorTotal;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual float SaldoPrincipal
        {
            get { return _saldoPrincipal; }
            set { _saldoPrincipal = value; }
        }

        public virtual Contrato Contrato
        {
            get { return _contrato; }
            set { _contrato = value; }
        }

        public virtual DateTime DataReferencia
        {
            get { return _dataReferencia; }
            set { _dataReferencia = value; }
        }


        public virtual Prestacao Prestacao
        {
            get { return _prestacao; }
            set { _prestacao = value; }
        }


        public virtual float Correcao
        {
            get { return _correcao; }
            set { _correcao = value; }
        }



        public virtual float SaldoDevedorTotal
        {
            get { return _saldoDevedorTotal; }
            set { _saldoDevedorTotal = value; }
        }
    }
}
