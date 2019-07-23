using System;

namespace Dapper.Contrib.Extensions
{
	[AttributeUsage(AttributeTargets.Property)]
	public class WriteAttribute : Attribute
	{
		public bool Write
		{
			get;
		}

		public WriteAttribute(bool write)
		{
			Write = write;
		}
	}
}
