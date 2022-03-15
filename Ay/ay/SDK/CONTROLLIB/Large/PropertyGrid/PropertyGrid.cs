using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>
	///   <para>Represents a control that allows users to inspect and edit the properties of an object.</para>
	/// </summary>
	[StyleTypedProperty(Property = "PropertyContainerStyle", StyleTargetType = typeof(PropertyItemBase))]
	[TemplatePart(Name = "PART_DragThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_PropertyItemsControl", Type = typeof(PropertyItemsControl))]
	public class PropertyGrid : Control, ISupportInitialize, IPropertyContainer, INotifyPropertyChanged
	{
		private const string PART_DragThumb = "PART_DragThumb";

		internal const string PART_PropertyItemsControl = "PART_PropertyItemsControl";

		private static readonly ComponentResourceKey SelectedObjectAdvancedOptionsMenuKey;

		private Thumb _dragThumb;

		private bool _hasPendingSelectedObjectChanged;

		private int _initializationCount;

		private ContainerHelperBase _containerHelper;

		private WeakEventListener<NotifyCollectionChangedEventArgs> _propertyDefinitionsListener;

		private WeakEventListener<NotifyCollectionChangedEventArgs> _editorDefinitionsListener;

		private List<KeyValuePair<string, PropertyItem>> _dependsOnPropertyItemsList = new List<KeyValuePair<string, PropertyItem>>();

		private WeakEventListener<NotifyCollectionChangedEventArgs> _categoryDefinitionsListener;

		private CategoryDefinitionCollection _categoryDefinitions;

		private WeakEventListener<NotifyCollectionChangedEventArgs> _selectedObjectsListener;

		private Binding _propertyValueBinding;

		private Binding _propertyNameBinding;

		private IList _selectedObjects;

		/// <summary>Identifies the AdvancedOptionsMenu dependency property.</summary>
		public static readonly DependencyProperty AdvancedOptionsMenuProperty;

		/// <summary>Identifies the AutoGenerateProperties dependency
		/// property.</summary>
		public static readonly DependencyProperty AutoGeneratePropertiesProperty;

		/// <summary>Identifies the CategoryGroupHeaderTemplate
		/// dependency property.</summary>
		public static readonly DependencyProperty CategoryGroupHeaderTemplateProperty;

		/// <summary>Identifies the ShowDescriptionByTooltip dependency
		/// property.</summary>
		public static readonly DependencyProperty ShowDescriptionByTooltipProperty;

		/// <summary>Identifies the ShowSummary dependency property.</summary>
		public static readonly DependencyProperty ShowSummaryProperty;

		/// <summary>Identifies the EditorDefinitions dependency property.</summary>
		public static readonly DependencyProperty EditorDefinitionsProperty;

		/// <summary>Identifies the Filter dependency property.</summary>
		public static readonly DependencyProperty FilterProperty;

		/// <summary>Identifies the FilterWatermark dependency property.</summary>
		public static readonly DependencyProperty FilterWatermarkProperty;

		/// <summary>Identifies the HideInheritedProperties dependency
		/// property.</summary>
		public static readonly DependencyProperty HideInheritedPropertiesProperty;

		/// <summary>Identifies the IsCategorized dependency property.</summary>
		public static readonly DependencyProperty IsCategorizedProperty;

		/// <summary>Identifies the IsMiscCategoryLabelHidden dependency
		/// property.</summary>
		public static readonly DependencyProperty IsMiscCategoryLabelHiddenProperty;

		/// <summary>Identifies the IsScrollingToTopAfterRefresh
		/// dependency property.</summary>
		public static readonly DependencyProperty IsScrollingToTopAfterRefreshProperty;

		/// <summary>Identifies the IsVirtualizing dependency property.</summary>
		public static readonly DependencyProperty IsVirtualizingProperty;

		/// <summary>Identifies the IsExpandingNonPrimitiveTypes
		/// dependency property.</summary>
		public static readonly DependencyProperty IsExpandingNonPrimitiveTypesProperty;

		/// <summary>Identifies the CategoryGroupDescription dependency
		/// property.</summary>
		public static readonly DependencyProperty CategoryGroupDescriptionProperty;

		/// <summary>Identifies the DefaultEditorDefinition dependency
		/// property.</summary>
		public static readonly DependencyProperty DefaultEditorDefinitionProperty;

		/// <summary>Identifies the NameColumnWidth dependency property.</summary>
		public static readonly DependencyProperty NameColumnWidthProperty;

		/// <summary>Identifies the PropertyNameLeftPadding dependency
		/// property.</summary>
		public static readonly DependencyProperty PropertyNameLeftPaddingProperty;

		/// <summary>Identifies the PropertiesSource dependency property.</summary>
		public static readonly DependencyProperty PropertiesSourceProperty;

		/// <summary>Identifies the PropertyContainerStyle dependency
		/// property.</summary>
		public static readonly DependencyProperty PropertyContainerStyleProperty;

		/// <summary>Identifies the PropertyDefinitions dependency property.</summary>
		public static readonly DependencyProperty PropertyDefinitionsProperty;

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty;

		/// <summary>Identifies the SelectedObject dependency property.</summary>
		public static readonly DependencyProperty SelectedObjectProperty;

		/// <summary>Identifies the SelectedObjectType dependency property.</summary>
		public static readonly DependencyProperty SelectedObjectTypeProperty;

		/// <summary>Identifies the SelectedObjectTypeName dependency
		/// property.</summary>
		public static readonly DependencyProperty SelectedObjectTypeNameProperty;

		/// <summary>Identifies the SelectedObjectName dependency property.</summary>
		public static readonly DependencyProperty SelectedObjectNameProperty;

		/// <summary>Identifies the SelectedObjectsOverride dependency
		/// property.</summary>
		public static readonly DependencyProperty SelectedObjectsOverrideProperty;

		private static readonly DependencyPropertyKey SelectedPropertyItemPropertyKey;

		/// <summary>Identifies the SelectedPropertyItem dependency property.</summary>
		public static readonly DependencyProperty SelectedPropertyItemProperty;

		/// <summary>Identifies the SelectedProperty dependency property.</summary>
		public static readonly DependencyProperty SelectedPropertyProperty;

		/// <summary>Identifies the ShowAdvancedOptions dependency property.</summary>
		public static readonly DependencyProperty ShowAdvancedOptionsProperty;

		/// <summary>Identifies the ShowHorizontalScrollBar dependency
		/// property.</summary>
		public static readonly DependencyProperty ShowHorizontalScrollBarProperty;

		/// <summary>Identifies the ShowPreview dependency property.</summary>
		public static readonly DependencyProperty ShowPreviewProperty;

		/// <summary>Identifies the ShowSearchBox dependency property.</summary>
		public static readonly DependencyProperty ShowSearchBoxProperty;

		/// <summary>Identifies the ShowSortOptions dependency property.</summary>
		public static readonly DependencyProperty ShowSortOptionsProperty;

		/// <summary>Identifies the ShowTitle dependency property.</summary>
		public static readonly DependencyProperty ShowTitleProperty;

		/// <summary>Identifies the UpdateTextBoxSourceOnEnterKey
		/// dependency property.</summary>
		public static readonly DependencyProperty UpdateTextBoxSourceOnEnterKeyProperty;

		/// <summary>Identifies the PropertyValueChanged routed event.</summary>
		public static readonly RoutedEvent PropertyValueChangedEvent;

		/// <summary>Identifies the SelectedPropertyItemChanged
		/// routed event.</summary>
		public static readonly RoutedEvent SelectedPropertyItemChangedEvent;

		/// <summary>Identifies the SelectedObjectChanged routed event.</summary>
		public static readonly RoutedEvent SelectedObjectChangedEvent;

		public static readonly RoutedEvent SelectedObjectsOverrideChangedEvent;

		/// <summary>Identifies the PreparePropertyItem routed event..</summary>
		public static readonly RoutedEvent PreparePropertyItemEvent;

		public static readonly RoutedEvent ClearPropertyItemEvent;

		public static readonly RoutedEvent PropertiesGeneratedEvent;

		/// <summary>Gets or sets the contextual menu to use when the advanced menu button is clicked.</summary>
		public ContextMenu AdvancedOptionsMenu
		{
			get
			{
				return (ContextMenu)GetValue(AdvancedOptionsMenuProperty);
			}
			set
			{
				SetValue(AdvancedOptionsMenuProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the PropertyGrid will generate all properties for a given object.</summary>
		public bool AutoGenerateProperties
		{
			get
			{
				return (bool)GetValue(AutoGeneratePropertiesProperty);
			}
			set
			{
				SetValue(AutoGeneratePropertiesProperty, value);
			}
		}

		/// <summary>Gets or sets the DataTemplate to use to define the category headers when the propertyGrid is categorized.</summary>
		public DataTemplate CategoryGroupHeaderTemplate
		{
			get
			{
				return (DataTemplate)GetValue(CategoryGroupHeaderTemplateProperty);
			}
			set
			{
				SetValue(CategoryGroupHeaderTemplateProperty, value);
			}
		}

		/// <summary>Gets/Sets if the Description of the PropertyItem will be displayed as a tooltip on the PART_Name of the PropertyItem. When ShowDescriptionByTooltip is True and
		/// the DescriptionAttribute on the PropertyItem is not null and not empty, it will be displayed as a tooltip.</summary>
		public bool ShowDescriptionByTooltip
		{
			get
			{
				return (bool)GetValue(ShowDescriptionByTooltipProperty);
			}
			set
			{
				SetValue(ShowDescriptionByTooltipProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the summary pane is shown.</summary>
		public bool ShowSummary
		{
			get
			{
				return (bool)GetValue(ShowSummaryProperty);
			}
			set
			{
				SetValue(ShowSummaryProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets a collection of custom editors to use in place of the default editors.</para>
		/// </summary>
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

		/// <summary>Gets or sets the filter used to filter the visible properties in the PropertyGrid.</summary>
		public string Filter
		{
			get
			{
				return (string)GetValue(FilterProperty);
			}
			set
			{
				SetValue(FilterProperty, value);
			}
		}

		/// <summary>Gets or sets the watermark used in the filter field.</summary>
		public string FilterWatermark
		{
			get
			{
				return (string)GetValue(FilterWatermarkProperty);
			}
			set
			{
				SetValue(FilterWatermarkProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets a value indicating if the inherited properties of the selected object will be hidden.</para>
		/// </summary>
		public bool HideInheritedProperties
		{
			get
			{
				return (bool)GetValue(HideInheritedPropertiesProperty);
			}
			set
			{
				SetValue(HideInheritedPropertiesProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the properties displayed in the PropertyGrid are categorized.</summary>
		public bool IsCategorized
		{
			get
			{
				return (bool)GetValue(IsCategorizedProperty);
			}
			set
			{
				SetValue(IsCategorizedProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the "Misc" category expander should be hidden.</summary>
		public bool IsMiscCategoryLabelHidden
		{
			get
			{
				return (bool)GetValue(IsMiscCategoryLabelHiddenProperty);
			}
			set
			{
				SetValue(IsMiscCategoryLabelHiddenProperty, value);
			}
		}

		/// <summary>
		///   <span id="BugEvents">Gets or sets if the PropertyGrid's vertical scrollViewer will scroll to top following a refresh of the PropertyItems.</span>
		/// </summary>
		public bool IsScrollingToTopAfterRefresh
		{
			get
			{
				return (bool)GetValue(IsScrollingToTopAfterRefreshProperty);
			}
			set
			{
				SetValue(IsScrollingToTopAfterRefreshProperty, value);
			}
		}

		/// <summary>&lt;P&gt;Gets or sets if the PropertyGrid is using Virtualization.&lt;/P&gt;&lt;innovasys:widget type="Note Box" layout="block"&gt;&lt;innovasys:widgetproperty
		/// layout="block" name="Content"&gt;Using Virtualization will load faster a SelectedObject with a large set of propertyItems, but the scrollbar thumb will
		/// estimate its size and re-adjust upon scrolling.&lt;/innovasys:widgetproperty&gt;&lt;/innovasys:widget&gt;</summary>
		public bool IsVirtualizing
		{
			get
			{
				return (bool)GetValue(IsVirtualizingProperty);
			}
			set
			{
				SetValue(IsVirtualizingProperty, value);
			}
		}

		/// <summary>Gets or sets if the non-primitive types properties will be expandables to edit their sub-items directly from the PropertyGrid. Default is false.</summary>
		public bool IsExpandingNonPrimitiveTypes
		{
			get
			{
				return (bool)GetValue(IsExpandingNonPrimitiveTypesProperty);
			}
			set
			{
				SetValue(IsExpandingNonPrimitiveTypesProperty, value);
			}
		}

		public CategoryDefinitionCollection CategoryDefinitions
		{
			get
			{
				return _categoryDefinitions;
			}
			set
			{
				if (_categoryDefinitions != value)
				{
					CategoryDefinitionCollection categoryDefinitions = _categoryDefinitions;
					_categoryDefinitions = value;
					OnCategoryDefinitionsChanged(categoryDefinitions, value);
				}
			}
		}

		/// <summary>
		///   <para>Gets or sets the GroupDescription to be applied on the source items in order to define the groups when the PropertyGrid is Categorized.</para>
		/// </summary>
		public GroupDescription CategoryGroupDescription
		{
			get
			{
				return (GroupDescription)GetValue(CategoryGroupDescriptionProperty);
			}
			set
			{
				SetValue(CategoryGroupDescriptionProperty, value);
			}
		}

		/// <summary>Gets or sets the Binding to be used on the property's underlying item to get the name of the property to display</summary>
		public Binding PropertyNameBinding
		{
			get
			{
				return _propertyNameBinding;
			}
			set
			{
				
				if (_propertyNameBinding != value)
				{
					ValidatePropertyBinding(value, () => PropertyNameBinding);
					_propertyNameBinding = value;
				}
			}
		}

		/// <summary>Gets or sets the Binding to be used on the property's underlying item to get the value of the property to display.</summary>
		public Binding PropertyValueBinding
		{
			get
			{
				return _propertyValueBinding;
			}
			set
			{
				
				if (_propertyValueBinding != value)
				{
					ValidatePropertyBinding(value, () => PropertyValueBinding);
					_propertyValueBinding = value;
				}
			}
		}

		/// <summary>Gets or sets the default editor definition to use when the property value type is not supported.</summary>
		public EditorDefinitionBase DefaultEditorDefinition
		{
			get
			{
				return (EditorDefinitionBase)GetValue(DefaultEditorDefinitionProperty);
			}
			set
			{
				SetValue(DefaultEditorDefinitionProperty, value);
			}
		}

		/// <summary>Gets or sets the width of the property name column.</summary>
		public double NameColumnWidth
		{
			get
			{
				return (double)GetValue(NameColumnWidthProperty);
			}
			set
			{
				SetValue(NameColumnWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the left padding of each propertyItem name in the PropertyGrid.</summary>
		public double PropertyNameLeftPadding
		{
			get
			{
				return (double)GetValue(PropertyNameLeftPaddingProperty);
			}
			set
			{
				SetValue(PropertyNameLeftPaddingProperty, value);
			}
		}

		/// <summary>Gets the current collection of generated PropertyItem instances.</summary>
		public IList Properties
		{
			get
			{
				if (_containerHelper == null)
				{
					return null;
				}
				return _containerHelper.Properties;
			}
		}

		/// <summary>Gets or sets the items source for the properties of the PropertyGrid.</summary>
		public IEnumerable PropertiesSource
		{
			get
			{
				return (IEnumerable)GetValue(PropertiesSourceProperty);
			}
			set
			{
				SetValue(PropertiesSourceProperty, value);
			}
		}

		/// <summary>Gets or sets the style that will be applied to all PropertyItemBase instances displayed in the property grid.</summary>
		public Style PropertyContainerStyle
		{
			get
			{
				return (Style)GetValue(PropertyContainerStyleProperty);
			}
			set
			{
				SetValue(PropertyContainerStyleProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets the collection of PropertyDefinition instances.</para>
		/// </summary>
		public PropertyDefinitionCollection PropertyDefinitions
		{
			get
			{
				return (PropertyDefinitionCollection)GetValue(PropertyDefinitionsProperty);
			}
			set
			{
				SetValue(PropertyDefinitionsProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the property grid is read-only.</summary>
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

		/// <summary>Gets or sets the current object the PropertyGrid is inspecting,</summary>
		public object SelectedObject
		{
			get
			{
				return GetValue(SelectedObjectProperty);
			}
			set
			{
				SetValue(SelectedObjectProperty, value);
			}
		}

		/// <summary>Gets or sets the Type of the SelectedObject.</summary>
		public Type SelectedObjectType
		{
			get
			{
				return (Type)GetValue(SelectedObjectTypeProperty);
			}
			set
			{
				SetValue(SelectedObjectTypeProperty, value);
			}
		}

		/// <summary>Gets or sets the name of the Type of the SelectedObject.</summary>
		public string SelectedObjectTypeName
		{
			get
			{
				return (string)GetValue(SelectedObjectTypeNameProperty);
			}
			set
			{
				SetValue(SelectedObjectTypeNameProperty, value);
			}
		}

		/// <summary>Gets or sets the name of the SelectedObject.</summary>
		public string SelectedObjectName
		{
			get
			{
				return (string)GetValue(SelectedObjectNameProperty);
			}
			set
			{
				SetValue(SelectedObjectNameProperty, value);
			}
		}

		/// <summary>Gets the currently selected objects the PropertyGrid is inspecting,</summary>
		public IList SelectedObjects
		{
			get
			{
				return _selectedObjects;
			}
		}

		/// <summary>Gets or sets the list of selected objects.</summary>
		public IList SelectedObjectsOverride
		{
			get
			{
				return (IList)GetValue(SelectedObjectsOverrideProperty);
			}
			set
			{
				SetValue(SelectedObjectsOverrideProperty, value);
			}
		}

		/// <summary>Gets the selected PropertyItem.</summary>
		public PropertyItemBase SelectedPropertyItem
		{
			get
			{
				return (PropertyItemBase)GetValue(SelectedPropertyItemProperty);
			}
			internal set
			{
				SetValue(SelectedPropertyItemPropertyKey, value);
			}
		}

		/// <summary>Gets or sets the selected property or returns null if the selection is empty.</summary>
		public object SelectedProperty
		{
			get
			{
				return GetValue(SelectedPropertyProperty);
			}
			set
			{
				SetValue(SelectedPropertyProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the advanced options button next to the properties is displayed.</summary>
		public bool ShowAdvancedOptions
		{
			get
			{
				return (bool)GetValue(ShowAdvancedOptionsProperty);
			}
			set
			{
				SetValue(ShowAdvancedOptionsProperty, value);
			}
		}

		/// <summary>Gets or sets if the horizontal scroll bar will be visible in the PropertyGrid.</summary>
		public bool ShowHorizontalScrollBar
		{
			get
			{
				return (bool)GetValue(ShowHorizontalScrollBarProperty);
			}
			set
			{
				SetValue(ShowHorizontalScrollBarProperty, value);
			}
		}

		public bool ShowPreview
		{
			get
			{
				return (bool)GetValue(ShowPreviewProperty);
			}
			set
			{
				SetValue(ShowPreviewProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the search box is displayed.</summary>
		public bool ShowSearchBox
		{
			get
			{
				return (bool)GetValue(ShowSearchBoxProperty);
			}
			set
			{
				SetValue(ShowSearchBoxProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the sort options are displayed (Categorized and Alphabetical).</summary>
		public bool ShowSortOptions
		{
			get
			{
				return (bool)GetValue(ShowSortOptionsProperty);
			}
			set
			{
				SetValue(ShowSortOptionsProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the PropertyGrid's title is displayed.</summary>
		public bool ShowTitle
		{
			get
			{
				return (bool)GetValue(ShowTitleProperty);
			}
			set
			{
				SetValue(ShowTitleProperty, value);
			}
		}

		public bool UpdateTextBoxSourceOnEnterKey
		{
			get
			{
				return (bool)GetValue(UpdateTextBoxSourceOnEnterKeyProperty);
			}
			set
			{
				SetValue(UpdateTextBoxSourceOnEnterKeyProperty, value);
			}
		}

		FilterInfo IPropertyContainer.FilterInfo
		{
			get
			{
				FilterInfo result = default(FilterInfo);
				result.Predicate = CreateFilter(Filter);
				result.InputString = Filter;
				return result;
			}
		}

		ContainerHelperBase IPropertyContainer.ContainerHelper
		{
			get
			{
				return _containerHelper;
			}
		}

		bool IPropertyContainer.IsSortedAlphabetically
		{
			get
			{
				return true;
			}
		}

		List<KeyValuePair<string, PropertyItem>> IPropertyContainer.DependsOnPropertyItemsList
		{
			get
			{
				return _dependsOnPropertyItemsList;
			}
		}

		/// <summary>Raised when the value of a property has changed.</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Raised when a property's value changes.</summary>
		public event PropertyValueChangedEventHandler PropertyValueChanged
		{
			add
			{
				AddHandler(PropertyValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(PropertyValueChangedEvent, value);
			}
		}

		/// <summary>Raised when SelectedProperty changes.</summary>
		public event RoutedPropertyChangedEventHandler<PropertyItemBase> SelectedPropertyItemChanged
		{
			add
			{
				AddHandler(SelectedPropertyItemChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedPropertyItemChangedEvent, value);
			}
		}

		/// <summary>Raised when the selected object changes.</summary>
		public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
		{
			add
			{
				AddHandler(SelectedObjectChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedObjectChangedEvent, value);
			}
		}

		/// <summary>
		///   <font size="2">Raised for each propertyItem of the PropertyGrid.SelectedObject when someone is registered to this event and want to set individually the
		/// visibility of propertyItems in the PropertyGrid.</font>
		/// </summary>
		public event IsPropertyBrowsableHandler IsPropertyBrowsable;

		/// <summary>Raised for each propertyItem of the PropertyGrid.SelectedObject when someone is registered to this event and want to set individually if propertyItems are
		/// expandables in the PropertyGrid.</summary>
		public event IsPropertyExpandableHandler IsPropertyExpandable;

		/// <summary>Raised when the SelectedObjectsOverride property changes.</summary>
		public event RoutedPropertyChangedEventHandler<object> SelectedObjectsOverrideChanged
		{
			add
			{
				AddHandler(SelectedObjectsOverrideChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedObjectsOverrideChangedEvent, value);
			}
		}

		/// <summary>
		///   <para>Raised when a property item is about to be displayed, either from PropertyItem or PropertyGrid, depending on which one is being expanded.</para>
		/// </summary>
		public event PropertyItemEventHandler PreparePropertyItem
		{
			add
			{
				AddHandler(PreparePropertyItemEvent, value);
			}
			remove
			{
				RemoveHandler(PreparePropertyItemEvent, value);
			}
		}

		/// <summary>Raised when an property item is about to be remove from the display in the PropertyGrid.</summary>
		public event PropertyItemEventHandler ClearPropertyItem
		{
			add
			{
				AddHandler(ClearPropertyItemEvent, value);
			}
			remove
			{
				RemoveHandler(ClearPropertyItemEvent, value);
			}
		}

		/// <summary>Raised when all the properties of the PropertyGrid are generated.</summary>
		public event RoutedEventHandler PropertiesGenerated
		{
			add
			{
				AddHandler(PropertiesGeneratedEvent, value);
			}
			remove
			{
				RemoveHandler(PropertiesGeneratedEvent, value);
			}
		}

		private static void OnEditorDefinitionsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnEditorDefinitionsChanged((EditorDefinitionCollection)e.OldValue, (EditorDefinitionCollection)e.NewValue);
			}
		}

		/// <summary>Called when EditorDefinitions changes.</summary>
		/// <param name="oldValue">The old EditorDefinitionCollection value.</param>
		/// <param name="newValue">The new EditorDefinitionCollection value.</param>
		protected virtual void OnEditorDefinitionsChanged(EditorDefinitionCollection oldValue, EditorDefinitionCollection newValue)
		{
			if (oldValue != null)
			{
				CollectionChangedEventManager.RemoveListener(oldValue, _editorDefinitionsListener);
			}
			if (newValue != null)
			{
				CollectionChangedEventManager.AddListener(newValue, _editorDefinitionsListener);
			}
			this.Notify(this.PropertyChanged, () => EditorDefinitions);
		}

		private void OnEditorDefinitionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_containerHelper != null)
			{
				_containerHelper.NotifyEditorDefinitionsCollectionChanged();
			}
		}

		private static void OnFilterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnFilterChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when Filter changes.</summary>
		/// <param name="oldValue">The old string value of Filter.</param>
		/// <param name="newValue">The new string value of Filter.</param>
		protected virtual void OnFilterChanged(string oldValue, string newValue)
		{
			this.Notify(this.PropertyChanged, () => ((IPropertyContainer)this).FilterInfo);
		}

		private static void OnIsCategorizedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnIsCategorizedChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		/// <summary>Called when IsCategorized changes.</summary>
		/// <param name="oldValue">The old bool value of IsCategorized.</param>
		/// <param name="newValue">The new bool value of IsCategorized.</param>
		protected virtual void OnIsCategorizedChanged(bool oldValue, bool newValue)
		{
			UpdateThumb();
		}

		private static void OnIsVirtualizingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnIsVirtualizingChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsVirtualizingChanged(bool oldValue, bool newValue)
		{
			UpdateContainerHelper();
		}

		private static void OnIsExpandingNonPrimitiveTypesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnIsExpandingNonPrimitiveTypesChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsExpandingNonPrimitiveTypesChanged(bool oldValue, bool newValue)
		{
			UpdateContainerHelper();
		}

		protected virtual void OnCategoryDefinitionsChanged(CategoryDefinitionCollection oldValue, CategoryDefinitionCollection newValue)
		{
			if (oldValue != null)
			{
				CollectionChangedEventManager.RemoveListener(oldValue, _categoryDefinitionsListener);
			}
			if (newValue != null)
			{
				CollectionChangedEventManager.AddListener(newValue, _categoryDefinitionsListener);
			}
			this.Notify(this.PropertyChanged, () => CategoryDefinitions);
		}

		private void OnCategoryDefinitionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_containerHelper != null)
			{
				_containerHelper.NotifyCategoryDefinitionsCollectionChanged();
			}
		}

		private static object OnCoerceCategoryGroupDescription(DependencyObject o, object value)
		{
			
			return value;
		}

		private static void OnCategoryGroupDescriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnCategoryGroupDescriptionChanged((GroupDescription)e.OldValue, (GroupDescription)e.NewValue);
			}
		}

		private void OnCategoryGroupDescriptionChanged(GroupDescription oldValue, GroupDescription newValue)
		{
			UpdateThumb();
		}

		private static object OnCoerceDefaultEditorDefinition(DependencyObject o, object value)
		{
			
			return value;
		}

		private static void OnNameColumnWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnNameColumnWidthChanged((double)e.OldValue, (double)e.NewValue);
			}
		}

		/// <summary>Called when NameColumnWidth changes.</summary>
		/// <param name="oldValue">The old double value.</param>
		/// <param name="newValue">The new double value.</param>
		protected virtual void OnNameColumnWidthChanged(double oldValue, double newValue)
		{
			if (_dragThumb != null)
			{
				((TranslateTransform)_dragThumb.RenderTransform).X = newValue;
			}
		}

		private static object OnCoercePropertiesSourceChanged(DependencyObject o, object value)
		{
			
			return value;
		}

		private static void OnPropertiesSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnPropertiesSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
			}
		}

		private void OnPropertiesSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			UpdateContainerHelper();
		}

		private static void OnPropertyContainerStyleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnPropertyContainerStyleChanged((Style)e.OldValue, (Style)e.NewValue);
			}
		}

		/// <summary>Called when PropertyContainerStyle has changed.</summary>
		protected virtual void OnPropertyContainerStyleChanged(Style oldValue, Style newValue)
		{
		}

		private static void OnPropertyDefinitionsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnPropertyDefinitionsChanged((PropertyDefinitionCollection)e.OldValue, (PropertyDefinitionCollection)e.NewValue);
			}
		}

		/// <summary>Called when PropertyDefinitions changes.</summary>
		/// <param name="oldValue">The old PropertyDefinitionCollection value.</param>
		/// <param name="newValue">The new PropertyDefinitionCollection value.</param>
		protected virtual void OnPropertyDefinitionsChanged(PropertyDefinitionCollection oldValue, PropertyDefinitionCollection newValue)
		{
			if (oldValue != null)
			{
				CollectionChangedEventManager.RemoveListener(oldValue, _propertyDefinitionsListener);
			}
			if (newValue != null)
			{
				CollectionChangedEventManager.AddListener(newValue, _propertyDefinitionsListener);
			}
			this.Notify(this.PropertyChanged, () => PropertyDefinitions);
		}

		private void OnPropertyDefinitionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_containerHelper != null)
			{
				_containerHelper.NotifyPropertyDefinitionsCollectionChanged();
			}
			if (base.IsLoaded)
			{
				UpdateContainerHelper();
			}
		}

		private static void OnIsReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnIsReadOnlyChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsReadOnlyChanged(bool oldValue, bool newValue)
		{
			UpdateContainerHelper();
		}

		private static void OnSelectedObjectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnSelectedObjectChanged(e.OldValue, e.NewValue);
			}
		}

		/// <summary>Called when SelectedObject changes.</summary>
		/// <param name="oldValue">The old value of SelectedObject.</param>
		/// <param name="newValue">The new value of SelectedObject.</param>
		protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
		{
			if (_initializationCount != 0)
			{
				_hasPendingSelectedObjectChanged = true;
			}
			else
			{
				UpdateContainerHelper();
				RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
			}
		}

		private static void OnSelectedObjectTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnSelectedObjectTypeChanged((Type)e.OldValue, (Type)e.NewValue);
			}
		}

		/// <summary>Called when SelectedObjectType changes.</summary>
		/// <param name="oldValue">The old SelectedObjectType value.</param>
		/// <param name="newValue">The new SelectedObjectType value.</param>
		protected virtual void OnSelectedObjectTypeChanged(Type oldValue, Type newValue)
		{
		}

		private static object OnCoerceSelectedObjectName(DependencyObject o, object baseValue)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null && propertyGrid.SelectedObject is FrameworkElement && string.IsNullOrEmpty((string)baseValue))
			{
				return "<no name>";
			}
			return baseValue;
		}

		private static void OnSelectedObjectNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.SelectedObjectNameChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when SelectedObjectName changes.</summary>
		/// <param name="oldValue">The old string value of SelectedObjectName.</param>
		/// <param name="newValue">The new string value of SelectedObjectName.</param>
		protected virtual void SelectedObjectNameChanged(string oldValue, string newValue)
		{
		}

		private static object OnCoerceSelectedObjectsOverride(DependencyObject sender, object value)
		{
			ValidateSelectedObjectsCollection((IList)value);
			return value;
		}

		private static void OnSelectedObjectsOverrideChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PropertyGrid)sender).OnSelectedObjectsOverrideChanged((IList)args.OldValue, (IList)args.NewValue);
		}

		private void OnSelectedObjectsOverrideChanged(IList oldValue, IList newValue)
		{
			SetSelectedObjects((newValue != null) ? newValue : new ObservableCollection<object>());
			RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectsOverrideChangedEvent));
		}

		private static void OnSelectedPropertyItemChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = o as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnSelectedPropertyItemChanged((PropertyItemBase)e.OldValue, (PropertyItemBase)e.NewValue);
			}
		}

		/// <summary>Called when SelectedPropertyItem has changed.</summary>
		protected virtual void OnSelectedPropertyItemChanged(PropertyItemBase oldValue, PropertyItemBase newValue)
		{
			if (oldValue != null)
			{
				oldValue.IsSelected = false;
			}
			if (newValue != null)
			{
				newValue.IsSelected = true;
			}
			SelectedProperty = ((newValue != null && _containerHelper != null) ? _containerHelper.ItemFromContainer(newValue) : null);
			RaiseEvent(new RoutedPropertyChangedEventArgs<PropertyItemBase>(oldValue, newValue, SelectedPropertyItemChangedEvent));
		}

		private static void OnSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			PropertyGrid propertyGrid = sender as PropertyGrid;
			if (propertyGrid != null)
			{
				propertyGrid.OnSelectedPropertyChanged(args.OldValue, args.NewValue);
			}
		}

		private void OnSelectedPropertyChanged(object oldValue, object newValue)
		{
			if (_containerHelper != null)
			{
				object objA = _containerHelper.ItemFromContainer(SelectedPropertyItem);
				if (!object.Equals(objA, newValue))
				{
					SelectedPropertyItem = _containerHelper.ContainerFromItem(newValue);
				}
			}
		}

		static PropertyGrid()
		{
			SelectedObjectAdvancedOptionsMenuKey = new ComponentResourceKey(typeof(PropertyGrid), "SelectedObjectAdvancedOptionsMenu");
			AdvancedOptionsMenuProperty = DependencyProperty.Register("AdvancedOptionsMenu", typeof(ContextMenu), typeof(PropertyGrid), new UIPropertyMetadata(null));
			AutoGeneratePropertiesProperty = DependencyProperty.Register("AutoGenerateProperties", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			CategoryGroupHeaderTemplateProperty = DependencyProperty.Register("CategoryGroupHeaderTemplate", typeof(DataTemplate), typeof(PropertyGrid));
			ShowDescriptionByTooltipProperty = DependencyProperty.Register("ShowDescriptionByTooltip", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			ShowSummaryProperty = DependencyProperty.Register("ShowSummary", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			EditorDefinitionsProperty = DependencyProperty.Register("EditorDefinitions", typeof(EditorDefinitionCollection), typeof(PropertyGrid), new UIPropertyMetadata(null, OnEditorDefinitionsChanged));
			FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata(null, OnFilterChanged));
			FilterWatermarkProperty = DependencyProperty.Register("FilterWatermark", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata("Search"));
			HideInheritedPropertiesProperty = DependencyProperty.Register("HideInheritedProperties", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			IsCategorizedProperty = DependencyProperty.Register("IsCategorized", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true, OnIsCategorizedChanged));
			IsMiscCategoryLabelHiddenProperty = DependencyProperty.Register("IsMiscCategoryLabelHidden", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			IsScrollingToTopAfterRefreshProperty = DependencyProperty.Register("IsScrollingToTopAfterRefresh", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			IsVirtualizingProperty = DependencyProperty.Register("IsVirtualizing", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false, OnIsVirtualizingChanged));
			IsExpandingNonPrimitiveTypesProperty = DependencyProperty.Register("IsExpandingNonPrimitiveTypes", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false, OnIsExpandingNonPrimitiveTypesChanged));
			CategoryGroupDescriptionProperty = DependencyProperty.Register("CategoryGroupDescription", typeof(GroupDescription), typeof(PropertyGrid), new UIPropertyMetadata(null, OnCategoryGroupDescriptionChanged, OnCoerceCategoryGroupDescription));
			DefaultEditorDefinitionProperty = DependencyProperty.Register("DefaultEditorDefinition", typeof(EditorDefinitionBase), typeof(PropertyGrid), new UIPropertyMetadata(null, null, OnCoerceDefaultEditorDefinition));
			NameColumnWidthProperty = DependencyProperty.Register("NameColumnWidth", typeof(double), typeof(PropertyGrid), new UIPropertyMetadata(150.0, OnNameColumnWidthChanged));
			PropertyNameLeftPaddingProperty = DependencyProperty.Register("PropertyNameLeftPadding", typeof(double), typeof(PropertyGrid), new UIPropertyMetadata(15.0));
			PropertiesSourceProperty = DependencyProperty.Register("PropertiesSource", typeof(IEnumerable), typeof(PropertyGrid), new UIPropertyMetadata(null, OnPropertiesSourceChanged, OnCoercePropertiesSourceChanged));
			PropertyContainerStyleProperty = DependencyProperty.Register("PropertyContainerStyle", typeof(Style), typeof(PropertyGrid), new UIPropertyMetadata(null, OnPropertyContainerStyleChanged));
			PropertyDefinitionsProperty = DependencyProperty.Register("PropertyDefinitions", typeof(PropertyDefinitionCollection), typeof(PropertyGrid), new UIPropertyMetadata(null, OnPropertyDefinitionsChanged));
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false, OnIsReadOnlyChanged));
			SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new UIPropertyMetadata(null, OnSelectedObjectChanged));
			SelectedObjectTypeProperty = DependencyProperty.Register("SelectedObjectType", typeof(Type), typeof(PropertyGrid), new UIPropertyMetadata(null, OnSelectedObjectTypeChanged));
			SelectedObjectTypeNameProperty = DependencyProperty.Register("SelectedObjectTypeName", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata(string.Empty));
			SelectedObjectNameProperty = DependencyProperty.Register("SelectedObjectName", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata(string.Empty, OnSelectedObjectNameChanged, OnCoerceSelectedObjectName));
			SelectedObjectsOverrideProperty = DependencyProperty.Register("SelectedObjectsOverride", typeof(IList), typeof(PropertyGrid), new UIPropertyMetadata(null, OnSelectedObjectsOverrideChanged, OnCoerceSelectedObjectsOverride));
			SelectedPropertyItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedPropertyItem", typeof(PropertyItemBase), typeof(PropertyGrid), new UIPropertyMetadata(null, OnSelectedPropertyItemChanged));
			SelectedPropertyItemProperty = SelectedPropertyItemPropertyKey.DependencyProperty;
			SelectedPropertyProperty = DependencyProperty.Register("SelectedProperty", typeof(object), typeof(PropertyGrid), new UIPropertyMetadata(null, OnSelectedPropertyChanged));
			ShowAdvancedOptionsProperty = DependencyProperty.Register("ShowAdvancedOptions", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			ShowHorizontalScrollBarProperty = DependencyProperty.Register("ShowHorizontalScrollBar", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			ShowPreviewProperty = DependencyProperty.Register("ShowPreview", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(false));
			ShowSearchBoxProperty = DependencyProperty.Register("ShowSearchBox", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			ShowSortOptionsProperty = DependencyProperty.Register("ShowSortOptions", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			ShowTitleProperty = DependencyProperty.Register("ShowTitle", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			UpdateTextBoxSourceOnEnterKeyProperty = DependencyProperty.Register("UpdateTextBoxSourceOnEnterKey", typeof(bool), typeof(PropertyGrid), new UIPropertyMetadata(true));
			PropertyValueChangedEvent = EventManager.RegisterRoutedEvent("PropertyValueChanged", RoutingStrategy.Bubble, typeof(PropertyValueChangedEventHandler), typeof(PropertyGrid));
			SelectedPropertyItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedPropertyItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<PropertyItemBase>), typeof(PropertyGrid));
			SelectedObjectChangedEvent = EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));
			SelectedObjectsOverrideChangedEvent = EventManager.RegisterRoutedEvent("SelectedObjectsOverrideChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));
			PreparePropertyItemEvent = EventManager.RegisterRoutedEvent("PreparePropertyItem", RoutingStrategy.Bubble, typeof(PropertyItemEventHandler), typeof(PropertyGrid));
			ClearPropertyItemEvent = EventManager.RegisterRoutedEvent("ClearPropertyItem", RoutingStrategy.Bubble, typeof(PropertyItemEventHandler), typeof(PropertyGrid));
			PropertiesGeneratedEvent = EventManager.RegisterRoutedEvent("PropertiesGenerated", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyGrid));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGrid), new FrameworkPropertyMetadata(typeof(PropertyGrid)));
		}

		/// <summary>Initializes a new instance of the PropertyGrid class.</summary>
		public PropertyGrid()
		{
			
			_propertyDefinitionsListener = new WeakEventListener<NotifyCollectionChangedEventArgs>(OnPropertyDefinitionsCollectionChanged);
			_editorDefinitionsListener = new WeakEventListener<NotifyCollectionChangedEventArgs>(OnEditorDefinitionsCollectionChanged);
			_selectedObjectsListener = new WeakEventListener<NotifyCollectionChangedEventArgs>(OnSelectedObjectsCollectionChanged);
			_categoryDefinitionsListener = new WeakEventListener<NotifyCollectionChangedEventArgs>(OnCategoryDefinitionsCollectionChanged);
			UpdateContainerHelper();
			SetCurrentValue(EditorDefinitionsProperty, new EditorDefinitionCollection());
			PropertyDefinitions = new PropertyDefinitionCollection();
			CategoryDefinitions = new CategoryDefinitionCollection();
			SetSelectedObjects(new ObservableCollection<object>());
			PropertyValueChanged += PropertyGrid_PropertyValueChanged;
			AddHandler(PropertyItemBase.ItemSelectionChangedEvent, new RoutedEventHandler(OnItemSelectionChanged));
			AddHandler(PropertyItemsControl.PreparePropertyItemEvent, new PropertyItemEventHandler(OnPreparePropertyItemInternal));
			AddHandler(PropertyItemsControl.ClearPropertyItemEvent, new PropertyItemEventHandler(OnClearPropertyItemInternal));
			base.CommandBindings.Add(new CommandBinding(PropertyGridCommands.ClearFilter, ClearFilter, CanClearFilter));
		}

		/// <summary>Builds the visual tree for the element.</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_dragThumb != null)
			{
				_dragThumb.DragDelta -= DragThumb_DragDelta;
			}
			_dragThumb = (GetTemplateChild("PART_DragThumb") as Thumb);
			if (_dragThumb != null)
			{
				_dragThumb.DragDelta += DragThumb_DragDelta;
			}
			if (_containerHelper != null)
			{
				_containerHelper.ChildrenItemsControl = (GetTemplateChild("PART_PropertyItemsControl") as PropertyItemsControl);
			}
			TranslateTransform translateTransform = new TranslateTransform();
			translateTransform.X = NameColumnWidth;
			if (_dragThumb != null)
			{
				_dragThumb.RenderTransform = translateTransform;
			}
			UpdateThumb();
		}

		/// <summary>Overridden to allow the control to respond to the Enter key.</summary>
		/// <param name="e">The event data.</param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			TextBox textBox = e.OriginalSource as TextBox;
			if (SelectedPropertyItem != null && e.Key == Key.Return && UpdateTextBoxSourceOnEnterKey && textBox != null && !textBox.AcceptsReturn)
			{
				BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
				if (bindingExpression != null)
				{
					bindingExpression.UpdateSource();
				}
			}
		}

		/// <summary>Called when the PropertyChanged event is raised.</summary>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (ReflectionHelper.IsPublicInstanceProperty(GetType(), e.Property.Name))
			{
				this.Notify(this.PropertyChanged, e.Property.Name);
			}
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		private void OnItemSelectionChanged(object sender, RoutedEventArgs args)
		{
			PropertyItemBase propertyItemBase = (PropertyItemBase)args.OriginalSource;
			if (propertyItemBase.IsSelected)
			{
				SelectedPropertyItem = propertyItemBase;
			}
			else if (object.ReferenceEquals(propertyItemBase, SelectedPropertyItem))
			{
				SelectedPropertyItem = null;
			}
		}

		private void OnPreparePropertyItemInternal(object sender, PropertyItemEventArgs args)
		{
			if (_containerHelper != null)
			{
				_containerHelper.PrepareChildrenPropertyItem(args.PropertyItem, args.Item);
			}
			args.Handled = true;
		}

		private void OnClearPropertyItemInternal(object sender, PropertyItemEventArgs args)
		{
			if (_containerHelper != null)
			{
				_containerHelper.ClearChildrenPropertyItem(args.PropertyItem, args.Item);
			}
			args.Handled = true;
		}

		private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			NameColumnWidth = Math.Min(Math.Max(base.ActualWidth * 0.1, NameColumnWidth + e.HorizontalChange), base.ActualWidth * 0.9);
		}

		private void PropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			PropertyItem propertyItem = e.OriginalSource as PropertyItem;
			if (propertyItem != null)
			{
				if (propertyItem.WillRefreshPropertyGrid)
				{
					Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
					foreach (object property in Properties)
					{
						CustomPropertyItem customPropertyItem = property as CustomPropertyItem;
						if (customPropertyItem != null && !dictionary.Keys.Contains(customPropertyItem.Category))
						{
							dictionary.Add(customPropertyItem.Category, customPropertyItem.IsCategoryExpanded);
						}
					}
					UpdateContainerHelper();
					foreach (KeyValuePair<string, bool> item in dictionary)
					{
						if (item.Value)
						{
							ExpandCategory(item.Key);
						}
						else
						{
							CollapseCategory(item.Key);
						}
					}
				}
				else
				{
					UpdateDependingPropertyItemEditors(propertyItem);
				}
				PropertyItem propertyItem2 = propertyItem.ParentNode as PropertyItem;
				if (propertyItem2 != null && propertyItem2.IsExpandable)
				{
					RebuildPropertyItemEditor(propertyItem2);
				}
			}
		}

		private void ClearFilter(object sender, ExecutedRoutedEventArgs e)
		{
			Filter = string.Empty;
		}

		private void CanClearFilter(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(Filter);
		}

		/// <summary>Collapses all categories in the PropertyGrid.</summary>
		public void CollapseAllCategories()
		{
			SetCategoryItemsExpansion(false);
		}

		/// <summary>Collapses the specified property in the PropertyGrid.</summary>
		/// <param name="groupName">A string representing the name of the category to collapse.</param>
		public void CollapseCategory(string groupName)
		{
			SetCategoryItemsExpansion(groupName, false);
		}

		/// <summary>Expands all categories in the PropertyGrid.</summary>
		public void ExpandAllCategories()
		{
			SetCategoryItemsExpansion(true);
		}

		/// <summary>Expands the specified property in the PropertyGrid.</summary>
		/// <param name="groupName">A string representing the name of the category to expand.</param>
		public void ExpandCategory(string groupName)
		{
			SetCategoryItemsExpansion(groupName, true);
		}

		/// <summary>Gets if the specified category is expanded.</summary>
		/// <returns>
		///   <strong>true</strong> if the specified category is expanded, otherwise <strong>false</strong>.</returns>
		/// <param name="categoryName">The name of the category.</param>
		public bool IsCategoryExpanded(string categoryName)
		{
			return IsCategoryInWantedState(categoryName, true);
		}

		/// <summary>Gets if the specified category is collapsed.</summary>
		/// <returns>
		///   <strong>true</strong> if the specified category is collapsed, otherwise <strong>false</strong>.</returns>
		/// <param name="categoryName">The name of the category.</param>
		public bool IsCategoryCollapsed(string categoryName)
		{
			return IsCategoryInWantedState(categoryName, false);
		}

		/// <summary>Gets a list containing the names of the categories that are collapsed.</summary>
		/// <returns>A list containing the names of the categories that are collapsed.</returns>
		public List<string> GetCollapsedCategories()
		{
			return GetSpecificCategories(false);
		}

		/// <summary>Gets a list containing the names of the categories that are expanded.</summary>
		/// <returns>A list containing the names of the categories that are expanded.</returns>
		public List<string> GetExpandedCategories()
		{
			return GetSpecificCategories(true);
		}

		/// <summary>Gets a list containing all the category names.</summary>
		/// <returns>A list containing all the category names.</returns>
		public List<string> GetCategories()
		{
			return GetSpecificCategories();
		}

		private bool IsCategoryInWantedState(string categoryName, bool isExpanded)
		{
			if (_containerHelper == null || _containerHelper.ChildrenItemsControl == null || !_containerHelper.ChildrenItemsControl.HasItems || _containerHelper.ChildrenItemsControl.Items.Groups == null)
			{
				return false;
			}
			ReadOnlyObservableCollection<object> groups = _containerHelper.ChildrenItemsControl.Items.Groups;
			CollectionViewGroup collectionViewGroup = groups.FirstOrDefault(delegate(object x)
			{
				if (x is CollectionViewGroup)
				{
					return ((CollectionViewGroup)x).Name.Equals(categoryName);
				}
				return false;
			}) as CollectionViewGroup;
			if (collectionViewGroup != null && collectionViewGroup.Items != null)
			{
				foreach (object item in collectionViewGroup.Items)
				{
					CustomPropertyItem customPropertyItem = item as CustomPropertyItem;
					if (customPropertyItem != null)
					{
						return (isExpanded && customPropertyItem.IsCategoryExpanded) || (!isExpanded && !customPropertyItem.IsCategoryExpanded);
					}
				}
			}
			return false;
		}

		private List<string> GetSpecificCategories(bool? isExpanded = default(bool?))
		{
			List<string> list = new List<string>();
			if (_containerHelper == null || _containerHelper.ChildrenItemsControl == null || !_containerHelper.ChildrenItemsControl.HasItems || _containerHelper.ChildrenItemsControl.Items.Groups == null)
			{
				return list;
			}
			ReadOnlyObservableCollection<object> groups = _containerHelper.ChildrenItemsControl.Items.Groups;
			foreach (object item in groups)
			{
				CollectionViewGroup collectionViewGroup = item as CollectionViewGroup;
				if (collectionViewGroup != null && collectionViewGroup.Items != null && collectionViewGroup.Items.Count > 0)
				{
					CustomPropertyItem customPropertyItem = collectionViewGroup.Items[0] as CustomPropertyItem;
					if (customPropertyItem != null && (!isExpanded.HasValue || (isExpanded.HasValue && isExpanded.Value && customPropertyItem.IsCategoryExpanded) || (isExpanded.HasValue && !isExpanded.Value && !customPropertyItem.IsCategoryExpanded)))
					{
						list.Add(customPropertyItem.Category);
					}
				}
			}
			return list;
		}

		private void SetCategoryItemsExpansion(string categoryName, bool isExpanded)
		{
			if (_containerHelper != null && _containerHelper.ChildrenItemsControl != null && _containerHelper.ChildrenItemsControl.HasItems && _containerHelper.ChildrenItemsControl.Items.Groups != null)
			{
				ReadOnlyObservableCollection<object> groups = _containerHelper.ChildrenItemsControl.Items.Groups;
				CollectionViewGroup collectionViewGroup = groups.FirstOrDefault(delegate(object x)
				{
					if (x is CollectionViewGroup)
					{
						return ((CollectionViewGroup)x).Name.Equals(categoryName);
					}
					return false;
				}) as CollectionViewGroup;
				if (collectionViewGroup != null && collectionViewGroup.Items != null)
				{
					foreach (object item in collectionViewGroup.Items)
					{
						if (item is CustomPropertyItem)
						{
							((CustomPropertyItem)item).IsCategoryExpanded = isExpanded;
						}
					}
				}
			}
		}

		private void SetCategoryItemsExpansion(bool isExpanded)
		{
			if (_containerHelper != null && _containerHelper.ChildrenItemsControl != null && _containerHelper.ChildrenItemsControl.HasItems && _containerHelper.ChildrenItemsControl.Items.Groups != null)
			{
				ReadOnlyObservableCollection<object> groups = _containerHelper.ChildrenItemsControl.Items.Groups;
				if (groups != null)
				{
					foreach (object item in groups)
					{
						CollectionViewGroup collectionViewGroup = item as CollectionViewGroup;
						if (collectionViewGroup != null && collectionViewGroup.Items != null)
						{
							foreach (object item2 in collectionViewGroup.Items)
							{
								if (item2 is CustomPropertyItem)
								{
									((CustomPropertyItem)item2).IsCategoryExpanded = isExpanded;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>Returns a value that contains the vertical offset of the PropertyGrid's ScrollViewer.</summary>
		public double GetScrollPosition()
		{
			ScrollViewer scrollViewer = GetScrollViewer();
			if (scrollViewer != null)
			{
				return scrollViewer.VerticalOffset;
			}
			return 0.0;
		}

		/// <summary>Scrolls the PropertyGrid's scrollViewer to the specified vertical offset position.</summary>
		public void ScrollToPosition(double position)
		{
			ScrollViewer scrollViewer = GetScrollViewer();
			if (scrollViewer != null)
			{
				scrollViewer.ScrollToVerticalOffset(position);
			}
		}

		/// <summary>Scrolls vertically to the beginning of the PropertyGrid's ScrollViewer.</summary>
		public void ScrollToTop()
		{
			ScrollViewer scrollViewer = GetScrollViewer();
			if (scrollViewer != null)
			{
				scrollViewer.ScrollToTop();
			}
		}

		/// <summary>Scrolls vertically to the end of the PropertyGrid's ScrollViewer.</summary>
		public void ScrollToBottom()
		{
			ScrollViewer scrollViewer = GetScrollViewer();
			if (scrollViewer != null)
			{
				scrollViewer.ScrollToBottom();
			}
		}

		/// <summary>Collapse all the PropertyItems and their sub-PropertyItems.</summary>
		public void CollapseAllProperties()
		{
			if (_containerHelper != null)
			{
				_containerHelper.SetPropertiesExpansion(false);
			}
		}

		/// <summary>Expand all the PropertyItems and their sub-PropertyItems.</summary>
		public void ExpandAllProperties()
		{
			if (_containerHelper != null)
			{
				_containerHelper.SetPropertiesExpansion(true);
			}
		}

		/// <summary>Expand the PropertyItem( or the sub-PropertyItem) with the specific propertyName.</summary>
		public void ExpandProperty(string propertyName)
		{
			if (_containerHelper != null)
			{
				_containerHelper.SetPropertiesExpansion(propertyName, true);
			}
		}

		/// <summary>Collapse the PropertyItem( or the sub-PropertyItem) with the specific propertyName.</summary>
		public void CollapseProperty(string propertyName)
		{
			if (_containerHelper != null)
			{
				_containerHelper.SetPropertiesExpansion(propertyName, false);
			}
		}

		private ScrollViewer GetScrollViewer()
		{
			if (_containerHelper != null && _containerHelper.ChildrenItemsControl != null)
			{
				return TreeHelper.FindChild<ScrollViewer>(_containerHelper.ChildrenItemsControl);
			}
			return null;
		}

		private void RebuildPropertyItemEditor(PropertyItem propertyItem)
		{
			if (propertyItem != null)
			{
				propertyItem.RebuildEditor();
			}
		}

		private void UpdateContainerHelper()
		{
			ItemsControl childrenItemsControl = (_containerHelper != null) ? _containerHelper.ChildrenItemsControl : null;
			ObjectContainerHelperBase objectContainerHelperBase = null;
			if (PropertiesSource != null)
			{
				SetContainerHelper(new PropertiesSourceContainerHelper(this, PropertiesSource));
			}
			else if (SelectedObjects != null && SelectedObjects.Count > 0)
			{
				objectContainerHelperBase = new ObjectsContainerHelper(this, SelectedObjects);
			}
			else if (SelectedObject != null)
			{
				objectContainerHelperBase = new ObjectContainerHelper(this, SelectedObject);
			}
			else
			{
				SetContainerHelper(new PropertiesContainerHelper(this));
			}
			if (objectContainerHelperBase != null)
			{
				objectContainerHelperBase.ChildrenItemsControl = childrenItemsControl;
				objectContainerHelperBase.ObjectsGenerated += ObjectContainerHelper_ObjectsGenerated;
				objectContainerHelperBase.GenerateProperties();
			}
			else if (_containerHelper != null)
			{
				FinalizeUpdateContainerHelper(childrenItemsControl);
			}
		}

		private void SetContainerHelper(ContainerHelperBase containerHelper)
		{
			if (_containerHelper != null)
			{
				_containerHelper.ClearHelper();
			}
			_containerHelper = containerHelper;
		}

		private void FinalizeUpdateContainerHelper(ItemsControl childrenItemsControl)
		{
			if (!(_containerHelper is ObjectContainerHelper) && _containerHelper != null)
			{
				
			}
			if (_containerHelper != null)
			{
				_containerHelper.ChildrenItemsControl = childrenItemsControl;
			}
			if (IsScrollingToTopAfterRefresh)
			{
				ScrollToTop();
			}
			this.Notify(this.PropertyChanged, () => Properties);
		}

		private void SetSelectedObjects(IList newSelectedObjectsList)
		{
			if (newSelectedObjectsList == null)
			{
				throw new ArgumentNullException("newSelectedObjectsList");
			}
			if (_selectedObjects != null)
			{
				
			}
			INotifyCollectionChanged notifyCollectionChanged = _selectedObjects as INotifyCollectionChanged;
			INotifyCollectionChanged notifyCollectionChanged2 = newSelectedObjectsList as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				CollectionChangedEventManager.RemoveListener(notifyCollectionChanged, _selectedObjectsListener);
			}
			if (notifyCollectionChanged2 != null)
			{
				CollectionChangedEventManager.AddListener(notifyCollectionChanged2, _selectedObjectsListener);
			}
			_selectedObjects = newSelectedObjectsList;
			UpdateContainerHelper();
		}

		private void OnSelectedObjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ValidateSelectedObjectsCollection((IList)sender);
			
			UpdateContainerHelper();
		}

		private static void ValidateSelectedObjectsCollection(IList objectList)
		{
			if (objectList != null && objectList.Cast<object>().Any((object o) => o == null))
			{
				throw new InvalidOperationException("The SelectedObjects collection cannot contain any null entries");
			}
		}

		private void UpdateThumb()
		{
			if (_dragThumb != null)
			{
				if (IsCategorized)
				{
					_dragThumb.Margin = new Thickness(6.0, 0.0, 0.0, 0.0);
				}
				else
				{
					_dragThumb.Margin = new Thickness(-1.0, 0.0, 0.0, 0.0);
				}
			}
		}

		/// <summary>Override this call to control the filter applied based on the text input.</summary>
		protected virtual Predicate<object> CreateFilter(string filter)
		{
			return null;
		}

		/// <summary>Updates all property values in the PropertyGrid with the data from the SelectedObject or <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid~SelectedObjects.html">SelectedObjects</see>.</summary>
		public void Update()
		{
			if (_containerHelper != null)
			{
				_containerHelper.UpdateValuesFromSource();
			}
		}

		private void ValidatePropertyBinding<TMember>(Binding value, Expression<Func<TMember>> property)
		{
			if (value != null)
			{
				if (!string.IsNullOrEmpty(value.BindingGroupName))
				{
					throw new InvalidOperationException("BindingGroupName must be null on " + ReflectionHelper.GetPropertyOrFieldName(property));
				}
				if (value.IsAsync)
				{
					throw new InvalidOperationException("IsAsync must be false on " + ReflectionHelper.GetPropertyOrFieldName(property));
				}
				if (value.RelativeSource != null)
				{
					throw new InvalidOperationException("RelativeSource must be null on " + ReflectionHelper.GetPropertyOrFieldName(property));
				}
				if (value.Source != null)
				{
					throw new InvalidOperationException("Source must be null on " + ReflectionHelper.GetPropertyOrFieldName(property));
				}
			}
		}

		private void UpdateDependingPropertyItemEditors(PropertyItem modifiedPropertyItem)
		{
			if (modifiedPropertyItem != null)
			{
				string text = (modifiedPropertyItem.DescriptorDefinition != null) ? modifiedPropertyItem.DescriptorDefinition.PropertyName : null;
				if (!string.IsNullOrEmpty(text) && modifiedPropertyItem.ParentNode != null && modifiedPropertyItem.ParentNode.DependsOnPropertyItemsList.Count > 0)
				{
					foreach (KeyValuePair<string, PropertyItem> dependsOnPropertyItems in modifiedPropertyItem.ParentNode.DependsOnPropertyItemsList)
					{
						if (dependsOnPropertyItems.Key == text)
						{
							PropertyItem value = dependsOnPropertyItems.Value;
							RebuildPropertyItemEditor(value);
						}
					}
				}
			}
		}

		private void ObjectContainerHelper_ObjectsGenerated(object sender, EventArgs e)
		{
			ObjectContainerHelperBase objectContainerHelperBase = sender as ObjectContainerHelperBase;
			if (objectContainerHelperBase != null)
			{
				objectContainerHelperBase.ObjectsGenerated -= ObjectContainerHelper_ObjectsGenerated;
				SetContainerHelper(objectContainerHelperBase);
				FinalizeUpdateContainerHelper(objectContainerHelperBase.ChildrenItemsControl);
				RaiseEvent(new RoutedEventArgs(PropertiesGeneratedEvent, this));
			}
		}

		/// <summary>Adds a handler for the PreparePropertyItem attached event.</summary>
		/// <param name="element">The element to attach the handler.</param>
		/// <param name="handler">The handler for the event.</param>
		public static void AddPreparePropertyItemHandler(UIElement element, PropertyItemEventHandler handler)
		{
			element.AddHandler(PreparePropertyItemEvent, handler);
		}

		/// <summary>Removes a handler for the PreparePropertyItem attached event.</summary>
		/// <param name="element">The element to attach the handler.</param>
		/// <param name="handler">The handler for the event.</param>
		public static void RemovePreparePropertyItemHandler(UIElement element, PropertyItemEventHandler handler)
		{
			element.RemoveHandler(PreparePropertyItemEvent, handler);
		}

		internal static void RaisePreparePropertyItemEvent(UIElement source, PropertyItemBase propertyItem, object item)
		{
			source.RaiseEvent(new PropertyItemEventArgs(PreparePropertyItemEvent, source, propertyItem, item));
		}

		/// <summary>Adds a handler for the ClearPropertyItem attached event</summary>
		public static void AddClearPropertyItemHandler(UIElement element, PropertyItemEventHandler handler)
		{
			element.AddHandler(ClearPropertyItemEvent, handler);
		}

		/// <summary>Removes a handler for the ClearPropertyItem attached event</summary>
		public static void RemoveClearPropertyItemHandler(UIElement element, PropertyItemEventHandler handler)
		{
			element.RemoveHandler(ClearPropertyItemEvent, handler);
		}

		internal static void RaiseClearPropertyItemEvent(UIElement source, PropertyItemBase propertyItem, object item)
		{
			source.RaiseEvent(new PropertyItemEventArgs(ClearPropertyItemEvent, source, propertyItem, item));
		}

		/// <summary>Signals the beginning of a batch modification process.</summary>
		public override void BeginInit()
		{
			base.BeginInit();
			_initializationCount++;
		}

		/// <summary>Signals the end of a batch modification process.</summary>
		public override void EndInit()
		{
			base.EndInit();
			if (--_initializationCount == 0)
			{
				if (_hasPendingSelectedObjectChanged)
				{
					UpdateContainerHelper();
					_hasPendingSelectedObjectChanged = false;
				}
				if (_containerHelper != null)
				{
					_containerHelper.OnEndInit();
				}
			}
		}

		bool? IPropertyContainer.IsPropertyVisible(PropertyDescriptor pd)
		{
			IsPropertyBrowsableHandler isPropertyBrowsable = this.IsPropertyBrowsable;
			if (isPropertyBrowsable != null)
			{
				IsPropertyBrowsableArgs isPropertyBrowsableArgs = new IsPropertyBrowsableArgs(pd);
				isPropertyBrowsable(this, isPropertyBrowsableArgs);
				return isPropertyBrowsableArgs.IsBrowsable;
			}
			return null;
		}

		bool? IPropertyContainer.CanExpandProperty(PropertyDescriptor pd)
		{
			IsPropertyExpandableHandler isPropertyExpandable = this.IsPropertyExpandable;
			if (isPropertyExpandable != null)
			{
				IsPropertyExpandableArgs isPropertyExpandableArgs = new IsPropertyExpandableArgs(pd);
				isPropertyExpandable(this, isPropertyExpandableArgs);
				return isPropertyExpandableArgs.IsExpandable;
			}
			return null;
		}
	}
}
