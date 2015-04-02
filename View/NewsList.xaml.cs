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
using WTTasks.Utility;

namespace WTTasks.View
{
    /// <summary>
    /// NewsList.xaml 的交互逻辑
    /// </summary>
    public partial class NewsList : MetroWindow
    {
        public NewsList()
        {
            InitializeComponent();
            this.Loaded += NewsList_Loaded;
        }

        void NewsList_Loaded(object sender, RoutedEventArgs e)
        {
            this.biDataload.IsBusy = true;
            BLL.TaskBLL.Instance.SpiderNewsToHtml();

            this.biDataload.IsBusy = false;
        }

        private void LoadNewsList()
        {

        }
    }
}
