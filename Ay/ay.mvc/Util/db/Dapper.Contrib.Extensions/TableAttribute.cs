using System;

namespace Dapper.Contrib.Extensions
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public TableAttribute(string tableName)
		{
			Name = tableName;
		}
	}
}
