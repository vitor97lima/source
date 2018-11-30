using FluentNHibernate.Mapping;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.DAL.NHibernate.Modelo.Emprestimo
{
    public class ContratoMapping : ClassMap<Contrato>
    {
        public ContratoMapping()
        {
            Schema("emp");
            Table("tbContrato");
            Id(x => x.Id, "con_int_id")
                .GeneratedBy.Native("sqContrato");
            Map(x => x.ValorEmprestimo, "con_vlr_valorEmprestimo")
                .Not.Nullable();
            Map(x => x.DataConcessao, "con_dat_dataConcessao")
                .Not.Nullable();
            Map(x => x.Prazo, "con_int_prazo")
                .Not.Nullable();
            Map(x => x.Codigo, "con_int_codigo");
            Map(x => x.Situacao, "con_chr_situação")
                .CustomType<ESituacaoContrato>();
            Map(x => x.ValorPrestacao, "con_vlr_valorPrestacao")
                .Not.Nullable();
            Map(x => x.InicioAmortizacao, "con_dat_inicioAmortizacao");
            HasMany(x => x.Prestacoes)
                .KeyColumn("fkContratoPrestacao")
                .Inverse()
                .LazyLoad()
                .Cascade.All();
            References(x => x.IndiceCorrecao)
                .Column("fkIndiceCorrecaoContrato")
                .Cascade.None();
            References(x => x.Empregado)
                .Column("fkEmpregadoContrato")
                .Cascade.None();
        }
    }
}
