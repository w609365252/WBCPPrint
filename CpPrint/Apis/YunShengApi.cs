using CpPrint.ViewModel;
using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Drawing;
using Crp.Tools.Crawler;
using Crp.Tools.DataTypeExtend;

namespace CpPrint.Apis
{
    public class YunShengApi : ApiBase
    {
        public YunShengApi(string url = "")
        {
            this.Url = url;
        }

        HttpHelper helper = new HttpHelper();

        public override string GetCode()
        {
            //HttpItem codeItem = new HttpItem()
            //{
            //    Method = "Get",
            //    URL = base.Url + "/code?_=1555401750411",
            //    UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/58.0.3029.110Safari/537.36SE2.XMetaSr1.0",
            //    Accept = "image/webp,image/apng,image/*,*/*;q=0.8",
            //    ResultType = CsharpHttpHelper.Enum.ResultType.Byte
            //};
            //var resCode = helper.GetHtml(codeItem);
            //this.Cookie = resCode.Cookie;
            //System.IO.MemoryStream ms = new System.IO.MemoryStream(resCode.ResultByte);
            //Image img = Image.FromStream(ms);
            //var code = CodeHelper.GeneralBasicDemo(img);
            //return code;
            return "";
        }

        private string GetHost(string url)
        {
            return url.Replace("http://", "").Replace("https://", "");
        }


        public override UserInfo Login(string userName, string pass, string code = "")
        {
            try
            {
                HttpItem cookieItem = new HttpItem()
                {
                    Host = GetHost(base.Url),
                    URL = Url + "/ssid1?url=/",
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8",
                    Method = "GET",
                    UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/71.0.3578.98Safari/537.36",
                    Cookie = "ssid1=8f08f51f9c3a4f47d0a81569d0296baf; random=8135; affid=null",
                };
                var cookieRes = helper.GetHtml(cookieItem);
                var cookie = cookieRes.Cookie;

                HttpItem loginItem = new HttpItem()
                {
                    Host = GetHost(Url),
                    URL = Url + "/web/rest/cashlogin?account=" + userName + "&password=" + pass,
                    Accept = "application/json,text/plain,*/*",
                    Method = "Post",
                    UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/71.0.3578.98Safari/537.36",
                    Cookie = cookie
                };
                var loginRes = helper.GetHtml(loginItem);
                var obj = JObject.Parse(loginRes.Html);
                var issuccess = obj["success"].ToString();
                if (issuccess.ToLower() != "true")
                {
                    WriteLogEvent.Invoke("登录失败:"+ loginRes.Html); return null;
                }
                var agreenPar = obj["message"].ToString();
                HttpItem agreenHtmlItem = new HttpItem()
                {
                    Host = GetHost(Url),
                    URL = Url + "/member/agreement?" + agreenPar,
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                    Method = "GET",
                    UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/71.0.3578.98Safari/537.36",
                    Cookie = cookie
                };
                var agreeRes = helper.GetHtml(agreenHtmlItem);

                var date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var timestamp = DateTimeTools.GetTimeStamp(date);

                agreeRes.Cookie = agreeRes.Cookie.Replace("HttpOnly,", "");
                agreeRes.Cookie = agreeRes.Cookie.Replace("HttpOnly", "");
                agreeRes.Cookie = agreeRes.Cookie + "_skin_=blue; defaultLT=SGFT; " + cookie+";" ;
                base.Cookie = agreeRes.Cookie;
            
                UserInfo userInfo = new UserInfo();
                userInfo.LoginCookie = agreeRes.Cookie;
                var balance = GetMoneyStr();
                userInfo.Money = Convert.ToDecimal(balance);
                userInfo.State = "登陆成功";
                WriteLogEvent.Invoke("会员登录成功");
                return userInfo;
            }
            catch (Exception ee)
            {
                WriteLogEvent.Invoke("登录失败"); return null;
            }
        }

