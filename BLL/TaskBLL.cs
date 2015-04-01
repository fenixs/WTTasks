/*
 * 业务层
 * 20150401went
 * 
 * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTTasks.Model;

namespace WTTasks.BLL
{
    public class TaskBLL
    {
        private static TaskBLL _Instance;
        private static object _Lock = new object();//使用static object作为互斥资源

        #region "唯一实例"

        private TaskBLL()
        {
        }

        public static TaskBLL Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock(_Lock)
                    {
                        _Instance = new TaskBLL();
                    }
                }
                return _Instance;
            }
        }

        #endregion

        #region "抓取新闻"

        /// <summary>
        /// 抓取新闻网页
        /// </summary>
        public void SpiderNewsToHtml()
        {
            if (string.IsNullOrEmpty(PubModel.__NewsUri) || PubModel.__NewsTag == null || PubModel.__NewsTag.Count == 0)
            {
                return;
            }
            string path = PubModel.__StartupPath + "\\News\\news.html";
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.CreationTime < DateTime.Now.AddHours(-2))
                {
                    //TODO:删除文件

                }
            }
            if(File.Exists(path))
            {
                return;
            }
        }

        #endregion

    }
}
