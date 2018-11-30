using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treinamento.DAL;
using System.Reflection;
using System.Linq.Expressions;
using Treinamento.Exceptions;
using Treinamento.DTO.Global;
using Treinamento.DTO;
using System.ComponentModel;
using System.IO;

namespace Treinamento.BLL
{
	/// <summary>
	/// Classe abstrata com métodos essenciais para a camada de negócios
	/// </summary>
	/// <typeparam name="TDTO">O tipo DTO que será implementado</typeparam>

	public abstract class ABLL<TDTO, TBLL>
		where TDTO : class
		where TBLL : ABLL<TDTO, TBLL>
	{
		protected ABLL()
		{
			_tDAL = FactoryDAL.CreateDAL<TDTO>();
		}

		private IDAL<TDTO> _tDAL;
		protected IDAL<TDTO> DAL
		{
			get { return _tDAL; }
		}

		private static TBLL _instance;
		public static TBLL Instance
		{
			get { return _instance = _instance ?? (TBLL)Activator.CreateInstance(typeof(TBLL), true); }
		}

		private static string _friendlyName = typeof(TDTO).GetFriendlyName();
		public static string FriendlyName { get { return _friendlyName; } }

		#region Métodos Listar
		/// <summary>
		/// Lista todos os objetos T
		/// </summary>
		/// <param name="pOrdenacao">Lista de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(params Ordem<TDTO>[] pOrdenacao)
		{
			return DAL.Listar(pOrdenacao);
		}

		/// <summary>
		/// Lista todos os objetos T que satisfaçam a condição pCondicao
		/// </summary>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <param name="pOrdenacao">Lista de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(Expression<Func<TDTO, bool>> pCondicao, params Ordem<TDTO>[] pOrdenacao)
		{
			if (pCondicao == null) return Listar(pOrdenacao);
			return DAL.Listar(pCondicao, pOrdenacao).ToList();
		}

		public List<TDTO> ListarSemCache(Expression<Func<TDTO, bool>> pCondicao, params Ordem<TDTO>[] pOrdenacao)
		{
			return DAL.ListarSemCache(pCondicao, pOrdenacao);
		}

		/// <summary>
		/// Lista N objetos T a partir do objeto T de índice pPrimeiroItem. Método utilizado para paginação
		/// </summary>
		/// <param name="pPrimeiroItem">O índice (zero-based) do primeiro objeto T a ser retornado. Calculado a partir do numero da página (zero-based) * número de itens por página</param>
		/// <param name="pItensPorPagina">A quantidade de objetos T</param>
		/// <param name="pOrdenacao">Lista de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(Expression<Func<TDTO, bool>> pCondicao, string pOrdenacaoString)
		{
			return DAL.Listar(pCondicao, pOrdenacaoString);
		}

		/// <summary>
		/// Lista N objetos T a partir do objeto T de índice pPrimeiroItem. Método utilizado para paginação
		/// </summary>
		/// <param name="pPrimeiroItem">O índice (zero-based) do primeiro objeto T a ser retornado. Calculado a partir do numero da página (zero-based) * número de itens por página</param>
		/// <param name="pItensPorPagina">A quantidade de objetos T</param>
		/// <param name="pOrdenacaoString">String representando a ordenação. Ex.: 'Property1.Property1_1 asc, Property2 desc'</param>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, string pOrdenacaoString, Expression<Func<TDTO, bool>> pCondicao)
		{
			return DAL.Listar(pPrimeiroItem, pItensPorPagina, pCondicao, pOrdenacaoString);
		}

		/// <summary>
		/// Lista N objetos T a partir do objeto T de índice pPrimeiroItem. Método utilizado para paginação
		/// </summary>
		/// <param name="pPrimeiroItem">O índice (zero-based) do primeiro objeto T a ser retornado. Calculado a partir do numero da página (zero-based) * número de itens por página</param>
		/// <param name="pItensPorPagina">A quantidade de objetos T</param>
		/// <param name="pOrdenacaoString">String representando a ordenação. Ex.: 'Property1.Property1_1 asc, Property2 desc'</param>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, string pOrdenacaoString, Expression<Func<TDTO, bool>> pCondicao, List<TDTO> pSource)
		{
			List<TDTO> pResult = null;
			if (pCondicao == null) pResult = pSource.ToList();
			else pResult = pSource.Where(pCondicao.Compile()).ToList();
			if (!pOrdenacaoString.IsNullOrEmpty())
				pResult.Sort(new Ordenadora<TDTO>(pOrdenacaoString));
			return pResult.Where((obj, index) => index >= pPrimeiroItem && index <= (pPrimeiroItem + pItensPorPagina)).ToList();
		}

		/// <summary>
		/// Lista N objetos T a partir do objeto T de índice pPrimeiroItem. Método utilizado para paginação
		/// </summary>
		/// <param name="pPrimeiroItem">O índice (zero-based) do primeiro objeto T a ser retornado. Calculado a partir do numero da página (zero-based) * número de itens por página</param>
		/// <param name="pItensPorPagina">A quantidade de objetos T</param>
		/// <param name="pOrdenacaoString">String representando a ordenação. Ex.: 'Property1.Property1_1 asc, Property2 desc'</param>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <param name="pTotalRows">Retorna a quantidade total de linhas afetadas desconsiderando a paginação</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, string pOrdenacaoString, Expression<Func<TDTO, bool>> pCondicao, out int pTotalRows)
		{
			pTotalRows = Quantidade(pCondicao);
			return DAL.Listar(pPrimeiroItem, pItensPorPagina, pCondicao, pOrdenacaoString);
		}

		/// <summary>
		/// Lista N objetos T a partir do objeto T de índice pPrimeiroItem. Método utilizado para paginação
		/// </summary>
		/// <param name="pPrimeiroItem">O índice (zero-based) do primeiro objeto T a ser retornado. Calculado a partir do numero da página (zero-based) * número de itens por página</param>
		/// <param name="pItensPorPagina">A quantidade de objetos T</param>
		/// <param name="pOrdenacao">Lista de expressões de ordenação</param>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <param name="pTotalRows">Retorna a quantidade total de linhas afetadas desconsiderando a paginação</param>
		/// <returns>Lista de objetos T</returns>
		public List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<TDTO, bool>> pCondicao, params Ordem<TDTO>[] pOrdenacao)
		{
			return DAL.Listar(pPrimeiroItem, pItensPorPagina, pCondicao, pOrdenacao);
		}

