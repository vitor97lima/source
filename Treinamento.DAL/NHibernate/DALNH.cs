using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Transform;

namespace Treinamento.DAL.NHibernate
{
	internal class DALNH<T> : IDAL<T> where T : class
	{
		private static ISession _sessao;
		protected static ISession Sessao
		{
			get { return _sessao = _sessao ?? NHibernateHelper.Sessao; }
		}

		public DALNH() { }

		#region IDAL<T> Members

		public T Salvar(T pObjeto)
		{
			if (((dynamic)pObjeto).Id > 0)
				Sessao.Merge(pObjeto);
			else Sessao.SaveOrUpdate(pObjeto);
			try { Sessao.Flush(); }
			catch { Sessao.Clear(); throw; }
			return pObjeto;
		}

		public void Salvar(List<T> pObjetos)
		{
			for (int i = 0; i < pObjetos.Count; i++)
			{
				Sessao.SaveOrUpdate(pObjetos[i]);
				if (i % 50 == 0)
				{
					try
					{
						Sessao.Flush();
						Sessao.Clear();
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
			Sessao.Flush();
		}

		public void Excluir(T pObjeto)
		{
			Sessao.Delete(pObjeto);
			Sessao.Flush();
			PropertyInfo pi = pObjeto.GetType().GetProperty("Id");
			MethodInfo mi;
			if (pi != null && (mi = pi.GetSetMethod(true)) != null)
				mi.Invoke(pObjeto, new object[] { 0 });
		}

		public void Excluir(List<T> pObjetos)
		{
			pObjetos.ForEach(o => Sessao.Delete(o));
			Sessao.Flush();
		}

		public int Excluir(params int[] pIds)
		{
			if (pIds.Length == 0) return 0;
			else return Sessao.CreateQuery(string.Format("delete from {0} where {1} in ({2})", typeof(T).Name, "Id", string.Join(",", pIds.ToArray()))).ExecuteUpdate();
		}

		public T BuscarPorId(int pId)
		{
			T o = Sessao.Get<T>(pId);
			//Sessao.Evict(o);
			return o;
		}

		public T Atualizar(T pObjeto)
		{
			if (pObjeto != null && ((dynamic)pObjeto).Id != 0)
				Sessao.Refresh(pObjeto);
			return pObjeto;
		}

		public List<T> Listar(params Ordem<T>[] pOrdenacao)
		{
			return Listar(null, pOrdenacao);
		}

		public List<T> Listar(Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao)
		{
			//int orders;

			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			Ordenar(lCriteria, pOrdenacao);

			lCriteria.SetResultTransformer(Transformers.DistinctRootEntity);

			return SafeList(lCriteria.List<T>().ToList());
		}

		public List<T> Listar(Expression<Func<T, bool>> pCondicao, string pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			foreach (Order order in CreateOrdersFrom(pOrdenacao, lCriteria))
			{
				lCriteria.AddOrder(order);
			}

			lCriteria.AddOrder(new Order("Id", true));

			lCriteria.SetResultTransformer(Transformers.DistinctRootEntity);

			return SafeList(lCriteria.List<T>().ToList());
		}

		public List<T> ListarSemCache(Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));
			if (pOrdenacao != null)
				Ordenar(lCriteria, pOrdenacao);

			lCriteria.SetResultTransformer(Transformers.DistinctRootEntity);

			return SafeList(lCriteria.SetCacheable(false).List<T>().ToList());
		}

		public List<T> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<T, bool>> pCondicao, string pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			foreach (Order order in CreateOrdersFrom(pOrdenacao, lCriteria))
			{
				lCriteria.AddOrder(order);
			}

			lCriteria.AddOrder(new Order("Id", true));

			lCriteria.SetResultTransformer(Transformers.DistinctRootEntity);

			return SafeList(lCriteria.SetFirstResult(pPrimeiroItem)
				.SetMaxResults(pItensPorPagina)
				.SetCacheable(false)
				.List<T>().ToList());
		}

		public List<T> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			Ordenar(lCriteria, pOrdenacao);

			lCriteria.AddOrder(new Order("Id", true));

			lCriteria.SetResultTransformer(Transformers.DistinctRootEntity);

			return SafeList(lCriteria.SetFirstResult(pPrimeiroItem)
				.SetMaxResults(pItensPorPagina)
				.SetCacheable(false)
				.List<T>().ToList());
		}

		public List<TResult> Listar<TResult>(Expression<Func<T, TResult>> pProperty, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			Ordenar(lCriteria, pOrdenacao);
			Ordenar(lCriteria, Ordem<T>.ASC(ConvertFunction(pProperty)));

			string[] properties = pProperty.Body.ToString().RemoveCaracters('(', ')').Split('.');
			if (properties.Length > 3)
				lCriteria.SetProjection(Projections.Property(properties[properties.Length - 2] + "." + properties[properties.Length - 1]));
			else
				lCriteria.SetProjection(Projections.Property<T>(ConvertFunction(pProperty)));

			return lCriteria.List<TResult>().ToList();
		}

		public TResult Maximo<TResult>(Expression<Func<T, TResult>> pProperty, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			//Ordenar(lCriteria, pOrdenacao);
			//Ordenar(lCriteria, Ordem<T>.ASC(pProperty));

			string[] properties = pProperty.Body.ToString().RemoveCaracters('(', ')').Split('.');
			if (properties.Length > 3)
				lCriteria.SetProjection(Projections.Max(properties[properties.Length - 2] + "." + properties[properties.Length - 1]));
			else
				lCriteria.SetProjection(Projections.Max<T>(ConvertFunction(pProperty)));

			return lCriteria.UniqueResult<TResult>();
		}

		public Int32 Quantidade()
		{
			return this.Quantidade(null);
		}

