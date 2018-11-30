using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class DinheiroPorExtenso
	{
		private static readonly string[] Currency = new string[] { "real", "reais", "centavo", "centavos" };
		private static readonly string[] Unidade = new string[] { "", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove" };
		private static readonly string[] Dezena = new string[] { "", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };
		private static readonly string[] Centena = new string[] { "", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" };
		private static readonly string[] DezAux = new string[] { "", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
		private static string EvalValue(string pTextValue, string pSingular, string pPlural)
		{
			string lTextCentena, lTextDezena, lTextUnidade;
			string result = "";
			if (pTextValue.ToInt32() == 0) return result;

			if (pTextValue.ToInt32() == 100) lTextCentena = "cem";
			else lTextCentena = Centena[pTextValue[0].ToInt32()];

			if (pTextValue[1] == '1' && pTextValue[2] != '0')
			{
				lTextDezena = DezAux[pTextValue[2].ToInt32()];
				lTextUnidade = "";
			}
			else
			{
				lTextDezena = Dezena[pTextValue[1].ToInt32()];
				lTextUnidade = Unidade[pTextValue[2].ToInt32()];
			}

			result = lTextCentena;
			if (result != "" && lTextDezena != "") result += " e ";
			result += lTextDezena;
			if (result != "" && lTextUnidade != "") result += " e ";
			result += lTextUnidade;

			if (pTextValue.ToInt32() > 1) result += pPlural;
			else result += pSingular;

			return result;
		}

		public static string GetExtenso(this double pValue)
		{
			return GetExtensoFmt((decimal)pValue, Currency[0], Currency[1], Currency[2], Currency[3]);
		}
		public static string GetExtenso(this decimal pValue)
		{
			return GetExtensoFmt(pValue, Currency[0], Currency[1], Currency[2], Currency[3]);
		}

		public static string GetExtensoFmt(this double pValue, string pUnit, string pUnits, string pFrac, string pFracs)
		{
			return GetExtensoFmt((decimal)pValue, pUnit, pUnits, pFrac, pFracs);
		}
		public static string GetExtensoFmt(this decimal pValue, string pUnit, string pUnits, string pFrac, string pFracs)
		{
			if (pUnit != "") pUnit = " " + pUnit;
			if (pUnits != "") pUnits = " " + pUnits;
			if (pFrac != "") pFrac = " " + pFrac;
			if (pFracs != "") pFracs = " " + pFracs;
			string lTextValue;
			string StrTrilhao, StrBilhao, StrMilhao, StrMilhar, StrCentena, StrCentavos;
			string ExtTrilhao, ExtBilhao, ExtMilhao, ExtMilhar, ExtCentena, ExtCentavos;
			int ValBilhao, ValMilhao, ValMilhar, ValCentena;
			string result = "";
			if (pValue == 0 || pValue >= 1e15m) return result;
			// converte
			lTextValue = pValue.ToString("f2").PadLeft(18, '0');

			// extrai composição
			StrTrilhao = lTextValue.Substring(0, 3);
			StrBilhao = lTextValue.Substring(3, 3);
			StrMilhao = lTextValue.Substring(6, 3);
			StrMilhar = lTextValue.Substring(9, 3);
			StrCentena = lTextValue.Substring(12, 3);
			StrCentavos = "0" + lTextValue.Right(2);

			// converte composição para inteiros
			ValBilhao = StrBilhao.ToInt32();
			ValMilhao = StrMilhao.ToInt32();
			ValMilhar = StrMilhar.ToInt32();
			ValCentena = StrCentena.ToInt32();

			// converte composição para extenso
			ExtTrilhao = EvalValue(StrTrilhao, " trilhão", " trilhões");
			ExtBilhao = EvalValue(StrBilhao, " bilhão", " bilhões");
			ExtMilhao = EvalValue(StrMilhao, " milhão", " milhões");
			ExtMilhar = EvalValue(StrMilhar, " mil", " mil");
			ExtCentena = EvalValue(StrCentena, "", "");
			ExtCentavos = EvalValue(StrCentavos, pFrac, pFracs);

			// adiciona string do trilhão
			result = ExtTrilhao;
			if (result != "" && (ExtBilhao != ""))
				if (ValBilhao < 100 || (ValBilhao % 100) == 0)
					if (ExtMilhao + ExtMilhar + ExtCentena == "") result += " e ";
					else result += ", ";
				else result += ", ";

			// adiciona string do bilhão
			result += ExtBilhao;
			if (result != "" && ExtMilhao != "")
				if (ValMilhao < 100 || (ValMilhao % 100) == 0)
					if (ExtMilhar + ExtCentena == "") result += " e ";
					else result += ", ";
				else result += ", ";

			// adiciona string do milhão
			result += ExtMilhao;
			if (result != "")
				if (ExtMilhar + ExtCentena == "") result += " de";
				else if (ValMilhar * ValCentena != 0) result += ", ";
				else if (ExtMilhar != "")
					if (ValMilhar < 100 || (ValMilhar % 100) == 0) result += " e ";
					else result += ", ";

			// adiciona string do milhar
			result += ExtMilhar;
			if (result != "" && ExtCentena != "")
				if (ValCentena < 100 || (ValCentena % 100) == 0) result += " e ";
				else result += ", ";

			// adiciona string da centena
			result += ExtCentena;

			// nome da moeda
			if (pValue >= 2) result += pUnits;
			else if (pValue >= 1) result += pUnit;

			// centavos
			if (result != "" && ExtCentavos != "")
				if (pUnit != "") result += " e ";
				else result += " vírgula ";
			result += ExtCentavos;
			return result;
		}
	}
}
