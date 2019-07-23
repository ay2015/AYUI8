using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace Dapper.Contrib.Extensions
{
	public static class SqlMapperExtensions
	{
		public interface IProxy
		{
			bool IsDirty
			{
				get;
				set;
			}
		}

		public interface ITableNameMapper
		{
			string GetTableName(Type type);
		}

		public delegate string GetDatabaseTypeDelegate(IDbConnection connection);

		public delegate string TableNameMapperDelegate(Type type);

		private static class ProxyGenerator
		{
			private static readonly Dictionary<Type, Type> TypeCache = new Dictionary<Type, Type>();

			private static AssemblyBuilder GetAsmBuilder(string name)
			{
				return Thread.GetDomain().DefineDynamicAssembly(new AssemblyName
				{
					Name = name
				}, AssemblyBuilderAccess.Run);
			}

			public static T GetInterfaceProxy<T>()
			{
				Type typeFromHandle = typeof(T);
				Type value;
				if (TypeCache.TryGetValue(typeFromHandle, out value))
				{
					return (T)Activator.CreateInstance(value);
				}
				ModuleBuilder moduleBuilder = GetAsmBuilder(typeFromHandle.Name).DefineDynamicModule("SqlMapperExtensions." + typeFromHandle.Name);
				Type typeFromHandle2 = typeof(IProxy);
				TypeBuilder typeBuilder = moduleBuilder.DefineType(typeFromHandle.Name + "_" + Guid.NewGuid(), TypeAttributes.Public);
				typeBuilder.AddInterfaceImplementation(typeFromHandle);
				typeBuilder.AddInterfaceImplementation(typeFromHandle2);
				MethodInfo setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);
				PropertyInfo[] properties = typeof(T).GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					bool isIdentity = propertyInfo.GetCustomAttributes(true).Any((object a) => a is KeyAttribute);
					CreateProperty<T>(typeBuilder, propertyInfo.Name, propertyInfo.PropertyType, setIsDirtyMethod, isIdentity);
				}
				Type type = typeBuilder.CreateType();
				TypeCache.Add(typeFromHandle, type);
				return (T)Activator.CreateInstance(type);
			}

			private static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
			{
				Type typeFromHandle = typeof(bool);
				FieldBuilder field = typeBuilder.DefineField("_IsDirty", typeFromHandle, FieldAttributes.Private);
				PropertyBuilder propertyBuilder = typeBuilder.DefineProperty("IsDirty", System.Reflection.PropertyAttributes.None, typeFromHandle, new Type[1]
				{
					typeFromHandle
				});
				MethodBuilder methodBuilder = typeBuilder.DefineMethod("get_IsDirty", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, typeFromHandle, Type.EmptyTypes);
				ILGenerator iLGenerator = methodBuilder.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldfld, field);
				iLGenerator.Emit(OpCodes.Ret);
				MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("set_IsDirty", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, null, new Type[1]
				{
					typeFromHandle
				});
				ILGenerator iLGenerator2 = methodBuilder2.GetILGenerator();
				iLGenerator2.Emit(OpCodes.Ldarg_0);
				iLGenerator2.Emit(OpCodes.Ldarg_1);
				iLGenerator2.Emit(OpCodes.Stfld, field);
				iLGenerator2.Emit(OpCodes.Ret);
				propertyBuilder.SetGetMethod(methodBuilder);
				propertyBuilder.SetSetMethod(methodBuilder2);
				MethodInfo method = typeof(IProxy).GetMethod("get_IsDirty");
				MethodInfo method2 = typeof(IProxy).GetMethod("set_IsDirty");
				typeBuilder.DefineMethodOverride(methodBuilder, method);
				typeBuilder.DefineMethodOverride(methodBuilder2, method2);
				return methodBuilder2;
			}

			private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod, bool isIdentity)
			{
				FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
				PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, System.Reflection.PropertyAttributes.None, propType, new Type[1]
				{
					propType
				});
				MethodBuilder methodBuilder = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, propType, Type.EmptyTypes);
				ILGenerator iLGenerator = methodBuilder.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldfld, field);
				iLGenerator.Emit(OpCodes.Ret);
				MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("set_" + propertyName, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[1]
				{
					propType
				});
				ILGenerator iLGenerator2 = methodBuilder2.GetILGenerator();
				iLGenerator2.Emit(OpCodes.Ldarg_0);
				iLGenerator2.Emit(OpCodes.Ldarg_1);
				iLGenerator2.Emit(OpCodes.Stfld, field);
				iLGenerator2.Emit(OpCodes.Ldarg_0);
				iLGenerator2.Emit(OpCodes.Ldc_I4_1);
				iLGenerator2.Emit(OpCodes.Call, setIsDirtyMethod);
				iLGenerator2.Emit(OpCodes.Ret);
				if (isIdentity)
				{
					CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(typeof(KeyAttribute).GetConstructor(new Type[0]), new object[0]);
					propertyBuilder.SetCustomAttribute(customAttribute);
				}
				propertyBuilder.SetGetMethod(methodBuilder);
				propertyBuilder.SetSetMethod(methodBuilder2);
				MethodInfo method = typeof(T).GetMethod("get_" + propertyName);
				MethodInfo method2 = typeof(T).GetMethod("set_" + propertyName);
				typeBuilder.DefineMethodOverride(methodBuilder, method);
				typeBuilder.DefineMethodOverride(methodBuilder2, method2);
			}
		}

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

		private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

		private static readonly ISqlAdapter DefaultAdapter = new SqlServerAdapter();

		private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary = new Dictionary<string, ISqlAdapter>
		{
			{
				"sqlconnection",
				new SqlServerAdapter()
			},
			{
				"sqlceconnection",
				new SqlCeServerAdapter()
			},
			{
				"npgsqlconnection",
				new PostgresAdapter()
			},
			{
				"sqliteconnection",
				new SQLiteAdapter()
			},
			{
				"mysqlconnection",
				new MySqlAdapter()
			}
		};

		public static TableNameMapperDelegate TableNameMapper;

		public static GetDatabaseTypeDelegate GetDatabaseType;

		private static List<PropertyInfo> ComputedPropertiesCache(Type type)
		{
			IEnumerable<PropertyInfo> value;
			if (ComputedProperties.TryGetValue(type.TypeHandle, out value))
			{
				return value.ToList();
			}
			List<PropertyInfo> list = (from p in TypePropertiesCache(type)
			where p.GetCustomAttributes(true).Any((object a) => a is ComputedAttribute)
			select p).ToList();
			ComputedProperties[type.TypeHandle] = list;
			return list;
		}

		private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
		{
			IEnumerable<PropertyInfo> value;
			if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out value))
			{
				return value.ToList();
			}
			List<PropertyInfo> list = (from p in TypePropertiesCache(type)
			where p.GetCustomAttributes(true).Any((object a) => a is ExplicitKeyAttribute)
			select p).ToList();
			ExplicitKeyProperties[type.TypeHandle] = list;
			return list;
		}

		private static List<PropertyInfo> KeyPropertiesCache(Type type)
		{
			IEnumerable<PropertyInfo> value;
			if (KeyProperties.TryGetValue(type.TypeHandle, out value))
			{
				return value.ToList();
			}
			List<PropertyInfo> source = TypePropertiesCache(type);
			List<PropertyInfo> list = (from p in source
			where p.GetCustomAttributes(true).Any((object a) => a is KeyAttribute)
			select p).ToList();
			if (list.Count == 0)
			{
				PropertyInfo propertyInfo = source.FirstOrDefault((PropertyInfo p) => p.Name.ToLower() == "id");
				if (propertyInfo != null && !propertyInfo.GetCustomAttributes(true).Any((object a) => a is ExplicitKeyAttribute))
				{
					list.Add(propertyInfo);
				}
			}
			KeyProperties[type.TypeHandle] = list;
			return list;
		}

		private static List<PropertyInfo> TypePropertiesCache(Type type)
		{
			IEnumerable<PropertyInfo> value;
			if (TypeProperties.TryGetValue(type.TypeHandle, out value))
			{
				return value.ToList();
			}
			PropertyInfo[] array = type.GetProperties().Where(IsWriteable).ToArray();
			TypeProperties[type.TypeHandle] = array;
			return array.ToList();
		}

		private static bool IsWriteable(PropertyInfo pi)
		{
			List<object> list = SqlMapper.AsList<object>((IEnumerable<object>)pi.GetCustomAttributes(typeof(WriteAttribute), false));
			if (list.Count != 1)
			{
				return true;
			}
			return ((WriteAttribute)list[0]).Write;
		}

		private static PropertyInfo GetSingleKey<T>(string method)
		{
			Type typeFromHandle = typeof(T);
			List<PropertyInfo> list = KeyPropertiesCache(typeFromHandle);
			List<PropertyInfo> list2 = ExplicitKeyPropertiesCache(typeFromHandle);
			int num = list.Count + list2.Count;
			if (num > 1)
			{
				throw new DataException(string.Format("{0}<T> only supports an entity with a single [Key] or [ExplicitKey] property", method));
			}
			if (num == 0)
			{
				throw new DataException(string.Format("{0}<T> only supports an entity with a [Key] or an [ExplicitKey] property", method));
			}
			if (!list.Any())
			{
				return list2.First();
			}
			return list.First();
		}

		public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			Type typeFromHandle = typeof(T);
			string value;
			if (!GetQueries.TryGetValue(typeFromHandle.TypeHandle, out value))
			{
				PropertyInfo singleKey = GetSingleKey<T>("Get");
				string tableName = GetTableName(typeFromHandle);
				value = string.Format("select * from {0} where {1} = @id", tableName, singleKey.Name);
				GetQueries[typeFromHandle.TypeHandle] = value;
			}
			DynamicParameters val = new DynamicParameters();
			val.Add("@id", id);
			T val2;
			if (typeFromHandle.IsInterface())
			{
				IDictionary<string, object> dictionary = SqlMapper.Query(connection, value, (object)val, (IDbTransaction)null, true, (int?)null, (CommandType?)null).FirstOrDefault() as IDictionary<string, object>;
				if (dictionary == null)
				{
					return null;
				}
				val2 = ProxyGenerator.GetInterfaceProxy<T>();
				foreach (PropertyInfo item in TypePropertiesCache(typeFromHandle))
				{
					object value2 = dictionary[item.Name];
					item.SetValue(val2, Convert.ChangeType(value2, item.PropertyType), null);
				}
				((IProxy)val2).IsDirty = false;
			}
			else
			{
				val2 = SqlMapper.Query<T>(connection, value, (object)val, transaction, true, commandTimeout, (CommandType?)null).FirstOrDefault();
			}
			return val2;
		}

		public static IEnumerable<T> GetAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(List<T>);
			string value;
			if (!GetQueries.TryGetValue(typeFromHandle2.TypeHandle, out value))
			{
				GetSingleKey<T>("GetAll");
				string tableName = GetTableName(typeFromHandle);
				value = "select * from " + tableName;
				GetQueries[typeFromHandle2.TypeHandle] = value;
			}
			if (!typeFromHandle.IsInterface())
			{
				return SqlMapper.Query<T>(connection, value, (object)null, transaction, true, commandTimeout, (CommandType?)null);
			}
			IEnumerable<object> enumerable = SqlMapper.Query(connection, value, (object)null, (IDbTransaction)null, true, (int?)null, (CommandType?)null);
			List<T> list = new List<T>();
			foreach (object item in enumerable)
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)item;
				T interfaceProxy = ProxyGenerator.GetInterfaceProxy<T>();
				foreach (PropertyInfo item2 in TypePropertiesCache(typeFromHandle))
				{
					object value2 = dictionary[item2.Name];
					item2.SetValue(interfaceProxy, Convert.ChangeType(value2, item2.PropertyType), null);
				}
				((IProxy)interfaceProxy).IsDirty = false;
				list.Add(interfaceProxy);
			}
			return list;
		}

		private static string GetTableName(Type type)
		{
			string value;
			if (TypeTableName.TryGetValue(type.TypeHandle, out value))
			{
				return value;
			}
			if (TableNameMapper != null)
			{
				value = TableNameMapper(type);
			}
			else
			{
				dynamic val = type.GetCustomAttributes(false).SingleOrDefault((object attr) => attr.GetType().Name == "TableAttribute");
				if (val != null)
				{
					value = (string)val.Name;
				}
				else
				{
					value = type.Name + "s";
					if (type.IsInterface() && value.StartsWith("I"))
					{
						value = value.Substring(1);
					}
				}
			}
			TypeTableName[type.TypeHandle] = value;
			return value;
		}

		public static long Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			bool flag = false;
			Type type = typeof(T);
			if (type.IsArray)
			{
				flag = true;
				type = type.GetElementType();
			}
			else if (type.IsGenericType())
			{
				flag = true;
				type = type.GetGenericArguments()[0];
			}
			string tableName = GetTableName(type);
			StringBuilder stringBuilder = new StringBuilder(null);
			List<PropertyInfo> first = TypePropertiesCache(type);
			List<PropertyInfo> list = KeyPropertiesCache(type);
			List<PropertyInfo> second = ComputedPropertiesCache(type);
			List<PropertyInfo> list2 = first.Except(list.Union(second)).ToList();
			ISqlAdapter formatter = GetFormatter(connection);
			for (int i = 0; i < list2.Count; i++)
			{
				PropertyInfo propertyInfo = list2.ElementAt(i);
				formatter.AppendColumnName(stringBuilder, propertyInfo.Name);
				if (i < list2.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder(null);
			for (int j = 0; j < list2.Count; j++)
			{
				PropertyInfo propertyInfo2 = list2.ElementAt(j);
				stringBuilder2.AppendFormat("@{0}", propertyInfo2.Name);
				if (j < list2.Count - 1)
				{
					stringBuilder2.Append(", ");
				}
			}
			bool num = connection.State == ConnectionState.Closed;
			if (num)
			{
				connection.Open();
			}
			int num2;
			if (!flag)
			{
				num2 = formatter.Insert(connection, transaction, commandTimeout, tableName, stringBuilder.ToString(), stringBuilder2.ToString(), list, entityToInsert);
			}
			else
			{
				string text = string.Format("insert into {0} ({1}) values ({2})", tableName, stringBuilder, stringBuilder2);
				num2 = SqlMapper.Execute(connection, text, (object)entityToInsert, transaction, commandTimeout, (CommandType?)null);
			}
			if (num)
			{
				connection.Close();
			}
			return num2;
		}

		public static bool Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			IProxy proxy = entityToUpdate as IProxy;
			if (proxy != null && !proxy.IsDirty)
			{
				return false;
			}
			Type type = typeof(T);
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			else if (type.IsGenericType())
			{
				type = type.GetGenericArguments()[0];
			}
			List<PropertyInfo> list = KeyPropertiesCache(type).ToList();
			List<PropertyInfo> list2 = ExplicitKeyPropertiesCache(type);
			if (!list.Any() && !list2.Any())
			{
				throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");
			}
			string tableName = GetTableName(type);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("update {0} set ", tableName);
			List<PropertyInfo> first = TypePropertiesCache(type);
			list.AddRange(list2);
			List<PropertyInfo> second = ComputedPropertiesCache(type);
			List<PropertyInfo> list3 = first.Except(list.Union(second)).ToList();
			ISqlAdapter formatter = GetFormatter(connection);
			for (int i = 0; i < list3.Count; i++)
			{
				PropertyInfo propertyInfo = list3.ElementAt(i);
				formatter.AppendColumnNameEqualsValue(stringBuilder, propertyInfo.Name);
				if (i < list3.Count - 1)
				{
					stringBuilder.AppendFormat(", ");
				}
			}
			stringBuilder.Append(" where ");
			for (int j = 0; j < list.Count; j++)
			{
				PropertyInfo propertyInfo2 = list.ElementAt(j);
				formatter.AppendColumnNameEqualsValue(stringBuilder, propertyInfo2.Name);
				if (j < list.Count - 1)
				{
					stringBuilder.AppendFormat(" and ");
				}
			}
			return SqlMapper.Execute(connection, stringBuilder.ToString(), (object)entityToUpdate, transaction, commandTimeout, (CommandType?)null) > 0;
		}

		public static bool Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			if (entityToDelete == null)
			{
				throw new ArgumentException("Cannot Delete null Object", "entityToDelete");
			}
			Type type = typeof(T);
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			else if (type.IsGenericType())
			{
				type = type.GetGenericArguments()[0];
			}
			List<PropertyInfo> list = KeyPropertiesCache(type).ToList();
			List<PropertyInfo> list2 = ExplicitKeyPropertiesCache(type);
			if (!list.Any() && !list2.Any())
			{
				throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");
			}
			string tableName = GetTableName(type);
			list.AddRange(list2);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("delete from {0} where ", tableName);
			ISqlAdapter formatter = GetFormatter(connection);
			for (int i = 0; i < list.Count; i++)
			{
				PropertyInfo propertyInfo = list.ElementAt(i);
				formatter.AppendColumnNameEqualsValue(stringBuilder, propertyInfo.Name);
				if (i < list.Count - 1)
				{
					stringBuilder.AppendFormat(" and ");
				}
			}
			return SqlMapper.Execute(connection, stringBuilder.ToString(), (object)entityToDelete, transaction, commandTimeout, (CommandType?)null) > 0;
		}

		public static bool DeleteAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
		{
			string tableName = GetTableName(typeof(T));
			string text = string.Format("delete from {0}", tableName);
			return SqlMapper.Execute(connection, text, (object)null, transaction, commandTimeout, (CommandType?)null) > 0;
		}

		private static ISqlAdapter GetFormatter(IDbConnection connection)
		{
			GetDatabaseTypeDelegate getDatabaseType = GetDatabaseType;
			string key = ((getDatabaseType != null) ? getDatabaseType(connection).ToLower() : null) ?? connection.GetType().Name.ToLower();
			if (AdapterDictionary.ContainsKey(key))
			{
				return AdapterDictionary[key];
			}
			return DefaultAdapter;
		}
	}
}
