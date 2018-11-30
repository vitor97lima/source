using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// Estrutura para armazenamento de telefone
	/// </summary>
	[Serializable]
	public struct Telefone
	{
		readonly int? _ddi;
		/// <summary>
		/// Gets the DDI code área
		/// </summary>
		public int? DDI
		{
			get { return _ddi; }
		}

		readonly int? _ddd;
		/// <summary>
		/// Gets the DDD code área
		/// </summary>
		public int? DDD
		{
			get { return _ddd; }
		}

		readonly string[] _parts;
		/// <summary>
		/// Gets the PhoneNumber
		/// </summary>
		public string Numero
		{
			get { return _parts != null ? (_parts.StringJoin("-")) : null; }
		}

		readonly string _ramal;
		/// <summary>
		/// Gets the Ramal
		/// </summary>
		public string Ramal
		{
			get { return _ramal; }
		}

		private static string[] split(string valor)
		{
			if (valor.IsNullOrEmpty()) return null;
			valor = valor.ClearMask();
			var sizes = new int[] { valor.Length % 4 }
				.Concat(Enumerable.Repeat(4, valor.Length / 4))
				.Where(x => x != 0).ToArray();
			return valor.Split(sizes);
		}

		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pNumero">String com o número do telefone</param>
		public Telefone(string pNumero) : this(null, null, pNumero, null) { }
		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pDDD">Inteiro com o DDD</param>
		/// <param name="pNumero">String com o número do telefone</param>
		public Telefone(int? pDDD, string pNumero) : this(null, pDDD, pNumero, null) { }
		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pNumero">String com o número do telefone</param>
		/// <param name="pRamal">String com o ramal do telefone</param>
		public Telefone(string pNumero, string pRamal) : this(null, null, pNumero, pRamal) { }
		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pDDI">Inteiro com o DDI</param>
		/// <param name="pDDD">Inteiro com o DDD</param>
		/// <param name="pNumero">String com o número do telefone</param>
		public Telefone(int? pDDI, int? pDDD, string pNumero) : this(pDDI, pDDD, pNumero, null) { }
		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pDDD">Inteiro com o DDD</param>
		/// <param name="pNumero">String com o número do telefone</param>
		/// <param name="pRamal">String com o ramal do telefone</param>
		public Telefone(int? pDDD, string pNumero, string pRamal) : this(null, pDDD, pNumero, pRamal) { }
		/// <summary>
		/// Contrutor de Telefone
		/// </summary>
		/// <param name="pDDI">Inteiro com o DDI</param>
		/// <param name="pDDD">Inteiro com o DDD</param>
		/// <param name="pNumero">String com o número do telefone</param>
		/// <param name="pRamal">String com o ramal do telefone</param>
		public Telefone(int? pDDI, int? pDDD, string pNumero, string pRamal)
		{
			_ddi = pDDI;
			_ddd = pDDD;
			_parts = split(pNumero);
			_ramal = pRamal.RemoveCaracters('/');
		}

		/// <summary>
		/// Trata determinada string separando os campos de telefone
		/// </summary>
		/// <param name="pFullNumber">String contendo um telefone</param>
		/// <returns>Estrutura Telefone equivalente</returns>
		private static Telefone FromString(string pFullNumber)
		{
			string lFullNumber = pFullNumber;
			pFullNumber = pFullNumber.Replace('.', ' ').RemoveLetras().Trim();
			int? lDDI = null, lDDD = null;
			string lNumero = null, lRamal = null;
			int index = 0;
			if ((index = pFullNumber.IndexOf("/")) >= 0)
			{
				lRamal = pFullNumber.Substring(index + 1);
				pFullNumber = pFullNumber.Left(index);
			}
			if ((index = pFullNumber.IndexOf("+")) >= 0)
			{
				if (index > 0)
				{
					pFullNumber = pFullNumber.Replace("+", " ");
				}
				else
				{
					int index2 = pFullNumber.IndexOf('(') >= 0 ? pFullNumber.IndexOf('(') : pFullNumber.IndexOf(' ');
					if (index2 >= 0)
					{
						lDDI = pFullNumber.Substring(index + 1, index2 - index - 1).ToInt32();
						if (lDDI == 0) lDDI = null;
						pFullNumber = pFullNumber.Substring(index2).Trim();
					}
				}
			}
			if ((index = pFullNumber.IndexOf('(')) >= 0)
			{
				int index2 = pFullNumber.IndexOf(')');
				if (index2 >= 0)
				{
					lDDD = pFullNumber.Substring(index + 1, index2 - index - 1).ToInt32();
					if (lDDD == 0) lDDD = null;
					pFullNumber = pFullNumber.Remove(index, index2 - index + 1).Trim();
				}
				else pFullNumber = pFullNumber.Remove(index, 1).Trim();
			}
			if (pFullNumber.IndexOf(' ') == pFullNumber.LastIndexOf(' ') && pFullNumber.IndexOf('-') < 0)
			{
				lNumero = pFullNumber;
				pFullNumber = "";
			}
			else if (pFullNumber.IndexOf('-') >= 0 && (index = pFullNumber.IndexOf(' ')) >= 0 && pFullNumber.IndexOf('-') > index && lDDD == null)
			{
				lDDD = pFullNumber.Left(index).ToInt32();
				lNumero = pFullNumber = pFullNumber.Substring(index + 1);
			}
			else if ((index = pFullNumber.IndexOf(' ')) != pFullNumber.LastIndexOf(' ') && lDDD == null)
			{
				lDDD = pFullNumber.Left(index).GetOnlyNumbers().ToInt32();
				lNumero = pFullNumber = pFullNumber.Substring(index + 1);
			}
			else lNumero = pFullNumber;

			Telefone t = new Telefone(lDDI, lDDD, lNumero, lRamal);
			if (t.ToString().GetOnlyNumbers() != lFullNumber.GetOnlyNumbers() &&
				t.ToString().GetOnlyNumbers().ToInt64() != lFullNumber.GetOnlyNumbers().ToInt64()
				)
				throw new Exception("Texto \'{0}\' virou telefone \'{1}\'".Formata(lFullNumber, t));
			return t;
		}

		/// <summary>
		/// Retorna o texto equivalente do telefone
		/// </summary>
		/// <returns>String de texto equivalente</returns>
		public override string ToString()
		{
			return (DDI != null ? "+" + DDI.ToString() + " " : "")
				+ (DDD != null ? "(" + DDD.ToString() + ") " : "")
				+ Numero
				+ (Ramal.IsNullOrEmpty() ? "" : "/" + Ramal);
		}

		/// <summary>
		/// Compara o Telefone em questão com determinado objeto
		/// </summary>
		/// <param name="obj">Objeto a ser comparado</param>
		/// <returns>true se obj for um Telefone e todos os campos forem iguais, false caso contrário</returns>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(String)) return this.ToString() == obj.ToString();
			if (!(obj is Telefone)) return false;
			Telefone tel = (Telefone)obj;
			return this.ToString() == tel.ToString();
		}

		/// <summary>
		/// Returns the hash code for this Telefone.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Compara dois Telefones
		/// </summary>
		/// <param name="t1">Telefone 1</param>
		/// <param name="t2">Telefone 2</param>
		/// <returns>true se os telefones forem iguais, false caso contrário</returns>
		public static bool operator ==(Telefone t1, Telefone t2)
		{
			if (((object)t1) != null) return t1.Equals(t2);
			else if (((object)t2) != null) return false; //t1 is null
			else return true; //both is null
		}

		/// <summary>
		/// Compara dois Telefones
		/// </summary>
		/// <param name="t1">Telefone 1</param>
		/// <param name="t2">Telefone 2</param>
		/// <returns>true se os telefones forem diferentes, false caso contrário</returns>
		public static bool operator !=(Telefone t1, Telefone t2)
		{
			return !(t1 == t2);
		}

		/// <summary>
		/// Verifica se a string gerada pelo método ToString() contém determinada string
		/// </summary>
		/// <param name="s">string para ser testada</param>
		/// <returns>True se ToString() contem a string s, false caso contrário</returns>
		public bool Contains(string s)
		{
			return this.ToString().Contains(s);
		}

		/// <summary>
		/// Operador de cast de Telefone para String
		/// </summary>
		/// <param name="t">Telefone</param>
		/// <returns>Uma string com a representação do Telefone</returns>
		public static implicit operator string (Telefone t)
		{
			return t.ToString();
		}

		public static implicit operator Telefone(string s)
		{
			return FromString(s);
		}

	}
}
