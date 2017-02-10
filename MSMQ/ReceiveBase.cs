using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using Framework.Log;
using Framework;
using Framework.Model;

namespace MSMQ
{
    public class ReceiveBase : IReceive
    {
        protected static MessageQueue queue;
        private readonly static string queuePath = Utility.GetConfig("queuePath");

        public ReceiveBase()
        { }

        /// <summary>
        /// 接受消息数据
        /// </summary>
        /// <returns></returns>
        public virtual MessageModel Receive()
        {
            MessageModel messageModel = null;
            try
            {
                if (queue == null && !String.IsNullOrEmpty(queuePath))
                {
                    queue = new MessageQueue(queuePath);
                    //注：由于消息的优先级是枚举类型，在直接messages[index].Priority.ToString();这种方式来获取优先级转化到字符串的时候，他需要一个过滤器(Filter)，否则会抛出一个InvalidCastExceptionle类型的异常，异常信息"接收消息时未检索到属性 Priority。请确保正确设置了 PropertyFilter。"，要解决这问题只需要把消息对象的MessageReadPropertyFilter（过滤器） 的Priority设置为true。
                    queue.MessageReadPropertyFilter.Priority = true;
                    //对象序列化
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                }

                if (queue == null)
                {
                    return messageModel;
                }

                using (var messageEnumerator = queue.GetMessageEnumerator2())
                {
                    if (messageEnumerator.MoveNext())
                    {
                        Message message = new Message();
                        message.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });

                        message = queue.Receive();

                        if (message != null)
                        {
                            messageModel = (MessageModel)message.Body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return messageModel;
        }

        /// <summary>
        /// 批量接受数据
        /// </summary>
        /// <returns></returns>
        public virtual List<MessageModel> BatchReceive()
        {
            List<MessageModel> messageModelList = null;
            try
            {
                if (queue == null && !String.IsNullOrEmpty(queuePath))
                {
                    queue = new MessageQueue(queuePath);
                    //注：由于消息的优先级是枚举类型，在直接messages[index].Priority.ToString();这种方式来获取优先级转化到字符串的时候，他需要一个过滤器(Filter)，否则会抛出一个InvalidCastExceptionle类型的异常，异常信息"接收消息时未检索到属性 Priority。请确保正确设置了 PropertyFilter。"，要解决这问题只需要把消息对象的MessageReadPropertyFilter（过滤器） 的Priority设置为true。
                    queue.MessageReadPropertyFilter.Priority = true;
                    //对象序列化
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });
                }

                if (queue == null)
                {
                    return messageModelList;
                }

                messageModelList = new List<MessageModel>();
                using (var messageEnumerator = queue.GetMessageEnumerator2())
                {
                    while (messageEnumerator.MoveNext())
                    {
                        Message message = new Message();
                        message.Formatter = new XmlMessageFormatter(new Type[] { typeof(MessageModel) });

                        message = queue.Receive();

                        if (message != null)
                        {
                            messageModelList.Add((MessageModel)message.Body);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return messageModelList;
        }
    }
}
