using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.ViewModel
{
    public class BetModeModel
    {
        /// <summary>
        /// 游戏方式枚举
        /// </summary>
        public int BetMode { get; set; }
        /// <summary>
        /// 投注最小数字号
        /// </summary>
        public int BetMinCount { get; set; }
        /// <summary>
        /// 投注最大数字号
        /// </summary>
        public int BetMaxCount { get; set; }
        /// <summary>
        /// 胆数量（只有胆拖玩法才会有）
        /// </summary>
        public int DanCount { get; set; } = 0;
        public string RuleName { get; set; } = "";
        /// <summary>
        /// 投注金额 每个投注数量都会对应金额
        /// </summary>
        public List<PriceModel> BetPriceMode { get; set; }
        /// <summary>
        /// 中奖金额 中奖几个数字对应金额
        /// </summary>
        public List<PriceModel> DrawMode { get; set; }
    }

    public class PlayRule
    {
        /// <summary>
        /// 投注标注
        /// </summary>
        public string BetName { get; set; }
        /// <summary>
        /// 投注玩法类型
        /// </summary>
        public int BetType { get; set; }
        /// <summary>
        /// 玩法种类
        /// </summary>
        public List<BetModeModel> BetMode { get; set; }
    }

    public class PriceModel
    {
        public int num { get; set; }
        public int price { get; set; }
    }

}
