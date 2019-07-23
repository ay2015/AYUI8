using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using ay.UIAutomation;
using ay.Controls.Args;
using ay.Controls.Enums;

namespace ay.Controls
{
    /// <summary>
    /// 基本文本框
    /// </summary>
    public class AyTextBoxBase : TextBox, IAyControl, IAyValidate, IAyHighlight
    {
        public string ControlID { get { return ControlGUID.AyTextBoxBase; } }

        /// <summary>文本框单击时候默认行为</summary>
        public static readonly DependencyProperty BoxSelectBehaviorProperty = DependencyProperty.Register("BoxSelectBehavior", typeof(AutoSelectBehavior), typeof(AyTextBoxBase), new UIPropertyMetadata(AutoSelectBehavior.Default));
        /// <summary>
        /// 是否错误
        /// </summary>
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register("IsError", typeof(bool), typeof(AyTextBoxBase), new UIPropertyMetadata(false));

        /// <summary>是否上下左右移动到下一个光标，默认false</summary>
        public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register("AutoMoveFocus", typeof(bool), typeof(AyTextBoxBase), new UIPropertyMetadata(false));

        /// <summary>焦点移动到下一个会触发</summary>
        public static readonly RoutedEvent QueryMoveFocusEvent = EventManager.RegisterRoutedEvent("QueryMoveFocus", RoutingStrategy.Bubble, typeof(QueryMoveFocusEventHandler), typeof(AyTextBoxBase));

        public bool IsError
        {
            get
            {
                return (bool)GetValue(IsErrorProperty);
            }
            set
            {
                SetValue(IsErrorProperty, value);
            }
        }

        /// <summary>文本框单击时候默认行为</summary>
        public AutoSelectBehavior AutoSelected
        {
            get
            {
                return (AutoSelectBehavior)GetValue(BoxSelectBehaviorProperty);
            }
            set
            {
                SetValue(BoxSelectBehaviorProperty, value);
            }
        }

        /// <summary>是否上下左右移动到下一个光标，默认false</summary>
        public bool AutoMoveFocus
        {
            get
            {
                return (bool)GetValue(AutoMoveFocusProperty);
            }
            set
            {
                SetValue(AutoMoveFocusProperty, value);
            }
        }
        #region 是否有文本
        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
      "HasText",
      typeof(bool),
      typeof(AyTextBoxBase),
      new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public bool HasText
        {
            get
            {
                return (bool)GetValue(HasTextProperty);
            }
        }

        #endregion
        #region 高亮

