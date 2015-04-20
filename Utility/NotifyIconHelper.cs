/*
 * 文件名:NotifyIconHelper.cs
 * 功能描述:托盘应用
 * 20150326Went
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WTTasks.Utility
{
    /// <summary>
    /// 托盘应用
    /// </summary>
    class NotifyIconHelper
    {
        private static NotifyIconHelper _Instance = null;
        public static NotifyIcon _NotifyIncon = new NotifyIcon();

        #region "初始"

        public NotifyIconHelper()
        {
            SetupNotifyIconHelper();
        }

        public static NotifyIconHelper Instance()
        {
            if (_Instance == null)
                _Instance = new NotifyIconHelper();
            return _Instance;
        }

        #endregion

        public void ShowBalloonTip(string title,string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return;
            }
            try
            {
                _NotifyIncon.BalloonTipTitle = title;
                _NotifyIncon.BalloonTipText = text;
                _NotifyIncon.BalloonTipIcon = ToolTipIcon.None;
                _NotifyIncon.ShowBalloonTip(2000);
            }
            catch(Exception e)
            {

            }
        }

        #region "菜单"
        
        private void SetupNotifyIconHelper()
        {
            //获取系统图标当托盘图标
            //_NotifyIncon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            _NotifyIncon.Icon = WTTasks.Properties.Resources.App;
            _NotifyIncon.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem closeMonitor = new ToolStripMenuItem("关闭显示器");
            closeMonitor.Click += closeMonitor_Click;

            ToolStripMenuItem menuItem1 = new ToolStripMenuItem("帮助");      //菜单1
            menuItem1.Click += menuItem1_Click;

            ToolStripSeparator separator1 = new ToolStripSeparator();   //分隔符

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("退出");
            exitMenuItem.Click += exitMenuItem_Click;

            _NotifyIncon.ContextMenuStrip.Items.AddRange(new ToolStripItem[] { closeMonitor,menuItem1,separator1,exitMenuItem });
            _NotifyIncon.Visible = true;
        }

        void closeMonitor_Click(object sender, EventArgs e)
        {
            Helper.Instance.CloseMonitor();
        }

        void exitMenuItem_Click(object sender, EventArgs e)
        {
            //TODO:退出程序

            if (MessageBox.Show("确定要退出托盘程序吗?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                BackgroundWindow.ExitApp();
                NotifyIconHelper.Instance().Hide();
            }

        }

        void menuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.baidu.com");
        }

        public void Hide()
        {
            _NotifyIncon.Visible = false;
        }

        #endregion

    }
}
