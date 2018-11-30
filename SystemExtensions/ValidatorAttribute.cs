using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ValidatorAttribute : Attribute
	{
		public ValidatorAttribute()
		{

		}

		public ValidatorAttribute(string pName, object pEmpty)
		{
			EmptyValue = pEmpty;
			Name = pName;
		}

		public object EmptyValue { get; set; }

		public string Name { get; set; }
	}
}
