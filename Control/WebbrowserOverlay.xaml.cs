using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WTTasks.Control
{
    /// <summary>
    /// WebbrowserOverlay.xaml 的交互逻辑
    /// </summary>
    public partial class WebbrowserOverlay : Window
    {
        /*
         * WebBrowser先天缺陷，优先级比wpf高
         * 需要使用一个webbrowseroverlay来覆盖
         * */

        FrameworkElement _placementTarget;

        public WebBrowser WebBrowser { get { return _wb; } }

        public WebbrowserOverlay(FrameworkElement placementTarget)
        {
            InitializeComponent();
            _placementTarget = placementTarget;
            Window owner = Window.GetWindow(placementTarget);
            Debug.Assert(owner != null);
            owner.LocationChanged += delegate { OnSizeLocationChanged(); };
            _placementTarget.SizeChanged += delegate { OnSizeLocationChanged(); };

            if (owner.IsVisible)
            {
                Owner = owner;                
                Show();                
            }
            else
            {                
                owner.IsVisibleChanged += delegate
                    {
                        if(owner.IsVisible)
                        {
                            Owner = owner;                            
                            Show();                            
                        }
                    };
            }

            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if(!e.Cancel)
            {
                Dispatcher.BeginInvoke((Action)delegate
                    {
                        Owner.Close();
                    }
                    );
            }
        }


        void OnSizeLocationChanged()
        {
            Point offset = _placementTarget.TranslatePoint(new Point(), Owner);
            Point size = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);

            if (Owner == null) return;

            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(Owner);
            CompositionTarget ct = hwndSource.CompositionTarget;
            offset = ct.TransformToDevice.Transform(offset);
            size = ct.TransformToDevice.Transform(size);

            Win32.POINT screenLoc = new Win32.POINT(offset);
            Win32.ClientToScreen(hwndSource.Handle, ref screenLoc);
            Win32.POINT screenSize = new Win32.POINT(size);

            Win32.MoveWindow(((HwndSource)HwndSource.FromVisual(this)).Handle, screenLoc.X, screenLoc.Y, screenSize.X, screenSize.Y, true);
            //Debug.Write(string.Format("{0},{1}",size.X,size.Y));
        }

    }

    public static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public POINT(Point pt)
            {
                X = Convert.ToInt32(pt.X);
                Y = Convert.ToInt32(pt.Y);
            }
        }

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hwnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
