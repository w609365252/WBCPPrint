using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Crp.Tools.EncryptionDecryption;
using CsharpHttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Crp.Tools.Api
{

    /// <summary>
    /// 快递100 api 帮助类
    /// </summary>
     public class Express100ApiHelper
    {
        static string appid = "bZUywK0crd5n";
        static string appsecret = "6dc0715c2b214cfea692c5b1d0a399be";
        static long appuid = 52341656;
        static string sign = "";
        static string baseParam = "";
        public Express100ApiHelper() {
            var timestamp = DateTime.Now.Ticks / 1000;
            sign = EncryptionDecryption.MD5.GetMD5(appsecret + EncryptionDecryption.MD5.GetMD5(appid + timestamp + appuid, true),true);
            baseParam = string.Format("appid={0}&appuid={1}&timestamp={2}&sign={3}", appid,appuid,timestamp,sign);
        }


        /// <summary>
        /// 订单信息导入接口        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool CreateExpressOrder(ExpressOrderData data) {
            var parmStr = JsonConvert.SerializeObject(data);
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://b.kuaidi100.com/v2/open/api.do?method=send",
                Method = "post",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36",
                Accept = "application/json;charset=UTF-8",
                ContentType = "application/x-www-form-urlencoded;charset=UTF-8",
                Postdata = baseParam + "&data="+ HttpUtility.UrlEncode(parmStr)
            };
            var result = helper.GetHtml(item);
            var m = JObject.Parse(result.Html);
            return Convert.ToBoolean(m["result"]);
        }

        /// <summary>
        /// 修改订单信息        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateExpressOrder(ExpressOrderData data)
        {

            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://b.kuaidi100.com/v2/open/api.do?method=update",
                Method = "post",
                Accept = "application/json;charset=UTF-8",
                ContentType = "application/x-www-form-urlencoded",
                Postdata = baseParam + "&data=" + data
            };
            var result = helper.GetHtml(item);
            var m = JObject.Parse(result.Html);
            return Convert.ToBoolean(m["result"]);
        }
        
        /// <summary>
        /// 获取待打印的地址
        /// </summary>
        /// <param name="orders">订单号 用,分隔</param>
        /// <returns></returns>
        public string GetPrintUrl(string ordernos)
        {
            var url = "http://b.kuaidi100.com/v2/open/api.do?method=print&"+baseParam+"&printlist="+ordernos;
            return url;
        }
    }

    /// <summary>
    /// 创建运单参数
    /// </summary>
    public class ExpressOrderData {

        /// <summary>
        /// 收件人电话
        /// </summary>
        public string recMobile { get; set; }
        /// <summary>
        /// 收件人固话
        /// </summary>
        public string recTel { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string recName { get; set; }

        /// <summary>
        /// 收件人详细地址
        /// </summary>
        public string recAddr { get; set; }

        /// <summary>
        /// 寄件人电话
        /// </summary>
        public string sendMobile { get; set; }

        /// <summary>
        /// 寄件人固话
        /// </summary>
        public string sendTel { get; set; }

        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string sendName { get; set; }

        /// <summary>
        /// 寄件地址
        /// </summary>
        public string sendAddr { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }


    /// <summary>
    /// 运单打印回调
    /// </summary>
    public class ExpressOrderCallBack{
        public string appid { get; set; }

        public string appuid{ get; set; }

        public string timestamp { get; set; }

        public string sign { get; set; }

        public string type { get; set; }

        public string orderNum { get; set; }

        public string kuaidicom { get; set; }

        public string kuaidinum { get; set; }
    }
}
