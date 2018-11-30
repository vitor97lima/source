using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public enum ESummaryOperation
	{
		None = 0,
		[FriendlyName("Soma")]
		Sum = 1,
		[FriendlyName("Média")]
		Average,
		[FriendlyName("Qtde")]
		Count,
		[FriendlyName("Produto")]
		Multiply,
		[FriendlyName("Acumulado")]
		Acumulate,
		[FriendlyName("Moda")]
		Mode,
		Last,
	}
}
