using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.ViewModel
{
    /// <summary>
    /// 当前开启的任务
    /// </summary>
    public class Order
    {
        public string OrderNo { get; set; }

        public string ExpectNo { get; set; }

        public string BetContent { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int times { get; set; } = 1;
        public string BetMode { get; set; }
        public int BetModeType { get; set; }
        public int BetType { get; set; }
        public DateTime CreateTime { get; set; }
        public int? price { get; set; } = 0;
        public string Result { get; set; }
        //中奖记录
        public string DrawRecordStr { get; set; }
        public string State { get; set; }
        /// <summary>
        /// 1已兑奖
        /// </summary>
        public int DrawState { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int StreamNum { get; set; }
        /// <summary>
        /// 验票码
        /// </summary>
        public string Validate { get; set; }
        public string buyModeStr
        {
            get
            {
                string desc = "";
                if (!string.IsNullOrEmpty(BetContent))
                {
                    string[] s = this.BetContent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in s)
                    {
                        var obj = item.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        string numStr = obj[0];
                        string timesStr = obj.Count == 2 ? obj[1].ToString() : "1";
                        if (this.BetType == 1)
                        {
                            var list = numStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (this.BetModeType != 6)
                            {
                                list = list.OrderBy(m => Convert.ToInt32(m)).ToList();
                            }
                            desc += FormatBuyModeName(list);
                        }
                        else if (this.BetType == 2)
                        {
                            var list = numStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().OrderBy(m => Convert.ToInt32(m)).ToList();
                            desc += FormatBuyModeName(list);
                        }
                        else if (this.BetType == 3)
                        {
                            var list = numStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            string k = "";
                            if (list.Count == 2)
                            {
                                var danList = list[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().OrderBy(m => Convert.ToInt32(m)).ToList();
                                var maList = list[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().OrderBy(m => Convert.ToInt32(m)).ToList();
                                k = FormatBuyModeName(danList) + "^" + FormatBuyModeName(maList);
                                desc += k;
                            }
                        }
                        desc += $"  [{timesStr}倍]" + ",";
                    }
                    
                }
                return desc.Trim(',');
            }
        }

        string FormatBuyModeName(List<string> numbers)
        {
            string desc = "";
            foreach (var num in numbers)
            {
                int number = Convert.ToInt32(num);
                if (number > 0 && number < 21)
                {
                    desc += "\n" + GetEnumDescription((NumDesc)number) + num.PadLeft(2, '0') + "+";
                }
            }
            return desc.Trim('+');
        }

        //public bool IsReal { get; set; }

        string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }

    enum NumDesc
    {
        [Description("西瓜")]
        XiGua = 1,
        [Description("椰子")]
        YeZi = 2,
        [Description("榴莲")]
        LiuLian = 3,
        [Description("柚子")]
        YouZi = 4,
        [Description("菠萝")]
        BoLuo = 5,
        [Description("葡萄")]
        PuTao = 6,
        [Description("荔枝")]
        LiZhi = 7,
        [Description("樱桃")]
        YingTao = 8,
        [Description("草莓")]
        CaoMei = 9,
        [Description("番茄")]
        FanQie = 10,
        [Description("梨子")]
        LiZi = 11,
        [Description("苹果")]
        PingGuo = 12,
        [Description("桃子")]
        TaoZi = 13,
        [Description("柑橘")]
        GanJu = 14,
        [Description("冬瓜")]
        DongGua = 15,
        [Description("萝卜")]
        LuoBo = 16,
        [Description("南瓜")]
        NanGua = 17,
        [Description("茄子")]
        QieZi = 18,
        [Description("家犬")]
        JiaQuan = 19,
        [Description("奶牛")]
        NaiNiu = 20,
    }

    

}