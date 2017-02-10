using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MSMQ
{
    public interface ISend
    {
        bool Send(MessageModel messageModel);

        bool BatchSend(List<MessageModel> messageModelList);
    }
}
