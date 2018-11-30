using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class ErrorException : BusinessException
	{
		public ErrorException() : base() { }
		public ErrorException(string pMessage) : base(pMessage) { }
		public ErrorException(string pMessage, BusinessException pChildException, Exception pInnerException = null)
			: base(pMessage, pChildException, pInnerException) { }
	}
}
