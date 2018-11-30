using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace System
{
	/// <summary>
	/// Classe para escrever na janela do Debug
	/// </summary>
	public class DebugWriter : TextWriter
	{
		/// <summary>
		/// Escreve na janela do Debug uma string e logo em seguida o caracter de nova linha.
		/// </summary>
		/// <param name="value">string para ser esctrita</param>
		public override void WriteLine(string value)
		{
			Debug.WriteLine(value);
		}

		/// <summary>
		/// Escreve na janela do Debug uma string.
		/// </summary>
		/// <param name="value">string para ser esctrita</param>
		public override void Write(string value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Retorna a codificação utilizada.
		/// </summary>
		public override Encoding Encoding
		{
			get { return Encoding.Unicode; }
		}
	}
}
