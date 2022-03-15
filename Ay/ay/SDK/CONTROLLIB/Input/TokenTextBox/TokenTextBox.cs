using ay.Controls.Args;
using ay.Controls.Info;
using ay.SDK.CONTROLLIB.Primitive;
using ay.UIAutomation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ay.Controls
{
    /// <summary>
    /// ay
    /// 1.不管是不是允许有效值，都会触发无效文本的事件
    /// 2.富文本不允许drop文本进去，会引发bug的
    /// </summary>
	[TemplatePart(Name = "PART_RichTextBox", Type = typeof(System.Windows.Controls.RichTextBox))]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_CoreItemsControl", Type = typeof(TokenTextBoxCoreItemsControl))]
	public class TokenTextBox : ay.SDK.CONTROLLIB.Primitive.Selector,IAyControl
	{
        public string ControlID { get { return ControlGUID.TokenTextBox; } }
        private class GroupedFilters
		{
			private List<Predicate<object>> _filters;

			private List<Predicate<object>> _internalFilters;

			public Predicate<object> ActiveFilter
			{
				get;
				private set;
			}

			public GroupedFilters()
			{
				_filters = new List<Predicate<object>>();
				_internalFilters = new List<Predicate<object>>();
				ActiveFilter = FilterPredicates;
			}

			public void AddFilter(Predicate<object> filter, bool isInternalFilter)
			{
				if (!_filters.Contains(filter))
				{
					_filters.Add(filter);
					if (isInternalFilter)
					{
						_internalFilters.Add(filter);
					}
				}
			}

			public void RemoveFilter(Predicate<object> filter)
			{
				if (_filters.Contains(filter))
				{
					_filters.Remove(filter);
				}
			}

			public void RemoveAllFilters()
			{
				_filters.Clear();
				_internalFilters.Clear();
			}

			public void RemoveInternalFilters()
			{
				foreach (Predicate<object> internalFilter in _internalFilters)
				{
					RemoveFilter(internalFilter);
				}
				_internalFilters.Clear();
			}

			private bool FilterPredicates(object obj)
			{
				foreach (Predicate<object> filter in _filters)
				{
					if (!filter(obj))
					{
						return false;
					}
				}
				return true;
			}
		}

		private const string PART_RichTextBox = "PART_RichTextBox";

		private const string PART_Popup = "PART_Popup";

		private const string PART_CoreItemsControl = "PART_CoreItemsControl";

		private static readonly ComponentResourceKey DefaultTokenItemTemplateKey;

		private System.Windows.Controls.RichTextBox _richTextBox;

		private Popup _dropDownPopup;

		private TokenTextBoxCoreItemsControl _coreItemsControl;

		private int _updateTextFlag;

		private int _updateSelectionFlag;

		private int _updateRichTextBoxFlag;

		private int _updateDropDownVisibilityFlag;

		private bool _ignoreNextSelectionChanged;

		private Paragraph _richContent;

		private string _caretText;

		private GroupedFilters _groupedFilters;

		private CollectionViewSource _collectionViewSource = new CollectionViewSource();

		private ICollectionView _TokenTextBoxView;

		/// <summary>Identifies the Text dependency property.</summary>
		public static readonly DependencyProperty TextProperty;

		public static readonly DependencyProperty FilteredItemsProperty;

		public static readonly DependencyProperty HighlightedItemProperty;

		/// <summary>Identifies the IsDropDownOpen dependency property.</summary>
		public static readonly DependencyProperty IsDropDownOpenProperty;

		/// <summary>Identifies the MaxDropDownHeight dependency property.</summary>
		public static readonly DependencyProperty MaxDropDownHeightProperty;

		/// <summary>Identifies the TokenDelimiter dependency property.</summary>
		public static readonly DependencyProperty TokenDelimiterProperty;

		/// <summary>Identifies the TokenItemTemplate dependency property.</summary>
		public static readonly DependencyProperty TokenItemTemplateProperty;

		/// <summary>Identifies the TokenDisplayMemberPath dependency property.</summary>
		public static readonly DependencyProperty TokenDisplayMemberPathProperty;

		/// <summary>Identifies the TokenItemContainerStyle dependency property.</summary>
		public static readonly DependencyProperty TokenItemContainerStyleProperty;

		/// <summary>Identifies the AllowInvalidValues dependency property.</summary>
		public static readonly DependencyProperty AllowInvalidValuesProperty;

		/// <summary>Identifies the AllowDuplicateValues dependency property.</summary>
		public static readonly DependencyProperty AllowDuplicateValuesProperty;

		/// <summary>Identifies the IsValid dependency property.</summary>
		public static readonly DependencyProperty IsValidProperty;

		/// <summary>Identifies the SearchMemberPaths dependency property.</summary>
		public static readonly DependencyProperty SearchMemberPathsProperty;

		public static readonly RoutedEvent InvalidValueEnteredEvent;

        /// <summary>
        /// 选中的颜色
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty;

        public Brush SelectionBrush
        {
            get
            {
                return (Brush)GetValue(SelectionBrushProperty);
            }
            set
            {
                SetValue(SelectionBrushProperty, value);
            }
        }

        /// <summary>Gets or sets the Text string behind the TokenTextBox's content.</summary>
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

		/// <summary>Gets the filtered items from the TokenTextBox.</summary>
		public IList FilteredItems
		{
			get
			{
				return (IList)GetValue(FilteredItemsProperty);
			}
			private set
			{
				SetValue(FilteredItemsProperty, value);
			}
		}

		/// <summary>Gets the Highlighted item in the ToknizedTextBox.</summary>
		public object HighlightedItem
		{
			get
			{
				return GetValue(HighlightedItemProperty);
			}
			private set
			{
				SetValue(HighlightedItemProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dropdown is open.</summary>
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)GetValue(IsDropDownOpenProperty);
			}
			set
			{
				SetValue(IsDropDownOpenProperty, value);
			}
		}

		/// <summary>Gets or sets a value representing the maximum dropdown height.</summary>
		public double MaxDropDownHeight
		{
			get
			{
				return (double)GetValue(MaxDropDownHeightProperty);
			}
			set
			{
				SetValue(MaxDropDownHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the Delimiter used to identify a Token.</summary>
		public char TokenDelimiter
		{
			get
			{
				return (char)GetValue(TokenDelimiterProperty);
			}
			set
			{
				SetValue(TokenDelimiterProperty, value);
			}
		}

		/// <summary>Gets or sets the DataTemplate used to display each selected token If this value is null, ItemTemplate will be used as a fallback.</summary>
		public DataTemplate TokenItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(TokenItemTemplateProperty);
			}
			set
			{
				SetValue(TokenItemTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets a path to a value on the source object to serve as the visual representation of a selected token. If this value is null, DisplayMemberPath will be
		/// used as a fallback.</summary>
		public string TokenDisplayMemberPath
		{
			get
			{
				return (string)GetValue(TokenDisplayMemberPathProperty);
			}
			set
			{
				SetValue(TokenDisplayMemberPathProperty, value);
			}
		}

		/// <summary>Gets or sets the Style that is applied to the container element generated for each selected token.</summary>
		public Style TokenItemContainerStyle
		{
			get
			{
				return (Style)GetValue(TokenItemContainerStyleProperty);
			}
			set
			{
				SetValue(TokenItemContainerStyleProperty, value);
			}
		}

		/// <summary>Gets or sets if invalid values are allowed (i.e.: values not found in the ItemsSource if an ItemsSource is used).</summary>
		public bool AllowInvalidValues
		{
			get
			{
				return (bool)GetValue(AllowInvalidValuesProperty);
			}
			set
			{
				SetValue(AllowInvalidValuesProperty, value);
			}
		}

		/// <summary>Gets or sets if duplicate Tokens are allowed.</summary>
		public bool AllowDuplicateValues
		{
			get
			{
				return (bool)GetValue(AllowDuplicateValuesProperty);
			}
			set
			{
				SetValue(AllowDuplicateValuesProperty, value);
			}
		}

		/// <summary>Gets if the TokenTextBox is currently valid (if all the Tokens are found in the ItemsSource, if one is provided).</summary>
		public bool IsValid
		{
			get
			{
				return (bool)GetValue(IsValidProperty);
			}
			private set
			{
				SetValue(IsValidProperty, value);
			}
		}

		/// <summary>Gets or sets the field(s) used for filtering valid values (only string columns are considered).</summary>
		public string SearchMemberPaths
		{
			get
			{
				return (string)GetValue(SearchMemberPathsProperty);
			}
			set
			{
				SetValue(SearchMemberPathsProperty, value);
			}
		}

		private Paragraph RichContent
		{
			get
			{
				if (_richTextBox != null)
				{
					if (_richTextBox.Document.Blocks.Count <= 0)
					{
						return null;
					}
					return _richTextBox.Document.Blocks.FirstBlock as Paragraph;
				}
				return _richContent;
			}
		}

		/// <summary>This event can be handled to extend the way a token is converted to text for displaying, matching text, or for updating the Text property. The query result
		/// have priority over the values underlined by the TokenDisplayMemberPath property.</summary>
		public event EventHandler<QueryTextFromItemEventArgs> QueryTextFromItem;

		/// <summary>This event can be handled to extend the way a text input is converted to the corresponding item. Text input may be typed by the user or specified through the
		/// Text property. The query results have priority over the values underlined by the TokenDisplayMemberPath property.</summary>
		public event EventHandler<QueryItemFromTextEventArgs> QueryItemFromText;

		/// <summary>This event can be handled to extend the way a text input is considered to be a search match for an item. Search matches are displayed in the dropdown
		/// suggestion box. The query results have priority over the values underlined by the SearchMemberPaths property.</summary>
		public event EventHandler<QuerySuggestItemForTextEventArgs> QuerySuggestItemForText;

		/// <summary>
		///   <font size="2">This event will be raised when an invalid value will be resolved in the TokenTextBox.</font>
		/// </summary>
		public event RoutedEventHandler InvalidValueEntered
		{
			add
			{
				AddHandler(InvalidValueEnteredEvent, value);
			}
			remove
			{
				RemoveHandler(InvalidValueEnteredEvent, value);
			}
		}

		private static void OnTextValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnTextValueChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		protected virtual void OnTextValueChanged(string oldValue, string newValue)
		{
			if (_updateTextFlag <= 0)
			{
				List<SegmentInfo> list = ConvertTextToSegments(newValue);
				ResolveTextSegments(list);
				SetSelectedItems(list);
				SetRichContent(ConvertSegmentsToRichText(list));
				UpdateIsValid(list);
			}
		}

		private static object OnCoerceTextValue(DependencyObject o, object value)
		{
			return ((TokenTextBox)o).OnCoerceTextValue((string)value);
		}

		protected virtual string OnCoerceTextValue(string value)
		{
			return value;
		}

		private void SetIsDropDownOpenInternal(bool value)
		{
			_updateDropDownVisibilityFlag++;
			try
			{
				SetCurrentValue(IsDropDownOpenProperty, value);
			}
			finally
			{
				_updateDropDownVisibilityFlag--;
			}
		}

		private static void OnIsDropDownOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnIsDropDownOpenChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsDropDownOpenChanged(bool oldValue, bool newValue)
		{
			if (_updateDropDownVisibilityFlag <= 0 && newValue)
			{
				UpdateDropDownContent();
				UpdateDropDownPosition();
				BringHiglightedItemIntoView();
			}
		}

		private static void OnMaxDropDownHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnMaxDropDownHeightChanged((double)e.OldValue, (double)e.NewValue);
			}
		}

		protected virtual void OnMaxDropDownHeightChanged(double oldValue, double newValue)
		{
		}

		private static void OnTokenDelimiterValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnTokenDelimiterValueChanged((char)e.OldValue, (char)e.NewValue);
			}
		}

		protected virtual void OnTokenDelimiterValueChanged(char oldValue, char newValue)
		{
		}

		private static void OnTokenItemTemplateValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnTokenItemTemplateValueChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
			}
		}

		protected virtual void OnTokenItemTemplateValueChanged(DataTemplate oldValue, DataTemplate newValue)
		{
		}

		private static void OnTokenDisplayMemberPathValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnTokenDisplayMemberPathValueChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		protected virtual void OnTokenDisplayMemberPathValueChanged(string oldValue, string newValue)
		{
		}

		private static void OnTokenItemContainerStyleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnTokenItemContainerStyleValueChanged((Style)e.OldValue, (Style)e.NewValue);
			}
		}

		protected virtual void OnTokenItemContainerStyleValueChanged(Style oldValue, Style newValue)
		{
		}

		private static void OnAllowInvalidValuesValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnAllowInvalidValuesValueChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnAllowInvalidValuesValueChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnAllowDuplicateValuesValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TokenTextBox TokenTextBox = o as TokenTextBox;
			if (TokenTextBox != null)
			{
				TokenTextBox.OnAllowDuplicateValuesValueChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnAllowDuplicateValuesValueChanged(bool oldValue, bool newValue)
		{
		}

		internal void SetIsValidInternal(bool isValid)
		{
			SetCurrentValue(IsValidProperty, isValid);
		}

		static TokenTextBox()
		{
			DefaultTokenItemTemplateKey = new ComponentResourceKey(typeof(TokenTextBox), "DefaultTokenItemTemplate");
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TokenTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextValueChanged, OnCoerceTextValue, true, UpdateSourceTrigger.LostFocus));
			FilteredItemsProperty = DependencyProperty.Register("FilteredItems", typeof(IList), typeof(TokenTextBox), new UIPropertyMetadata(null));
			HighlightedItemProperty = DependencyProperty.Register("HighlightedItem", typeof(object), typeof(TokenTextBox), new UIPropertyMetadata(null));
			IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(TokenTextBox), new UIPropertyMetadata(false, OnIsDropDownOpenChanged));
			MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(TokenTextBox), new UIPropertyMetadata(130.0, OnMaxDropDownHeightChanged));
			TokenDelimiterProperty = DependencyProperty.Register("TokenDelimiter", typeof(char), typeof(TokenTextBox), new PropertyMetadata(';', OnTokenDelimiterValueChanged));
			TokenItemTemplateProperty = DependencyProperty.Register("TokenItemTemplate", typeof(DataTemplate), typeof(TokenTextBox), new UIPropertyMetadata(null, OnTokenItemTemplateValueChanged));
			TokenDisplayMemberPathProperty = DependencyProperty.Register("TokenDisplayMemberPath", typeof(string), typeof(TokenTextBox), new UIPropertyMetadata(null, OnTokenDisplayMemberPathValueChanged));
			TokenItemContainerStyleProperty = DependencyProperty.Register("TokenItemContainerStyle", typeof(Style), typeof(TokenTextBox), new UIPropertyMetadata(null, OnTokenItemContainerStyleValueChanged));
			AllowInvalidValuesProperty = DependencyProperty.Register("AllowInvalidValues", typeof(bool), typeof(TokenTextBox), new PropertyMetadata(false, OnAllowInvalidValuesValueChanged));
			AllowDuplicateValuesProperty = DependencyProperty.Register("AllowDuplicateValues", typeof(bool), typeof(TokenTextBox), new PropertyMetadata(true, OnAllowDuplicateValuesValueChanged));
			IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(TokenTextBox), new PropertyMetadata(true));
			SearchMemberPathsProperty = DependencyProperty.Register("SearchMemberPaths", typeof(string), typeof(TokenTextBox), new PropertyMetadata(null));
            ///AY 2019-7-4 12:04:51
            SelectionBrushProperty = DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(TokenTextBox), new PropertyMetadata(null));

            InvalidValueEnteredEvent = EventManager.RegisterRoutedEvent("InvalidValueEntered", RoutingStrategy.Bubble, typeof(EventHandler), typeof(TokenTextBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TokenTextBox), new FrameworkPropertyMetadata(typeof(TokenTextBox)));
		}

		/// <summary>Initializes a new instance of the TokenTextBox class.</summary>
		public TokenTextBox()
		{
	
			SetCurrentValue(FilteredItemsProperty, new List<object>());
			base.Loaded += TokenTextBox_Loaded;
			((INotifyCollectionChanged)base.Items).CollectionChanged += TokenTextBox_CollectionChanged;
			_groupedFilters = new GroupedFilters();
		}

		protected override void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.OnSelectedItemsCollectionChanged(sender, e);
			HandleSelectedItemCollectionChanged(e);
		}

		protected override void OnSelectedItemsOverrideChanged(IList oldValue, IList newValue)
		{
			base.OnSelectedItemsOverrideChanged(oldValue, newValue);
			if (base.IsInitialized)
			{
				HandleSelectedItemCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			CollectionView collectionView = newValue as CollectionView;
			if (collectionView != null)
			{
				_collectionViewSource.Source = collectionView.SourceCollection;
				foreach (SortDescription sortDescription in collectionView.SortDescriptions)
				{
					_collectionViewSource.SortDescriptions.Add(sortDescription);
				}
				foreach (GroupDescription groupDescription in collectionView.GroupDescriptions)
				{
					_collectionViewSource.GroupDescriptions.Add(groupDescription);
				}
			}
			else
			{
				_collectionViewSource.Source = newValue;
			}
			_TokenTextBoxView = _collectionViewSource.View;
		}

		private void HandleSelectedItemCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (_updateSelectionFlag <= 0)
			{
				List<SegmentInfo> list = null;
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					list = ((RichContent != null) ? ConvertRichContentToSegments(RichContent) : new List<SegmentInfo>());
					foreach (object newItem in e.NewItems)
					{
						list.Add(new TokenSegmentInfo(newItem));
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					list = ((RichContent != null) ? ConvertRichContentToSegments(RichContent) : new List<SegmentInfo>());
					foreach (object oldItem in e.OldItems)
					{
						List<SegmentInfo> source = list;
						Func<SegmentInfo, bool> predicate = delegate(SegmentInfo s)
						{
							if (!s.IsTextSegment)
							{
								return s.TokenItem == oldItem;
							}
							return false;
						};
						SegmentInfo segmentInfo = source.FirstOrDefault(predicate);
						if (segmentInfo != null)
						{
							list.Remove(segmentInfo);
						}
					}
					break;
				case NotifyCollectionChangedAction.Replace:
					list = ((RichContent != null) ? ConvertRichContentToSegments(RichContent) : new List<SegmentInfo>());
					if (e.NewItems != null && e.NewItems.Count == 1 && e.OldItems != null && e.OldItems.Count == 1)
					{
						int num = list.FindIndex(delegate(SegmentInfo s)
						{
							if (!s.IsTextSegment)
							{
								return s.TokenItem == e.OldItems[0];
							}
							return false;
						});
						if (num >= 0)
						{
							object tokenValue = e.NewItems[0];
							list[num] = new TokenSegmentInfo(tokenValue);
						}
					}
					break;
				default:
					list = (from object item in base.SelectedItems
					select new TokenSegmentInfo(item)).ToList<SegmentInfo>();
					break;
				}
				TrimSegments(list);
				UpdateFromSegments(list);
			}
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TokenTextBoxItem();
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GenericAutomationPeer(this);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_richTextBox != null)
			{
				UnInitializeRichTextBox();
			}
			_richTextBox = (GetTemplateChild("PART_RichTextBox") as System.Windows.Controls.RichTextBox);
			if (_richTextBox != null)
			{
				InitializeRichTextBox();
			}
			if (_dropDownPopup != null)
			{
				_dropDownPopup.RemoveHandler(TokenTextBoxItem.RequestHighlightEvent, new RoutedEventHandler(OnTokenziedTextBoxItemRequestHighlight));
				_dropDownPopup.RemoveHandler(TokenTextBoxItem.RequestSelectionEvent, new RoutedEventHandler(OnTokenziedTextBoxItemRequestSelection));
			}
			_dropDownPopup = (GetTemplateChild("PART_Popup") as Popup);
			if (_dropDownPopup != null)
			{
				_dropDownPopup.AddHandler(TokenTextBoxItem.RequestHighlightEvent, new RoutedEventHandler(OnTokenziedTextBoxItemRequestHighlight));
				_dropDownPopup.AddHandler(TokenTextBoxItem.RequestSelectionEvent, new RoutedEventHandler(OnTokenziedTextBoxItemRequestSelection));
			}
			_coreItemsControl = (GetTemplateChild("PART_CoreItemsControl") as TokenTextBoxCoreItemsControl);
		}

		private void TokenTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			if (IsDropDownOpen)
			{
				SetCaretText(Text);
			}
			else
			{
				_caretText = Text;
			}
		}

		private void _richTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (IsDropDownOpen)
			{
				if (e.Key == Key.Down)
				{
					MoveHighlightedContainer(true, false);
					e.Handled = true;
				}
				else if (e.Key == Key.Up)
				{
					MoveHighlightedContainer(false, false);
					e.Handled = true;
				}
				if (e.Key == Key.Next)
				{
					MoveHighlightedContainer(true, true);
					e.Handled = true;
				}
				else if (e.Key == Key.Prior)
				{
					MoveHighlightedContainer(false, true);
					e.Handled = true;
				}
				else if (e.Key == Key.Tab || e.Key == Key.Return)
				{
					e.Handled = ApplyHighlightedItem();
				}
				else if (e.Key == Key.Escape)
				{
					SetIsDropDownOpenInternal(false);
					e.Handled = true;
				}
			}
		}

		private void _richTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (_richTextBox != null)
			{
				SetIsDropDownOpenInternal(false);
				List<SegmentInfo> segments = ConvertRichContentToSegments(RichContent);
				TrimSegments(segments);
				ResolveTextSegments(segments);
				UpdateFromSegments(segments);
			}
		}

		private void _richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (!_ignoreNextSelectionChanged)
			{
				SetIsDropDownOpenInternal(false);
			}
			_ignoreNextSelectionChanged = false;
		}

		private void _richTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (_updateRichTextBoxFlag <= 0 && _richTextBox != null)
			{
				_ignoreNextSelectionChanged = true;
				int caretSegmentIndex;
				List<SegmentInfo> list = ConvertRichContentToSegments(RichContent, _richTextBox.CaretPosition, out caretSegmentIndex);
				SetTextInternal(ConvertSegmentsToText(list));
				SetSelectedItems(list);
				UpdateIsValid(list);
				if (caretSegmentIndex >= 0)
				{
					SegmentInfo segmentInfo = list[caretSegmentIndex];
					SetCaretText(segmentInfo.IsTextSegment ? segmentInfo.Text : null);
				}
				else
				{
					SetCaretText(null);
				}
			}
		}

		private static void CannotExecuteCommand(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = false;
			args.Handled = true;
		}

		private static void OnCanExecuteCutCopyPaste(object sender, CanExecuteRoutedEventArgs args)
		{
			System.Windows.Controls.RichTextBox richTextBox = (System.Windows.Controls.RichTextBox)sender;
			TokenTextBox TokenTextBox = richTextBox.TemplatedParent as TokenTextBox;
			if (TokenTextBox != null && object.ReferenceEquals(TokenTextBox._richTextBox, richTextBox))
			{
				if (args.Command == ApplicationCommands.Copy)
				{
					TokenTextBox.OnCanExecuteCopy(args);
				}
				else if (args.Command == ApplicationCommands.Paste)
				{
					TokenTextBox.OnCanExecutePaste(args);
				}
				else if (args.Command == ApplicationCommands.Cut)
				{
					TokenTextBox.OnCanExecuteCut(args);
				}
			}
		}

		private static void OnExecuteCutCopyPaste(object sender, ExecutedRoutedEventArgs args)
		{
			System.Windows.Controls.RichTextBox richTextBox = (System.Windows.Controls.RichTextBox)sender;
			TokenTextBox TokenTextBox = richTextBox.TemplatedParent as TokenTextBox;
			if (TokenTextBox != null && object.ReferenceEquals(TokenTextBox._richTextBox, richTextBox))
			{
				if (args.Command == ApplicationCommands.Copy)
				{
					TokenTextBox.OnExecuteCopy(args);
				}
				else if (args.Command == ApplicationCommands.Paste)
				{
					TokenTextBox.OnExecutePaste(args);
				}
				else if (args.Command == ApplicationCommands.Cut)
				{
					TokenTextBox.OnExecuteCut(args);
				}
			}
		}

		private void OnCanExecuteCut(CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (_richTextBox != null && _richTextBox.IsEnabled && !_richTextBox.Selection.IsEmpty && !_richTextBox.IsReadOnly);
			args.Handled = true;
		}

		private void OnCanExecuteCopy(CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (_richTextBox != null && _richTextBox.IsEnabled && !_richTextBox.Selection.IsEmpty);
			args.Handled = true;
		}

		private void OnCanExecutePaste(CanExecuteRoutedEventArgs args)
		{
			string text = Clipboard.GetText();
			args.CanExecute = (_richTextBox != null && _richTextBox.IsEnabled && !_richTextBox.IsReadOnly && text != null);
			args.Handled = true;
		}

		private void OnExecuteCut(ExecutedRoutedEventArgs args)
		{
			string text = SerializeRichTextBoxSelection();
			if (_richTextBox != null)
			{
				_richTextBox.Selection.Text = string.Empty;
			}
			Clipboard.SetText(text);
			args.Handled = true;
		}

		private void OnExecuteCopy(ExecutedRoutedEventArgs args)
		{
			string text = SerializeRichTextBoxSelection();
            try
            {

			}
			catch (Exception eex)
            {


            }
		
		}

		private void OnExecutePaste(ExecutedRoutedEventArgs args)
		{
			string text = Clipboard.GetText();
			if (string.IsNullOrEmpty(text))
			{
				args.Handled = true;
			}
			else if (_richTextBox != null)
			{
				if (!_richTextBox.Selection.IsEmpty)
				{
					_richTextBox.Selection.Text = "";
				}
				TextPointer textPointer = _richTextBox.CaretPosition;
				if (textPointer != null && !textPointer.IsAtInsertionPosition)
				{
					textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
					if (!textPointer.IsAtInsertionPosition)
					{
						textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
					}
				}
				if (textPointer == null || !textPointer.IsAtInsertionPosition)
				{
					textPointer = _richTextBox.Document.ContentEnd;
				}
				if (textPointer.Parent is Run)
				{
					Run run = (Run)textPointer.Parent;
					int offsetToPosition = run.ContentStart.GetOffsetToPosition(textPointer);
					textPointer.InsertTextInRun(text);
					textPointer = run.ContentStart.GetPositionAtOffset(text.Length + offsetToPosition);
					_richTextBox.CaretPosition = textPointer;
				}
				else
				{
					Paragraph paragraph = textPointer.Parent as Paragraph;
					bool flag = paragraph != null && paragraph.ContentEnd.CompareTo(textPointer) == 0;
					textPointer.InsertTextInRun(text);
					if (flag)
					{
						_richTextBox.CaretPosition = paragraph.ContentEnd;
					}
				}
				args.Handled = true;
			}
		}

		private void OnTokenziedTextBoxItemRequestSelection(object sender, RoutedEventArgs args)
		{
			TokenTextBoxItem TokenTextBoxItem = args.OriginalSource as TokenTextBoxItem;
			if (TokenTextBoxItem != null)
			{
				object obj = GetCoreItemsControl().ItemContainerGenerator.ItemFromContainer(TokenTextBoxItem);
				if (obj == HighlightedItem)
				{
					ApplyHighlightedItem();
				}
			}
		}

		private void OnTokenziedTextBoxItemRequestHighlight(object sender, RoutedEventArgs args)
		{
			TokenTextBoxItem TokenTextBoxItem = args.OriginalSource as TokenTextBoxItem;
			if (TokenTextBoxItem != null)
			{
				object obj = GetCoreItemsControl().ItemContainerGenerator.ItemFromContainer(TokenTextBoxItem);
				if (obj != null)
				{
					if (HighlightedItem != null)
					{
						TokenTextBoxItem TokenTextBoxItem2 = GetCoreItemsControl().ItemContainerGenerator.ContainerFromItem(HighlightedItem) as TokenTextBoxItem;
						if (TokenTextBoxItem2 != null)
						{
							TokenTextBoxItem2.SetIsHighlighted(false);
						}
					}
					HighlightedItem = obj;
					TokenTextBoxItem.SetIsHighlighted(true);
				}
			}
		}

		private void TokenTextBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (base.IsInitialized && base.ItemsSource == null)
			{
				_TokenTextBoxView = CollectionViewSource.GetDefaultView(base.Items);
			}
		}

		protected virtual object OnQueryItemFromText(string text)
		{
			if (this.QueryItemFromText != null)
			{
				QueryItemFromTextEventArgs queryItemFromTextEventArgs = new QueryItemFromTextEventArgs(text);
				this.QueryItemFromText(this, queryItemFromTextEventArgs);
				return queryItemFromTextEventArgs.Item;
			}
			return null;
		}

		protected virtual string OnQueryTextFromItem(object item)
		{
			if (this.QueryTextFromItem != null)
			{
				QueryTextFromItemEventArgs queryTextFromItemEventArgs = new QueryTextFromItemEventArgs(item);
				this.QueryTextFromItem(this, queryTextFromItemEventArgs);
				return queryTextFromItemEventArgs.Text;
			}
			return null;
		}

		protected virtual bool? OnQuerySuggestItemForText(object item, string text)
		{
			if (text == null)
			{
				return false;
			}
			if (this.QuerySuggestItemForText != null)
			{
				QuerySuggestItemForTextEventArgs querySuggestItemForTextEventArgs = new QuerySuggestItemForTextEventArgs(item, text);
				this.QuerySuggestItemForText(this, querySuggestItemForTextEventArgs);
				return querySuggestItemForTextEventArgs.SuggestItem;
			}
			return null;
		}

		private string ConvertSegmentsToText(List<SegmentInfo> segments)
		{
			if (segments == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SegmentInfo segment in segments)
			{
				string text = segment.IsTextSegment ? segment.Text : GetTextFromItem(segment.TokenItem);
				if (text != null)
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		private Paragraph ConvertSegmentsToRichText(List<SegmentInfo> segments)
		{
			if (segments == null)
			{
				return null;
			}
			Paragraph paragraph = new Paragraph();
			foreach (SegmentInfo segment in segments)
			{
				if (segment.IsTextSegment)
				{
					paragraph.Inlines.Add(new Run(segment.Text));
				}
				else
				{
					InlineUIContainer inlineUIContainer = new InlineUIContainer();
					TokenItem tokenItem = new TokenItem();
					tokenItem.UserItem = segment.TokenItem;
					tokenItem.DataContext = segment.TokenItem;
					tokenItem.Content = segment.TokenItem;
					tokenItem.SetIsValidInternal(FilteredItems.Contains(segment.TokenItem));
					if (TokenItemContainerStyle != null)
					{
						tokenItem.Style = TokenItemContainerStyle;
					}
					DataTemplate dataTemplate = TokenItemTemplate ?? base.ItemTemplate;
					if (dataTemplate != null)
					{
						tokenItem.ContentTemplate = dataTemplate;
					}
					else if (tokenItem.ContentTemplate == null)
					{
						tokenItem.SetResourceReference(ContentControl.ContentTemplateProperty, DefaultTokenItemTemplateKey);
						tokenItem.Content = GetDisplayMemberValue(segment.TokenItem);
					}
					inlineUIContainer.Child = tokenItem;
					inlineUIContainer.BaselineAlignment = BaselineAlignment.Bottom;
					paragraph.Inlines.Add(inlineUIContainer);
				}
			}
			return paragraph;
		}

		private List<SegmentInfo> ConvertTextToSegments(string text)
		{
			if (text == null)
			{
				return null;
			}
			return (from s in SplitText(text)
			where s.Length > 0
			select new TextSegmentInfo(s)).ToList<SegmentInfo>();
		}

		private List<SegmentInfo> ConvertRichContentToSegments(Paragraph document)
		{
			int caretSegmentIndex;
			return ConvertRichContentToSegments(document, null, out caretSegmentIndex);
		}

		private List<SegmentInfo> ConvertRichContentToSegments(Paragraph document, TextPointer caretPosition, out int caretSegmentIndex)
		{
			caretSegmentIndex = -1;
			if (document == null)
			{
				return null;
			}
			List<SegmentInfo> segments = new List<SegmentInfo>();
			Run run = (caretPosition != null) ? (caretPosition.Parent as Run) : null;
			int caretIndex = -1;
			int caretTextOffset = -1;
			StringBuilder text = new StringBuilder();
			Action action = delegate
			{
				int caretStringIndex;
				string[] array = SplitText(text.ToString(), caretTextOffset, out caretStringIndex);
				if (caretStringIndex >= 0)
				{
					caretIndex = segments.Count + caretStringIndex;
				}
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					segments.Add(new TextSegmentInfo(text2));
				}
				caretTextOffset = -1;
				text = new StringBuilder();
			};
			foreach (Inline inline in document.Inlines)
			{
				Run run2 = inline as Run;
				InlineUIContainer inlineUIContainer = inline as InlineUIContainer;
				if (run2 != null)
				{
					if (run2 == run)
					{
						caretTextOffset = text.Length + run2.ContentStart.GetOffsetToPosition(caretPosition);
					}
					text.Append(run2.Text);
				}
				else if (inlineUIContainer != null)
				{
					action();
					TokenItem tokenItem = inlineUIContainer.Child as TokenItem;
					if (tokenItem != null)
					{
						segments.Add(new TokenSegmentInfo(tokenItem.UserItem));
					}
				}
			}
			action();
			caretSegmentIndex = caretIndex;
			return segments;
		}

		private void TrimSegments(List<SegmentInfo> segments)
		{
			if (segments != null)
			{
				for (int i = 0; i < segments.Count; i++)
				{
					SegmentInfo segmentInfo = segments[i];
					if (segmentInfo.IsTextSegment)
					{
						string text = TrimText(segmentInfo.Text);
						if (text.Length == 0)
						{
							segments.RemoveAt(i);
							i--;
						}
						else if (text != segmentInfo.Text)
						{
							segments[i] = new TextSegmentInfo(text);
						}
					}
				}
				int num;
				for (num = 0; num < segments.Count; num++)
				{
					segments.Insert(num + 1, new TextSegmentInfo(TokenDelimiter.ToString() + " "));
					num++;
				}
			}
		}

		private void ResolveTextSegments(List<SegmentInfo> segments)
		{
			if (segments != null)
			{
				HashSet<object> hashSet = null;
				if (!AllowDuplicateValues)
				{
					hashSet = new HashSet<object>(from s in segments
					where !s.IsTextSegment
					select s.TokenItem);
				}
				for (int i = 0; i < segments.Count; i++)
				{
					SegmentInfo segmentInfo = segments[i];
					if (segmentInfo.IsTextSegment)
					{
						string text = TrimText(segmentInfo.Text);
						if (!string.IsNullOrEmpty(text))
						{
							object itemFromText = GetItemFromText(text);
							object obj = null;
							if (itemFromText != null)
							{
								obj = itemFromText;
							}
							else if (AllowInvalidValues)
							{
								obj = text;			
							}
                            RaiseEvent(new InvalidValueEventArgs(InvalidValueEnteredEvent, this, text));
                            if (obj != null)
							{
								if (!AllowDuplicateValues)
								{
									if (hashSet.Contains(obj))
									{
										obj = null;
									}
									else
									{
										hashSet.Add(obj);
									}
								}
								if (obj != null)
								{
									segments[i] = new TokenSegmentInfo(obj);
									int num = segmentInfo.Text.IndexOf(text);
									string text2 = segmentInfo.Text.Substring(0, num);
									string text3 = segmentInfo.Text.Substring(num + text.Length);
									if (text2.Length > 0)
									{
										segments.Insert(i, new TextSegmentInfo(text2));
										i++;
									}
									if (text3.Length > 0)
									{
										segments.Insert(i + 1, new TextSegmentInfo(text3));
										i++;
									}
								}
							}
						}
					}
				}
			}
		}

		private string[] SplitText(string text, int caretTextOffset, out int caretStringIndex)
		{
			string[] array = SplitText(text);
			caretStringIndex = -1;
			if (caretTextOffset >= 0)
			{
				int num = 0;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					caretStringIndex++;
					num += text2.Length;
					if (num >= caretTextOffset)
					{
						break;
					}
				}
			}
			return array;
		}

		private string[] SplitText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			string[] array = text.Split(TokenDelimiter);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((i == 0) ? array[i] : (TokenDelimiter.ToString() + array[i]));
			}
			return array;
		}

		private string TrimText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			string a;
			do
			{
				a = text;
				text = text.Trim(TokenDelimiter);
				text = text.Trim();
			}
			while (a != text);
			return text;
		}

		private void UpdateFromSegments(List<SegmentInfo> segments)
		{
			string textInternal = ConvertSegmentsToText(segments);
			Paragraph richContent = ConvertSegmentsToRichText(segments);
			SetRichContent(richContent);
			SetTextInternal(textInternal);
			SetSelectedItems(segments);
			UpdateIsValid(segments);
		}

		private void SetRichContent(Paragraph value)
		{
			if (RichContent != value)
			{
				_updateRichTextBoxFlag++;
				try
				{
					SetCaretText(null);
					if (_richTextBox != null)
					{
						_richTextBox.Document = ((value != null) ? new FlowDocument(value) : new FlowDocument());
						_richTextBox.CaretPosition = _richTextBox.Document.ContentEnd;
					}
					else
					{
						_richContent = value;
					}
				}
				finally
				{
					_updateRichTextBoxFlag--;
				}
			}
		}

		private void SetTextInternal(string text)
		{
			_updateTextFlag++;
			try
			{
				SetCurrentValue(TextProperty, text);
			}
			finally
			{
				_updateTextFlag--;
			}
		}

		private void SetSelectedItems(List<SegmentInfo> segments)
		{
			_updateSelectionFlag++;
			try
			{
				List<object> list = (segments != null) ? (from s in segments
				where !s.IsTextSegment
				select s.TokenItem).ToList() : new List<object>(0);
				if (list.Count > base.SelectedItems.Count)
				{
					IEnumerable<object> enumerable = list.Except(base.SelectedItems as IEnumerable<object>);
					foreach (object item in enumerable)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(SDK.CONTROLLIB.Primitive.Selector.ItemSelectionChangedEvent, this, item, true));
					}
				}
				else if (list.Count < base.SelectedItems.Count)
				{
					IEnumerable<object> enumerable2 = ((IEnumerable<object>)base.SelectedItems).Except(list);
					foreach (object item2 in enumerable2)
					{
						OnItemSelectionChanged(new ItemSelectionChangedEventArgs(SDK.CONTROLLIB.Primitive.Selector.ItemSelectionChangedEvent, this, item2, false));
					}
				}
				UpdateSelectedItems(list);
			}
			finally
			{
				_updateSelectionFlag--;
			}
		}

		private void SetCaretText(string text)
		{
			if (text != null)
			{
				text = TrimText(text);
			}
			_caretText = text;
			bool flag = UpdateDropDownContent();
			SetIsDropDownOpenInternal(flag);
			if (flag)
			{
				UpdateDropDownPosition();
				BringHiglightedItemIntoView();
			}
		}

		private object GetItemFromText(string text)
		{
			if (text == null)
			{
				return null;
			}
			object obj = OnQueryItemFromText(text);
			if (obj == null)
			{
				foreach (object filteredItem in FilteredItems)
				{
					if (filteredItem != null)
					{
						string textFromItem = GetTextFromItem(filteredItem);
						if (string.Equals(textFromItem, text, StringComparison.InvariantCultureIgnoreCase))
						{
							return filteredItem;
						}
					}
				}
				return obj;
			}
			return obj;
		}

		private bool GetSuggestItemForText(object item, string text)
		{
			if (text == null)
			{
				return false;
			}
			bool? flag = OnQuerySuggestItemForText(item, text);
			if (!flag.HasValue)
			{
				if (!AllowDuplicateValues && base.SelectedItems.Contains(item))
				{
					return false;
				}
				text = text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					return false;
				}
				if (base.DisplayMemberPath != null && GetPathValue(item, base.DisplayMemberPath) == null)
				{
					return false;
				}
				string[] itemSearchValuesText = GetItemSearchValuesText(item);
				flag = (from sv in itemSearchValuesText
				where sv != null
				select sv).Any((string sv) => sv.ToUpper().Contains(text.ToUpper()));
			}
			return flag.GetValueOrDefault();
		}

		private string[] GetItemSearchValuesText(object item)
		{
			string[] searchMemberPaths = GetSearchMemberPaths();
			if (searchMemberPaths == null)
			{
				return new string[1]
				{
					GetTextFromItem(item)
				};
			}
			string[] array = new string[searchMemberPaths.Length];
			if (item != null)
			{
				for (int i = 0; i < searchMemberPaths.Length; i++)
				{
					array[i] = (GetPathValue(item, searchMemberPaths[i]) as string);
				}
			}
			return array;
		}

		private string[] GetSearchMemberPaths()
		{
			string searchMemberPaths = SearchMemberPaths;
			if (searchMemberPaths == null)
			{
				return null;
			}
			return (from p in searchMemberPaths.Split(',')
			select p.Trim()).ToArray();
		}

		private string GetTextFromItem(object item)
		{
			string text = OnQueryTextFromItem(item);
			if (text == null)
			{
				text = (GetDisplayMemberValue(item) as string);
			}
			return text;
		}

		private object GetDisplayMemberValue(object item)
		{
			if (item == null)
			{
				return null;
			}
			string propertyPath = TokenDisplayMemberPath ?? base.DisplayMemberPath;
			object obj = GetPathValue(item, propertyPath);
			if (obj == null)
			{
				obj = (item as string);
			}
			return obj;
		}

		private void UpdateIsValid(List<SegmentInfo> segments)
		{
			bool isValidInternal = true;
			if (segments != null)
			{
				foreach (SegmentInfo segment in segments)
				{
					if (segment.IsTextSegment && TrimText(segment.Text).Length > 0)
					{
						isValidInternal = false;
						break;
					}
				}
			}
			SetIsValidInternal(isValidInternal);
		}

		private bool FilterItemsMethod(object obj)
		{
			return GetSuggestItemForText(obj, _caretText);
		}

		private bool UpdateDropDownContent()
		{
			bool result = false;
			HighlightedItem = null;
			if (_TokenTextBoxView != null && _TokenTextBoxView.CanFilter)
			{
				_groupedFilters.RemoveInternalFilters();
				if (base.Items.Filter != null && !base.Items.Filter.Equals(_groupedFilters.ActiveFilter))
				{
					_groupedFilters.RemoveAllFilters();
					_groupedFilters.AddFilter(base.Items.Filter, false);
				}
				_groupedFilters.AddFilter(FilterItemsMethod, true);
				_TokenTextBoxView.Filter = _groupedFilters.ActiveFilter;
				FilteredItems = _TokenTextBoxView.Cast<object>().ToList();
				result = (FilteredItems.Count > 0);
				foreach (object filteredItem in FilteredItems)
				{
					if (HighlightedItem == null)
					{
						HighlightedItem = filteredItem;
					}
					UIElement uIElement = GetCoreItemsControl().ItemContainerGenerator.ContainerFromItem(filteredItem) as UIElement;
					TokenTextBoxItem TokenTextBoxItem = uIElement as TokenTextBoxItem;
					if (TokenTextBoxItem != null)
					{
						TokenTextBoxItem.SetIsHighlighted(HighlightedItem == filteredItem);
					}
				}
				ScrollViewer scrollViewer = TreeHelper.FindChild<ScrollViewer>(GetCoreItemsControl());
				if (scrollViewer != null)
				{
					scrollViewer.ScrollToVerticalOffset(0.0);
				}
			}
			return result;
		}

		private void UpdateDropDownPosition()
		{
			if (_dropDownPopup != null && _richTextBox != null)
			{
				double num = 0.0;
				if (_richTextBox.CaretPosition != null)
				{
					Rect characterRect = _richTextBox.CaretPosition.GetCharacterRect(LogicalDirection.Backward);
					if (!characterRect.IsEmpty)
					{
						num += characterRect.Left;
					}
				}
				_dropDownPopup.HorizontalOffset = num;
			}
		}

		private bool ApplyHighlightedItem()
		{
			if (IsDropDownOpen && HighlightedItem != null && _richTextBox != null)
			{
				int caretSegmentIndex;
				List<SegmentInfo> list = ConvertRichContentToSegments(RichContent, _richTextBox.CaretPosition, out caretSegmentIndex);
				if (caretSegmentIndex >= 0)
				{
					list[caretSegmentIndex] = new TokenSegmentInfo(HighlightedItem);
				}
				TrimSegments(list);
				UpdateFromSegments(list);
				SetIsDropDownOpenInternal(false);
				return true;
			}
			return false;
		}

		private void MoveHighlightedContainer(bool next, bool page)
		{
			if (FilteredItems.Count != 0)
			{
				TokenTextBoxItem TokenTextBoxItem = GetCoreItemsControl().ItemContainerGenerator.ContainerFromItem(HighlightedItem) as TokenTextBoxItem;
				if (TokenTextBoxItem != null)
				{
					TokenTextBoxItem.SetIsHighlighted(false);
				}
				ScrollViewer scrollViewer = TreeHelper.FindChild<ScrollViewer>(GetCoreItemsControl());
				int num = Math.Max(0, FilteredItems.IndexOf(HighlightedItem));
				int num2 = 0;
				if (scrollViewer != null)
				{
					if (scrollViewer.CanContentScroll)
					{
						num2 = ((!page) ? 1 : Math.Max(1, Convert.ToInt32(Math.Floor(scrollViewer.ViewportHeight - 1.0))));
					}
					else if (page)
					{
						FrameworkElement frameworkElement = GetCoreItemsControl().ItemContainerGenerator.ContainerFromIndex(num) as FrameworkElement;
						int num3 = Convert.ToInt32(Math.Floor(scrollViewer.ViewportHeight / frameworkElement.ActualHeight));
						int num4 = 0;
						int num5 = num;
						int num6 = num5;
						int num7 = 0;
						while (num6 < FilteredItems.Count && num6 >= 0)
						{
							UIElement uIElement = GetCoreItemsControl().ItemContainerGenerator.ContainerFromIndex(num6) as UIElement;
							if (uIElement.Visibility == Visibility.Visible)
							{
								num7 = num6;
								if (++num4 == num3)
								{
									break;
								}
							}
							num6 = (next ? (num6 + 1) : (num6 - 1));
						}
						num6 = Math.Max(0, Math.Min(num6, FilteredItems.Count - 1));
						UIElement uIElement2 = GetCoreItemsControl().ItemContainerGenerator.ContainerFromIndex(num6) as UIElement;
						if (uIElement2.Visibility == Visibility.Collapsed)
						{
							num6 = num7;
						}
						num2 = Math.Max(1, Math.Abs(num6 - num5));
					}
					else
					{
						num2 = 1;
					}
				}
				num += (next ? num2 : (-num2));
				for (num = Math.Max(0, Math.Min(num, FilteredItems.Count - 1)); num < FilteredItems.Count && num >= 0; num += (next ? 1 : (-1)))
				{
					object obj = FilteredItems[num];
					UIElement uIElement3 = GetCoreItemsControl().ItemContainerGenerator.ContainerFromIndex(num) as UIElement;
					if ((uIElement3 != null) ? (uIElement3.Visibility == Visibility.Visible) : GetSuggestItemForText(obj, _caretText))
					{
						HighlightedItem = obj;
						if (uIElement3 == null && scrollViewer.CanContentScroll)
						{
							scrollViewer.ScrollToVerticalOffset((double)(next ? (num - num2) : num));
						}
						TokenTextBoxItem = (uIElement3 as TokenTextBoxItem);
						break;
					}
				}
				if (TokenTextBoxItem != null)
				{
					TokenTextBoxItem.SetIsHighlighted(true);
					TokenTextBoxItem.BringIntoView();
				}
			}
		}

		private void BringHiglightedItemIntoView()
		{
			FrameworkElement frameworkElement = GetCoreItemsControl().ItemContainerGenerator.ContainerFromItem(HighlightedItem) as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
			}
		}

		private string SerializeRichTextBoxSelection()
		{
			if (_richTextBox == null || _richTextBox.Selection.IsEmpty)
			{
				return string.Empty;
			}
			TextPointer start = _richTextBox.Selection.Start;
			TextPointer end = _richTextBox.Selection.End;
			if (start.CompareTo(end) >= 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			TextPointer textPointer = (start.Parent is Run) ? start : null;
			TextPointer nextContextPosition = start.GetNextContextPosition(LogicalDirection.Forward);
			while (nextContextPosition != null && nextContextPosition.CompareTo(end) <= 0)
			{
				if (nextContextPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
				{
					Run run = nextContextPosition.Parent as Run;
					InlineUIContainer inlineUIContainer = nextContextPosition.Parent as InlineUIContainer;
					if (run != null)
					{
						textPointer = (textPointer ?? run.ContentStart);
						stringBuilder.Append(new TextRange(textPointer, run.ContentEnd).Text);
					}
					else if (inlineUIContainer != null)
					{
						TokenItem tokenItem = inlineUIContainer.Child as TokenItem;
						if (tokenItem != null)
						{
							stringBuilder.Append(GetTextFromItem(tokenItem.UserItem) ?? string.Empty);
						}
					}
				}
				textPointer = null;
				nextContextPosition = nextContextPosition.GetNextContextPosition(LogicalDirection.Forward);
			}
			if (nextContextPosition != null && nextContextPosition.CompareTo(end) > 0)
			{
				Run run2 = end.Parent as Run;
				if (run2 != null)
				{
					textPointer = (textPointer ?? run2.ContentStart);
					stringBuilder.Append(new TextRange(textPointer, end).Text);
				}
			}
			return stringBuilder.ToString();
		}

		private TokenTextBoxCoreItemsControl GetCoreItemsControl()
		{
			if (_coreItemsControl == null)
			{
				ApplyTemplate();
			}
			if (_coreItemsControl == null)
			{
				throw new NotSupportedException("PART_CoreItemsControl can't be found.");
			}
			return _coreItemsControl;
		}

		private void InitializeRichTextBox()
		{
			if (_richTextBox != null)
			{
				_richTextBox.IsDocumentEnabled = true;
				_richTextBox.CommandBindings.Clear();
				_richTextBox.IsUndoEnabled = false;
                _richTextBox.AllowDrop = false;
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.AlignCenter, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.AlignJustify, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.AlignLeft, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.AlignRight, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.CorrectSpellingError, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.DecreaseFontSize, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.DecreaseIndentation, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.EnterLineBreak, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.EnterParagraphBreak, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.IgnoreSpellingError, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.IncreaseFontSize, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.IncreaseIndentation, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleBold, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleBullets, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleItalic, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleNumbering, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleSubscript, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleSuperscript, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleUnderline, null, CannotExecuteCommand));
				_richTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnExecuteCutCopyPaste, OnCanExecuteCutCopyPaste));
				_richTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnExecuteCutCopyPaste, OnCanExecuteCutCopyPaste));
				_richTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnExecuteCutCopyPaste, OnCanExecuteCutCopyPaste));
				_richTextBox.Document = ((_richContent != null) ? new FlowDocument(_richContent) : new FlowDocument());
				_richContent = null;
				_richTextBox.TextChanged += _richTextBox_TextChanged;
				_richTextBox.SelectionChanged += _richTextBox_SelectionChanged;
				_richTextBox.PreviewKeyDown += _richTextBox_PreviewKeyDown;
				_richTextBox.LostFocus += _richTextBox_LostFocus;
			}
		}

		private void UnInitializeRichTextBox()
		{
			if (_richTextBox != null)
			{
				_richTextBox.TextChanged -= _richTextBox_TextChanged;
				_richTextBox.SelectionChanged -= _richTextBox_SelectionChanged;
				_richTextBox.PreviewKeyDown -= _richTextBox_PreviewKeyDown;
				_richTextBox.LostFocus -= _richTextBox_LostFocus;
				_richContent = ((_richTextBox.Document.Blocks.Count > 0) ? (_richTextBox.Document.Blocks.FirstBlock as Paragraph) : null);
				_richTextBox.CommandBindings.Clear();
			}
		}
	}
}
