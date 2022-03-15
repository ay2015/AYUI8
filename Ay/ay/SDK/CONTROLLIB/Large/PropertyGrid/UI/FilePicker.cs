using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;


namespace Xceed.Wpf.Toolkit
{
	/// <summary>
	///   <para>Represents an editor that allows a user to pick a file from the disk.</para>
	/// </summary>
	public class FilePicker : Control
	{
		private bool _isLocalUpdate;

		/// <summary>Identifies the BrowseContent dependency property.</summary>
		public static readonly DependencyProperty BrowseContentProperty;

		/// <summary>Identifies the BrowseButtonStyle dependency property.</summary>
		public static readonly DependencyProperty BrowseButtonStyleProperty;

		/// <summary>Identifies the Filter dependency property.</summary>
		public static readonly DependencyProperty FilterProperty;

		/// <summary>Identifies the MultiSelect dependency property.</summary>
		public static readonly DependencyProperty MultiSelectProperty;

		/// <summary>Identifies the SelectedFile dependency property.</summary>
		public static readonly DependencyProperty SelectedFileProperty;

		/// <summary>Identifies the SelectedFiles dependency property.</summary>
		public static readonly DependencyProperty SelectedFilesProperty;

		/// <summary>Identifies the SelectedValue dependency property.</summary>
		public static readonly DependencyProperty SelectedValueProperty;

		/// <summary>Identifies the IntialDirectory dependency property.</summary>
		public static readonly DependencyProperty InitialDirectoryProperty;

		/// <summary>Identifies the IsOpen dependency property.</summary>
		public static readonly DependencyProperty IsOpenProperty;

		/// <summary>Identifies the Title dependency property.</summary>
		public static readonly DependencyProperty TitleProperty;

		/// <summary>Identifies the UseFullPath dependency property.</summary>
		public static readonly DependencyProperty UseFullPathProperty;

		/// <summary>Identifies the Watermark dependency property.</summary>
		public static readonly DependencyProperty WatermarkProperty;

		/// <summary>Identifies the WatermarkTemplate dependency property.</summary>
		public static readonly DependencyProperty WatermarkTemplateProperty;

		/// <summary>Identifies the SeletedFileChanged event.</summary>
		public static readonly RoutedEvent SelectedFileChangedEvent;

		/// <summary>Identifies the SelectedFileChanged event.</summary>
		public static readonly RoutedEvent SelectedFilesChangedEvent;

		/// <summary>Gets or sets the content of the "Browse" button.</summary>
		public object BrowseContent
		{
			get
			{
				return GetValue(BrowseContentProperty);
			}
			set
			{
				SetValue(BrowseContentProperty, value);
			}
		}

