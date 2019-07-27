using CpPrint.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.Apis
{
    public abstract class ApiBase
    {
        public WriteLogDelegate WriteLogEvent { get; set; }
        public SendVoiceDelegate SendVoiceEvent { get; set; }
        public SaveRecordDelegate SaveRecordEvent { get; set; }
        public string Url = "";
        public string Cookie = "";
        /// <summary>
        /// 获取验证码  并识别
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public abstract string GetCode();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">账户</param>
        /// <param name="pass">密码</param>
        /// <param name="url">网站域名地址</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public abstract UserInfo Login(string userName, string pass, string code = "");

        /// <summary>
        /// 获取下期开奖信息
        /// </summary>
        /// <returns></returns>
        public abstract OpenNext GetNextOpenModel(string cateNo);

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="num">条数</param>
        /// <param name="cookie">当前登录用户的cookie</param>
        /// <returns></returns>
        public abstract List<OpenRecord> GetOldOpenCode(int num);

        /// <summary>
        /// 判断是否在线
        /// </summary>
        /// <returns></returns>
        public abstract bool IsOnline();

        /// <summary>
        /// 获取当前的赔率
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public abstract string GetRate();

        /// <summary>
        /// 开始投注
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="betExpectNo">期次</param>
        /// <param name="betModel">投注对象</param>
        /// <param name="money">金额</param>
        /// <returns></returns>
        public abstract string BeginBet(string str);

        /// <summary>
        /// 获取最大投注额 用于大额分头
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public abstract string GetMaxList(string cateName);

        /// <summary>
        /// 获取用户余额  今日盈亏
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public abstract string GetMoneyStr();



        /// <summary>
        /// 获取投注记录
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        //public abstract List<Record> GetBetRecord(string cookie);

    }

}
