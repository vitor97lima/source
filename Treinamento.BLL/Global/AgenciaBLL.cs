using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class AgenciaBLL : ABLL<Agencia, AgenciaBLL>
    {
        public override void Validar(Agencia pAgencia, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pAgencia.Nome.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Agencia", "Nome", true, ex);
            if (pAgencia.Codigo == null)
                ex = new CampoNaoInformadoException("Agencia", "Codigo", true, ex);
            if (pAgencia.Digito == null)
                ex = new CampoNaoInformadoException("Agencia", "Digito", true, ex);
            if (pAgencia.Banco == null)
                ex = new CampoNaoInformadoException("Agencia", "Banco", true, ex);
            if (pAgencia.Endereco == null)
                ex = new CampoNaoInformadoException("Agencia", "Endereco", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
