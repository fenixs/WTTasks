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
    }
}
