using System;
using System.Collections.Generic;

namespace Crp.Tools.EnumHelper
{
    /// <summary>
    /// 枚举组操作, 使用该方法, 需要把枚举标识为[Flags]
    /// </summary>
    
    public static class EnumManager
    {
        /// <summary>
        /// 把枚举字符串的和值转为对应的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="sum">枚举的和值字符串形式</param>
        /// <returns></returns>
        public static T ConverEnum<T>(string sum) where T : struct
        {
            T perm = (T)Enum.Parse(typeof(T), sum);
            return perm;
        }

        /// <summary>
        /// 把枚举long和值转为对应的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enum">枚举int和值形式</param>
        /// <returns></returns>
        public static T ConverEnum<T>(long @enum) where T : struct
        {
            T perm = (T)Enum.Parse(typeof(T), @enum.ToString());
            return perm;
        }

        /// <summary>
        /// 获得枚举的和值
        /// </summary>
        /// <param name="enum">枚举</param>
        /// <returns>和值</returns>
        public static long GetEnumIntValue(this Enum @enum) => Convert.ToInt64(@enum);

        /// <summary>
        /// 判断该枚举组是否包含某单个枚举
        /// </summary>
        /// <param name="type">枚举</param>
        /// <param name="value">单个枚举</param>
        /// <returns>是否有</returns>
        public static bool HasPermission(this Enum type, Enum value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断该枚举组是否等于第二个枚举组
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value">枚举值</param>
        /// <returns>是否相等</returns>
        public static bool IsEquals(this Enum type, Enum value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加新枚举，记得处理异常
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="type"></param>
        /// <param name="value">枚举值</param>
        /// <returns>添加完成后的枚举组</returns>
        public static T AddPermission<T>(this Enum type, Enum value) where T : struct
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除枚举，记得处理异常
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="type"></param>
        /// <param name="value">要移除的枚举</param>
        /// <returns>移除后的枚举组</returns>
        public static T RemovePermission<T>(this Enum type, Enum value) where T : struct
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 把二进制枚举组转为正常的List枚举集合(记得处理异常)
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="per">枚举组元数据</param>
        /// <returns>枚举集合</returns>
        public static List<T> PerToList<T>(this Enum per) where T : struct
        {
            List<T> result = new List<T>();
            try
            {
                string[] perString = per.ToString().Split(',');
                if (perString != null && perString.Length > 0)
                    foreach (var item in perString)
                        result.Add((T)Enum.Parse(typeof(T), item));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}