        //获取最近N期
        public override List<OpenRecord> GetOldOpenCode(int num)
        {
            HttpItem item = new HttpItem()
            {
                URL = base.Url + "/member/dresult?lottery=PK10JSC",
                Method = "Get",
                UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit /537.36(KHTML,likeGecko)Chrome/58.0.3029.110Safari/537.36SE2.XMetaSr1.0",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                Cookie = Cookie
            };
            var res = helper.GetHtml(item);
            var regBalls = new Regex(@"<tr>\s+<td class=""period"">(?<expectNo>.*?)</td>\s+<td class=""drawTime"">(?<drawTime>.*?)</td>(?<numStr>.*?)<td class=""other1"">.*?</td>.*?</tr>", RegexOptions.Singleline);
            var matches = regBalls.Matches(res.Html);
            List<OpenRecord> records = new List<OpenRecord>();
            foreach (Match m in matches)
            {
                var numStr = m.Groups["numStr"].Value;
                var numsReg = new Regex(@"<td class=""name ballname""><span class="".*?"">(?<num>.*?)</span></td>");
                var numsMatch = numsReg.Matches(numStr);
                var openNum = "";
                foreach (Match match in numsMatch)
                {
                    var numCode = match.Groups["num"].Value;
                    if (Convert.ToInt32(numCode) <= 9)
                    {
                        numCode = "0" + numCode;
                    }
                    openNum += numCode + ",";
                }
                openNum = openNum.Substring(0, openNum.Length - 1);
                var newRecord = new OpenRecord()
                {
                    OpenTime = DateTime.Now.Year + "-" + m.Groups["drawTime"].Value.Substring(0, 5) + " " + m.Groups["drawTime"].Value.Substring(7),
                    ExpectNo = m.Groups["expectNo"].Value,
                    OpenNo = openNum
                };
                records.Add(newRecord);
            }
            var resList = records.OrderByDescending(d => d.ExpectNo).Take(num).ToList();
            return resList;
        }

        /// <summary>
        /// 监听开奖是否达到指定期数未出现同号  例如100期开出1 号 101期开出 1号 就是开出跟上一期一样号码 就会自动开始启动 
        /// </summary>
        /// <returns></returns>
        public override OpenNext GetNextOpenModel(string cateNo = "")
        {
            try
            {
                HttpItem item = new HttpItem()
                {
                    URL = base.Url + "/member/period?lottery=" + cateNo,
                    Method = "GET",
                    Host = GetHost(Url),
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                    Cookie = base.Cookie,
                    Accept = "*/*",
                    ContentType = "application/json;charset=UTF-8"
                };
                item.Header.Add("X-Requested-With", "XMLHttpRequest");
                var res = helper.GetHtml(item);
                if (res.Html == "") { return null; }
                //{"closeTime":1555557445000,"currentTime":1555557368000,"drawDate":1555516800000,"drawNumber":"20580085","drawTime":1555557540000,"openTime":1555557240000,"pnumber":"20580084","status":1}
                var j = JObject.Parse(res.Html);
                var closeTime = j["closeTime"].ToString();
                var currentTime = j["currentTime"].ToString();
                var openTime = j["openTime"].ToString();
                var expectNo = j["drawNumber"].ToString();
                var pnumber = j["pnumber"].ToString();
                Config.CurrentTime = DateTimeTools.GetTime(currentTime, false); //赋值当前时间
                var model = new OpenNext()
                {
                    NextCloseTime = DateTimeTools.GetTime(closeTime, false),
                    NextExpectNo = expectNo,
                    NextOpenTime = DateTimeTools.GetTime(openTime, false),
                    CurrTime = Config.CurrentTime,
                    CurrExpectNo = pnumber
                };
                return model;
            }
            catch (Exception ee) { return null; }
        }

        //此盘口不需要赔率获取
        public override string GetRate()
        {
            return "";
        }