		/// <summary>
		/// Busca por propriedade de objeto
		/// </summary>
		/// <typeparam name="TResult">O tipo da propriedade</typeparam>
		/// <param name="pProperty">Expressão representando a propriedade</param>
		/// <param name="pOrdenacao">Expressões de ordenação</param>
		/// <returns>Lista das propriedades</returns>
		public List<TResult> SubListar<TResult>(Expression<Func<TDTO, TResult>> pProperty, params Ordem<TDTO>[] pOrdenacao)
		{
			return DAL.Listar<TResult>(pProperty, null, pOrdenacao).ToList<TResult>();
		}

		/// <summary>
		/// Busca por propriedade de objeto
		/// </summary>
		/// <typeparam name="TResult">O tipo da propriedade</typeparam>
		/// <param name="pProperty">Expressão representando a propriedade</param>
		/// <param name="pCondicao">O filtro a ser aplicado</param>
		/// <param name="pOrdenacao">Expressões de ordenação</param>
		/// <returns>Lista das propriedades</returns>
		public List<TResult> SubListar<TResult>(Expression<Func<TDTO, TResult>> pProperty, Expression<Func<TDTO, bool>> pCondicao, params Ordem<TDTO>[] pOrdenacao)
		{
			return DAL.Listar<TResult>(pProperty, pCondicao, pOrdenacao).ToList<TResult>();
		}
		#endregion

		/// <summary>
		/// Busca o objeto T pelo Id
		/// </summary>
		/// <param name="pId">Id do objeto</param>
		/// <returns>Objeto T</returns>
		public virtual TDTO BuscarPorId(int pId)
		{
			if (pId == 0) return null;
			return DAL.BuscarPorId(pId);
		}

		/// <summary>
		/// Lê novamente o objeto do banco de dados
		/// </summary>
		/// <param name="pObjeto">Objeto a ser atualizado</param>
		/// <returns>O objeto atualizado</returns>
		public TDTO Atualizar(TDTO pObjeto)
		{
			return DAL.Atualizar(pObjeto);
		}

		public virtual TDTO Persistir(TDTO pObjeto)
		{
			Validar(pObjeto, false);
			return PersistirObjeto(pObjeto);
		}

		/// <summary>
		/// Persiste o objeto T na Base de Dados
		/// </summary>
		/// <param name="pObjeto">Objeto T</param>
		private TDTO PersistirObjeto(TDTO pObjeto)
		{
			return DAL.Salvar(pObjeto);
		}

		public virtual List<TDTO> Persistir(List<TDTO> pObjetos)
		{
			pObjetos.ForEach(o => Validar(o, false));
			return PersistirObjetos(pObjetos);
		}

		/// <summary>
		/// Persiste uma lista de objetos T na base de dados
		/// </summary>
		/// <param name="pObjetos">Lista de objetos T</param>
		private List<TDTO> PersistirObjetos(List<TDTO> pObjetos)
		{
			DAL.Salvar(pObjetos);
			return pObjetos;
		}

		public virtual void Excluir(TDTO pObjeto)
		{
			VerificarDependencias(pObjeto);
			ExcluirObjeto(pObjeto);
		}

		public virtual int Excluir(params int[] pIds)
		{
			pIds.ForEach(x => VerificarDependencias(x));
			return DAL.Excluir(pIds);
		}

		/// <summary>
		/// Exclui o objeto T da base de dados
		/// </summary>
		/// <param name="pObjeto">Objeto T</param>
		private void ExcluirObjeto(TDTO pObjeto)
		{
			DAL.Excluir(pObjeto);
		}

