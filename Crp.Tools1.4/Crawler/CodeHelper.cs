using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crp.Tools.Crawler
{
    public class CodeHelper
    {
        private static String m_softKey = "54605903da6ca03cafc8116dfcf13cf9";
        private static String m_userName = "wcx793935690";
        private static String m_password = "wcx139";

        /// <summary>
        /// 暂时接打码平台 测试期间
        /// </summary>
        /// <param name="bytes"></param>        
        /// <returns></returns>
        public static string GetCodeImage(byte[] bytes)
        {
            try
            {
                //请求答题
                StringBuilder VCodeText = new StringBuilder(100);
                int ret = Dama2.D2Buf(
                    m_softKey, //softawre key (software id)
                    m_userName,    //user name
                    m_password,     //password
                    bytes,         //图片数据，图片数据不可大于4M
                    (uint)bytes.Length, //图片数据长度
                    60,         //超时时间，单位为秒，更换为实际需要的超时时间
                    4,        //验证码类型ID，参见 http://wiki.dama2.com/index.php?n=ApiDoc.GetSoftIDandKEY   //23 四位英文
                    VCodeText); //成功时返回验证码文本（答案)
                return VCodeText.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
