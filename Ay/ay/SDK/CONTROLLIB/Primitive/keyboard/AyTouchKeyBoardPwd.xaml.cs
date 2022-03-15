using ay.Controls;
using ay.FuncFactory;
using Ay.Framework.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ay.Controls
{
    /// <summary>
    /// AyTouchKeyBoardPwd.xaml 的交互逻辑
    /// </summary>
    public partial class AyTouchKeyBoardPwd : UserControl
    {
        public AyTouchKeyBoardPwd()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserControl_Loaded;
            if (!WpfTreeHelper.IsInDesignMode)
            {
                var _1 = this.GetLogicalAncestor<Popup>();
                if (_1.IsNotNull())
                {
                    _1.Opened += _1_Opened;
                }
                //this.IsVisibleChanged += AyTouchKeyBoardPwd_IsVisibleChanged;
                var window = Application.Current.MainWindow;
                window.KeyDown -= UserControl_KeyDown;
                window.KeyDown += UserControl_KeyDown;
                window.KeyUp -= UserControl_KeyUp;
                window.KeyUp += UserControl_KeyUp;
                //if (b1 == null)
                //{
                //    b1 = SolidColorBrushConverter.From16JinZhi("#000000");
                //}

                //if (b2 == null)
                //{
                //    b2 = SolidColorBrushConverter.From16JinZhi("#888888");
                //}

            }



        }

        private void _1_Opened(object sender, EventArgs e)
        {
            ShowPP(); 
        }

        //  [DllImport("user32.dll")]
        //public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
    /// <summary>
    /// SQHX手写输入法
    /// </summary>
    public static void HandInputMethods()
        {
            Process[] ravProcesses = Process.GetProcessesByName("handinput");
            //if (ravProcesses.Length == 1)
            //{
            //    IntPtr handle = ravProcesses[0].MainWindowHandle;
            //    SwitchToThisWindow(handle, true); // 激活，显示在最前
            //}
            //else
            //{
                foreach (Process p in ravProcesses)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch
                    {

                    }

                }
                Process.Start(HandInput);
            //}
        
        }



        /// <summary>
        /// 关闭SQHX手写输入法
        /// </summary>
        public static void CloseHandInputMethods()
        {
            Process[] ravProcesses = Process.GetProcessesByName("handinput");
            foreach (Process p in ravProcesses)
            {
                p.Kill();
            }
        }
        private static string _HandInput;
        public static string HandInput
        {
            get
            {
                if (_HandInput == null)
                {
                    _HandInput = AyGlobalConfig.ReturnCurrentFolderCombinePath2("Content\\handinput\\handinput.exe");
                }
                return _HandInput;
            }
        }

        private void AyTouchKeyBoardPwd_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool ff = (bool)e.NewValue;
            if (ff)
            {
                ShowPP();

            }


        }

        public void ShowPP()
        {
            if (Keyboard.IsKeyToggled(Key.CapsLock) == true)
            {

                btnCapsLock.IsChecked = true;
            }
            else if (btnCapsLock.IsChecked == true)
            {

                isTriggerLockChecked = false;
                btnCapsLock.IsChecked = false;
            }
            if (ElementName != null)
            {
                ElementName.Focus();
                //CloseHandInputMethods();
                if (ElementName.IsContainChineseKeyboard)
                {
                    HandInputMethods();
                    this.layay.Height = 0;
                    //allkey.Visibility = Visibility.Collapsed;
                    //numkey.Visibility = Visibility.Collapsed;
                    //IdCardkey.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    if (ElementName.IsIDCard)
                    {

                        this.layay.Height = 200;
                        allkey.Visibility = Visibility.Collapsed;
                        numkey.Visibility = Visibility.Collapsed;
                        IdCardkey.Visibility = Visibility.Visible;
                    }
                    else if (ElementName.IsIntegerBox)
                    {
                        this.layay.Height = 200;
                        allkey.Visibility = Visibility.Collapsed;
                        numkey.Visibility = Visibility.Visible;
                        IdCardkey.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (InputMethod.GetIsInputMethodEnabled(ElementName))
                        {
                            this.layay.Height = 200;
                            allkey.Visibility = Visibility.Visible;
                            numkey.Visibility = Visibility.Collapsed;
                            IdCardkey.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            //全键盘  拓展
                            this.layay.Height = 200;
                            allkey.Visibility = Visibility.Visible;
                            numkey.Visibility = Visibility.Collapsed;
                            IdCardkey.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                //ElementName.CaretIndex = ElementName.Text.Length;
            }
        }

        public AyFormInput ElementName
        {
            get { return (AyFormInput)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(AyFormInput), typeof(AyTouchKeyBoardPwd), new PropertyMetadata(null));



        int uppercase = 1;//默认小写字母
        bool shiftIsChecked = false;
        //Brush b1 = null;
        //Brush b2 = null;
        private void btnshift_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in fd.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = true;
                }
            }
            foreach (var item in char0.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = true;
                }
            }
            foreach (var item in char1.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = true;
                }
            }
            foreach (var item in char2.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = true;
                }
            }
            if (ElementName != null)
                ElementName.Focus();
            if (uppercase == 1)
            {
                ToUpperCase();
                uppercase = 2;
            }
            else
            {
                ToLowCase();
                uppercase = 1;
            }
            shiftIsChecked = true;
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private void btnshift_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in fd.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = false;
                }
            }
            foreach (var item in char0.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = false;
                }
            }
            foreach (var item in char1.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = false;
                }
            }
            foreach (var item in char2.Children)
            {
                var _1 = item as DoubleCharButton;
                if (_1.IsNotNull())
                {
                    _1.IsSwitched = false;
                }
            }

            if (ElementName != null)
                ElementName.Focus();
            if (uppercase == 1)
            {
                ToUpperCase();
                uppercase = 2;
            }
            else
            {
                ToLowCase();
                uppercase = 1;
            }
            shiftIsChecked = false;
        }

        private void ToLowCase()
        {
            foreach (var item in char0.Children)
            {
                ChangedLower(item);

            }
            foreach (var item in char1.Children)
            {
                ChangedLower(item);
            }
            foreach (var item in char2.Children)
            {
                ChangedLower(item);
            }

        }
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;
        private void btnCapsLock_Checked(object sender, RoutedEventArgs e)
        {
            if (ElementName != null)
                ElementName.Focus();
            if (uppercase == 1)
            {
                ToUpperCase();
                uppercase = 2;
            }
            else
            {
                ToLowCase();
                uppercase = 1;
            }

            if (isTriggerLockChecked)
            {
                keybd_event(20, 0, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(20, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            isTriggerLockChecked = true;
        }

        private void ToUpperCase()
        {
            foreach (var item in char0.Children)
            {
                ChangedUpper(item);

            }
            foreach (var item in char1.Children)
            {
                ChangedUpper(item);

            }
            foreach (var item in char2.Children)
            {
                ChangedUpper(item);
            }
        }

        private static void ChangedUpper(object item)
        {
            Button btn = item as Button;
            if (btn != null)
            {
                var _1 = btn.Content as string;
                if (_1 != null)
                {
                    btn.Content = _1.ToUpper();
                }

            }
        }
        private static void ChangedLower(object item)
        {
            Button btn = item as Button;
            if (btn != null)
            {
                var _1 = btn.Content as string;
                if (_1 != null)
                {
                    btn.Content = _1.ToLower();
                }

            }
        }
        bool isTriggerLockChecked = true;
        private void btnCapsLock_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ElementName != null)
                ElementName.Focus();
            if (uppercase == 1)
            {
                ToUpperCase();
                uppercase = 2;
            }
            else
            {
                ToLowCase();
                uppercase = 1;
            }

            if (isTriggerLockChecked)
            {
                keybd_event(20, 0, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(20, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            isTriggerLockChecked = true;
        }

        private void btnTwo_Click(object sender, RoutedEventArgs e)
        {
            if (ElementName != null)
                ElementName.Focus();
            DoubleCharButton btntwo = sender as DoubleCharButton;
            if (btntwo != null)
            {
                //Canvas btnC = btntwo.Content as Canvas;
                //Label uncheckChar = btnC.Children[0] as Label;
                //Label checkChar = btnC.Children[1] as Label;
                //if (shiftIsChecked)
                //{
                InputCharacter(btntwo.CurrentText);
                btnshift.IsChecked = false;
                //}
                //else
                //{
                //    InputCharacter(uncheckChar.Content.ToString());
                //}
            }
        }

        private void btnOne_Click(object sender, RoutedEventArgs e)
        {
            Button btnOne = sender as Button;
            if (btnOne != null)
            {
                InputCharacter(btnOne.Content.ToString());
            }
            if (shiftIsChecked)
            {
                btnshift.IsChecked = false;
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                btnshift.IsChecked = true;
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.LeftShift || e.Key == Key.RightShift))
            {
                btnshift.IsChecked = false;
            }
            if (e.Key == Key.CapsLock & Keyboard.IsKeyToggled(Key.CapsLock) == true)
            {
                isTriggerLockChecked = false;
                btnCapsLock.IsChecked = true;

            }
            else if (e.Key == Key.CapsLock & Keyboard.IsKeyToggled(Key.CapsLock) == false)
            {
                isTriggerLockChecked = false;
                btnCapsLock.IsChecked = false;

            }
        }


        public void InputCharacter(string text)
        {
            //Keyboard.Focus(ElementName);
            ElementName.CaretIndex = ElementName.SelectionStart;
            ElementName.Focus();
            if (ElementName.MaxLength != 0 && ElementName.MaxLength <= ElementName.Text.Length)
            {
                return;
            }
            int ad = ElementName.SelectionStart;
            if (ElementName.IsPasswordBox)
            {
                string Pwding = ElementName.Password.Insert(ad, text);

                ElementName.Password = Pwding;
                StringBuilder d = new StringBuilder();
                foreach (var item in Pwding)
                {
                    d.Append(ElementName.PasswordChar);
                }
                ElementName.IsResponseChange = false;
                ElementName.Text = d.ToString();
                ElementName.IsResponseChange = true;
                ElementName.CaretIndex = ad + 1;

            }
            else
            {
                string Pwding = ElementName.Text.Insert(ad, text);
                ElementName.Text = Pwding;
                ElementName.CaretIndex = ad + 1;
            }

        }


        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            ElementName.Focus();
            ElementName.CaretIndex = ElementName.SelectionStart;
            if (ElementName.Text.Length > 0)
            {
                //System.Windows.Forms.SendKeys.SendWait("{BACKSPACE}");
                if (ElementName.IsPasswordBox)
                {
                    //ElementName.Text = ElementName.Password.Remove(ElementName.SelectionStart - 1, 1);
                    //ElementName.CaretIndex = ElementName.Text.Length;
                    int ad = ElementName.SelectionStart;
                    if (ad > 0)
                    {
                        string Pwding = ElementName.Password.Remove(ElementName.SelectionStart - 1, 1);
                        //ElementName.Text
                        ElementName.Password = Pwding;
                        StringBuilder d = new StringBuilder();
                        foreach (var item in Pwding)
                        {
                            d.Append(ElementName.PasswordChar);
                        }
                        ElementName.IsResponseChange = false;
                        ElementName.Text = d.ToString();
                        ElementName.IsResponseChange = true;
                        ElementName.CaretIndex = ad - 1;
                    }
                    else
                    {
                        //ElementName.CaretIndex = 0;
                    }

                }
                else
                {
                    int ad = ElementName.SelectionStart;
                    if (ad > 0)
                    {
                        ElementName.Text = ElementName.Text.Remove(ElementName.SelectionStart - 1, 1);
                        ElementName.CaretIndex = ad - 1;
                    }
                    else
                    {
                        //ElementName.CaretIndex = 0;
                    }



                }
            }

        }
        private void btnCurorRight_Click(object sender, RoutedEventArgs e)
        {
            ElementName.Focus();
            if (ElementName.Text.Length > 0)
            {
                var _1 = ElementName.CaretIndex + 1;
                if (_1 > ElementName.Text.Length)
                {
                    ElementName.CaretIndex = ElementName.Text.Length;
                }
                else
                {
                    ElementName.CaretIndex = _1;

                }
            }

        }
        private void btnCurorLeft_Click(object sender, RoutedEventArgs e)
        {
            ElementName.Focus();
            if (ElementName.Text.Length > 0)
            {
                var _1 = ElementName.CaretIndex - 1;
                if (_1 < 0)
                {
                    ElementName.CaretIndex = 0;
                }
                else
                {
                    ElementName.CaretIndex = _1;

                }
            }

        }
        private void clearText_Click(object sender, RoutedEventArgs e)
        {
            ElementName.Focus();
            ElementName.Text = "";
            ElementName.CaretIndex = ElementName.Text.Length;
        }

        private void hideKeyboard_Click(object sender, RoutedEventArgs e)
        {
            HideKeyboard();
            //else
            //{
            //    this.Visibility = Visibility.Collapsed;
            //}
        }

        public void HideKeyboard()
        {
            var _1 = this.GetLogicalAncestor<Popup>();
            if (_1.IsNotNull())
            {
                _1.IsOpen = false;
            }
        }
    }
}
