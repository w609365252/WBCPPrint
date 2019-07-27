using CpPrint.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.Tools
{
    public class CateTool
    {
        public static List<CateOddsModel> LoadGames(string cateName = "")
        {
            if (cateName.Contains("幸运10") || cateName.Contains("极速赛车")|| cateName.Contains("幸运飞艇") || cateName.Contains("北京赛车")) {
                //数字盘 直接用数字比对就行
                var listStr1 = "[{\"CateNo\":\"GYH_3\",\"Odds\":\"\",\"Names\":\"和_3\"},{\"CateNo\":\"GYH_4\",\"Odds\":\"\",\"Names\":\"和_4\"},{\"CateNo\":\"GYH_5\",\"Odds\":\"\",\"Names\":\"和_5\"},{\"CateNo\":\"GYH_6\",\"Odds\":\"\",\"Names\":\"和_6\"},{\"CateNo\":\"GYH_7\",\"Odds\":\"\",\"Names\":\"和_7\"},{\"CateNo\":\"GYH_8\",\"Odds\":\"\",\"Names\":\"和_8\"},{\"CateNo\":\"GYH_9\",\"Odds\":\"\",\"Names\":\"和_9\"},{\"CateNo\":\"GYH_10\",\"Odds\":\"\",\"Names\":\"和_10\"},{\"CateNo\":\"GYH_11\",\"Odds\":\"\",\"Names\":\"和_11\"},{\"CateNo\":\"GYH_12\",\"Odds\":\"\",\"Names\":\"和_12\"},{\"CateNo\":\"GYH_13\",\"Odds\":\"\",\"Names\":\"和_13\"},{\"CateNo\":\"GYH_14\",\"Odds\":\"\",\"Names\":\"和_14\"},{\"CateNo\":\"GYH_15\",\"Odds\":\"\",\"Names\":\"和_15\"},{\"CateNo\":\"GYH_16\",\"Odds\":\"\",\"Names\":\"和_16\"},{\"CateNo\":\"GYH_17\",\"Odds\":\"\",\"Names\":\"和_17\"},{\"CateNo\":\"GYH_18\",\"Odds\":\"\",\"Names\":\"和_18\"},{\"CateNo\":\"GYH_19\",\"Odds\":\"\",\"Names\":\"和_19\"},{\"CateNo\":\"GDS_D\",\"Odds\":\"\",\"Names\":\"和_单\"},{\"CateNo\":\"GDS_S\",\"Odds\":\"\",\"Names\":\"和_双\"},{\"CateNo\":\"GDX_D\",\"Odds\":\"\",\"Names\":\"和_大\"},{\"CateNo\":\"GDX_X\",\"Odds\":\"\",\"Names\":\"和_小\"}]";
                var list1 = JsonConvert.DeserializeObject<List<CateOddsModel>>(listStr1);
                var listStr2 = "[{\"CateNo\":\"DX1_D\",\"Odds\":\"\",\"Names\":\"1_大\"},{\"CateNo\":\"DX1_X\",\"Odds\":\"\",\"Names\":\"1_小\"},{\"CateNo\":\"DS1_D\",\"Odds\":\"\",\"Names\":\"1_单\"},{\"CateNo\":\"DS1_S\",\"Odds\":\"\",\"Names\":\"1_双\"},{\"CateNo\":\"LH1_L\",\"Odds\":\"\",\"Names\":\"1_龙\"},{\"CateNo\":\"LH1_H\",\"Odds\":\"\",\"Names\":\"1_虎\"},{\"CateNo\":\"DX2_D\",\"Odds\":\"\",\"Names\":\"2_大\"},{\"CateNo\":\"DX2_X\",\"Odds\":\"\",\"Names\":\"2_小\"},{\"CateNo\":\"DS2_D\",\"Odds\":\"\",\"Names\":\"2_单\"},{\"CateNo\":\"DS2_S\",\"Odds\":\"\",\"Names\":\"2_双\"},{\"CateNo\":\"LH2_L\",\"Odds\":\"\",\"Names\":\"2_龙\"},{\"CateNo\":\"LH2_H\",\"Odds\":\"\",\"Names\":\"2_虎\"},{\"CateNo\":\"DX3_D\",\"Odds\":\"\",\"Names\":\"3_大\"},{\"CateNo\":\"DX3_X\",\"Odds\":\"\",\"Names\":\"3_小\"},{\"CateNo\":\"DS3_D\",\"Odds\":\"\",\"Names\":\"3_单\"},{\"CateNo\":\"DS3_S\",\"Odds\":\"\",\"Names\":\"3_双\"},{\"CateNo\":\"LH3_L\",\"Odds\":\"\",\"Names\":\"3_龙\"},{\"CateNo\":\"LH3_H\",\"Odds\":\"\",\"Names\":\"3_虎\"},{\"CateNo\":\"DX4_D\",\"Odds\":\"\",\"Names\":\"4_大\"},{\"CateNo\":\"DX4_X\",\"Odds\":\"\",\"Names\":\"4_小\"},{\"CateNo\":\"DS4_D\",\"Odds\":\"\",\"Names\":\"4_单\"},{\"CateNo\":\"DS4_S\",\"Odds\":\"\",\"Names\":\"4_双\"},{\"CateNo\":\"LH4_L\",\"Odds\":\"\",\"Names\":\"4_龙\"},{\"CateNo\":\"LH4_H\",\"Odds\":\"\",\"Names\":\"4_虎\"},{\"CateNo\":\"DX5_D\",\"Odds\":\"\",\"Names\":\"5_大\"},{\"CateNo\":\"DX5_X\",\"Odds\":\"\",\"Names\":\"5_小\"},{\"CateNo\":\"DS5_D\",\"Odds\":\"\",\"Names\":\"5_单\"},{\"CateNo\":\"DS5_S\",\"Odds\":\"\",\"Names\":\"5_双\"},{\"CateNo\":\"LH5_L\",\"Odds\":\"\",\"Names\":\"5_龙\"},{\"CateNo\":\"LH5_H\",\"Odds\":\"\",\"Names\":\"5_虎\"},{\"CateNo\":\"DX6_D\",\"Odds\":\"\",\"Names\":\"6_大\"},{\"CateNo\":\"DX6_X\",\"Odds\":\"\",\"Names\":\"6_小\"},{\"CateNo\":\"DS6_D\",\"Odds\":\"\",\"Names\":\"6_单\"},{\"CateNo\":\"DS6_S\",\"Odds\":\"\",\"Names\":\"6_双\"},{\"CateNo\":\"DX7_D\",\"Odds\":\"\",\"Names\":\"7_大\"},{\"CateNo\":\"DX7_X\",\"Odds\":\"\",\"Names\":\"7_小\"},{\"CateNo\":\"DS7_D\",\"Odds\":\"\",\"Names\":\"7_单\"},{\"CateNo\":\"DS7_S\",\"Odds\":\"\",\"Names\":\"7_双\"},{\"CateNo\":\"DX8_D\",\"Odds\":\"\",\"Names\":\"8_大\"},{\"CateNo\":\"DX8_X\",\"Odds\":\"\",\"Names\":\"8_小\"},{\"CateNo\":\"DS8_D\",\"Odds\":\"\",\"Names\":\"8_单\"},{\"CateNo\":\"DS8_S\",\"Odds\":\"\",\"Names\":\"8_双\"},{\"CateNo\":\"DX9_D\",\"Odds\":\"\",\"Names\":\"9_大\"},{\"CateNo\":\"DX9_X\",\"Odds\":\"\",\"Names\":\"9_小\"},{\"CateNo\":\"DS9_D\",\"Odds\":\"\",\"Names\":\"9_单\"},{\"CateNo\":\"DS9_S\",\"Odds\":\"\",\"Names\":\"9_双\"},{\"CateNo\":\"DX10_D\",\"Odds\":\"\",\"Names\":\"10_大\"},{\"CateNo\":\"DX10_X\",\"Odds\":\"\",\"Names\":\"10_小\"},{\"CateNo\":\"DS10_D\",\"Odds\":\"\",\"Names\":\"10_单\"},{\"CateNo\":\"DS10_S\",\"Odds\":\"\",\"Names\":\"10_双\"}]";
                var list2 = JsonConvert.DeserializeObject<List<CateOddsModel>>(listStr2);
                list1.AddRange(list2);
                for (int i = 1; i < 11; i++)
                {
                    for (int k = 1; k < 11; k++)
                    {
                        var m = new CateOddsModel() {
                            CateNo = "B" + i + "_" + k,
                            Names = i + "_" + k,
                            Odds = ""
                        };
                        list1.Add(m);
                    }
                }
                return list1;
            }
            if (cateName.Contains("幸运5") ||  cateName.Contains("重庆时时彩")) {
                //双面盘  总和大小 前三后三中
                var listStr1 = "[{\"CateNo\":\"ZDX_D\",\"Odds\":\"\",\"Names\":\"和_大\"},{\"CateNo\":\"ZDX_X\",\"Odds\":\"\",\"Names\":\"和_小\"},{\"CateNo\":\"ZDS_D\",\"Odds\":\"\",\"Names\":\"和_单\"},{\"CateNo\":\"ZDS_S\",\"Odds\":\"\",\"Names\":\"和_双\"},{\"CateNo\":\"LH_L\",\"Odds\":\"\",\"Names\":\"1_龙\"},{\"CateNo\":\"LH_H\",\"Odds\":\"\",\"Names\":\"1_虎\"},{\"CateNo\":\"LH_T\",\"Odds\":\"\",\"Names\":\"1_和\"},{\"CateNo\":\"TS1_0\",\"Odds\":\"\",\"Names\":\"前三_豹子\"},{\"CateNo\":\"TS1_1\",\"Odds\":\"\",\"Names\":\"前三_顺子\"},{\"CateNo\":\"TS1_2\",\"Odds\":\"\",\"Names\":\"前三_对子\"},{\"CateNo\":\"TS1_3\",\"Odds\":\"\",\"Names\":\"前三_半顺\"},{\"CateNo\":\"TS1_4\",\"Odds\":\"\",\"Names\":\"前三_杂六\"},{\"CateNo\":\"TS2_0\",\"Odds\":\"\",\"Names\":\"中三_豹子\"},{\"CateNo\":\"TS2_1\",\"Odds\":\"\",\"Names\":\"中三_顺子\"},{\"CateNo\":\"TS2_2\",\"Odds\":\"\",\"Names\":\"中三_对子\"},{\"CateNo\":\"TS2_3\",\"Odds\":\"\",\"Names\":\"中三_半顺\"},{\"CateNo\":\"TS2_4\",\"Odds\":\"\",\"Names\":\"中三_杂六\"},{\"CateNo\":\"TS3_0\",\"Odds\":\"\",\"Names\":\"后三_豹子\"},{\"CateNo\":\"TS3_1\",\"Odds\":\"\",\"Names\":\"后三_顺子\"},{\"CateNo\":\"TS3_2\",\"Odds\":\"\",\"Names\":\"后三_对子\"},{\"CateNo\":\"TS3_3\",\"Odds\":\"\",\"Names\":\"后三_半顺\"},{\"CateNo\":\"TS3_4\",\"Odds\":\"\",\"Names\":\"后三_杂六\"}]";
                var list1 = JsonConvert.DeserializeObject<List<CateOddsModel>>(listStr1);

                //1-5 大小单双
                var listStr2 = "[{\"CateNo\":\"DX1_D\",\"Odds\":\"\",\"Names\":\"1_大\"},{\"CateNo\":\"DX1_X\",\"Odds\":\"\",\"Names\":\"1_小\"},{\"CateNo\":\"DS1_D\",\"Odds\":\"\",\"Names\":\"1_单\"},{\"CateNo\":\"DS1_S\",\"Odds\":\"\",\"Names\":\"1_双\"},{\"CateNo\":\"DX2_D\",\"Odds\":\"\",\"Names\":\"2_大\"},{\"CateNo\":\"DX2_X\",\"Odds\":\"\",\"Names\":\"2_小\"},{\"CateNo\":\"DS2_D\",\"Odds\":\"\",\"Names\":\"2_单\"},{\"CateNo\":\"DS2_S\",\"Odds\":\"\",\"Names\":\"2_双\"},{\"CateNo\":\"DX3_D\",\"Odds\":\"\",\"Names\":\"3_大\"},{\"CateNo\":\"DX3_X\",\"Odds\":\"\",\"Names\":\"3_小\"},{\"CateNo\":\"DS3_D\",\"Odds\":\"\",\"Names\":\"3_单\"},{\"CateNo\":\"DS3_S\",\"Odds\":\"\",\"Names\":\"3_双\"},{\"CateNo\":\"DX4_D\",\"Odds\":\"\",\"Names\":\"4_大\"},{\"CateNo\":\"DX4_X\",\"Odds\":\"\",\"Names\":\"4_小\"},{\"CateNo\":\"DS4_D\",\"Odds\":\"\",\"Names\":\"4_单\"},{\"CateNo\":\"DS4_S\",\"Odds\":\"\",\"Names\":\"4_双\"},{\"CateNo\":\"DX5_D\",\"Odds\":\"\",\"Names\":\"5_大\"},{\"CateNo\":\"DX5_X\",\"Odds\":\"\",\"Names\":\"5_小\"},{\"CateNo\":\"DS5_D\",\"Odds\":\"\",\"Names\":\"5_单\"},{\"CateNo\":\"DS5_S\",\"Odds\":\"\",\"Names\":\"5_双\"}]";
                var list2 = JsonConvert.DeserializeObject<List<CateOddsModel>>(listStr2);

                list1.AddRange(list2);
                for (int i = 1; i < 6; i++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        var m = new CateOddsModel()
                        {
                            CateNo = "B" + i + "_" + k,
                            Names = i + "_" + k,
                            Odds = ""
                        };
                        list1.Add(m);
                    }
                }
                return list1;
            }

            return new List<CateOddsModel>();
        }
    }

    public class CateOddsModel
    {
        /// <summary>
        /// 4036_1  当前这个选项的Id
        /// </summary>
        public string CateNo { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public string Odds { get; set; }

        /// <summary>
        /// 名称（比如和3）
        /// </summary>
        public string Names { get; set; }
    }
}
