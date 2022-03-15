using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace ay.Controls
{
    public struct Dpi
    {
        public Dpi(Double x, Double y)
        {
            DpiX = x;
            DpiY = y;

            Px2WpfX = 96 / DpiX;
            Px2WpfY = 96 / DpiY;
        }

        public Double DpiX { get; }

        public Double DpiY { get; }

        public Double Px2WpfX { get; }

        public Double Px2WpfY { get; }

        /// <summary>
        /// 英寸-厘米
        /// </summary>
        public static readonly Double In2Cm = 2.54;
        /// <summary>
        /// 英寸-磅
        /// </summary>
        public static readonly Double In2Pt = 72;
        /// <summary>
        /// 厘米-wpf
        /// </summary>
        public static readonly Double Cm2Wpf = 96 / 2.54;
    }
    public sealed class DpiHelper
    {
        #region Graphics

        public static Dpi GetDpiByGraphics(IntPtr hWnd)
        {
            using (var graphics = Graphics.FromHwnd(hWnd))
            {
                return new Dpi(graphics.DpiX, graphics.DpiY);
            }
        }

        #endregion

        #region CompositionTarget

        public static Dpi GetDpiFromVisual(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);
            return (source == null || source.CompositionTarget == null) ? GetDpiByWin32(IntPtr.Zero) : new Dpi(96.0 * source.CompositionTarget.TransformToDevice.M11, 96.0 * source.CompositionTarget.TransformToDevice.M22);
        }

        #endregion

        #region Win32 API

        private const Int32 LOGPIXELSX = 88;
        private const Int32 LOGPIXELSY = 90;

        [DllImport("gdi32.dll")]
        private static extern Int32 GetDeviceCaps(IntPtr hdc, Int32 index);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern Int32 ReleaseDC(IntPtr hWnd, IntPtr hDc);

        public static Dpi GetDpiByWin32(IntPtr hwnd)
        {
            var hDc = GetDC(hwnd);

            var dpiX = GetDeviceCaps(hDc, LOGPIXELSX);
            var dpiY = GetDeviceCaps(hDc, LOGPIXELSY);

            ReleaseDC(hwnd, hDc);
            return new Dpi(dpiX, dpiY);
        }

        #endregion
    }


}
