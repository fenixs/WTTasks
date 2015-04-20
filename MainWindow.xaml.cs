using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WTTasks.Utility;
using WTTasks.View;

namespace WTTasks
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();            
            
            this.Closing += MainWindow_Closing;

            //this.Icon = new BitmapImage(new Uri("pack://application:,,,/WTTasks;component/Resources/Mobile.png"));

            NotifyIconHelper._NotifyIncon.MouseClick += _NotifyIncon_MouseClick;
            this.StateChanged += MainWindow_StateChanged;
            

        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Minimize();
        }

        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == System.Windows.WindowState.Minimized)
            {
                Minimize();
            }
        }

        #region "托盘"
        /// <summary>
        /// 点击任务栏图标打开系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _NotifyIncon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button== System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();        //显示系统
                //如果系统最小化，还原系统
                if(this.WindowState== System.Windows.WindowState.Minimized)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                }
                this.Activate();
                
            }
        }
        #endregion

        #region "窗口相关"

        /// <summary>
        /// 最小化窗口到托盘
        /// </summary>
        void Minimize()
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            this.Visibility = System.Windows.Visibility.Hidden;
        }


        
        #endregion
        #region "菜单操作"
        /// <summary>
        /// 菜单操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string arg = (sender as MenuItem).CommandParameter.ToString();
            if (string.IsNullOrEmpty(arg))
                return;
            switch (arg)
            {
                case "news":
                    {
                        var news = new NewsList();
                        news.ShowDialog();
                        break;
                    }
                case "exit":
                    {
                        //退出事件
                        BackgroundWindow.ExitApp();
                        break;
                    }
                case "about":
                    {
                        var about = new About();                        
                        about.ShowDialog();
                        break;
                    }
            }
        }
        #endregion
        


    }
}
