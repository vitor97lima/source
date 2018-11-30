using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class TipoContaBancariaMapping : ClassMap<TipoContaBancaria>
    {
        public TipoContaBancariaMapping()
        {
            Schema("glo");
            Table("tbTipoContaBancaria");
            Id(x => x.Id, "tcb_int_id")
                .GeneratedBy.Native("sqTipoContaBancaria");
            Map(x => x.Descricao, "tcp_str_descricao")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Operacao, "tcp_str_operacao")
                .Not.Nullable()
                .Length(10);
            HasMany(x => x.ContasBancaria)
                .KeyColumn("fkTipoContaBancariaContaBancaria")
                .Inverse()
                .Cascade.All();
        }
    }
}
