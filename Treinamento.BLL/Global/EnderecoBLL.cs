using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class EnderecoBLL : ABLL<Endereco, EnderecoBLL>
    {
        public override void Validar(Endereco pEndereco, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pEndereco.Logradouro.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Endereco", "Logadouro", true, ex);
            if (pEndereco.Numero == null)
                ex = new CampoNaoInformadoException("Endereco", "Numero", true, ex);
            if (pEndereco.Cidade == null)
                ex = new CampoNaoInformadoException("Endereco", "Cidade", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
