using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Global;

namespace Treinamento.DAL.NHibernate.Modelo.Global
{
    public class EnderecoMapping : ClassMap<Endereco>
    {
        public EnderecoMapping()
        {
            Schema("glo");
            Table("tbEndereco");
            Id(x => x.Id, "end_int_id")
                .GeneratedBy.Native("sqEnderecoId");
            Map(x => x.Logradouro, "end_str_logadouro")
                .Not.Nullable()
                .Length(255);
            Map(x => x.Numero, "end_str_num")
            .Not.Nullable()
            .Length(10);
            Map(x => x.Complemento, "end_str_complemento")
                .Length(255);
            Map(x => x.Cep, "end_int_cep")
                .Not.Nullable();
            References(x => x.Cidade)
               .Column("fkCidadeEndereco")
               .Cascade.None();
            
        }
    }
}
