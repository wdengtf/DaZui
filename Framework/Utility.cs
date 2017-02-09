using Framework.Log;
using Framework.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Framework
{
    public class Utility
    {
        /// <summary>
        /// Dictionary 非空验证
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public JsonResult NotEmptyVerify(Dictionary<string, string> list)
        {
            try
            {
                //无参数验证
                if (list == null || list.Count < 1)
                    return JsonResult.FailResult(MsgShowConfig.ObjectIsEmpty);

                foreach (KeyValuePair<string, string> pair in list)
                {
                    if (String.IsNullOrEmpty(pair.Value))
                        return JsonResult.FailResult(pair.Key + MsgShowConfig.NoEmpty);
                }
                return JsonResult.SuccessResult(MsgShowConfig.Success);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 限制字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ilenth"></param>
        /// <returns></returns>
        public static string LimitTitleLen(string str, int ilenth)
        {
            return LimitTitleLen(str, ilenth, "...");
        }

        /// <summary>
        /// 限制字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ilenth"></param>
        /// <param name="ReplaceStr"></param>
        /// <returns></returns>
        public static string LimitTitleLen(string str, int ilenth, string ReplaceStr)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > ilenth)
                str = str.Substring(0, ilenth) + ReplaceStr;

            return str;
        }

        /// <summary>
        /// 字符串过滤
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterString(string str)
        {
            if (String.IsNullOrEmpty(str))
                return "";

            str = str.Replace(";", "；");
            str = str.Replace("<s", "");
            str = str.Replace("/>", "");
            str = str.Replace("'", "”");
            str = str.Replace("<", "&lt");
            str = str.Replace(">", "&gt");
            str = str.Replace("(", "（");
            str = str.Replace(")", "）");
            return str;
        }

        /// <summary>
        /// 获取Int参数 顺序依次[Form、QueryString、Cookies]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int FNumeric(string str)
        {
            string sStr = "";
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
                return 0;

            if (System.Web.HttpContext.Current.Request[str] != null)
                sStr = System.Web.HttpContext.Current.Request[str].Trim();

            if (sStr == null || !IsNumeric(sStr))
            {
                return 0;
            }
            return Convert.ToInt32(sStr);
        }

        /// <summary>
        /// 获取参数 顺序依次[Form、QueryString、Cookies]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RF(string str)
        {
            string sStr = "";
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
                    return sStr;
                if (System.Web.HttpContext.Current.Request[str] != null)
                    sStr = System.Web.HttpContext.Current.Request[str].Trim();
            }
            catch (Exception ex)
            {
                sStr = "";
                LogService.LogDebug(ex);
            }

            if (!String.IsNullOrEmpty(sStr))
            {
                sStr = FilterString(sStr);
            }
            return sStr;
        }

        /// <summary>
        /// QueryString 获取参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RQuery(string str)
        {
            string sStr = "";
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
                    return sStr;

                if (System.Web.HttpContext.Current.Request.QueryString[str] != null)
                    sStr = System.Web.HttpContext.Current.Request.QueryString[str].Trim();
            }
            catch { sStr = ""; }
            if (!String.IsNullOrEmpty(sStr))
            {
                sStr = FilterString(sStr);
            }
            return sStr;
        }

        /// <summary>
        ///  Form 获取参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RForm(string str)
        {
            string sStr = "";
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
                    return sStr;

                if (System.Web.HttpContext.Current.Request.Form[str] != null)
                    sStr = System.Web.HttpContext.Current.Request.Form[str].Trim();
            }
            catch { sStr = ""; }
            if (!String.IsNullOrEmpty(sStr))
            {
                sStr = FilterString(sStr);
            }
            return sStr;
        }

        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg.IsMatch(str);
        }

        #region 随机数
        private static char[] constant =
      {
        '0','1','2','3','4','5','6','7','8','9',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
        private static char[] constantUpperCase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] constantNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        /// <summary>生成指定长度的字符串</summary>
        /// <param name="len">指定长度</param> 
        /// <returns></returns>
        public static string GetRandomStr(int len, int cycleTimes = 1)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(36);
            Random rd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + cycleTimes);
            for (int i = 0; i < len; i++)
            {
                newRandom.Append(constant[rd.Next(36)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成指定长度的数字
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRandomNum(int len, int cycleTimes = 1)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + cycleTimes);
            for (int i = 0; i < len; i++)
            {
                newRandom.Append(constantNum[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 获取指定长度的大写字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GetUpperCaseRandom(int length, int cycleTimes = 1)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(26);
            Random rd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + cycleTimes);
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constantUpperCase[rd.Next(26)]);
            }
            return newRandom.ToString();
        }
        #endregion

        /// <summary>
        /// 将对象转换为json
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(object o)
        {
            string reStr = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });
            if (!string.IsNullOrEmpty(reStr))
                reStr = reStr.Replace("\r", "").Replace("\n", "").Replace("%0d%0a", "");

            return reStr;
        }


        /// <summary>
        /// 将json转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 微信浏览器
        /// </summary>
        /// <returns></returns>
        public static bool ExitsWXBrowser()
        {
            bool _flag = false;
            if (Utility.GetBrowserUserAgent().ToLower().IndexOf("micromessenger") > -1)
            {
                _flag = true;
            }
            return _flag;
        }

        /// <summary>
        /// //取得当前domain路径
        /// </summary>
        /// <returns></returns>
        public static string GetCurPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 执行js脚本
        /// </summary>
        /// <param name="strScript"></param>
        public static void ScriptMessage(string strScript)
        {
            System.Web.HttpContext.Current.Response.Write("<script>" + strScript + "</script>");
        }

        #region 通过Request获取信息
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetRealIp()
        {
            string ip = "";
            try
            {
                if (System.Web.HttpContext.Current == null)
                    return ip;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == string.Empty)
                {
                    ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                }
            }
            catch (Exception e)
            {
                LogService.LogDebug(e);
            }

            return ip;
        }

        /// <summary>
        /// 功能：获取浏览器名称
        /// 作者：wdeng
        /// 日期：2015-08-27
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserName()
        {
            string name = "";
            try
            {
                HttpBrowserCapabilities bc = System.Web.HttpContext.Current.Request.Browser;
                name = bc.Browser;
            }
            catch (Exception e)
            {
                throw e;
            }

            return name;
        }

        /// <summary>
        /// 功能：获取浏览器用户代理
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserUserAgent()
        {
            string name = "";
            try
            {
                name = System.Web.HttpContext.Current.Request.UserAgent;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }

            return name;
        }
        #endregion

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetConfig(string strName)
        {
            string str = "";
            try
            {
                if (String.IsNullOrEmpty(strName))
                    return str;

                str = ConfigurationManager.AppSettings[strName].ToString();
            }
            catch { str = ""; }
            return str;
        }


        /// <summary>
        /// 获取外网IP
        /// </summary>
        /// <returns></returns>
        public static string GetOutIP()
        {
            string ip = "";
            try
            {
                string strUrl = "http://www.ip138.com/ip2city.asp"; //获得IP的网址了
                Uri uri = new Uri(strUrl);
                WebRequest wr = WebRequest.Create(uri);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("[") + 1;
                int end = all.IndexOf("]", start);
                ip = all.Substring(start, end - start);

                sr.Close();
                s.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return ip;
        }

        #region 进程操作
        /// <summary>
        /// 根据进程路径获取进程名称
        /// </summary>
        /// <param name="processUrl"></param>
        /// <returns></returns>
        public static string GetProcessName(string processUrl)
        {
            string processName = "";
            try
            {
                if (processUrl.IndexOf(".exe") < 0)
                    return processName;
                List<string> splitList = new List<string>(Regex.Split(processUrl, @"\\", RegexOptions.IgnoreCase));

                processName = splitList[splitList.Count - 1].ToString().Replace(".exe", "");
            }
            catch (Exception ex)
            {
                LogService.LogDebug("根据进程路径获取进程名称异常" + ex.ToString());
            }
            return processName;
        }

        /// <summary>
        /// 启动一个进程
        /// </summary>
        public static bool ProcessStart(string processUrl)
        {
            bool result = false;
            try
            {
                System.Diagnostics.Process iProcess = System.Diagnostics.Process.Start(processUrl);
                iProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 干掉一个进程
        /// 注意：GetProcessesByName("QQ")中的参数即进程的名字，别写成GetProcessesByName("QQ.exe"),
        /// </summary>
        /// <param name="processName">进程名称</param>
        public static bool ProcessKill(string processName)
        {
            bool result = false;
            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processName);
                if (processes == null || processes.Length == 0)
                    return result;

                foreach (System.Diagnostics.Process processe in processes)
                {
                    processe.Kill();
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug("干掉一个进程" + ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 获得为该进程(程序)分配的内存
        /// </summary>
        /// <returns></returns>
        public static double GetProcessUsedMemory(long WorkingSet64)
        {
            double usedMemory = 0;

            usedMemory = WorkingSet64 / 1024.00 / 1024.00;

            return usedMemory;
        }
        #endregion

        #region 服务操作


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static bool ServiceStart(string serviceName)
        {
            bool result = false;
            try
            {
                if (!IsWindowsServiceInstalled(serviceName))
                    return result;

                using (System.ServiceProcess.ServiceController control = new ServiceController(serviceName))
                {
                    if (control.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                    {
                        control.Start();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static bool ServiceStop(string serviceName)
        {
            bool result = false;
            try
            {
                if (!IsWindowsServiceInstalled(serviceName))
                    return result;

                if (!IsStart(serviceName))
                    return result;

                using (System.ServiceProcess.ServiceController control = new ServiceController(serviceName))
                {
                    if (control.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        control.Stop();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }

        /// <summary>  
        /// 判断是否安装了某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        /// <returns></returns>  
        private static bool IsWindowsServiceInstalled(string serviceName)
        {
            bool result = false;
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }


        /// <summary>  
        /// 判断某个服务是否启动  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static bool IsStart(string serviceName)
        {
            bool result = true;
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        if ((service.Status == ServiceControllerStatus.Stopped) || (service.Status == ServiceControllerStatus.StopPending))
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }
        #endregion
    }
}
