using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

public class SqlCeServerAdapter : ISqlAdapter
{
	public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
	{
		string text = string.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);
		SqlMapper.Execute(connection, text, entityToInsert, transaction, commandTimeout, (CommandType?)null);
		List<object> source = SqlMapper.Query(connection, "select @@IDENTITY id", (object)null, transaction, true, commandTimeout, (CommandType?)null).ToList();
		if (((dynamic)source.First()).id == null)
		{
			return 0;
		}
		int num = (int)((dynamic)source.First()).id;
		PropertyInfo[] source2 = (keyProperties as PropertyInfo[]) ?? keyProperties.ToArray();
		if (!source2.Any())
		{
			return num;
		}
		PropertyInfo propertyInfo = source2.First();
		propertyInfo.SetValue(entityToInsert, Convert.ChangeType(num, propertyInfo.PropertyType), null);
		return num;
	}

	public void AppendColumnName(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("[{0}]", columnName);
	}

	public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
	}
}
