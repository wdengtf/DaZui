using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public interface IEventHandler
    {
        /// <summary>
        /// 事件
        /// </summary>
        event EventHandler OnBegin;
        event EventHandler OnTipMsg;
        event EventHandler OnException;
        event EventHandler OnCompelete;
        event EventHandler OnSuccess;
        event EventHandler OnFail;
    }
}
