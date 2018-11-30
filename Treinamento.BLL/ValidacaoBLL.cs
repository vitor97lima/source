using System;
using Treinamento.Exceptions;
using System.Text.RegularExpressions;

namespace Treinamento.BLL
{
	/// <summary>
	/// Classe com m�todos de valida��o de n�meros de registro
	/// </summary>
	public static class ValidacaoBLL
	{
		/// <summary>
		/// M�todo para c�lcular o d�gito do PIS
		/// </summary>
		/// <param name="pPIS">N�mero do PIS sem o d�gito</param>
		/// <returns>D�gito verificador do PIS</returns>
		public static string DigitoPIS(string pPIS)
		{
			if (pPIS.Trim().Length == 0) return "";

			pPIS = pPIS.Trim().Replace("-", "").Replace(".", "").PadLeft(10, '0').Left(10).Reverse();

			int soma = 0;
			for (int i = 0, fator = 0; i < 10; i++, fator = (fator + 1) % 8)
				soma += pPIS[i].ToInt32() * (fator + 2);

			int digito = soma % 11;

			if (digito < 2) digito = 0;
			else digito = 11 - digito;

			return digito.ToString();
		}

		/// <summary>
		/// Verifica se determinado n�mero de PIS � valido
		/// </summary>
		/// <param name="pPIS">N�mero de PIS a ser verificado</param>
		/// <returns>true se o n�mero for v�lido, false caso contr�rio</returns>
		public static bool ValidaPIS(string pPIS)
		{
			if (pPIS.Trim().Length == 0)
				return false;

			return pPIS.EndsWith(DigitoPIS(pPIS));
		}

		/// <summary>
		/// Verifica se o d�gito inicial do PIS � 1, 2 ou 9
		/// </summary>
		/// <param name="pPIS">N�mero de PIS a ser verificado</param>
		/// <returns>true se o d�gito inicial for 1, 2 ou 9, false caso contr�rio</returns>
		public static bool ValidaDigitoInicialPIS(string pPIS)
		{
			if (pPIS.Trim().Length == 0)
				return false;

			return pPIS.Trim().StartsWith("1") || pPIS.Trim().StartsWith("2") || pPIS.Trim().StartsWith("9");
		}

		/// <summary>
		/// M�todo para c�lcular o d�gito do CNPJ
		/// </summary>
		/// <param name="pCNPJ">N�mero do CNPJ sem o d�gito</param>
		/// <returns>D�gito verificador do CNPJ</returns>
		public static string DigitoCNPJ(string pCNPJ)
		{
			if (pCNPJ.Trim().Length == 0) return "";

			pCNPJ = pCNPJ.Trim().Replace("-", "").Replace(".", "").Replace("/", "").PadLeft(12, '0').Left(12).Reverse();

			int soma = 0;
			for (int i = 0, fator = 0; i < 12; i++, fator = (fator + 1) % 8)
				soma += pCNPJ[i].ToInt32() * (fator + 2);

			int digito1 = soma % 11;

			if (digito1 < 2) digito1 = 0;
			else digito1 = 11 - digito1;

			pCNPJ = digito1 + pCNPJ;

			soma = 0;
			for (int i = 0, fator = 0; i < 13; i++, fator = (fator + 1) % 8)
				soma += pCNPJ[i].ToInt32() * (fator + 2);

			int digito2 = soma % 11;

			if (digito2 < 2) digito2 = 0;
			else digito2 = 11 - digito2;

			return digito1.ToString()+digito2.ToString();
		}

		/// <summary>
		/// Verifica se determinado n�mero de CNPJ � valido
		/// </summary>
		/// <param name="pCNPJ">N�mero de CNPJ a ser verificado</param>
		/// <returns>true se o n�mero for v�lido, false caso contr�rio</returns>
		public static bool ValidaCNPJ(string pCNPJ)
		{
			if (pCNPJ.Trim().Length == 0)
				return false;

			return pCNPJ.EndsWith(DigitoCNPJ(pCNPJ));
		}

		/// <summary>
		/// M�todo para c�lcular o d�gito do CPF
		/// </summary>
		/// <param name="pCPF">N�mero do CPF sem o d�gito</param>
		/// <returns>D�gito verificador do CPF</returns>
		public static string DigitoCPF(string pCPF)
		{
			if (pCPF.Trim().Length == 0) return "";

			pCPF = pCPF.Trim().Replace("-", "").Replace(".", "").Replace("/", "").PadLeft(9, '0').Left(9).Reverse();

			int soma = 0;
			for (int i = 0, fator = 0; i < 9; i++, fator = (fator + 1))
				soma += pCPF[i].ToInt32() * (fator + 2);

			int digito1 = soma % 11;

			if (digito1 < 2) digito1 = 0;
			else digito1 = 11 - digito1;

			pCPF = digito1 + pCPF;

			soma = 0;
			for (int i = 0, fator = 0; i < 10; i++, fator = (fator + 1) )
				soma += pCPF[i].ToInt32() * (fator + 2);

			int digito2 = soma % 11;

			if (digito2 < 2) digito2 = 0;
			else digito2 = 11 - digito2;

			return digito1.ToString() + digito2.ToString();
		}

		/// <summary>
		/// Verifica se determinado n�mero de CPF � valido
		/// </summary>
		/// <param name="pCPF">N�mero de CPF a ser verificado</param>
		/// <returns>true se o n�mero for v�lido, false caso contr�rio</returns>
		public static bool ValidaCPF(string pCPF)
		{
			if (pCPF.Trim().Length == 0)
				return false;

			return pCPF.EndsWith(DigitoCPF(pCPF));
		}

		/// <summary>
		/// Verifica se determinado e-mail � v�lido
		/// </summary>
		/// <param name="email">E-mail a ser verificado</param>
		/// <returns>true se o e-mail for v�lido, false caso contr�rio</returns>
		public static bool ValidaEmail(string email)
		{
			Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
			return rg.IsMatch(email);
		}
	}
}
