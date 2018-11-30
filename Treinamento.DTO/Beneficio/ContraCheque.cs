using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.DTO.Beneficio
{
    public class ContraCheque
    {
        private int _id;
        private DateTime _data;
        private IDictionary<EventoFolha, float> _eventos;
        private Empregado _empregado;
        private double _valorLiquido;
        private double _salarioBase;

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public virtual IDictionary<EventoFolha, float> Eventos
        {
            get { return _eventos; }
            set { _eventos = value; }
        }

        public virtual Empregado Empregado
        {
            get { return _empregado; }
            set { _empregado = value; }
        }

        public virtual double ValorLiquido
        {
            get { return _valorLiquido; }
            set { _valorLiquido = value; }
        }
        public virtual double SalarioBase
        {
            get { return _salarioBase; }
            set { _salarioBase = value; }
        }
    }
}
