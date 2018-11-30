using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// Classe contendo pré-definições de máscaras
	/// </summary>
	public class Masks
	{
		private Masks() { }
		private Masks(String pMask)
		{
			_value = pMask;
		}

		private string _value;

		/// <summary>
		/// Retorna a string de máscara equivalente
		/// </summary>
		/// <returns>String com a máscara</returns>
		public override string ToString()
		{
			return _value; 
		}

		/// <summary>
		/// Cast para capturar a string de máscara equivalente
		/// </summary>
		/// <param name="pMask">O objeto Masks</param>
		/// <returns></returns>
		public static implicit operator String(Masks pMask)
		{
			return pMask.ToString();
		}

		/// <summary>
		/// Máscara de CPF '999.999.999-99'
		/// </summary>
		public static Masks CPF { get { return new Masks("999\\.999\\.999-99"); } }
		/// <summary>
		/// Máscara de CNPJ '99.999.999/9999-99'
		/// </summary>
		public static Masks CNPJ { get { return new Masks("99\\.999\\.999\\/9999-99"); } }
		/// <summary>
		/// Máscara de PIS '999.99999.99-9'
		/// </summary>
		public static Masks PIS { get { return new Masks("999\\.99999\\.99-9"); } }
		/// <summary>
		/// Máscara de CNPB '9999.9999-99'
		/// </summary>
		public static Masks CNPB { get { return new Masks("9999\\.9999-99"); } }
		/// <summary>
		/// Máscara de CEP '99999-999'
		/// </summary>
		public static Masks CEP { get { return new Masks("99999-999"); } }
		/// <summary>
		/// Máscara de CNAE '99.99-9-99'
		/// </summary>
		public static Masks CNAE { get { return new Masks("99\\.99\\-9\\-99"); } }
		/// <summary>
		/// Máscara de IBGE-Cidade '99-99.999'
		/// </summary>
		public static Masks IBGE_Cidade { get { return new Masks("99\\-99\\.999"); } }
		/// <summary>
		/// Máscara de SUSEP '99999.999999/9999-99'
		/// </summary>
		public static Masks SUSEP { get { return new Masks("99999\\.999999\\/9999-99"); } }
	}
}