		public Int32 Quantidade(System.Linq.Expressions.Expression<Func<T, bool>> pCondicao)
		{
			ICriteria lCriteria = Sessao.CreateCriteria<T>();

			if (pCondicao != null)
				lCriteria.Add(ExpressionProcessor.ProcessExpression<T>(pCondicao, lCriteria));

			return (Int32)lCriteria.SetProjection(Projections.Count("Id")).UniqueResult();
		}

		#endregion

		#region Acessórios

		private Expression<Func<T, object>> ConvertFunction<TResult>(Expression<Func<T, TResult>> pProperty)
		{
			if (typeof(TResult).IsByRef)
				return global::System.Linq.Expressions.Expression.Lambda<Func<T, object>>(pProperty.Body, pProperty.Parameters);
			else return global::System.Linq.Expressions.Expression.Lambda<Func<T, object>>(global::System.Linq.Expressions.Expression.Convert(pProperty.Body, typeof(object)), pProperty.Parameters);
		}

		private ICriteria Ordenar(ICriteria pCriteria, params Ordem<T>[] pOrdenacao)
		{
			if (pOrdenacao == null || pOrdenacao.Length == 0 || pCriteria == null) return pCriteria;

			for (int i = 0; i < pOrdenacao.Length; i++)
				pCriteria = pCriteria.AddOrder(ExpressionProcessor.ProcessOrder<T>(pOrdenacao[i].Property, s => new Order(s, pOrdenacao[i].Crescente), pCriteria));

			return pCriteria;
		}

		private static List<T> SafeList(List<T> pList)
		{
			if (pList.Count > 0 && pList.First().GetType().ToString().Contains("Proxy"))
				pList[0] = (T)Sessao.GetSessionImplementation().PersistenceContext.Unproxy(pList[0]);
			return pList;
		}

		private static List<Order> CreateOrdersFrom(string pOrdenacao, ICriteria pCriteria)
		{
			if (string.IsNullOrEmpty(pOrdenacao))
				return new List<Order>();

			List<Order> orders = new List<Order>();
			string[] sortExpressions = pOrdenacao.Trim().Split(',');

			Array.ForEach(sortExpressions, sortExpression =>
			{
				string[] columnAndSort = sortExpression.Trim().Split(' ');

				string column = "", sort = "asc";

				column = IncluiSubcriteria(columnAndSort[0], pCriteria);
				if (columnAndSort.Length == 2)
				{
					sort = columnAndSort[1];
				}
				orders.Add(sort.ToLowerInvariant() == "asc" ? Order.Asc(column) : Order.Desc(column));
			});

			return orders;
		}

		private static string IncluiSubcriteria(string column, ICriteria pCriteria)
		{
			string[] Items = column.Split('.');
			ICriteria lCriteria = pCriteria;
			for (int i = 0; i < Items.Length - 1; i++)
			{
				if (pCriteria.GetCriteriaByAlias(Items[i]) == null) lCriteria = lCriteria.CreateCriteria(Items[i], Items[i]);
				else lCriteria = lCriteria.GetCriteriaByAlias(Items[i]);
			}
			return Items.Length > 1 ? Items[Items.Length - 2] + "." + Items[Items.Length - 1] : Items[Items.Length - 1];
		}

		private ProjectionList ProjectionProperties(out int pOrders, params Expression<Func<T, object>>[] pOrdenacao)
		{
			String[] properties = Sessao.SessionFactory.GetClassMetadata(typeof(T)).PropertyNames;
			global::NHibernate.Type.IType[] types = Sessao.SessionFactory.GetClassMetadata(typeof(T)).PropertyTypes;

			ProjectionList list = Projections.ProjectionList();
			list.Add(Projections.Property(Sessao.SessionFactory.GetClassMetadata(typeof(T)).IdentifierPropertyName), Sessao.SessionFactory.GetClassMetadata(typeof(T)).IdentifierPropertyName);
			for (int i = 0; i < properties.Length; ++i)
				if (!types[i].IsCollectionType)
					list.Add(Projections.Property(properties[i]), properties[i]);


			List<Order> lOrders = new List<Order>();
			pOrders = 0;
			if (pOrdenacao.Length == 0) return list;

			for (int i = 0; i < pOrdenacao.Length; i++)
			{
				if (pOrdenacao[i].Body is UnaryExpression
					&& ((UnaryExpression)pOrdenacao[i].Body).Operand is ConstantExpression)
					throw new Exception("Invalid construction of ordering parameters.");

				if (i + 1 < pOrdenacao.Length
					&& pOrdenacao[i + 1].Body is UnaryExpression
					&& ((UnaryExpression)pOrdenacao[i + 1].Body).Operand is ConstantExpression
					&& (int)((ConstantExpression)((UnaryExpression)pOrdenacao[i + 1].Body).Operand).Value < 0)
				{
					lOrders.Add(ExpressionProcessor.ProcessOrder<T>(pOrdenacao[i], s => new Order(s, false)));
				}
				else
				{
					lOrders.Add(ExpressionProcessor.ProcessOrder<T>(pOrdenacao[i], s => new Order(s, true)));
				}
				if (i + 1 < pOrdenacao.Length
					&& pOrdenacao[i + 1].Body is UnaryExpression
					&& ((UnaryExpression)pOrdenacao[i + 1].Body).Operand is ConstantExpression) i++;
			}

			foreach (Order o in lOrders)
			{
				String[] members = o.PropertyName.Split('.');
				if (members.Length >= 2)
				{
					list.Add(Projections.Property(members[members.Length - 2] + "." + members[members.Length - 1]), members[members.Length - 2] + "." + members[members.Length - 1]);
					pOrders++;
				}
			}

			return list;
		}

		#endregion

	}
}
