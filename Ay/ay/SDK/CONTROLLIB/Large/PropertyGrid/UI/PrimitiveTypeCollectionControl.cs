using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;


namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents an editor of primitive types.</summary>
	public class PrimitiveTypeCollectionControl : ContentControl
	{
		private bool _surpressTextChanged;

		private bool _conversionFailed;

		/// <summary>Identifies the IsOpen dependency property.</summary>
		public static readonly DependencyProperty IsOpenProperty;

		/// <summary>Identifies the ItemsSource dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceProperty;

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty;

		/// <summary>Identifies the ItemsSourceType dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceTypeProperty;

		/// <summary>Identifies the ItemType dependency property.</summary>
		public static readonly DependencyProperty ItemTypeProperty;

		/// <summary>Identifies the Text dependency property.</summary>
		public static readonly DependencyProperty TextProperty;

		/// <summary>Gets or sets a value indicating whether the editor's dropdown is open.</summary>
		public bool IsOpen
		{
			get
			{
				return (bool)GetValue(IsOpenProperty);
			}
			set
			{
				SetValue(IsOpenProperty, value);
			}
		}

		/// <summary>Gets or sets a collection used to generate the content of the control.<span></span></summary>
		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		/// <summary>Gets or sets whether the control is read-only.</summary>
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

		/// <summary>Gets the type of the item.</summary>
		public Type ItemType
		{
			get
			{
				return (Type)GetValue(ItemTypeProperty);
			}
			set
			{
				SetValue(ItemTypeProperty, value);
			}
		}

		/// <summary>Gets or sets the text of the editor.</summary>
		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PrimitiveTypeCollectionControl primitiveTypeCollectionControl = o as PrimitiveTypeCollectionControl;
			if (primitiveTypeCollectionControl != null)
			{
				primitiveTypeCollectionControl.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		/// <summary>Called when IsOpen changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PrimitiveTypeCollectionControl primitiveTypeCollectionControl = o as PrimitiveTypeCollectionControl;
			if (primitiveTypeCollectionControl != null)
			{
				primitiveTypeCollectionControl.OnItemsSourceChanged((IList)e.OldValue, (IList)e.NewValue);
			}
		}

		/// <summary>Called when ItemsSource changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnItemsSourceChanged(IList oldValue, IList newValue)
		{
			if (newValue != null)
			{
				if (ItemsSourceType == null)
				{
					ItemsSourceType = newValue.GetType();
				}
				if (ItemType == null && newValue.GetType().ContainsGenericParameters)
				{
					ItemType = newValue.GetType().GetGenericArguments()[0];
				}
				SetText(newValue);
			}
		}

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PrimitiveTypeCollectionControl primitiveTypeCollectionControl = o as PrimitiveTypeCollectionControl;
			if (primitiveTypeCollectionControl != null)
			{
				primitiveTypeCollectionControl.OnTextChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when Text changes.</summary>
		protected virtual void OnTextChanged(string oldValue, string newValue)
		{
			if (!_surpressTextChanged)
			{
				PersistChanges();
			}
		}

		static PrimitiveTypeCollectionControl()
		{
			IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(false, OnIsOpenChanged));
			ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(null, OnItemsSourceChanged));
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(false));
			ItemsSourceTypeProperty = DependencyProperty.Register("ItemsSourceType", typeof(Type), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(null));
			ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(Type), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(null));
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PrimitiveTypeCollectionControl), new UIPropertyMetadata(null, OnTextChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PrimitiveTypeCollectionControl), new FrameworkPropertyMetadata(typeof(PrimitiveTypeCollectionControl)));
		}

		/// <summary>Initializes a new instance of the PrimitiveTypeCollectionControl class.</summary>
		public PrimitiveTypeCollectionControl()
		{
		
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		private void PersistChanges()
		{
			IList list = ComputeItemsSource();
			if (list != null)
			{
				IList list2 = ComputeItems();
				list.Clear();
				int num = 0;
				foreach (object item in list2)
				{
					if (list is Array)
					{
						((Array)list).SetValue(item, num++);
					}
					else
					{
						list.Add(item);
					}
				}
				if (_conversionFailed)
				{
					SetText(list);
				}
			}
		}

		private IList ComputeItems()
		{
			IList list = new List<object>();
			if (ItemType == null)
			{
				return list;
			}
			string[] array = Text.Split('\n');
			string[] array2 = array;
			foreach (string text in array2)
			{
				string value = text.TrimEnd('\r');
				if (!string.IsNullOrEmpty(value))
				{
					object obj = null;
					try
					{
						obj = ((!ItemType.IsEnum) ? Convert.ChangeType(value, ItemType) : Enum.Parse(ItemType, value));
					}
					catch
					{
						_conversionFailed = true;
					}
					if (obj != null)
					{
						list.Add(obj);
					}
				}
			}
			return list;
		}

		private IList ComputeItemsSource()
		{
			if (ItemsSource == null)
			{
				string text = Text;
				ItemsSource = CreateItemsSource();
				Text = text;
			}
			return ItemsSource;
		}

		private IList CreateItemsSource()
		{
			IList result = null;
			if (ItemsSourceType != null)
			{
				ConstructorInfo constructor = ItemsSourceType.GetConstructor(Type.EmptyTypes);
				result = (IList)constructor.Invoke(null);
			}
			return result;
		}

		private void SetText(IEnumerable collection)
		{
			_surpressTextChanged = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object item in collection)
			{
				stringBuilder.Append(item.ToString());
				stringBuilder.AppendLine();
			}
			Text = stringBuilder.ToString().Trim();
			_surpressTextChanged = false;
		}
	}
}
