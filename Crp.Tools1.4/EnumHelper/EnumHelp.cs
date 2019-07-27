using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Crp.Tools.EnumHelper
{
    /// <summary>
    /// 枚举封装方法
    /// </summary>
    
    public class EnumHelp
    {
        /// <summary>
        /// 返回枚举里面所有的枚举名称和值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>List元组集合，Item1为枚举名，Item2为枚举int值</returns>
        public static List<Tuple<string, int>> GetNameValue<T>() where T : struct
        {
            var list = new List<Tuple<string, int>>();
            foreach (var value in Enum.GetValues(typeof(T)))
                list.Add(new Tuple<string, int>(value.ToString(), Convert.ToInt32(value)));
            return list;
        }

        /// <summary>
        /// 返回枚举里面所有的枚举描述值和枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>List元组集合，Item1为枚举描述值，Item2为枚举int值</returns>
        public static List<Tuple<string, int>> GetDescValue<T>() where T : struct
        {
            var list = new List<Tuple<string, int>>();
            foreach (var value in Enum.GetValues(typeof(T)))
                list.Add(new Tuple<string, int>(GetDescription<T>((T)value), Convert.ToInt32(value)));
            return list;
        }

        /// <summary>
        /// 获取枚举Description的描述值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举</param>
        /// <returns>Description描述的字符串值</returns>
        public static string GetDescription<T>(T value) where T : struct
        {
            FieldInfo field = typeof(T).GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 返回枚举类型里面所有的枚举名称，枚举值和枚举描述值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>List的元组集合，Item1为枚举名，Item2为枚举int值，Item3为枚举的描述值</returns>
        public static List<Tuple<string, int, string>> GetNameValueDescription<T>() where T : struct
        {
            var list = new List<Tuple<string, int, string>>();
            foreach (var value in Enum.GetValues(typeof(T)))
                list.Add(new Tuple<string, int, string>(value.ToString(), Convert.ToInt32(value), GetDescription<T>((T)value)));
            return list;
        }
    }
}