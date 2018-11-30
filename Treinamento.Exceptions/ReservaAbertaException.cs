using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
	public class ReservaAbertaException : BusinessException
	{
		private DateTime _dataReferencia;
		public DateTime DataReferencia
		{
			get { return _dataReferencia; }
			set { _dataReferencia = value; }
		}

		public ReservaAbertaException(DateTime pDataReferencia) : this(pDataReferencia, null, null) { }

		public ReservaAbertaException(DateTime pDataReferencia, BusinessException pChildException, Exception pInnerException = null)
			: base("", pChildException, pInnerException)
		{
			_dataReferencia = pDataReferencia;
		}

		public override string Message
		{
			get { return String.Format("Reserva para a referência '{0:MM/yyyy}' não foi fechada.", DataReferencia); }
		}
	}
}
