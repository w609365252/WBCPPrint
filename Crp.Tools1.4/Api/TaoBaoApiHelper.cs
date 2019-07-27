using Crp.Tools.Crawler;
using CsharpHttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crp.Tools.Api
{
    /// <summary>
    /// 淘宝开放接口帮助类  文档 http://open.taobao.com/docs/api.htm?spm=a219a.7629065.0.0.cDW99g&apiId=5
    /// </summary>
    public static class TaoBaoApiHelper
    {
        /// <summary>
        /// 正式环境地址
        /// </summary>
        public static string url = "http://gw.api.taobao.com/router/rest";
        /// <summary>
        /// 应用Appkey
        /// </summary>
        public static string appKey = "";
        /// <summary>
        /// 应用Appkey
        /// </summary>
        public static string appSecret = "";

        /// <summary>
        /// 生成Api请求链接（已签名）
        /// </summary>
        /// <param name="method">api方法名</param>      
        /// <param name="parameters">请求参数集合</param> 
        /// <param name="signMethod">签名算法 md5或hmac （默认Md5)</param>
        /// <returns></returns>
        public static string GetRequestStr(string method, IDictionary<string, string> parameters, string signMethod = "md5")
        {
            var signParam = SignTopRequest(parameters, appSecret, signMethod);
            return TaoBaoApiHelper.url + "method=" + method + "&app_key=" + appKey + "&sign_method=" + signMethod + "&sign=" + signParam + "&timestamp=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "&format=json&v=2.0";
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="secret"></param>
        /// <param name="signMethod"></param>
        /// <returns></returns>
        public static string SignTopRequest(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (signMethod.Trim().ToLower() == "md5")
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (signMethod.Trim().ToLower() == "hmac")
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }


        /// <summary>
        /// 查询商品列表ids(爬虫)
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static List<string> SearchListHtml(string key, string pageIndex)
        {
            var pIndex = Convert.ToInt32(pageIndex);
            HttpHelper http = new HttpHelper();
            HttpItem httpi = new HttpItem()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8",
                URL = "https://s.taobao.com/search?data-key=" + key + "&data-value=" + pIndex * 44 + "&ajax=true&_ksTS=1512634687155_950&callback=jsonp951&q=" + key + "&imgfile=&commend=all&ssid=s5-e&search_type=item&sourceId=tb.index&spm=a21bo.2017.201856-taobao-item.1&ie=utf8&initiative_id=tbindexz_20170306&bcoffset=4&ntoffset=0&p4ppushleft=1%2C48&s=" + (pIndex - 1) * 44,
                Method = "GET",
                ContentType = "application/x-www-form-urlencoded",
                Referer = "https://www.taobao.com/",
                Cookie = "miid=2183595127850111581; cna=EfFkEichMi8CAXFXr4V4Rgzm; hng=CN%7Czh-CN%7CCNY%7C156; thw=cn; UM_distinctid=15f8f9549424e7-0f36daefde1d35-3e63430c-1fa400-15f8f95494379b; v=0; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; swfstore=115790; uc3=sg2=BYjbHPj2N6qcdaMQ9GEJ0aVE0ntDGNJCy0kC0Zzf8Ag%3D&nk2=0P9h8zr37jFknZM%3D&id2=UUtO%2BNSvTWr4aw%3D%3D&vt3=F8dBzLKBqCGA5qKntRM%3D&lg2=U%2BGCWk%2F75gdr5Q%3D%3D; existShop=MTUxMDAyMTQwMw%3D%3D; lgc=%5Cu8BDA%5Cu4EE5%5Cu5F85%5Cu4EBA123; tracknick=%5Cu8BDA%5Cu4EE5%5Cu5F85%5Cu4EBA123; cookie2=24c3e88e4d67b1e2f4437560e37ede19; mt=np=&ci=49_1; skt=fd9da2fe998aecbe; t=acc301068b790e04aafb631e6bc02610; _cc_=UIHiLt3xSw%3D%3D; tg=0; JSESSIONID=2433A0A36999827DBEF586D701E78568; _tb_token_=f578f35e6a43b; linezing_session=aEg7sPyKK9xJxxU4UEBTWXFN_1510028889904XGx9_2; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0%26__ll%3D-1%26_ato%3D0; uc1=cookie14=UoTde9YRTZkHCg%3D%3D&lng=zh_CN&cookie16=VFC%2FuZ9az08KUQ56dCrZDlbNdA%3D%3D&existShop=false&cookie21=U%2BGCWk%2F7pY%2FF&tag=8&cookie15=W5iHLLyFOGW7aA%3D%3D&pas=0; isg=AqSkE6gOohZeINW_rtAL7VoWdaJWlcjYs9EU1b7HCW8xaU8z7EkbNwqLX_cK"
            };
            var html = http.GetHtml(httpi).Html;
            var rids = new Regex(@"""nid"":""(?<id>.*?)""", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            var idsCollection = rids.Matches(html);
            var ids = new List<string>();
            for (int i = 0; i < idsCollection.Count; i++)
            {
                var id = idsCollection[i].Groups["id"].Value;
                ids.Add(id);
            }
            return ids;
        }


        /// <summary>
        /// 获取天猫商品详情 如天猫没有 则请求淘宝
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static TBDetailResult GetDetailHtml(string productId)
        {
            var html = HttpGetDetailHtml(productId);
            var result = new TBDetailResult();
            var risNotFind = new Regex("302 Found");
            var isNotFind = risNotFind.Match(html).Value;
            if (isNotFind.Length > 0)
            {//淘宝商品
                html = HttpGetDetailHtml(productId, "https://item.taobao.com");
                var rvideoId = new Regex(@"""videoId"":""(?<videoid>.*?)""", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var videoid = rvideoId.Match(html).Groups["videoid"].Value;
                if (videoid == "") { return null; }
                var rtitle = new Regex(@" data-title=""?(title).*?""", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                var rshopname = new Regex(@"sellerNick\s+:.*?'(?<shopname>.*?)'", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var rsellerid = new Regex(@"sellerId \s+:.*?'(?<sellerid>.*?)',", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var rshopscores = new Regex(@"<a href="".*?"" target=""_blank"" title="".*?"">.*?\n(?<shopscores>.*?)\n .*?  </a>", RegexOptions.Multiline);
                var rimg = new Regex(@"<a href=""#""><img data-src=""(?<img>.*?)"" /></a>", RegexOptions.Singleline);
                var rgoodsname = new Regex(@"data-title=""(?<title>.*?)""", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //判断是否有data-size="400x400"属性 有则取dataSize的大小
                var rimgSize = new Regex(@"<img id=""J_ImgBooth"" .*? data-hasZoom="".*?"" data-size=""(?<datasize>.*?)""/>");
                var imgSize = rimgSize.Match(html).Groups["datasize"].Value;
                //if (imgSize == "") {
                imgSize = "500x500";
                //}
                var mimgs = rimg.Matches(html);
                Match mimg;  //取第五张张主图
                if (mimgs.Count >= 5)
                {
                    mimg = mimgs[4];
                }
                else
                {
                    mimg = mimgs[mimgs.Count - 1];
                }
                var oldimg = mimg.Groups["img"].Value;
                var ind = oldimg.LastIndexOf('_');
                result.bigImg = "http:" + oldimg.Substring(0, ind + 1) + imgSize + "q90.jpg";
                result.goodsUrl = "https://item.taobao.com/item.htm?spm=a230r.1.14.28.59cfa6e5A93fSw&id=" + productId + "&ns=1&abbucket=5";
                result.shopName = rshopname.Match(html).Groups["shopname"].Value;
                result.sellerId = rsellerid.Match(html).Groups["sellerid"].Value;
                result.videoUrl = "https://cloud.video.taobao.com/play/u/" + result.sellerId + "/p/1/e/6/t/1/" + videoid + ".mp4";
                result.goodsName = rgoodsname.Match(html).Groups["title"].Value;
                var mshopscores = rshopscores.Matches(html);
                var shopscores = new List<string>();
                for (int i = 0; i < mshopscores.Count; i++)
                {
                    shopscores.Add(mshopscores[i].Groups["shopscores"].ToString().Trim());
                }
                result.shopScores = shopscores;
            }
            else
            {
                //天猫商品
                var rvideoIds = new Regex(@":""//cloud.video.taobao.com/.*?/t/./(?<video>.*?).swf", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var videoid = rvideoIds.Match(html).Groups["video"].Value;
                //只要有视频的
                if (videoid == "") { return null; }
                var rshopname = new Regex(@"<a class=""slogo-shopname"" href="".*?"" data-spm="".*?""><strong>(?<shopname>.*?)</strong></a>", RegexOptions.Singleline);
                var rsellerid = new Regex("sellerId=(?<sellerid>.*?)&", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var rshopscores = new Regex(@"<span class=""shopdsr-score-con"">(?<shopscores>.*?)</span>");  //描述  服务  物流
                var rtitle = new Regex(@"""title"":""(?<title>.*?)""", RegexOptions.Singleline);
                var rimg = new Regex(@"<a href=""#""><img src=""(?<img>.*?)"" /></a>", RegexOptions.Singleline);
                var rgoodsName = new Regex(@"<meta name=""keywords"" content=""(?<title>.*?)""/>");

                var mimgs = rimg.Matches(html);
                Match mimg;
                if (mimgs.Count >= 5)//取第五张张主图
                {
                    mimg = mimgs[4];
                }
                else
                {
                    mimg = mimgs[mimgs.Count - 1];
                }
                var oldimg = mimg.Groups["img"].Value;
                var ind = oldimg.LastIndexOf('_');
                result.bigImg = "http:" + oldimg.Substring(0, ind + 1) + "430x430q90.jpg";
                result.goodsUrl = "https://detail.tmall.com/item.htm?spm=a230r.1.14.28.59cfa6e5A93fSw&id=" + productId + "&ns=1&abbucket=5";
                result.shopName = rshopname.Match(html).Groups["shopname"].Value;
                result.sellerId = rsellerid.Match(html).Groups["sellerid"].Value;
                result.goodsName = rgoodsName.Match(html).Groups["title"].Value;
                result.videoUrl = "https://cloud.video.taobao.com/play/u/" + result.sellerId + "/p/1/e/6/t/1/" + videoid + ".mp4";
                var mshopscores = rshopscores.Matches(html);
                var shopscores = new List<string>();
                for (int i = 0; i < mshopscores.Count; i++)
                {
                    shopscores.Add(mshopscores[i].Groups["shopscores"].ToString());
                }
                result.videoId = videoid;
                result.shopScores = shopscores;
            }
            return result;
        }

        /// <summary>
        /// 查询商品详情(爬虫)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="host">默认查询天猫 如果302 再查淘宝</param>
        /// <returns></returns>
        private static string HttpGetDetailHtml(string productId, string host = "https://detail.tmall.com")
        {
            HttpHelper http = new HttpHelper();
            HttpItem httpi2 = new HttpItem()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*;q=0.8",
                URL = host + "/item.htm?spm=a230r.1.14.28.59cfa6e5A93fSw&id=" + productId + "&ns=1&abbucket=5",
                Method = "GET",
                ContentType = "application/x-www-form-urlencoded",
                Referer = "https://s.taobao.com/search?q=" + "+无人机+" + "&imgfile=&commend=all&ssid=s5-e&search_type=item&sourceId=tb.index&spm=a21bo.2017.201856-taobao-item.1&ie=utf8&initiative_id=tbindexz_20170306",
                Cookie = "cna=EfFkEichMi8CAXFXr4V4Rgzm; sm4=440300; _m_h5_tk=09d0f304d56944ac49f9f4ff48d15656_1509944760373; _m_h5_tk_enc=cec671cc9081609c036db88cff3c1022; swfstore=123277; hng=CN%7Czh-CN%7CCNY%7C156; uc1=cookie14=UoTde9YX6C8VIQ%3D%3D&lng=zh_CN&cookie16=Vq8l%2BKCLySLZMFWHxqs8fwqnEw%3D%3D&existShop=false&cookie21=VFC%2FuZ9ainBZ&tag=8&cookie15=URm48syIIVrSKA%3D%3D&pas=0; uc3=sg2=BYjbHPj2N6qcdaMQ9GEJ0aVE0ntDGNJCy0kC0Zzf8Ag%3D&nk2=0P9h8zr37jFknZM%3D&id2=UUtO%2BNSvTWr4aw%3D%3D&vt3=F8dBzLKBqCGA5qKntRM%3D&lg2=W5iHLLyFOGW7aA%3D%3D; tracknick=%5Cu8BDA%5Cu4EE5%5Cu5F85%5Cu4EBA123; ck1=; lgc=%5Cu8BDA%5Cu4EE5%5Cu5F85%5Cu4EBA123; cookie2=24c3e88e4d67b1e2f4437560e37ede19; t=acc301068b790e04aafb631e6bc02610; skt=fd9da2fe998aecbe; _tb_token_=f578f35e6a43b; whl=-1%260%260%260; x=__ll%3D-1%26_ato%3D0; pnm_cku822=098%23E1hvN9vUvbpvUvCkvvvvvjiPPLsv6jiEPFMwAjD2PmPWljnHnLz9gjtnn2MwQjlPRphvChCvvvvCvpvVphhvvvvvmphvLhbCz9mFejZINhmxfXkOjLhDN%2BLZdigDN5Hma4AAdcHjjLVxfXkXjoCABYoO%2Bul1BC61D70OV169PDrr1EeKfvyf8%2B1l53DlpVO2fwotvpvIphvv2vvvphavpCAGvvC23ZCvjvUvvhBGphvwmpvvBj1vpCsyvvChwOyCvvXmp99hVtkivpvUphvhNwIYoUpPvpvhvv2MMTwCvvBvpvpZ; cq=ccp%3D1; isg=AsfHKlTeUV-DJNY2UubhkPeEVntRZJvRzBBXHJm0-dZ9COfKoZwr_gXA3P-s; otherx=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0"
            };
            return http.GetHtml(httpi2).Html;
        }

        /// <summary>
        /// 查询商品详情(临时租用 可以查到创建时间)
        /// </summary>
        /// <returns></returns>
        public static TBDetailResult GetProductDetailFromApi(string productId)
        {

            HttpHelper http = new HttpHelper();
            TBDetailResult result = new TBDetailResult();
            HttpItem item = new HttpItem()
            {
                URL = "http://3f4d1b-11.sh.1253485869.clb.myqcloud.com:8013/Q69341380.ashx",
                Method = "POST",
                Accept = "*/*",
                Postdata = "type=itemget&numiid=" + productId + "&fields=created,title,nick,detail_url",
                ContentType = "application/x-www-form-urlencoded"
            };
            var list = http.GetHtml(item).Html;
            //{"item_get_response":{"item":{"created":"2014-06-10 23:23:46","detail_url":"https:\/\/item.taobao.com\/item.htm?id=39437375897&spm=2014.21281452.0.0","nick":"永结同心婚庆店","title":"喜庆红盆 喜庆用品 红色结婚脸盆新娘婚庆嫁妆红盆 鸳鸯喜盆"},"request_id":"11iedlw1h8qn7"}}
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(list);
            var items = (JObject)((obj)["item_get_response"])["item"];
            result.createTime = items["created"].ToString();
            result.goodsName = items["title"].ToString();
            result.shopName = items["nick"].ToString();
            return result;
        }


        /// <summary>
        /// 查询商品描述符合度  来自于谷歌插件的算法
        /// </summary>
        /// <returns></returns>
        public static bool CheckProductYes(string id)
        {
            HttpHelper http = new HttpHelper();
            TBDetailResult result = new TBDetailResult();
            HttpItem item = new HttpItem()
            {
                URL = "https://scenes.taobao.com/content/json/getItemDetailByUrl.do?appKey=jiyoujia&isNeedTaoke=true&isCheckItem=true&url=https://item.taobao.com/item.htm?id=" + id,
                Method = "get",
                Accept = "*/*",
            };
            var list = http.GetHtml(item).Html;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(list);
            return obj["errorMsg"].ToString().Trim() == "";
        }

        #region 轻淘客Api
        public enum TKCateType
        {
            全部_默认 = 0,
            母婴 = 2,
            美妆 = 3,
            居家 = 4,
            鞋包配饰 = 5,
            美食 = 6,
            文体 = 7,
            家电数码 = 8,
            其他 = 9,
            女装 = 10,
            内衣 = 11,
            男装 = 12
        }

        public enum TKSortType
        {
            人气排序从高到低_默认 = 1,
            最新排序从高到低 = 2,
            销量排序从高到低 = 3,
            价格排序从低到高 = 4,
            佣金排序从高到低 = 5,
            价格排序从高到低 = 7,
            人气排序从低到高 = 8,
            最新排序从低到高 = 9
        }

        /// <summary>
        /// 查询商品列表 From淘客api http://www.qingtaoke.com/openapi  
        /// </summary>
        ///<param name="key">关键字</param>
        /// <param name="page">分页获取数据 默认1 最多获取到100页（每页100条）</param>
        /// <param name="page_size"></param>
        /// <param name="type">	 商品分类： </param>
        /// <param name="sort">排序方式：</param>
        /// <param name="min_price">价区间（最小值）</param>
        /// <param name="max_price">价区间（最大值）</param>
        /// <param name="is_tqg">	是否取淘抢购商品，0表示取全部，1表示取淘抢购。默认0</param>
        /// <param name="is_ali">	是否取阿里券商品，0表示只取非阿里券商品，1表示只取阿里券商品，不传获取全部。默认获取全部</param>
        /// <returns></returns>
        public static List<TKListResult> QueryListFromTKApi(string key , int page, int page_size, TKCateType type, TKSortType sort, int min_price, int max_price, bool is_tqg, bool is_ali)
        {

            var param = "&sort=" + Convert.ToInt32(sort);
            param += "&cat=" + Convert.ToInt32(type);
            param += "&key_word=" + key;
            


            if (min_price > 0)
                param += "&min_price=" + min_price;
            if (max_price > 0)
                param += "&max_price=" + max_price;

            if (page > 0)
                param += "&page=" + page;
            if (page_size > 0)
                param += "&page_size=" + page_size;

            if (is_tqg)
                param += "&is_tqg=1";

            if (is_ali)
                param += "&is_ali=1";



            HttpHelper http = new HttpHelper();

            HttpItem item = new HttpItem()
            {
                URL = "http://openapi.qingtaoke.com/search?s_type=2&app_key=JjaMPH4f&v=1.0" + param,
                Method = "get",
                Accept = "*/*",
            };
            var result = http.GetHtml(item).Html;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(result);
            var list = (JArray)(obj["data"]["list"]);

            List<TKListResult> rlist = new List<TKListResult>();
            foreach (var jobj in list)
            {
                if (jobj["commission"].ToString() == "0" || jobj["commission"].ToString() == "")
                    break;


                TKListResult model = new TKListResult()
                {
                    goods_id = jobj["goods_id"].ToString(),
                    commission = Convert.ToDecimal(jobj["commission"]),
                    commission_Proportion = Convert.ToDecimal(jobj["commission"]) / Convert.ToDecimal(jobj["goods_price"]),
                    goods_sales = Convert.ToInt32(jobj["goods_sales"]),
                    goods_title = jobj["goods_title"].ToString(),
                    is_tmall = Convert.ToInt32(jobj["is_tmall"]),
                    goods_price = Convert.ToDecimal(jobj["goods_price"])
                };
                rlist.Add(model);
            }
            return rlist;
        }

        /// <summary>
        /// 是否符合描述
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public static bool DescIsOk(string id)
        {
            HttpHelper http = new HttpHelper();
            HttpItem httpi2 = new HttpItem()
            {
                URL = "https://scenes.taobao.com/content/json/getItemDetailByUrl.do?appKey=jiyoujia&isNeedTaoke=true&isCheckItem=true&url=https://item.taobao.com/item.htm?id=" + id
            };
            var html = http.GetHtml(httpi2).Html;
            var jobj = (JObject)JsonConvert.DeserializeObject(html);
            return jobj["errorMsg"].ToString() == "" ? true : false;
        }


        public class TKListResult
        {
            public string goods_id { get; set; }
            /// <summary>
            /// 佣金
            /// </summary>
            public decimal commission { get; set; }
            /// <summary>
            /// 佣金比例
            /// </summary>
            public decimal commission_Proportion { get; set; }
            public string goods_title { get; set; }
            public decimal goods_price { get; set; }
            public int goods_sales { get; set; }
            public int is_tmall { get; set; }

            //"goods_id" : "522914459446"  //商品ID string
            //"goods_pic" : "http://img.alicdn.com/imgextra/i4/TB1u18HLpXXXXXbXpXX_!!0-item_pic.jpg" //商品主图 string
            //"goods_title" : "云奈2016秋季新品韩版高腰抓痕牛仔裤女"   //商品标题 string
            //"goods_short_title" : "韩版高腰抓痕牛仔裤女"   //商品短标题 string
            //"goods_cat" : 7   //商品分类 int 2母婴 3美妆 4居家 5鞋包配饰 6美食 7文体 8家电数码 10女装 11内衣 12男装 9其它
            //"goods_price" : 249    //商品售价 float
            //"goods_sales" : 1349   //商品销量 int
            //"goods_introduce" : "字迹清晰，归纳调理，简洁全面，物美价廉，内容丰富，值得信赖的选择。"   //商品文案描述 string
            //"is_tmall" : 1    //是否是天猫int 1是 其他否
            //"commission" : 90.5    //佣金 float
            //"commission_type" : 1   //佣金类型 int 1定向计划2高拥3通用4营销计划
            //"commission_link" : "http://pub.alimama.com/myunion.htm?#!/promo/self/campaign?
            //         campaignId=38403567&shopkeeperId=99183620&userNumberId=2203671411"    //计划链接 string
            //"coupon_is_check" : "1"    //是否校验后的券 string 0 未验证 1 已验证有效性
            //"coupon_type" : "1"    //券类型 string  0 未知  1 商品单品  2 店铺
            //"seller_id" : "683728440"    //卖家ID string
            //"coupon_id" : "f2a48d16b81647718f997481713333d8"    //券ID string，（tip：阿里券的券id不可用）
            //"coupon_price" : 10    //券价格 float
            //"coupon_number" : 5000    //券剩余数 int
            //"coupon_limit" : -1    //券限领数 int -1表示无限制
            //"coupon_over" : 200    //券已领数 int
            //"coupon_condition" : 30.5    //券使用条件 float
            //"coupon_start_time" : "2017-04-28 00:00:00"    //券开始时间 string
            //"coupon_end_time" : "2017-05-28 23:59:59"    //券结束时间 string
            //"is_ju" : "0"    //是否取聚划算 int  0 否  1 是  
            //"is_tqg" : "0"    //是否取淘抢购 int  0 否  1 是
            //"is_ali" : "0"    //是否取阿里券 int  0 否  1 是（tip：阿里券的券id不可用）
        }
        #endregion

        public class TBDetailResult
        {

            public string goodsUrl { get; set; }

            public string shopName { get; set; }

            public string goodsName { get; set; }

            public string sellerId { get; set; }

            public string videoUrl { get; set; }

            public string videoId { get; set; }

            /// <summary>
            /// 最后一张主图
            /// </summary>
            public string bigImg { get; set; }

            /// <summary>
            /// 商家评分   描述  服务  物流
            /// </summary>
            public List<string> shopScores { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public string createTime { get; set; }
        }

    }





}
