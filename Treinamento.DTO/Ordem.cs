using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace System.Linq
{
	public class Ordem<TDTO> where TDTO : class
	{
		public bool Crescente { get; set; }

		private Expression<Func<TDTO, object>> _property;
		public Expression<Func<TDTO, object>> Property
		{
			get { return _property; }
			set { _property = value; }
		}

		private Ordem() { }

		private Ordem(Expression<Func<TDTO, object>> pProperty, bool pCrescente)
		{
			Crescente = pCrescente;
			Property = pProperty;
		}

		public static Ordem<TDTO> ASC(Expression<Func<TDTO, object>> pProperty)
		{
			return new Ordem<TDTO>(pProperty, true);
		}

		public static Ordem<TDTO> DESC(Expression<Func<TDTO, object>> pProperty)
		{
			return new Ordem<TDTO>(pProperty, false);
		}
	}
}