        public override string BeginBet(string str)
        {
            return "";
            //var obj = JObject.Parse(str);
            //var cateName = obj["cateName"].ToString();
            //var lottery = obj["lottery"].ToString();
            //var drawNumber = obj["drawNumber"].ToString();
            //var bets = obj["bets"].ToString();
            //HttpItem item = new HttpItem()
            //{
            //    URL = base.Url + "/member/bet",
            //    Method = "POST",
            //    Host = base.Url.Replace("https://", "").Replace("http://", ""),
            //    UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
            //    ContentType = "application/json",
            //    Cookie = base.Cookie,
            //    Accept = "*/*",
            //    Referer = base.Url + "/member/index",
            //    Postdata = "{\"lottery\":\"PK10JSC\",\"drawNumber\":\""+ drawNumber + "\",\"bets\":"+ bets + ",\"ignore\":true}",
            //};         
            //item.PostEncoding = Encoding.UTF8;
            //item.Header.Add("Origin", base.Url);
            //item.Header.Add("X-Requested-With", "XMLHttpRequest");
            //var res = helper.GetHtml(item);
            //var resObj = JObject.Parse(res.Html);
            //if (resObj["status"].ToString() == "0")
            //{
            //    var betArray = JArray.Parse(bets);
            //    var betStr = "";
            //    long money = 0;
            //    foreach (JObject betObj in betArray)
            //    {
            //        var title = betObj["title"].ToString().Replace("_", "/");
            //        betStr += title + "/" + betObj["amount"].ToString() + "元" + "  ";
            //        money += Convert.ToInt64(betObj["amount"]);
            //    }
            //    WriteLogEvent.Invoke(drawNumber + "期 投注成功:" + betStr+ "\r\n");
            //    var balance = resObj["account"]["balance"].ToString();
            //    var hasList = Main.betList.Where(d => d.CateName == cateName && d.BetExpectNo == drawNumber).ToList();
            //    var newRecord = new BetRecord()
            //    {
            //        CateName = cateName,
            //        BetMoney = money.ToString(),
            //        BetTime = Config.CurrentTime.ToString("HH:mm:ss"),
            //        BetExpectNo = drawNumber,
            //        BetContent = betStr
            //    };
            //    Main.betList.Add(newRecord);
            //    //SaveRecordEvent.Invoke(newRecord);
            //    //RefreshMoneyEvent.Invoke(balance);
            //    return balance;
            //}
            //else
            //{
            //    var state = resObj["status"].ToString();
            //    if (state == "2") { WriteLogEvent.Invoke("投注失败 已封盘:"); }
            //    else if(state=="3"){
            //        WriteLogEvent.Invoke("投注失败"+ resObj["message"].ToString());
            //    }
            //    //if (state == "3")
            //    //{
            //    //    long money = 0;
            //    //    var betArray = JArray.Parse(bets);
            //    //    foreach (JObject betObj in betArray)
            //    //    {
            //    //        var title = betObj["title"].ToString().Replace("_", "/");
            //    //        money += Convert.ToInt64(betObj["amount"]);
            //    //    }
            //    //    strs += "3";
            //    //}
            //    //WriteLogEvent.Invoke(cateName + "" + drawNumber + "期 错误码:" + strs);
            //    return "error";
            //}
        }

