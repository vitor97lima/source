using System;
using System.Collections.Generic;
using System.IO;

namespace System
{
	public interface IExcel
	{
		Excel<TResult> Append<TResult>(IEnumerable<TResult> pDataSource);
		Excel<TResult> Append<TResult>(IEnumerable<TResult> pDataSource, params string[] pHeaderText);
		Excel<TResult> Append<TResult>(Excel<TResult> pExcel);
		IExcel Append(IExcel pExcel);
		Excel<TResult> InnerAppend<TResult>(IEnumerable<TResult> pDataSource);
		Excel<TResult> InnerAppend<TResult>(IEnumerable<TResult> pDataSource, params string[] pHeaderText);
		Excel<TResult> InnerAppend<TResult>(Excel<TResult> pExcel);
		IExcel InnerAppend(IExcel pExcel);
		Excel<TResult> ToExcel<TResult>();
		FileInfo Export(string pFileName);
	}
}
