using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.Exceptions;
using Treinamento.DTO.Emprestimo;

namespace Treinamento.BLL.Emprestimo
{
    public class SaldoEmprestimoBLL : ABLL<SaldoEmprestimo, SaldoEmprestimoBLL>
    {
        public override void Validar(SaldoEmprestimo pObjeto, bool pOpcionais = true)
        {
            throw new NotImplementedException();
        }
    }
}
