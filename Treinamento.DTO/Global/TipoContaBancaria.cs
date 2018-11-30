using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Global
{
    public class TipoContaBancaria
    {
        private int _id;
        private string _descricao;
        private string _operacao;
        private IList<ContaBancaria> _contasBancaria;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public virtual string Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }
        
        public virtual string Operacao
        {
            get { return _operacao; }
            set { _operacao = value; }
        }
        public virtual IList<ContaBancaria> ContasBancaria
        {
            get { return _contasBancaria; }
            set { _contasBancaria = value; }
        }
    }
}
