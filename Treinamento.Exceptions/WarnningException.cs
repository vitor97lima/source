using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class WarnningException : BusinessException
	{
		public WarnningException() : base() { }
		public WarnningException(string pMessage) : base(pMessage) { }
		public WarnningException(string pMessage, BusinessException pChildException, Exception pInnerException = null)
			: base(pMessage, pChildException, pInnerException) { }
		public override bool HasError
		{
			get { return Child != null ? Child.HasError : false; }
		}

		public override string Errors
		{
			get { return (Child != null ? Child.Errors : ""); }
		}

		public override string Warnings
		{
			get { return (Child != null ? Child.Warnings : "") + this.Message + "\n"; }
		}

	}
}
