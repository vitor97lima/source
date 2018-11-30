using FluentNHibernate.Mapping;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DAL.NHibernate.Modelo.Emprestimo
{
    public class IndiceFinanceiroMapping : ClassMap<IndiceFinanceiro>
    {
        public IndiceFinanceiroMapping()
        {
            Schema("emp");
            Table("tbIndiceFinanceiro");
            Id(x => x.Id, "idf_int_id")
                .GeneratedBy.Native("sqIndiceFinanceiro");
            Map(x=>x.Codigo, "idf_str_codigo")
                .Not.Nullable();
            Map(x => x.Periodicidade, "idf_chr_periodicidade")
                .Not.Nullable();
            HasMany(x=>x.Valores)
                .KeyColumn("fkIndiceFinanceiroIndiceFinanceiroValor")
                .Cascade.All();
        }
    }
}
