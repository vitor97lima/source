using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public class GenericCompare<T> : IEqualityComparer<T>
	{
		private Func<T, object> _expr { get; set; }

		private Func<T, T, bool> _equals { get; set; }

		public GenericCompare(Func<T, object> expr)
		{
			this._expr = expr;
		}

		public GenericCompare(Func<T, T, bool> expr)
		{
			this._equals = expr;
		}

		public bool Equals(T x, T y)
		{
			if (_equals != null) return _equals(x, y);
			var first = _expr.Invoke(x);
			var sec = _expr.Invoke(y);
			if (first != null && first.Equals(sec)) return true;
			else return false;
		}

		public int GetHashCode(T obj)
		{
			return _expr != null ? _expr(obj).GetHashCode() : 0;
		}
	}
}
