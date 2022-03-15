using ay.UIAutomation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Provides a button containing a collection editor.</summary>
	public class CollectionControlButton : Button
	{
		/// <summary>Identifies the EditorDefinitions dependency property.</summary>
		public static readonly DependencyProperty EditorDefinitionsProperty;

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty;

		/// <summary>Identifies the ItemsSource dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceProperty;

		/// <summary>Identifies the ItemsSourceType dependency property.</summary>
		public static readonly DependencyProperty ItemsSourceTypeProperty;

		/// <summary>Identifies the NewItemTypes dependency property.</summary>
		public static readonly DependencyProperty NewItemTypesProperty;

		public static readonly RoutedEvent CollectionUpdatedEvent;

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

		/// <summary>Gets or sets whether the CollectionControlButton is read-only.</summary>
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

		/// <summary>Raised when an item is added, removed, moved or modified in the collection.</summary>
		public event RoutedEventHandler CollectionUpdated
		{
			add
			{
				AddHandler(CollectionUpdatedEvent, value);
			}
			remove
			{
				RemoveHandler(CollectionUpdatedEvent, value);
			}
		}

		static CollectionControlButton()
		{
			EditorDefinitionsProperty = DependencyProperty.Register("EditorDefinitions", typeof(EditorDefinitionCollection), typeof(CollectionControlButton), new UIPropertyMetadata(null));
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CollectionControlButton), new UIPropertyMetadata(false));
			ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CollectionControlButton), new UIPropertyMetadata(null));
			ItemsSourceTypeProperty = DependencyProperty.Register("ItemsSourceType", typeof(Type), typeof(CollectionControlButton), new UIPropertyMetadata(null));
			NewItemTypesProperty = DependencyProperty.Register("NewItemTypes", typeof(IList), typeof(CollectionControlButton), new UIPropertyMetadata(null));
			CollectionUpdatedEvent = EventManager.RegisterRoutedEvent("CollectionUpdated", RoutingStrategy.Bubble, typeof(EventHandler), typeof(CollectionControlButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionControlButton), new FrameworkPropertyMetadata(typeof(CollectionControlButton)));
		}

		public CollectionControlButton()
		{
			
			base.Click += CollectionControlButton_Click;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GenericAutomationPeer(this);
		}

		private void CollectionControlButton_Click(object sender, RoutedEventArgs e)
		{
			CollectionControlDialog collectionControlDialog = new CollectionControlDialog();
			Binding binding = new Binding("ItemsSource");
			binding.Source = this;
			binding.Mode = BindingMode.TwoWay;
			Binding binding2 = binding;
			BindingOperations.SetBinding(collectionControlDialog, CollectionControlDialog.ItemsSourceProperty, binding2);
			collectionControlDialog.NewItemTypes = NewItemTypes;
			collectionControlDialog.ItemsSourceType = ItemsSourceType;
			collectionControlDialog.IsReadOnly = IsReadOnly;
			collectionControlDialog.EditorDefinitions = EditorDefinitions;
			bool? flag = collectionControlDialog.ShowDialog();
			if (flag.HasValue && flag.Value)
			{
				RaiseEvent(new RoutedEventArgs(CollectionUpdatedEvent, this));
			}
		}
	}
}
