
using DZ.Model.Auth.Wx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDZ.Model.Auth.Wx
{
    /// <summary>
    /// 公共参数Model
    /// </summary>
    public class WxCommonModel
    {
        public WxCommonModel()
        {
            this.appid = WxConfig.appid;
            this.timestamp = WxConfig.GetTimestamp();
            this.nonceStr = WxConfig.GetNoncestr();
        }

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }

        public string timestamp { get; set; }

        public string nonceStr { get; set; }

        public string jsapi_ticket { get; set; }


        public string signature { get; set; }

    }
}
