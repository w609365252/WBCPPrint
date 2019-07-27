using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CpPrint.Main;

namespace CpPrint.ViewModel
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class Config
    {
        public static bool IsBeginBet { get; set; }

        public static bool IsRealBet { get; set; } = true;

        public static bool IsMonitor { get; set; } = true;

        public static DateTime? BeginTime { get; set; }


        public static List<int> Moneys { get; set; }

        /// <summary>
        /// 选择的彩种信息 
        /// </summary>
        public static CateModel Cate = new CateModel();

        public static DateTime CurrentTime { get; set; }

    }

    public class CateModel
    {

        /// <summary>
        /// 车道前缀 比如 北京赛车DX  
        /// </summary>
        public string CarRoadID { get; set; }


        public string CateNo { get; set; } = "PK10JSC";

        public string CateName { get; set; } = "极速赛车";

        /// <summary>
        /// 下注时的彩种ID
        /// </summary>
        public string CateBetID { get; set; }

        /// <summary>
        /// 监听开奖的路径
        /// </summary>
        public string CatMonitorUrl { get; set; }
    }

    /// <summary>
    /// 投注名次列表
    /// </summary>
    public class RankModel
    {
        /// <summary>
        /// 名词 冠军
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 名次 1 
        /// </summary>
        public string RankNumber { get; set; }

        /// <summary>
        /// 车次
        /// </summary>
        public string CarRoadNum { get; set; }

        /// <summary>
        /// 未连开次数
        /// </summary>
        public int Count { get; set; }


        /// <summary>
        /// 已投注次数
        /// </summary>
        public int BetCount { get; set; }

        public string MaxMoney { get; set; }

        public string IsOpen { get; set; } = "";

    }
}
