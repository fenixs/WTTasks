/*
 * 文件名:helper.cs
 * 功能描述:功能类
 * 20150331went
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WTTasks.Utility
{
    class Helper
    {
        private static Helper _Instance;
        private static readonly object _lock = new object();

        #region "单一实例"

        private Helper()
        {
        }

        ~Helper()
        {
            Dispose();
        }

        public static Helper Instance
        {
            get
            {
                if(_Instance==null)
                {
                    lock(_lock)
                    {
                        _Instance = new Helper();
                    }
                }
                return _Instance;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);            
        }
        #endregion

        #region "获取程序版本"

        /// <summary>
        /// 获取程序版本
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        #endregion

        #region "多个匹配内容"
        
        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="pattern">表达式</param>
        /// <param name="groupName">分组</param>
        /// <returns></returns>
        public List<string> GetList(string input,string pattern,string groupName)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mcs = re.Matches(input);
            foreach (Match mc in mcs)
            {
                if((string.IsNullOrEmpty(groupName)))
                {
                    list.Add(mc.Value);
                }
                else
                {
                    list.Add(mc.Groups[groupName].Value);
                }
            }

            return list;
        }



        #endregion

        #region "启动外部程序"


        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <returns></returns>
        public bool StartApp(string appName)
        {
            return StartApp(appName, null, System.Diagnostics.ProcessWindowStyle.Normal);
        }

        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns></returns>
        public bool StartApp(string appName, string arguments, System.Diagnostics.ProcessWindowStyle style)
        {
            bool result = false;
            using(System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = appName;
                p.StartInfo.WindowStyle = style;
                p.StartInfo.Arguments = arguments;
                try
                {
                    p.Start();
                    p.WaitForExit();
                    p.Close();
                    result = true;
                }
                catch
                {

                }
            }

            return result;
        }

        #endregion

        #region "显示器操作"

        /// <summary>
        /// 系统消息
        /// </summary>
        private const uint WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// 启动屏幕保护
        /// </summary>
        private const uint SC_SCREENSAVE = 0xF140;

        /// <summary>
        /// 关闭显示器的系统命令
        /// </summary>
        private const uint SC_MONITORPOWER = 0xF170;
        /// <summary>
        /// 广播消息，所有顶级窗体都会接收
        /// </summary>
        private readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        /// <summary>
        /// 打开显示器
        /// </summary>
        public void OpenMonitor()
        {               
            SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }

        /// <summary>
        /// 启动屏幕保护，关闭显示器
        /// </summary>
        public void CloseMonitor()
        {
            //SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_SCREENSAVE, 0);
            SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }

        #endregion
    }
}
