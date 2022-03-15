using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>
	///   <para>Represents an editor that allows a user to pick a color from a predefind color palettes.</para>
	/// </summary>
	[TemplatePart(Name = "PART_RecentColors", Type = typeof(ListBox))]
	[TemplatePart(Name = "PART_StandardColors", Type = typeof(ListBox))]
	[TemplatePart(Name = "PART_ColorPickerPalettePopup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_AvailableColors", Type = typeof(ListBox))]
	[TemplatePart(Name = "PART_ColorPickerToggleButton", Type = typeof(ToggleButton))]
	public class ColorPicker : Control
	{
		private const string PART_AvailableColors = "PART_AvailableColors";

		private const string PART_StandardColors = "PART_StandardColors";

		private const string PART_RecentColors = "PART_RecentColors";

		private const string PART_ColorPickerToggleButton = "PART_ColorPickerToggleButton";

		private const string PART_ColorPickerPalettePopup = "PART_ColorPickerPalettePopup";

		private ListBox _availableColors;

		private ListBox _standardColors;

		private ListBox _recentColors;

		private ToggleButton _toggleButton;

		private Popup _popup;

		private Color? _initialColor;

		private bool _selectionChanged;

		/// <summary>Identifies the AdvancedTabHeader dependency property.</summary>
		public static readonly DependencyProperty AdvancedTabHeaderProperty;

		/// <summary>Identifies the AvailableColors dependency property.</summary>
		public static readonly DependencyProperty AvailableColorsProperty;

		/// <summary>Identifies the AvailableColorsSortingMode dependency property.</summary>
		public static readonly DependencyProperty AvailableColorsSortingModeProperty;

		/// <summary>Identifies the AvailableColorsHeader dependency property.</summary>
		public static readonly DependencyProperty AvailableColorsHeaderProperty;

		/// <summary>Identifies the ButtonStyle dependency property.</summary>
		public static readonly DependencyProperty ButtonStyleProperty;

		/// <summary>Identifies the DisplayColorAndName dependency property.</summary>
		public static readonly DependencyProperty DisplayColorAndNameProperty;

		/// <summary>Identifies the DisplayColorTooltip dependency property.</summary>
		public static readonly DependencyProperty DisplayColorTooltipProperty;

		/// <summary>Identifies the ColorMode dependency property.</summary>
		public static readonly DependencyProperty ColorModeProperty;

		/// <summary>Identifies the DropDownBackground dependency property.</summary>
		public static readonly DependencyProperty DropDownBackgroundProperty;

		/// <summary>Identifies the HeaderBackground dependency property.</summary>
		public static readonly DependencyProperty HeaderBackgroundProperty;

		/// <summary>Identifies the HeaderForeground dependency property.</summary>
		public static readonly DependencyProperty HeaderForegroundProperty;

		/// <summary>Identifies the IsOpen dependency property.</summary>
		public static readonly DependencyProperty IsOpenProperty;

		/// <summary>Identifies the MaxDropDownWidth dependency property.</summary>
		public static readonly DependencyProperty MaxDropDownWidthProperty;

		/// <summary>Identifies the RecentColors dependency property.</summary>
		public static readonly DependencyProperty RecentColorsProperty;

		/// <summary>Identifies the RecentColorsHeader dependency property.</summary>
		public static readonly DependencyProperty RecentColorsHeaderProperty;

		/// <summary>Identifies the SelectedColor dependency property.</summary>
		public static readonly DependencyProperty SelectedColorProperty;

		/// <summary>Identifies the SelectedColorText dependency property.</summary>
		public static readonly DependencyProperty SelectedColorTextProperty;

		/// <summary>Identifies the ShowTabHeaders dependency property.</summary>
		public static readonly DependencyProperty ShowTabHeadersProperty;

		/// <summary>Identifies the ShowAvailableColors dependency property.</summary>
		public static readonly DependencyProperty ShowAvailableColorsProperty;

		/// <summary>Identifies the ShowRecentColors dependency property.</summary>
		public static readonly DependencyProperty ShowRecentColorsProperty;

		/// <summary>Identifies the ShowStandardColors dependency property.</summary>
		public static readonly DependencyProperty ShowStandardColorsProperty;

		/// <summary>Identifies the ShowDropDown dependency property.</summary>
		public static readonly DependencyProperty ShowDropDownButtonProperty;

		/// <summary>Identifies the StandardTabHeader dependency property.</summary>
		public static readonly DependencyProperty StandardTabHeaderProperty;

		/// <summary>Identifies the StandardColors dependency property. .</summary>
		public static readonly DependencyProperty StandardColorsProperty;

		/// <summary>Identifies the StandardColorsHeader dependency property.</summary>
		public static readonly DependencyProperty StandardColorsHeaderProperty;

		/// <summary>Identifies the TabBackground dependency property.</summary>
		public static readonly DependencyProperty TabBackgroundProperty;

		/// <summary>Identifies the TabForeground dependency property.</summary>
		public static readonly DependencyProperty TabForegroundProperty;

		/// <summary>Identifies the UsingAlphaChannel dependency property.</summary>
		public static readonly DependencyProperty UsingAlphaChannelProperty;

		/// <summary>Identifies the SelectedColorChanged routed event.</summary>
		public static readonly RoutedEvent SelectedColorChangedEvent;

		public static readonly RoutedEvent OpenedEvent;

		public static readonly RoutedEvent ClosedEvent;

		/// <summary>Gets or sets the text to use for the "Advanced" tab in the ColorPicker's popup.</summary>
		public string AdvancedTabHeader
		{
			get
			{
				return (string)GetValue(AdvancedTabHeaderProperty);
			}
			set
			{
				SetValue(AdvancedTabHeaderProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets all colors that are available to the user for selection.</para>
		/// </summary>
		public ObservableCollection<ColorItem> AvailableColors
		{
			get
			{
				return (ObservableCollection<ColorItem>)GetValue(AvailableColorsProperty);
			}
			set
			{
				SetValue(AvailableColorsProperty, value);
			}
		}

		public ColorSortingMode AvailableColorsSortingMode
		{
			get
			{
				return (ColorSortingMode)GetValue(AvailableColorsSortingModeProperty);
			}
			set
			{
				SetValue(AvailableColorsSortingModeProperty, value);
			}
		}

		/// <summary>Gets or sets the header text of the Available Colors section in the dropdown.</summary>
		public string AvailableColorsHeader
		{
			get
			{
				return (string)GetValue(AvailableColorsHeaderProperty);
			}
			set
			{
				SetValue(AvailableColorsHeaderProperty, value);
			}
		}

		/// <summary>Gets or sets the dropdown button style.</summary>
		public Style ButtonStyle
		{
			get
			{
				return (Style)GetValue(ButtonStyleProperty);
			}
			set
			{
				SetValue(ButtonStyleProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should display the color, or both the color and the color name, when the color dropdown is closed.</summary>
		public bool DisplayColorAndName
		{
			get
			{
				return (bool)GetValue(DisplayColorAndNameProperty);
			}
			set
			{
				SetValue(DisplayColorAndNameProperty, value);
			}
		}

		/// <summary>Gets or sets if Tooltips will be shown in the popup of the ColorPicker to display the preset color names.</summary>
		public bool DisplayColorTooltip
		{
			get
			{
				return (bool)GetValue(DisplayColorTooltipProperty);
			}
			set
			{
				SetValue(DisplayColorTooltipProperty, value);
			}
		}

		/// <summary>Gets or sets the current display of the ColorPicker (ColorPalette or ColorCanvas). By default, ColorPalette.</summary>
		public ColorMode ColorMode
		{
			get
			{
				return (ColorMode)GetValue(ColorModeProperty);
			}
			set
			{
				SetValue(ColorModeProperty, value);
			}
		}

		/// <summary>Gets or sets the background of the popup.</summary>
		public Brush DropDownBackground
		{
			get
			{
				return (Brush)GetValue(DropDownBackgroundProperty);
			}
			set
			{
				SetValue(DropDownBackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the background of the headers used in the popup.</summary>
		public Brush HeaderBackground
		{
			get
			{
				return (Brush)GetValue(HeaderBackgroundProperty);
			}
			set
			{
				SetValue(HeaderBackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the foreground of the headers used in the popup.</summary>
		public Brush HeaderForeground
		{
			get
			{
				return (Brush)GetValue(HeaderForegroundProperty);
			}
			set
			{
				SetValue(HeaderForegroundProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the color dropdown is open.</summary>
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

		/// <summary>Gets or Sets the maximum width of the ColorPicker popup's content.</summary>
		public double MaxDropDownWidth
		{
			get
			{
				return (double)GetValue(MaxDropDownWidthProperty);
			}
			set
			{
				SetValue(MaxDropDownWidthProperty, value);
			}
		}

		/// <summary>Gets or sets all the recently selected colors.</summary>
		public ObservableCollection<ColorItem> RecentColors
		{
			get
			{
				return (ObservableCollection<ColorItem>)GetValue(RecentColorsProperty);
			}
			set
			{
				SetValue(RecentColorsProperty, value);
			}
		}

		/// <summary>Gets or sets the header text of the Recent Colors section in the dropdown.</summary>
		public string RecentColorsHeader
		{
			get
			{
				return (string)GetValue(RecentColorsHeaderProperty);
			}
			set
			{
				SetValue(RecentColorsHeaderProperty, value);
			}
		}

		/// <summary>Gets or sets the currently selected color.</summary>
		public Color? SelectedColor
		{
			get
			{
				return (Color?)GetValue(SelectedColorProperty);
			}
			set
			{
				SetValue(SelectedColorProperty, value);
			}
		}

		/// <summary>Gets the known color name of the SelectedColor, or its color hexadecimal
		/// string.</summary>
		public string SelectedColorText
		{
			get
			{
				return (string)GetValue(SelectedColorTextProperty);
			}
			protected set
			{
				SetValue(SelectedColorTextProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if the Advanced color mode tabs are visible in the popup.</summary>
		public bool ShowTabHeaders
		{
			get
			{
				return (bool)GetValue(ShowTabHeadersProperty);
			}
			set
			{
				SetValue(ShowTabHeadersProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the Show Available section in the dropdown is shown.</summary>
		public bool ShowAvailableColors
		{
			get
			{
				return (bool)GetValue(ShowAvailableColorsProperty);
			}
			set
			{
				SetValue(ShowAvailableColorsProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the Recent Colors section in the dropdown is shown.</summary>
		public bool ShowRecentColors
		{
			get
			{
				return (bool)GetValue(ShowRecentColorsProperty);
			}
			set
			{
				SetValue(ShowRecentColorsProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the Standard Colors section in the dropdown is shown.</summary>
		public bool ShowStandardColors
		{
			get
			{
				return (bool)GetValue(ShowStandardColorsProperty);
			}
			set
			{
				SetValue(ShowStandardColorsProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dropdown button is shown.</summary>
		public bool ShowDropDownButton
		{
			get
			{
				return (bool)GetValue(ShowDropDownButtonProperty);
			}
			set
			{
				SetValue(ShowDropDownButtonProperty, value);
			}
		}

		/// <summary>Gets or sets the text to use for the "Standard" tab in the ColorPicker's popup.</summary>
		public string StandardTabHeader
		{
			get
			{
				return (string)GetValue(StandardTabHeaderProperty);
			}
			set
			{
				SetValue(StandardTabHeaderProperty, value);
			}
		}

		/// <summary>Gets or sets the collection of standard colors.</summary>
		public ObservableCollection<ColorItem> StandardColors
		{
			get
			{
				return (ObservableCollection<ColorItem>)GetValue(StandardColorsProperty);
			}
			set
			{
				SetValue(StandardColorsProperty, value);
			}
		}

		/// <summary>Gets or sets the header text of the Standard Colors section in the dropdown.</summary>
		public string StandardColorsHeader
		{
			get
			{
				return (string)GetValue(StandardColorsHeaderProperty);
			}
			set
			{
				SetValue(StandardColorsHeaderProperty, value);
			}
		}

		/// <summary>Gets or sets the background of the tabs used in the popup.</summary>
		public Brush TabBackground
		{
			get
			{
				return (Brush)GetValue(TabBackgroundProperty);
			}
			set
			{
				SetValue(TabBackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the foreground of the tabs used in the popup.</summary>
		public Brush TabForeground
		{
			get
			{
				return (Brush)GetValue(TabForegroundProperty);
			}
			set
			{
				SetValue(TabForegroundProperty, value);
			}
		}

		/// <summary>Gets a value indicating whether the alpha channel is being used.</summary>
		public bool UsingAlphaChannel
		{
			get
			{
				return (bool)GetValue(UsingAlphaChannelProperty);
			}
			set
			{
				SetValue(UsingAlphaChannelProperty, value);
			}
		}

		/// <summary>Raised when SelectedColor changes.</summary>
		public event RoutedPropertyChangedEventHandler<Color?> SelectedColorChanged
		{
			add
			{
				AddHandler(SelectedColorChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedColorChangedEvent, value);
			}
		}

		/// <summary>
		///   <para>
		///     <font size="2">Raised when the ColorPicker popup is opened.</font>
		///   </para>
		/// </summary>
		public event RoutedEventHandler Opened
		{
			add
			{
				AddHandler(OpenedEvent, value);
			}
			remove
			{
				RemoveHandler(OpenedEvent, value);
			}
		}

		/// <summary>R <font size="2">aised when the ColorPicker popup is closed.</font></summary>
		public event RoutedEventHandler Closed
		{
			add
			{
				AddHandler(ClosedEvent, value);
			}
			remove
			{
				RemoveHandler(ClosedEvent, value);
			}
		}

		private static void OnAvailableColorsSortingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;
			if (colorPicker != null)
			{
				colorPicker.OnAvailableColorsSortingModeChanged((ColorSortingMode)e.OldValue, (ColorSortingMode)e.NewValue);
			}
		}

		private void OnAvailableColorsSortingModeChanged(ColorSortingMode oldValue, ColorSortingMode newValue)
		{
			ListCollectionView listCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(AvailableColors);
			if (listCollectionView != null)
			{
				listCollectionView.CustomSort = ((AvailableColorsSortingMode == ColorSortingMode.HueSaturationBrightness) ? new ColorSorter() : null);
			}
		}

		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;
			if (colorPicker != null)
			{
				colorPicker.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		private void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (newValue)
			{
				_initialColor = SelectedColor;
			}
			RoutedEventArgs e = new RoutedEventArgs(newValue ? OpenedEvent : ClosedEvent, this);
			RaiseEvent(e);
		}

		private static void OnMaxDropDownWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = o as ColorPicker;
			if (colorPicker != null)
			{
				colorPicker.OnMaxDropDownWidthChanged((double)e.OldValue, (double)e.NewValue);
			}
		}

		protected virtual void OnMaxDropDownWidthChanged(double oldValue, double newValue)
		{
		}

		private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;
			if (colorPicker != null)
			{
				colorPicker.OnSelectedColorChanged((Color?)e.OldValue, (Color?)e.NewValue);
			}
		}

		private void OnSelectedColorChanged(Color? oldValue, Color? newValue)
		{
			SelectedColorText = GetFormatedColorString(newValue);
			RoutedPropertyChangedEventArgs<Color?> routedPropertyChangedEventArgs = new RoutedPropertyChangedEventArgs<Color?>(oldValue, newValue);
			routedPropertyChangedEventArgs.RoutedEvent = SelectedColorChangedEvent;
			RaiseEvent(routedPropertyChangedEventArgs);
		}

		private static void OnUsingAlphaChannelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker colorPicker = (ColorPicker)d;
			if (colorPicker != null)
			{
				colorPicker.OnUsingAlphaChannelChanged();
			}
		}

		private void OnUsingAlphaChannelChanged()
		{
			SelectedColorText = GetFormatedColorString(SelectedColor);
		}

		static ColorPicker()
		{
			AdvancedTabHeaderProperty = DependencyProperty.Register("AdvancedTabHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Advanced"));
			AvailableColorsProperty = DependencyProperty.Register("AvailableColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(CreateAvailableColors()));
			AvailableColorsSortingModeProperty = DependencyProperty.Register("AvailableColorsSortingMode", typeof(ColorSortingMode), typeof(ColorPicker), new UIPropertyMetadata(ColorSortingMode.Alphabetical, OnAvailableColorsSortingModeChanged));
			AvailableColorsHeaderProperty = DependencyProperty.Register("AvailableColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Available Colors"));
			ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(ColorPicker));
			DisplayColorAndNameProperty = DependencyProperty.Register("DisplayColorAndName", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));
			DisplayColorTooltipProperty = DependencyProperty.Register("DisplayColorTooltip", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
			ColorModeProperty = DependencyProperty.Register("ColorMode", typeof(ColorMode), typeof(ColorPicker), new UIPropertyMetadata(ColorMode.ColorPalette));
			DropDownBackgroundProperty = DependencyProperty.Register("DropDownBackground", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(null));
			HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(null));
			HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(Brushes.Black));
			IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false, OnIsOpenChanged));
			MaxDropDownWidthProperty = DependencyProperty.Register("MaxDropDownWidth", typeof(double), typeof(ColorPicker), new UIPropertyMetadata(214.0));
			RecentColorsProperty = DependencyProperty.Register("RecentColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(null));
			RecentColorsHeaderProperty = DependencyProperty.Register("RecentColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Recent Colors"));
			SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color?), typeof(ColorPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorPropertyChanged));
			SelectedColorTextProperty = DependencyProperty.Register("SelectedColorText", typeof(string), typeof(ColorPicker), new UIPropertyMetadata(""));
			ShowTabHeadersProperty = DependencyProperty.Register("ShowTabHeaders", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
			ShowAvailableColorsProperty = DependencyProperty.Register("ShowAvailableColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
			ShowRecentColorsProperty = DependencyProperty.Register("ShowRecentColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));
			ShowStandardColorsProperty = DependencyProperty.Register("ShowStandardColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
			ShowDropDownButtonProperty = DependencyProperty.Register("ShowDropDownButton", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
			StandardTabHeaderProperty = DependencyProperty.Register("StandardTabHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Standard"));
			StandardColorsProperty = DependencyProperty.Register("StandardColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(CreateStandardColors()));
			StandardColorsHeaderProperty = DependencyProperty.Register("StandardColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Standard Colors"));
			TabBackgroundProperty = DependencyProperty.Register("TabBackground", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(null));
			TabForegroundProperty = DependencyProperty.Register("TabForeground", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(Brushes.Black));
			UsingAlphaChannelProperty = DependencyProperty.Register("UsingAlphaChannel", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUsingAlphaChannelPropertyChanged));
			SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color?>), typeof(ColorPicker));
			OpenedEvent = EventManager.RegisterRoutedEvent("OpenedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));
			ClosedEvent = EventManager.RegisterRoutedEvent("ClosedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
		}

		/// <summary>Initializes a new instance of the ColorPicker class.</summary>
		public ColorPicker()
		{

			SetCurrentValue(RecentColorsProperty, new ObservableCollection<ColorItem>());
			Keyboard.AddKeyDownHandler(this, OnKeyDown);
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
		}

		/// <summary>Builds the visual tree for the element.</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_availableColors != null)
			{
				_availableColors.SelectionChanged -= Color_SelectionChanged;
			}
			_availableColors = (GetTemplateChild("PART_AvailableColors") as ListBox);
			if (_availableColors != null)
			{
				_availableColors.SelectionChanged += Color_SelectionChanged;
			}
			if (_standardColors != null)
			{
				_standardColors.SelectionChanged -= Color_SelectionChanged;
			}
			_standardColors = (GetTemplateChild("PART_StandardColors") as ListBox);
			if (_standardColors != null)
			{
				_standardColors.SelectionChanged += Color_SelectionChanged;
			}
			if (_recentColors != null)
			{
				_recentColors.SelectionChanged -= Color_SelectionChanged;
			}
			_recentColors = (GetTemplateChild("PART_RecentColors") as ListBox);
			if (_recentColors != null)
			{
				_recentColors.SelectionChanged += Color_SelectionChanged;
			}
			if (_popup != null)
			{
				_popup.Opened -= Popup_Opened;
			}
			_popup = (GetTemplateChild("PART_ColorPickerPalettePopup") as Popup);
			if (_popup != null)
			{
				_popup.Opened += Popup_Opened;
			}
			_toggleButton = (base.Template.FindName("PART_ColorPickerToggleButton", this) as ToggleButton);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (_selectionChanged)
			{
				CloseColorPicker(true);
				_selectionChanged = false;
			}
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!IsOpen)
			{
				if (KeyboardUtilities.IsKeyModifyingPopupState(e))
				{
					IsOpen = true;
					e.Handled = true;
				}
			}
			else if (KeyboardUtilities.IsKeyModifyingPopupState(e))
			{
				CloseColorPicker(true);
				e.Handled = true;
			}
			else if (e.Key == Key.Escape)
			{
				SelectedColor = _initialColor;
				CloseColorPicker(true);
				e.Handled = true;
			}
		}

		private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
		{
			CloseColorPicker(true);
		}

		private void Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (e.AddedItems.Count > 0)
			{
				ColorItem colorItem = (ColorItem)e.AddedItems[0];
				SelectedColor = colorItem.Color;
				if (!string.IsNullOrEmpty(colorItem.Name))
				{
					SelectedColorText = colorItem.Name;
				}
				UpdateRecentColors(colorItem);
				_selectionChanged = true;
				listBox.SelectedIndex = -1;
			}
		}

		private void Popup_Opened(object sender, EventArgs e)
		{
			if (_availableColors != null && ShowAvailableColors)
			{
				FocusOnListBoxItem(_availableColors);
			}
			else if (_standardColors != null && ShowStandardColors)
			{
				FocusOnListBoxItem(_standardColors);
			}
			else if (_recentColors != null && ShowRecentColors)
			{
				FocusOnListBoxItem(_recentColors);
			}
		}

		private void FocusOnListBoxItem(ListBox listBox)
		{
			ListBoxItem listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
			if (listBoxItem == null && listBox.Items.Count > 0)
			{
				listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items[0]);
			}
			if (listBoxItem != null)
			{
				listBoxItem.Focus();
			}
		}

		private void CloseColorPicker(bool isFocusOnColorPicker)
		{
			if (IsOpen)
			{
				IsOpen = false;
			}
			ReleaseMouseCapture();
			if (isFocusOnColorPicker && _toggleButton != null)
			{
				_toggleButton.Focus();
			}
			UpdateRecentColors(new ColorItem(SelectedColor, SelectedColorText));
		}

		private void UpdateRecentColors(ColorItem colorItem)
		{
			if (!RecentColors.Contains(colorItem))
			{
				RecentColors.Add(colorItem);
			}
			if (RecentColors.Count > 10)
			{
				RecentColors.RemoveAt(0);
			}
		}

		private string GetFormatedColorString(Color? colorToFormat)
		{
			if (!colorToFormat.HasValue || !colorToFormat.HasValue)
			{
				return string.Empty;
			}
			return ColorUtilities.FormatColorString(colorToFormat.Value.GetColorName(), UsingAlphaChannel);
		}

		private static ObservableCollection<ColorItem> CreateStandardColors()
		{
			ObservableCollection<ColorItem> observableCollection = new ObservableCollection<ColorItem>();
			observableCollection.Add(new ColorItem(Colors.Transparent, "Transparent"));
			observableCollection.Add(new ColorItem(Colors.White, "White"));
			observableCollection.Add(new ColorItem(Colors.Gray, "Gray"));
			observableCollection.Add(new ColorItem(Colors.Black, "Black"));
			observableCollection.Add(new ColorItem(Colors.Red, "Red"));
			observableCollection.Add(new ColorItem(Colors.Green, "Green"));
			observableCollection.Add(new ColorItem(Colors.Blue, "Blue"));
			observableCollection.Add(new ColorItem(Colors.Yellow, "Yellow"));
			observableCollection.Add(new ColorItem(Colors.Orange, "Orange"));
			observableCollection.Add(new ColorItem(Colors.Purple, "Purple"));
			return observableCollection;
		}

		private static ObservableCollection<ColorItem> CreateAvailableColors()
		{
			ObservableCollection<ColorItem> observableCollection = new ObservableCollection<ColorItem>();
			foreach (KeyValuePair<string, Color> knownColor in ColorUtilities.KnownColors)
			{
				if (!string.Equals(knownColor.Key, "Transparent"))
				{
					ColorItem item = new ColorItem(knownColor.Value, knownColor.Key);
					if (!observableCollection.Contains(item))
					{
						observableCollection.Add(item);
					}
				}
			}
			return observableCollection;
		}
	}
}
