using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class UnidadeFederativa
    {
        private int _id;
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _nome;
        public virtual string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        private string _sigla;
        public virtual string Sigla
        {
            get { return _sigla; }
            set { _sigla = value.ToUpper(); }
        }
        private IList<Cidade> _cidades;
        public virtual IList<Cidade> Cidades
        {
            get { return _cidades; }
            set { _cidades = value; }
        }
    }
}
