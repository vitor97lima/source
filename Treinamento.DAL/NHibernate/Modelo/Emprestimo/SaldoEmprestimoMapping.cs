using FluentNHibernate.Mapping;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DAL.NHibernate.Modelo.Emprestimo
{
    public class SaldoEmprestimoMapping : ClassMap<SaldoEmprestimo>
    {
        public SaldoEmprestimoMapping()
        {
            Schema("emp");
            Table("tbSaldoEmprestimo");
            Id(x => x.Id, "sae_int_id")
                .GeneratedBy.Foreign("Prestacao");
            Map(x => x.SaldoPrincipal, "sae_vlr_saldoPrincipal")
                .Not.Nullable();
            Map(x => x.DataReferencia, "sae_dat_dataReferencia");
            Map(x => x.Correcao, "sae_vlr_correcao");
            Map(x => x.SaldoDevedorTotal, "sae_vlr_saldoDevedorTotal");
            References(x => x.Contrato)
                .Column("fkContratoSaldoEmprestimo")
                .Cascade.None();
            HasOne(x => x.Prestacao).Constrained();

        }
    }
}
