using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MSMQ
{
    public interface IReceive
    {
        /// <summary>
        /// 接受消息
        /// </summary>
        MessageModel Receive();

        /// <summary>
        /// 批量接受消息
        /// </summary>
        /// <returns></returns>
        List<MessageModel> BatchReceive();
    }
}
