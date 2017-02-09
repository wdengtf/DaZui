using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class MemberBaseModel
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        public string data_type { get; set; }
        /// <summary>
        /// out_id
        /// </summary>
        public string out_id { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
    }
}
