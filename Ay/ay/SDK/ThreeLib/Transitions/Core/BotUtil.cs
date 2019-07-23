using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using Ay.Framework.WPF;
using System.Windows;
#if CONTRACTS_FULL
using System.Diagnostics.Contracts;
#else
using PixelLab.Contracts;
#endif

namespace PixelLab.Common
{
    /// <summary>
    ///     Contains general helper methods.
    /// </summary>
    public static class BotUtil
    {
        ///// <summary>
        ///// Returns an hash aggregation of an array of elements.
        ///// </summary>
        ///// <param name="items">An array of elements from which to create a hash.</param>
        //public static int GetHashCode(params object[] items)
        //{
        //    items = items ?? new object[0];

        //    return items
        //        .Select(item => (item == null) ? 0 : item.GetHashCode())
        //        .Aggregate(0, (current, next) =>
        //        {
        //            unchecked
        //            {
        //                return (current * 397) ^ next;
        //            }
        //        });
        //}

        /// <summary>
        ///     Wraps <see cref="Interlocked.CompareExchange{T}(ref T,T,T)"/>
        ///     for atomically setting null fields.
        /// </summary>
        /// <typeparam name="T">The type of the field to set.</typeparam>
        /// <param name="location">
        ///     The field that, if null, will be set to <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        ///     If <paramref name="location"/> is null, the object to set it to.
        /// </param>
        /// <returns>true if <paramref name="location"/> was null and has now been set; otherwise, false.</returns>
        [Obsolete("The name of this method is pretty wrong. Use InterlockedSetNullField instead.")]
        public static bool InterlockedSetIfNotNull<T>(ref T location, T value) where T : class
        {
            return InterlockedSetNullField<T>(ref location, value);
        }

        /// <summary>
        ///     Wraps <see cref="Interlocked.CompareExchange{T}(ref T,T,T)"/>
        ///     for atomically setting null fields.
        /// </summary>
        /// <typeparam name="T">The type of the field to set.</typeparam>
        /// <param name="location">
        ///     The field that, if null, will be set to <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        ///     If <paramref name="location"/> is null, the object to set it to.
        /// </param>
        /// <returns>true if <paramref name="location"/> was null and has now been set; otherwise, false.</returns>
        public static bool InterlockedSetNullField<T>(ref T location, T value) where T : class
        {
            Contract.Requires(value != null);

            // Strictly speaking, this null check is not nessesary, but
            // while CompareExchange is fast, it's still much slower than a
            // null check.
            if (location == null)
            {
                // This is a paranoid method. In a multi-threaded environment, it's possible
                // for two threads to get through the null check before a value is set.
                // This makes sure than one and only one value is set to field.
                // This is super important if the field is used in locking, for instance.

                var valueWhenSet = Interlocked.CompareExchange<T>(ref location, value, null);
                return (valueWhenSet == null);
            }
            else
            {
                return false;
            }
        }

        /// Returns true if the provided <see cref="Exception"/> is considered 'critical'
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to evaluate for critical-ness.</param>
        /// <returns>true if the Exception is conisdered critical; otherwise, false.</returns>
        /// <remarks>
        /// These exceptions are consider critical:
        /// <list type="bullets">
        ///     <item><see cref="OutOfMemoryException"/></item>
        ///     <item><see cref="StackOverflowException"/></item>
        ///     <item><see cref="ThreadAbortException"/></item>
        ///     <item><see cref="SEHException"/></item>
        /// </list>
        /// </remarks>
        public static bool IsCriticalException(this Exception exception)
        {
            // Copied with respect from WPF WindowsBase->MS.Internal.CriticalExceptions.IsCriticalException
            // NullReferencException, SecurityException --> not going to consider these critical
            while (exception != null)
            {
                if (exception is OutOfMemoryException ||
                        exception is StackOverflowException ||
                        exception is ThreadAbortException
#if !WP7
 || exception is System.Runtime.InteropServices.SEHException
#endif
)
                {
                    return true;
                }
                exception = exception.InnerException;
            }
            return false;
        } //*** static IsCriticalException

