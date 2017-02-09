using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class JqGridPagingModel<T>
    {
        public JqGridPagingModel()
        { }

        public JqGridPagingModel(int pageIndex, int totalPage, int totalRecord, T rows)
        {
            this.pageIndex = pageIndex;
            this.totalPage = totalPage;
            this.totalRecord = totalRecord;
            this.rows = rows;
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalRecord { get; set; }

        /// <summary>
        /// 具体对象
        /// </summary>
        public T rows { get; set; }
    }
}
