using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class CidadeMapping : ClassMap<Cidade>
    {
        public CidadeMapping()
        {
            Schema("glo");
            Table("tbCidade");
            Id(x => x.Id, "cid_int_id")
                .GeneratedBy.Native("sqCidadeId");
            Map(x => x.Nome, "cid_str_nome")
                .Not.Nullable()
                .Length(255);
            References(x => x.Uf)
                .Column("fkUnidadeFederativaCidade")
                .Cascade.None();
        }
    }
}
