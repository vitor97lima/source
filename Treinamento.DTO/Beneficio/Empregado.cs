using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DTO.Beneficio
{
    public class Empregado
    {
        private int _id;
        private string _nome;
        private string _cpf;
        private IList<ContraCheque> _contraCheques;
        private float _salarioBase;
        private Endereco _endereco;
        private ContaBancaria _contaBancaria;
        private DateTime _dataAdmissao;
        private IList<Contrato> _contratos;

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
        
        public virtual string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }
        
        public virtual IList<ContraCheque> ContraCheques
        {
            get { return _contraCheques; }
            set { _contraCheques = value; }
        }
        
        public virtual float SalarioBase
        {
            get { return _salarioBase; }
            set { _salarioBase = value; }
        }

        public virtual Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value; }
        }

        public virtual ContaBancaria ContaBancaria
        {
            get { return _contaBancaria; }
            set { _contaBancaria = value; }
        }
        
        public virtual DateTime DataAdmissao
        {
            get { return _dataAdmissao; }
            set { _dataAdmissao = value; }
        }

        public virtual IList<Contrato> Contratos
        {
            get { return _contratos; }
            set { _contratos = value; }
        }
    }
}
