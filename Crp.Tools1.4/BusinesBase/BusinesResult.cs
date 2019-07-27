using System;
namespace Crp.Tools.BusinesBase
{
    /// <summary>
    /// 业务层返回值封装, 为了更好的规范, 特此封装
    /// </summary>
    /// <typeparam name="Result">返回值类型</typeparam>
    
    public class BusinessResult<Result>
    {
        /// <summary>
        /// 业务层返回值封装
        /// </summary>
        internal BusinessResult() { }

        /// <summary>
        /// 获取返回值
        /// </summary>
        public Result Value { get; internal set; }

        /// <summary>
        /// 执行后的消息，成功执行该属性值为string.Empty
        /// </summary>
        public string Msg { get; internal set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool Success { get; internal set; }

        /// <summary>
        /// 发生异常后的异常对象, 如果没有异常, 则为null
        /// </summary>
        public Exception Exception { get; set; } = null;
    }
}