using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Treinamento.Exceptions;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace System
{
	/// <summary>
	/// Classe com métodos extendidos para diversas classes do assembly System
	/// </summary>
	public static class ExtensionMethodsSys
	{
		#region Extensions for IEnumerable<T>

		/// <summary>
		/// Performs the specified action on each element of the System.Collections.Generic.List(T).
		/// </summary>
		/// <typeparam name="T">The Type of the List</typeparam>
		/// <param name="pLista">The List on which elements peform the action</param>
		/// <param name="pAction">The System.Action(T) delegate to perform on each element of the System.Collections.Generic.List(T).</param>
		public static void ForEach<T>(this IEnumerable<T> pLista, Action<T> pAction)
		{
			foreach (T item in pLista)
				pAction.Invoke(item);
		}

		public static void ParallelForEach<T>(this IEnumerable<T> pLista, Action<T> pAction)
		{
			ParallelLoopResult r = Parallel.ForEach(pLista, pAction);
			while (!r.IsCompleted) Thread.Sleep(100);
		}

		public static List<TResult> ParallelSelect<T, TResult>(this IEnumerable<T> pLista, Func<T, TResult> pAction)
		{
			return ParallelEnumerable.Select(pLista.AsParallel(), pAction).ToList();
		}

		public static double StandardDeviation(this IEnumerable<double> values)
		{
			double avg = values.Average();
			return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
		}

		public static TResult Mode<T, TResult>(this IEnumerable<T> list, Func<T, TResult> prop)
		{
			return list.GroupBy(prop).OrderByDescending(x => x.Count()).FirstOrDefault().Key;
		}

		public static TResult Mode<T, TResult>(this IEnumerable<T> list, Func<T, TResult> prop, TResult pDefaultIfNoSingle)
		{
			var t = list.GroupBy(prop).OrderByDescending(x => x.Count());
			if (!t.Any()) return pDefaultIfNoSingle;
			if (t.Count() > 1 && t.Skip(1).Any(x => x.Count() == t.ElementAt(0).Count()))
				return pDefaultIfNoSingle;
			return t.First().Key;
		}

		public static double StandardDeviation(this IEnumerable<long> values)
		{
			double avg = values.Average();
			return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
		}

		public static int MDC(this IEnumerable<int> list)
		{
			if (list.Count() == 0) return 0;
			if (list.Count() == 1) return list.ElementAt(0);
			int a = list.ElementAt(0), b = list.ElementAt(1);
			int resto;
			while (b != 0)
			{
				resto = a % b;
				a = b;
				b = resto;
			}
			return MDC(list.Skip(2).Concat(a));
		}

		public static IEnumerable<IGrouping<TKey, TElement>> GroupPartitionBy<TKey, TElement, TOrder>(
			this IEnumerable<TElement> list, Func<TElement, TKey> keySelector, Func<TElement, TOrder> oderSelector)
		{
			return list.OrderBy(oderSelector).GroupPartitionBy(keySelector);
		}

		public static IEnumerable<IGrouping<TKey, TElement>> GroupPartitionBy<TKey, TElement>(
			this IEnumerable<TElement> list, Func<TElement, TKey> keySelector)
		{
			if (list.Count() == 0) yield break;
			List<TElement> sub = new List<TElement>();
			TKey lastKey = keySelector.Invoke(list.First()), key = default(TKey);
			foreach (var item in list)
			{
				key = keySelector.Invoke(item);
				if (!key.Equals(lastKey))
				{
					yield return sub.GroupBy(x => key).FirstOrDefault();
					sub = new List<TElement>();
				}
				sub.Add(item);
				lastKey = key;
			}
			yield return sub.GroupBy(x => key).FirstOrDefault();
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pLista)
		{
			return pLista.ToDictionary(x => x.Key, x => x.Value);
		}

		/// <summary>
		/// Invoke ToString Method for each element and generates a file with name 'pFileName'
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pList"></param>
		/// <param name="pFileName">The full name of the file</param>
		/// <returns>null if sequence contains no elements, a FileInfo otherwise</returns>
		public static FileInfo SaveToFile<T>(this IEnumerable<T> pList, string pFileName)
		{
			if (pList.Count() == 0) return null;
			Directory.CreateDirectory(Directory.GetParent(pFileName).FullName);
			StreamWriter lArquivo = new StreamWriter(pFileName, false, Encoding.GetEncoding("iso-8859-15"));
			foreach (T item in pList)
				lArquivo.WriteLine(item.ToString());
			lArquivo.Close();
			return new FileInfo(pFileName);
		}

		public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return FullOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}
		public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			var innerLookup = inner.ToLookup(innerKeySelector);
			var outerLookup = outer.ToLookup(outerKeySelector);

			var innerJoinItems = comparer != null
			? inner.Where(innerItem => !outerLookup.Select(x => x.Key).Contains(innerKeySelector(innerItem), comparer))
				.Select(innerItem => resultSelector(default(TOuter), innerItem))
			: inner.Where(innerItem => !outerLookup.Contains(innerKeySelector(innerItem)))
				.Select(innerItem => resultSelector(default(TOuter), innerItem));

			return outer
				.SelectMany(outerItem =>
				{
					var innerItems = comparer != null
						? innerLookup.Where(x => comparer.Equals(x.Key, outerKeySelector(outerItem))).SelectMany(x => x.ToList())
						: innerLookup[outerKeySelector(outerItem)];

					return innerItems.Any() ? innerItems : new TInner[] { default(TInner) };
				}, resultSelector)
				.Concat(innerJoinItems);
		}

		public static IEnumerable<TResult> Concat<TResult>(this IEnumerable<TResult> pList, params TResult[] pValue)
		{
			foreach (var item in pList)
				yield return item;
			foreach (var item in pValue)
				yield return item;
		}

		public static IEnumerable<TResult> Except<TResult>(this IEnumerable<TResult> pList, IEnumerable<TResult> pValue, Func<TResult, object> pKey)
		{
			return pList.Except(pValue, new GenericCompare<TResult>(pKey));
		}

		public static IEnumerable<TResult> Except<TResult>(this IEnumerable<TResult> pList, IEnumerable<TResult> pValue, Func<TResult, TResult, bool> pEquality)
		{
			return pList.Except(pValue, new GenericCompare<TResult>(pEquality));
		}

		public static IEnumerable<TResult> Except<TResult>(this IEnumerable<TResult> pList, params TResult[] pValue)
		{
			return pList.Except((IEnumerable<TResult>)pValue);
		}

		public static string StringJoin<T>(this IEnumerable<T> pLista, string pSeparator = "", int pMaxLength = 0)
		{
			string lResult = string.Join(pSeparator, pLista);
			if (pMaxLength > 0 && lResult.Length > pMaxLength)
				lResult = lResult.Left(pMaxLength - 1) + "…";
			return lResult;
		}

		public class PivotRow<TColumn, TValue> : Dictionary<TColumn, TValue>
		{
			public new TValue this[TColumn column]
			{
				get
				{
					TValue result;
					this.TryGetValue(column, out result);
					return result;
				}
				set
				{
					this.Remove(column);
					this.Add(column, value);
				}
			}
		}

		public class PivotTable<TRow, TColumn, TValue> : Dictionary<TRow, PivotRow<TColumn, TValue>>
		{
			/// <summary>
			/// Gets the columns of the table.
			/// </summary>
			internal TColumn[] _columns;
			public TColumn[] Columns
			{
				get { return _columns.ToArray(); }
			}

			public void Add(TRow row, TColumn column, TValue value)
			{
				if (!this.ContainsKey(row)) this.Add(row, new PivotRow<TColumn, TValue>());
				if (!this[row].ContainsKey(column)) this[row].Add(column, value);
				else this[row][column] = value;
			}

			public TValue this[TRow row, TColumn column]
			{
				get
				{
					if (this.ContainsKey(row))
						if (this[row].ContainsKey(column))
							return this[row][column];
					return default(TValue);
				}
				set
				{
					Add(row, column, value);
				}
			}
		}

		public static PivotTable<TRow, TColumn, TValue> Pivot<T, TRow, TColumn, TValue>(this IEnumerable<T> pList, Func<T, TRow> pKey1, Func<T, TColumn> pKey2, Func<T, TValue> pValueSelector)
		{
			PivotTable<TRow, TColumn, TValue> result = new PivotTable<TRow, TColumn, TValue>();
			result._columns = pList.Select(pKey2).Distinct().ToArray();
			pList.GroupBy(x => new { row = pKey1(x), column = pKey2(x) })
				.ForEach(key1 => result.Add(key1.Key.row, key1.Key.column, pValueSelector(key1.First())));
			return result;
		}

		public static PivotTable<TRow, TColumn, decimal> Pivot<T, TRow, TColumn>(this IEnumerable<T> pList, Func<T, TRow> pKey1, Func<T, TColumn> pKey2, Func<T, decimal> pValueSelector)
		{
			PivotTable<TRow, TColumn, decimal> result = new PivotTable<TRow, TColumn, decimal>();
			result._columns = pList.Select(pKey2).Distinct().ToArray();
			pList.GroupBy(x => new { row = pKey1(x), column = pKey2(x) })
				.ForEach(x => result.Add(x.Key.row, x.Key.column, x.Sum(y => pValueSelector(y))));
			return result;
		}

		#endregion

		#region Extensions for IList<T>

		public static bool Any<TSource>(this IList<TSource> pList, Expression<Func<TSource, bool>> pAction = null)
		{
			if (pAction != null)
				return pList.Any(pAction.Compile());
			return pList.Count > 0;
		}

		/// <summary>
		/// Mescla duas listas desconsiderando os elementos repetidos
		/// </summary>
		/// <typeparam name="T">O tipo de elemento da lista</typeparam>
		/// <param name="pLista">A lista afetada</param>
		/// <param name="pMergeList">A lista que será mesclada</param>
		public static IList<T> Merge<T>(this IList<T> pLista, IEnumerable<T> pMergeList)
		{
			return pLista.Merge(pMergeList, null);
		}

		public static IList<T> Merge<T>(this IList<T> pLista, IEnumerable<T> pMergeList, IEqualityComparer<T> comparer)
		{
			pMergeList.Except(pLista, comparer).ForEach(x => pLista.Add(x));
			return pLista;
		}

		public static IList<T> Merge<T>(this IList<T> pLista, params T[] pMergeList)
		{
			return pLista.Merge((IEnumerable<T>)pMergeList, null);
		}

		/// <summary>
		/// Adiciona os elementos de uma segunda lista na primeira lista
		/// </summary>
		/// <typeparam name="T">O tipo de elemento da lista</typeparam>
		/// <param name="pLista">A lista afetada</param>
		/// <param name="pMergeList">A lista que será adicionada</param>
		public static IList<T> Append<T>(this IList<T> pLista, IEnumerable<T> pMergeList)
		{
			pMergeList.ToList().ForEach(o => pLista.Add(o));
			return pLista;
		}

		public static int RemoveAll<T>(this IList<T> pLista, IEnumerable<T> pRemoveList)
		{
			int qt = 0;
			pRemoveList.ToList().ForEach(o => qt += pLista.Remove(o).ToInt32());
			return qt;
		}

		/// <summary>
		/// Adiciona vários elementos à lista
		/// </summary>
		/// <typeparam name="T">O tipo de elemento da lista</typeparam>
		/// <param name="pLista">A lista afetada</param>
		/// <param name="pObjects">Os elementos a serem adicionados</param>
		public static void Add<T>(this IList<T> pLista, params T[] pObjects)
		{
			pObjects.ToList().ForEach(o => pLista.Add(o));
		}

		/// <summary>
		/// Remove da lista todas as ocorrências que combinam com determinada condição
		/// </summary>
		/// <typeparam name="T">O tipo da lista</typeparam>
		/// <param name="pLista">A lista para manipulação</param>
		/// <param name="pMatch">A condição de eliminação</param>
		/// <returns>Quantidade de objetos removidos</returns>
		public static int RemoveAll<T>(this IList<T> pLista, Predicate<T> pMatch)
		{
			int c = 0;
			for (int i = pLista.Count - 1; i >= 0; i--)
			{
				if (pMatch.Invoke(pLista.ElementAt(i)))
				{
					pLista.RemoveAt(i);
					c++;
				}
			}
			return c;
		}

		public static Excel<T> Excel<T>(this IEnumerable<T> pLista, params string[] pHeaderText)
		{
			return new Excel<T>(pLista).HeaderText(pHeaderText);
		}

		/// <summary>
		/// Gera uma pasta de trabalho do excel com os dados informados
		/// </summary>
		/// <typeparam name="T">O tipo do dado</typeparam>
		/// <param name="pLista">Lista de objetos</param>
		/// <param name="pTitulos">Lista de títulos das colunas</param>
		/// <param name="pValores">Lista de expressões para apuração dos valores</param>
		public static Data.DataTable ToDataTable<T>(this IList<T> pLista, string[] pTitulos, Func<T, object>[] pValores)
		{
			Data.DataTable result = new Data.DataTable();

			for (int i = 0; i < pTitulos.Length; i++)
				result.Columns.Add(pTitulos[i], typeof(String));

			for (int i = 0; i < pLista.Count; i++)
			{
				DataRow r = result.NewRow();
				for (int j = 0; j < pValores.Length; j++)
					r[j] = pValores[j].Invoke(pLista.ElementAt(i));
			}

			return result;
		}
		#endregion

		#region Generic List

		/// <summary>
		/// Replaces a specific item from the System.Collections.Generic.List&lt;T&gt;.
		/// </summary>
		/// <typeparam name="T">object type</typeparam>
		/// <param name="olditem">item to be replaced.</param>
		/// <param name="newitem">item to replce.</param>
		/// <returns></returns>
		public static List<T> Replace<T>(this List<T> list, T olditem, T newitem)
		{
			int i = list.IndexOf(olditem);
			if (i >= 0)
			{
				list.Remove(olditem);
				list.Insert(i, newitem);
			}
			return list;
		}

		/// <summary>
		/// Replaces an item that matches the conditions of the specified predicate from the System.Collections.Generic.List&lt;T&gt;.
		/// </summary>
		/// <typeparam name="T">object type</typeparam>
		/// <param name="match">predicate that defines the conditions to search for.</param>
		/// <param name="newitem">item to replce.</param>
		/// <returns></returns>
		public static List<T> Replace<T>(this List<T> list, Predicate<T> match, T newitem)
		{
			T olditem = list.Find(match);
			int i = list.IndexOf(olditem);
			if (i >= 0)
			{
				list.Remove(olditem);
				list.Insert(i, newitem);
			}
			return list;
		}

		public static List<T> ReplaceOrAdd<T>(this List<T> list, T olditem, T newitem)
		{
			int i = list.IndexOf(olditem);
			if (i >= 0)
			{
				list.Remove(olditem);
				list.Insert(i, newitem);
			}
			else list.Add(newitem);
			return list;
		}

		public static List<T> ReplaceOrAdd<T>(this List<T> list, Predicate<T> match, T newitem)
		{
			T olditem = list.Find(match);
			int i = list.IndexOf(olditem);
			if (i >= 0)
			{
				list.Remove(olditem);
				list.Insert(i, newitem);
			}
			else list.Add(newitem);
			return list;
		}

		#endregion

		#region Extensions for Array

		/// <summary>
		/// Remove todos os elementos de um array que satisfaçam determinada condição
		/// </summary>
		/// <typeparam name="T">O tipo dos elementos do array</typeparam>
		/// <param name="pArray">O array afetado</param>
		/// <param name="match">A condição de exclusão</param>
		/// <returns></returns>
		public static T[] RemoveAll<T>(this T[] pArray, Predicate<T> match)
		{
			List<T> lLista = new List<T>(pArray);
			lLista.RemoveAll(match);
			return lLista.ToArray();
		}
		#endregion

		#region Extensions for String

		public static string ReplaceLast(this string pString, string oldValue, string newValue)
		{
			int index = pString.LastIndexOf(oldValue);
			if (index >= 0)
			{
				pString = pString.Remove(index, oldValue.Length);
				pString = pString.Insert(index, newValue);
			}
			return pString;
		}

		public static string Abreviar(this string pNome)
		{
			Dictionary<string, string> n = new Dictionary<string, string>() {
				{ "Junior", "Jr." },
				{ "Filho", "Fl." },
				{ "Neto", "Nt." },
				{ "de", "de" },
				{ "da", "da" },
				{ "do", "do" },
				{ "dos", "dos" },
				{ "das", "das" },
			};
			var words = pNome.Split(' ');
			int ix;
			if (n.ContainsKey(words[words.Length - 1]))
			{
				words[words.Length - 1] = n[words[words.Length - 1]];
				ix = words.Length - 2;
			}
			else { ix = words.Length - 1; }
			for (int i = 1; i < ix; i++)
				if (n.ContainsKey(words[i])) words[i] = n[words[i]];
				else words[i] = words[i][0] + ".";

			return string.Join(" ", words);
		}

		public static bool Between(this string pString, string pStringInicial, string pStringFinal)
		{
			return pString.CompareTo(pStringInicial) >= 0 && pString.CompareTo(pStringFinal) <= 0;
		}

		public static string[] Split(this string s, params int[] lengths)
		{
			List<string> result = new List<string>();
			for (int i = 0; i < lengths.Length; i++)
			{
				result.Add(s.Left(lengths[i]));
				s = s.Right(-lengths[i]);
			}
			return result.ToArray();
		}

		/// <summary>
		/// Transforma a string para iniciais maiúsculas
		/// </summary>
		/// <param name="str">string para ser transformada</param>
		/// <returns>string com iniciais maiúsculas</returns>
		public static string ToPretty(this string str)
		{
			str = str.ToLower();
			string[] vet = str.Split(' ');
			for (int i = 0; i < vet.Length; i++)
			{
				if (vet[i].IsNullOrEmpty() || i != 0
					&& vet[i].In("à", "e", "o", "a", "os", "as",
					"de", "do", "da", "dos", "das",
					"para", "pro", "pra", "pros", "pras",
					"em", "no", "na", "nos", "nas", "por", "p/", "para"))
					continue;
				vet[i] = Char.ToUpper(vet[i][0]) + vet[i].Right(-1);
			}
			return string.Join(" ", vet);
		}

		public static bool Inside(this string str, string value)
		{
			return value.Contains(str);
		}

		public static bool AtBegining(this string str, string value)
		{
			return value.StartsWith(str);
		}

		/// <summary>
		/// Método para internacionalização
		/// </summary>
		/// <param name="s">string a ser internacionalizada</param>
		/// <returns>string internacionalizada</returns>
		private static string i18n(this string s)
		{
			//TODO implementar esta rotina
			for (int i = 1; i < s.Length; i++)
			{
				if (Char.IsUpper(s[i]))
				{
					s = s.Insert(i, " ");
					i++;
				}
			}
			return s.Replace('_', ' ').Replace("  ", " ");
		}

		/// <summary>
		/// Compara as strings desconsiderando acentos e Ç
		/// </summary>
		/// <param name="s1">String a ser comparada</param>
		/// <param name="s2">String a ser comparada</param>
		/// <returns></returns>
		public static bool EqualsIgnoreSymbols(this String s1, String s2)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(s1.RemoveCaracters(' ', '-'), s2.RemoveCaracters(' ', '-'),
				CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreNonSpace) == 0 ? true : false;
		}

		/// <summary>
		/// Searches for the specified substring and returns the zero-based index of
		/// the first occurrence within the section of the source string that extends
		/// from the specified index to the end of the string using the specified System.Globalization.CompareOptions
		/// value.
		/// </summary>
		/// <param name="source">The string to search.</param>
		/// <param name="value">The string to locate within source.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="options">
		/// The System.Globalization.CompareOptions value that defines how source and
		/// value should be compared. options is either the value System.Globalization.CompareOptions.Ordinal
		/// used by itself, or the bitwise combination of one or more of the following
		/// values: System.Globalization.CompareOptions.IgnoreCase, System.Globalization.CompareOptions.IgnoreSymbols,
		/// System.Globalization.CompareOptions.IgnoreNonSpace, System.Globalization.CompareOptions.IgnoreWidth,
		/// and System.Globalization.CompareOptions.IgnoreKanaType.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of value within the section
		/// of source that extends from startIndex to the end of source using the specified
		/// System.Globalization.CompareOptions value, if found; otherwise, -1.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">source is null.-or- value is null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for source.</exception>
		/// <exception cref="System.ArgumentException">options contains an invalid System.Globalization.CompareOptions value.</exception>
		public static int IndexOf(this string source, string value, int startIndex, CompareOptions options)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, value, startIndex, options);
		}

		/// <summary>
		/// Returns a value indicate whether a specified substring ocurrs within this string using CompareOptions: IgnoreCase
		/// </summary>
		/// <param name="source">The string to search</param>
		/// <param name="value">The string to locate within source</param>
		/// <returns>True if the value is within source, false otherwise</returns>
		public static bool ContainsCIAS(this string source, string value)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, value, 0, CompareOptions.IgnoreCase) >= 0;
		}

		/// <summary>
		/// Returns a value indicate whether a specified substring ocurrs within this string using CompareOptions: IgnoreCase | IgnoreNonSpacing
		/// </summary>
		/// <param name="source">The string to search</param>
		/// <param name="value">The string to locate within source</param>
		/// <returns>True if the value is within source, false otherwise</returns>
		public static bool ContainsCIAI(this string source, string value)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, value, 0, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
		}

		/// <summary>
		/// Remove todos os caracteres em branco na string
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <returns>Nova string sem espaços em branco</returns>
		public static string RemoveWhiteSpace(this string str)
		{
			if (!String.IsNullOrEmpty(str))
				return new Regex(@"\s*").Replace(str, string.Empty);
			else
				return str;
		}

		public static string RemoveNonSpacing(this string regularString)
		{
			string normalizedString = regularString.Normalize(NormalizationForm.FormD);

			StringBuilder sb = new StringBuilder(normalizedString);

			for (int i = 0; i < sb.Length; i++)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(sb[i]) == UnicodeCategory.NonSpacingMark)
					sb.Remove(i, 1);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Inverte a ordem dos caracteres de uma string
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <returns>Nova string com caracteres na ordem inversa</returns>
		public static string Reverse(this string str)
		{
			char[] arr = str.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		/// <summary>
		/// Corta a string n caracteres à esquerda
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <param name="length">Comprimento a ser cortado</param>
		/// <returns>Nova string com tamanho n</returns>
		public static string Left(this string str, int length)
		{
			if (length > 0) return str.Length > length ? str.Substring(0, length) : str;
			else return str.Substring(0, str.Length + length);
		}

		/// <summary>
		/// Corta a string n caracteres à direita
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <param name="length">Comprimento a ser cortado</param>
		/// <returns>Nova string com tamanho n</returns>
		public static string Right(this string str, int length)
		{
			if (length > 0) return str.Length > length ? str.Substring(str.Length - length) : str;
			else return str.Substring(-length, str.Length + length);
		}

		/// <summary>
		/// Captura a primeira palavra de uma string
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <returns>Nova string com a primeira palavra</returns>
		public static string GetFirstName(this string str)
		{
			string[] index = str.Trim().Split(' ');
			return index[0];
		}

		/// <summary>
		/// Remove caracteres de uma string
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <param name="caracteres">Lista de caracteres a serem removidos</param>
		/// <returns>Nova string sem os caracteres</returns>
		public static string RemoveCaracters(this string str, params char[] caracteres)
		{
			if (str == null) return str;
			caracteres.ToList().ForEach(c => str = str.Replace(c.ToString(), ""));
			return str;
		}

		public static string RemoveLetras(this string str)
		{
			return str.RemoveCaracters('a'.Range('z').Concat('A'.Range('Z')).ToArray());
		}

		/// <summary>
		/// Filtra somente os dígitos de uma determinada string
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <returns>Nova string contendo somente dígitos</returns>
		public static string GetOnlyNumbers(this string str)
		{
			string s = "";
			for (int i = 0; i < str.Length; i++)
				if (((int)str[i]).Between('0', '9')) s += str[i];
			return s;
		}

		/// <summary>
		/// Limpara caracteres de máscara {'.', ',', '/', '-', '_', ' '}
		/// </summary>
		/// <param name="s">String de origem</param>
		/// <returns>Nova string sem os caracteres de máscara</returns>
		public static string ClearMask(this String s)
		{
			if (s == null) return null;
			return s.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "");
		}

		/// <summary>
		/// Coloca máscara em uma string
		/// </summary>
		/// <param name="s">String de origem</param>
		/// <param name="pMask">Máscara a ser aplicada</param>
		/// <returns>Nova string com a máscara especificada</returns>
		public static string PutMask(this String s, String pMask)
		{
			if (pMask.IsNullOrEmpty() || s.IsNullOrEmpty()) return s;

			MaskedTextProvider lMaskProvider = new MaskedTextProvider(pMask);
			lMaskProvider.Set(s);
			return lMaskProvider.ToDisplayString();
		}

		public static string PutMaskWithCaracter(this String s, String pMask, char pCaracter)
		{
			MaskedTextProvider lMaskProvider = new MaskedTextProvider(pMask);

			lMaskProvider.Set(s.PadLeft(pMask.GetOnlyNumbers().Length, pCaracter));
			return lMaskProvider.ToDisplayString();
		}

		/// <summary>
		/// Coloca máscara em uma string
		/// </summary>
		/// <param name="s">String de origem</param>
		/// <param name="pMask">Máscara a ser aplicada</param>
		/// <returns>Nova string com a máscara especificada</returns>
		public static string PutMask(this String s, Masks pMask)
		{
			return s.PutMask(pMask.ToString());
		}

		/// <summary>
		/// Separa uma string de telefones separados por '/' em uma lista de telefones
		/// </summary>
		/// <param name="str">String de origem</param>
		/// <returns>Lista de Telefones contidos na string</returns>
		public static List<Telefone> CheckPhone(this string str)
		{
			string telefones = str.RemoveCaracters('r', 'a', 'm', 'l', '(', ')');//.Split('/').RemoveAll(s => s == "");
			return new List<Telefone> { telefones };
			//List<Telefone> lTelefones = new List<Telefone>();
			//foreach (string s in telefones)
			//{
			//    lTelefones.Add(Telefone.FromString(s));
			//}
			//return lTelefones;
		}

		/// <summary>
		/// Verifica se uma string é nula ou está vazia
		/// </summary>
		/// <param name="s1">String a ser verificada</param>
		/// <returns>true se a string for nula ou vazia, false caso contrário</returns>
		public static bool IsNullOrEmpty(this String s1)
		{
			return string.IsNullOrWhiteSpace(s1);
		}

		/// <summary>
		/// Verifica se uma string é nula ou está vazia e em caso afirmativo retorna o valor default
		/// </summary>
		/// <param name="s1">String a ser verificada</param>
		/// <param name="pDefault">Valor default</param>
		/// <returns>true se a string for nula ou vazia, false caso contrário</returns>
		public static string DefaultIfEmpty(this String s1, string pDefault)
		{
			return string.IsNullOrWhiteSpace(s1) ? pDefault : s1;
		}


		/// <summary>
		/// Substitui o item de formatação em uma string especificada pela string representada pelo objeto correspondente no array
		/// </summary>
		/// <param name="s1">String de formatação</param>
		/// <param name="args">Os objetos a serem formatados</param>
		/// <returns></returns>
		public static String Formata(this String s1, params object[] args)
		{
			return String.Format(s1, args);
		}

		/// <summary>
		/// Converte uma string em DateTime. Verifica também se é válida para o SQL (entre 01/01/1753 e 31/12/9999)
		/// </summary>
		/// <param name="pData">A representação da data em string</param>
		/// <param name="pNomeCampoData">Nome do Campo da Tela</param>
		/// <returns>Data convertida se a string representar uma data válida</returns>
		public static DateTime ToDateTime(this String pData, String pNomeCampoData)
		{
			DateTime lData;
			try
			{
				lData = Convert.ToDateTime(pData.Trim());
			}
			catch (FormatException ex)
			{
				throw new ErrorException("{0}: \'{1}\' inválida!".Formata(pNomeCampoData ?? "Data", pData), null, ex);
			}

			if (DataSQLValida(lData))
				return lData;
			else
				throw new ErrorException("{0} \'{1}\' inválida!".Formata(pNomeCampoData ?? "Data", pData));
		}

		/// <summary>
		/// Converte uma string em DateTime. Verifica também se é válida para o SQL (entre 01/01/1753 e 31/12/9999)
		/// </summary>
		/// <param name="pData">A representação da data em string</param>
		/// <returns>Data convertida se a string representar uma data válida</returns>
		public static DateTime ToDateTime(this String pData)
		{
			DateTime lData;
			try
			{
				lData = Convert.ToDateTime(pData.Trim());
			}
			catch (FormatException ex)
			{
				throw new ErrorException("{0}: \'{1}\' inválida!".Formata("Data", pData), null, ex);
			}

			if (DataSQLValida(lData))
				return lData;
			else
				throw new ErrorException("{0} \'{1}\' inválida!".Formata("Data", pData));
		}

		/// <summary>
		/// Converte uma string em Nullable DateTime. Verifica também se é válida para o SQL (entre 01/01/1753 e 31/12/9999)
		/// </summary>
		/// <param name="pData">A representação da data em string</param>
		/// <param name="pNomeCampoData">Nome do Campo da Tela</param>
		/// <returns>Null se a string for vazia, Data convertida se a string representar uma data válida</returns>
		public static DateTime? ToNullableDateTime(this String pData, String pNomeCampoData = null)
		{
			if (pData.Trim().Equals("")) return null;
			else return pData.ToDateTime(pNomeCampoData);
		}

		/// <summary>
		/// Converte uma string representativa de um número em um inteiro equivalente
		/// </summary>
		/// <param name="pInt">String representativa</param>
		/// <returns>Inteiro representado pela string, 0 se string vazia.</returns>
		/// <exception cref="BusinessException">String não representa um inteiro válido.</exception>
		public static Int32 ToInt32(this String pInt)
		{
			if (pInt.IsNullOrEmpty()) return 0;
			try { return Int32.Parse(pInt); }
			catch (FormatException ex) { throw new ErrorException(String.Format("\'{0}\' não é um inteiro válido.", pInt), null, ex); }
		}

		/// <summary>
		/// Converte uma string representativa de um número em um inteiro equivalente
		/// </summary>
		/// <param name="pInt">String representativa</param>
		/// <returns>Inteiro representado pela string, 0 se string vazia.</returns>
		/// <exception cref="BusinessException">String não representa um inteiro válido.</exception>
		public static long ToInt64(this String pInt)
		{
			if (pInt.IsNullOrEmpty()) return 0;
			try { return long.Parse(pInt); }
			catch (FormatException ex) { throw new ErrorException(String.Format("\'{0}\' não é um inteiro válido."), null, ex); }
		}

		/// <summary>
		/// Converte uma string representativa de um número em um decimal equivalente
		/// </summary>
		/// <param name="pDecimal">String representativa</param>
		/// <returns>Decimal representado pela string, 0 se string vazia.</returns>
		/// <exception cref="BusinessException">String não representa um decimal válido.</exception>
		public static Decimal ToDecimal(this String pDecimal)
		{
			if (pDecimal.Trim().Equals("")) return 0;
			try { return Decimal.Parse(pDecimal); }
			catch (FormatException ex) { throw new ErrorException(String.Format("\'{0}\' não é um número válido.", pDecimal), null, ex); }
		}

		/// <summary>
		/// Converte uma string representativa de um GUID em uma struct System.Guid equivalente
		/// </summary>
		/// <param name="pGuid">String representativa</param>
		/// <returns>System.Guid representado pela string.</returns>
		/// <exception cref="BusinessException">String não representa um GUID válido.</exception>
		public static Guid ToGuid(this String pGuid)
		{
			if (pGuid.Trim().Equals("")) return Guid.Empty;
			try { return Guid.Parse(pGuid); }
			catch (FormatException ex) { throw new ErrorException(String.Format("\'{0}\' não é um Guid válido.", pGuid), null, ex); }
		}

		public static bool ToBoolean(this string pBool)
		{
			return Convert.ToBoolean(pBool);
		}

		public static String InverterData(this String pData)
		{
			string[] lArrayString = new string[3];

			lArrayString[0] = pData.Substring(6, 2);
			lArrayString[1] = pData.Substring(4, 2);
			lArrayString[2] = pData.Substring(0, 4);

			string lDataInvertida = "";
			for (int i = 0; i < lArrayString.Length; i++)
			{
				if (!lDataInvertida.IsNullOrEmpty())
					lDataInvertida += "/";
				lDataInvertida += lArrayString[i];
			}
			return lDataInvertida;
		}

		public static bool MaiorIgual(this string pString, string pString2)
		{
			return pString.CompareTo(pString2) >= 0;
		}

		public static bool Maior(this string pString, string pString2)
		{
			return pString.CompareTo(pString2) > 0;
		}

		public static bool MenorIgual(this string pString, string pString2)
		{
			return pString.CompareTo(pString2) <= 0;
		}

		public static bool Menor(this string pString, string pString2)
		{
			return pString.CompareTo(pString2) < 0;
		}

		public static string PadRight(this string pString, int totalWidth, string paddingString)
		{
			while (pString.Length < totalWidth)
				pString += paddingString;
			return pString.Left(totalWidth);
		}

		public static string PadLeft(this string pString, int totalWidth, string paddingString)
		{
			while (pString.Length < totalWidth)
				pString = paddingString + pString;
			return pString.Right(totalWidth);
		}

		#endregion

		#region Extensions for Char

		/// <summary>
		/// Converte um Charactere representativo de um número em um inteiro equivalente
		/// </summary>
		/// <param name="chr">Charactere representativo</param>
		/// <returns>Inteiro representado pelo Charactere</returns>
		public static Int32 ToInt32(this char chr)
		{
			return chr.ToString().ToInt32();
		}

		public static IEnumerable<char> Range(this char pStart, char pEnd)
		{
			return Enumerable.Range(pStart, pEnd - pStart + 1).Select(x => (char)x);
		}

		#endregion

		#region Extensions for Boolean

		/// <summary>
		/// Converte o booleano especificado para o inteiro equivalente
		/// </summary>
		/// <param name="pBool">Boolean a ser convertido</param>
		/// <returns>0 se booleano é false, 1 se booleano é true</returns>
		public static Int32 ToInt32(this Boolean pBool)
		{
			return Convert.ToInt32(pBool);
		}

		public static string ToString(this bool pBool, string pTrueString, string pFalseString)
		{
			return pBool ? pTrueString : pFalseString;
		}

		#endregion

		#region Extensions for DateTime

		public static int GetTrimestre(this DateTime pData)
		{
			return (int)Math.Ceiling(pData.Month / 3d);
		}

		/// <summary>
		/// Retorna a Idade de acordo com a Data de Nascimento
		/// </summary>
		/// <param name="pDateTime">Data de Nascimento</param>
		/// <param name="pDataReferencia">Data de Referência</param>
		/// <returns>A idade em inteiro</returns>
		public static int GetIdade(this DateTime pDateTime, DateTime? pDataReferencia = null)
		{
			DateTime lData = (pDataReferencia ?? DateTime.Today);
			int idade = lData.Year - pDateTime.Year;
			if (lData.Month < pDateTime.Month || (lData.Month == pDateTime.Month && pDateTime.Day > lData.Day))
				idade--;
			return idade;
		}

		/// <summary>
		/// Verifica se determinada data está limitada aos valores máximo e mínimo do sql
		/// </summary>
		/// <param name="pDateTime">Data a ser verificada</param>
		/// <returns>true se a data está limitada, false caso contrário</returns>
		private static bool DataSQLValida(this DateTime pDateTime)
		{
			return (pDateTime >= System.Data.SqlTypes.SqlDateTime.MinValue.Value
				&& pDateTime <= System.Data.SqlTypes.SqlDateTime.MaxValue.Value);
		}

		public static DateTime AddWeeks(this DateTime pDateTime, int weeks)
		{
			return pDateTime.AddDays(weeks * 7);
		}

		/// <summary>
		/// Calcula a data que representa o início do mês da data de referência
		/// </summary>
		/// <param name="pDateTime">Data de referência</param>
		/// <returns>DateTime representando o início do mês</returns>
		public static DateTime MonthStart(this DateTime pDateTime)
		{
			return new DateTime(pDateTime.Year, pDateTime.Month, 1);
		}

		/// <summary>
		/// Calcula a data que representa o fim do mês da data de referência
		/// </summary>
		/// <param name="pDateTime">Data de referência</param>
		/// <returns>DateTime representando o fim do mês</returns>
		public static DateTime MonthEnd(this DateTime pDateTime)
		{
			return new DateTime(pDateTime.Year, pDateTime.Month, DateTime.DaysInMonth(pDateTime.Year, pDateTime.Month));
		}

		/// <summary>
		/// Calcula a data que representa o início do ano da data de referência
		/// </summary>
		/// <param name="pDateTime">Data de referência</param>
		/// <returns>DateTime representando o início do ano</returns>
		public static DateTime YearStart(this DateTime pDateTime)
		{
			return new DateTime(pDateTime.Year, 1, 1);
		}

		/// <summary>
		/// Calcula a data que representa o fim do ano da data de referência
		/// </summary>
		/// <param name="pDateTime">Data de referência</param>
		/// <returns>DateTime representando o fim do ano</returns>
		public static DateTime YearEnd(this DateTime pDateTime)
		{
			return new DateTime(pDateTime.Year, 12, 31);
		}

		public static bool MonthEquals(this DateTime pDateTime, int pMes)
		{
			return pDateTime.Month == pMes;
		}

		public static bool MonthBetween(this DateTime pDateTime, int pMesInicial, int pMesFinal)
		{
			return pDateTime.Month.Between(pMesInicial, pMesFinal);
		}

		public static bool DayBetween(this DateTime pDateTime, int pDiaInicial, int pDiaFinal)
		{
			return pDateTime.Day.Between(pDiaInicial, pDiaFinal);
		}

		public static bool YearEquals(this DateTime pDateTime, int pAno)
		{
			return pDateTime.Year == pAno;
		}

		/// <summary>
		/// Cálcula a diferença em dias entre duas datas
		/// </summary>
		/// <param name="pData1">Data inicial</param>
		/// <param name="pData2">Data final</param>
		/// <param name="pMode">Metodologia de cálculo</param>
		/// <param name="pFeriados">Lista de feriados</param>
		/// <returns>Diferença em dias entre a data inicial e final 
		/// positivo se data inicial menor que data final
		/// negativo se data inicial maior que data final</returns>
		public static int TotalDays(this DateTime lData1, DateTime lData2, EModeDiffDate pMode, params DateTime[] pFeriados)
		{
			DateTime pData1 = lData1, pData2 = lData2;
			if (pData1 > pData2)
			{
				DateTime aux = pData1;
				pData1 = pData2;
				pData2 = aux;
			}
			int result;
			switch (pMode)
			{
				case EModeDiffDate.DiasCorridos:
					result = (int)(pData2 - pData1).TotalDays;
					break;
				case EModeDiffDate.DiasÚteis:
					{
						result = (int)(pData2 - pData1).TotalDays;
						DateTime aux = pData2.AddDays((int)-result % 7);
						result -= (int)(result / 7) * 2;
						while (aux <= pData2)
						{
							if (aux.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday) && aux != pData1)
								result--;
							aux = aux.AddDays(1);
						}
						if (pFeriados != null)
							result -= pFeriados.Where(d => !d.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday) && d.Between(pData1.AddDays(1), pData2)).Count();
						break;
					}
				case EModeDiffDate.MêsComercial:
					result = Dias360(pData1, pData2);
					break;
				case (EModeDiffDate)4:
					{
						for (result = 0; pData1 < pData2; result++)
						{
							pData1 = pData1.AddDays(1);
							if (pData1.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday) || (pFeriados != null && pFeriados.Contains(pData1)))
								result--;
						}
						break;
					}
				default: throw new Exception("Mode not identified!");
			}
			if (lData2 < lData1) result = -result;
			return result;
		}

		/// <summary>
		/// Returns an indication whether the specified year is a leap year.
		/// </summary>
		/// <param name="pDateTime">Data de referência</param>
		/// <returns>true if year is a leap year; otherwise, false.</returns>
		public static bool IsLeapYear(this DateTime pDateTime)
		{
			return DateTime.IsLeapYear(pDateTime.Year);
		}

		private static int Dias360(DateTime dti, DateTime dtf)
		{
			int diff = 0;

			if (dti.Day == 28 && dti.Month == 2 && !dti.IsLeapYear()) diff = -2;
			if (dti.Day == 29 && dti.Month == 2 && dti.IsLeapYear()) diff = -1;

			if (dtf.Day == 28 && dtf.Month == 2 && !dtf.IsLeapYear()) diff += 2;
			if (dtf.Day == 29 && dtf.Month == 2 && dtf.IsLeapYear()) diff += 1;

			return (dtf.Year - dti.Year) * 360
				+ (dtf.Month - dti.Month) * 30
				+ Math.Min(dtf.Day, 30)
				- Math.Min(dti.Day, 30)
				+ diff;
		}

		public static DateTime AddWorkDays(this DateTime pData, int days, params DateTime[] feriados)
		{
			DateTime result = pData.AddDays(days);
			if (days != 0)
			{
				int inc = days / Math.Abs(days);
				while (pData.TotalDays(result, EModeDiffDate.DiasÚteis, feriados) < days)
					result = result.AddDays(inc);
			}
			return result;
		}

		/// <summary>
		/// Verifica se determinada data está compreendida em um determinado intervalo
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <param name="pDataMin">Data mínima do intervalo</param>
		/// <param name="pDataMax">Data máxima do intervalo</param>
		/// <returns></returns>
		public static bool Between(this DateTime pDataRef, DateTime pDataMin, DateTime pDataMax)
		{
			if (pDataRef == null) throw new ArgumentNullException("pDataRef");
			if (pDataMin == null) throw new ArgumentNullException("pDataMin");
			if (pDataMax == null) throw new ArgumentNullException("pDataMax");
			return pDataRef >= pDataMin && pDataRef <= pDataMax;
		}

		/// <summary>
		/// Calcula a quantidade de dias no mês
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <param name="pMode">Metodologia de cálculo</param>
		/// <param name="pFeriados">Lista de feriados</param>
		/// <returns>Inteiro com a quantidade de dias</returns>
		public static int DiasMes(this DateTime pDataRef, EModeDiffDate pMode, params DateTime[] pFeriados)
		{
			if (pDataRef == null) throw new ArgumentNullException("pDataRef");
			return pDataRef.MonthStart().AddDays(-1).TotalDays(pDataRef.MonthEnd(), pMode, pFeriados);
		}

		/// <summary>
		/// Calcula a quantidade de dias no ano
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <param name="pMode">Metodologia de cálculo</param>
		/// <param name="pFeriados">Lista de feriados</param>
		/// <returns>Inteiro com a quantidade de dias</returns>
		public static int DiasAno(this DateTime pDataRef, EModeDiffDate pMode, params DateTime[] pFeriados)
		{
			if (pDataRef == null) throw new ArgumentNullException("pDataRef");
			return new DateTime(pDataRef.Year, 1, 1).AddDays(-1).TotalDays(new DateTime(pDataRef.Year, 12, 31), pMode, pFeriados);
		}

		/// <summary>
		/// Monta um inteiro no formato AAAAMM
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <returns>Inteiro no formato AAAAMM</returns>
		public static int AnoMes(this DateTime pDataRef)
		{
			if (pDataRef == null) throw new ArgumentNullException("pDataRef");
			return pDataRef.Year * 100 + pDataRef.Month;
		}

		/// <summary>
		/// Monta um inteiro no formato MMDD
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <returns>Inteiro no formato AAAAMM</returns>
		public static int DiaMes(this DateTime pDataRef)
		{
			if (pDataRef == null) throw new ArgumentNullException("pDataRef");
			return pDataRef.Month * 100 + pDataRef.Day;
		}

		public static DateTime Max(this DateTime d1, DateTime d2)
		{
			return d1 > d2 ? d1 : d2;
		}

		public static DateTime? Min(this DateTime? d1, DateTime? d2)
		{
			return (d1 ?? DateTime.MaxValue) > (d2 ?? DateTime.MaxValue) ? d2 : d1;
		}

		public static DateTime Min(this DateTime d1, DateTime d2)
		{
			return d1 > d2 ? d2 : d1;
		}

		/// <summary>
		/// Converte o valor do objeto System.DateTime para a sua representação em string
		/// </summary>
		/// <param name="pDateTime">Data a ser convertida</param>
		/// <returns>String com a representação da data</returns>
		public static String ToShortDateString(this DateTime? pDateTime)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.ToShortDateString();
			else
				return pDateTime.ToString();
		}

		/// <summary>
		/// Adiciona n anos a uma data
		/// </summary>
		/// <param name="pDateTime">Data de origem</param>
		/// <param name="pValue">Quantidade de anos a serem adicionados</param>
		/// <returns>Nova data com os anos adicionados</returns>
		public static DateTime? AddYears(this DateTime? pDateTime, int pValue)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.AddYears(pValue);
			else
				return null;
		}

		/// <summary>
		/// Adiciona n meses a uma data
		/// </summary>
		/// <param name="pDateTime">Data de origem</param>
		/// <param name="pValue">Quantidade de meses a serem adicionados</param>
		/// <returns>Nova data com os meses adicionados</returns>
		public static DateTime? AddMonths(this DateTime? pDateTime, int pValue)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.AddMonths(pValue);
			else
				return null;
		}

		/// <summary>
		/// Verifica se determinada data está compreendida em um determinado intervalo
		/// </summary>
		/// <param name="pDataRef">Data de referência</param>
		/// <param name="pDataMin">Data mínima do intervalo</param>
		/// <param name="pDataMax">Data máxima do intervalo</param>
		/// <returns></returns>
		public static bool Between(this DateTime? pDataRef, DateTime pDataMin, DateTime pDataMax)
		{
			if (!pDataRef.HasValue) throw new NullReferenceException();
			if (pDataMin == null) throw new ArgumentNullException("pDataMin");
			if (pDataMax == null) throw new ArgumentNullException("pDataMax");
			return pDataRef.Value >= pDataMin && pDataRef <= pDataMax;
		}

		public static bool MonthEquals(this DateTime? pDateTime, int pMes)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.Month == pMes;
			return false;
		}

		public static bool YearEquals(this DateTime? pDateTime, int pAno)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.Year == pAno;
			return false;
		}

		public static bool MonthBetween(this DateTime? pDateTime, int pMesInicial, int pMesFinal)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.Month.Between(pMesInicial, pMesFinal);
			return false;
		}

		public static bool DayBetween(this DateTime? pDateTime, int pDiaInicial, int pDiaFinal)
		{
			if (pDateTime.HasValue)
				return pDateTime.Value.Day.Between(pDiaInicial, pDiaFinal);
			return false;
		}

		/// <summary>
		/// Verifica se o dia informado é útil.
		/// Dia útil é qualquer dia que não seja sábado, domingo, ou feriado.
		/// </summary>
		/// <param name="pDateToTest"></param>
		/// <param name="pHolidays">Lista de Feriados</param>
		/// <returns>True se pDateToTest é dia útil, false caso contrário</returns>
		public static bool IsWorkDay(this DateTime pDateToTest, params DateTime[] pHolidays)
		{
			return !pDateToTest.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday) && !pHolidays.Contains(pDateToTest.Date);
		}

		/// <summary>
		/// Retorna o próximo dia útil a partir da data informada.
		/// </summary>
		/// <param name="pDateToAdd"></param>
		/// <param name="pDaysToAdd"></param>
		/// <param name="pHolidays"></param>
		/// <returns></returns>
		public static DateTime NextWorkDay(this DateTime pDateToAdd, int pDaysToAdd = 1, params DateTime[] pHolidays)
		{
			for (int totalDias = 0; totalDias < pDaysToAdd; totalDias++)
			{
				for (;;)
				{
					pDateToAdd = pDateToAdd.AddDays(1);
					if (pDateToAdd.IsWorkDay(pHolidays)) break;
				}
			}
			return pDateToAdd;
		}

		/// <summary>
		/// Retorna o dia útil anterior a partir da data informada.
		/// </summary>
		/// <param name="pDateToDecrease"></param>
		/// <param name="pDaysToDecrease"></param>
		/// <param name="pHolidays"></param>
		/// <returns></returns>
		public static DateTime PreviousWorkDay(this DateTime pDateToDecrease, int pDaysToDecrease = 1, params DateTime[] pHolidays)
		{
			for (int totalDias = 0; totalDias < pDaysToDecrease; totalDias++)
			{
				for (;;)
				{
					pDateToDecrease = pDateToDecrease.AddDays(-1);
					if (IsWorkDay(pDateToDecrease, pHolidays)) break;
				}
			}
			return pDateToDecrease;
		}

		/// <summary>
		/// Calcula a quantidade de dias úteis entre duas datas.
		/// </summary>
		/// <param name="pStart">Data Inicial</param>
		/// <param name="pEnd">Data Final</param>
		/// <param name="pHolidays">Lista de feriados</param>
		/// <returns>Quantidade de dias úteis entre pStart e pEnd</returns>
		public static int CountWorkDays(this DateTime pStart, DateTime pEnd, params DateTime[] pHolidays)
		{
			if (pEnd < pStart)
			{
				var temp = pEnd;
				pEnd = pStart;
				pStart = pEnd;
			}
			// sempre assumimos a conta a partir do primeiro dia útil a partir da data inicial.
			pStart = ProximoDiaUtilSeCorrenteNaoUtil(pStart, pHolidays);
			if (pStart > pEnd)
				return 0;
			// total de feriados compreendidos entre as 2 datas, e que caiam em dias da semana exceto sábado e domingo.
			int totFeriados = pHolidays.Count(d => !d.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday) && (d >= pStart && d <= pEnd));
			// total de dias corridos entre as datas.
			int totDiasCorridos = (pEnd - pStart).Days;
			// total de semanas cheias entre as datas. A saber: para cada semana, temos 1 sábado e 1 domingo, a subtrair do total de dias corridos.
			int totSemanas = ((int)Math.Floor((double)(totDiasCorridos / 7))) + ((pStart.DayOfWeek > pEnd.DayOfWeek) ? 1 : 0);
			// fórmula final
			return totDiasCorridos - totFeriados - (totSemanas * 2) - ((pEnd.DayOfWeek == DayOfWeek.Saturday) ? 1 : 0); // sábado é o fim da semana de 7 dias.
		}

		/// <summary>
		/// Verifica se o dia passado como parâmetro é útil. Se não for, retorna o próximo dia útil.
		/// </summary>
		/// <param name="dtVerificar"></param>
		/// <param name="pHolidays"></param>
		/// <returns></returns>
		public static DateTime ProximoDiaUtilSeCorrenteNaoUtil(this DateTime dtVerificar, params DateTime[] pHolidays)
		{
			return !dtVerificar.IsWorkDay(pHolidays) ? dtVerificar.NextWorkDay(pHolidays: pHolidays) : dtVerificar;
		}

		/// <summary>
		/// Verifica se o dia passado como parâmetro é útil. Se não for, retorna o dia útil anterior.
		/// </summary>
		/// <param name="dtVerificar"></param>
		/// <param name="pHolidays"></param>
		/// <returns></returns>
		public static DateTime DiaUtilAnteriorSeCorrenteNaoUtil(this DateTime dtVerificar, params DateTime[] pHolidays)
		{
			return !dtVerificar.IsWorkDay(pHolidays) ? dtVerificar.PreviousWorkDay(pHolidays: pHolidays) : dtVerificar;
		}

		public static String DiaMesAnoPorExtenso(this DateTime pData)
		{
			string dia = ((decimal)pData.Day).GetExtensoFmt("dia", "dias", "", "");
			return "{1} do mês de {0:MMMM} de {0:yyyy}".Formata(pData, dia);
		}

		public static String MesAnoPorExtenso(this DateTime pData)
		{
			return "{0:MMMM} de {0:yyyy}".Formata(pData);
		}

		public static IEnumerable<DateTime> Range(this DateTime pStart, DateTime pEnd)
		{
			return Enumerable.Range(0, (pEnd.Date - pStart.Date).TotalDays.ToInt32() + 1).Select(x => pStart.AddDays(x));
		}

		#endregion

		#region Extensions for Predicate

		/// <summary>
		/// Mescla duas expressões com operação de Or
		/// </summary>
		/// <typeparam name="T">O tipo da expressão</typeparam>
		/// <param name="f1">Expressão da esquerda</param>
		/// <param name="f2">Espressão da direita</param>
		/// <returns>Nova expressão mesclada</returns>
		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> f1,
														Expression<Func<T, bool>> f2)
		{
			if (f1 == null) return f2;
			if (f2 == null) return f1;
			// create a new expression tree that replace the argument of the second expression with the argument on the first one
			LambdaExpression lambdaExpression = RebuildExpressionIfNeeded(f1.Parameters[0], f2);
			return Expression.Lambda<Func<T, bool>>(
				Expression.OrElse(f1.Body, lambdaExpression.Body),
				f1.Parameters[0]);
		}

		/// <summary>
		/// Mescla duas expressões com operação de And
		/// </summary>
		/// <typeparam name="T">O tipo da expressão</typeparam>
		/// <param name="f1">Expressão da esquerda</param>
		/// <param name="f2">Espressão da direita</param>
		/// <returns>Nova expressão mesclada</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> f1,
															 Expression<Func<T, bool>> f2)
		{
			if (f1 == null) return f2;
			if (f2 == null) return f1;
			// create a new expression tree that replace the argument of the second expression with the argument on the first one
			LambdaExpression lambdaExpression = RebuildExpressionIfNeeded(f1.Parameters[0], f2);
			return Expression.Lambda<Func<T, bool>>(
				Expression.AndAlso(f1.Body, lambdaExpression.Body),
				f1.Parameters[0]);
		}

		public static Expression<Func<TResult, bool>> Cast<T, TResult>(this Expression<Func<T, bool>> f1) where TResult : T
		{
			if (f1 == null) return null;
			return Expression.Lambda<Func<TResult, bool>>(f1.Body, f1.Parameters);
		}

		/// <summary>
		/// Reconstrói a espressão mudando o nome do parâmetro
		/// </summary>
		/// <param name="p">O novo nome do parâmetro</param>
		/// <param name="expr2">Expressão a ser modificada</param>
		/// <returns></returns>
		private static LambdaExpression RebuildExpressionIfNeeded<T>(ParameterExpression p, Expression<Func<T, bool>> expr2)
		{
			LambdaExpression lambdaExpression;
			Expression expression = (new ParameterModifier()).Modify(expr2, p);
			lambdaExpression = ((LambdaExpression)expression);
			return lambdaExpression;
		}

		private class ParameterModifier : ExpressionVisitor
		{
			public Expression Modify(Expression expression, ParameterExpression newParam)
			{
				_newParam = newParam;
				return Visit(expression);
			}

			private ParameterExpression _newParam;

			protected override Expression VisitParameter(ParameterExpression p)
			{
				if (p.Type == _newParam.Type)
					return _newParam;
				return p;
			}
		}

		#endregion

		#region Extensions for Object

		public static T Clone<T>(this T obj) where T : class
		{
			MethodInfo m = typeof(object).GetMethod("MemberwiseClone", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
			return (T)m.Invoke(obj, null);
		}

		/// <summary>
		/// Captura o nome da propriedade dentro da qual este método foi chamado
		/// </summary>
		/// <param name="pObject"></param>
		/// <returns>O nome da propriedade que chamou o método</returns>
		public static string GetPropertyName<TSource, TResult>(this TSource pObject, Expression<Func<TSource, TResult>> pExpression)
		{
			UnaryExpression lUnaryExpression = pExpression.Body as UnaryExpression;
			MemberExpression lMemberExpression = pExpression.Body as MemberExpression;
			if (lMemberExpression == null && lUnaryExpression != null)
				lMemberExpression = lUnaryExpression.Operand as MemberExpression;
			if (lMemberExpression == null) return "";
			Expression<Func<TSource, object>> f = (Expression<Func<TSource, object>>)Expression<Func<TSource, object>>.Lambda(
					typeof(Func<TSource, object>),
					Expression<Func<TSource, object>>.Convert(lMemberExpression.Expression, typeof(object)),
					pExpression.Parameters.ToArray());
			string p = GetPropertyName<TSource, object>(pObject, f);
			if (p != "") return p + "." + lMemberExpression.Member.Name;
			else return lMemberExpression.Member.Name;
		}

		public static PropertyInfo GetProperty<TSource, TResult>(this Type pObject, Expression<Func<TSource, TResult>> pExpression)
		{
			UnaryExpression lUnaryExpression = pExpression.Body as UnaryExpression;
			MemberExpression lMemberExpression = pExpression.Body as MemberExpression;
			if (lMemberExpression == null && lUnaryExpression != null)
				lMemberExpression = lUnaryExpression.Operand as MemberExpression;
			if (lMemberExpression == null) throw new InvalidOperationException("pExpression must be a member expression");
			return pObject.GetProperty(lMemberExpression.Member.Name);
		}

		/// <summary>
		/// Verifica se determinado conjunto contém determinado objeto
		/// </summary>
		/// <param name="pObjects">Conjunto de objetos</param>
		/// <param name="pObject">Objeto</param>
		/// <returns>true se o objeto estiver contido no conjunto, false caso contrário</returns>
		public static bool Contains(this object[] pObjects, object pObject)
		{
			return pObjects.ToList().Contains(pObject);
		}

		/// <summary>
		/// Verifica quando um objeto T está contido em determinado conjunto
		/// </summary>
		/// <typeparam name="T">Tipo do objeto</typeparam>
		/// <param name="obj">Objeto a ser pesquisado</param>
		/// <param name="values">Conjunto de objetos</param>
		/// <returns>true se o conjunto contém o objeto, false caso contrário</returns>
		public static bool In<T>(this T obj, params T[] values)
		{
			if (null == obj) throw new ArgumentNullException("source");
			return values.Contains(obj);
		}

		/// <summary>
		/// Verifica quando um objeto T está contido em determinado conjunto
		/// </summary>
		/// <typeparam name="T">Tipo do objeto</typeparam>
		/// <param name="obj">Objeto a ser pesquisado</param>
		/// <param name="values">Conjunto de objetos</param>
		/// <returns>true se o conjunto contém o objeto, false caso contrário</returns>
		public static bool In<T>(this T obj, IEnumerable<T> values)
		{
			if (null == obj) throw new ArgumentNullException("source");
			return values.Contains(obj);
		}

		public static bool In<T>(this T obj, IQueryable<T> values)
		{
			if (null == obj) throw new ArgumentNullException("source");
			return values.Contains(obj);
		}

		public static int ToInt32<T>(this T pInt) where T : struct
		{
			return Convert.ToInt32(pInt);
		}

		public static long ToInt64<T>(this T pInt) where T : struct
		{
			return Convert.ToInt64(pInt);
		}

		public static decimal ToDecimal<T>(this T pValue) where T : struct
		{
			return Convert.ToDecimal(pValue);
		}

		/// <summary>
		/// Verifica se determinado objeto é nulo antes de acessar a propriedade dele através de uma Expression
		/// </summary>
		/// <typeparam name="T">Tipo do objeto</typeparam>
		/// <typeparam name="TResult">Tipo do campo que se deseja acessar</typeparam>
		/// <param name="obj">Objeto a ser acessado</param>
		/// <param name="expression">Expression que acessará o campo do objeto que poderá ou não ser nulo</param>
		/// <returns>Se o objeto não for nulo, o valor da propriedade acessada será retornado normalmente. Se o objeto for nulo, será retornado o valor Default da propriedade que se deseja acessar.</returns>
		public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> expression)
			where TResult : class
		{
			if (obj == null) return null;
			return expression(obj);
		}

		public static TResult? IfNotNullStruct<T, TResult>(this T obj, Func<T, TResult> expression)
			where TResult : struct
		{
			if (obj == null) return null;
			return expression(obj);
		}

		/// <summary>
		/// Verifica se determinado objeto é nulo antes de acessar a propriedade dele através de uma Expression
		/// </summary>
		/// <typeparam name="T">Tipo do objeto</typeparam>
		/// <typeparam name="TResult">Tipo do campo que se deseja acessar</typeparam>
		/// <param name="obj">Objeto a ser acessado</param>
		/// <param name="expression">Expression que acessará o campo do objeto que poderá ou não ser nulo</param>
		/// <param name="defaultValue">Valor que deve ser retornado caso o objeto seja nulo</param>
		/// <returns>Se o objeto não for nulo, o valor da propriedade acessada será retornado normalmente. Se o objeto for nulo, será retornado o valor passado por parâmetro.</returns>
		public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> expression, TResult defaultValue)
		{
			if (obj == null) return defaultValue;
			return expression(obj);
		}

		public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> expression, Func<TResult> defaultValue)
		{
			if (obj == null) return defaultValue();
			return expression(obj);
		}

		/// <summary>
		/// Verifica se determinado objeto é nulo antes de acessar a propriedade dele através de uma Expression
		/// </summary>
		/// <typeparam name="T">Tipo do objeto</typeparam>
		/// <typeparam name="TResult">Tipo do campo que se deseja acessar</typeparam>
		/// <param name="obj">Objeto a ser acessado</param>
		/// <param name="expression">Expression que acessará o campo do objeto que poderá ou não ser nulo</param>
		public static void IfNotNull<T>(this T obj, Action<T> expression)
		{
			if (obj != null)
				expression(obj);
		}

		#endregion

		#region Extensions for Enum

		/// <summary>
		/// Verifica se o valor de determinada variável está definida no Enum do seu tipo
		/// </summary>
		/// <typeparam name="T">O tipo da variável</typeparam>
		/// <param name="pEnum">A variável a ser testada</param>
		/// <returns>true se o valor da variável estiver definida no Enum, false caso contrário</returns>
		public static bool IsDefined(this Enum pEnum)
		{
			return Enum.IsDefined(pEnum.GetType(), pEnum);
		}

		/// <summary>
		/// Verifica se o valor de determinada variável esta definida em um enum do que possua o atributo FLAG
		/// </summary>
		/// <param name="pEnum">O enum com um ou mais valores emm sua composição</param>
		/// <returns>true se o valor da variável ou de sua composição estiver definida no Enum, false caso contrário</returns>
		public static bool IsDefinedFlag(this Enum pEnum)
		{
			bool isDefined = false;
			isDefined = Enum.IsDefined(pEnum.GetType(), pEnum);

			//se ja estiver definido OU não for flag OU for um enum zerado
			if (isDefined || !HasFlags(pEnum) || pEnum.ToInt32() == 0) return isDefined;

			dynamic lEnum = pEnum;
			var lAll = Enum.GetValues(pEnum.GetType()).Cast<dynamic>().Aggregate((e1, e2) => e1 | e2);
			return (lAll & lEnum) == lEnum;
		}

		/// <summary>
		/// Transforma um enum FLAG em um List contendo os enums contidos no parâmetro.
		/// </summary>
		/// <typeparam name="T">Tipo do Enum</typeparam>
		/// <param name="pEnum">Enum que possui o atributo Flag</param>
		/// <returns>Retorna um List contendo os enums contidos em pEnum</returns>
		public static List<T> FlagsToList<T>(this Enum pEnum) where T : struct
		{
			List<T> lReturn = new List<T>();
			if (HasFlags(pEnum) && IsDefinedFlag(pEnum))
			{
				dynamic lEnum = pEnum;
				foreach (var enu in Enum.GetValues(pEnum.GetType()).Cast<T>())
				{
					if ((lEnum & enu) == enu) lReturn.Add(enu);
				}
			}
			return lReturn;
		}

		public static bool IsSingleFlag(this Enum pValor)
		{
			long n = Convert.ToInt64(pValor);
			return n != 0 && (n & (n - 1)) == 0;
		}

		public static bool HasAnyFlag<T>(this T pValor, T pValor2) where T : struct
		{
			return (Convert.ToInt64(pValor) & Convert.ToInt64(pValor2)) > 0;
		}

		/// <summary>
		/// Verifica se um Enum possui o atributo "Flag"
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool HasFlags(this Enum value)
		{
			return value.GetType().GetCustomAttributes(typeof(FlagsAttribute),
				false).Length > 0;
		}

		/// <summary>
		/// Converte uma variável enum para o seu respectivo valor inteiro
		/// </summary>
		/// <typeparam name="T">O tipo do enum</typeparam>
		/// <param name="pEnum">A variável enum a ser convertida</param>
		/// <returns>Inteiro correspondente ao valor do enum</returns>
		public static Int32 ToInt32(this Enum pEnum)
		{
			return Convert.ToInt32(pEnum);
		}

		public static string GetFriendlyName(this Enum pEnum)
		{
			if (pEnum.IsDefined())
			{
				FieldInfo lFI = pEnum.GetType().GetField(pEnum.ToString());
				FriendlyNameAttribute lAtt = (FriendlyNameAttribute)lFI.GetCustomAttributes(typeof(FriendlyNameAttribute), false).FirstOrDefault();
				return lAtt != null ? lAtt.NameSingle : pEnum.ToString().i18n();
			}
			else if (pEnum.GetType().GetCustomAttributes(typeof(FlagsAttribute), false) != null && pEnum.ToInt32() != 0)
				return pEnum.ToString().Split(',').Select(x => (Enum.Parse(pEnum.GetType(), x) as Enum).GetFriendlyName()).StringJoin(", ");
			else return "";
		}

		public static Dictionary<string, string> GetKeysAndValues(this Type pEnum)
		{
			return Enum.GetNames(pEnum).ToDictionary(x => x, x => (Enum.Parse(pEnum, x) as Enum).GetFriendlyName());
		}

		#endregion

		#region Extensions for int

		public static bool Between(this int pInt, int pMinValue, int pMaxValue)
		{
			return pInt >= pMinValue && pInt <= pMaxValue;
		}

		public static bool Between(this uint pInt, uint pMinValue, uint pMaxValue)
		{
			return pInt >= pMinValue && pInt <= pMaxValue;
		}

		public static IEnumerable<int> Range(this int pStart, int pEnd)
		{
			return Enumerable.Range(pStart, pEnd - pStart + 1);
		}

		public static IEnumerable<int> RangeCount(this int pStart, int pCount)
		{
			return Enumerable.Range(pStart, pCount);
		}


		#endregion

		#region Extensions for Decimal
		public static bool Between(this decimal pDecimal, decimal pMinValue, decimal pMaxValue)
		{
			return pDecimal >= pMinValue && pDecimal <= pMaxValue;
		}

		public static decimal RoundAwayFromZero(this decimal pDecimal)
		{
			return Math.Round(pDecimal, MidpointRounding.AwayFromZero);
		}

		public static decimal RoundAwayFromZero(this decimal pDecimal, int pDecimals)
		{
			return Math.Round(pDecimal, pDecimals, MidpointRounding.AwayFromZero);
		}
		#endregion

		#region Extensions for Image
		public static Image Thumbnail(this Image img, int width, int height)
		{
			int w = Math.Min(width, img.Width * height / img.Size.Height);
			int h = Math.Min(height, img.Height * width / img.Size.Width);
			return img.GetThumbnailImage(w, h, null, new System.IntPtr(0));
		}

		public static Image Crop(this Image img, Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(img);
			Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
			return (Image)(bmpCrop);
		}

		public static byte[] ToByteArray(this Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		public static string ImageToBase64(this Image image, System.Drawing.Imaging.ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				// Convert Image to byte[]
				image.Save(ms, format);
				byte[] imageBytes = ms.ToArray();

				// Convert byte[] to Base64 String
				string base64String = Convert.ToBase64String(imageBytes);
				return base64String;
			}
		}

		public static Image Base64ToImage(string base64String)
		{
			// Convert Base64 String to byte[]
			byte[] imageBytes = Convert.FromBase64String(base64String);
			MemoryStream ms = new MemoryStream(imageBytes, 0,
			  imageBytes.Length);

			// Convert byte[] to Image
			ms.Write(imageBytes, 0, imageBytes.Length);
			Image image = Image.FromStream(ms, true);
			return image;
		}

		#endregion

		#region Busca de CEP

		public class CustomWebClient : WebClient
		{
			/// <summary>
			/// Time in milliseconds
			/// </summary>
			public int Timeout { get; set; }
			public CustomWebClient() : this(60000) { }

			public CustomWebClient(int timeout)
			{
				this.Timeout = timeout;
			}

			protected override WebRequest GetWebRequest(Uri address)
			{
				var request = base.GetWebRequest(address);
				if (request != null)
				{
					request.Timeout = this.Timeout;
				}
				return request;
			}
		}

		public static DataTable BuscarCEP(string pCEP)
		{

			string URI = "http://m.correios.com.br/movel/buscaCepConfirma.do";
			string myParameters = String.Format("cepEntrada={0}&metodo=buscarCep", pCEP.Replace("-", ""));
			string HtmlResult;
			using (CustomWebClient wc = new CustomWebClient())
			{
				wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				wc.Timeout = 10000;
				HtmlResult = wc.UploadString(URI, myParameters);
			}

			var list = HtmlResult.BuscarEndereco();

			foreach (var lAddress in list)
			{
				if (lAddress.ContainsKey("Endereço"))
				{
					var campos = lAddress["Endereço"].Split(',');
					lAddress["Logradouro"] = campos[0];
					if (campos.Length > 1)
						lAddress["Número"] = campos[1];
					lAddress.Remove("Endereço");
				}
			}

			DataTable data = new DataTable();

			DataRow row;
			data.TableName = "Endereços";
			data.PrimaryKey = new DataColumn[] { data.Columns.Add("#", typeof(int)) };
			foreach (string column in list.SelectMany(x => x.Keys).Distinct())
				data.Columns.Add(column, typeof(String));

			int i = 0;
			foreach (var item in list)
			{
				row = data.NewRow();
				row["#"] = ++i;
				foreach (var campo in item)
					row[campo.Key] = campo.Value;
				data.Rows.Add(row);
			}
			return data;
		}


		private static List<Dictionary<string, string>> BuscarEndereco(this string htmlContents)
		{
			HtmlDocument lDocument = new HtmlDocument();
			lDocument.LoadHtml(htmlContents.RemoveCaracters('\r', '\n', '\t').Replace("&nbsp;", " "));
			return ExtractCEP(lDocument.DocumentNode.ChildNodes).ToList();
		}

		private static IEnumerable<Dictionary<string, string>> ExtractCEP(HtmlAgilityPack.HtmlNodeCollection pNodes)
		{
			string key = "";
			Dictionary<string, string> d = new Dictionary<string, string>();
			foreach (var node in pNodes)
			{
				if (node.Attributes.Contains("class") && node.Attributes["class"].Value == "resposta")
					key = node.InnerHtml.Trim().Left(-1).Replace(" ", "");
				if (node.Attributes.Contains("class") && node.Attributes["class"].Value == "respostadestaque")
					d.Add(key, HttpUtility.HtmlDecode(node.InnerHtml.Trim()));
				if (node.Attributes.Contains("class") && node.Attributes["class"].Value == "mopcoes orientacao")
					yield return d;
				foreach (var x in ExtractCEP(node.ChildNodes))
					yield return x;
			}
		}
		#endregion

		#region Compactação
		/// <summary>
		///	Realiza a extração de um arquivo Zip
		/// </summary>
		/// <param name="pArquivoZip">Path do arquivo Zip</param>
		/// <param name="pLocalExtracao">Path do local de extração do arquivo Zip</param>
		/// <param name="pFiltro">Filtra o que deve ser extraído do arquivo Zip</param>
		/// <returns>Retorna uma lista FileInfo[] dos arquivos extraídos</returns>
		public static FileInfo[] ExtrairArquivoZip(string pArquivoZip, string pLocalExtracao, string pFiltro)
		{
			FastZip f = new FastZip();
			if (!Directory.Exists(pLocalExtracao))
				Directory.CreateDirectory(pLocalExtracao);
			f.ExtractZip(pArquivoZip, pLocalExtracao, pFiltro);
			return new DirectoryInfo(pLocalExtracao).GetFiles();
		}
		private class MyNameTransform : INameTransform
		{
			#region INameTransform Members

			public string Root { get; set; }

			public MyNameTransform(string pRoot = "")
			{
				Root = pRoot;
			}

			public string TransformDirectory(string name)
			{
				if (Root.IsNullOrEmpty()) return name.Right(-1 - name.LastIndexOf('\\'));
				else return name.Right(-Root.Length - 1);
			}

			public string TransformFile(string name)
			{
				if (Root.IsNullOrEmpty()) return name.Right(-1 - name.LastIndexOf('\\'));
				else return name.Right(-Root.Length - 1);
			}

			#endregion
		}

		public static FileInfo Compact(this DirectoryInfo pDirectory, string pFileName, bool pDeleteFiles = false)
		{
			FastZip f = new FastZip();
			f.NameTransform = new MyNameTransform(pDirectory.FullName);
			f.CreateEmptyDirectories = true;
			f.CreateZip(pFileName, pDirectory.FullName, true, "");
			if (File.Exists(pFileName))
			{
				if (pDeleteFiles) pDirectory.Delete(true);
				return new FileInfo(pFileName);
			}
			else throw new OperacaoNaoRealizadaException();
		}

		public static FileInfo Compact(this IEnumerable<FileInfo> pList, string pFileName, bool pDeleteFiles = false)
		{
			ZipFile f = ZipFile.Create(pFileName);
			f.NameTransform = new MyNameTransform();
			f.BeginUpdate();
			pList.ForEach(x => f.Add(x.FullName));
			f.CommitUpdate();
			f.Close();
			if (pDeleteFiles)
				pList.ForEach(x => x.Delete());
			return new FileInfo(pFileName);
		}

		public static FileInfo Compact(this IEnumerable<FileStream> pList, string pFileName, bool pDeleteFiles = false)
		{
			ZipFile f = ZipFile.Create(pFileName);
			f.NameTransform = new MyNameTransform();
			f.BeginUpdate();
			pList.ForEach(x => f.Add(x.Name));
			f.CommitUpdate();
			f.Close();
			if (pDeleteFiles)
				pList.ForEach(x => File.Delete(x.Name));
			return new FileInfo(pFileName);
		}

		#endregion

		#region Extensions for DataRow
		public static T? As<T>(this DataRow pDataRow, int columnIndex)
			where T : struct
		{
			object result = pDataRow[columnIndex];
			if (result is DBNull) return null;
			if (result is T) return (T)result;
			return (T)Convert.ChangeType(result, typeof(T));
		}

		public static T? As<T>(this DataRow pDataRow, string columnName)
			where T : struct
		{
			object result = pDataRow[columnName];
			if (result is DBNull) return null;
			if (result is T) return (T)result;
			return (T)Convert.ChangeType(result, typeof(T));
		}

		public static string AsString(this DataRow pDataRow, int columnIndex)
		{
			object result = pDataRow[columnIndex];
			if (result is DBNull) return null;
			if (result is string) return (string)result;
			return (string)Convert.ChangeType(result, typeof(string));
		}
		#endregion

		#region Extensions for FileInfo
		public static DataTable ToDataTable(this FileInfo pArquivo, string pTableName = null)
		{
			if (!pArquivo.Exists)
				throw new ErrorException("O arquivo '{0}' não existe.".Formata(pArquivo.Name));
			string Conexao =
				pArquivo.Name.EndsWith("xlsx")

				? "Provider=Microsoft.ACE.OLEDB.12.0;"
				+ "Data Source='" + pArquivo.FullName + "';"
				+ "Extended Properties=Excel 12.0 Xml;"

				: "Provider=Microsoft.ACE.OLEDB.12.0;"
				+ "Data Source=" + pArquivo.FullName + ";"
				+ "Extended Properties=Excel 8.0;";
			using (System.Data.OleDb.OleDbConnection Cn = new System.Data.OleDb.OleDbConnection())
			{
				Cn.ConnectionString = Conexao;
				Cn.Open();
				object[] Restricoes = { null, null, null, "TABLE" };
				DataTable DTSchema = Cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, Restricoes);
				if (DTSchema.Rows.Count > 0)
				{
					string Sheet = pTableName == null ? DTSchema.Rows[0]["TABLE_NAME"].ToString() : pTableName + "$";
					System.Data.OleDb.OleDbCommand Comando = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + Sheet + "]", Cn);
					DataTable Dados = new DataTable();
					System.Data.OleDb.OleDbDataAdapter DA = new System.Data.OleDb.OleDbDataAdapter(Comando);
					DA.Fill(Dados);
					Cn.Close();
					return Dados;
				}
			}
			return null;
		}

		public static string GetContentTypeSys(this FileInfo pFile)
		{
			// set a default mimetype if not found.
			string contentType = "application/octet-stream";
			try
			{
				// get the registry classes root
				RegistryKey classes = Registry.ClassesRoot;

				// find the sub key based on the file extension
				RegistryKey fileClass = classes.OpenSubKey(Path.GetExtension(pFile.Name));
				if (fileClass.GetValue("Content Type") != null)
					contentType = fileClass.GetValue("Content Type").ToString();
				else
					contentType = pFile.Name.GetMIMEType();
			}
			catch { }

			return contentType;
		}

		public static IEnumerable<string> EnumerateRows(this FileInfo pFile)
		{
			using (StreamReader s = new StreamReader(pFile.FullName))
				yield return s.ReadLine();
		}
		#endregion

		#region Extensions for Type
		public static PropertyInfo GetPublicProperty(this Type type, string PropertyName)
		{
			return type.GetPublicProperties().LastOrDefault(x => x.Name == PropertyName);
		}

		public static PropertyInfo[] GetPublicProperties(this Type type)
		{
			if (type.IsInterface)
			{
				var propertyInfos = new List<PropertyInfo>();

				var considered = new List<Type>();
				var queue = new Queue<Type>();
				considered.Add(type);
				queue.Enqueue(type);
				while (queue.Count > 0)
				{
					var subType = queue.Dequeue();
					foreach (var superInterface in subType.GetInterfaces())
					{
						if (considered.Contains(superInterface)) continue;

						considered.Add(superInterface);
						queue.Enqueue(superInterface);
					}

					var typeProperties = subType.GetProperties(
						BindingFlags.FlattenHierarchy
						| BindingFlags.Public
						| BindingFlags.Instance
						| BindingFlags.Static);

					var newPropertyInfos = typeProperties
						.Where(x => !propertyInfos.Contains(x));

					propertyInfos.InsertRange(0, newPropertyInfos);
				}

				return propertyInfos.ToArray();
			}

			return type.GetProperties(BindingFlags.FlattenHierarchy
				| BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}

		public static string GetFriendlyName(this MemberInfo type, bool pPlural = false)
		{
			FriendlyNameAttribute f = (FriendlyNameAttribute)Attribute.GetCustomAttribute(type, typeof(FriendlyNameAttribute));
			if (f != null) return pPlural ? f.NamePlural : f.NameSingle;
			//else if (type is Type && (type as Type).UnderlyingSystemType.IsInterface)
			//{
			//	type = Type.GetType(Container.ResolveAssemblyQualifiedName(type as Type));
			//	return type.GetFriendlyName(pPlural);
			//}
			return type.Name;
		}

		#endregion

		#region Extensions for Exception
		public static BusinessException AsBusinessException(this Exception pException)
		{
			for (; pException != null; pException = pException.InnerException)
				if (pException is BusinessException) return pException as BusinessException;
			return null;
		}

		public static string GetMessages(this Exception pException)
		{
			List<string> lResult = new List<string>();
			for (; pException != null; pException = pException.InnerException)
				if (pException is BusinessException)
				{
					lResult.AddRange((pException as BusinessException).Errors.Split('\n'));
					lResult.AddRange((pException as BusinessException).Warnings.Split('\n'));
				}
				else lResult.Add(pException.Message);
			return lResult.Where(x => !x.IsNullOrEmpty()).StringJoin("; ");
		}
		#endregion

		public static void SetValue<T>(this T pObject, T pValue)
		{
		}

		public static void Set(this IPrincipal pPrincipal, string identityName, params string[] roles)
		{
			Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(identityName), roles);
		}
	}
}
