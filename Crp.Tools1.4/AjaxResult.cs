using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crp.Tools
{
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "";

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
        
        public object OtherData { get; set; }
        public int State { get; set; }

        public AjaxResult(bool success, string msg)
        {
            this.Success = success;
            this.Msg = msg;
        }

        public AjaxResult(int state,bool success, string msg,object otherData = null)
        {
            this.State = state;
            this.Success = success;
            this.Msg = msg;
            this.OtherData = otherData;
        }

        public AjaxResult(bool success, object data, object otherData = null)
        {
            this.Success = success;
            this.Data = data;
            this.OtherData = otherData;
        }


        public AjaxResult(bool success)
        {
            this.Success = success;
        }

        public string toJsonString()
        {
           var b = JsonConvert.SerializeObject(this);
            return b;
        }
    }
}
