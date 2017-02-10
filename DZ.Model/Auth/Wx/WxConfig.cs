using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DZ.Model.Auth.Wx
{
    public class WxConfig
    {
        public const string appid = "wxb5f424ccbb74ed55"; //"wxf2467c535a774bbd";
        public const string appSecret = "419238a2732fdac9641e714f57c62013"; //"a257f94c37e30cfe7eb87d10690e7dca";

        public WxConfig()
        {

        }

        /// <summary>
        /// 获取微信Code
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public static string GetCode(string redirectUrl)
        {
            string codeUrl = "";
            try
            {
                codeUrl = String.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", appid, redirectUrl, 1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return codeUrl;
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK");
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 微信配置生成签名
        /// </summary>
        /// <returns></returns>
        public static string GetConfigSign(string jsapi_ticket, string timestamp, string nonceStr, string url)
        {
            //生成签名
            String param = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url;
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
        }

        #region 私有方法

        /** 获取大写的MD5签名结果 */
        private static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        #endregion
    }
}
