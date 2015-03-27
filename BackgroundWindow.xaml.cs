using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WTTasks.Utility;

namespace WTTasks
{
    /// <summary>
    /// BackgroundWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BackgroundWindow : Window
    {

        private static Timer _TimerTask = null;

        public static void ExitApp()
        {
            App.Current.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (_TimerTask != null)
                        _TimerTask.Dispose();
                    NotifyIconHelper.Instance().Hide();
                    App.Current.Shutdown();

                }), null);
        }

        public BackgroundWindow()
        {
            InitializeComponent();
            this.Loaded += BackgroundWindow_Loaded;
        }

        void BackgroundWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            NotifyIconHelper.Instance();
            NotifyIconHelper.Instance().ShowBalloonTip("提示", "程序运行成功");


        }
    }
}
