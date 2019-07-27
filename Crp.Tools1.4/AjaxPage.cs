using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crp.Tools
{
    /// <summary>
    /// 分页统一回调
    /// </summary>
    /// <author>戴总写的分页统一回调</author>
    public class AjaxPage
    {

        public int TotalCount { get; set; }
        public object Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "";


        public AjaxPage(int total, object data,bool success = true,string msg = "")
        {
            this.TotalCount = total;
            this.Data = data;
            this.Success = success;
            this.Msg = msg;
        }
    }
}
