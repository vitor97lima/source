using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Treinamento.DTO.Beneficio;

namespace Treinamento.DAL.NHibernate.Modelo.Beneficio
{
	public class EventoFolhaMapping : ClassMap<EventoFolha>
	{
		public EventoFolhaMapping()
		{
			Schema("ben");
			Table("tbEventoFolha");
			Id(x => x.Id, "evf_int_id")
				.GeneratedBy.Native("sqEventoFolha");
			Map(x => x.Descricao, "evf_str_descricao")
				.Not.Nullable()
				.Length(255);
			Map(x => x.TipoEvento, "evf_chr_tipoEvento")
				.CustomType<ETipoEventoType>();
            Map(x => x.Percentual, "evf_pct_percentual")
                .Not.Nullable();
		}
	}
}
