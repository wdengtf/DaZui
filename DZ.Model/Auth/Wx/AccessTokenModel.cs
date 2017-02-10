using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ.Model.Auth.Wx
{
    /// <summary>
    /// 微信网页授权获取AccessToken返回对象
    /// </summary>
    public class AccessTokenWebModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }

        public string unionid { get; set; }
    }

    /// <summary>
    /// 微信公众号授权获取AccessToken返回对象
    /// </summary>
    public class AccessTokenServerModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 微信公众号获取jsapi_ticket返回对象[包括AccessToken]
    /// </summary>
    public class ServerTokenAndTicketModel : AccessTokenServerModel
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string ticket { get; set; }
    }
}
