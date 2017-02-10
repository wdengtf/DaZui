using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;
using Auth;
using Framework.Model;
using DZ.Model.Auth.Wx;

namespace Auth
{
    public class Factory
    {
        private Factory()
        {

        }

        #region 外部可直接调用 直接调用则未验证签名
        /// <summary>
        /// 微信公众号授权
        /// </summary>
        /// <returns></returns>
        public static IBase<WxServerAuthModel> WxServerAuth()
        {
            return new AuthCall<WxServerAuthModel>();
        }

        /// <summary>
        /// 微信网页授权
        /// </summary>
        /// <param name="wxWebAuthModel"></param>
        /// <returns></returns>
        public static IBase<WxWebAuthModel> WxWebAuth()
        {
            return new AuthCall<WxWebAuthModel>();
        }
        #endregion

        /// <summary>
        /// 工厂模式 Call调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IBaseEvent<T> Auth<T>() where T : BaseModel
        {
            IBaseEvent<T> iBaseEvent = null;
            if (typeof(T) == typeof(WxServerAuthModel))
            {
                iBaseEvent = new Wx.WxServerAuth<T>();
            }
            else if (typeof(T) == typeof(WxWebAuthModel))
            {
                iBaseEvent = new Wx.WxWebAuth<T>();
            }
            return iBaseEvent;
        }
    }
}
