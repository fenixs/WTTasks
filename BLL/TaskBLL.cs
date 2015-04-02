/*
 * 业务层
 * 20150401went
 * 
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTTasks.Model;
using WTTasks.Utility;

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
            string path = PubModel.__StartupPath + "\\News\\news.htm";
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.CreationTime < DateTime.Now.AddHours(-2))
                {                    
                    WTTools.IOHelper.Instance.DeleteFile(path);
                }

            }
            if(File.Exists(path))
            {
                return;
            }
            string html = "";
            html = HtmlHelper.Instance.GetHtml(PubModel.__NewsUri);
            HtmlHelper.Instance.FixUrl(PubModel.__NewsUri, html);

            StringBuilder sbTag = new StringBuilder();
            StringBuilder sbList = new StringBuilder();
            string templatePath = PubModel.__StartupPath + "\\News\\news.html";
            string newHtml = WTTools.IOHelper.Instance.ReadFile(templatePath);

            int i = 1;
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string no = "";
            foreach (DictionaryEntry m   in PubModel.__NewsTag)
            {
                string content = WTTools.CString.CutString(html, m.Value.ToString().Split('⊙')[0], m.Value.ToString().Split('⊙')[1], false);
                ArrayList al = HtmlHelper.Instance.GetLinks(content);

            }
        }

        #endregion

    }
}
