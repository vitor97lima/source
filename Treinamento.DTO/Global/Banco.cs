using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class Banco
    {
        private int _id;
        private string _codigo;
        private string _nome;
        private IList<Agencia> _agencias;

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

        public virtual IList<Agencia> Agencias
        {
            get { return _agencias; }
            set { _agencias = value; }
        }
    }
}
