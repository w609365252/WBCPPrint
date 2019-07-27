using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpPrint
{
    public static class DateTimeTools
    {
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }

        /// <summary>
        /// 毫秒 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
            return t;
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <param name="isSecond">是否为秒</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime GetTime(string timeStamp,bool isSecond)
        {
            long lTime = long.Parse(timeStamp);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            if (isSecond)
            {
                DateTime dt = startTime.AddSeconds(lTime);
                return dt;
            }
            else {
                DateTime dt = startTime.AddMilliseconds(lTime);
                return dt;

            }
            
            
        }

    }
}
