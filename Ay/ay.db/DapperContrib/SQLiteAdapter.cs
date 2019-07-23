using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;

public class SQLiteAdapter : ISqlAdapter
{
	public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
	{
		string text = string.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT last_insert_rowid() id", tableName, columnList, parameterList);
		GridReader val = SqlMapper.QueryMultiple(connection, text, entityToInsert, transaction, commandTimeout, (CommandType?)null);
		int num = (int)((dynamic)val.Read(true).First()).id;
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
		sb.AppendFormat("\"{0}\"", columnName);
	}

	public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
	{
		sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
	}
}
