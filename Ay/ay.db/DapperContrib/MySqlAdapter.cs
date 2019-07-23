using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

public class MySqlAdapter : ISqlAdapter
{
	public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
	{
		string text = string.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);
		SqlMapper.Execute(connection, text, entityToInsert, transaction, commandTimeout, (CommandType?)null);
		IEnumerable<object> source = SqlMapper.Query(connection, "Select LAST_INSERT_ID() id", (object)null, transaction, true, commandTimeout, (CommandType?)null);
		dynamic val = ((dynamic)source.First()).id;
		if (!((val == null) ? true : false))
		{
			PropertyInfo[] source2 = (keyProperties as PropertyInfo[]) ?? keyProperties.ToArray();
			if (source2.Any())
			{
				PropertyInfo propertyInfo = source2.First();
				propertyInfo.SetValue(entityToInsert, Convert.ChangeType(val, propertyInfo.PropertyType), null);
				return (int)Convert.ToInt32(val);
			}
			return (int)Convert.ToInt32(val);
		}
		return 0;
	}

	public void AppendColumnName(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("`{0}`", columnName);
	}

	public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("`{0}` = @{1}", columnName, columnName);
	}
}
