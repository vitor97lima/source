using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class AgenciaMapping : ClassMap<Agencia>
    {
        public AgenciaMapping()
        {
            Schema("glo");
            Table("tbAgencia");
            Id(x => x.Id, "age_int_id")
                .GeneratedBy.Native("sqAgenciaId");
            Map(x => x.Nome, "age_str_nome")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Codigo, "age_str_codigo")
            .Not.Nullable()
            .Length(10);
            Map(x => x.Digito, "age_str_digito")
                .Not.Nullable()
                .Length(1);
            References(x => x.Banco)
                .Column("fkBancoAgencia")
                .Cascade.None();
            References(x => x.Endereco)
                .Column("fkEnderecoAgencia")
                .ForeignKey("end_int_id")
                .Cascade.All();
        }
    }
}