		/// <summary>Gets or sets the style of the "Browse" button.</summary>
		public Style BrowseButtonStyle
		{
			get
			{
				return (Style)GetValue(BrowseButtonStyleProperty);
			}
			set
			{
				SetValue(BrowseButtonStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the filter to ues when opening the browsing window. For example, "Image Files | *.jpg;*.jpeg"</summary>
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

		/// <summary>Gets or sets a value indicating whether mulitple files can be selected in the browsing window.</summary>
		public bool MultiSelect
		{
			get
			{
				return (bool)GetValue(MultiSelectProperty);
			}
			set
			{
				SetValue(MultiSelectProperty, value);
			}
		}

		/// <summary>Gets or sets the name of the selected file.</summary>
		public string SelectedFile
		{
			get
			{
				return (string)GetValue(SelectedFileProperty);
			}
			set
			{
				SetValue(SelectedFileProperty, value);
			}
		}

		/// <summary>Gets or sets a collection of strings representing the names of the selected files.</summary>
		public ObservableCollection<string> SelectedFiles
		{
			get
			{
				return (ObservableCollection<string>)GetValue(SelectedFilesProperty);
			}
			set
			{
				SetValue(SelectedFilesProperty, value);
			}
		}

		/// <summary>Gets the string representing the displayed selection(s) in the FilePicker.</summary>
		public string SelectedValue
		{
			get
			{
				return (string)GetValue(SelectedValueProperty);
			}
			private set
			{
				SetValue(SelectedValueProperty, value);
			}
		}

		/// <summary>Gets or sets the path that will be used as the intial directory when the browsing window is opened.</summary>
		public string InitialDirectory
		{
			get
			{
				return (string)GetValue(InitialDirectoryProperty);
			}
			set
			{
				SetValue(InitialDirectoryProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the browsing window is open.</summary>
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

		/// <summary>Gets or sets the title of the browsing window.</summary>
		public string Title
		{
			get
			{
				return (string)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the full path of the selected file(s) should be included when return the name of the selected file(s) through the
		/// SelectedFile or <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.FilePicker~SelectedFiles.html">SelectedFiles</see> properties.</summary>
		public bool UseFullPath
		{
			get
			{
				return (bool)GetValue(UseFullPathProperty);
			}
			set
			{
				SetValue(UseFullPathProperty, value);
			}
		}

		/// <summary>Gets or sets the watermark to display when no files are selected.</summary>
		public object Watermark
		{
			get
			{
				return GetValue(WatermarkProperty);
			}
			set
			{
				SetValue(WatermarkProperty, value);
			}
		}

		/// <summary>Gets or sets the <strong>DataTemplate</strong> to use for the Watermark.</summary>
		public DataTemplate WatermarkTemplate
		{
			get
			{
				return (DataTemplate)GetValue(WatermarkTemplateProperty);
			}
			set
			{
				SetValue(WatermarkTemplateProperty, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<string> SelectedFileChanged
		{
			add
			{
				AddHandler(SelectedFileChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedFileChangedEvent, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<ObservableCollection<string>> SelectedFilesChanged
		{
			add
			{
				AddHandler(SelectedFilesChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedFilesChangedEvent, value);
			}
		}

		static FilePicker()
		{
			BrowseContentProperty = DependencyProperty.Register("BrowseContent", typeof(object), typeof(FilePicker), new UIPropertyMetadata(null));
			BrowseButtonStyleProperty = DependencyProperty.Register("BrowseButtonStyle", typeof(Style), typeof(FilePicker), new UIPropertyMetadata(null));
			FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FilePicker), new UIPropertyMetadata(""));
			MultiSelectProperty = DependencyProperty.Register("MultiSelect", typeof(bool), typeof(FilePicker), new UIPropertyMetadata(false));
			SelectedFileProperty = DependencyProperty.Register("SelectedFile", typeof(string), typeof(FilePicker), new UIPropertyMetadata("", OnSelectedFileChanged));
			SelectedFilesProperty = DependencyProperty.Register("SelectedFiles", typeof(ObservableCollection<string>), typeof(FilePicker), new UIPropertyMetadata(null, OnSelectedFilesChanged));
			SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(string), typeof(FilePicker), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));
			InitialDirectoryProperty = DependencyProperty.Register("InitialDirectory", typeof(string), typeof(FilePicker), new UIPropertyMetadata(""));
			IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(FilePicker), new UIPropertyMetadata(false, OnIsOpenChanged));
			TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FilePicker), new UIPropertyMetadata("FilePicker"));
			UseFullPathProperty = DependencyProperty.Register("UseFullPath", typeof(bool), typeof(FilePicker), new UIPropertyMetadata(true));
			WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(FilePicker), new UIPropertyMetadata(null));
			WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(FilePicker), new UIPropertyMetadata(null));
			SelectedFileChangedEvent = EventManager.RegisterRoutedEvent("SelectedFileChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(FilePicker));
			SelectedFilesChangedEvent = EventManager.RegisterRoutedEvent("SelectedFilesChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<ObservableCollection<string>>), typeof(FilePicker));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePicker), new FrameworkPropertyMetadata(typeof(FilePicker)));
		}

		/// <summary>Initializes a new instance of the <strong>FilePicker</strong> class.</summary>
		public FilePicker()
		{
			
			SetCurrentValue(SelectedFilesProperty, new ObservableCollection<string>());
			base.Loaded += FilePicker_Loaded;
		}

		private static void OnSelectedFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilePicker filePicker = (FilePicker)d;
			if (filePicker != null)
			{
				filePicker.OnSelectedFileChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		private void OnSelectedFileChanged(string oldValue, string newValue)
		{
			if (!UseFullPath && newValue != null && newValue.IndexOfAny(new char[2]
			{
				'/',
				'\\'
			}) != -1)
			{
				throw new InvalidOperationException("SelectedFile shouldn't contain \"/\" or \"\\\" when UseFullPath is false.");
			}
			UpdateSelectedValue();
			RoutedPropertyChangedEventArgs<string> routedPropertyChangedEventArgs = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue);
			routedPropertyChangedEventArgs.RoutedEvent = SelectedFileChangedEvent;
			RaiseEvent(routedPropertyChangedEventArgs);
		}

		private static void OnSelectedFilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilePicker filePicker = (FilePicker)d;
			if (filePicker != null)
			{
				filePicker.OnSelectedFilesChanged((ObservableCollection<string>)e.OldValue, (ObservableCollection<string>)e.NewValue);
			}
		}

		private void OnSelectedFilesChanged(ObservableCollection<string> oldValue, ObservableCollection<string> newValue)
		{
			if (!UseFullPath && newValue != null && newValue.FirstOrDefault((string x) => x.IndexOfAny(new char[2]
			{
				'/',
				'\\'
			}) != -1) != null)
			{
				throw new InvalidOperationException("SelectedFiles shouldn't contain a string with \"/\" or \"\\\" when UseFullPath is false.");
			}
			if (oldValue != null)
			{
				((INotifyCollectionChanged)oldValue).CollectionChanged -= SelectedFiles_CollectionChanged;
			}
			SelectedFile = null;
			if (newValue != null)
			{
				((INotifyCollectionChanged)newValue).CollectionChanged += SelectedFiles_CollectionChanged;
				SelectedFile = newValue.FirstOrDefault();
			}
			UpdateSelectedValue();
			RoutedPropertyChangedEventArgs<ObservableCollection<string>> routedPropertyChangedEventArgs = new RoutedPropertyChangedEventArgs<ObservableCollection<string>>(oldValue, newValue);
			routedPropertyChangedEventArgs.RoutedEvent = SelectedFilesChangedEvent;
			RaiseEvent(routedPropertyChangedEventArgs);
		}

		private static void OnSelectedValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			FilePicker filePicker = o as FilePicker;
			if (filePicker != null)
			{
				filePicker.OnSelectedValueChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Raises the <strong>SelectedValueChanged</strong> event.</summary>
		protected virtual void OnSelectedValueChanged(string oldValue, string newValue)
		{
			if (!_isLocalUpdate)
			{
				if (MultiSelect)
				{
					string[] array = null;
					if (!string.IsNullOrEmpty(SelectedValue))
					{
						string text = SelectedValue.Replace("\"", "");
						array = text.Split(' ');
					}
					SelectedFiles.Clear();
					if (array != null)
					{
						string[] array2 = array;
						foreach (string item in array2)
						{
							SelectedFiles.Add(item);
						}
					}
				}
				else
				{
					SelectedFile = SelectedValue;
				}
			}
		}

		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilePicker filePicker = (FilePicker)d;
			if (filePicker != null)
			{
				filePicker.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		private void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (newValue)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = Filter;
				openFileDialog.FileName = SelectedValue;
				openFileDialog.InitialDirectory = InitialDirectory;
				openFileDialog.Title = Title;
				openFileDialog.Multiselect = MultiSelect;
				bool? flag = openFileDialog.ShowDialog();
				if (flag.HasValue && flag.Value)
				{
					if (MultiSelect)
					{
						string[] array = UseFullPath ? openFileDialog.FileNames : openFileDialog.SafeFileNames;
						SelectedFiles.Clear();
						string[] array2 = array;
						foreach (string item in array2)
						{
							SelectedFiles.Add(item);
						}
					}
					else
					{
						SelectedFile = (UseFullPath ? openFileDialog.FileName : openFileDialog.SafeFileName);
					}
				}
				IsOpen = false;
			}
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		private void UpdateSelectedValue()
		{
			_isLocalUpdate = true;
			SelectedValue = (MultiSelect ? CreateSelectedValueFromStrings() : SelectedFile);
			_isLocalUpdate = false;
		}

		private string CreateSelectedValueFromStrings()
		{
			string text = "";
			if (SelectedFiles != null)
			{
				foreach (string selectedFile in SelectedFiles)
				{
					text = text + "\"" + selectedFile + "\" ";
				}
				return text;
			}
			return text;
		}

		private void FilePicker_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateSelectedValue();
		}

		private void SelectedFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!UseFullPath && e.NewItems != null && e.NewItems.Count > 0 && e.NewItems[0] is string && ((string)e.NewItems[0]).IndexOfAny(new char[2]
			{
				'/',
				'\\'
			}) != -1)
			{
				throw new InvalidOperationException("SelectedFiles shouldn't contain a string with \"/\" or \"\\\" when UseFullPath is false.");
			}
			SelectedFile = ((e.NewItems != null && e.NewItems.Count > 0 && e.NewItems[0] is string) ? ((string)e.NewItems[0]) : null);
			UpdateSelectedValue();
		}
	}
}
