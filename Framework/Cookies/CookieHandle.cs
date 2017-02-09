using System;
using System.Collections.Generic;
using System.Web;
using Framework.Log;

namespace Framework.Cookies
{
    /// <summary>
    /// Cookies Session处理
    /// </summary>
    public class CookieHandle
    {
        private int DefaultExpiresTime = 24;//单位小时
        /// <summary>
        /// Cookies名称
        /// </summary>
        public string strCookiesName { get; set; }
        /// <summary>
        /// Cookies值
        /// </summary>
        public string strCookiesValue { get; set; }

        /// <summary>
        /// Cookies值
        /// </summary>
        public Dictionary<string, string> strCookiesParm { get; set; }

        public int ExpiresTime { get; set; } //单位小时

        /// <summary>
        /// 构造函数
        /// </summary>
        public CookieHandle() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_strCookiesName"></param>
        public CookieHandle(string _strCookiesName)
        {
            this.strCookiesName = _strCookiesName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_strCookiesName">Cookies名称</param>
        /// <param name="_strCookiesValue">Cookies值</param>
        public CookieHandle(string _strCookiesName, string _strCookiesValue)
        {
            this.strCookiesName = _strCookiesName;
            this.strCookiesValue = _strCookiesValue;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_strCookiesName">Cookies名称</param>
        /// <param name="_strCookiesValue">Cookies值</param>
        /// <param name="_ExpiresTime">过期时间（小时）</param>
        public CookieHandle(string _strCookiesName, string _strCookiesValue, int _ExpiresTime)
        {
            this.strCookiesName = _strCookiesName;
            this.strCookiesValue = _strCookiesValue;
            this.ExpiresTime = _ExpiresTime;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_strCookiesName">Cookies名称</param>
        /// <param name="_strCookiesParm">Cookies值[名称/值]</param>
        public CookieHandle(string _strCookiesName, Dictionary<string, string> _strCookiesParm)
        {
            this.strCookiesName = _strCookiesName;
            this.strCookiesParm = _strCookiesParm;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_strCookiesName">Cookies名称</param>
        /// <param name="_strCookiesParm">Cookies值[名称/值]</param>
        /// <param name="_ExpiresTime">过期时间（小时）</param>
        public CookieHandle(string _strCookiesName, Dictionary<string, string> _strCookiesParm, int _ExpiresTime)
        {
            this.strCookiesName = _strCookiesName;
            this.strCookiesParm = _strCookiesParm;
            this.ExpiresTime = _ExpiresTime;
        }

        #region 设置Cookies操作
        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <returns></returns>
        public bool SetCookies()
        {
            bool flag = false;
            try
            {
                if (!String.IsNullOrEmpty(strCookiesName) && !String.IsNullOrEmpty(strCookiesValue))
                {
                    flag = SetCookiesValue(strCookiesName, strCookiesValue);
                }
                if (!String.IsNullOrEmpty(strCookiesName) && (strCookiesParm != null && strCookiesParm.Count > 0))
                {
                    flag = SetCookiesKeyValue(strCookiesName, strCookiesParm);
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flag;
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="CookiesName">Cookies名称</param>
        /// <param name="CookiesValue">Cookies值</param>
        /// <returns></returns>
        private bool SetCookiesValue(string CookiesName, string CookiesValue)
        {
            bool flag = false;
            try
            {
                //过滤特殊字符
                CookiesValue = CookiesValue.Replace("\r", "").Replace("\n", "").Replace("%0d%0a", "");
                if (String.IsNullOrEmpty(CookiesName) || String.IsNullOrEmpty(CookiesValue))
                    return false;

                if (this.ExpiresTime < 1)
                    this.ExpiresTime = DefaultExpiresTime;

                CookUtils.AddCookie(CookiesName, CookiesValue, DateTime.Now.AddHours(ExpiresTime));
                flag = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flag;
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="CookiesName">Cookies名称</param>
        /// <param name="_strCookiesParm">Cookies键值</param>
        /// <returns></returns>
        private bool SetCookiesKeyValue(string CookiesName, Dictionary<string, string> CookiesParm)
        {
            bool flag = false;
            try
            {
                if (String.IsNullOrEmpty(CookiesName) || CookiesParm == null || CookiesParm.Count < 1)
                    return false;

                if (this.ExpiresTime < 1)
                    this.ExpiresTime = DefaultExpiresTime;

                CookUtils.AddCookie(CookiesName, CookiesParm, DateTime.Now.AddHours(ExpiresTime));
                flag = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flag;
        }

        /// <summary>
        /// 获取Cookies
        /// </summary>
        /// <param name="CookiesName">Cookies名称</param>
        /// <param name="CookiesKeyName">Cookies子名称</param>
        /// <returns></returns>
        public string GetCookies(string CookiesName, string CookiesKeyName = null)
        {
            string strCookValue = "";
            try
            {
                if (!String.IsNullOrEmpty(CookiesName) && !String.IsNullOrEmpty(CookiesKeyName))
                    return CookUtils.GetCookieValue(CookiesName, CookiesKeyName);
                else if (!String.IsNullOrEmpty(CookiesName))
                    return CookUtils.GetCookieValue(CookiesName);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return strCookValue;
        }

        public bool RemoveCookies()
        {
            bool flag = false;
            try
            {
                //清除所有的Session
                System.Web.HttpContext.Current.Session.RemoveAll();

                //删除所有的Cookies数据
                int limit = System.Web.HttpContext.Current.Request.Cookies.Count;
                HttpCookie aCookie;
                string cookieName = "";
                for (int i = 0; i < limit; i++)
                {
                    cookieName = System.Web.HttpContext.Current.Request.Cookies[i].Name;
                    aCookie = new HttpCookie(cookieName);
                    aCookie.Expires = DateTime.Now.AddDays(-1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(aCookie);
                }

                flag = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flag;
        }
        #endregion

        #region Session操作
        /// <summary>
        /// 保存Session数据
        /// </summary>
        /// <param name="saveName">名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetSession(string saveName, string value)
        {
            bool flage = false;
            try
            {
                System.Web.HttpContext.Current.Session[saveName] = value;
                flage = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flage;
        }
        /// <summary>
        /// 取 Session数据
        /// </summary>
        /// <param name="saveName">名称</param>
        /// <returns></returns>
        public string GetSession(string saveName)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[saveName] != null)
                {
                    return System.Web.HttpContext.Current.Session[saveName].ToString();
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
                return "";
            }
            return "";
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="SessionName">Session名称</param>
        /// <returns></returns>
        public bool RemoveSession(string SessionName)
        {
            bool flage = false;
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionName] != null)
                {
                    System.Web.HttpContext.Current.Session.Remove(SessionName);
                    flage = true;
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flage;
        }

        #endregion

        #region 设置后台用户信息
        private string strManagerCookName = "YYTManage";
        private string[] UserCookName = { "userId", "userName", "droit", "groupId", "loginGroupId" };

        /// <summary>
        /// 设置后台用户的Cookies
        /// </summary>
        /// <param name="userId">会员id</param>
        /// <param name="userName">会员名</param>
        /// <param name="Droit">权限值</param>
        /// <param name="groupId">权限id</param>
        /// <param name="loginGroupId">登陆权限id</param>
        /// <returns></returns>
        public bool SetManagerUser(int userId, string userName, string droit, int groupId,int loginGroupId)
        {
            bool flag = false;
            try
            {
                if (userId < 1 || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(droit))
                    return false;

                IDictionary<string, string> parm = new Dictionary<string, string>();
                parm.Add(UserCookName[0], userId.ToString());
                parm.Add(UserCookName[1], userName);
                parm.Add(UserCookName[2], droit);
                parm.Add(UserCookName[3], groupId.ToString());
                parm.Add(UserCookName[4], loginGroupId.ToString());

                DateTime ExpiresTime = DateTime.Now.AddDays(1);

                CookUtils.AddCookie(strManagerCookName, parm, ExpiresTime);
                flag = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return flag;
        }

        /// <summary>
        /// 获取后台用户id
        /// </summary>
        /// <returns></returns>
        public int GetManagerUserId()
        {
            int userId = 0;
            try
            {
                string cookValue = CookUtils.GetCookieValue(strManagerCookName, UserCookName[0]);

                if (string.IsNullOrEmpty(cookValue))
                    return userId;

                int.TryParse(cookValue, out userId);

                return userId;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return userId;
        }

        /// <summary>
        /// 获取后台用户名称
        /// </summary>
        /// <returns></returns>
        public string GetManagerUserName()
        {
            string strCookValue = "";
            try
            {
                return CookUtils.GetCookieValue(strManagerCookName, UserCookName[1]);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return strCookValue;
        }

        /// <summary>
        /// 获取后台用户权限
        /// </summary>
        /// <returns></returns>
        public string GetManagerDroit()
        {
            string strCookValue = "";
            try
            {
                return CookUtils.GetCookieValue(strManagerCookName, UserCookName[2]);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return strCookValue;
        }

        /// <summary>
        /// 获取后台用户组id
        /// </summary>
        /// <returns></returns>
        public int GetManagerGroupId()
        {
            int groupId = 0;
            try
            {
                string cookValue = CookUtils.GetCookieValue(strManagerCookName, UserCookName[3]);

                if (string.IsNullOrEmpty(cookValue))
                    return groupId;

                int.TryParse(cookValue, out groupId);

                return groupId;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return groupId;
        }


        /// <summary>
        /// 获取后台用户登陆组id
        /// </summary>
        /// <returns></returns>
        public int GetManagerLoginGroupId()
        {
            int loginGroupId = 0;
            try
            {
                string cookValue = CookUtils.GetCookieValue(strManagerCookName, UserCookName[4]);

                if (string.IsNullOrEmpty(cookValue))
                    return loginGroupId;

                int.TryParse(cookValue, out loginGroupId);

                return loginGroupId;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return loginGroupId;
        }

        #endregion
    }
}
