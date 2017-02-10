using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 处理之前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBegin(object sender, EventArgs e);
        /// <summary>
        /// 提示信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTipMsg(object sender, EventArgs e);
        /// <summary>
        /// 处理异常错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnException(object sender, EventArgs e);
        /// <summary>
        /// 处理成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSuccess(object sender, EventArgs e);
        /// <summary>
        /// 处理失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFail(object sender, EventArgs e);
        /// <summary>
        /// 处理完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCompelete(object sender, EventArgs e);
    }
}
