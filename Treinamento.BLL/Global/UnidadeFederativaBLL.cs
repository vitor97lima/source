using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
	public class UnidadeFederativaBLL : ABLL<UnidadeFederativa, UnidadeFederativaBLL>
	{
		private UnidadeFederativaBLL() { }

		public override void Validar(UnidadeFederativa pUnidadeFederativa, bool pOpcionais = true)
		{
			BusinessException ex = null;
			
			if (pUnidadeFederativa.Nome.Trim().Equals(""))
				ex = new CampoNaoInformadoException("Unidade Federativa", "Nome", true, ex);
			if (pUnidadeFederativa.Sigla.Trim().Equals(""))
				ex = new CampoNaoInformadoException("Unidade Federativa", "Sigla", true, ex);
			
			if (pOpcionais)
			{
			}

			if (ex != null) throw ex;
		}
	}
}
