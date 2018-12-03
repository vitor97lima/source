using FluentNHibernate.Mapping;
using Treinamento.DTO.Beneficio;

namespace Treinamento.DAL.NHibernate.Modelo.Beneficio
{
    public class ContraChequeMapping : ClassMap<ContraCheque>
    {
        public ContraChequeMapping()
        {
            Schema("ben");
            Table("tbContraCheque");
            Id(x => x.Id, "cch_int_id")
                .GeneratedBy.Native("sqContraCheque");
            Map(x => x.Data, "cch_dat_data")
                .Not.Nullable();
            Map(x => x.ValorLiquido, "cch_vlr_valorLiquido")
                .Not.Nullable();
            Map(x => x.SalarioBase, "cch_vlr_SalarioBase")
                .Not.Nullable();
            References(x => x.Empregado)
                .Column("fkEmpregadoContraCheque")
                .Cascade.None();
            HasMany(x => x.Eventos)
              .Table("tbContraChequeEventos")
              .KeyColumn("cch_int_id")
              .AsEntityMap("evf_int_id")
              .Element("cce_vlr_valor", y => y.Type<float>());
        }
    }
}
