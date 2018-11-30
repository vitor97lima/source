using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class UnidadeFederativaMapping : ClassMap<UnidadeFederativa>
    {
        public UnidadeFederativaMapping()
        {
            Schema("glo");
            Table("tbUnidadeFederativa");
            Id(x => x.Id, "ufe_int_id")
                .GeneratedBy.Native("sqUnidadeFederativaId");
            Map(x => x.Nome, "ufe_str_nome")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Sigla, "ufe_str_sigla")
                .Not.Nullable()
                .Length(50);
            HasMany(x => x.Cidades)
                .KeyColumn("fkUnidadeFederativaCidade")
                .Inverse()
                .Cascade.All();
        }
    }
}
