using Crp.Tools.DataTypeExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Crp.Tools
{
    /// <summary>
    /// 一些公共的扩展方法或者封装方法
    /// </summary>
    public static class Tool
    {
        /// <summary>
        /// 大写字母
        /// </summary>
        private readonly static char[] Capital = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

        /// <summary>
        /// 小写字母
        /// </summary>
        private readonly static char[] Lowercase = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };

        /// <summary>
        /// 数字
        /// </summary>
        private readonly static char[] Number = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        /// <summary>
        /// 生成订单号 (20位纯数字, 格式为yyyyMMddHHmmss+随机6位数)
        /// </summary>
        /// <returns>生成的订单号字符串</returns>
        
        public static string GetOrderNO()
        {
            Thread.Sleep(15);
            return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random().Next(100000, 1000000)}";
        }

        /// <summary>
        /// 生成订单号 (纯数字, 格式为yyyyMMddHHmmss+随机数)
        /// </summary>
        /// <param name="length">订单号后面随机数长度, 最大长度支持为10位, 超出报错</param>
        /// <returns>生成的订单号字符串</returns>
        public static string GetOrderNO(int length)
        {
            if (length > 10)
            {
                throw new Exception("随机数长度最大支持为10");
            }
            StringBuilder item = new StringBuilder("1");
            for (int i = 1; i < length; i++)
            {
                item.Append("0");
            }
            int start = Convert.ToInt32(item.ToString());
            item.Append("0");
            int end = (int)Convert.ToInt64(item.ToString()) - 1;
            Thread.Sleep(15);
            return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random().Next(start, end)}";
        }

        /// <summary>
        /// 生成订单号 (纯数字, 格式为 订单号前缀+yyyyMMddHHmmss+随机数)
        /// </summary>
        /// <param name="orderStart">订单号前缀字符串</param>
        /// <param name="length">订单号后面随机数长度, 最大长度支持为10位, 超出报错</param>
        /// <returns>生成的订单号字符串</returns>
        
        public static string GetOrderNO(string orderStart, int length)
        {
            if (length > 10)
            {
                throw new Exception("随机数长度最大支持为10");
            }
            StringBuilder item = new StringBuilder("1");
            for (int i = 1; i < length; i++)
            {
                item.Append("0");
            }
            int start = Convert.ToInt32(item.ToString());
            item.Append("0");
            int end = (int)Convert.ToInt64(item.ToString()) - 1;
            Thread.Sleep(15);
            return $"{orderStart}{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random().Next(start, end)}";
        }

        /// <summary>
        /// 获得当天0点
        /// </summary>
        /// <returns></returns>
        
        public static DateTime GetToday00() => DateTime.Now.ToString("yyyy-MM-dd").ToDateTime();

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="field">属性名称</param>
        /// <returns></returns>
        
        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        /// <summary>
        /// 获得指定长度的验证码, 大小写和数字混合
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        
        public static string GetCodeMixed(int length)
        {
            List<char> listChar = new List<char>();
            listChar.AddRange(Capital);
            listChar.AddRange(Lowercase);
            listChar.AddRange(Number);
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Thread.Sleep(15);
                sb.Append(listChar[random.Next(0, listChar.Count)]);
            }
            listChar.Clear(); listChar = null;
            return sb.ToString();
        }

        /// <summary>
        /// 获得指定长度的验证码, 只包含数字
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        
        public static string GetCodeOnlyNumber(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Thread.Sleep(15);
                sb.Append(Number[random.Next(0, Number.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得指定长度的验证码, 只包含大写字母
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        
        public static string GetCodeOnlyCapital(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Thread.Sleep(15);
                sb.Append(Capital[random.Next(0, Capital.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得指定长度的验证码, 只包含小写字母
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        
        public static string GetCodeOnlyLowercase(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Thread.Sleep(15);
                sb.Append(Lowercase[random.Next(0, Lowercase.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得指定长度的验证码, 大小写混合
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        
        public static string GetCodeLetter(int length)
        {
            List<char> listChar = new List<char>();
            listChar.AddRange(Capital);
            listChar.AddRange(Lowercase);
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Thread.Sleep(15);
                sb.Append(listChar[random.Next(0, listChar.Count)]);
            }
            listChar.Clear(); listChar = null;
            return sb.ToString();
        }
    }
}