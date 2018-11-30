using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class BancoMapping: ClassMap<Banco>
    {
        public BancoMapping()
        {
            Schema("glo");
            Table("tbBanco");
            Id(x => x.Id, "ban_int_id")
                .GeneratedBy.Native("sqBancoId");
            Map(x => x.Nome, "ban_str_nome")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Codigo, "ban_str_codigo")
                .Not.Nullable()
                .Length(10);
            HasMany(x => x.Agencias)
                .KeyColumn("fkBancoAgencia")
                .Inverse()
                .Cascade.All();
        }
    }
}
