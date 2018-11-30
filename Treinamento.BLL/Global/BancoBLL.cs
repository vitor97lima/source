using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class BancoBLL : ABLL<Banco, BancoBLL>
    {
        public override void Validar(Banco pBanco, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pBanco.Nome.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Banco", "Nome", true, ex);
            if (pBanco.Codigo == null)
                ex = new CampoNaoInformadoException("Banco", "Codigo", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
