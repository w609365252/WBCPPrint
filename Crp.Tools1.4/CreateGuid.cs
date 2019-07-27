using System;

namespace Crp.Tools
{
    /// <summary>
    /// 创建GUID类
    /// </summary>
    public class CreateGuid
    {
        /// <summary>
        /// 获得B格式的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetB_Guid() => Guid.NewGuid().ToString("B").ToUpper();

        /// <summary>
        /// 获得P格式的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetP_Guid() => Guid.NewGuid().ToString("P").ToUpper();

        /// <summary>
        /// 获得N格式的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetN_Guid() => Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 获得D格式的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetD_Guid() => Guid.NewGuid().ToString("D").ToUpper();
    }
}