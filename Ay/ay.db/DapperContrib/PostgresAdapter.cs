using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

public class PostgresAdapter : ISqlAdapter
{
	public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);
		PropertyInfo[] array = (keyProperties as PropertyInfo[]) ?? keyProperties.ToArray();
		PropertyInfo[] array2;
		if (!array.Any())
		{
			stringBuilder.Append(" RETURNING *");
		}
		else
		{
			stringBuilder.Append(" RETURNING ");
			bool flag = true;
			array2 = array;
			foreach (PropertyInfo propertyInfo in array2)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append(propertyInfo.Name);
			}
		}
		List<object> source = SqlMapper.Query(connection, stringBuilder.ToString(), entityToInsert, transaction, true, commandTimeout, (CommandType?)null).ToList();
		int num = 0;
		array2 = array;
		foreach (PropertyInfo propertyInfo2 in array2)
		{
			object value = ((IDictionary<string, object>)source.First())[propertyInfo2.Name.ToLower()];
			propertyInfo2.SetValue(entityToInsert, value, null);
			if (num == 0)
			{
				num = Convert.ToInt32(value);
			}
		}
		return num;
	}

	public void AppendColumnName(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("\"{0}\"", columnName);
	}

	public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
	}
}
