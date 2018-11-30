using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using LinqExpression = System.Linq.Expressions.Expression;

namespace System.Linq
{
	/// <summary>
	/// Estrutura para armazenamento e manipulação de expressões booleanas de busca
	/// </summary>
	/// <typeparam name="TDTO">O tipo da expressão</typeparam>
	public struct Busca<TDTO> where TDTO : class
	{
		private Expression<Func<TDTO, bool>> _expression;
		/// <summary>
		/// Retorna System.Linq.Expression equivalente
		/// </summary>
		public Expression<Func<TDTO, bool>> Expression
		{
			get { return _expression; }
			set { _expression = value; }
		}

		/// <summary>
		/// Construtor do objeto
		/// </summary>
		/// <param name="pExpression">Expressão booleana inicial</param>
		public Busca(Expression<Func<TDTO, bool>> pExpression)
		{
			_expression = pExpression;
		}

		/// <summary>
		/// Limpa a estrutura
		/// </summary>
		public void Clear()
		{
			Expression = null;
		}

		/// <summary>
		/// Mescla determinada expressão booleana com a expressão corrente usando operação AndAlso
		/// </summary>
		/// <param name="f">Expressão a ser mesclada</param>
		/// <returns>Este objeto com a expressão mesclada</returns>
		public Busca<TDTO> And(Expression<Func<TDTO, bool>> f)
		{
			Expression = Expression.And(f);
			return this;
		}

		/// <summary>
		/// Mescla determinada expressão booleana com a expressão corrente usando operação OrElse
		/// </summary>
		/// <param name="f">Expressão a ser mesclada</param>
		/// <returns>Este objeto com a expressão mesclada</returns>
		public Busca<TDTO> Or(Expression<Func<TDTO, bool>> f)
		{
			Expression = Expression.Or(f);
			return this;
		}

		/// <summary>
		/// Mescla duas expressões booleanas usando operação AndAlso
		/// </summary>
		/// <param name="f1">Expressão da esquerda</param>
		/// <param name="f2">Expressão da direita</param>
		/// <returns>Expressão mesclada</returns>
		public static Expression<Func<TDTO, bool>> And(Expression<Func<TDTO, bool>> f1, Expression<Func<TDTO, bool>> f2)
		{
			return f1.And(f2);
		}

		/// <summary>
		/// Mescla duas expressões booleanas usando operação OrElse
		/// </summary>
		/// <param name="f1">Expressão da esquerda</param>
		/// <param name="f2">Expressão da direita</param>
		/// <returns>Expressão mesclada</returns>
		public static Expression<Func<TDTO, bool>> Or(Expression<Func<TDTO, bool>> f1, Expression<Func<TDTO, bool>> f2)
		{
			return f1.Or(f2);
		}

		/// <summary>
		/// Operador de cast implícito
		/// </summary>
		/// <param name="pExpGen">Objeto Busca\<T\> a ser convertido"</param>
		/// <returns>System.Linq.Expression equivalente</returns>
		public static implicit operator Expression<Func<TDTO, bool>>(Busca<TDTO> pExpGen)
		{
			return pExpGen.Expression;
		}
	}
}
