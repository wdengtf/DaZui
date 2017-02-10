using Framework;
using Framework.Log;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    /// <summary>
    /// 基础事件处理类 通过日志记录
    /// </summary>
    public class Log4NetEvents : IEvent
    {
        public Log4NetEvents()
        {

        }

        public virtual void OnBegin(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogInfo("开始：" + Utility.ToJson(Args));
        }

        public virtual void OnTipMsg(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogInfo("提示：" + Utility.ToJson(Args));
        }

        public virtual void OnException(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogError("异常：" + Utility.ToJson(Args));
        }

        public virtual void OnSuccess(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogInfo("成功：" + Utility.ToJson(Args));
        }

        public virtual void OnFail(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogError("失败：" + Utility.ToJson(Args));
        }

        public virtual void OnCompelete(object sender, EventArgs e)
        {
            DataHandleEventArgs Args = (DataHandleEventArgs)e;
            LogService.LogInfo("完成：" + Utility.ToJson(Args));
        }
    }
}
