using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class OperacaoNaoRealizadaException : BusinessException
	{
		public OperacaoNaoRealizadaException()
			: this(null, null)
		{

		}
		public OperacaoNaoRealizadaException(BusinessException pChildException, Exception pInnerException = null)
			: base("", pChildException, pInnerException)
		{

		}

		public override string Message
		{
			get { return "Não foi possível realizar a operação solicitada."; }
		}
	}
}
