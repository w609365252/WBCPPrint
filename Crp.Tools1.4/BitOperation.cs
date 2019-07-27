using System.Collections.Generic;

namespace Crp.Tools
{
    /// <summary>
    /// 位运算
    /// </summary>
    public static class BitOperation
    {
        /// <summary>
        /// 判断数字是不是2的N次方
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsFlag(this long num)
        {
            if (num < 1) return false;
            return (num & num - 1) == 0;
        }

        /// <summary>
        /// 累加位运算
        /// </summary>
        /// <param name="d">源</param>
        /// <param name="value">要累加的值</param>
        /// <returns></returns>
        
        public static long AddByBit(this long d, long value)
        {
            if (!IsFlag(value))
            {
                throw new System.Exception($"{value}不是2的N次方数字");
            }
            return (d | value);
        }

        /// <summary>
        /// 减去位运算
        /// </summary>
        /// <param name="d">源</param>
        /// <param name="value">要减去的值</param>
        /// <returns></returns>
        public static long RemoveBit(this long d, long value)
        {
            if (!IsFlag(value))
            {
                throw new System.Exception($"{value}不是2的N次方数字");
            }
            return (d & ~value);
        }
        /// <summary>
        /// 数字转Lsit集合
        /// </summary>
        /// <param name="d">源</param>
        /// <returns></returns>
        public static List<long> BitToList(this long d)
        {
            List<long> result = new List<long>();
            for (long i = 1; ;)
            {
                if ((d & i) > 0)
                {
                    result.Add(i);
                }
                if ((i *= 2) >= d)
                {
                    break;
                }
            }
            return result;
        }
    }
}