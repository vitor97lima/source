using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Beneficio;

namespace Treinamento.DAL.NHibernate.Modelo.Beneficio
{
    public class EmpregadoMapping : ClassMap<Empregado>
    {
        public EmpregadoMapping()
        {
            Schema("ben");
            Table("tbEmpregado");
            Id(x => x.Id, "emp_int_id")
                .GeneratedBy.Native("sqEmpregado");
            Map(x => x.Nome, "emp_str_nome")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Cpf, "emp_str_cpf")
                .Not.Nullable()
                .Length(15);
            Map(x=>x.SalarioBase, "emp_vlw_salarioBase");
            Map(x => x.DataAdmissao, "emp_dat_dataAdmissao")
                .Not.Nullable();
            HasMany(x => x.ContraCheques)
                .KeyColumn("fkEmpregadoContraCheque")
                .Inverse()
                .LazyLoad()
                .Cascade.All();
            HasMany(x => x.Contratos)
                .KeyColumn("fkEmpregadoContrato")
                .Inverse()
                .LazyLoad()
                .Cascade.None();
            References(x => x.Endereco)
                .Column("fkEnderecoEmpregado")
                .ForeignKey("end_int_id")
                .Cascade.All();
            References(x => x.ContaBancaria)
                .Column("fkContaBancariaAgencia")
                .ForeignKey("cob_int_id")
                .Cascade.All();
        }
    }
}
