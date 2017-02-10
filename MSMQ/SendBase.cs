using System;
using System.Messaging;
using Framework;
using Framework.Log;
using Message = System.Messaging.Message;
using System.Collections.Generic;
using Framework.Model;

namespace MSMQ
{
    public class SendBase : ISend
    {
        protected MessageQueue queue;
        protected MessageQueue queueTransaction;
        private readonly string queuePath = Utility.GetConfig("queuePath");

        public SendBase()
        { }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual bool Send(MessageModel messageModel)
        {
            bool result = false;
            try
            {
                if (queue == null)
                {
                    queue = new MessageQueue(queuePath);
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                }

                Message message = new Message();
                //设置优先级
                switch (messageModel.messageObjType)
                {
                    case MessageObjType.Auth:
                    case MessageObjType.Payment:
                        message.Priority = MessagePriority.Low;
                        break;
                }
                //对象序列化
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                message.Body = messageModel;
                //发送消息到队列中
                queue.Send(message);
                result = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return result;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="messageModel"></param>
        public virtual bool BatchSend(List<MessageModel> messageModelList)
        {
            bool result = false;
            try
            {
                if (queue == null)
                {
                    queue = new MessageQueue(queuePath);
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                }

                Message message = new Message();
                foreach (MessageModel messageModel in messageModelList)
                {
                    //设置优先级
                    switch (messageModel.messageObjType)
                    {
                        case MessageObjType.Auth:
                        case MessageObjType.Payment:
                            message.Priority = MessagePriority.Low;
                            break;
                    }
                    //对象序列化
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                    message.Body = messageModel;
                    //发送消息到队列中
                    queue.Send(message);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return result;
        }
    }
}
