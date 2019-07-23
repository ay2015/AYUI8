using System;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media;
using System.Runtime.InteropServices;
using static ay.Controls.AyWindow;

namespace ay.Controls
{

    //AY讲解用法
    //< Border x: Name = "baobiao" Width = "200" Height = "200" Margin = "200,0,0,0" ></ Border > 
    //new AyWinformHost(new TestForm(), baobiao);
    public class AyWinformHost
    {
        Window _owner;
        FrameworkElement _placementTarget;
        public Form _form;
        public bool _formCloseLock = false;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Modifiers == Keys.Alt)
                e.Handled = true;
        }


        public AyWinformHost(Form formWindow, FrameworkElement placementTarget, bool closeWhenParentClose = false) //, Window mainWindow)
        {
            _placementTarget = placementTarget;
            Window owner = Window.GetWindow(placementTarget);
            _placementTarget.Unloaded += _placementTarget_Unloaded;
            _placementTarget.IsVisibleChanged += _placementTarget_IsVisibleChanged;
            //Window owner = mainWindow; //page中传入window
            Debug.Assert(owner != null);
            _owner = owner;

            _form = formWindow;
            _form.Opacity = owner.Opacity;
            _form.ShowInTaskbar = false;
            _form.KeyPreview = true;
            _form.FormBorderStyle = FormBorderStyle.None;


            _form.Width = (int)placementTarget.ActualWidth;
            _form.Height = (int)placementTarget.ActualHeight;


            Reposition();
            owner.LocationChanged += delegate { OnSizeLocationChanged(); };
            _placementTarget.SizeChanged += delegate { OnSizeLocationChanged(); };

            if (owner.IsVisible)
                InitialShow();
            else
                owner.SourceInitialized += delegate
                {
                    InitialShow();
                };

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(UIElement.OpacityProperty, typeof(Window));
            dpd.AddValueChanged(owner, delegate { _form.Opacity = _owner.Opacity; });
            _form.KeyDown += Form1_KeyDown;
            _form.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (closeWhenParentClose)
                {
                    _owner.Close();
                }
                else
                {
                    if (_formCloseLock)
                    {
                        _formCloseLock = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }

                }
            };
        }

        private void _placementTarget_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var d = (bool)e.NewValue;
            if (d)
            {
                _form.Show();
            }
            else
            {
                _form.Hide();
            }
        }

        private void _placementTarget_Unloaded(object sender, RoutedEventArgs e)
        {
            _form.Close();
        }

        void InitialShow()
        {
            NativeWindow owner = new NativeWindow();

            owner.AssignHandle(((HwndSource)HwndSource.FromVisual(_owner)).Handle);

            _form.Show(owner);
            owner.ReleaseHandle();
        }

        DispatcherOperation _repositionCallback;

        void OnSizeLocationChanged()
        {
            if (_repositionCallback == null)
                _repositionCallback = _owner.Dispatcher.BeginInvoke(new Action(Reposition), DispatcherPriority.SystemIdle);
        }

        void Reposition()
        {
            _repositionCallback = null;

            Point offset = _placementTarget.TranslatePoint(new Point(), _owner);
            Point size = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(_owner);
            CompositionTarget ct = hwndSource.CompositionTarget;
            offset = ct.TransformToDevice.Transform(offset);
            size = ct.TransformToDevice.Transform(size);

            POINT screenLocation = new POINT(offset);
            WinformWin32.ClientToScreen(hwndSource.Handle, ref screenLocation);
            POINT screenSize = new POINT(size);

            try
            {
                WinformWin32.MoveWindow(_form.Handle, screenLocation.x, screenLocation.y, screenSize.x, screenSize.y, true);
            }
            catch
            {


            }
        }
    }

    class WinformWin32
    {

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    }
}
