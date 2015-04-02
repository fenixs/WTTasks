/*
 * HtmlHelper.cs
 * HTML相关类
 * 20150402went
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WTTasks.Utility
{
    public class HtmlHelper
    {
        #region "变量"

        /// <summary>
        /// 图片正则
        /// </summary>
        private const string REG_IMG = "<img[^>]+src=\\s*(?:'(?<src>[^']+)'|\"(?<src>[^\"]+)\"|(?<src>[^>\\s]+))\\s*[^>]*>";

        /// <summary>
        /// 链接正则
        /// </summary>
        private const string REG_LINK = @"<a(.*?)href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>";

        /// <summary>
        /// 链接正则1        
        /// </summary>
        private const string REG_LINK1 = "<a(.*?)href=\\s*['\"](?<href>[^\"']+)['\"][^>]*>(?<title>(?:[\\s\\S]*?))</a>";

        /// <summary>
        /// HTML标题正则
        /// </summary>
        private const string REG_TITLE = "<Title[^>]*>(?<Title>[\\s\\S]{5,})</Title>";

        private static HtmlHelper _Instance;
        private static readonly object _lock = new object();

        #endregion

        #region "唯一实例"

        private HtmlHelper() { }

        ~HtmlHelper() { Dispose(); }

        private void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public static HtmlHelper Instance
        {
            get
            {
                if(_Instance==null)
                {
                    lock(_lock)
                    {
                        _Instance = new HtmlHelper();
                    }
                }
                return _Instance;
            }
        }

        #endregion

        #region "获得链接"

        /// <summary>
        /// 多个链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ArrayList GetLinks(string input)
        {
            ArrayList al = new ArrayList();
            Regex re = new Regex(REG_LINK1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection mcs = re.Matches(input);
            foreach (Match mc in mcs)
            {
                string[] link = new string[3];
                link[0] = mc.Groups["title"].Value;
                link[1] = mc.Groups["href"].Value;
                al.Add(link);
            }

            return al;
        }

        #endregion

        #region "获取页面内容"

        /// <summary>
        /// 获取html源码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetHtml(string url)
        {
            return GetHtml(url, "");
        }

        /// <summary>
        /// 获取html源码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageEncode"></param>
        /// <returns></returns>
        public string GetHtml(string url,string pageEncode)
        {
            string content = "";
            try
            {
                HttpWebResponse response = GetResponse(url);
                if(response==null)
                {
                    return content;
                }
                url = response.ResponseUri.AbsoluteUri;
                Stream stream = response.GetResponseStream();
                byte[] buffer = GetContent(stream);
                stream.Close();
                stream.Dispose();
                string charset = "";
                if(string.IsNullOrEmpty(pageEncode))
                {
                    //从header中查找
                    string ht = response.GetResponseHeader("Content-Type");
                    response.Close();
                    string regCharSet = "[\\s\\S]*charset=(?<charset>[\\S]*)";
                    Regex r = new Regex(regCharSet, RegexOptions.IgnoreCase);
                    Match m = r.Match(ht);
                    charset = (m.Captures.Count != 0) ? m.Result("${charset}") : "";
                    if (charset == "-8") charset = "utf-8";

                    if(charset=="")
                    {
                        //找不到，在文件信息中查找
                        //gb2312
                        content = System.Text.Encoding.GetEncoding("gb2312").GetString(buffer);
                        regCharSet = "(<meta[^>]*charset=(?<charset>[^>'\"]*)[\\s\\S]*?>)|(xml[^>]+encoding=(\"|')*(?<charset>[^>'\"]*)[\\s\\S]*?>)";
                        r = new Regex(regCharSet, RegexOptions.IgnoreCase);
                        m = r.Match(content);
                        if (m.Captures.Count == 0)
                        {
                            return content;
                        }
                        charset = m.Result("${charset}");
                    }
                }
                else
                {
                    response.Close();
                    charset = pageEncode.ToLower();
                }
                try
                {
                    content = System.Text.Encoding.GetEncoding(charset).GetString(buffer);
                }
                catch(ArgumentException)
                {
                    //指定编码不可识别
                    content = System.Text.Encoding.GetEncoding("gb2312").GetString(buffer);
                }
            }
            catch
            {
                content = "";
            }
            return content;
        }

        #endregion

        #region "补全链接"
        
        /// <summary>
        /// 补全链接
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public string FixUrl(string baseUrl,string html)
        {
            html = Regex.Replace(html, "(?is)(href|src)=(\"|\')([^(\"|\')]+)(\"|\')", (match) =>
            {
                string org = match.Value;
                string link = match.Groups[3].Value;
                if (link.StartsWith("http"))
                {
                    return org;
                }
                try
                {
                    Uri uri = new Uri(baseUrl);
                    Uri thisUri = new Uri(uri, link);
                    string fullUrl = String.Format("{0}=\"{1}\"", match.Groups[1].Value, thisUri.AbsoluteUri);
                    return fullUrl;
                }
                catch (Exception)
                {
                    return org;
                }
            });
            return html;
        }

        #endregion

        #region "获取图片地址"
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <param name="input">输入内容</param>
        public List<String> GetImgLinks(string input)
        {
            return Helper.Instance.GetList(input, REG_IMG, "src");
        }
        #endregion

        #region "私有方法"

        /// <summary>
        /// 获取response
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebResponse GetResponse(string url)
        {
            int timeOut = 1000;
            bool isCookie = false;
            bool isRepeat = false;
            Uri target = new Uri(url);
            ReCatch:
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target);
                
                request.ReadWriteTimeout = 120000;
                request.Timeout = timeOut;
                request.MaximumAutomaticRedirections = 50;
                request.MaximumResponseHeadersLength = 5;
                request.AllowAutoRedirect = true;
                if(isCookie)
                {
                    request.CookieContainer = new CookieContainer();
                }
                request.UserAgent = "Mozilla/6.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                return (HttpWebResponse)request.GetResponse();
            }
            catch(WebException)
            {
                if(!isRepeat)
                {
                    isRepeat = true;
                    isCookie = true; 
                    goto ReCatch;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        private byte[] GetContent(Stream stream)
        {
            ArrayList al = new ArrayList();
            try
            {
                byte[] buffer = new byte[4096];
                int count = stream.Read(buffer, 0, 4096);
                while (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        al.Add(buffer[i]);
                    }
                    count = stream.Read(buffer, 0, 4096);
                }
            }
            catch { }
            return (byte[])al.ToArray(typeof(byte));
        }        

        #endregion

        #region "公共方法"
        
        /// <summary>
        /// 获取网页body
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public string GetBody(string htmlCode)
        {
            if(htmlCode.Contains("<body"))
            {
                htmlCode = htmlCode.Substring(htmlCode.IndexOf("<body"));
            }
            else if(htmlCode.Contains("<BODY"))
            {
                htmlCode = htmlCode.Substring(htmlCode.IndexOf("<BODY"));
            }
            //替换掉</body>之后的部分
            htmlCode = Regex.Replace(htmlCode, @"</\bbody\b[^>]*>\s*</html>", "", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            return htmlCode;
        }

        #endregion
    }
}
