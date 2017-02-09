using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Model
{
    public class DataHandleEventArgs : EventArgs
    {
        public DataHandleEventArgs()
        {
            this.OperationTime = DateTime.Now;
        }

        /// <summary>
        /// 操作用户名
        /// </summary>
        public string OperationUserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public MessageObjType OperationType { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string OperationFilePath { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public string RawData { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime OperationTime { get; set; }
    }
}
