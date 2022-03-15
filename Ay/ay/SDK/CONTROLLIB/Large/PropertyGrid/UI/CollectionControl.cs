using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ay.UIAutomation;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Xceed.Wpf.Toolkit
{

    /// <summary>Provides a user interface that can edit types of collections.</summary>
    [TemplatePart(Name = "PART_PropertyGrid", Type = typeof(Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid))]
	[TemplatePart(Name = "PART_NewItemTypesComboBox", Type = typeof(ComboBox))]
	[TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
	public class CollectionControl : Control
	{
		/// <summary>Handles the ItemDeleting routed event.</summary>
		public delegate void ItemDeletingRoutedEventHandler(object sender, ItemDeletingEventArgs e);

		/// <summary>Handles the ItemDeleted routed event.</summary>
		public delegate void ItemDeletedRoutedEventHandler(object sender, ItemEventArgs e);

		/// <summary>Handles the ItemAdding routed event.</summary>
		public delegate void ItemAddingRoutedEventHandler(object sender, ItemAddingEventArgs e);

		/// <summary>Handles the ItemAdded routed event.</summary>
		public delegate void ItemAddedRoutedEventHandler(object sender, ItemEventArgs e);

		/// <summary>Handles the ItemMovedDown routed event.</summary>
		public delegate void ItemMovedDownRoutedEventHandler(object sender, ItemEventArgs e);

		/// <summary>Handles the ItemMovedUp routed event.</summary>
		public delegate void ItemMovedUpRoutedEventHandler(object sender, ItemEventArgs e);

		private const string PART_NewItemTypesComboBox = "PART_NewItemTypesComboBox";

		private const string PART_PropertyGrid = "PART_PropertyGrid";

		private const string PART_ListBox = "PART_ListBox";

		private ComboBox _newItemTypesComboBox;

		private Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid _propertyGrid;

		private ListBox _listBox;

		private bool _isCollectionUpdated;

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty;

		/// <summary>Identifies the Items dependency property.</summary>
		public static readonly DependencyProperty ItemsProperty;

		/// <summary>Identifies the ItemsSource dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceProperty;

		/// <summary>Identifies the ItemsSourceType dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceTypeProperty;

		/// <summary>Identifies the NewItemTypes dependency property.</summary>
		public static readonly DependencyProperty NewItemTypesProperty;

		/// <summary>Identifies the PropertiesLabel dependency property</summary>
		public static readonly DependencyProperty PropertiesLabelProperty;

		/// <summary>Identifies the SelectedItem dependency property.</summary>
		public static readonly DependencyProperty SelectedItemProperty;

		/// <summary>Identifies the TypeSelectionLabel dependency property</summary>
		public static readonly DependencyProperty TypeSelectionLabelProperty;

		/// <summary>Identifies the EditorDefinitions dependency property.</summary>
		public static readonly DependencyProperty EditorDefinitionsProperty;

		/// <summary>Identifies the ItemDeleting routed event.</summary>
		public static readonly RoutedEvent ItemDeletingEvent;

		/// <summary>Identifies the ItemDeleted routed event.</summary>
		public static readonly RoutedEvent ItemDeletedEvent;

		/// <summary>Identifies the ItemAdding routed event.</summary>
		public static readonly RoutedEvent ItemAddingEvent;

		/// <summary>Identifies the ItemAdded routed event.</summary>
		public static readonly RoutedEvent ItemAddedEvent;

		public static readonly RoutedEvent ItemMovedDownEvent;

		public static readonly RoutedEvent ItemMovedUpEvent;

		/// <summary>Gets or sets whether the CollectionControl is read-only.</summary>
		public bool IsReadOnly
		{
			get
			{
				return (bool)GetValue(IsReadOnlyProperty);
			}
			set
			{
				SetValue(IsReadOnlyProperty, value);
			}
		}

		/// <summary>Gets or sets the collection used to generate the content of the CollectionControl.</summary>
		public ObservableCollection<object> Items
		{
			get
			{
				return (ObservableCollection<object>)GetValue(ItemsProperty);
			}
			set
			{
				SetValue(ItemsProperty, value);
			}
		}

		/// <summary>Gets or sets a list used to generate the content of the CollectionControl.</summary>
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		/// <summary>Gets or sets the type of ItemsSource.</summary>
		public Type ItemsSourceType
		{
			get
			{
				return (Type)GetValue(ItemsSourceTypeProperty);
			}
			set
			{
				SetValue(ItemsSourceTypeProperty, value);
			}
		}

		/// <summary>Gets or sets a list of custom item types that appear in the Add ComboBox.</summary>
		public IList<Type> NewItemTypes
		{
			get
			{
				return (IList<Type>)GetValue(NewItemTypesProperty);
			}
			set
			{
				SetValue(NewItemTypesProperty, value);
			}
		}

		/// <summary>
		///   <span id="BugEvents">Gets or sets the content of properties label (the label on top of the PropertyGrid).</span>
		/// </summary>
		public object PropertiesLabel
		{
			get
			{
				return GetValue(PropertiesLabelProperty);
			}
			set
			{
				SetValue(PropertiesLabelProperty, value);
			}
		}

		/// <summary>Gets or sets the currently selected item.</summary>
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

		/// <summary>
		///   <span id="BugEvents">Gets or sets the content of types label (the label on top of the ComboBox).</span>
		/// </summary>
		public object TypeSelectionLabel
		{
			get
			{
				return GetValue(TypeSelectionLabelProperty);
			}
			set
			{
				SetValue(TypeSelectionLabelProperty, value);
			}
		}

		/// <summary>Gets or sets the custom editors for the PropertyGrid located in the CollectionControl.</summary>
		public EditorDefinitionCollection EditorDefinitions
		{
			get
			{
				return (EditorDefinitionCollection)GetValue(EditorDefinitionsProperty);
			}
			set
			{
				SetValue(EditorDefinitionsProperty, value);
			}
		}

		/// <summary>Gets the PropertyGrid associated with the CollectionControl.</summary>
		public Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid PropertyGrid
		{
			get
			{
				if (_propertyGrid == null)
				{
					ApplyTemplate();
				}
				return _propertyGrid;
			}
		}

		/// <summary>Raised when an item is about to be deleted from the CollectionControl.</summary>
		public event ItemDeletingRoutedEventHandler ItemDeleting
		{
			add
			{
				AddHandler(ItemDeletingEvent, value);
			}
			remove
			{
				RemoveHandler(ItemDeletingEvent, value);
			}
		}

		/// <summary>Raised when an item is deleted from the CollectionControl.</summary>
		public event ItemDeletedRoutedEventHandler ItemDeleted
		{
			add
			{
				AddHandler(ItemDeletedEvent, value);
			}
			remove
			{
				RemoveHandler(ItemDeletedEvent, value);
			}
		}

		/// <summary>Raised when an item is about to be added to the CollectionControl.</summary>
		public event ItemAddingRoutedEventHandler ItemAdding
		{
			add
			{
				AddHandler(ItemAddingEvent, value);
			}
			remove
			{
				RemoveHandler(ItemAddingEvent, value);
			}
		}

		/// <summary>Raised when an item is added to the CollectionControl.</summary>
		public event ItemAddedRoutedEventHandler ItemAdded
		{
			add
			{
				AddHandler(ItemAddedEvent, value);
			}
			remove
			{
				RemoveHandler(ItemAddedEvent, value);
			}
		}

		public event ItemMovedDownRoutedEventHandler ItemMovedDown
		{
			add
			{
				AddHandler(ItemMovedDownEvent, value);
			}
			remove
			{
				RemoveHandler(ItemMovedDownEvent, value);
			}
		}

		public event ItemMovedUpRoutedEventHandler ItemMovedUp
		{
			add
			{
				AddHandler(ItemMovedUpEvent, value);
			}
			remove
			{
				RemoveHandler(ItemMovedUpEvent, value);
			}
		}

		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionControl collectionControl = (CollectionControl)d;
			if (collectionControl != null)
			{
				collectionControl.OnItemSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
			}
		}

		/// <summary>Called when ItemsSource changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public void OnItemSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			if (newValue != null)
			{
				IDictionary dictionary = newValue as IDictionary;
				if (dictionary != null)
				{
					IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
							Type keyType = (dictionaryEntry.Key != null) ? dictionaryEntry.Key.GetType() : ((dictionary.GetType().GetGenericArguments().Count() > 0) ? dictionary.GetType().GetGenericArguments()[0] : typeof(object));
							Type valueType = (dictionaryEntry.Value != null) ? dictionaryEntry.Value.GetType() : ((dictionary.GetType().GetGenericArguments().Count() > 1) ? dictionary.GetType().GetGenericArguments()[1] : typeof(object));
							object item = ListUtilities.CreateEditableKeyValuePair(dictionaryEntry.Key, keyType, dictionaryEntry.Value, valueType);
							Items.Add(item);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				else
				{
					foreach (object item2 in newValue)
					{
						if (item2 != null)
						{
							Items.Add(item2);
						}
					}
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_newItemTypesComboBox != null)
			{
				_newItemTypesComboBox.Loaded -= NewItemTypesComboBox_Loaded;
			}
			_newItemTypesComboBox = (GetTemplateChild("PART_NewItemTypesComboBox") as ComboBox);
			if (_newItemTypesComboBox != null)
			{
				_newItemTypesComboBox.Loaded += NewItemTypesComboBox_Loaded;
			}
			_listBox = (GetTemplateChild("PART_ListBox") as ListBox);
			if (_propertyGrid != null)
			{
				_propertyGrid.PropertyValueChanged -= PropertyGrid_PropertyValueChanged;
			}
			_propertyGrid = (GetTemplateChild("PART_PropertyGrid") as Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid);
			if (_propertyGrid != null)
			{
				_propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
			}
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GenericAutomationPeer(this);
		}

		static CollectionControl()
		{
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CollectionControl), new UIPropertyMetadata(false));
			ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(CollectionControl), new UIPropertyMetadata(null));
			ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CollectionControl), new UIPropertyMetadata(null, OnItemsSourceChanged));
			ItemsSourceTypeProperty = DependencyProperty.Register("ItemsSourceType", typeof(Type), typeof(CollectionControl), new UIPropertyMetadata(null));
			NewItemTypesProperty = DependencyProperty.Register("NewItemTypes", typeof(IList), typeof(CollectionControl), new UIPropertyMetadata(null));
			PropertiesLabelProperty = DependencyProperty.Register("PropertiesLabel", typeof(object), typeof(CollectionControl), new UIPropertyMetadata("Properties:"));
			SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(CollectionControl), new UIPropertyMetadata(null));
			TypeSelectionLabelProperty = DependencyProperty.Register("TypeSelectionLabel", typeof(object), typeof(CollectionControl), new UIPropertyMetadata("Select type:"));
			EditorDefinitionsProperty = DependencyProperty.Register("EditorDefinitions", typeof(EditorDefinitionCollection), typeof(CollectionControl), new UIPropertyMetadata(null));
			ItemDeletingEvent = EventManager.RegisterRoutedEvent("ItemDeleting", RoutingStrategy.Bubble, typeof(ItemDeletingRoutedEventHandler), typeof(CollectionControl));
			ItemDeletedEvent = EventManager.RegisterRoutedEvent("ItemDeleted", RoutingStrategy.Bubble, typeof(ItemDeletedRoutedEventHandler), typeof(CollectionControl));
			ItemAddingEvent = EventManager.RegisterRoutedEvent("ItemAdding", RoutingStrategy.Bubble, typeof(ItemAddingRoutedEventHandler), typeof(CollectionControl));
			ItemAddedEvent = EventManager.RegisterRoutedEvent("ItemAdded", RoutingStrategy.Bubble, typeof(ItemAddedRoutedEventHandler), typeof(CollectionControl));
			ItemMovedDownEvent = EventManager.RegisterRoutedEvent("ItemMovedDown", RoutingStrategy.Bubble, typeof(ItemMovedDownRoutedEventHandler), typeof(CollectionControl));
			ItemMovedUpEvent = EventManager.RegisterRoutedEvent("ItemMovedUp", RoutingStrategy.Bubble, typeof(ItemMovedUpRoutedEventHandler), typeof(CollectionControl));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionControl), new FrameworkPropertyMetadata(typeof(CollectionControl)));
		}

		/// <summary>Initializes a new instance of the CollectionControl class.</summary>
		public CollectionControl()
		{

			Items = new ObservableCollection<object>();
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, AddNew, CanAddNew));
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete, CanDelete));
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Duplicate, CanDuplicate));
			base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, MoveDown, CanMoveDown));
			base.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, MoveUp, CanMoveUp));
		}

		private void NewItemTypesComboBox_Loaded(object sender, RoutedEventArgs e)
		{
			if (_newItemTypesComboBox != null)
			{
				_newItemTypesComboBox.SelectedIndex = 0;
			}
		}

		private void PropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			if (_listBox != null)
			{
				_isCollectionUpdated = true;
				_listBox.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate
				{
					_listBox.Items.Refresh();
				});
			}
		}

		private void AddNew(object sender, ExecutedRoutedEventArgs e)
		{
			object newItem = CreateNewItem((Type)e.Parameter);
			AddNewCore(newItem);
		}

		private void CanAddNew(object sender, CanExecuteRoutedEventArgs e)
		{
			Type t = e.Parameter as Type;
			CanAddNewCore(t, e);
		}

		private void CanAddNewCore(Type t, CanExecuteRoutedEventArgs e)
		{
			if (t != null && !IsReadOnly && ((t.IsValueType && !t.IsEnum && !t.IsPrimitive) || t.GetConstructor(Type.EmptyTypes) != null))
			{
				e.CanExecute = true;
			}
		}

		private void AddNewCore(object newItem)
		{
			if (newItem == null)
			{
				throw new ArgumentNullException("newItem");
			}
			ItemAddingEventArgs itemAddingEventArgs = new ItemAddingEventArgs(ItemAddingEvent, newItem);
			RaiseEvent(itemAddingEventArgs);
			if (!itemAddingEventArgs.Cancel)
			{
				newItem = itemAddingEventArgs.Item;
				Items.Add(newItem);
				RaiseEvent(new ItemEventArgs(ItemAddedEvent, newItem));
				_isCollectionUpdated = true;
				SelectedItem = newItem;
			}
		}

		private void Delete(object sender, ExecutedRoutedEventArgs e)
		{
			ItemDeletingEventArgs itemDeletingEventArgs = new ItemDeletingEventArgs(ItemDeletingEvent, e.Parameter);
			RaiseEvent(itemDeletingEventArgs);
			if (!itemDeletingEventArgs.Cancel)
			{
				Items.Remove(e.Parameter);
				RaiseEvent(new ItemEventArgs(ItemDeletedEvent, e.Parameter));
				_isCollectionUpdated = true;
			}
		}

		private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (e.Parameter != null && !IsReadOnly);
		}

		private void Duplicate(object sender, ExecutedRoutedEventArgs e)
		{
			object newItem = DuplicateItem(e);
			AddNewCore(newItem);
		}

		private void CanDuplicate(object sender, CanExecuteRoutedEventArgs e)
		{
			Type t = (e.Parameter != null) ? e.Parameter.GetType() : null;
			CanAddNewCore(t, e);
		}

		private object DuplicateItem(ExecutedRoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			object parameter = e.Parameter;
			Type type = parameter.GetType();
			object obj = CreateNewItem(type);
			Type type2 = type;
			while (type2 != null)
			{
				FieldInfo[] fields = type2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo[] array = fields;
				foreach (FieldInfo fieldInfo in array)
				{
					fieldInfo.SetValue(obj, fieldInfo.GetValue(parameter));
				}
				type2 = type2.BaseType;
			}
			return obj;
		}

		private void MoveDown(object sender, ExecutedRoutedEventArgs e)
		{
			object parameter = e.Parameter;
			int num = Items.IndexOf(parameter);
			Items.RemoveAt(num);
			Items.Insert(++num, parameter);
			RaiseEvent(new ItemEventArgs(ItemMovedDownEvent, parameter));
			_isCollectionUpdated = true;
			SelectedItem = parameter;
		}

		private void CanMoveDown(object sender, CanExecuteRoutedEventArgs e)
		{
			if (e.Parameter != null && Items.IndexOf(e.Parameter) < Items.Count - 1 && !IsReadOnly)
			{
				e.CanExecute = true;
			}
		}

		private void MoveUp(object sender, ExecutedRoutedEventArgs e)
		{
			object parameter = e.Parameter;
			int num = Items.IndexOf(parameter);
			Items.RemoveAt(num);
			Items.Insert(--num, parameter);
			RaiseEvent(new ItemEventArgs(ItemMovedUpEvent, parameter));
			_isCollectionUpdated = true;
			SelectedItem = parameter;
		}

		private void CanMoveUp(object sender, CanExecuteRoutedEventArgs e)
		{
			if (e.Parameter != null && Items.IndexOf(e.Parameter) > 0 && !IsReadOnly)
			{
				e.CanExecute = true;
			}
		}

		/// <summary>Persists changes made in the collection editor.</summary>
		public bool PersistChanges()
		{
			PersistChanges(Items);
			return _isCollectionUpdated;
		}

		internal void PersistChanges(IList sourceList)
		{
			IEnumerable enumerable = ComputeItemsSource();
			if (enumerable != null)
			{
				if (enumerable is IDictionary)
				{
					IDictionary dictionary = (IDictionary)enumerable;
					dictionary.Clear();
					foreach (object source in sourceList)
					{
						PropertyInfo property = source.GetType().GetProperty("Key");
						PropertyInfo property2 = source.GetType().GetProperty("Value");
						if (property != null && property2 != null)
						{
							dictionary.Add(property.GetValue(source, null), property2.GetValue(source, null));
						}
					}
				}
				else if (enumerable is IList)
				{
					IList list = (IList)enumerable;
					list.Clear();
					if (list.IsFixedSize)
					{
						if (sourceList.Count > list.Count)
						{
							throw new IndexOutOfRangeException("Exceeding array size.");
						}
						for (int i = 0; i < sourceList.Count; i++)
						{
							list[i] = sourceList[i];
						}
					}
					else
					{
						foreach (object source2 in sourceList)
						{
							list.Add(source2);
						}
					}
				}
				else
				{
					Type type = enumerable.GetType();
					Type type2 = type.GetInterfaces().FirstOrDefault(delegate(Type x)
					{
						if (x.IsGenericType)
						{
							return x.GetGenericTypeDefinition() == typeof(ICollection<>);
						}
						return false;
					});
					if (type2 != null)
					{
						Type type3 = type2.GetGenericArguments().FirstOrDefault();
						if (type3 != null)
						{
							Type type4 = typeof(ICollection<>).MakeGenericType(type3);
							type4.GetMethod("Clear").Invoke(enumerable, null);
							foreach (object source3 in sourceList)
							{
								type4.GetMethod("Add").Invoke(enumerable, new object[1]
								{
									source3
								});
							}
						}
					}
				}
			}
		}

		private IEnumerable CreateItemsSource()
		{
			IEnumerable result = null;
			if (ItemsSourceType != null)
			{
				ConstructorInfo constructor = ItemsSourceType.GetConstructor(Type.EmptyTypes);
				if (constructor != null)
				{
					result = (IEnumerable)constructor.Invoke(null);
				}
				else if (ItemsSourceType.IsArray)
				{
					result = Array.CreateInstance(ItemsSourceType.GetElementType(), Items.Count);
				}
			}
			return result;
		}

		private object CreateNewItem(Type type)
		{
			return Activator.CreateInstance(type);
		}

		private IEnumerable ComputeItemsSource()
		{
			if (ItemsSource == null)
			{
				ItemsSource = CreateItemsSource();
			}
			return ItemsSource;
		}
	}
}
