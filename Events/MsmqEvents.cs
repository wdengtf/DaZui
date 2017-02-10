using Framework;
using Framework.Log;
using Framework.Model;
using MSMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    /// <summary>
    /// 基础事件处理类 通过消息队列发送
    /// </summary>
    public class MsmqEvents : IEvent
    {
        private SendBase msmqSend = new SendBase();

        public MsmqEvents()
        {

        }

        /// <summary>
        /// 处理之前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnBegin(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Success, EventEnum.OnBegin);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        /// <summary>
        /// 处理完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnCompelete(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Success, EventEnum.OnCompelete);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnException(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Fail, EventEnum.OnException);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        /// <summary>
        /// 处理失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnFail(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Fail, EventEnum.OnFail);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        /// <summary>
        /// 处理成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnSuccess(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Success, EventEnum.OnSuccess);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        /// <summary>
        /// 处理提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnTipMsg(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            try
            {
                MsmqSendEvent(e, StatusSFEnmu.Fail, EventEnum.OnTipMsg);
            }
            catch (Exception ex)
            {
                LogService.LogDebug(Utility.ToJson(Args) + ex.ToString());
            }
        }

        #region 私有方法
        /// <summary>
        /// 日志发送消息队列
        /// </summary>
        /// <param name="args"></param>
        /// <param name="statusEnum"></param>
        /// <param name="eventEnum"></param>
        private void MsmqSendEvent(EventArgs e, StatusSFEnmu statusEnum, EventEnum eventEnum)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                DataHandleEventArgs args = (DataHandleEventArgs)e;
                if (args == null)
                    return;

                switch (args.OperationType)
                {
                    case MessageObjType.Auth:
                    case MessageObjType.Payment:
                        messageModel.messageObjType = args.OperationType;
                        //messageModel.messageObjBody = Utility.ToJson(CreateChannelLog(args, statusEnum, eventEnum));
                        break;
                }
                if (messageModel.messageObjBody != null)
                    msmqSend.Send(messageModel);
            }
            catch (Exception ex)
            {
                LogService.LogDebug("发送消息异常" + ex.ToString());
                LogService.LogInfo("原始数据：" + Utility.ToJson(messageModel));
            }
        }


        /// <summary>
        /// 创建官网消息发送对象
        /// </summary>
        /// <param name="args"></param>
        /// <param name="status"></param>
        /// <param name="failType"></param>
        //private QD_ChannelLog CreateChannelLog(DataHandleEventArgs args, StatusSFEnmu statusEnum, EventEnum eventEnum)
        //{
        //    QD_ChannelLog channelLogModel = new QD_ChannelLog();
        //    try
        //    {
        //        channelLogModel.channelName = args.OperationUserName;
        //        channelLogModel.interfaceName = Utility.LimitTitleLen(args.OperationName, 50, "");
        //        channelLogModel.@interface = Utility.LimitTitleLen(args.OperationFilePath, 100, "");
        //        channelLogModel.status = (int)statusEnum;
        //        channelLogModel.failType = eventEnum.ToString();
        //        channelLogModel.failMessage = args.Description;
        //        channelLogModel.RawData = args.RawData;
        //        channelLogModel.ip = Utility.GetRealIp();
        //        channelLogModel.Createtime = args.OperationTime;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogService.LogDebug("创建消息发送对象异常" + ex.ToString());
        //    }
        //    return channelLogModel;
        //}
        #endregion

    }
}