        [DebuggerStepThrough]
        public static void ThrowUnless(bool truth, string message = null)
        {
            ThrowUnless<Exception>(truth, message);
        }

        [DebuggerStepThrough]
        public static void ThrowUnless<TException>(bool truth, string message) where TException : Exception
        {
            if (!truth)
            {
                throw InstanceFactory.CreateInstance<TException>(message);
            }
        }

        [DebuggerStepThrough]
        public static void ThrowUnless<TException>(bool truth) where TException : Exception, new()
        {
            if (!truth)
            {
                throw new TException();
            }
        }

        public static IEqualityComparer<T> ToEqualityComparer<T>(this Func<T, T, bool> func)
        {
            Contract.Requires(func != null);
            return new FuncEqualityComparer<T>(func);
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> compareFunction)
        {
            Contract.Requires(compareFunction != null);
            return new FuncComparer<T>(compareFunction);
        }

        public static IComparer<T> ToComparer<T>(this Comparison<T> compareFunction)
        {
            Contract.Requires(compareFunction != null);
            return new ComparisonComparer<T>(compareFunction);
        }

        public static IComparer<string> ToComparer<T>(this CompareInfo compareInfo)
        {
            Contract.Requires(compareInfo != null);
            return new FuncComparer<string>(compareInfo.Compare);
        }



        /// <summary>
        /// Verifies that a property name exists in this ViewModel. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        /// <para>This method is only active in DEBUG mode.</para>
        /// </summary>
        /// <param name="element">The object to watch.</param>
        /// <remarks>Thanks to Laurent Bugnion for the idea.</remarks>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void VerifyPropertyNamesOnChange(this INotifyPropertyChanged element)
        {
            Contract.Requires(element != null);
            var myType = element.GetType();
            element.PropertyChanged += (sender, args) =>
            {
                BotUtil.ThrowUnless<InvalidOperationException>(myType.HasPublicInstanceProperty(args.PropertyName), "The object '{0}' of type '{1}' raised a property change for '{2}' which isn't a public property on the type.".StringFormat(element, myType, args.PropertyName));
            };
        }


        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            Contract.Requires(attributeProvider != null);
            return attributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        [Pure]
        public static bool HasPublicInstanceProperty(this IReflect type, string name)
        {
            Contract.Requires(type != null);
            return type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance) != null;
        }

        public static PropertyChangeWatcher AddWatcher(this INotifyPropertyChanged source, string propertyName, Action handler)
        {
            return PropertyChangeWatcher.AddWatcher(source, new[] { propertyName }, handler);
        }

        public static PropertyChangeWatcher AddWatcher(this INotifyPropertyChanged source, string propertyName1, string propertyName2, Action handler)
        {
            return PropertyChangeWatcher.AddWatcher(source, new[] { propertyName1, propertyName2 }, handler);
        }

        public static PropertyChangeWatcher AddWatcher(this INotifyPropertyChanged source, IList<string> propertyNames, Action handler)
        {
            return PropertyChangeWatcher.AddWatcher(source, propertyNames, handler);
        }

        public static IComparer<string> GetStringComparer(this CultureInfo cultureInfo, CompareOptions options = CompareOptions.None)
        {
            Contract.Requires(cultureInfo != null);
            var func = new Func<string, string, int>((a, b) => cultureInfo.CompareInfo.Compare(a, b, options));
            return func.ToComparer();
        }

        #region impl
        private class FuncComparer<T> : IComparer<T>
        {
            public FuncComparer(Func<T, T, int> func)
            {
                Contract.Requires(func != null);
                m_func = func;
            }

            public int Compare(T x, T y)
            {
                return m_func(x, y);
            }

            private readonly Func<T, T, int> m_func;
        }

        private class ComparisonComparer<T> : IComparer<T>
        {
            public ComparisonComparer(Comparison<T> func)
            {
                Contract.Requires(func != null);
                m_func = func;
            }

