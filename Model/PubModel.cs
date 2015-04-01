/*
 * 文件名:PubModel.cs
 * 功能描述:公共变量存储
 * 20150326Went
 * 修改:
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTTasks.Model
{
    /// <summary>
    /// 全局变量
    /// </summary>
    class PubModel
    {
        #region "系统运行相关"
        /// <summary>
        /// 程序名
        /// "WTTasks"
        /// </summary>
        public const string __APPNAME = "WTTasks";

        /// <summary>
        /// 主窗口是否打开
        /// </summary>
        public static bool __IsMainWindow { get; set; }

        /// <summary>
        /// 程序主路径
        /// </summary>
        public static string __StartupPath { get; set; }

        /// <summary>
        /// config.xml文件路径
        /// </summary>
        public static string __Config { get; set; }

        /// <summary>
        /// 当前系统是32位还是64位
        /// </summary>
        public static int __OSType { get; set; }
        #endregion

        #region "新闻列表相关"
        /// <summary>
        /// 获取新闻的地址
        /// </summary>
        public static string __NewsUri { get; set; }

        /// <summary>
        /// 新闻列表页面标签
        /// </summary>
        public static SortedList __NewsTag { get; set; }
        #endregion

    }
}