        public override bool IsOnline()
        {
            HttpItem item = new HttpItem()
            {
                URL = base.Url + "/member/lastResult?lottery=AULUCKY10",
                Method = "Get",
                Host = base.Url.Replace("https://", "").Replace("http://", ""),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                ContentType = "application/json",
                Cookie = base.Cookie,
                Accept = "*/*",
                Referer = base.Url + "/member/index",
            };
            var res = helper.GetHtml(item);
            return res.Html == "" ? false : true;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="cateName"></param>
        /// <returns></returns>
        public override string GetMaxList(string cateNo)
        {
            HttpItem item = new HttpItem()
            {
                URL = base.Url + "/member/info?lottery=" + cateNo,
                Method = "Get",
                Host = base.Url.Replace("https://", "").Replace("http://", ""),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                ContentType = "application/json",
                Cookie = base.Cookie,
                Accept = "*/*",
                Referer = base.Url + "/member/index",
            };
            var res = helper.GetHtml(item);
            //幸运10 极速赛车 幸运飞艇 北京赛车
            //两面 冠亚军和大小 冠亚军和单双 冠亚军和
            var cates1 = new string[] { "澳洲幸运10", "极速赛车", "幸运飞艇", "北京赛车" };
            var cates2 = new string[] { "澳洲幸运5", "重庆时时彩" };

            if (cates1.Contains(Config.Cate.CateName))
            {
                var reg1 = new Regex(@"<tbody>\s+<tr>\s+<th>1-10车号</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_num>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>两面</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_two>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>冠亚军和大小</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_hebig>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>冠亚军和单双</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_hedan>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>冠亚军和</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_he>.*?)</td>\s+<td>.*?</td>\s+</tr>", RegexOptions.Multiline);
                var reg1Res = reg1.Match(res.Html);
                var Max_num = reg1Res.Groups["Max_num"].ToString();
                var Max_two = reg1Res.Groups["Max_two"].ToString();
                var Max_hebig = reg1Res.Groups["Max_hebig"].ToString();
                var Max_hedan = reg1Res.Groups["Max_hedan"].ToString();
                var Max_he = reg1Res.Groups["Max_he"].ToString();
                var j = new
                {
                    Max_type = "car",
                    Max_num = Max_num,
                    Max_two,
                    Max_hebig,
                    Max_hedan,
                    Max_he
                };
                return JsonConvert.SerializeObject(j);
            }
            if (cates2.Contains(Config.Cate.CateName))
            {
                var reg2 = new Regex(@"<tr>\s+<th>1-5球号</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_num>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>两面</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_two>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>龙虎和</th>\s+<td class=""cm"">.*?</td>\s+<td>1</td>\s+<td>(?<Max_he>.*?)</td>\s+<td>.*?</td>\s+</tr>\s+<tr>\s+<th>前三中三后三</th>\s+<td class=""cm"">.*?</td>\s+<td>.*?</td>\s+<td>(?<Max_san>.*?)</td>\s+<td>.*?</td>\s+</tr>", RegexOptions.Multiline);
                var reg2Res = reg2.Match(res.Html);
                var Max_num = reg2Res.Groups["Max_num"].ToString();
                var Max_two = reg2Res.Groups["Max_two"].ToString();
                var Max_he = reg2Res.Groups["Max_he"].ToString();
                var Max_san = reg2Res.Groups["Max_san"].ToString();
                var j = new
                {
                    Max_type = "ssc",
                    Max_num = Max_num,
                    Max_two,
                    Max_he,
                    Max_san
                };
                return JsonConvert.SerializeObject(j);
            }
            var str = "";
            return str;
        }


        public override string GetMoneyStr()
        {
            try
            {
                HttpItem indexItem = new HttpItem()
                {
                    Host = GetHost(Url),
                    URL = Url + "/member/index",
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                    Method = "GET",
                    UserAgent = "Mozilla/5.0(WindowsNT10.0;WOW64)AppleWebKit/537.36(KHTML,likeGecko)Chrome/71.0.3578.98Safari/537.36",
                    Cookie = base.Cookie,
                    ContentType = "application/json;charset=UTF-8"
                };
                var indexRes = helper.GetHtml(indexItem);
                var reg = new Regex(@"<span id=""infoMainBalance"" class=""balance"">(?<balance>.*?)</span>");
                var balance = reg.Match(indexRes.Html).Groups["balance"].Value;
                return balance;
            }
            catch (Exception ee)
            {
                return "网络繁忙";
            }
        }
    }
}
