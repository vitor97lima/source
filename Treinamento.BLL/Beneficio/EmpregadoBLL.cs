using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DTO.Beneficio;
using Treinamento.Exceptions;

namespace Treinamento.BLL.Beneficio
{
    public class EmpregadoBLL : ABLL<Empregado, EmpregadoBLL>
    {
        public override void Validar(Empregado pEmpregado, bool pOpcionais = true)
        {
            BusinessException ex = null;

            if (pEmpregado.Nome.Trim().Equals(""))
                ex = new CampoNaoInformadoException("Empregado", "Nome", true, ex);
            if (pEmpregado.Cpf == null)
                ex = new CampoNaoInformadoException("Empregado", "Cpf", true, ex);
            if (pEmpregado.DataAdmissao == null)
                ex = new CampoNaoInformadoException("Empregado", "Data de Admissão", true, ex);

            if (pOpcionais)
            {
            }

            if (ex != null) throw ex;
        }
    }
}
