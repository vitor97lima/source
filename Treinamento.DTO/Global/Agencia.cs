using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class Agencia
    {
        private int _id;
        private string _codigo;
        private string _nome;
        private string _digito;
        private Banco _banco;
        private Endereco _endereco;
        private IList<ContaBancaria> _contasBancaria;
        
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

        public virtual string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public virtual string Digito
        {
            get { return _digito; }
            set { _digito = value; }
        }

        public virtual Banco Banco
        {
            get { return _banco; }
            set { _banco = value; }
        }

        public virtual Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value; }
        }

        public virtual IList<ContaBancaria> ContasBancaria
        {
            get { return _contasBancaria; }
            set { _contasBancaria = value; }
        }
    }
}
