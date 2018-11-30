using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Treinamento.Exceptions;

namespace Treinamento.BLL
{
	/// <summary>
	/// Classe abstrata com métodos essenciais para tratamento de enums
	/// </summary>
	/// <typeparam name="TDTO">O tipo DTO que será implementado</typeparam>
	public abstract class EBLL<TDTO, TBLL>
		where TDTO : struct
		where TBLL : EBLL<TDTO, TBLL>
	{
		protected EBLL() { }

		private static TBLL _instance;
		public static TBLL Instance
		{
			get { return _instance = _instance ?? (TBLL)Activator.CreateInstance(typeof(TBLL), true); }
		}

		/// <summary>
		/// Fornece uma lista de pares (Key,Value) para utilização em componentes
		/// </summary>
		/// <returns>Dictionary contendo Chave e Valor de todos os itens do enum <![CDATA[$T$]]></returns>
		public Dictionary<TDTO, string> GetKeysAndValues()
		{
			return GetKeys().ToDictionary(x => x, x => GetValue(x));
		}

		public Dictionary<TDTO, string> GetKeysAndValues(Func<TDTO, bool> pFilter)
		{
			return GetKeys().Where(pFilter).ToDictionary(x => x, x => GetValue(x));
		}

		public Dictionary<TDTO, string> GetKeysAndValuesFlasg(TDTO pFilter)
		{
			return GetKeys().Where(x => pFilter.HasAnyFlag(x)).ToDictionary(x => x, x => GetValue(x));
		}

		private List<string> GetValues()
		{
			return Enum.GetValues(typeof(TDTO)).Cast<TDTO>().Select(s => GetValue(s)).ToList();
		}

		private List<TDTO> GetKeys()
		{
			return Enum.GetValues(typeof(TDTO)).Cast<TDTO>().ToList();
		}

		/// <summary>
		/// Retorna o texto representado pelo enum
		/// </summary>
		/// <param name="pObject">Variável do tipo enum T</param>
		/// <returns></returns>
		public virtual string GetValue(TDTO pObject)
		{
			return (pObject as Enum).GetFriendlyName();
		}

		/// <summary>
		/// Converte a string em enum
		/// </summary>
		/// <param name="pObject">String a ser converitda</param>
		/// <returns>Valor do Enum T</returns>
		public TDTO Parse(string pObject)
		{
			if (pObject.IsNullOrEmpty()) pObject = "0";
			TDTO result;
			if (Enum.TryParse<TDTO>(pObject, true, out result))
				return result;
			foreach (string value in Enum.GetNames(typeof(TDTO)))
			{
				if (pObject.EqualsIgnoreSymbols(value))
					return (TDTO)Enum.Parse(typeof(TDTO), value, true);
			}
			return (TDTO)Enum.Parse(typeof(TDTO), "0", true);
		}

		/// <summary>
		/// Converte um inteiro em enum
		/// </summary>
		/// <param name="pObject">Inteiro a ser converitda</param>
		/// <returns>Valor do Enum T</returns>
		public TDTO Parse(int pObject)
		{
			return (TDTO)Enum.Parse(typeof(TDTO), pObject.ToString());
		}

		/// <summary>
		/// Verifica se a o valor de pEnum está definido no tipo enum T
		/// </summary>
		/// <param name="pEnum">Variável a ser verificada</param>
		/// <returns>true se tipo enum T define o valor de pEnum, false caso contrário</returns>
		public bool IsDefined(TDTO pEnum)
		{
			return Enum.IsDefined(typeof(TDTO), pEnum);
		}
	}
}
