using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;
using Framework.Log;
using Framework.Model;

namespace Auth
{
    public class AuthCall<T> : IBase<T> where T : BaseModel
    {
        private readonly IBaseEvent<T> iBaseEvent = null;
        private readonly IEvent events = new MsmqEvents();
        //private readonly IEvent events = new Log4NetEvents();

        public AuthCall()
        {
            iBaseEvent = Factory.Auth<T>();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="channelUser"></param>
        public void Set(T req)
        {
            iBaseEvent.Set(req);
        }

        /// <summary>
        /// 执行 返回结果
        /// </summary>
        /// <returns></returns>
        public bool Excute()
        {
            DeleteEvent();
            AddEvent();
            return iBaseEvent.Excute();
        }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            string message = "";
            try
            {
                message = iBaseEvent.GetMessage();
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return message;
        }

        /// <summary>
        /// 返回执行结果状态
        /// </summary>
        /// <returns></returns>
        public object GetResult()
        {
            bool result = false;
            try
            {
                return iBaseEvent.GetResult();
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }
            return result;
        }

        #region 事件操作
        /// <summary>
        /// 添加事件
        /// </summary>
        private void AddEvent()
        {
            iBaseEvent.OnBegin += new EventHandler(events.OnBegin);
            iBaseEvent.OnTipMsg += new EventHandler(events.OnTipMsg);
            iBaseEvent.OnSuccess += new EventHandler(events.OnSuccess);
            iBaseEvent.OnFail += new EventHandler(events.OnFail);
            iBaseEvent.OnException += new EventHandler(events.OnException);
            iBaseEvent.OnCompelete += new EventHandler(events.OnCompelete);
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        private void DeleteEvent()
        {
            iBaseEvent.OnBegin -= new EventHandler(events.OnBegin);
            iBaseEvent.OnTipMsg -= new EventHandler(events.OnTipMsg);
            iBaseEvent.OnException -= new EventHandler(events.OnException);
            iBaseEvent.OnSuccess -= new EventHandler(events.OnSuccess);
            iBaseEvent.OnFail -= new EventHandler(events.OnFail);
            iBaseEvent.OnCompelete -= new EventHandler(events.OnCompelete);
        }
        #endregion

    }
}
