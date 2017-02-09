using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    /// <summary>
    /// 事件枚举
    /// </summary>
    public enum EventEnum
    {
        OnBegin,
        OnTipMsg,
        OnException,
        OnSuccess,
        OnFail,
        OnCompelete
    }

    /// <summary>
    /// 排序枚举
    /// </summary>
    public enum OrderByEnum
    {
        ASC, DESC
    }

    /// <summary>
    /// 默认状态
    /// </summary>
    public enum StatusEnmu
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 锁定
        /// </summary>
        Locking = 0,
    }

    /// <summary>
    /// 成功 失败状态
    /// </summary>
    public enum StatusSFEnmu
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1
    }
}