            public int Compare(T x, T y)
            {
                return m_func(x, y);
            }

            private readonly Comparison<T> m_func;
        }

        private class FuncEqualityComparer<T> : IEqualityComparer<T>
        {
            public FuncEqualityComparer(Func<T, T, bool> func)
            {
                Contract.Requires(func != null);
                m_func = func;
            }
            public bool Equals(T x, T y)
            {
                return m_func(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0; // this is on purpose. Should only use function...not short-cut by hashcode compare
            }

            [ContractInvariantMethod]
            void ObjectInvariant()
            {
                Contract.Invariant(m_func != null);
            }

            private readonly Func<T, T, bool> m_func;
        }
        #endregion




        #region 2016-8-4 13:36:13  数学处理
        [Pure]
        public static bool IsValid(this double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        [Pure]
        public static bool IsValid(this Point value)
        {
            return value.X.IsValid() && value.Y.IsValid();
        }

        [Pure]
        public static bool IsValid(this Size value)
        {
            return value.Width.IsValid() && value.Height.IsValid();
        }

        [Pure]
        public static bool IsValid(this Vector value)
        {
            return value.X.IsValid() && value.Y.IsValid();
        }
        public static Vector Subtract(this Point point, Point other)
        {
            Contract.Requires(point.IsValid());
            Contract.Requires(other.IsValid());
            Contract.Ensures(Contract.Result<Vector>().IsValid());
            return new Vector(point.X - other.X, point.Y - other.Y);
        }

        public static Vector Subtract(this Size size, Size other)
        {
            Contract.Requires(size.IsValid());
            Contract.Requires(other.IsValid());
            Contract.Ensures(Contract.Result<Vector>().IsValid());
            return new Vector(size.Width - other.Width, size.Height - other.Height);
        }

        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        public static Point GetCenter(this Rect value)
        {
            Contract.Requires(!value.IsEmpty);
            Contract.Ensures(Contract.Result<Point>().IsValid());
            return new Point(value.X + value.Width / 2, value.Y + value.Height / 2);
        }

        public static Rect Expand(this Rect target, double amount)
        {
            Contract.Requires(amount >= 0);
            Contract.Requires(!target.IsEmpty);
            Contract.Ensures(!Contract.Result<Rect>().IsEmpty);
            var value = new Rect(target.X - amount, target.Y - amount, target.Width + 2 * amount, target.Height + 2 * amount);
            Contract.Assume(!value.IsEmpty);
            return value;
        }

        public static Point TopLeft(this Rect rect)
        {
            Contract.Requires(!rect.IsEmpty);
            Contract.Ensures(Contract.Result<Point>().IsValid());
            return new Point(rect.Left, rect.Top);
        }

        public static Point BottomRight(this Rect rect)
        {
            Contract.Requires(!rect.IsEmpty);
            Contract.Ensures(Contract.Result<Point>().IsValid());
            return new Point(rect.Right, rect.Bottom);
        }

        public static Point BottomLeft(this Rect rect)
        {
            Contract.Requires(!rect.IsEmpty);
            Contract.Ensures(Contract.Result<Point>().IsValid());
            return new Point(rect.Left, rect.Bottom);
        }

        public static Point TopRight(this Rect rect)
        {
            Contract.Requires(!rect.IsEmpty);
            Contract.Ensures(Contract.Result<Point>().IsValid());
            return new Point(rect.Right, rect.Top);
        }

        public static Size Size(this Rect rect)
        {
            Contract.Requires(!rect.IsEmpty);
            return new Size(rect.Width, rect.Height);
        }

        public static Point ToPoint(this Vector vector)
        {
            return (Point)vector;
        }

        public static Vector CenterVector(this Size size)
        {
            return ((Vector)size) * .5;
        }
        public static Vector RightAngle(this Vector vector)
        {
            return new Vector(-vector.Y, vector.X);
        }
        #endregion
    }
} 