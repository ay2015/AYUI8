using ay.Controls.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ay.SDK.CONTROLLIB.Primitive
{
	/// <summary>Represents a selector class from which others are derived (CheckComboBox and CheckListBox).</summary>
	public class Selector : ItemsControl, IWeakEventListener
	{
		private class ValueEqualityComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
			}

			public int GetHashCode(string obj)
			{
				return 1;
			}
		}

		private bool _surpressItemSelectionChanged;

		private bool _ignoreSelectedItemChanged;

		private bool _ignoreSelectedValueChanged;

		private int _ignoreSelectedItemsCollectionChanged;

		private int _ignoreSelectedMemberPathValuesChanged;

		private IList _selectedItems;

		private IList _removedItems = new ObservableCollection<object>();

		private object[] _internalSelectedItems;

		private ValueChangeHelper _selectedMemberPathValuesHelper;

		private ValueChangeHelper _valueMemberPathValuesHelper;

		/// <summary>Identifies the Command dependency property.</summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(Selector), new PropertyMetadata((object)null));

		/// <summary>Identifies the Delimiter dependency property.</summary>
		public static readonly DependencyProperty DelimiterProperty = DependencyProperty.Register("Delimiter", typeof(string), typeof(Selector), new UIPropertyMetadata(",", OnDelimiterChanged));

		/// <summary>Identifies the SelectedItem dependency property.</summary>
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Selector), new UIPropertyMetadata(null, OnSelectedItemChanged));

		/// <summary>Identifies the SelectedItemsOverride dependency property.</summary>
		public static readonly DependencyProperty SelectedItemsOverrideProperty = DependencyProperty.Register("SelectedItemsOverride", typeof(IList), typeof(Selector), new UIPropertyMetadata(null, SelectedItemsOverrideChanged));

		/// <summary>Identifies the SelectedMemberPath dependency property.</summary>
		public static readonly DependencyProperty SelectedMemberPathProperty = DependencyProperty.Register("SelectedMemberPath", typeof(string), typeof(Selector), new UIPropertyMetadata(null, OnSelectedMemberPathChanged));

		/// <summary>Identifies the SelectedValue dependency property.</summary>
		public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(string), typeof(Selector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));

		/// <summary>Identifies the ValueMemberPath dependency property.</summary>
		public static readonly DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(Selector), new UIPropertyMetadata(OnValueMemberPathChanged));

		/// <summary>Identifies the Selected routed event.</summary>
		public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("SelectedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));

		/// <summary>Identifies the Unselected routed event.</summary>
		public static readonly RoutedEvent UnSelectedEvent = EventManager.RegisterRoutedEvent("UnSelectedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));

		/// <summary>Identifies the ItemSelectionChanged routed event.</summary>
		public static readonly RoutedEvent ItemSelectionChangedEvent = EventManager.RegisterRoutedEvent("ItemSelectionChanged", RoutingStrategy.Bubble, typeof(ItemSelectionChangedEventHandler), typeof(Selector));

		/// <summary>Gets or sets the command to execute when an item is checked/unchecked.</summary>
		[TypeConverter(typeof(CommandConverter))]
		public ICommand Command
		{
			get
			{
				return (ICommand)GetValue(CommandProperty);
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}

		/// <summary>Gets or sets the separator used by SelectedValue between the selected
		/// items.</summary>
		public string Delimiter
		{
			get
			{
				return (string)GetValue(DelimiterProperty);
			}
			set
			{
				SetValue(DelimiterProperty, value);
			}
		}

		/// <summary>Gets or sets the last checked or unchecked item.</summary>
		public object SelectedItem
		{
			get
			{
				return GetValue(SelectedItemProperty);
			}
			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		/// <summary>Gets the collection of checked items.</summary>
		public IList SelectedItems
		{
			get
			{
				return _selectedItems;
			}
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				INotifyCollectionChanged notifyCollectionChanged = _selectedItems as INotifyCollectionChanged;
				INotifyCollectionChanged notifyCollectionChanged2 = value as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					CollectionChangedEventManager.RemoveListener(notifyCollectionChanged, this);
				}
				if (notifyCollectionChanged2 != null)
				{
					CollectionChangedEventManager.AddListener(notifyCollectionChanged2, this);
				}
				IList selectedItems = _selectedItems;
				if (selectedItems != null)
				{
					foreach (object item in selectedItems)
					{
						if ((value != null && !value.Contains(item)) || value == null)
						{
							OnItemSelectionChanged(new ItemSelectionChangedEventArgs(ItemSelectionChangedEvent, this, item, false));
							if (Command != null)
							{
								Command.Execute(item);
							}
						}
					}
				}
				if (value != null)
				{
					foreach (object item2 in value)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(ItemSelectionChangedEvent, this, item2, true));
						if (((selectedItems != null && !selectedItems.Contains(item2)) || selectedItems == null) && Command != null)
						{
							Command.Execute(item2);
						}
					}
				}
				_selectedItems = value;
			}
		}

		/// <summary>
		///   <para>Gets or sets a <span id="BugEvents">custom IList of selected items.</span></para>
		/// </summary>
		public IList SelectedItemsOverride
		{
			get
			{
				return (IList)GetValue(SelectedItemsOverrideProperty);
			}
			set
			{
				SetValue(SelectedItemsOverrideProperty, value);
			}
		}

		/// <summary>Gets or sets a path to a value on the source object used to determine whether an item is selected.</summary>
		public string SelectedMemberPath
		{
			get
			{
				return (string)GetValue(SelectedMemberPathProperty);
			}
			set
			{
				SetValue(SelectedMemberPathProperty, value);
			}
		}

		/// <summary>Gets or sets a string containing the selected items separated by the value of Delimiter (ex., "Item1, Item2, Item3").</summary>
		public string SelectedValue
		{
			get
			{
				return (string)GetValue(SelectedValueProperty);
			}
			set
			{
				SetValue(SelectedValueProperty, value);
			}
		}

		/// <summary>Gets or sets a path to a value on the source object representing the value to use.</summary>
		public string ValueMemberPath
		{
			get
			{
				return (string)GetValue(ValueMemberPathProperty);
			}
			set
			{
				SetValue(ValueMemberPathProperty, value);
			}
		}

		protected IEnumerable ItemsCollection
		{
			get
			{
				return (IEnumerable)(base.ItemsSource ?? ((object)base.Items) ?? ((object)new object[0]));
			}
		}

		/// <summary>Raised when the item selection changes.</summary>
		public event ItemSelectionChangedEventHandler ItemSelectionChanged
		{
			add
			{
				AddHandler(ItemSelectionChangedEvent, value);
			}
			remove
			{
				RemoveHandler(ItemSelectionChangedEvent, value);
			}
		}

		/// <summary>Initializes a new instance of the Selector class.</summary>
		public Selector()
		{

			SelectedItems = new ObservableCollection<object>();
			RoutedEvent selectedEvent = SelectedEvent;
			RoutedEventHandler handler = delegate(object s, RoutedEventArgs args)
			{
				OnItemSelectionChangedCore(args, false);
			};
			AddHandler(selectedEvent, handler);
			AddHandler(UnSelectedEvent, (RoutedEventHandler)delegate(object s, RoutedEventArgs args)
			{
				OnItemSelectionChangedCore(args, true);
			});
			_selectedMemberPathValuesHelper = new ValueChangeHelper(OnSelectedMemberPathValuesChanged);
			_valueMemberPathValuesHelper = new ValueChangeHelper(OnValueMemberPathValuesChanged);
		}

		private static void OnDelimiterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((Selector)o).OnSelectedItemChanged((string)e.OldValue, (string)e.NewValue);
		}

		protected virtual void OnSelectedItemChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized)
			{
				UpdateSelectedValue();
			}
		}

		private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((Selector)sender).OnSelectedItemChanged(args.OldValue, args.NewValue);
		}

		protected virtual void OnSelectedItemChanged(object oldValue, object newValue)
		{
			if (base.IsInitialized && !_ignoreSelectedItemChanged)
			{
				_ignoreSelectedItemsCollectionChanged++;
				SelectedItems.Clear();
				if (newValue != null)
				{
					SelectedItems.Add(newValue);
				}
				UpdateFromSelectedItems();
				_ignoreSelectedItemsCollectionChanged--;
			}
		}

		private static void SelectedItemsOverrideChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((Selector)sender).OnSelectedItemsOverrideChanged((IList)args.OldValue, (IList)args.NewValue);
		}

		protected virtual void OnSelectedItemsOverrideChanged(IList oldValue, IList newValue)
		{
			if (base.IsInitialized)
			{
				SelectedItems = ((newValue != null) ? newValue : new ObservableCollection<object>());
				UpdateFromSelectedItems();
			}
		}

		private static void OnSelectedMemberPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)o;
			selector.OnSelectedMemberPathChanged((string)e.OldValue, (string)e.NewValue);
		}

		protected virtual void OnSelectedMemberPathChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized)
			{
				UpdateSelectedMemberPathValuesBindings();
			}
		}

		private static void OnSelectedValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = o as Selector;
			if (selector != null)
			{
				selector.OnSelectedValueChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when SelectedValue changes.</summary>
		/// <param name="oldValue">The old string value of SelectedValue.</param>
		/// <param name="newValue">The new string value of SelectedValue.</param>
		protected virtual void OnSelectedValueChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized && !_ignoreSelectedValueChanged)
			{
				UpdateFromSelectedValue();
			}
		}

		private static void OnValueMemberPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)o;
			selector.OnValueMemberPathChanged((string)e.OldValue, (string)e.NewValue);
		}

		protected virtual void OnValueMemberPathChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized)
			{
				UpdateValueMemberPathValuesBindings();
			}
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <returns>
		///   <strong>true</strong> if the item is (or is eligible to be) its own container; false, otherwise.</returns>
		/// <param name="item">
		///   <span>
		///     <span>The item to check.</span>
		///   </span>
		/// </param>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is SelectorItem;
		}

		/// <summary>Creates or identifies the element that is used to display the given item.</summary>
		/// <returns>
		///   <span>
		///     <span>The element that is used to display the given item.</span>
		///   </span>
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new SelectorItem();
		}

		/// <summary>Prepares the specified element to display the specified item.</summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">
		///   <span>
		///     <span>Specified item.</span>
		///   </span>
		/// </param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			_surpressItemSelectionChanged = true;
			FrameworkElement frameworkElement = element as FrameworkElement;
			frameworkElement.SetValue(SelectorItem.IsSelectedProperty, SelectedItems.Contains(item));
			_surpressItemSelectionChanged = false;
		}

		/// <summary>Called when the ItemsSource property changes.</summary>
		/// <param name="oldValue">Old value of the ItemsSource property.</param>
		/// <param name="newValue">New value of the ItemsSource property.</param>
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			INotifyCollectionChanged notifyCollectionChanged = oldValue as INotifyCollectionChanged;
			INotifyCollectionChanged notifyCollectionChanged2 = newValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				CollectionChangedEventManager.RemoveListener(notifyCollectionChanged, this);
			}
			if (notifyCollectionChanged2 != null)
			{
				CollectionChangedEventManager.AddListener(notifyCollectionChanged2, this);
			}
			if (base.IsInitialized)
			{
				if (!VirtualizingStackPanel.GetIsVirtualizing((DependencyObject)this) || (VirtualizingStackPanel.GetIsVirtualizing((DependencyObject)this) && newValue != null))
				{
					RemoveUnavailableSelectedItems();
				}
				UpdateSelectedMemberPathValuesBindings();
				UpdateValueMemberPathValuesBindings();
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			RemoveUnavailableSelectedItems();
		}

		public override void EndInit()
		{
			base.EndInit();
			if (SelectedItemsOverride != null)
			{
				OnSelectedItemsOverrideChanged(null, SelectedItemsOverride);
			}
			else if (SelectedMemberPath != null)
			{
				OnSelectedMemberPathChanged(null, SelectedMemberPath);
			}
			else if (SelectedValue != null)
			{
				OnSelectedValueChanged(null, SelectedValue);
			}
			else if (SelectedItem != null)
			{
				OnSelectedItemChanged(null, SelectedItem);
			}
			if (ValueMemberPath != null)
			{
				OnValueMemberPathChanged(null, ValueMemberPath);
			}
		}

		protected object GetPathValue(object item, string propertyPath)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (string.IsNullOrEmpty(propertyPath) || propertyPath == ".")
			{
				return item;
			}
			PropertyInfo property = item.GetType().GetProperty(propertyPath);
			if (!(property != null))
			{
				return null;
			}
			return property.GetValue(item, null);
		}

		/// <summary>Retrieves the value of the passed item.</summary>
		/// <returns>The value of <em>item</em>.</returns>
		/// <param name="item">The item whose value will be retrieved.</param>
		protected object GetItemValue(object item)
		{
			if (item == null)
			{
				return null;
			}
			return GetPathValue(item, ValueMemberPath);
		}

		/// <summary>Retrieves the item in ItemsSource corresponding to the sub-string representing that item in SelectedValue.</summary>
		/// <returns>The item in ItemsSource corresponding to the sub-string representing that item in SelectedValue.</returns>
		/// <param name="value">The sub-string representation of an item in SelectedValue.</param>
		protected object ResolveItemByValue(string value)
		{
			if (!string.IsNullOrEmpty(ValueMemberPath))
			{
				foreach (object item in ItemsCollection)
				{
					PropertyInfo property = item.GetType().GetProperty(ValueMemberPath);
					if (property != null)
					{
						object value2 = property.GetValue(item, null);
						if (value.Equals(value2.ToString(), StringComparison.InvariantCultureIgnoreCase))
						{
							return item;
						}
					}
				}
				return value;
			}
			return value;
		}

		internal void UpdateFromList(List<string> selectedValues, Func<object, object> GetItemfunction)
		{
			_ignoreSelectedItemsCollectionChanged++;
			SelectedItems.Clear();
			if (selectedValues != null && selectedValues.Count > 0)
			{
				ValueEqualityComparer comparer = new ValueEqualityComparer();
				foreach (object item in ItemsCollection)
				{
					object obj = GetItemfunction(item);
					if (obj != null && selectedValues.Contains(obj.ToString(), comparer))
					{
						SelectedItems.Add(item);
					}
				}
			}
			_ignoreSelectedItemsCollectionChanged--;
			UpdateFromSelectedItems();
		}

		private bool? GetSelectedMemberPathValue(object item)
		{
			if (string.IsNullOrEmpty(SelectedMemberPath))
			{
				return null;
			}
			if (item == null)
			{
				return null;
			}
			string[] array = SelectedMemberPath.Split('.');
			if (array.Length == 1)
			{
				PropertyInfo property = item.GetType().GetProperty(SelectedMemberPath);
				if (property != null && property.PropertyType == typeof(bool))
				{
					return property.GetValue(item, null) as bool?;
				}
				return null;
			}
			for (int i = 0; i < array.Count(); i++)
			{
				Type type = item.GetType();
				PropertyInfo property2 = type.GetProperty(array[i]);
				if (property2 == null)
				{
					return null;
				}
				if (i == array.Count() - 1)
				{
					if (property2.PropertyType == typeof(bool))
					{
						return property2.GetValue(item, null) as bool?;
					}
				}
				else
				{
					item = property2.GetValue(item, null);
				}
			}
			return null;
		}

		private void SetSelectedMemberPathValue(object item, bool value)
		{
			if (!string.IsNullOrEmpty(SelectedMemberPath) && item != null)
			{
				string[] array = SelectedMemberPath.Split('.');
				if (array.Length == 1)
				{
					PropertyInfo property = item.GetType().GetProperty(SelectedMemberPath);
					if (property != null && property.PropertyType == typeof(bool))
					{
						property.SetValue(item, value, null);
					}
				}
				else
				{
					for (int i = 0; i < array.Count(); i++)
					{
						Type type = item.GetType();
						PropertyInfo property2 = type.GetProperty(array[i]);
						if (property2 == null)
						{
							break;
						}
						if (i == array.Count() - 1)
						{
							if (property2.PropertyType == typeof(bool))
							{
								property2.SetValue(item, value, null);
							}
						}
						else
						{
							item = property2.GetValue(item, null);
						}
					}
				}
			}
		}

		protected virtual void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_ignoreSelectedItemsCollectionChanged <= 0)
			{
				if (e.Action == NotifyCollectionChangedAction.Reset && _internalSelectedItems != null)
				{
					object[] internalSelectedItems = _internalSelectedItems;
					foreach (object obj in internalSelectedItems)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(ItemSelectionChangedEvent, this, obj, false));
						if (Command != null)
						{
							Command.Execute(obj);
						}
					}
				}
				if (e.OldItems != null)
				{
					foreach (object oldItem in e.OldItems)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(ItemSelectionChangedEvent, this, oldItem, false));
						if (Command != null)
						{
							Command.Execute(oldItem);
						}
					}
				}
				if (e.NewItems != null)
				{
					foreach (object newItem in e.NewItems)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(ItemSelectionChangedEvent, this, newItem, true));
						if (Command != null)
						{
							Command.Execute(newItem);
						}
					}
				}
				UpdateFromSelectedItems();
			}
		}

		private void OnItemSelectionChangedCore(RoutedEventArgs args, bool unselected)
		{
			object obj = base.ItemContainerGenerator.ItemFromContainer((DependencyObject)args.OriginalSource);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = args.OriginalSource;
			}
			if (unselected)
			{
				while (SelectedItems.Contains(obj))
				{
					SelectedItems.Remove(obj);
				}
			}
			else if (!SelectedItems.Contains(obj))
			{
				SelectedItems.Add(obj);
			}
		}

		private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			RemoveUnavailableSelectedItems();
			AddAvailableRemovedItems();
			UpdateSelectedMemberPathValuesBindings();
			UpdateValueMemberPathValuesBindings();
		}

		private void OnSelectedMemberPathValuesChanged()
		{
			if (_ignoreSelectedMemberPathValuesChanged <= 0)
			{
				UpdateFromSelectedMemberPathValues();
			}
		}

		private void OnValueMemberPathValuesChanged()
		{
			UpdateSelectedValue();
		}

		private void UpdateSelectedMemberPathValuesBindings()
		{
			_selectedMemberPathValuesHelper.UpdateValueSource(ItemsCollection, SelectedMemberPath);
			UpdateFromSelectedMemberPathValues();
		}

		private void UpdateValueMemberPathValuesBindings()
		{
			_valueMemberPathValuesHelper.UpdateValueSource(ItemsCollection, ValueMemberPath);
		}

		/// <summary>Called when the IsSelected property of a SelectorItem has been
		/// modified.</summary>
		/// <param name="args">An ItemSelectionChangedEventArgs that contains the event data.</param>
		protected virtual void OnItemSelectionChanged(ItemSelectionChangedEventArgs args)
		{
			if (!_surpressItemSelectionChanged)
			{
				RaiseEvent(args);
			}
		}

		private void UpdateSelectedValue()
		{
			string text = string.Join(Delimiter, from object x in SelectedItems
			select GetItemValue(x));
			if (string.IsNullOrEmpty(SelectedValue) || !SelectedValue.Equals(text))
			{
				_ignoreSelectedValueChanged = true;
				SelectedValue = text;
				_ignoreSelectedValueChanged = false;
			}
		}

		private void UpdateSelectedItem()
		{
			if (!SelectedItems.Contains(SelectedItem))
			{
				_ignoreSelectedItemChanged = true;
				SelectedItem = ((SelectedItems.Count > 0) ? SelectedItems[0] : null);
				_ignoreSelectedItemChanged = false;
			}
		}

		private void UpdateFromSelectedMemberPathValues()
		{
			_ignoreSelectedItemsCollectionChanged++;
			foreach (object item in ItemsCollection)
			{
				bool? selectedMemberPathValue = GetSelectedMemberPathValue(item);
				if (selectedMemberPathValue.HasValue)
				{
					if (selectedMemberPathValue.Value)
					{
						if (!SelectedItems.Contains(item))
						{
							SelectedItems.Add(item);
						}
					}
					else if (SelectedItems.Contains(item))
					{
						SelectedItems.Remove(item);
					}
				}
			}
			_ignoreSelectedItemsCollectionChanged--;
			UpdateFromSelectedItems();
		}

		internal void UpdateSelectedItems(IList selectedItems)
		{
			if (selectedItems == null)
			{
				throw new ArgumentNullException("selectedItems");
			}
			if (selectedItems.Count != SelectedItems.Count || !selectedItems.Cast<object>().SequenceEqual(SelectedItems.Cast<object>()))
			{
				_ignoreSelectedItemsCollectionChanged++;
				SelectedItems.Clear();
				foreach (object selectedItem in selectedItems)
				{
					SelectedItems.Add(selectedItem);
				}
				_ignoreSelectedItemsCollectionChanged--;
				UpdateFromSelectedItems();
			}
		}

		private void UpdateFromSelectedItems()
		{
			foreach (object item in ItemsCollection)
			{
				bool value = SelectedItems.Contains(item);
				_ignoreSelectedMemberPathValuesChanged++;
				SetSelectedMemberPathValue(item, value);
				_ignoreSelectedMemberPathValuesChanged--;
				SelectorItem selectorItem = base.ItemContainerGenerator.ContainerFromItem(item) as SelectorItem;
				if (selectorItem != null)
				{
					selectorItem.IsSelected = value;
				}
			}
			UpdateSelectedItem();
			UpdateSelectedValue();
			_internalSelectedItems = new object[SelectedItems.Count];
			SelectedItems.CopyTo(_internalSelectedItems, 0);
		}

		private void RemoveUnavailableSelectedItems()
		{
			_ignoreSelectedItemsCollectionChanged++;
			HashSet<object> hashSet = new HashSet<object>(ItemsCollection.Cast<object>());
			for (int i = 0; i < SelectedItems.Count; i++)
			{
				if (!hashSet.Contains(SelectedItems[i]))
				{
					_removedItems.Add(SelectedItems[i]);
					SelectedItems.RemoveAt(i);
					i--;
				}
			}
			_ignoreSelectedItemsCollectionChanged--;
			UpdateSelectedItem();
			UpdateSelectedValue();
		}

		private void AddAvailableRemovedItems()
		{
			HashSet<object> hashSet = new HashSet<object>(ItemsCollection.Cast<object>());
			for (int i = 0; i < _removedItems.Count; i++)
			{
				if (hashSet.Contains(_removedItems[i]))
				{
					SelectedItems.Add(_removedItems[i]);
					_removedItems.RemoveAt(i);
					i--;
				}
			}
		}

		private void UpdateFromSelectedValue()
		{
			List<string> selectedValues = null;
			if (!string.IsNullOrEmpty(SelectedValue))
			{
				selectedValues = SelectedValue.Split(new string[1]
				{
					Delimiter
				}, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
			UpdateFromList(selectedValues, GetItemValue);
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(CollectionChangedEventManager))
			{
				if (object.ReferenceEquals(_selectedItems, sender))
				{
					OnSelectedItemsCollectionChanged(sender, (NotifyCollectionChangedEventArgs)e);
					return true;
				}
				if (object.ReferenceEquals(ItemsCollection, sender))
				{
					OnItemsSourceCollectionChanged(sender, (NotifyCollectionChangedEventArgs)e);
					return true;
				}
			}
			return false;
		}
	}
}
