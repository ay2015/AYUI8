using ay.Controls.Args;
using ay.Controls.Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ay.Controls
{
//    0

//数字，必选。此元素将接受 0 到 9 之间的任何一个数字。

//9

//数字或空间，可选。

//#

//数字或空间，可选。如果掩码中该位置为空白，在 属性中将把它呈现为一个空格。允许使用加号(+) 和减号(-)。

//L

//字母，必选。将输入限定为 ASCII 字母 a-z 和 A-Z。此掩码元素等效于正则表达式中的[a - zA - Z]。

//?

//字母，可选。输入限定为 ASCII 字母 a-z 和 A-Z。此掩码元素等效于正则表达式中的[a - zA - Z]?。

//&

//字符，必选。如果 属性设置为 true，此元素的行为将与“L”元素类似。

//C

//字符，可选。任何非控制字符。如果 AsciiOnly 属性设置为 true，此元素的行为将类似于“?”元素。

//A

//字母数字，可选。如果将 AsciiOnly 属性设置为 true，则它接受的唯一字符是 ASCII 字母 a-z 和 A-Z。

//a

//字母数字，可选。如果将 AsciiOnly 属性设置为 true，则它接受的唯一字符是 ASCII 字母 a-z 和 A-Z。

//.

//小数点占位符。使用的实际显示字符将是适合于格式提供程序的小数点符号，格式提供程序由控件的FormatProvider 属性决定。

//,

//千分位占位符。使用的实际显示字符将是相应于格式提供程序的千分位占位符，格式提供程序由控件的 FormatProvider 属性决定。

//:

//时间分隔符。使用的实际显示字符将是适合于格式提供程序的时间符号，格式提供程序由控件的 FormatProvider 属性决定。

///

//日期分隔符。使用的实际显示字符将是适合于格式提供程序的日期符号，格式提供程序由控件的 FormatProvider 属性决定。

//$

//货币符号。显示的实际字符将是相应于格式提供程序的货币符号，格式提供程序由控件的 FormatProvider 属性决定。

//<

//转换为小写。将后续所有字符都转换为小写。

//>

//转换为大写。将后续所有字符都转换为大写。

//|

//禁用前一个大写转换或小写转换。

///

//转义。对掩码字符进行转义，将其转变为原义字符。“//”是反斜杠的转义序列。

//其他所有字符

//原义字符。所有非掩码元素都将原样出现在 MaskedTextBox 中。原义字符在运行时始终占据掩码中的一个固定位置，用户不能移动或删除该字符。

    /// <summary>
    /// 控件迁移 Wpf.Toolkit的
    /// </summary>
	public class MaskedTextBox : ValueRangeTextBox
	{
        public new string ControlID { get { return ControlGUID.MaskedTextBox; } }

        private static readonly char[] MaskChars;

		private static char DefaultPasswordChar;

		private static string NullMaskString;

		/// <summary>Identifies the AllowPromptAsInput dependency property.</summary>
		public static readonly DependencyProperty AllowPromptAsInputProperty;

		/// <summary>Identifies the ClipboardMaskFormat dependency property.</summary>
		public static readonly DependencyProperty ClipboardMaskFormatProperty;

		/// <summary>Identifies the HidePromptOnLeave dependency property.</summary>
		public static readonly DependencyProperty HidePromptOnLeaveProperty;

		/// <summary>Identifies the IncludeLiteralsInValue dependency property.</summary>
		public static readonly DependencyProperty IncludeLiteralsInValueProperty;

		/// <summary>Identifies the IncludePromptInValue dependency property.</summary>
		public static readonly DependencyProperty IncludePromptInValueProperty;

		/// <summary>Identifies the InsertKeyMode dependency property.</summary>
		public static readonly DependencyProperty InsertKeyModeProperty;

		private static readonly DependencyPropertyKey IsMaskCompletedPropertyKey;

		/// <summary>Identifies the IsMaskCompleted dependency property.</summary>
		public static readonly DependencyProperty IsMaskCompletedProperty;

		private static readonly DependencyPropertyKey IsMaskFullPropertyKey;

		/// <summary>Identifies the IsMaskFull dependency property.</summary>
		public static readonly DependencyProperty IsMaskFullProperty;

		/// <summary>Identifies the Mask dependency property.</summary>
		public static readonly DependencyProperty MaskProperty;

		/// <summary>Identifies the PromptChar dependency property. The identifier for the
		/// PromptChar dependency property.</summary>
		public static readonly DependencyProperty PromptCharProperty;

		/// <summary>Identifies the RejectInputOnFirstFailure dependency property.</summary>
		public static readonly DependencyProperty RejectInputOnFirstFailureProperty;

		/// <summary>Identifies the ResetOnPrompt dependency property.</summary>
		public static readonly DependencyProperty ResetOnPromptProperty;

		/// <summary>Identifies the ResetOnSpace dependency property.</summary>
		public static readonly DependencyProperty ResetOnSpaceProperty;

		/// <summary>Identifies the RestrictToAscii dependency property.</summary>
		public static readonly DependencyProperty RestrictToAsciiProperty;

		/// <summary>Identifies the SkipLiterals dependency property.</summary>
		public static readonly DependencyProperty SkipLiteralsProperty;

		private MaskedTextProvider m_maskedTextProvider;

		private bool m_insertToggled;

		private bool m_maskIsNull = true;

		private bool m_forcingMask;

		private List<int> m_unhandledLiteralsPositions;

		private string m_formatSpecifier;

		private MethodInfo m_valueToStringMethodInfo;

		/// <summary>Gets or sets a value indicating if the prompt character should be considered as
		/// a valid input character.</summary>
		public bool AllowPromptAsInput
		{
			get
			{
				return (bool)GetValue(AllowPromptAsInputProperty);
			}
			set
			{
				SetValue(AllowPromptAsInputProperty, value);
			}
		}

		/// <summary>Gets or sets a value representing the mask format that will be used when the inputted text is copied to the clipboard.</summary>
		public MaskFormat ClipboardMaskFormat
		{
			get
			{
				return (MaskFormat)GetValue(ClipboardMaskFormatProperty);
			}
			set
			{
				SetValue(ClipboardMaskFormatProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if the prompt character is hidden when the
		/// masked text box loses focus.</summary>
		public bool HidePromptOnLeave
		{
			get
			{
				return (bool)GetValue(HidePromptOnLeaveProperty);
			}
			set
			{
				SetValue(HidePromptOnLeaveProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if literal characters in the input mask should be used to calculate the numeric value of the input text.</summary>
		public bool IncludeLiteralsInValue
		{
			get
			{
				return (bool)GetValue(IncludeLiteralsInValueProperty);
			}
			set
			{
				SetValue(IncludeLiteralsInValueProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if prompt characters in the input mask should be included in the numeric value of the input text.</summary>
		public bool IncludePromptInValue
		{
			get
			{
				return (bool)GetValue(IncludePromptInValueProperty);
			}
			set
			{
				SetValue(IncludePromptInValueProperty, value);
			}
		}

		/// <summary>Gets or sets a value representing the text insertion mode of the masked text box.</summary>
		public InsertKeyMode InsertKeyMode
		{
			get
			{
				return (InsertKeyMode)GetValue(InsertKeyModeProperty);
			}
			set
			{
				SetValue(InsertKeyModeProperty, value);
			}
		}

		/// <summary>Gets a value indicating if all <strong>required</strong> characters have been inputted into the mask.</summary>
		public bool IsMaskCompleted
		{
			get
			{
				return (bool)GetValue(IsMaskCompletedProperty);
			}
		}

		/// <summary>Gets a value indicating if all characters, required and optional, have been inputted into the mask.</summary>
		public bool IsMaskFull
		{
			get
			{
				return (bool)GetValue(IsMaskFullProperty);
			}
		}

		/// <summary>Gets or sets the input mask.</summary>
		public string Mask
		{
			get
			{
				return (string)GetValue(MaskProperty);
			}
			set
			{
				SetValue(MaskProperty, value);
			}
		}

		/// <summary>
		///   <span class="st">Gets the MaskedTextProvider that was used to mask the input text.</span>
		/// </summary>
		public MaskedTextProvider MaskedTextProvider
		{
			get
			{
				if (!m_maskIsNull)
				{
					return m_maskedTextProvider.Clone() as MaskedTextProvider;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the character that represents the positions in the masked text box that require user input.</summary>
		public char PromptChar
		{
			get
			{
				return (char)GetValue(PromptCharProperty);
			}
			set
			{
				SetValue(PromptCharProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if inputted text that is pasted into the masked text box can be rejected if it contains an invalid character for the
		/// corresponding mask position.</summary>
		public bool RejectInputOnFirstFailure
		{
			get
			{
				return (bool)GetValue(RejectInputOnFirstFailureProperty);
			}
			set
			{
				SetValue(RejectInputOnFirstFailureProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if the character at the current caret position should be reset when the prompt character is pressed.</summary>
		public bool ResetOnPrompt
		{
			get
			{
				return (bool)GetValue(ResetOnPromptProperty);
			}
			set
			{
				SetValue(ResetOnPromptProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if the character at the current caret position should be reset when the space bar is pressed.</summary>
		public bool ResetOnSpace
		{
			get
			{
				return (bool)GetValue(ResetOnSpaceProperty);
			}
			set
			{
				SetValue(ResetOnSpaceProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if the masked text box accepts non-ASCII characters.</summary>
		public bool RestrictToAscii
		{
			get
			{
				return (bool)GetValue(RestrictToAsciiProperty);
			}
			set
			{
				SetValue(RestrictToAsciiProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating if literal values can be overwritten by their same values.</summary>
		public bool SkipLiterals
		{
			get
			{
				return (bool)GetValue(SkipLiteralsProperty);
			}
			set
			{
				SetValue(SkipLiteralsProperty, value);
			}
		}

		internal bool IsForcingMask
		{
			get
			{
				return m_forcingMask;
			}
		}

		internal string FormatSpecifier
		{
			get
			{
				return m_formatSpecifier;
			}
			set
			{
				m_formatSpecifier = value;
			}
		}

		internal override bool IsTextReadyToBeParsed
		{
			get
			{
				return IsMaskCompleted;
			}
		}

		private bool IsOverwriteMode
		{
			get
			{
				if (!m_maskIsNull)
				{
					switch (InsertKeyMode)
					{
					case InsertKeyMode.Default:
						return m_insertToggled;
					case InsertKeyMode.Insert:
						return false;
					case InsertKeyMode.Overwrite:
						return true;
					}
				}
				return false;
			}
		}

		private string MaskedTextOutput
		{
			get
			{
				return m_maskedTextProvider.ToString();
			}
		}

		/// <summary>Raised when a mask is being automatically created.</summary>
		public event EventHandler<AutoCompletingMaskEventArgs> AutoCompletingMask;

		private static string GetRawText(MaskedTextProvider provider)
		{
			return provider.ToString(true, false, false, 0, provider.Length);
		}

		/// <summary>Retrieves a format specifier from the specified mask and format provider.</summary>
		/// <param name="mask">The mask from which the format specifier is to be extracted.</param>
		/// <param name="formatProvider">An <strong>IFormatProvider</strong> that will be used to extract the appropriate symbols for any numeric mask characters contained in the mask.</param>
		public static string GetFormatSpecifierFromMask(string mask, IFormatProvider formatProvider)
		{
			List<int> unhandledLiteralsPositions;
			return GetFormatSpecifierFromMask(mask, MaskChars, formatProvider, true, out unhandledLiteralsPositions);
		}

		private static string GetFormatSpecifierFromMask(string mask, char[] maskChars, IFormatProvider formatProvider, bool includeNonSeparatorLiteralsInValue, out List<int> unhandledLiteralsPositions)
		{
			unhandledLiteralsPositions = new List<int>();
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(formatProvider);
			StringBuilder stringBuilder = new StringBuilder(32);
			bool flag = false;
			int i = 0;
			int num = 0;
			for (; i < mask.Length; i++)
			{
				char c = mask[i];
				if (c == '\\' && !flag)
				{
					flag = true;
				}
				else if (flag || Array.IndexOf(maskChars, c) < 0)
				{
					flag = false;
					stringBuilder.Append('\\');
					stringBuilder.Append(c);
					if (!includeNonSeparatorLiteralsInValue && c != ' ')
					{
						unhandledLiteralsPositions.Add(num);
					}
					num++;
				}
				else
				{
					switch (c)
					{
					case '#':
					case '0':
					case '9':
						stringBuilder.Append('0');
						num++;
						break;
					case '.':
						stringBuilder.Append('.');
						num += instance.NumberDecimalSeparator.Length;
						break;
					case ',':
						stringBuilder.Append(',');
						num += instance.NumberGroupSeparator.Length;
						break;
					case '$':
					{
						string currencySymbol = instance.CurrencySymbol;
						stringBuilder.Append('"');
						stringBuilder.Append(currencySymbol);
						stringBuilder.Append('"');
						for (int j = 0; j < currencySymbol.Length; j++)
						{
							if (!includeNonSeparatorLiteralsInValue)
							{
								unhandledLiteralsPositions.Add(num);
							}
							num++;
						}
						break;
					}
					default:
						stringBuilder.Append(c);
						if (!includeNonSeparatorLiteralsInValue && c != ' ')
						{
							unhandledLiteralsPositions.Add(num);
						}
						num++;
						break;
					}
				}
			}
			return stringBuilder.ToString();
		}

		static MaskedTextBox()
		{
			MaskChars = new char[18]
			{
				'0',
				'9',
				'#',
				'L',
				'?',
				'&',
				'C',
				'A',
				'a',
				'.',
				',',
				':',
				'/',
				'$',
				'<',
				'>',
				'|',
				'\\'
			};
			DefaultPasswordChar = '\0';
			NullMaskString = "<>";
			AllowPromptAsInputProperty = DependencyProperty.Register("AllowPromptAsInput", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true, AllowPromptAsInputPropertyChangedCallback));
			ClipboardMaskFormatProperty = DependencyProperty.Register("ClipboardMaskFormat", typeof(MaskFormat), typeof(MaskedTextBox), new UIPropertyMetadata(MaskFormat.IncludeLiterals));
			HidePromptOnLeaveProperty = DependencyProperty.Register("HidePromptOnLeave", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(false));
			IncludeLiteralsInValueProperty = DependencyProperty.Register("IncludeLiteralsInValue", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true, InlcudeLiteralsInValuePropertyChangedCallback));
			IncludePromptInValueProperty = DependencyProperty.Register("IncludePromptInValue", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(false, IncludePromptInValuePropertyChangedCallback));
			InsertKeyModeProperty = DependencyProperty.Register("InsertKeyMode", typeof(InsertKeyMode), typeof(MaskedTextBox), new UIPropertyMetadata(InsertKeyMode.Default));
			IsMaskCompletedPropertyKey = DependencyProperty.RegisterReadOnly("IsMaskCompleted", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false));
			IsMaskCompletedProperty = IsMaskCompletedPropertyKey.DependencyProperty;
			IsMaskFullPropertyKey = DependencyProperty.RegisterReadOnly("IsMaskFull", typeof(bool), typeof(MaskedTextBox), new PropertyMetadata(false));
			IsMaskFullProperty = IsMaskFullPropertyKey.DependencyProperty;
			MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(MaskedTextBox), new UIPropertyMetadata(string.Empty, MaskPropertyChangedCallback, MaskCoerceValueCallback));
			PromptCharProperty = DependencyProperty.Register("PromptChar", typeof(char), typeof(MaskedTextBox), new UIPropertyMetadata('_', PromptCharPropertyChangedCallback, PromptCharCoerceValueCallback));
			RejectInputOnFirstFailureProperty = DependencyProperty.Register("RejectInputOnFirstFailure", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true));
			ResetOnPromptProperty = DependencyProperty.Register("ResetOnPrompt", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true, ResetOnPromptPropertyChangedCallback));
			ResetOnSpaceProperty = DependencyProperty.Register("ResetOnSpace", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true, ResetOnSpacePropertyChangedCallback));
			RestrictToAsciiProperty = DependencyProperty.Register("RestrictToAscii", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(false, RestrictToAsciiPropertyChangedCallback, RestrictToAsciiCoerceValueCallback));
			SkipLiteralsProperty = DependencyProperty.Register("SkipLiterals", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(true, SkipLiteralsPropertyChangedCallback));
			TextBox.TextProperty.OverrideMetadata(typeof(MaskedTextBox), new FrameworkPropertyMetadata(null, TextCoerceValueCallback));
		}

		/// <summary>Initializes a new instance of the MaskedTextBox class.</summary>
		public MaskedTextBox()
		{
		
			CommandManager.AddPreviewCanExecuteHandler(this, OnPreviewCanExecuteCommands);
			CommandManager.AddPreviewExecutedHandler(this, OnPreviewExecutedCommands);
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, null, CanExecutePaste));
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CanExecuteCut));
			base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, null, CanExecuteCopy));
			base.CommandBindings.Add(new CommandBinding(EditingCommands.ToggleInsert, ToggleInsertExecutedCallback));
			base.CommandBindings.Add(new CommandBinding(EditingCommands.Delete, null, CanExecuteDelete));
			base.CommandBindings.Add(new CommandBinding(EditingCommands.DeletePreviousWord, null, CanExecuteDeletePreviousWord));
			base.CommandBindings.Add(new CommandBinding(EditingCommands.DeleteNextWord, null, CanExecuteDeleteNextWord));
			base.CommandBindings.Add(new CommandBinding(EditingCommands.Backspace, null, CanExecuteBackspace));
			DragDrop.AddPreviewQueryContinueDragHandler(this, PreviewQueryContinueDragCallback);
			base.AllowDrop = false;
		}

		private void InitializeMaskedTextProvider()
		{
			string text = base.Text;
			string mask = Mask;
			if (mask == string.Empty)
			{
				m_maskedTextProvider = CreateMaskedTextProvider(NullMaskString);
				m_maskIsNull = true;
			}
			else
			{
				m_maskedTextProvider = CreateMaskedTextProvider(mask);
				m_maskIsNull = false;
			}
			if (!m_maskIsNull && text != string.Empty && !m_maskedTextProvider.Add(text) && !DesignerProperties.GetIsInDesignMode(this))
			{
				throw new InvalidOperationException("An attempt was made to apply a new mask that cannot be applied to the current text.");
			}
		}

		/// <summary>Raises the Initialized event. This method is invoked whenever IsInitialized is set to true internally.</summary>
		/// <param name="e">The RoutedEventArgs that contains the event data.</param>
		protected override void OnInitialized(EventArgs e)
		{
			InitializeMaskedTextProvider();
			SetIsMaskCompleted(m_maskedTextProvider.MaskCompleted);
			SetIsMaskFull(m_maskedTextProvider.MaskFull);
			base.OnInitialized(e);
		}

		private static void AllowPromptAsInputPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider = maskedTextBox.CreateMaskedTextProvider(maskedTextBox.Mask);
			}
		}

		private static void InlcudeLiteralsInValuePropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized)
			{
				maskedTextBox.RefreshConversionHelpers();
				maskedTextBox.RefreshValue();
			}
		}

		private static void IncludePromptInValuePropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized)
			{
				maskedTextBox.RefreshValue();
			}
		}

		private void SetIsMaskCompleted(bool value)
		{
			SetValue(IsMaskCompletedPropertyKey, value);
		}

		private void SetIsMaskFull(bool value)
		{
			SetValue(IsMaskFullPropertyKey, value);
		}

		private static object MaskCoerceValueCallback(DependencyObject sender, object value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (value.Equals(string.Empty))
			{
				return value;
			}
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (!maskedTextBox.IsInitialized)
			{
				return value;
			}
			bool flag;
			try
			{
				MaskedTextProvider maskedTextProvider = maskedTextBox.CreateMaskedTextProvider((string)value);
				string rawText = GetRawText(maskedTextBox.m_maskedTextProvider);
				flag = maskedTextProvider.VerifyString(rawText);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("An error occured while testing the current text against the new mask.", innerException);
			}
			if (!flag)
			{
				throw new ArgumentException("The mask cannot be applied to the current text.", "Mask");
			}
			return value;
		}

		private static void MaskPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized)
			{
				MaskedTextProvider maskedTextProvider = null;
				string text = (string)e.NewValue;
				if (text == string.Empty)
				{
					maskedTextProvider = maskedTextBox.CreateMaskedTextProvider(NullMaskString);
					maskedTextBox.m_maskIsNull = true;
					maskedTextBox.Text = "";
				}
				else
				{
					maskedTextProvider = maskedTextBox.CreateMaskedTextProvider(text);
					maskedTextBox.m_maskIsNull = false;
				}
				maskedTextBox.m_maskedTextProvider = maskedTextProvider;
				maskedTextBox.RefreshConversionHelpers();
				if (maskedTextBox.ValueDataType != null)
				{
					string textFromValue = maskedTextBox.GetTextFromValue(maskedTextBox.Value);
					maskedTextBox.m_maskedTextProvider.Set(textFromValue);
				}
				maskedTextBox.RefreshCurrentText(true);
			}
		}

		private static object PromptCharCoerceValueCallback(object sender, object value)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized)
			{
				MaskedTextProvider maskedTextProvider = maskedTextBox.m_maskedTextProvider.Clone() as MaskedTextProvider;
				try
				{
					maskedTextProvider.PromptChar = (char)value;
					return value;
				}
				catch (Exception innerException)
				{
					throw new ArgumentException("The prompt character is invalid.", innerException);
				}
			}
			return value;
		}

		private static void PromptCharPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider.PromptChar = (char)e.NewValue;
				maskedTextBox.RefreshCurrentText(true);
			}
		}

		private static void ResetOnPromptPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider.ResetOnPrompt = (bool)e.NewValue;
			}
		}

		private static void ResetOnSpacePropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider.ResetOnSpace = (bool)e.NewValue;
			}
		}

		private static object RestrictToAsciiCoerceValueCallback(object sender, object value)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (!maskedTextBox.IsInitialized)
			{
				return value;
			}
			if (maskedTextBox.m_maskIsNull)
			{
				return value;
			}
			bool flag = (bool)value;
			if (!flag)
			{
				return value;
			}
			MaskedTextProvider maskedTextProvider = maskedTextBox.CreateMaskedTextProvider(maskedTextBox.Mask, maskedTextBox.GetCultureInfo(), maskedTextBox.AllowPromptAsInput, maskedTextBox.PromptChar, DefaultPasswordChar, flag);
			if (!maskedTextProvider.VerifyString(maskedTextBox.Text))
			{
				throw new ArgumentException("The current text cannot be restricted to ASCII characters. The RestrictToAscii property is set to true.", "RestrictToAscii");
			}
			return flag;
		}

		private static void RestrictToAsciiPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider = maskedTextBox.CreateMaskedTextProvider(maskedTextBox.Mask);
				maskedTextBox.RefreshCurrentText(true);
			}
		}

		private static void SkipLiteralsPropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox.IsInitialized && !maskedTextBox.m_maskIsNull)
			{
				maskedTextBox.m_maskedTextProvider.SkipLiterals = (bool)e.NewValue;
			}
		}

		private static object TextCoerceValueCallback(DependencyObject sender, object value)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (!maskedTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (maskedTextBox.IsInIMEComposition)
			{
				return value;
			}
			if (value == null)
			{
				value = string.Empty;
			}
			if (maskedTextBox.IsForcingText || maskedTextBox.m_maskIsNull)
			{
				return value;
			}
			return maskedTextBox.ValidateText((string)value);
		}

		private string ValidateText(string text)
		{
			string formattedString;
			if (RejectInputOnFirstFailure)
			{
				MaskedTextProvider maskedTextProvider = m_maskedTextProvider.Clone() as MaskedTextProvider;
				int testPosition;
				MaskedTextResultHint resultHint;
				if (maskedTextProvider.Set(text, out testPosition, out resultHint) || maskedTextProvider.Mask.StartsWith(">") || maskedTextProvider.Mask.StartsWith("<"))
				{
					formattedString = GetFormattedString(maskedTextProvider, text);
				}
				else
				{
					formattedString = GetFormattedString(m_maskedTextProvider, text);
					m_maskedTextProvider.Set(formattedString);
				}
			}
			else
			{
				MaskedTextProvider provider = (MaskedTextProvider)m_maskedTextProvider.Clone();
				int tentativeCaretIndex;
				if (CanReplace(provider, text, 0, m_maskedTextProvider.Length, RejectInputOnFirstFailure, out tentativeCaretIndex))
				{
					formattedString = GetFormattedString(provider, text);
				}
				else
				{
					formattedString = GetFormattedString(m_maskedTextProvider, text);
					m_maskedTextProvider.Set(formattedString);
				}
			}
			return formattedString;
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (!m_maskIsNull && (base.IsInValueChanged || !base.IsForcingText))
			{
				string text = base.Text;
				if (m_maskIsNull)
				{
					base.CaretIndex = text.Length;
				}
				else
				{
					m_maskedTextProvider.Set(text);
					if (m_maskedTextProvider.Mask.StartsWith(">") || m_maskedTextProvider.Mask.StartsWith("<"))
					{
						base.CaretIndex = text.Length;
					}
					else
					{
						int num = m_maskedTextProvider.FindUnassignedEditPositionFrom(0, true);
						if (num == -1)
						{
							num = m_maskedTextProvider.Length;
						}
						base.CaretIndex = num;
					}
				}
			}
			if (m_maskedTextProvider != null)
			{
				SetIsMaskCompleted(m_maskedTextProvider.MaskCompleted);
				SetIsMaskFull(m_maskedTextProvider.MaskFull);
			}
			base.OnTextChanged(e);
		}

		private void OnPreviewCanExecuteCommands(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				RoutedUICommand routedUICommand = e.Command as RoutedUICommand;
				if (routedUICommand != null && (routedUICommand.Name == "Space" || routedUICommand.Name == "ShiftSpace"))
				{
					if (base.IsReadOnly)
					{
						e.CanExecute = false;
					}
					else
					{
						MaskedTextProvider provider = (MaskedTextProvider)m_maskedTextProvider.Clone();
						int tentativeCaretIndex;
						e.CanExecute = CanReplace(provider, " ", base.SelectionStart, base.SelectionLength, RejectInputOnFirstFailure, out tentativeCaretIndex);
					}
					e.Handled = true;
				}
				else if (e.Command == ApplicationCommands.Undo || e.Command == ApplicationCommands.Redo)
				{
					e.CanExecute = false;
					e.Handled = true;
				}
			}
		}

		private void OnPreviewExecutedCommands(object sender, ExecutedRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				if (e.Command == EditingCommands.Delete)
				{
					e.Handled = true;
					Delete(base.SelectionStart, base.SelectionLength, true);
				}
				else if (e.Command == EditingCommands.DeleteNextWord)
				{
					e.Handled = true;
					EditingCommands.SelectRightByWord.Execute(null, this);
					Delete(base.SelectionStart, base.SelectionLength, true);
				}
				else if (e.Command == EditingCommands.DeletePreviousWord)
				{
					e.Handled = true;
					EditingCommands.SelectLeftByWord.Execute(null, this);
					Delete(base.SelectionStart, base.SelectionLength, false);
				}
				else if (e.Command == EditingCommands.Backspace)
				{
					e.Handled = true;
					Delete(base.SelectionStart, base.SelectionLength, false);
				}
				else if (e.Command == ApplicationCommands.Cut)
				{
					e.Handled = true;
					if (ApplicationCommands.Copy.CanExecute(null, this))
					{
						ApplicationCommands.Copy.Execute(null, this);
					}
					Delete(base.SelectionStart, base.SelectionLength, true);
				}
				else if (e.Command == ApplicationCommands.Copy)
				{
					e.Handled = true;
					ExecuteCopy();
				}
				else if (e.Command == ApplicationCommands.Paste)
				{
					e.Handled = true;
					string text = (string)Clipboard.GetDataObject().GetData("System.String");
					Replace(text, base.SelectionStart, base.SelectionLength);
				}
				else
				{
					RoutedUICommand routedUICommand = e.Command as RoutedUICommand;
					if (routedUICommand != null && (routedUICommand.Name == "Space" || routedUICommand.Name == "ShiftSpace"))
					{
						e.Handled = true;
						ProcessTextInput(" ");
					}
				}
			}
		}

		private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.CanExecute = CanDelete(base.SelectionStart, base.SelectionLength, true, MaskedTextProvider.Clone() as MaskedTextProvider);
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecuteDeletePreviousWord(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				bool flag = !base.IsReadOnly && EditingCommands.SelectLeftByWord.CanExecute(null, this);
				if (flag)
				{
					int selectionStart = base.SelectionStart;
					int selectionLength = base.SelectionLength;
					EditingCommands.SelectLeftByWord.Execute(null, this);
					flag = CanDelete(base.SelectionStart, base.SelectionLength, false, MaskedTextProvider.Clone() as MaskedTextProvider);
					if (!flag)
					{
						base.SelectionStart = selectionStart;
						base.SelectionLength = selectionLength;
					}
				}
				e.CanExecute = flag;
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecuteDeleteNextWord(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				bool flag = !base.IsReadOnly && EditingCommands.SelectRightByWord.CanExecute(null, this);
				if (flag)
				{
					int selectionStart = base.SelectionStart;
					int selectionLength = base.SelectionLength;
					EditingCommands.SelectRightByWord.Execute(null, this);
					flag = CanDelete(base.SelectionStart, base.SelectionLength, true, MaskedTextProvider.Clone() as MaskedTextProvider);
					if (!flag)
					{
						base.SelectionStart = selectionStart;
						base.SelectionLength = selectionLength;
					}
				}
				e.CanExecute = flag;
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecuteBackspace(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.CanExecute = CanDelete(base.SelectionStart, base.SelectionLength, false, MaskedTextProvider.Clone() as MaskedTextProvider);
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecuteCut(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				bool flag = !base.IsReadOnly && base.SelectionLength > 0;
				if (flag)
				{
					int endPosition = (base.SelectionLength > 0) ? (base.SelectionStart + base.SelectionLength - 1) : base.SelectionStart;
					MaskedTextProvider maskedTextProvider = m_maskedTextProvider.Clone() as MaskedTextProvider;
					flag = maskedTextProvider.RemoveAt(base.SelectionStart, endPosition);
				}
				e.CanExecute = flag;
				e.Handled = true;
				if (!flag && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				bool canExecute = false;
				if (!base.IsReadOnly)
				{
					string empty = string.Empty;
					try
					{
						empty = (string)Clipboard.GetDataObject().GetData("System.String");
						if (empty != null)
						{
							MaskedTextProvider provider = (MaskedTextProvider)m_maskedTextProvider.Clone();
							int tentativeCaretIndex;
							canExecute = CanReplace(provider, empty, base.SelectionStart, base.SelectionLength, RejectInputOnFirstFailure, out tentativeCaretIndex);
						}
					}
					catch
					{
					}
				}
				e.CanExecute = canExecute;
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void CanExecuteCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.CanExecute = !m_maskedTextProvider.IsPassword;
				e.Handled = true;
				if (!e.CanExecute && base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
			}
		}

		private void ExecuteCopy()
		{
			string selectedText = GetSelectedText();
			try
			{
				new UIPermission(UIPermissionClipboard.AllClipboard).Demand();
				if (selectedText.Length == 0)
				{
					Clipboard.Clear();
				}
				else
				{
					Clipboard.SetText(selectedText);
				}
			}
			catch (SecurityException)
			{
			}
		}

		private void ToggleInsertExecutedCallback(object sender, ExecutedRoutedEventArgs e)
		{
			m_insertToggled = !m_insertToggled;
		}

		private void PreviewQueryContinueDragCallback(object sender, QueryContinueDragEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.Action = DragAction.Cancel;
				e.Handled = true;
			}
		}

		/// <summary>Called when an unhandled <strong>DragEnter</strong> attached routed event reaches this element.</summary>
		protected override void OnDragEnter(DragEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
			}
			base.OnDragEnter(e);
		}

		/// <summary>Called when an unhandled <strong>DragOver</strong> attached routed event reaches this element.</summary>
		/// <param name="e">Event data.</param>
		protected override void OnDragOver(DragEventArgs e)
		{
			if (!m_maskIsNull)
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
			}
			base.OnDragOver(e);
		}

		protected override bool QueryValueFromTextCore(string text, out object value)
		{
			Type valueDataType = base.ValueDataType;
			if (valueDataType != null && m_unhandledLiteralsPositions != null && m_unhandledLiteralsPositions.Count > 0)
			{
				text = m_maskedTextProvider.ToString(false, false, true, 0, m_maskedTextProvider.Length);
				for (int num = m_unhandledLiteralsPositions.Count - 1; num >= 0; num--)
				{
					text = text.Remove(m_unhandledLiteralsPositions[num], 1);
				}
			}
			return base.QueryValueFromTextCore(text, out value);
		}

		protected override string QueryTextFromValueCore(object value)
		{
			if (m_valueToStringMethodInfo != null && value != null)
			{
				try
				{
					return (string)m_valueToStringMethodInfo.Invoke(value, new object[2]
					{
						m_formatSpecifier,
						GetActiveFormatProvider()
					});
				}
				catch
				{
				}
			}
			return base.QueryTextFromValueCore(value);
		}

		/// <summary>Retrieves the mask characters that are supported by the masked text box.</summary>
		/// <returns>An array of characters containing the mask characters that are supported by the masked text box (see Remarks).</returns>
		protected virtual char[] GetMaskCharacters()
		{
			return MaskChars;
		}

		private MaskedTextProvider CreateMaskedTextProvider(string mask)
		{
			return CreateMaskedTextProvider(mask, GetCultureInfo(), AllowPromptAsInput, PromptChar, DefaultPasswordChar, RestrictToAscii);
		}

		/// <summary>Creates the masked-text provider that will be used by the masked text box specifying various settings.</summary>
		/// <param name="mask">The string that will be used as the input mask.</param>
		/// <param name="cultureInfo">The CultureInfo that will be used by the masked-text provider.</param>
		/// <param name="allowPromptAsInput">true if the prompt char should be considered as a valid input character; false, otherwise. By default, true.</param>
		/// <param name="promptChar">A character that represents the positions in the masked text box that require input. By default, "_"? (underscore).</param>
		/// <param name="passwordChar">A character that represents the character to used to mask a password.</param>
		/// <param name="restrictToAscii">true if non-ASCII characters are accepted; false otherwise. By default, false.</param>
		protected virtual MaskedTextProvider CreateMaskedTextProvider(string mask, CultureInfo cultureInfo, bool allowPromptAsInput, char promptChar, char passwordChar, bool restrictToAscii)
		{
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider(mask, cultureInfo, allowPromptAsInput, promptChar, passwordChar, restrictToAscii);
			maskedTextProvider.ResetOnPrompt = ResetOnPrompt;
			maskedTextProvider.ResetOnSpace = ResetOnSpace;
			maskedTextProvider.SkipLiterals = SkipLiterals;
			maskedTextProvider.IncludeLiterals = true;
			maskedTextProvider.IncludePrompt = true;
			maskedTextProvider.IsPassword = false;
			return maskedTextProvider;
		}

		internal override void OnIMECompositionEnded(CachedTextInfo cachedTextInfo)
		{
			ForceText(cachedTextInfo.Text, false);
			base.CaretIndex = cachedTextInfo.CaretIndex;
			base.SelectionStart = cachedTextInfo.SelectionStart;
			base.SelectionLength = cachedTextInfo.SelectionLength;
		}

		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			if (base.IsInIMEComposition)
			{
				EndIMEComposition();
			}
			if (m_maskIsNull || m_maskedTextProvider == null || base.IsReadOnly)
			{
				base.OnTextInput(e);
			}
			else
			{
				e.Handled = true;
				if (base.CharacterCasing == CharacterCasing.Upper)
				{
					ProcessTextInput(e.Text.ToUpper());
				}
				else if (base.CharacterCasing == CharacterCasing.Lower)
				{
					ProcessTextInput(e.Text.ToLower());
				}
				else
				{
					ProcessTextInput(e.Text);
				}
				base.OnTextInput(e);
			}
		}

		private void ProcessTextInput(string text)
		{
			if (text.Length == 1)
			{
				string maskedTextOutput = MaskedTextOutput;
				int caretIndex;
				if (PlaceChar(text[0], base.SelectionStart, base.SelectionLength, IsOverwriteMode, out caretIndex))
				{
					if (MaskedTextOutput != maskedTextOutput)
					{
						RefreshCurrentText(false);
					}
					base.SelectionStart = caretIndex + 1;
				}
				else if (base.BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
				if (base.SelectionLength > 0)
				{
					base.SelectionLength = 0;
				}
			}
			else
			{
				Replace(text, base.SelectionStart, base.SelectionLength);
			}
		}

		/// <summary>Validates the specified value to ensure that it fits in the mask specified by the Mask property.</summary>
		protected override void ValidateValue(object value)
		{
			base.ValidateValue(value);
			if (!m_maskIsNull)
			{
				string textFromValue = GetTextFromValue(value);
				MaskedTextProvider maskedTextProvider = m_maskedTextProvider.Clone() as MaskedTextProvider;
				if (!maskedTextProvider.VerifyString(textFromValue))
				{
					throw new ArgumentException("The value representation '" + textFromValue + "' does not match the mask.", "value");
				}
			}
		}

		internal override bool GetIsEditTextEmpty()
		{
			if (!m_maskIsNull)
			{
				return MaskedTextProvider.AssignedEditPositionCount == 0;
			}
			return true;
		}

		internal override string GetCurrentText()
		{
			if (m_maskIsNull)
			{
				return base.GetCurrentText();
			}
			return GetFormattedString(m_maskedTextProvider, base.Text);
		}

		internal override string GetParsableText()
		{
			if (m_maskIsNull)
			{
				return base.GetParsableText();
			}
			bool includePrompt = false;
			bool includeLiterals = true;
			if (base.ValueDataType == typeof(string))
			{
				includePrompt = IncludePromptInValue;
				includeLiterals = IncludeLiteralsInValue;
			}
			return m_maskedTextProvider.ToString(false, includePrompt, includeLiterals, 0, m_maskedTextProvider.Length);
		}

		internal override void OnFormatProviderChanged()
		{
			MaskedTextProvider maskedTextProvider = m_maskedTextProvider = new MaskedTextProvider(Mask);
			RefreshConversionHelpers();
			RefreshCurrentText(true);
			base.OnFormatProviderChanged();
		}

		internal override void RefreshConversionHelpers()
		{
			Type valueDataType = base.ValueDataType;
			if (valueDataType == null || !base.IsNumericValueDataType)
			{
				m_formatSpecifier = null;
				m_valueToStringMethodInfo = null;
				m_unhandledLiteralsPositions = null;
			}
			else
			{
				m_valueToStringMethodInfo = valueDataType.GetMethod("ToString", new Type[2]
				{
					typeof(string),
					typeof(IFormatProvider)
				});
				string mask = m_maskedTextProvider.Mask;
				IFormatProvider activeFormatProvider = GetActiveFormatProvider();
				char[] maskCharacters = GetMaskCharacters();
				List<int> unhandledLiteralsPositions;
				m_formatSpecifier = GetFormatSpecifierFromMask(mask, maskCharacters, activeFormatProvider, IncludeLiteralsInValue, out unhandledLiteralsPositions);
				NumberFormatInfo numberFormatInfo = activeFormatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
				if (numberFormatInfo != null)
				{
					string negativeSign = numberFormatInfo.NegativeSign;
					if (m_formatSpecifier.Contains(negativeSign))
					{
						m_formatSpecifier = m_formatSpecifier + ";" + m_formatSpecifier + ";" + m_formatSpecifier;
					}
				}
				m_unhandledLiteralsPositions = unhandledLiteralsPositions;
			}
		}

		internal void SetValueToStringMethodInfo(MethodInfo valueToStringMethodInfo)
		{
			m_valueToStringMethodInfo = valueToStringMethodInfo;
		}

		internal void ForceMask(string mask)
		{
			m_forcingMask = true;
			try
			{
				Mask = mask;
			}
			finally
			{
				m_forcingMask = false;
			}
		}

		private bool PlaceChar(char ch, int startPosition, int length, bool overwrite, out int caretIndex)
		{
			return PlaceChar(m_maskedTextProvider, ch, startPosition, length, overwrite, out caretIndex);
		}

		private bool PlaceChar(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite, out int caretPosition)
		{
			if (ShouldQueryAutoCompleteMask(provider.Clone() as MaskedTextProvider, ch, startPosition))
			{
				AutoCompletingMaskEventArgs autoCompletingMaskEventArgs = new AutoCompletingMaskEventArgs(m_maskedTextProvider.Clone() as MaskedTextProvider, startPosition, length, ch.ToString());
				OnAutoCompletingMask(autoCompletingMaskEventArgs);
				if (!autoCompletingMaskEventArgs.Cancel && autoCompletingMaskEventArgs.AutoCompleteStartPosition > -1)
				{
					caretPosition = startPosition;
					for (int i = 0; i < autoCompletingMaskEventArgs.AutoCompleteText.Length; i++)
					{
						if (!PlaceCharCore(provider, autoCompletingMaskEventArgs.AutoCompleteText[i], autoCompletingMaskEventArgs.AutoCompleteStartPosition + i, 0, true, out caretPosition))
						{
							return false;
						}
					}
					caretPosition = autoCompletingMaskEventArgs.AutoCompleteStartPosition + autoCompletingMaskEventArgs.AutoCompleteText.Length;
					return true;
				}
			}
			return PlaceCharCore(provider, ch, startPosition, length, overwrite, out caretPosition);
		}

		private bool ShouldQueryAutoCompleteMask(MaskedTextProvider provider, char ch, int startPosition)
		{
			if (provider.IsEditPosition(startPosition))
			{
				int num = provider.FindNonEditPositionFrom(startPosition, true);
				if (num != -1 && provider[num].Equals(ch))
				{
					int startPosition2 = provider.FindNonEditPositionFrom(startPosition, false);
					if (provider.FindUnassignedEditPositionInRange(startPosition2, num, true) != -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Raises the AutoCompletingMask event.</summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnAutoCompletingMask(AutoCompletingMaskEventArgs e)
		{
			if (this.AutoCompletingMask != null)
			{
				this.AutoCompletingMask(this, e);
			}
		}

		private bool PlaceCharCore(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite, out int caretPosition)
		{
			caretPosition = startPosition;
			if (startPosition < m_maskedTextProvider.Length)
			{
				MaskedTextResultHint resultHint;
				if (length > 0)
				{
					int endPosition = startPosition + length - 1;
					return provider.Replace(ch, startPosition, endPosition, out caretPosition, out resultHint);
				}
				if (overwrite)
				{
					return provider.Replace(ch, startPosition, out caretPosition, out resultHint);
				}
				return provider.InsertAt(ch, startPosition, out caretPosition, out resultHint);
			}
			return false;
		}

		internal void Replace(string text, int startPosition, int selectionLength)
		{
			MaskedTextProvider maskedTextProvider = (MaskedTextProvider)m_maskedTextProvider.Clone();
			int tentativeCaretIndex;
			if (CanReplace(maskedTextProvider, text, startPosition, selectionLength, RejectInputOnFirstFailure, out tentativeCaretIndex))
			{
				bool flag = MaskedTextOutput != maskedTextProvider.ToString();
				m_maskedTextProvider = maskedTextProvider;
				if (flag)
				{
					RefreshCurrentText(false);
				}
				base.CaretIndex = tentativeCaretIndex + 1;
			}
			else if (base.BeepOnError)
			{
				SystemSounds.Beep.Play();
			}
		}

		internal virtual bool CanReplace(MaskedTextProvider provider, string text, int startPosition, int selectionLength, bool rejectInputOnFirstFailure, out int tentativeCaretIndex)
		{
			int num = startPosition + selectionLength - 1;
			tentativeCaretIndex = -1;
			bool result = false;
			foreach (char c in text)
			{
				if (!m_maskedTextProvider.VerifyEscapeChar(c, startPosition))
				{
					int num2 = provider.FindEditPositionFrom(startPosition, true);
					if (num2 == MaskedTextProvider.InvalidIndex)
					{
						break;
					}
					startPosition = num2;
				}
				int num3 = (num >= startPosition) ? 1 : 0;
				bool overwrite = num3 > 0;
				if (PlaceChar(provider, c, startPosition, num3, overwrite, out tentativeCaretIndex))
				{
					result = true;
					startPosition = tentativeCaretIndex + 1;
				}
				else if (rejectInputOnFirstFailure)
				{
					return false;
				}
			}
			int testPosition;
			MaskedTextResultHint resultHint;
			if (selectionLength > 0 && startPosition <= num && !provider.RemoveAt(startPosition, num, out testPosition, out resultHint))
			{
				result = false;
			}
			return result;
		}

		private bool CanDelete(int startPosition, int selectionLength, bool deleteForward, MaskedTextProvider provider)
		{
			if (base.IsReadOnly)
			{
				return false;
			}
			if (selectionLength == 0)
			{
				if (!deleteForward)
				{
					if (startPosition == 0)
					{
						return false;
					}
					startPosition--;
				}
				else if (startPosition + selectionLength == provider.Length)
				{
					return false;
				}
			}
			int testPosition = startPosition;
			int endPosition = (selectionLength > 0) ? (startPosition + selectionLength - 1) : startPosition;
			MaskedTextResultHint resultHint;
			return provider.RemoveAt(startPosition, endPosition, out testPosition, out resultHint);
		}

		private void Delete(int startPosition, int selectionLength, bool deleteForward)
		{
			if (!base.IsReadOnly)
			{
				if (selectionLength == 0)
				{
					if (!deleteForward)
					{
						if (startPosition == 0)
						{
							return;
						}
						startPosition--;
					}
					else if (startPosition + selectionLength == m_maskedTextProvider.Length)
					{
						return;
					}
				}
				int testPosition = startPosition;
				int endPosition = (selectionLength > 0) ? (startPosition + selectionLength - 1) : startPosition;
				string maskedTextOutput = MaskedTextOutput;
				MaskedTextResultHint resultHint;
				if (!m_maskedTextProvider.RemoveAt(startPosition, endPosition, out testPosition, out resultHint))
				{
					if (base.BeepOnError)
					{
						SystemSounds.Beep.Play();
					}
				}
				else
				{
					if (MaskedTextOutput != maskedTextOutput)
					{
						RefreshCurrentText(false);
					}
					else if (selectionLength > 0)
					{
						testPosition = startPosition;
					}
					else if (resultHint == MaskedTextResultHint.NoEffect)
					{
						if (deleteForward)
						{
							testPosition = m_maskedTextProvider.FindEditPositionFrom(startPosition, true);
						}
						else
						{
							testPosition = ((m_maskedTextProvider.FindAssignedEditPositionFrom(startPosition, true) != MaskedTextProvider.InvalidIndex) ? m_maskedTextProvider.FindEditPositionFrom(startPosition, false) : m_maskedTextProvider.FindAssignedEditPositionFrom(startPosition, false));
							if (testPosition != MaskedTextProvider.InvalidIndex)
							{
								testPosition++;
							}
						}
						if (testPosition == MaskedTextProvider.InvalidIndex)
						{
							testPosition = startPosition;
						}
					}
					else if (!deleteForward)
					{
						testPosition = startPosition;
					}
					base.CaretIndex = testPosition;
				}
			}
		}

		private string GetRawText()
		{
			if (m_maskIsNull)
			{
				return base.Text;
			}
			return GetRawText(m_maskedTextProvider);
		}

		private string GetFormattedString(MaskedTextProvider provider, string text)
		{
			bool includePrompt = !HidePromptOnLeave || base.IsFocused;
			string text2 = provider.ToString(false, includePrompt, true, 0, m_maskedTextProvider.Length);
			if (provider.Mask.StartsWith(">"))
			{
				return text2.ToUpper();
			}
			if (provider.Mask.StartsWith("<"))
			{
				return text2.ToLower();
			}
			return text2;
		}

		private string GetSelectedText()
		{
			int selectionLength = base.SelectionLength;
			if (selectionLength == 0)
			{
				return string.Empty;
			}
			bool includePrompt = (ClipboardMaskFormat & MaskFormat.IncludePrompt) != MaskFormat.ExcludePromptAndLiterals;
			bool includeLiterals = (ClipboardMaskFormat & MaskFormat.IncludeLiterals) != MaskFormat.ExcludePromptAndLiterals;
			return m_maskedTextProvider.ToString(true, includePrompt, includeLiterals, base.SelectionStart, selectionLength);
		}
	}
}
