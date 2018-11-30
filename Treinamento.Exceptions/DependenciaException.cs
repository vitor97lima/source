using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class DependenciaException : BusinessException
	{
		private readonly string _objeto;
		public string Objeto
		{
			get { return _objeto; }
		}

		private readonly string _dependente;
		public string Dependente
		{
			get { return _dependente; }
		}

		public DependenciaException() : this("", "", null, null) { }

		public DependenciaException(string pObjeto, string pDependente)
			: this(pObjeto, pDependente, null, null) { }

		public DependenciaException(string pObjeto, string pDependente, BusinessException pChildException)
			: this(pObjeto, pDependente, pChildException, null) { }

		public DependenciaException(string pObjeto, string pDependente, BusinessException pChildException, Exception pInnerException)
			: base("", pChildException, pInnerException)
		{
			_objeto = pObjeto;
			_dependente = pDependente;
		}

		public override string Errors
		{
			get { return (Child != null ? Child.Errors : "") + this.Message + "\n"; }
		}

		public override string Warnings
		{
			get { return (Child != null ? Child.Warnings : "") ; }
		}

		public override bool HasError
		{
			get { return true; }
		}

		public override string Message
		{
			get { return String.Format("Existe registro de '{0}' vinculado ao registro de '{1}'.", Dependente, Objeto); }
		}
	}
}
