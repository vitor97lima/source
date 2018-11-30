using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Treinamento.DAL
{
    public interface IDAL<T> where T : class
	{
		/// <summary>
		/// Insere ou atualiza um objeto no banco de dados
		/// </summary>
		/// <param name="pObjeto">O objeto a ser salvo</param>
		/// <returns>O objeto salvo</returns>
		T Salvar(T pObjeto);

		/// <summary>
		/// Insere ou atualiza os objetos de uma lista no banco de dados
		/// </summary>
		/// <param name="pObjetos">A lista de objetos</param>
		void Salvar(List<T> pObjetos);

		/// <summary>
		/// Exclui um objeto do banco de dados
		/// </summary>
		/// <param name="pObjeto">O objeto a ser excluído</param>
		void Excluir(T pObjeto);

		/// <summary>
		/// Exclui uma lista de objetos do banco de dados
		/// </summary>
		/// <param name="pObjetos">A lista de objetos</param>
		void Excluir(List<T> pObjetos);

		int Excluir(params int[] pIds);

		/// <summary>
		/// Busca no cache ou no banco de dados um objeto do tipo T pelo Id
		/// </summary>
		/// <param name="pId">O valor do identificador do objeto</param>
		/// <returns>Objeto do tipo T</returns>
		T BuscarPorId(int pId);

		/// <summary>
		/// Desfaz alterações buscando o objeto no banco de dados novamente
		/// </summary>
		/// <param name="pObjeto">O objeto a ser atualizado</param>
		/// <returns>O objeto atualizado</returns>
		T Atualizar(T pObjeto);

		/// <summary>
		/// Busca no banco de dados os objetos do tipo T
		/// </summary>
		/// <param name="pOrdenacao">Array de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		List<T> Listar(params Ordem<T>[] pOrdenacao);

		/// <summary>
		/// Busca no banco de dados os objetos do tipo T
		/// </summary>
		/// <param name="pCondicao">Expressão contendo o filtro da busca</param>
		/// <param name="pOrdenacao">Array de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		List<T> Listar(Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao);

		/// <summary>
		/// Busca no banco de dados os objetos do tipo T
		/// </summary>
		/// <param name="pPrimeiroItem">Ponto de partida da busca</param>
		/// <param name="pItensPorPagina">Quantidade de objetos a ser buscada</param>
		/// <param name="pOrdenacao">Array de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		List<T> Listar(Expression<Func<T, bool>> pCondicao, string pOrdenacao);

		List<T> ListarSemCache(Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao);

		/// <summary>
		/// Busca no banco de dados os objetos do tipo T
		/// </summary>
		/// <param name="pPrimeiroItem">Ponto de partida da busca</param>
		/// <param name="pItensPorPagina">Quantidade de objetos a ser buscada</param>
		/// <param name="pCondicao">Expressão contendo o filtro da busca</param>
		/// <param name="pOrdenacao">String de ordenação no formato "nome_da_propriedade[{.nome_da_propriedade}] [asc|desc][{, nome_da_propriedade[{.nome_da_propriedade}] [asc|desc]}]</param>
		/// <returns>Lista de objetos T</returns>
		List<T> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<T, bool>> pCondicao, string pOrdenacao);

		/// <summary>
		/// Busca no banco de dados os objetos do tipo T
		/// </summary>
		/// <param name="pPrimeiroItem">Ponto de partida da busca</param>
		/// <param name="pItensPorPagina">Quantidade de objetos a ser buscada</param>
		/// <param name="pCondicao">Expressão contendo o filtro da busca</param>
		/// <param name="pOrdenacao">Array de expressões de ordenação</param>
		/// <returns>Lista de objetos T</returns>
		List<T> Listar(int pPrimeiroItem, int pItensPorPagina, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao);

		/// <summary>
		/// Busca por propriedade de objeto
		/// </summary>
		/// <typeparam name="TResult">O tipo da propriedade</typeparam>
		/// <param name="pProperty">Expressão representando a propriedade</param>
		/// <param name="pCondicao">O filtro a ser aplicado</param>
		/// <param name="pOrdenacao">Expressões de ordenação</param>
		/// <returns>Lista das propriedades</returns>
		List<TResult> Listar<TResult>(Expression<Func<T, TResult>> pProperty, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao);

		TResult Maximo<TResult>(Expression<Func<T, TResult>> pProperty, Expression<Func<T, bool>> pCondicao, params Ordem<T>[] pOrdenacao);

		/// <summary>
		/// Retorna a quantidade de objetos T cadastrados no banco de dados
		/// </summary>
		/// <returns></returns>
		Int32 Quantidade();

		/// <summary>
		/// Retorna a quantidade de objetos T cadastrados no banco de dados que satisfaçam determinada condição
		/// </summary>
		/// <param name="pCondicao">Predicate contendo o filtro a ser aplicado</param>
		/// <returns></returns>
		Int32 Quantidade(Expression<Func<T, bool>> pCondicao);

	}
}
