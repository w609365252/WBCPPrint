using Crp.Tools.NLog;
using Crp.Tools;
using System;

namespace Crp.Tools.BusinesBase
{
    /// <summary>
    /// 所有逻辑层父类
    /// </summary>
    public class BusinessBase
    {
        /// <summary>
        /// 加锁对象
        /// </summary>
        protected static object LockObject = new object();

        /// <summary>
        /// 执行方法, 并且返回
        /// </summary>
        /// <typeparam name="Result">返回值类型, 如果执行失败, 返回的是default(Result)的值</typeparam>
        /// <param name="func">委托</param>
        /// <returns>BusinesResult</returns>
        
        protected BusinessResult<Result> Runing<Result>(Func<Result> func)
        {
            try
            {
                if (func == null) throw new Exception("方法体为空, 处理无意义, 异常来自BusinesBase类的Runing方法");
                Result result = func();
                return new BusinessResult<Result>
                {
                    Msg = string.Empty,
                    Success = true,
                    Value = result
                };
            }
            catch (ParamsException ex)
            {
                return new BusinessResult<Result>
                {
                    Msg = ex.Message,
                    Success = false,
                    Value = default(Result),
                    Exception = ex
                };
            }
            catch (Exception ex)
            {
                LogerManager.Error(ex);
                return new BusinessResult<Result>
                {
                    Msg = ex.Message,
                    Success = false,
                    Value = default(Result),
                    Exception = ex
                };
            }
        }
    }
}