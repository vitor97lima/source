using FluentNHibernate.Mapping;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DAL.NHibernate.Modelo.Emprestimo
{
    public class PrestacaoMapping : ClassMap<Prestacao>
    {
        public PrestacaoMapping()
        {
            Schema("emp");
            Table("tbPrestacao");
            Id(x => x.Id, "prs_int_id")
                .GeneratedBy.Native("sqPrestacao");
            Map(x => x.DataVencimento, "prs_dat_dataVencimento")
                .Not.Nullable();
            Map(x => x.ValorPrincipal, "prs_vlr_valorPrincipal")
                .Not.Nullable();
            Map(x => x.ValorPrestacao, "prs_vlr_valorPrestacao")
                .Not.Nullable();
            Map(x => x.ValorCorrecao, "prs_vlr_valorCorrecao");
            Map(x => x.NumeroPrestacao, "prs_int_numeroPrestacao")
                .Not.Nullable();
            Map(x => x.DataPagamento, "prs_dat_dataPagamento");
            References(x => x.Contrato)
                .Column("fkContratoPrestacao")
                .Cascade.None();
            References(x => x.Saldo, "prs_vlr_saldo")
                .Column("fkSaldoEmprestimoPrestacao")
                .Cascade.None();
        }
    }
}
