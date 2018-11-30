using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class ReservaFechadaException : BusinessException
	{
		private DateTime _dataReferencia;
		public DateTime DataReferencia
		{
			get { return _dataReferencia; }
			set { _dataReferencia = value; }
		}

		public ReservaFechadaException(DateTime pDataReferencia) : this(pDataReferencia, null, null) { }

		public ReservaFechadaException(DateTime pDataReferencia, BusinessException pChildException, Exception pInnerException = null)
			: base("", pChildException, pInnerException)
		{
			_dataReferencia = pDataReferencia;
		}

		public override string Message
		{
			get { return String.Format("Reserva fechada para a referência '{0:MM/yyyy}'.", DataReferencia); }
		}
	}
}
