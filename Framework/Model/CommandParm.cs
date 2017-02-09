using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class CommandParm
    {
        public string strSql { get; set; }
        public SqlParameter[] SqlPara { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandParm()
        { }

        /// <summary>
        /// 功能：返回CommandParm对象
        /// </summary>
        /// <param name="strSql">sql</param>
        /// <param name="SqlPara">参数</param>
        /// <returns></returns>
        public static CommandParm Command(string strSql, SqlParameter[] SqlPara)
        {
            CommandParm comm = new CommandParm();
            comm.strSql = strSql;
            comm.SqlPara = SqlPara;

            return comm;
        }
    }
}
