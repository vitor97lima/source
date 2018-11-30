using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class Cidade
    {
        private int _id;
        private string _nome;
        private UnidadeFederativa _uf;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public virtual UnidadeFederativa Uf
        {
            get { return _uf; }
            set { _uf = value; }
        }
    }
}