		/// <summary>
		/// Exclui uma lista de objetos T da base de dados
		/// </summary>
		/// <param name="pObjetos">Lista de objetos T</param>
		private void ExcluirObjetos(List<TDTO> pObjetos)
		{
			DAL.Excluir(pObjetos);
		}

		/// <summary>
		/// Retorna a quantidade de objetos T cadastrados no banco de dados
		/// </summary>
		/// <returns></returns>
		public Int32 Quantidade()
		{
			return Quantidade(null);
		}

		/// <summary>
		/// Retorna a quantidade de objetos T cadastrados no banco de dados que satisfaçam determinada condição
		/// </summary>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <returns></returns>
		public Int32 Quantidade(Expression<Func<TDTO, bool>> pCondicao)
		{
			if (pCondicao == null) return DAL.Quantidade();
			return DAL.Quantidade(pCondicao);
		}

		/// <summary>
		/// Retorna a quantidade de objetos T incluídos em uma lista que satisfaçam determinada condição
		/// </summary>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <param name="pOrdenacaoString">Ignorado. Apenas para compatibilidade da paginação</param>
		/// <returns></returns>
		public Int32 Quantidade(Expression<Func<TDTO, bool>> pCondicao, List<TDTO> pSource)
		{
			if (pCondicao == null) return pSource.Count;
			else return pSource.Where(pCondicao.Compile()).Count();
		}

		/// <summary>
		/// Retorna a quantidade de objetos T cadastrados no banco de dados que satisfaçam determinada condição
		/// </summary>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <param name="pOrdenacaoString">Ignorado. Apenas para compatibilidade da paginação</param>
		/// <param name="pTotalRows">Retorna a quantidade total de linhas afetadas desconsiderando a paginação</param>
		/// <returns></returns>
		public Int32 Quantidade(Expression<Func<TDTO, bool>> pCondicao, out int pTotalRows)
		{
			if (pCondicao == null) return pTotalRows = DAL.Quantidade();
			return pTotalRows = DAL.Quantidade(pCondicao);
		}

		/// <summary>
		/// Percorre todo o assembly verificando a existência de métodos TemVinculo com parâmetro do tipo $T$, acumulando exceções caso o método retorne true
		/// </summary>
		/// <param name="pObjeto">Objeto a ser testado</param>
		public void VerificarDependencias(TDTO pObjeto)
		{
			BusinessException ex = null;
			Assembly ass = Assembly.GetExecutingAssembly();
			List<Type> types = ass.GetTypes().Where(
				t =>
				{
					for (Type t2 = t.BaseType; t2 != null; t2 = t2.BaseType)
						if (t2.IsGenericType && t2.GetGenericTypeDefinition() == typeof(ABLL<TDTO, TBLL>).GetGenericTypeDefinition()) return true;
					return false;
				}
				).ToList();
			foreach (Type t in types)
			{
				List<MethodInfo> methods = t.GetMethods().Where(
					m => m.Name == "TemVinculo"
						&& m.ReturnType == typeof(bool)
						&& m.GetParameters().Count() == 1
						&& m.GetParameters().ElementAt(0).ParameterType == typeof(TDTO)
				).ToList();
				foreach (MethodInfo m in methods)
				{
					object value = t.InvokeMember("Instance", BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, null);
					if ((bool)m.Invoke(value, new object[] { pObjeto }))
						ex = new DependenciaException(FriendlyName, t.BaseType.GetGenericArguments().ElementAt(0).GetFriendlyName(), ex);
				}
			}
			if (ex != null) throw ex;
		}

		public void VerificarDependencias(int id)
		{
			var obj = Activator.CreateInstance<TDTO>();
			typeof(TDTO).GetProperty("Id").GetSetMethod(true).Invoke(obj, new object[] { id });
			VerificarDependencias(obj);
		}

		public abstract void Validar(TDTO pObjeto, bool pOpcionais = true);

		public virtual FileInfo GerarExcel(string PFileName)
		{
			throw new NotImplementedException();
		}

		//public bool TemVinculo(object pObjeto) { return false; }

		#region Paginação
		public static Int32 Quantidade(Expression<Func<TDTO, bool>> pCondicao, string pOrdenacaoString)
		{
			return Instance.Quantidade(pCondicao);
		}

		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<TDTO, bool>> pCondicao, string pOrdenacaoString)
		{
			return Instance.Listar(pPrimeiroItem, pItensPorPagina, pOrdenacaoString, pCondicao);
		}

		public static Int32 Quantidade(Expression<Func<TDTO, bool>> pCondicao, List<TDTO> pSource, string pOrdenacaoString)
		{
			return Instance.Quantidade(pCondicao, pSource);
		}

		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static List<TDTO> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<TDTO, bool>> pCondicao, string pOrdenacaoString, List<TDTO> pSource)
		{
			return Instance.Listar(pPrimeiroItem, pItensPorPagina, pOrdenacaoString, pCondicao, pSource);
		}

		#endregion
	}
}
