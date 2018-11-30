using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public class FriendlyNameAttribute : Attribute
	{
		public FriendlyNameAttribute(string pNameSingle, string pNamePlural)
		{
			_nameSingle = pNameSingle;
			_namePlural = pNamePlural;
		}

		public FriendlyNameAttribute(string pNameSingle)
		{
			_nameSingle = pNameSingle;
			_namePlural = "";
		}

		private readonly string _nameSingle;
		public string NameSingle
		{
			get { return _nameSingle; }
		}

		private readonly string _namePlural;
		public string NamePlural
		{
			get { return _namePlural; }
		}

	}
}
