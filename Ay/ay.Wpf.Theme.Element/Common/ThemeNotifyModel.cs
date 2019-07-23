using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace ay.Wpf.Theme.Element.Common
{
    public class ThemeNotifyModel : INotifyPropertyChanged
    {
        #region 实现通知
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal bool Set<T>(ref T storage, T value,  string propertyName = null,bool isCheckEquals = true)
        {
            if (isCheckEquals)
                if (object.Equals(storage, value)) { return false; }
            storage = value;
            this.OnPropertyChanged2(propertyName);
            return true;
        }


        private void OnPropertyChanged2(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        internal void OnPropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException("propertyNames");

            foreach (var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
        }

        internal void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExtractPropertyName(propertyExpression);
            this.OnPropertyChanged(propertyName);
        }

        internal static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", "propertyExpression");
            }

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("PropertySupport_StaticExpression_Exception", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }
        #endregion



    }

}

