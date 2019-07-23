using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

    public class WpfTypeUtilities
    {
        public static IEnumerable CreateDynamicallyTypedList(IEnumerable source)
        {
            Type type = GetCommonBaseClass(source);
            Type listType = typeof(List<>).MakeGenericType(type);
            MethodInfo addMethod = listType.GetMethod("Add");
            object list = listType.GetConstructor(Type.EmptyTypes).Invoke(null);

            foreach (object o in source)
            {
                addMethod.Invoke(list, new[] { o });
            }

            return (IEnumerable)list;
        }

        public static Type GetCommonBaseClass(IEnumerable e)
        {
            Type[] types = e.Cast<object>().Select(o => o.GetType()).ToArray<Type>();
            return GetCommonBaseClass(types);
        }

        public static Type GetCommonBaseClass(Type[] types)
        {
            if (types.Length == 0)
                return typeof(object);

            Type ret = types[0];

            for (int i = 1; i < types.Length; ++i)
            {
                if (types[i].IsAssignableFrom(ret))
                    ret = types[i];
                else
                {
                    // This will always terminate when ret == typeof(object)
                    while (!ret.IsAssignableFrom(types[i]))
                        ret = ret.BaseType;
                }
            }

            return ret;
        }
    }

