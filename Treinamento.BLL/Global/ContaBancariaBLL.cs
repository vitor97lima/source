using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class ContaBancariaBLL : ABLL<ContaBancaria, ContaBancariaBLL>
    {
        public override void Validar(ContaBancaria pContaBancaria, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pContaBancaria.Numero.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Conta Bancária", "Número", true, ex);
            if (pContaBancaria.Digito == null)
                ex = new CampoNaoInformadoException("Conta Bancária", "Dígito", true, ex);
            if (pContaBancaria.Agencia == null)
                ex = new CampoNaoInformadoException("Conta Bancária", "Agência", true, ex);
            if (pContaBancaria.Tipo == null)
                ex = new CampoNaoInformadoException("Conta Bancária", "Tipo", true, ex);


            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
