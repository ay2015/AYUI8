using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace Ay.MvcFramework
{
	/// <summary>
	/// 动态属性访问工具类。
	/// </summary>
	public sealed class DynaAccessUtils
	{
		private static readonly char[] DELIM_CHARS = new char[] {'.','[',']'};

		/// <summary>
		/// 线程安全的类型属性Map(Type,IDynaAccessProxy)
		/// </summary>
		private static Hashtable _proxyCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// 数组或者IList实例的代理
		/// </summary>
		private static IDynaAccessProxy _listProxy = new ListProxy();

		/// <summary>
		/// IDictionary实例的代理
		/// </summary>
		private static IDynaAccessProxy _dictionaryProxy = new DictionaryProxy();

		private DynaAccessUtils()
		{
		}

		/// <summary>
		/// 得到指定对象实例的属性值
		/// </summary>
		public static System.Object GetProperty(System.Object target, System.String propertyName)
		{
			if (target == null)
			{
				throw new System.ArgumentException("No target specified");
			}
			if (propertyName == null)
			{
				throw new System.ArgumentException("No propertyName specified");
			}

			// 得到嵌套列表
			string[] names = PickPropertyNames(propertyName);
			int len = names.Length;
			object child = target;

			for(int i = 0; i<len ;i++)
			{
				IDynaAccessProxy proxy = GetObjectProxy(child);
				child = proxy.GetProperty(child,names[i]);
				if(child == null)
					break;
			}
			return child;
		}

		/// <summary>
		/// 设置指定对象实例的属性值
		/// </summary>
		public static void  SetProperty(System.Object target, System.String propertyName, System.Object propertyValue)
		{
			if (target == null)
			{
				throw new System.ArgumentException("No target specified");
			}
			if (propertyName == null)
			{
				throw new System.ArgumentException("No name specified");
			}

			string[] names = PickPropertyNames(propertyName);
			int len = names.Length;
			object child = target;

			for(int i = 0; i<(len - 1) ;i++)
			{
				IDynaAccessProxy proxy = GetObjectProxy(child);
				object parent = child;
				child = proxy.GetProperty(parent,names[i]);
				if(child == null)
				{
					try 
					{
						child = Activator.CreateInstance(proxy.GetPropertyType(parent,names[i]));
						proxy.SetProperty(parent,names[i],child);
					} 
					catch (Exception e) 
					{
						throw new DynaAccessException(e);
					}
				}
			}
			IDynaAccessProxy setProxy = GetObjectProxy(child);
			setProxy.SetProperty(child,names[len - 1],propertyValue);
		}

		/// <summary>
		/// 得到指定对象实例的属性名称字串数组
		/// </summary>
		public static string[] GetPropertyNames(object target)
		{
			IDynaAccessProxy proxy = GetObjectProxy(target);
			return proxy.GetPropertyNames(target);
		}

		/// <summary>
		/// 判断指定对象实例是否存在该属性
		/// </summary>
		public static bool HasProperty(object target , string propertyName)
		{
			if (target == null || propertyName==null)
			{
				return false;
			}

			string[] names = PickPropertyNames(propertyName);
			int len = names.Length;
			object child = target;

			for(int i = 0; i<(len - 1) ;i++)
			{
				IDynaAccessProxy proxy = GetObjectProxy(child);
				object parent = child;
				child = proxy.GetProperty(parent,names[i]);
				if(child == null)
				{
					try 
					{
						child = Activator.CreateInstance(proxy.GetPropertyType(parent,names[i]));
					} 
					catch (Exception) 
					{
						return false;
					}
				}
			}
			IDynaAccessProxy endProxy = GetObjectProxy(child);
			return endProxy.HasProperty(child, names[len - 1]);
		}

		/// <summary>
		/// 得到指定对象实例中属性的类型
		/// </summary>
		public static Type GetPropertyType(object target , string propertyName)
		{
			if (target == null)
			{
				throw new System.ArgumentException("No target specified");
			}
			if (propertyName == null)
			{
				throw new System.ArgumentException("No name specified");
			}

			string[] names = PickPropertyNames(propertyName);
			int len = names.Length;
			object child = target;

			for(int i = 0; i<(len - 1) ;i++)
			{
				IDynaAccessProxy proxy = GetObjectProxy(child);
				object parent = child;
				child = proxy.GetProperty(parent,names[i]);
				if(child == null)
				{
					try 
					{
						child = Activator.CreateInstance(proxy.GetPropertyType(parent,names[i]));
					} 
					catch (Exception e) 
					{
						throw new DynaAccessException(e);
					}
				}
			}
			IDynaAccessProxy endProxy = GetObjectProxy(child);
			return endProxy.GetPropertyType(child, names[len - 1]);
		}

		/// <summary>
		/// 得到指定对象实例的动态属性访问代理
		/// </summary>
		public static IDynaAccessProxy GetObjectProxy(object target)
		{
			if (target == null)
			{
				throw new System.ArgumentException("No target specified");
			}

			if(target is IDictionary)
				return _dictionaryProxy;
			else if( target is IList)
				return _listProxy;
			else
			{
				Type type = target.GetType();

				lock (type) 
				{
					IDynaAccessProxy cache = (IDynaAccessProxy) _proxyCache[type];
					if (cache == null) 
					{
						cache = new BasicProxy(type);
						_proxyCache.Add(type, cache);
					}
					return cache;
				}
			}
		}

		private static string [] PickPropertyNames(string propertyName)
		{
			string[] childNames = propertyName.Trim().Split(DELIM_CHARS);
			ArrayList list = new ArrayList(childNames.Length);
			foreach(string name in childNames)
			{
				if(name.Length==0)
					continue;
				list.Add(name);
			}
			childNames = new string[list.Count];
			list.CopyTo(childNames,0);
			return childNames;
		} 
        
	}
}
