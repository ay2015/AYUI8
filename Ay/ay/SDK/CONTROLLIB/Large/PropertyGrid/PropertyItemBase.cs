using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of CustomPropertyItem.</summary>
	[TemplatePart(Name = "PART_PropertyItemsControl", Type = typeof(PropertyItemsControl))]
	[TemplatePart(Name = "PART_ValueContainer", Type = typeof(ContentControl))]
	public abstract class PropertyItemBase : Control, IPropertyContainer, INotifyPropertyChanged
	{
		internal const string PART_ValueContainer = "PART_ValueContainer";

		internal const int MaxSubLevelSearch = 10;

		private ContentControl _valueContainer;

		private ContainerHelperBase _containerHelper;

		private IPropertyContainer _parentNode;

		private List<KeyValuePair<string, PropertyItem>> _dependsOnPropertyItemsList = new List<KeyValuePair<string, PropertyItem>>();

		internal bool _isExpandingNonPrimitiveTypes;

		internal bool _isPropertyGridCategorized;

		internal bool _isSortedAlphabetically = true;

		/// <summary>Identifies the AdvancedOptionsIcon dependency
		/// property.</summary>
		public static readonly DependencyProperty AdvancedOptionsIconProperty;

		/// <summary>Identifies the AdvancedOptionsTooltip dependency
		/// property.</summary>
		public static readonly DependencyProperty AdvancedOptionsTooltipProperty;

		/// <summary>Identifies the DefinitionKey dependency property.</summary>
		public static readonly DependencyProperty DefinitionKeyProperty;

		/// <summary>Identifies the Description dependency property.</summary>
		public static readonly DependencyProperty DescriptionProperty;

		/// <summary>Identifies the DisplayName dependency property.</summary>
		public static readonly DependencyProperty DisplayNameProperty;

		/// <summary>Identifies the Editor dependency property.</summary>
		public static readonly DependencyProperty EditorProperty;

		/// <summary>Identifies the HighlightedText dependency property.</summary>
		public static readonly DependencyProperty HighlightedTextProperty;

		/// <summary>Identifies the IsExpanded dependency property.</summary>
		public static readonly DependencyProperty IsExpandedProperty;

		/// <summary>Identifies the IsExpandable dependency property.</summary>
		public static readonly DependencyProperty IsExpandableProperty;

		/// <summary>Identifies the IsSelected dependency property.</summary>
		public static readonly DependencyProperty IsSelectedProperty;

		/// <summary>Identifies the WillRefreshPropertyGrid dependency
		/// property.</summary>
		public static readonly DependencyProperty WillRefreshPropertyGridProperty;

		internal static readonly RoutedEvent ItemSelectionChangedEvent;

		public ImageSource AdvancedOptionsIcon
		{
			get
			{
				return (ImageSource)GetValue(AdvancedOptionsIconProperty);
			}
			set
			{
				SetValue(AdvancedOptionsIconProperty, value);
			}
		}

		public object AdvancedOptionsTooltip
		{
			get
			{
				return GetValue(AdvancedOptionsTooltipProperty);
			}
			set
			{
				SetValue(AdvancedOptionsTooltipProperty, value);
			}
		}

		/// <summary>Get or set the definition key to be used in order to determine the editor used or editor definition for the value.</summary>
		public object DefinitionKey
		{
			get
			{
				return GetValue(DefinitionKeyProperty);
			}
			set
			{
				SetValue(DefinitionKeyProperty, value);
			}
		}

		public string Description
		{
			get
			{
				return (string)GetValue(DescriptionProperty);
			}
			set
			{
				SetValue(DescriptionProperty, value);
			}
		}

		public string DisplayName
		{
			get
			{
				return (string)GetValue(DisplayNameProperty);
			}
			set
			{
				SetValue(DisplayNameProperty, value);
			}
		}

		public FrameworkElement Editor
		{
			get
			{
				return (FrameworkElement)GetValue(EditorProperty);
			}
			set
			{
				SetValue(EditorProperty, value);
			}
		}

		/// <summary>Gets or sets the text part to highlight in the Property name of the PropertyItem.</summary>
		public string HighlightedText
		{
			get
			{
				return (string)GetValue(HighlightedTextProperty);
			}
			set
			{
				SetValue(HighlightedTextProperty, value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return (bool)GetValue(IsExpandedProperty);
			}
			set
			{
				SetValue(IsExpandedProperty, value);
			}
		}

		public bool IsExpandable
		{
			get
			{
				return (bool)GetValue(IsExpandableProperty);
			}
			set
			{
				SetValue(IsExpandableProperty, value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return (bool)GetValue(IsSelectedProperty);
			}
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		/// <summary>Gets the parent property grid element of this property.</summary>
		public FrameworkElement ParentElement
		{
			get
			{
				return ParentNode as FrameworkElement;
			}
		}

		internal IPropertyContainer ParentNode
		{
			get
			{
				return _parentNode;
			}
			set
			{
				_parentNode = value;
			}
		}

		internal ContentControl ValueContainer
		{
			get
			{
				return _valueContainer;
			}
		}

		public int Level
		{
			get;
			internal set;
		}

		public IList Properties
		{
			get
			{
				if (_containerHelper == null)
				{
					_containerHelper = new ObjectContainerHelper(this, null);
				}
				return _containerHelper.Properties;
			}
		}

		public Style PropertyContainerStyle
		{
			get
			{
				if (ParentNode == null)
				{
					return null;
				}
				return ParentNode.PropertyContainerStyle;
			}
		}

		internal ContainerHelperBase ContainerHelper
		{
			get
			{
				return _containerHelper;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_containerHelper = value;
				RaisePropertyChanged(() => Properties);
			}
		}

		public bool WillRefreshPropertyGrid
		{
			get
			{
				return (bool)GetValue(WillRefreshPropertyGridProperty);
			}
			set
			{
				SetValue(WillRefreshPropertyGridProperty, value);
			}
		}

		Binding IPropertyContainer.PropertyNameBinding
		{
			get
			{
				return null;
			}
		}

		Binding IPropertyContainer.PropertyValueBinding
		{
			get
			{
				return null;
			}
		}

		EditorDefinitionBase IPropertyContainer.DefaultEditorDefinition
		{
			get
			{
				return null;
			}
		}

		CategoryDefinitionCollection IPropertyContainer.CategoryDefinitions
		{
			get
			{
				return null;
			}
		}

		GroupDescription IPropertyContainer.CategoryGroupDescription
		{
			get
			{
				return null;
			}
		}

		List<KeyValuePair<string, PropertyItem>> IPropertyContainer.DependsOnPropertyItemsList
		{
			get
			{
				return _dependsOnPropertyItemsList;
			}
		}

		bool IPropertyContainer.IsExpandingNonPrimitiveTypes
		{
			get
			{
				return _isExpandingNonPrimitiveTypes;
			}
		}

		Style IPropertyContainer.PropertyContainerStyle
		{
			get
			{
				return PropertyContainerStyle;
			}
		}

		EditorDefinitionCollection IPropertyContainer.EditorDefinitions
		{
			get
			{
				if (ParentNode == null)
				{
					return null;
				}
				return ParentNode.EditorDefinitions;
			}
		}

		PropertyDefinitionCollection IPropertyContainer.PropertyDefinitions
		{
			get
			{
				return GetPropertItemPropertyDefinitions();
			}
		}

		ContainerHelperBase IPropertyContainer.ContainerHelper
		{
			get
			{
				return ContainerHelper;
			}
		}

		bool IPropertyContainer.IsCategorized
		{
			get
			{
				return _isPropertyGridCategorized;
			}
		}

		bool IPropertyContainer.IsSortedAlphabetically
		{
			get
			{
				return _isSortedAlphabetically;
			}
		}

		bool IPropertyContainer.AutoGenerateProperties
		{
			get
			{
				if (ParentNode != null)
				{
					PropertyDefinitionCollection propertItemPropertyDefinitions = GetPropertItemPropertyDefinitions();
					if (propertItemPropertyDefinitions == null || propertItemPropertyDefinitions.Count == 0)
					{
						return true;
					}
					return ParentNode.AutoGenerateProperties;
				}
				return true;
			}
		}

		bool IPropertyContainer.HideInheritedProperties
		{
			get
			{
				return false;
			}
		}

		FilterInfo IPropertyContainer.FilterInfo
		{
			get
			{
				return default(FilterInfo);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private static void OnDefinitionKeyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItemBase propertyItemBase = o as PropertyItemBase;
			if (propertyItemBase != null)
			{
				propertyItemBase.OnDefinitionKeyChanged(e.OldValue, e.NewValue);
			}
		}

		internal virtual void OnDefinitionKeyChanged(object oldValue, object newValue)
		{
		}

		private static void OnEditorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItemBase propertyItemBase = o as PropertyItemBase;
			if (propertyItemBase != null)
			{
				propertyItemBase.OnEditorChanged((FrameworkElement)e.OldValue, (FrameworkElement)e.NewValue);
			}
		}

		protected virtual void OnEditorChanged(FrameworkElement oldValue, FrameworkElement newValue)
		{
		}

		private static void OnIsExpandedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItemBase propertyItemBase = o as PropertyItemBase;
			if (propertyItemBase != null)
			{
				propertyItemBase.OnIsExpandedChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsExpandedChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnIsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyItemBase propertyItemBase = o as PropertyItemBase;
			if (propertyItemBase != null)
			{
				propertyItemBase.OnIsSelectedChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
		{
			RaiseItemSelectionChangedEvent();
		}

		private void RaiseItemSelectionChangedEvent()
		{
			RaiseEvent(new RoutedEventArgs(ItemSelectionChangedEvent));
		}

		internal void RaisePropertyChanged<TMember>(Expression<Func<TMember>> propertyExpression)
		{
			this.Notify(this.PropertyChanged, propertyExpression);
		}

		internal void RaisePropertyChanged(string name)
		{
			this.Notify(this.PropertyChanged, name);
		}

		static PropertyItemBase()
		{
			AdvancedOptionsIconProperty = DependencyProperty.Register("AdvancedOptionsIcon", typeof(ImageSource), typeof(PropertyItemBase), new UIPropertyMetadata(null));
			AdvancedOptionsTooltipProperty = DependencyProperty.Register("AdvancedOptionsTooltip", typeof(object), typeof(PropertyItemBase), new UIPropertyMetadata(null));
			DefinitionKeyProperty = DependencyProperty.Register("DefinitionKey", typeof(object), typeof(PropertyItemBase), new UIPropertyMetadata(null, OnDefinitionKeyChanged));
			DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PropertyItemBase), new UIPropertyMetadata(null));
			DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PropertyItemBase), new UIPropertyMetadata(null));
			EditorProperty = DependencyProperty.Register("Editor", typeof(FrameworkElement), typeof(PropertyItemBase), new UIPropertyMetadata(null, OnEditorChanged));
			HighlightedTextProperty = DependencyProperty.Register("HighlightedText", typeof(string), typeof(PropertyItemBase), new UIPropertyMetadata(null));
			IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(PropertyItemBase), new UIPropertyMetadata(false, OnIsExpandedChanged));
			IsExpandableProperty = DependencyProperty.Register("IsExpandable", typeof(bool), typeof(PropertyItemBase), new UIPropertyMetadata(false));
			IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PropertyItemBase), new UIPropertyMetadata(false, OnIsSelectedChanged));
			WillRefreshPropertyGridProperty = DependencyProperty.Register("WillRefreshPropertyGrid", typeof(bool), typeof(PropertyItemBase), new UIPropertyMetadata(false));
			ItemSelectionChangedEvent = EventManager.RegisterRoutedEvent("ItemSelectionChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PropertyItemBase));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyItemBase), new FrameworkPropertyMetadata(typeof(PropertyItemBase)));
		}

		internal PropertyItemBase()
		{
			base.GotFocus += PropertyItemBase_GotFocus;
			base.RequestBringIntoView += PropertyItemBase_RequestBringIntoView;
			AddHandler(PropertyItemsControl.PreparePropertyItemEvent, new PropertyItemEventHandler(OnPreparePropertyItemInternal));
			AddHandler(PropertyItemsControl.ClearPropertyItemEvent, new PropertyItemEventHandler(OnClearPropertyItemInternal));
		}

		private void OnPreparePropertyItemInternal(object sender, PropertyItemEventArgs args)
		{
			args.PropertyItem.Level = Level + 1;
			_containerHelper.PrepareChildrenPropertyItem(args.PropertyItem, args.Item);
			args.Handled = true;
		}

		private void OnClearPropertyItemInternal(object sender, PropertyItemEventArgs args)
		{
			_containerHelper.ClearChildrenPropertyItem(args.PropertyItem, args.Item);
			args.PropertyItem.Level = 0;
			args.Handled = true;
		}

		private void PropertyItemBase_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			e.Handled = true;
		}

		protected virtual Type GetPropertyItemType()
		{
			return null;
		}

		protected virtual string GetPropertyItemName()
		{
			return DisplayName;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_containerHelper.ChildrenItemsControl = (GetTemplateChild("PART_PropertyItemsControl") as PropertyItemsControl);
			_valueContainer = (GetTemplateChild("PART_ValueContainer") as ContentControl);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			IsSelected = true;
			if (!base.IsKeyboardFocusWithin)
			{
				Focus();
			}
			e.Handled = true;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.PropertyItemAutomationPeer(this);
		}

		private void PropertyItemBase_GotFocus(object sender, RoutedEventArgs e)
		{
			IsSelected = true;
			e.Handled = true;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (ReflectionHelper.IsPublicInstanceProperty(GetType(), e.Property.Name) && base.IsLoaded && _parentNode != null && !_parentNode.ContainerHelper.IsCleaning)
			{
				RaisePropertyChanged(e.Property.Name);
			}
		}

		private PropertyDefinitionCollection GetPropertItemPropertyDefinitions()
		{
			if (ParentNode != null && ParentNode.PropertyDefinitions != null)
			{
				string propertyItemName = GetPropertyItemName();
				foreach (PropertyDefinition propertyDefinition in ParentNode.PropertyDefinitions)
				{
					if (propertyDefinition.TargetProperties.Contains(propertyItemName))
					{
						return propertyDefinition.PropertyDefinitions;
					}
					Type propertyItemType = GetPropertyItemType();
					if (propertyItemType != null)
					{
						foreach (object targetProperty in propertyDefinition.TargetProperties)
						{
							Type type = targetProperty as Type;
							if (type != null && type.IsAssignableFrom(propertyItemType))
							{
								return propertyDefinition.PropertyDefinitions;
							}
						}
					}
				}
			}
			return null;
		}

		bool? IPropertyContainer.IsPropertyVisible(PropertyDescriptor pd)
		{
			if (_parentNode != null)
			{
				return _parentNode.IsPropertyVisible(pd);
			}
			return null;
		}

		bool? IPropertyContainer.CanExpandProperty(PropertyDescriptor pd)
		{
			if (_parentNode != null)
			{
				return _parentNode.CanExpandProperty(pd);
			}
			return null;
		}
	}
}
