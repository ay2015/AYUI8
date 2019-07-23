using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using PixelLab.Contracts;
using PixelLab.Common;

namespace Ay.Framework.WPF
{
    /// <summary>
    /// 排序 ObservableCollection的顺序的
    /// 2016-8-4 11:17:54
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    public class SortedObservableEnumerable<TItem, TSource> : Changeable, IEnumerable<TItem>, INotifyCollectionChanged where TSource : class, IEnumerable<TItem>, INotifyCollectionChanged
    {
        private readonly TSource _source;
        private IComparer<TItem> _comparer;

        public SortedObservableEnumerable(TSource source, IComparer<TItem> comparer = null)
        {
            Contract.Requires(source != null);
            _source = source;
            _comparer = comparer;
            _source.CollectionChanged += (sender, args) =>
            {
                OnCollectionChanged();
            };
        }

        public IComparer<TItem> Comparer
        {
            get { return _comparer; }
            set
            {
                if (UpdateProperty("Comparer", ref _comparer, value))
                {
                    OnCollectionChanged();
                }
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            if (_comparer == null)
            {
                return _source.GetEnumerator();
            }
            else
            {
                return _source.OrderBy(a => a, _comparer).GetEnumerator();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args = null)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, args ?? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
