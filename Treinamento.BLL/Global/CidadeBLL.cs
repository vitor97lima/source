using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class CidadeBLL : ABLL<Cidade, CidadeBLL>
    {
        public override void Validar(Cidade pCidade, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pCidade.Nome.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Cidade", "Nome", true, ex);
            if (pCidade.Uf == null)
                ex = new CampoNaoInformadoException("Cidade", "Uf", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
