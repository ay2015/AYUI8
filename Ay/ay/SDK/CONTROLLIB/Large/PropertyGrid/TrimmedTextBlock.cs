using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class TrimmedTextBlock : TextBlock
	{
		/// <summary>Identifies the IsTextTrimmed dependency property.</summary>
		public static readonly DependencyProperty IsTextTrimmedProperty = DependencyProperty.Register("IsTextTrimmed", typeof(bool), typeof(TrimmedTextBlock), new PropertyMetadata(false, OnIsTextTrimmedChanged));

		/// <summary>Identifies the HighlightedBrush dependency property.</summary>
		public static readonly DependencyProperty HighlightedBrushProperty = DependencyProperty.Register("HighlightedBrush", typeof(Brush), typeof(TrimmedTextBlock), new FrameworkPropertyMetadata(Brushes.Yellow));

		/// <summary>Identifies the HighlightedText dependency property.</summary>
		public static readonly DependencyProperty HighlightedTextProperty = DependencyProperty.Register("HighlightedText", typeof(string), typeof(TrimmedTextBlock), new FrameworkPropertyMetadata(null, HighlightedTextChanged));

		public bool IsTextTrimmed
		{
			get
			{
				return (bool)GetValue(IsTextTrimmedProperty);
			}
			private set
			{
				SetValue(IsTextTrimmedProperty, value);
			}
		}

		public Brush HighlightedBrush
		{
			get
			{
				return (Brush)GetValue(HighlightedBrushProperty);
			}
			set
			{
				SetValue(HighlightedBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the text part to highlight.</summary>
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

		public TrimmedTextBlock()
		{
			base.SizeChanged += TrimmedTextBlock_SizeChanged;
		}

		private static void OnIsTextTrimmedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TrimmedTextBlock trimmedTextBlock = d as TrimmedTextBlock;
			if (trimmedTextBlock != null)
			{
				trimmedTextBlock.OnIsTextTrimmedChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		private void OnIsTextTrimmedChanged(bool oldValue, bool newValue)
		{
			base.ToolTip = (newValue ? base.Text : null);
		}

		private static void HighlightedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TrimmedTextBlock trimmedTextBlock = sender as TrimmedTextBlock;
			if (trimmedTextBlock != null)
			{
				trimmedTextBlock.HighlightedTextChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		protected virtual void HighlightedTextChanged(string oldValue, string newValue)
		{
			if (base.Text.Length != 0)
			{
				if (newValue == null)
				{
					Run item = new Run(base.Text);
					base.Inlines.Clear();
					base.Inlines.Add(item);
				}
				else
				{
					int num = base.Text.IndexOf(newValue, StringComparison.InvariantCultureIgnoreCase);
					int num2 = num + newValue.Length;
					string text = base.Text.Substring(0, num);
					string text2 = base.Text.Substring(num, newValue.Length);
					string text3 = base.Text.Substring(num2, base.Text.Length - num2);
					base.Inlines.Clear();
					Run item2 = new Run(text);
					base.Inlines.Add(item2);
					item2 = new Run(text2);
					item2.Background = HighlightedBrush;
					base.Inlines.Add(item2);
					item2 = new Run(text3);
					base.Inlines.Add(item2);
				}
			}
		}

		private void TrimmedTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			TextBlock textBlock = sender as TextBlock;
			if (textBlock != null)
			{
				IsTextTrimmed = GetIsTextTrimmed(textBlock);
			}
		}

		private bool GetIsTextTrimmed(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return false;
			}
			if (textBlock.TextTrimming == TextTrimming.None)
			{
				return false;
			}
			if (textBlock.TextWrapping != TextWrapping.NoWrap)
			{
				return false;
			}
			double actualWidth = textBlock.ActualWidth;
			textBlock.Measure(new Size(1.7976931348623157E+308, 1.7976931348623157E+308));
			double width = textBlock.DesiredSize.Width;
			return actualWidth < width;
		}
	}
}
