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
using System.Windows.Shapes;
using WTTasks.Model;
using WTTasks.Utility;

namespace WTTasks.View
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : MetroWindow
    {
        public About()
        {
            InitializeComponent();

            StringBuilder sbMsg = new StringBuilder();
            sbMsg.Append("版 本 号：V");
            sbMsg.Append(Helper.Instance.GetVersion() + "\r\n");
            sbMsg.Append("版权所有：fenixs@126.com \r\n");
            sbMsg.Append("Copyright@ 2015 Went.All Right Reserved.\r\n");
            sbMsg.Append("作品说明：本作品只用于学习交流, 如若转载, 请注明作者与出处。");

            this.tbInfo.Text = sbMsg.ToString();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Hyperlink).CommandParameter != null)
            {
                string uri = (sender as Hyperlink).CommandParameter.ToString();
                System.Diagnostics.Process.Start(uri);
            }
        }

        /// <summary>
        /// 查看说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", PubModel.__StartupPath + "\\说明.txt");
        }


    }
}
