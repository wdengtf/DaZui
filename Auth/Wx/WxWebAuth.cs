using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Log;
using Framework.Utils;
using Framework;
using Events;
using System.Reflection;
using Framework.Model;
using DZ.Model.Auth.Wx;

namespace Auth.Wx
{
    /// <summary>
    /// 微信网页授权
    /// </summary>
    public class WxWebAuth<T> : AuthBase<T>
        where T : BaseModel
    {
        private MethodBase methodBase = System.Reflection.MethodBase.GetCurrentMethod();
        private string ClassName = "微信网页授权";
        private const string appid = WxConfig.appid;
        private const string appSecret = WxConfig.appSecret;
        private WebUtils webUtils = new WebUtils();
        public WxWebAuth()
        { }

        /// <summary>
        /// 授权操作
        /// </summary>
        /// <returns></returns>
        public override bool Excute()
        {
            OperationFilePath = methodBase.DeclaringType.FullName + "." + methodBase.Name;
            OperationName = "获取" + ClassName + "数据";
            try
            {
                WxWebAuthModel wxWebAuthModel = (WxWebAuthModel)Convert.ChangeType(req, typeof(WxWebAuthModel));
                OperationUserName = wxWebAuthModel.channelUser;

                RawData = Utility.ToJson(wxWebAuthModel);
                BaseEvent(EventEnum.OnBegin);
                if (!Validate())
                {
                    Description = BaseMessage;
                    BaseEvent(EventEnum.OnTipMsg);
                    return false;
                }

                string tokenPost = webUtils.DoGet(GetAccess_token(wxWebAuthModel.code), null);
                if (!tokenPost.Contains("access_token"))
                {
                    BaseMessage = "获取Access_token失败";
                    Description = BaseMessage + tokenPost;
                    BaseEvent(EventEnum.OnTipMsg);
                    return false;
                }
                AccessTokenWebModel accessTokenModel = Utility.JsonToObject<AccessTokenWebModel>(tokenPost);

                string strUserinfo = webUtils.DoGet(GetUserInfo(accessTokenModel.access_token, accessTokenModel.openid), null);
                //strUserinfo = System.Text.Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(strUserinfo));

                if (!strUserinfo.Contains("openid"))
                {
                    BaseMessage = "获取微信用户信息失败";
                    Description = BaseMessage + strUserinfo;
                    BaseEvent(EventEnum.OnTipMsg);
                    return false;
                }
                WxMemberModel wxMemberModel = Utility.JsonToObject<WxMemberModel>(strUserinfo);
                this.data = wxMemberModel;

                Description = strUserinfo;
                BaseEvent(EventEnum.OnSuccess);
            }
            catch (Exception ex)
            {
                BaseMessage = MsgShowConfig.Exception;
                Description = ex.ToString();
                BaseEvent(EventEnum.OnException);
            }
            BaseEvent(EventEnum.OnCompelete);
            return true;
        }

        ///// <summary>
        ///// 保存微信信息
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public WxMemberModel WxAuthGetUserInfo(string code)
        //{
        //    WxMemberModel wxMemberModel = null;
        //    try
        //    {
        //        if (String.IsNullOrWhiteSpace(code))
        //            return wxMemberModel;

        //        string tokenPost = webUtils.DoGet(GetAccess_token(code), null);
        //        if (!tokenPost.Contains("access_token"))
        //        {
        //            LogService.LogInfo("获取Access_token失败:" + tokenPost);
        //            return wxMemberModel;
        //        }
        //        AccessTokenWebModel accessTokenModel = Utility.JsonToObject<AccessTokenWebModel>(tokenPost);

        //        string strUserinfo = webUtils.DoGet(GetUserInfo(accessTokenModel.access_token, accessTokenModel.openid), null);
        //        //strUserinfo = System.Text.Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(strUserinfo));

        //        if (!strUserinfo.Contains("openid"))
        //        {
        //            LogService.LogInfo("获取微信用户信息失败:" + strUserinfo);
        //            return wxMemberModel;
        //        }
        //        wxMemberModel = Utility.JsonToObject<WxMemberModel>(strUserinfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogService.LogDebug(ex);
        //    }
        //    return wxMemberModel;
        //}

        #region 私有方法
        /// <summary>
        /// 获取微信Access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetAccess_token(string code)
        {
            string tokenUrl = "";
            try
            {
                tokenUrl = String.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, appSecret, code);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return tokenUrl;
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        private static string RefreshToken(string refresh_token)
        {
            string tokenUrl = "";
            try
            {
                tokenUrl = String.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", appid, refresh_token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return tokenUrl;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="open_id"></param>
        /// <returns></returns>
        private static string GetUserInfo(string access_token, string open_id)
        {
            string userinfoUrl = "";
            try
            {
                userinfoUrl = String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, open_id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return userinfoUrl;
        }
        #endregion
    }
}
