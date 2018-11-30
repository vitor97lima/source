using FluentNHibernate.Mapping;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DAL.NHibernate.Modelo.Emprestimo
{
    public class IndiceFinanceiroValorMapping : ClassMap<IndiceFinanceiroValor>
    {
        public IndiceFinanceiroValorMapping()
        {
            Schema("emp");
            Table("tbIndiceFinanceiroValor");
            Id(x => x.Id, "ifv_int_id")
                .GeneratedBy.Native("sqIndiceFinanceiroValor");
            Map(x => x.Valor, "ifv_vlr_valor")
                .Not.Nullable();
            Map(x => x.DataReferencia, "ifv_dat_dataReferencia")
                .Not.Nullable();
            References(x => x.IndiceFinanceiro)
                .Column("fkIndiceFinanceiroIndiceFinanceiroValor");
        }
    }
}
