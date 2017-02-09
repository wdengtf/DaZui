using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    /// <summary>
    /// Msmq消息对象
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息类型枚举
        /// </summary>
        public MessageObjType messageObjType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string messageObjBody { get; set; }
    }



    public enum MessageObjType
    {
        /// <summary>
        /// 抽奖
        /// </summary>
        LuckDraw,
        /// <summary>
        /// 问卷调查
        /// </summary>
        Questionnaire,
        /// <summary>
        /// 授权
        /// </summary>
        Auth,
        /// <summary>
        /// 微信企业付款
        /// </summary>
        CompanyPayment,
        /// <summary>
        /// 微信红包
        /// </summary>
        RedPackets

    }
}
