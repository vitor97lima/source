using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Global;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Global
{
    public class TipoContaBancariaBLL : ABLL<TipoContaBancaria, TipoContaBancariaBLL>
    {
        public override void Validar(TipoContaBancaria pTipoContaBancaria, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pTipoContaBancaria.Descricao.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Tipo Conta Bancária", "Descrição", true, ex);
            if (pTipoContaBancaria.Operacao == null)
                ex = new CampoNaoInformadoException("Tipo Conta Bancária", "Operação", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
