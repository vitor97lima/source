using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class Endereco
    {
        private int _id;
        private string _logradouro;
        private string _numero;
        private string _complemento;
        private Cidade _cidade;
        private Int32 _cep;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Logradouro
        {
            get { return _logradouro; }
            set { _logradouro = value; }
        }

        public virtual string Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        public virtual string Complemento
        {
            get { return _complemento; }
            set { _complemento = value; }
        }

        public virtual Cidade Cidade
        {
            get { return _cidade; }
            set { _cidade = value; }
        }

        public virtual Int32 Cep
        {
            get { return _cep; }
            set { _cep = value; }
        }
    }
}
