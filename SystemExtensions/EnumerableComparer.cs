using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// Classe para comparação de objetos em uma lista
	/// </summary>
	public class EnumerableComparer : IEqualityComparer<IEnumerable<object>>
	{
		/// <summary>
		/// Verifica se duas listas são iguais comparando cada objeto
		/// </summary>
		/// <param name="x">Lista 1 para comparação</param>
		/// <param name="y">Lista 2 para comparação</param>
		/// <returns>true se cada elemento da lista 1 for igual ao elemento de mesmo índice na lista 2</returns>
		public bool Equals(IEnumerable<object> x, IEnumerable<object> y)
		{
			if (x.Count() != y.Count()) return false;
			for (int i = 0; i < x.Count(); i++)
			{
				if (x.ElementAt(i) != null)
					if (!x.ElementAt(i).Equals(y.ElementAt(i))) return false;
					else { }
				else if (y.ElementAt(i) != null) return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <param name="obj">instance</param>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public int GetHashCode(IEnumerable<object> obj)
		{
			int result = obj.Count().GetHashCode();
			for (int i = 0; i < obj.Count(); i++)
			{
				if (obj.ElementAt(i) != null)
					result *= obj.ElementAt(i).GetHashCode();
			}
			return (int)Math.Sqrt(result);
		}
	}
}
