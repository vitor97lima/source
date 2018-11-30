using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Treinamento.DTO
{
	public class Ordenadora<T> : IComparer<T>
	{
		private string _stringOrdenacao;
		public string StringOrdenacao
		{
			get { return _stringOrdenacao; }
			set { _stringOrdenacao = value; }
		}

		public Ordenadora(string pStringOrdenacao)
		{
			this.StringOrdenacao = pStringOrdenacao;
		}

		public int Compare(T x, T y)
		{
			string[] lSortExpressions = _stringOrdenacao.Trim().Split(',');
			for (int i = 0; i < lSortExpressions.Length; i++)
			{
				string lFieldName, direction = "asc";
				if (lSortExpressions[i].Trim().EndsWith(" desc"))
				{
					lFieldName = lSortExpressions[i].Replace(" desc", "").Trim();
					direction = "desc";
				}
				else
				{
					lFieldName = lSortExpressions[i].Replace(" asc", "").Trim();
				}

				//Compare values, using IComparable interface of the property's type
				int iResult = Comparer.DefaultInvariant.Compare(Eval(x, lFieldName), Eval(y, lFieldName));
				if (iResult != 0)
				{
					//Return if not equal
					if (direction == "desc")
					{
						//Invert order
						return -iResult;
					}
					else
					{
						return iResult;
					}
				}
			}
			return 0;
		}

		private object Eval(T x, string pFieldName)
		{
			string[] path = pFieldName.Split('.');
			object valor = x;
			foreach (string field in path)
			{
				if (valor == null) return null;
				if (field == "!") return valor;
				valor = valor.GetType().InvokeMember(field, BindingFlags.GetProperty | BindingFlags.GetField, null, valor, null);
			}
			return valor;
		}
	}
}
