using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class ContaBancaria
    {
        private int _id;
        private string _numero;
        private string _digito;
        private Agencia _agencia;
        private TipoContaBancaria _tipo;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        public virtual string Digito
        {
            get { return _digito; }
            set { _digito = value; }
        }

        public virtual Agencia Agencia
        {
            get { return _agencia; }
            set { _agencia = value; }
        }

        public virtual TipoContaBancaria Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
    }
}
