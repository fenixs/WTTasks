using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using WTTasks.Control;
using WTTasks.Model;
using WTTasks.Utility;

namespace WTTasks.View
{
    /// <summary>
    /// NewsList.xaml 的交互逻辑
    /// </summary>
    public partial class NewsList : MetroWindow
    {
        private WebBrowser wb = null;
        public NewsList()
        {
            InitializeComponent();
            //this.Loaded += NewsList_Loaded;
            //WebbrowserOverlay wo = new WebbrowserOverlay(brNews);
            //wb = wo.WebBrowser;
            //wb.Navigate("http://www.baidu.com");

            LoadNewsList();
        }

        void NewsList_Loaded(object sender, RoutedEventArgs e)
        {
            this.biDataload.IsBusy = true;
            BLL.TaskBLL.Instance.SpiderNewsToHtml();

            this.biDataload.IsBusy = false;
            LoadNewsList();
        }

        private void LoadNewsList()
        {
            string path = PubModel.__StartupPath + "\\News\\news.htm";
            if(!File.Exists(path))
            {
                path = Model.PubModel.__NewsUri;
            }
            WebbrowserOverlay wo = new WebbrowserOverlay(brNews);
            wb = wo.WebBrowser;

            path = "http://www.baidu.com";
            wb.Navigate(new Uri(path));
        }
    }
}
