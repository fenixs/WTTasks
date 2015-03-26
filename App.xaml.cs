using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WTTasks.Model;

namespace WTTasks
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 单实例运行代码
        /// </summary>
        Mutex _mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool startUpFlag = false;
            //判断进程中是否已经存在当前程序实例
            _mutex = new Mutex(true, PubModel.__APPNAME, out startUpFlag);
            if (!startUpFlag)
            {
                if (!PubModel.__IsMainWindow)
                {
                    PubModel.__IsMainWindow = true;
                }
                MessageBox.Show("程序已经启动!");
                Environment.Exit(0);
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        /// <summary>
        ///  初始化
        /// </summary>
        private void InitializeWTT()
        {
            PubModel.__StartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            PubModel.__Config = PubModel.__StartupPath + "\\Config.xml";
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            if(!System.IO.File.Exists(PubModel.__Config))
            {
                MessageBox.Show("没有找到配置文件");
                Application.Current.Shutdown();
            }


        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //TODO:写log文件
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        
    }
}
