using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class CampoNaoInformadoException : BusinessException
	{
		private readonly bool _requerido;
		public bool Requerido
		{
			get { return _requerido; }
		}

		private readonly string _campo;
		public string Campo
		{
			get { return _campo; }
		}

		private readonly string _objeto;
		public string Objeto
		{
			get { return _objeto; }
		}

		public CampoNaoInformadoException() : this("", "", false, null, null) { }

		public CampoNaoInformadoException(string pObjeto, string pCampo, bool pRequerido)
			: this(pObjeto, pCampo, pRequerido, null, null) { }

		public CampoNaoInformadoException(string pObjeto, string pCampo, bool pRequerido, BusinessException pChildException)
			: this(pObjeto, pCampo, pRequerido, pChildException, null) { }

		public CampoNaoInformadoException(string pObjeto, string pCampo, bool pRequerido, BusinessException pChildException, Exception pInnerException)
			: base("", pChildException, pInnerException)
		{
			_objeto = pObjeto;
			_campo = pCampo;
			_requerido = pRequerido;
		}

		public override string Errors
		{
			get { return (Child != null ? Child.Errors : "") + (Requerido ? this.Message + "\n" : ""); }
		}

		public override string Warnings
		{
			get { return (Child != null ? Child.Warnings : "") + (Requerido ? "" : this.Message + "\n"); }
		}

		public override bool HasError
		{
			get { return Requerido || (Child != null ? Child.HasError : false); }
		}

		public override string Message
		{
			get { return String.Format("Campo {0}'{1}' de '{2}' não informado.", (Requerido ? "obrigatório " : ""), Campo, Objeto); }
		}
	}
}