        /// <summary>
        /// 是否高亮，默认false
        /// </summary>
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(AyTextBoxBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnIsHighlightChanged)));

        private static void OnIsHighlightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyTextBox).SetOnIsHighlightChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        //高亮以后，必须先获得焦点，然后失去焦点，才能自动IsHighlight=false
        bool isOpenHighlightThenFocus = false;
        public void SetOnIsHighlightChanged(bool oldv, bool newv)
        {
            if (newv)
            {
                this.GotKeyboardFocus -= Box_Highlight_GotKeyboardFocus;
                this.GotKeyboardFocus += Box_Highlight_GotKeyboardFocus;
                this.LostKeyboardFocus -= Box_Highlight_LostKeyboardFocus;
                this.LostKeyboardFocus += Box_Highlight_LostKeyboardFocus;
            }
            else
            {
                this.GotKeyboardFocus -= Box_Highlight_GotKeyboardFocus;
                this.LostKeyboardFocus -= Box_Highlight_LostKeyboardFocus;
            }
        }

        private void Box_Highlight_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (isOpenHighlightThenFocus)
            {
                IsHighlight = false;
                isOpenHighlightThenFocus = false;
            }
        }

        private void Box_Highlight_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (IsHighlight)
            {
                isOpenHighlightThenFocus = true;
            }
        }
        #endregion

        /// <summary>初始化</summary>
        public AyTextBoxBase()
        {

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!AutoMoveFocus)
            {
                base.OnPreviewKeyDown(e);
            }
            else
            {
                if (e.Key == Key.Left && (Keyboard.Modifiers == ModifierKeys.None || Keyboard.Modifiers == ModifierKeys.Control))
                {
                    e.Handled = MoveFocusLeft();
                }
                if (e.Key == Key.Right && (Keyboard.Modifiers == ModifierKeys.None || Keyboard.Modifiers == ModifierKeys.Control))
                {
                    e.Handled = MoveFocusRight();
                }
                if ((e.Key == Key.Up || e.Key == Key.Prior) && (Keyboard.Modifiers == ModifierKeys.None || Keyboard.Modifiers == ModifierKeys.Control))
                {
                    e.Handled = MoveFocusUp();
                }
                if ((e.Key == Key.Down || e.Key == Key.Next) && (Keyboard.Modifiers == ModifierKeys.None || Keyboard.Modifiers == ModifierKeys.Control))
                {
                    e.Handled = MoveFocusDown();
                }
                base.OnPreviewKeyDown(e);
            }
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            if (AutoSelected == AutoSelectBehavior.OnFocus && !TreeHelper.IsDescendantOf(e.OldFocus as DependencyObject, this))
            {
                SelectAll();
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (AutoSelected != 0 && !base.IsKeyboardFocusWithin)
            {
                Focus();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (AutoMoveFocus && base.Text.Length != 0 && base.Text.Length == base.MaxLength && base.CaretIndex == base.MaxLength && CanMoveFocus(FocusNavigationDirection.Right, true))
            {
                FocusNavigationDirection focusNavigationDirection = (base.FlowDirection == FlowDirection.LeftToRight) ? FocusNavigationDirection.Right : FocusNavigationDirection.Left;
                MoveFocus(new TraversalRequest(focusNavigationDirection));
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new UIAutomation.TextBoxAutomationPeer(this);
        }

        private bool CanMoveFocus(FocusNavigationDirection direction, bool reachedMax)
        {
            QueryMoveFocusEventArgs queryMoveFocusEventArgs = new QueryMoveFocusEventArgs(direction, reachedMax);
            RaiseEvent(queryMoveFocusEventArgs);
            return queryMoveFocusEventArgs.CanMoveFocus;
        }

        private bool MoveFocusLeft()
        {
            if (base.FlowDirection == FlowDirection.LeftToRight)
            {
                if (base.CaretIndex == 0 && base.SelectionLength == 0)
                {
                    if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusBack.Execute(null, this);
                        return true;
                    }
                    if (CanMoveFocus(FocusNavigationDirection.Left, false))
                    {
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                        return true;
                    }
                }
            }
            else if (base.CaretIndex == base.Text.Length && base.SelectionLength == 0)
            {
                if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusBack.Execute(null, this);
                    return true;
                }
                if (CanMoveFocus(FocusNavigationDirection.Left, false))
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                    return true;
                }
            }
            return false;
        }

        private bool MoveFocusRight()
        {
            if (base.FlowDirection == FlowDirection.LeftToRight)
            {
                if (base.CaretIndex == base.Text.Length && base.SelectionLength == 0)
                {
                    if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusForward.Execute(null, this);
                        return true;
                    }
                    if (CanMoveFocus(FocusNavigationDirection.Right, false))
                    {
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                        return true;
                    }
                }
            }
            else if (base.CaretIndex == 0 && base.SelectionLength == 0)
            {
                if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusForward.Execute(null, this);
                    return true;
                }
                if (CanMoveFocus(FocusNavigationDirection.Right, false))
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                    return true;
                }
            }
            return false;
        }

        private bool MoveFocusUp()
        {
            if (GetLineIndexFromCharacterIndex(base.SelectionStart) == 0)
            {
                if (ComponentCommands.MoveFocusUp.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusUp.Execute(null, this);
                    return true;
                }
                if (CanMoveFocus(FocusNavigationDirection.Up, false))
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                    return true;
                }
            }
            return false;
        }

        private bool MoveFocusDown()
        {
            int lineIndexFromCharacterIndex = GetLineIndexFromCharacterIndex(base.SelectionStart);
            if (lineIndexFromCharacterIndex == base.LineCount - 1)
            {
                if (ComponentCommands.MoveFocusDown.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusDown.Execute(null, this);
                    return true;
                }
                if (CanMoveFocus(FocusNavigationDirection.Down, false))
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    return true;
                }
            }
            return false;
        }

        public virtual bool Validate()
        {
            throw new System.NotImplementedException();
        }

        public virtual void HighlightElement()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool ValidateButNotShowError()
        {
            throw new System.NotImplementedException();
        }

        public virtual void ShowError()
        {
            throw new System.NotImplementedException();
        }
    }
}
