using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Emprestimo
{
    public class Prestacao
    {
        private int _id;
        private DateTime? _dataVencimento;
        private float _valorPrincipal;
        private float _valorPrestacao;
        private float _valorCorrecao;
        private int _numeroPrestacao;
        private DateTime? _dataPagamento;
        private Contrato _contrato;
        private SaldoEmprestimo _saldo;


        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        

        public virtual DateTime? DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; }
        }
        

        public virtual float ValorPrincipal
        {
            get { return _valorPrincipal; }
            set { _valorPrincipal = value; }
        }
        

        public virtual float ValorCorrecao
        {
            get { return _valorCorrecao; }
            set { _valorCorrecao = value; }
        }
        
        public virtual float ValorPrestacao
        {
            get { return _valorPrestacao; }
            set { _valorPrestacao = value; }
        }
        

        public virtual int NumeroPrestacao
        {
            get { return _numeroPrestacao; }
            set { _numeroPrestacao = value; }
        }
        

        public virtual DateTime? DataPagamento
        {
            get { return _dataPagamento; }
            set { _dataPagamento = value; }
        }
        

        public virtual Contrato Contrato
        {
            get { return _contrato; }
            set { _contrato = value; }
        }

        

        public virtual SaldoEmprestimo Saldo
        {
            get { return _saldo; }
            set { _saldo = value; }
        }
    }
}
