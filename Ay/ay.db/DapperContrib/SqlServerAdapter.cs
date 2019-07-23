using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

public class SqlServerAdapter : ISqlAdapter
{
	public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
	{
		string text = string.Format("insert into {0} ({1}) values ({2});select SCOPE_IDENTITY() id", tableName, columnList, parameterList);
		dynamic val = SqlMapper.QueryMultiple(connection, text, entityToInsert, transaction, commandTimeout, (CommandType?)null).Read(true).FirstOrDefault();
		if (val == null || val.id == null)
		{
			return 0;
		}
		int num = (int)val.id;
		PropertyInfo[] source = (keyProperties as PropertyInfo[]) ?? keyProperties.ToArray();
		if (!source.Any())
		{
			return num;
		}
		PropertyInfo propertyInfo = source.First();
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
