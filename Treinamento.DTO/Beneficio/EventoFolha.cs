using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Beneficio
{
    public class EventoFolha
    {
        private int _id;
        private string _descricao;
        private ETipoEvento _tipoEvento;
        private float _percentual;

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
        
        public virtual ETipoEvento TipoEvento
        {
            get { return _tipoEvento; }
            set { _tipoEvento = value; }
        }
        
        public virtual float Percentual
        {
            get { return _percentual; }
            set { _percentual = value; }
        }
    }
}
