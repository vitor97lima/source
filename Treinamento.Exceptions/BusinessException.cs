using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public abstract class BusinessException : Exception
	{
		public BusinessException() : base() { }
		public BusinessException(string pMessage) : base(pMessage) { }
		public BusinessException(string pMessage, BusinessException pChildException, Exception pInnerException = null)
			: base(pMessage, pInnerException)
		{
			Child = pChildException;
		}

		private BusinessException _child;
		public BusinessException Child
		{
			get { return _child; }
			set { _child = value; }
		}

		public BusinessException Encadear(BusinessException pException)
		{
			if (Child != null) Child.Encadear(pException);
			else Child = pException;
			return this;
		}

		public virtual string Errors
		{
			get { return (Child != null ? Child.Errors : "") + this.Message + "\n"; }
		}

		public virtual string Warnings
		{
			get { return (Child != null ? Child.Warnings : ""); }
		}

		public virtual bool HasError
		{
			get { return true; }
		}
	}
}
