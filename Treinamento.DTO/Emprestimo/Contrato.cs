using System;
using System.Collections.Generic;
using Treinamento.DTO.Beneficio;

namespace Treinamento.DTO.Emprestimo
{
    public class Contrato
    {
        private int _id;
        private Empregado _empregado;
        private DateTime _dataConcessao;
        private float _valorEmprestimo;
        private int _prazo;
        private int _codigo;
        private IList<Prestacao> _prestacoes;
        private ESituacaoContrato _situacao;
        private float _valorPrestacao;
        private IndiceFinanceiro _indiceCorrecao;
        private DateTime? _inicioAmortizacao;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public virtual Empregado Empregado
        {
            get { return _empregado; }
            set { _empregado = value; }
        }


        public virtual DateTime DataConcessao
        {
            get { return _dataConcessao; }
            set { _dataConcessao = value; }
        }


        public virtual float ValorEmprestimo
        {
            get { return _valorEmprestimo; }
            set { _valorEmprestimo = value; }
        }


        public virtual int Prazo
        {
            get { return _prazo; }
            set { _prazo = Math.Abs(value); }
        }


        public virtual DateTime? InicioAmortizacao
        {
            get { return _inicioAmortizacao; }
            set { _inicioAmortizacao = value; }
        }

        public virtual IndiceFinanceiro IndiceCorrecao
        {
            get { return _indiceCorrecao; }
            set { _indiceCorrecao = value; }
        }
        public virtual float ValorPrestacao
        {
            get { return _valorPrestacao; }
            set { _valorPrestacao = value; }
        }


        public virtual ESituacaoContrato Situacao
        {
            get { return _situacao; }
            set { _situacao = value; }
        }


        public virtual IList<Prestacao> Prestacoes
        {
            get { return _prestacoes; }
            set { _prestacoes = value; }
        }


        public virtual int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
    }
}
