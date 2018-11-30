using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class ContaBancariaMapping : ClassMap<ContaBancaria>
    {
        public ContaBancariaMapping()
        {
            Schema("glo");
            Table("tbContaBancaria");
            Id(x => x.Id, "cob_int_id")
                .GeneratedBy.Native("sqContaBancaria");
            Map(x => x.Numero, "cob_str_numero")
                .Not.Nullable()
                .Length(15);
            Map(x => x.Digito, "cob_str_digito")
                .Not.Nullable()
                .Length(1);
            References(x => x.Tipo)
                .Column("fkTipoContaBancariaContaBancaria")
                .Cascade.None();
            References(x => x.Agencia)
                .Column("fkAgenciaContaBancaria")
                .Cascade.None();
        }
    }
}
