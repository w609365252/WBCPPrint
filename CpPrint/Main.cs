/********************************************************************
 * *
 * * 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
 * * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
 * *
 * * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
 * * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
 * * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
 * * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
 * *
 * * Copyright (C) 2013-? cskin Corporation All rights reserved.
 * * 网站：CSkin界面库 http://www.cskin.net 论坛 http://bbs.cskin.net
 * * 作者： 乔克斯 QQ：345015918 .Net项目技术组群：306485590
 * * 请保留以上版权信息，否则作者将保留追究法律责任。
 * *
 * * 创建时间：2014-08-26
 * * 说明：FrmMain.cs
 * *
********************************************************************/
using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CsharpHttpHelper;
using System.Text.RegularExpressions;
using System.Linq;
using CpPrint.ViewModel;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using Newtonsoft.Json;

using Microsoft.CSharp;
using Crp.Tools;
using Crp.Tools.ZXing;
using Crp.Tools.FileHelper;
using CpPrint.Apis;
using System.Web.Configuration;
using System.Threading.Tasks;
using CpPrint.Tools;
using Crp.Tools.DataTypeExtend;
using FormInvoke;
using System.Runtime.InteropServices;
using BetingSystem;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using Microsoft.Win32;

namespace CpPrint
{
    public partial class Main : CCSkinMain
    {
        
 

        public class GetImage
        {
            private int S_Height;
            private int S_Width;
            private int F_Height;
            private int F_Width;
            private string HTML;

            public int ScreenHeight
            {
                get { return S_Height; }
                set { S_Height = value; }
            }

            public int ScreenWidth
            {
                get { return S_Width; }
                set { S_Width = value; }
            }

            public int ImageHeight
            {
                get { return F_Height; }
                set { F_Height = value; }
            }

            public int ImageWidth
            {
                get { return F_Width; }
                set { F_Width = value; }
            }

            public string WebSite
            {
                get { return HTML; }
                set { HTML = value; }
            }

            public GetImage(string WebSite, int ScreenWidth, int ScreenHeight, int ImageWidth, int ImageHeight)
            {
                this.WebSite = WebSite;
                this.ScreenWidth = ScreenWidth;
                this.ScreenHeight = ScreenHeight;
                this.ImageHeight = ImageHeight;
                this.ImageWidth = ImageWidth;
            }

            public Bitmap GetBitmap()
            {
                WebPageBitmap Shot = new WebPageBitmap(this.WebSite, this.ScreenWidth, this.ScreenHeight);
                Shot.GetIt();
                Bitmap Pic = Shot.DrawBitmap(this.ImageHeight, this.ImageWidth);
                return Pic;
            }
        }

        class WebPageBitmap
        {
            WebBrowser MyBrowser;
            string HTML;
            int Height;
            int Width;

            public WebPageBitmap(string html, int width, int height)
            {
                this.Height = height;
                this.Width = width;
                this.HTML = html;
                MyBrowser = new WebBrowser();
                MyBrowser.ScrollBarsEnabled = false;
                MyBrowser.Size = new Size(this.Width, this.Height);
            }

            public void GetIt()
            {
                MyBrowser.DocumentText = HTML;
                //也可用rul
                //MyBrowser.Navigate(@"C:\Documents and Settings\Administrator\桌面\1.html");
                while (MyBrowser.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
            }

            public Bitmap DrawBitmap(int theight, int twidth)
            {
                Bitmap myBitmap = new Bitmap(Width, Height);
                Rectangle DrawRect = new Rectangle(0, 0, Width, Height);
                MyBrowser.DrawToBitmap(myBitmap, DrawRect);
                System.Drawing.Image imgOutput = myBitmap;
                System.Drawing.Image oThumbNail = new Bitmap(twidth, theight, imgOutput.PixelFormat);
                Graphics g = Graphics.FromImage(oThumbNail);
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                Rectangle oRectangle = new Rectangle(0, 0, twidth, theight);
                g.DrawImage(imgOutput, oRectangle);
                try
                {
                    return (Bitmap)oThumbNail;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    imgOutput.Dispose();
                    imgOutput = null;
                    MyBrowser.Dispose();
                    MyBrowser = null;
                }
            }
        }

         /// <summary>
    /// 
    /// </summary>
        int betType = 1;
        /// <summary>
        /// 玩法类型
        /// </summary>
        int modeType = 1;
        int selectBetType;
        int isLoad = 0;
        //倍数
        int times = 1;
        //1、设置几点启动 几点自动关闭 。
        //2、极速赛车换接口。
        //4、VR赛车加上冠亚和所有选项的自动投注。
        //极速帐号7889密码Aa8888   密码Aa9999才对极速的
        //飞艇的d00339密码aa778899
        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            SelectItem = SkinTool1;
        }
        string ruleFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\rule";
        #region UI 事件

        #region 控制web控件滚动条
        /// <summary>
        /// 控制web控件滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webShow_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        #endregion

        #region 画窗体边框
        /// <summary>
        /// 画窗体边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.FromArgb(33, 40, 46), 2);
            g.DrawRectangle(p, tabShow.Left, tabShow.Top, tabShow.Width, tabShow.Height);
        }
        #endregion

        #region 换肤菜单
        /// <summary>
        /// 选中的MenuItem
        /// </summary>
        ToolStripMenuItem SelectItem;
        private void SkinTool_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            //选中当前Item
            item.Checked = true;
            //取消选中上一个Item，并存储当前Item
            SelectItem.Checked = false;
            SelectItem = item;
            //如果是0，则是默认皮肤
            if (item.Tag.ToString().Equals("0"))
            {
                this.Back = null;
                this.BackColor = Color.FromArgb(63, 176, 215);
                this.Opacity = this.SkinOpacity = 0.9;
            }
            else
            {
                //其他皮肤，从程序集资源中提取，并且设置透明度为不透明
                this.Opacity = this.SkinOpacity = 1;
                this.Back = ImageObject.GetResBitmap(string.Format("BetingSystem.Skin.{0}.jpg", item.Tag));
            }
        }
        #endregion

        #region 自定义系统按钮事件
        //自定义系统按钮事件
        private void FrmMain_SysBottomClick(object sender, CCWin.SkinControl.SysButtonEventArgs e)
        {
            //获得弹出坐标
            Point l = PointToScreen(e.SysButton.Location);
            l.Y += e.SysButton.Size.Height + 1;
            //如果是皮肤菜单
            if (e.SysButton.Name == "ToolSkin")
            {
                SkinMenu.Show(l);
            }
            else if (e.SysButton.Name == "ToolSet")
            {
                //如果是设置菜单
                SkinToolMenu.Show(l);
            }
        }
        #endregion

        #region Tab切换时事件，用于子窗体更改了提示Lbl后的还原
        private void tabShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTs.Text = lblTs.Tag.ToString();
        }
        #endregion

        #endregion
        public static List<OpenRecord> openList = new List<OpenRecord>(); //开奖记录
        public static List<Order> orderList = new List<Order>(); //在订单
                                                                //public static List<BetRecord> orderList = new List<BetRecord>(); //下注记录
        public static List<PlayRule> playRuleList = new List<PlayRule>(); //游戏规则
        public static Main mainForm;
        List<string> openNum = new List<string>();
            
        string playModeTxt = "";

        #region 窗口加载时
        /// <summary>
        /// 窗口加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 21; i++) openNum.Add(i.ToString().PadLeft(2, '0'));
            mainForm = this;
            selectBetType = betType;
            var type = (BetType)betType;

            if (type == BetType.OneBet)
            {
                lab_NumCheckList.Text = "选择号码：";
                clb_TuoNumList.Hide();
                lab_TuoNumList.Hide();
                lab_tuoNumDesc.Hide();
                playModeTxt = GetEnumDescription((OneBetPlayMode)modeType);
            }
            else if (type == BetType.CompoundBet)
            {
                lab_NumCheckList.Text = "选择号码：";
                lab_TuoNumList.Hide();
                clb_TuoNumList.Hide();
                lab_tuoNumDesc.Hide();
                playModeTxt = GetEnumDescription((CompoundBetPlayMode)modeType);
            }
            else if (type == BetType.DTBet)
            {
                clb_TuoNumList.Show();
                lab_TuoNumList.Show();
                lab_tuoNumDesc.Show();
                lab_NumCheckList.Text = "选择胆码：";
                playModeTxt = GetEnumDescription((DTBetPlayDesc)modeType);
            }
            SetConfigStyle(betType);
            SetTimes(1);
            lab_PlayType.Text = GetEnumDescription(type)+$"({playModeTxt})";

            //lblTs.Text = "系统设置";
            if (isLoad == 0)
            {
                InitEvent();
                isLoad = 1;
                playRuleList = JsonConvert.DeserializeObject<List<PlayRule>>(File.ReadAllText(ruleFilePath));
                Task.Run(new Action(delegate ()
                {
                    orderList = OrderListOperate.GetOrders();
                    BindOrderList();
                    while (true)
                    {
                        LoadOpenList();
                        Thread.Sleep(5000);
                    }
                }));
            }
        }

        void InitEvent()
        {
            for (int i = 0; i < pnl_playMode.Controls.Count; i++)
            {
                if (pnl_playMode.Controls[i] is Button)
                {
                    var button = pnl_playMode.Controls[i] as Button;
                    button.Click += new System.EventHandler(this.ClickMode);
                }
            }
        }
        #endregion

        //设置倍数
        void SetTimes(int num)
        {
            times = num;
            lab_times.Text = times.ToString();
        }

        private void btn_confirmOrder_Click(object sender, EventArgs e)
        {
            var a = lbl_currExpectNo.Text;
            var b = txt_number.Text;
            var orderNo = Guid.NewGuid().ToString("N").ToUpper();
            orderNo = orderNo.Substring(0, 8) + "-" + orderNo.Substring(8, 8) + "-" + orderNo.Substring(16, 8) + "-" + orderNo.Substring(24, 8);
            List<string> numberList = b.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            var order = new Order()
            {
                BetContent = b,
                ExpectNo = a.Trim(),
                CreateTime = DateTime.Now,
                OrderNo = orderNo,
                price = GetBuyPrice(numberList)
            };
            orderList.Add(order);
            OrderListOperate.InsertOrder(order);

            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("订单录入成功,是否需要打印?", "提示", mess);
            if (dr == DialogResult.OK)
            {
                PrintHtml();
            }
        }

        public void PrintHtml()
        {
            var filename = AppDomain.CurrentDomain.BaseDirectory + "h\\1.html";
            webBrowser1.Navigate(filename);
            webBrowser1.Print();
        }

        string tempOrderNo = "";
        string tempOpenNum = "";
        string tempBuyCPNum = "";
        string tempOrderTime = "";
        string tempOpenPrice = "";
        string tempOpenState = "";
        string tempBetMode = "";
        string tempOneCodeFilePath = "";
        string tempBetType = "幸运农场";

        int isPrints1 = 0;
        public void PrintHtml(string orderNo, string OpenNum, string BuyCPNum, string OrderTime,
            string OpenPrice, string OpenState, string BetMode, string BetType = "幸运农场")
        {
            tempOrderNo = orderNo;
            tempOpenNum = OpenNum;
            tempBuyCPNum = BuyCPNum;
            tempOrderTime = OrderTime;
            tempOpenPrice = OpenPrice;
            tempOpenState = OpenState;
            tempBetMode = BetMode;
            tempBetType = BetType;
            var filename = AppDomain.CurrentDomain.BaseDirectory + "h\\1.html";
            string tempFileName = Guid.NewGuid().ToString();
            string tempFilePath = AppDomain.CurrentDomain.BaseDirectory + $"h\\{tempFileName}.html";
            //Bitmap bitmap = ZXingHelper.Generate2("1", 300, 100);
            string onecodeDirectory = AppDomain.CurrentDomain.BaseDirectory + $"h\\OneCode";
            if (!Directory.Exists(onecodeDirectory)) Directory.CreateDirectory(onecodeDirectory);
            string OneCodeFilePath = onecodeDirectory + $"\\{tempFileName}.png";
            //bitmap.Save(OneCodeFilePath);
            OneCodeFilePath = onecodeDirectory + $"\\default.jpg";
            tempOneCodeFilePath = OneCodeFilePath;

            File.Copy(filename, tempFilePath);
            string strContent = File.ReadAllText(tempFilePath);

            Order o = orderList.Find(m => string.Equals(m.OrderNo, orderNo, StringComparison.OrdinalIgnoreCase));
            if (o.BetType == 1)
            {

                var buys = BuyCPNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string newStr = "";
                var newL = "①,②,③,④,⑤,⑥,⑦,⑧,⑨,⑩".Split(',');

                int i = 0;

                foreach (var item in buys)
                {
                    var listBuy = item.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    int count = 0;
                    foreach (var str in listBuy)
                    {
                        var n = str.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);

                        string numTest = "";
                        var lst = Regex.Split(n[0].Replace("\n", ""), "(?<=\\G.{2})");
                        numTest = lst[0] + "<span class='fontstyle'>" + lst[1] + "</span>";
                        n[0] = numTest;

                        if (n.Count() > 1)
                        {
                            newStr += n[0] + "&nbsp;&nbsp;&nbsp;<span style='margin-left:-3px;'>[&nbsp;" + "<span class='fontstyle1'>" + n[1].Replace("]", "").Replace("倍","") + "</span>倍" + "&nbsp;]</span>";
                        }
                        else newStr += (count == 0 ? "<span style='font-size:11px;'>" + newL[i] + "</span>" + "&nbsp;&nbsp;&nbsp;" : "") + n[0] + "&nbsp;&nbsp;&nbsp;";
                        count++;
                    }
                    i++;
                    newStr = newStr.Trim();
                    newStr += ",";
                }
                newStr = newStr.Trim(',');
                BuyCPNum = newStr;
                strContent = strContent.Replace("<div class='big1'>", "<div class='big1' style='margin-left:22px;'>");
                strContent = strContent.Replace(".big1 span {margin-right:6px;}", ".big1 span {margin-right:13px;}");
            }
            else
            {
                string str = "";
                var buy = BuyCPNum.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string beiShu = buy[1];
                var nm = buy[0].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                int tj = 1;

                string[] numList = new string[] { };
                int totalCount = 0;

                if (nm.Count() == 2)
                {
                    var danList= nm[0].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in danList)
                    {
                        string numTest = "";
                        var lst = Regex.Split(item.Replace("\n", ""), "(?<=\\G.{2})");
                        numTest = lst[0] + "<span class='fontstyle'>" + lst[1] + "</span>";

                        str += numTest + (danList.Count() == tj ? "" : "+");
                        if (tj % 6 == 0) str += "<br/>";
                        tj++;
                    }
                    str += "^";
                    numList = nm[1].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    totalCount = danList.Count() + numList.Count();
                }
                else
                {
                    numList = nm[0].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    totalCount = numList.Count();
                }

                foreach (var item in numList)
                {
                    string numTest = "";
                    var lst = Regex.Split(item.Replace("\n", ""), "(?<=\\G.{2})");
                    numTest = lst[0] + "<span class='fontstyle'>" + lst[1] + "</span>";

                    str += numTest + (totalCount == tj ? "" : "+");
                    if (tj % 6 == 0) str += "<br/>";
                    tj++;
                }
                str = str.Trim('+');
                str += beiShu;
                BuyCPNum = str;
            }

            strContent = strContent.Replace("<div id='orderNo'></div>", $"<div id='orderNo'>{orderNo}</div>");//替换玩法
            strContent = strContent.Replace("<span id='betType'>幸运农场</span>", $"<span id='betType'>{BetType}</span>");//替换玩法
            strContent = strContent.Replace("<span id='betMode'>幸运五复式</span>", $"<span id=\"betMode\">{BetMode}</span>");//替换玩法
            strContent = strContent.Replace("<span id='openNum'></span>", $"<span id=\"openNum\">{OpenNum}</span>");//替换期数
            strContent = strContent.Replace("<span id='streamNum'>1</span>", $"<span id='streamNum'>{o.StreamNum}</span>");//替换流水号
            strContent = strContent.Replace("<span id='price'>12</span>", $"<span id='price'>{OpenPrice}</span>");//替换金额
            strContent = strContent.Replace("<span>销售时间:<span id='orderTime'>2019/06/28-20:04:05</span></span>", $"<span>销售时间:<span id='orderTime'>{OrderTime}</span></span>");//替换时间
            strContent = strContent.Replace("<div id='betContent'></div>", $"<div id='betContent'>{BuyCPNum.Replace("\n","").Replace(",", "<br />")}</div>");//替换开奖号码
            strContent = strContent.Replace("<span id='donatePrice'>3.48</span>", $"<span id='donatePrice'>{(OpenPrice.ToDecimal() * 0.29m)}</span>");//贡献公益金
            strContent = strContent.Replace("<div id='address'>站地址:渝北区松石支路77附14号</div>", "<div id='address'>站地址:渝北区松石支路77附14号</div>");//替换地址
            strContent = strContent.Replace("<span id='invoiceNum'></span>", $"<span id='invoiceNum'>{o.Validate}</span>");//验票码
            strContent = strContent.Replace("<img id='code' src='img/text_lottery.png' />", $"<img id='code' src='{OneCodeFilePath}' />");//条形码
            File.WriteAllText(tempFilePath, strContent);
            try
            {
                webBrowser1.Navigate(tempFilePath);
                string keyName = @"Software\Microsoft\Internet Explorer\PageSetup\";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {

                        key.SetValue("footer", "");  //设置页脚为空
                        key.SetValue("header", "");  //设置页眉为空
                        key.SetValue("Print_Background", true); //设置打印背景颜色
                        key.SetValue("margin_bottom", 0);  //设置下页边距为0
                        key.SetValue("margin_left", 0);   //设置左页边距为0
                        key.SetValue("margin_right", 0);  //设置右页边距为0
                        key.SetValue("margin_top", 0);   //设置上页边距为0


                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(10);
                            webBrowser1.Invoke(new Action(delegate
                            {
                                webBrowser1.Print();  //打印
                                strContent = strContent.Replace("<body id='bodyObj' class='print'>", "<body id='bodyObj'>");
                                File.WriteAllText(tempFilePath, strContent);
                                webBrowser1.Navigate(tempFilePath);
                            }));
                        });

                        isPrints1 = 1;
                    }
                    else
                    {
                        MessageBox.Show("错误");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        int isFirstPrint = 0;
        public void PrintDrawHtml(string expectNo,string streamNum,string drawPrice,string drawTime)
        {
            var filename = AppDomain.CurrentDomain.BaseDirectory + "draw\\draw.html";
            string tempFileName = Guid.NewGuid().ToString();
            string tempFilePath = AppDomain.CurrentDomain.BaseDirectory + $"draw\\{tempFileName}.html";
            File.Copy(filename, tempFilePath);
            string strContent = File.ReadAllText(tempFilePath);

            strContent = strContent.Replace("<div id='expectNo'></div>", $"<div id='expectNo'>{expectNo}</div>");//替换玩法
            strContent = strContent.Replace("<div id='streamNum'></div>", $"<div id='streamNum'>{streamNum}</div>");//替换玩法
            strContent = strContent.Replace("<div id='drawPrice'></div>", $"<div id='drawPrice'>{drawPrice}</div>");//替换玩法
            strContent = strContent.Replace("<div id='drawTime'></div>", $"<div id='drawTime'>{drawTime}</div>");//替换期数
            File.WriteAllText(tempFilePath, strContent);

            webBrowser1.Navigate(tempFilePath);
            
            string keyName = @"Software\Microsoft\Internet Explorer\PageSetup\";
            
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.SetValue("footer", "");  //设置页脚为空
                    key.SetValue("header", "");  //设置页眉为空
                    key.SetValue("Print_Background", true); //设置打印背景颜色
                    key.SetValue("margin_bottom", 0);  //设置下页边距为0
                    key.SetValue("margin_left", 0);   //设置左页边距为0
                    key.SetValue("margin_right", 0);  //设置右页边距为0
                    key.SetValue("margin_top", 0);   //设置上页边距为0
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(10);
                        webBrowser1.Invoke(new Action(delegate
                        {
                            Thread.Sleep(10);
                            webBrowser1.Print();  //打印
                            strContent = strContent.Replace("<body id='bodyObj' class='print'>", "<body id='bodyObj'>");
                            File.WriteAllText(tempFilePath, strContent);
                            webBrowser1.Navigate(tempFilePath);
                        }));
                    });
                    
                    isFirstPrint = 1;
                }
            }
        }

        class NativeMethods
        {
            [ComImport]
            [Guid("0000010D-0000-0000-C000-000000000046")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            interface IViewObject
            {
                void Draw([MarshalAs(UnmanagedType.U4)] uint dwAspect, int lindex, IntPtr pvAspect, [In] IntPtr ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [MarshalAs(UnmanagedType.Struct)] ref RECT lprcBounds, [In] IntPtr lprcWBounds, IntPtr pfnContinue, [MarshalAs(UnmanagedType.U4)] uint dwContinue);
            }

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            public static void GetImage(object obj, Image destination, Color backgroundColor)
            {
                using (Graphics graphics = Graphics.FromImage(destination))
                {
                    IntPtr deviceContextHandle = IntPtr.Zero;
                    RECT rectangle = new RECT();

                    rectangle.Right = destination.Width;
                    rectangle.Bottom = destination.Height;

                    graphics.Clear(backgroundColor);

                    try
                    {
                        deviceContextHandle = graphics.GetHdc();

                        IViewObject viewObject = obj as IViewObject;
                        viewObject.Draw(1, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, deviceContextHandle, ref rectangle, IntPtr.Zero, IntPtr.Zero, 0);
                    }
                    finally
                    {
                        if (deviceContextHandle != IntPtr.Zero)
                        {
                            graphics.ReleaseHdc(deviceContextHandle);
                        }
                    }
                }
            }
        }

        // <summary>
        /// 打印的格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            /*如果需要改变自己 可以在new Font(new FontFamily("黑体"),11）中的“黑体”改成自己要的字体就行了，黑体 后面的数字代表字体的大小
             System.Drawing.Brushes.Blue , 170, 10 中的 System.Drawing.Brushes.Blue 为颜色，后面的为输出的位置 */
            //第一行绘制订单号
            e.Graphics.DrawString(tempOrderNo, new Font(new FontFamily("黑体"), 12), System.Drawing.Brushes.Black, 10, 15);
            //第二行
            e.Graphics.DrawString(tempBetType, new Font(new FontFamily("黑体"), 12,FontStyle.Bold), System.Drawing.Brushes.Black, 9, 35);
            e.Graphics.DrawString(tempBetMode, new Font(new FontFamily("黑体"), 12,FontStyle.Bold), System.Drawing.Brushes.Black, 88, 35);
            e.Graphics.DrawString(tempOpenNum, new Font(new FontFamily("黑体"), 12, FontStyle.Bold), System.Drawing.Brushes.Black, 188, 35);

            //第三行
            e.Graphics.DrawString("站号:40130023", new Font(new FontFamily("黑体"), 11), System.Drawing.Brushes.Black, 9, 55);
            e.Graphics.DrawString("流水号:1", new Font(new FontFamily("黑体"), 11), System.Drawing.Brushes.Black, 160, 55);

            //第四行
            e.Graphics.DrawString($"金额:{tempOpenPrice}", new Font(new FontFamily("黑体"), 11), System.Drawing.Brushes.Black, 9, 75);
            e.Graphics.DrawString($"销售时间:{tempOrderTime}", new Font(new FontFamily("黑体"), 11), System.Drawing.Brushes.Black, 100, 75);

            //产品信息
            int line = 75;
            var numStr = tempBuyCPNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //是否要换行了 1要换行
            int state = 0;
            int x = 9;
            foreach (var item in numStr)
            {
                string[] g = item.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                string str = "+";
                int count = 1;
                for (int i = 0; i < g.Length; i++)
                {
                    string num = g[i];
                    if ((count - 1) % 5 == 0)
                    {
                        line += 20;
                        x = 9;
                    }
                    else x += 55;
                    e.Graphics.DrawString(num + (g.Count() == count ? "" : str), new Font(new FontFamily("黑体"), 10), System.Drawing.Brushes.Black, x, line);
                    count++;
                }
                x = 9;
                line += 20;
            }
            e.Graphics.DrawString($"★感谢您为公益事业贡献公益金{(tempOpenPrice.ToDecimal() * 0.29m)}元★", new Font(new FontFamily("黑体"), 10), System.Drawing.Brushes.Black, 9, line);
            line += 20;
            e.Graphics.DrawString($"站地址:渝北区松石支路77附14号", new Font(new FontFamily("黑体"), 10), System.Drawing.Brushes.Black, 9, line);
            line += 20;
            e.Graphics.DrawString($"验票码:{tempOrderNo}", new Font(new FontFamily("黑体"), 9), System.Drawing.Brushes.Black, 9, line);
            Image image = Image.FromFile(tempOneCodeFilePath);
            e.Graphics.DrawImage(image, 0, line+35, image.Width, 40);
            e.Graphics.DrawString($"123", new Font(new FontFamily("黑体"), 1), System.Drawing.Brushes.Black, 9, 320);
        }

        public void LoadOpenList()
        {
            lab_currentNo.Text = GetCurrentNo();
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                //URL = "http://self.cqwxcp.club/selfWeiClient/cqkl10_trendchart.action",
                URL = "https://kkj.13322.com/kl10_cqkl10_BaseTrend.html"
            };
            var res = helper.GetHtml(item);
            var html = res.Html;
            var reg = new Regex(@"<div id=""chartLinediv"" style=""position:relative;\*position:static;"">(?<content>.*?)</div>", RegexOptions.Singleline);
            var regStrs = reg.Matches(html);
            var newList = new List<OpenRecord>();

            string tableListStr = regStrs[0].Groups["content"].ToString();
            //匹配期号
            var regNum = new Regex(@"<td class=""tdbbs tdbrs"">(?<expectNo>.*?)</td>", RegexOptions.Singleline);
            var regNumMatch = regNum.Matches(tableListStr);

            //匹配开奖号码
            var regOpenNum = new Regex(@"<p class=""mw160"">(?<content>.*?)</p>", RegexOptions.Singleline);
            var regOpenNumMatch = regOpenNum.Matches(tableListStr);

            //每日开奖多少期
            var totalNum = new Regex(@"<font class=""red"" id=""top_total"">(?<number>.*?)</font>", RegexOptions.Singleline);
            var totalNumMatch = totalNum.Matches(html);
            string totalQi = "41";

            //剩余开奖多少期
            var syNum = new Regex(@"<font class=""red"" id=""top_remain"">(?<number>.*?)</font>", RegexOptions.Singleline);
            var syNumMatch = syNum.Matches(html);
            string syCount = (syNumMatch.Count > 0 ? syNumMatch[0].Groups["number"].ToString() : "");

            openJiangDesc.Text = $"每日开奖{totalQi}期";
            int i = 0;
            foreach (Match m in regNumMatch)
            {
                var expectNo = m.Groups["expectNo"].ToString();
                var contentStr = regOpenNumMatch[i].Groups["content"].ToString();
                
                var regResult = new Regex(@"<b>(?<number>.*?)</b>", RegexOptions.Singleline);
                var trsRegexes = regResult.Matches(contentStr);
                var numbers = "";
                foreach (Match mItem in trsRegexes)
                {
                    var a = mItem.Groups["number"].ToString();
                    numbers += a + ",";
                }
                newList.Add(new OpenRecord()
                {
                    ExpectNo = expectNo,
                    OpenNo = numbers,
                    OpenTime = ""
                });
                i++;
            }

            if (newList.Count > 0)
            {
                newList = newList.OrderBy(d => d.OpenTime).ToList();
                foreach (var record in newList)
                {
                    var list = openList.Where(d => d.ExpectNo == record.ExpectNo).ToList();
                    if (list.Count == 0)
                    {
                        openList.Insert(0, record);
                    }
                }
            }
            //(openList.Max(m => m.ExpectNo.ToInt64() + 1)).ToString()
            
            //结算
            Confirn();
            BindOpenList();
        }

        void LoadHttpOpenNum(string OrderNo)
        {
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL =$"https://kkj.13322.com/kl10_cqkl10_BaseTrend_{OrderNo}_{OrderNo}_s.html"
            };
            var res = helper.GetHtml(item);
            var html = res.Html;
            var reg = new Regex(@"<div id=""chartLinediv"" style=""position:relative;\*position:static;"">(?<content>.*?)</div>", RegexOptions.Singleline);
            var regStrs = reg.Matches(html);
            var newList = new List<OpenRecord>();

            string tableListStr = regStrs[0].Groups["content"].ToString();
            //匹配期号
            var regNum = new Regex(@"<td class=""tdbbs tdbrs"">(?<expectNo>.*?)</td>", RegexOptions.Singleline);
            var regNumMatch = regNum.Matches(tableListStr);

            //匹配开奖号码
            var regOpenNum = new Regex(@"<p class=""mw160"">(?<content>.*?)</p>", RegexOptions.Singleline);
            var regOpenNumMatch = regOpenNum.Matches(tableListStr);

            //每日开奖多少期
            var totalNum = new Regex(@"<font class=""red"" id=""top_total"">(?<number>.*?)</font>", RegexOptions.Singleline);
            var totalNumMatch = totalNum.Matches(html);
            string totalQi = "41";

            //剩余开奖多少期
            var syNum = new Regex(@"<font class=""red"" id=""top_remain"">(?<number>.*?)</font>", RegexOptions.Singleline);
            var syNumMatch = syNum.Matches(html);
            string syCount = (syNumMatch.Count > 0 ? syNumMatch[0].Groups["number"].ToString() : "");

            openJiangDesc.Text = $"每日开奖{totalQi}期";
            int i = 0;
            foreach (Match m in regNumMatch)
            {
                var expectNo = m.Groups["expectNo"].ToString();
                var contentStr = regOpenNumMatch[i].Groups["content"].ToString();

                var regResult = new Regex(@"<b>(?<number>.*?)</b>", RegexOptions.Singleline);
                var trsRegexes = regResult.Matches(contentStr);
                var numbers = "";
                foreach (Match mItem in trsRegexes)
                {
                    var a = mItem.Groups["number"].ToString();
                    numbers += a + ",";
                }
                newList.Add(new OpenRecord()
                {
                    ExpectNo = expectNo,
                    OpenNo = numbers,
                    OpenTime = ""
                });
                i++;
            }
            if (newList.Count > 0)
            {
                newList = newList.OrderBy(d => d.OpenTime).ToList();
                foreach (var record in newList)
                {
                    var list = openList.Where(d => d.ExpectNo == record.ExpectNo).ToList();
                    if (list.Count == 0)
                    {
                        openList.Insert(0, record);
                    }
                }
            }
        }

        /// <summary>
        /// 计算开奖
        /// </summary>
        void Confirn()
        {
            //原本是下了10块 中了20 result就是20 没中就是 0 state只有两种状态 中和挂
            foreach (DataGridViewRow item in dgv_orderlist.Rows)
            {
                try
                {
                    string OrderNo = item.Cells["OrderNo"]?.Value?.ToString()??"";
                    if (!string.IsNullOrEmpty(OrderNo))
                    {
                        string stateColumnStr = item.Cells["OpenState"].Value?.ToString() ?? "";
                        string OpenNum = item.Cells["CN_OpenNum"].Value.ToString();
                        string BuyCPNum = item.Cells["BuyCPNum"].Value.ToString();
                        if (string.IsNullOrEmpty(stateColumnStr)|| stateColumnStr=="未开奖")
                        {
                            List<string> currentOpenNum = openList?.Find(k => k.ExpectNo.Replace("\r\n", "").Replace("\t", "").Replace(" ", "") == OpenNum)?
                                .OpenNo?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
                            if (currentOpenNum.Count == 0)
                            {
                                LoadHttpOpenNum(OpenNum);
                                currentOpenNum = openList?.Find(k => k.ExpectNo.Replace("\r\n", "").Replace("\t", "").Replace(" ", "") == OpenNum)?
                                .OpenNo?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
                            }

                            //开奖期数
                            int state;
                            int money;
                            string DrawRecordStr;
                            if (currentOpenNum.Count > 0)
                            {
                                string number = BuyCPNum;
                                ComputeResult(OrderNo, currentOpenNum, out state, out money,out DrawRecordStr);
                            }
                            else
                            {
                                state = 0;
                                money = 0;
                                DrawRecordStr = "";
                            }
                            string stateStr = state == 1 ? "中" : state == 2 ? "挂" : "未开奖";
                            OrderListOperate.UpdateOrder(OrderNo, stateStr, money, DrawRecordStr);
                            var order = orderList.Find(m => m.OrderNo == OrderNo);
                            order.Result = state == 0 ? "" : money.ToString();
                            order.State = stateStr;
                            item.Cells["OpenState"].Value = stateStr;
                            item.Cells["OpenPrice"].Value = order.Result;
                            if (string.IsNullOrEmpty(order.Result))
                            {
                                item.Cells["Del"].Value = "撤单";
                            }
                            if (state==1)
                            {
                                item.Cells["PrintDrawOrder"].Value = "打印兑奖票";
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            
            orderList.ForEach(m =>
            {
                List<string> currentOpenNum = openList?.Find(k => k.ExpectNo.Replace("\r\n", "").Replace("\t", "").Replace(" ", "") == m.ExpectNo)?.OpenNo?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
                //开奖期数
                int state;
                int money;
                string DrawRecordStr;
                if (currentOpenNum.Count > 0)
                {
                    string number = m.BetContent;
                    ComputeResult(m.OrderNo, currentOpenNum, out state, out money,out DrawRecordStr);
                }
                else
                {
                    state = 0;
                    money = 0;
                    DrawRecordStr = "";
                }
                if (string.IsNullOrEmpty(m.Result))
                {
                    string stateStr = state == 1 ? "中" : state == 2 ? "挂" : "未开奖";
                    OrderListOperate.UpdateOrder(m.OrderNo, stateStr, money, DrawRecordStr);
                    m.Result = state == 0 ? "" : money.ToString();
                    m.State = state == 1 ? "中" : state == 2 ? "挂" : "未开奖";
                }
            });
        }

        /// <summary>
        /// 计算是否中奖
        /// </summary>
        /// <param name="number">号码</param>
        /// <param name="state">1 中 2挂 0未开奖</param>
        /// <param name="money">挂就是0</param>
        void ComputeResult(string OrderNo, List<string> currentOpenNum, out int state, out int money,out string DrawRecordStr)
        {
            state = 2;
            money = 0;
            DrawRecordStr="";
            var order = orderList.Find(m => m.OrderNo == OrderNo);
            if (order == null) return;
            int orderBetType = order.BetType;
            int orderPlayType = order.BetModeType;
            BetType betTypeEnum = (BetType)orderBetType;
            string content = order.BetContent;

            var cList = content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in cList)
            {
                int drawTimes = 1;
                var betNumObj = item.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                int drawState = 2;
                int drawMoney = 0;
                if (betNumObj.Count() > 0)
                {
                    if (betNumObj.Count() == 2) drawTimes = betNumObj[1].ToInt32();
                    string num = betNumObj[0];
                    List<string> numberList = num.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (BetType.OneBet == betTypeEnum)
                    {
                        OneBet(orderBetType, orderPlayType, numberList, currentOpenNum, ref drawState, ref drawMoney);
                    }
                    else if (BetType.CompoundBet == betTypeEnum)
                    {
                        CompoundBet(orderBetType, orderPlayType, numberList, currentOpenNum, ref drawState, ref drawMoney);
                    }
                    else if (BetType.DTBet == betTypeEnum)
                    {
                        DTBet(orderBetType, orderPlayType, num, currentOpenNum, ref drawState, ref drawMoney);
                    }
                    if (drawState == 1)
                    {
                        DrawRecordStr = item + ",";
                        state = 1;
                        money += drawMoney * drawTimes;
                    }
                }
            }
            DrawRecordStr = DrawRecordStr.Trim(',');
        }

        //单式投注
        void OneBet(int betType,int modeType, List<string> number, List<string> currentOpenNum, ref int state, ref int money)
        {
            OneBetPlayMode playMode = (OneBetPlayMode)modeType;
            BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType);
            switch (playMode)
            {
                case OneBetPlayMode.OneBetLuckyTwo:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num ==2).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 2)
                        {
                            number = number.Take(2).ToList();
                            if (currentOpenNum.Intersect(number).Count() == number.Count())
                            {
                                state = 1;
                                money = drawPrice;
                            }
                        }
                    }
                    break;
                case OneBetPlayMode.OneBetLuckyThree:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num == 3).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 3)
                        {
                            number = number.Take(3).ToList();
                            if (currentOpenNum.Intersect(number).Count() == number.Count())
                            {
                                state = 1;
                                money = drawPrice;
                            }
                        }
                    }
                    break;
                case OneBetPlayMode.OneBetLuckyFour:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num == 4).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 4)
                        {
                            number = number.Take(4).ToList();
                            if (currentOpenNum.Intersect(number).Count() == number.Count())
                            {
                                state = 1;
                                money = drawPrice;
                            }
                        }
                    }
                    break;
                case OneBetPlayMode.OneBetLuckyFive:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num == 5).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 5)
                        {
                            number = number.Take(5).ToList();
                            if (currentOpenNum.Intersect(number).Count() == number.Count())
                            {
                                state = 1;
                                money = drawPrice;
                            }
                        }
                    }
                    break;
                case OneBetPlayMode.OneBetThreeScore:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num == 3).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 3 && currentOpenNum.Count >= 3)
                        {
                            //把两个数组都取前三个数字进行排序，让前三个数字一样
                            number = number.Take(3).OrderBy(m => m.ToInt32()).ToList();
                            currentOpenNum = currentOpenNum.Take(3).OrderBy(m => m.ToInt32()).ToList();
                            int i = 0;
                            bool flag = false;
                            foreach (var item in number)
                            {
                                //如果有一个数字没匹配就没中奖
                                if (item != currentOpenNum[i])
                                {
                                    flag = true;
                                    break;
                                }
                                i++;
                            }
                            //如果前三个数字匹配
                            if (!flag)
                            {
                                state = 1;
                                money = 1300;
                            }
                        }
                    }
                    break;
                case OneBetPlayMode.OneBetThreeSeriesScore:
                    {
                        int betPrice = betModeModel.BetPriceMode.Find(m => m.num == 3).price;
                        int drawPrice = betModeModel.DrawMode.FirstOrDefault().price;
                        if (number.Count >= 3 && currentOpenNum.Count >= 3)
                        {
                            number = number.Take(3).ToList();
                            currentOpenNum = currentOpenNum.Take(3).ToList();
                            int i = 0;
                            bool flag = false;
                            foreach (var item in number)
                            {
                                //如果有一个数字没匹配就没中奖
                                if (item != currentOpenNum[i])
                                {
                                    flag = true;
                                    break;
                                }
                                i++;
                            }
                            //如果前三个数字匹配
                            if (!flag)
                            {
                                state = 1;
                                money = 8000;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //复试投注
        void CompoundBet(int betType, int modeType, List<string> number, List<string> currentOpenNum, ref int state, ref int money)
        {
            CompoundBetPlayMode playMode = (CompoundBetPlayMode)modeType;
            if (playMode == CompoundBetPlayMode.CompoundBetBetThreeScore)
            {
                //三全中玩法只匹配前面三个数字
                currentOpenNum = currentOpenNum.Take(3).ToList();
            }

            BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType);
            int count = betModeModel.BetMinCount;

            //投注金额
            int betPrice = betModeModel.BetPriceMode.Find(m => m.num == number.Count())?.price ?? 0;

            //中奖列表（里面标注着中了多少个多少钱）
            var drawList = betModeModel.DrawMode;
            //当前玩法最大开奖数量
            int maxDrawCount = betModeModel.DrawMode.Max(m => m.num);
            //当前投注中了多少个数字
            int drawCount = currentOpenNum.Intersect(number).Count();
            //drawCount = drawCount > maxDrawCount ? maxDrawCount : drawCount;
            //当前投注是否中奖了
            var priceModel = drawList.Find(m => m.num == drawCount);
            if (priceModel != null)
            {
                state = 1;
                money = priceModel.price;
            }
        }

        //胆拖投注
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="currentOpenNum"></param>
        /// <param name="state"></param>
        /// <param name="money"></param>
        void DTBet(int betType, int modeType, string number, List<string> currentOpenNum, ref int state, ref int money)
        {
            List<string> numberCollection = number.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (numberCollection.Count != 2)
            {
                return;
            }
            if (modeType == 5)
            {
                currentOpenNum=currentOpenNum.Take(3).ToList();
            }
            //胆码
            List<string> danNumList= numberCollection[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //拖码
            List<string> tuoNumList = numberCollection[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //当前投注中了多少个数字
            int drawCount = currentOpenNum.Intersect(danNumList).Count();
            //胆码必须要全中
            if (drawCount != danNumList.Count) return;

            //拖码中的个数
            int tuoDrawCount = currentOpenNum.Intersect(tuoNumList).Count();
            BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType && m.DanCount == danNumList.Count);
            var drawList = betModeModel.DrawMode;
            //drawCount = drawCount > maxDrawCount ? maxDrawCount : drawCount;
            //当前投注是否中奖了
            var priceModel = drawList.Find(m => m.num == tuoDrawCount);
            if (priceModel != null)
            {
                state = 1;
                money = priceModel.price;
            }

        }

        int GetBuyPrice(List<string> number,int danCount=0)
        {
            BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType);
            if (betType == 3) betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType && m.DanCount == danCount);
            return betModeModel.BetPriceMode.Find(m => m.num == number.Count)?.price ?? 0;
        }

        /// <summary>
        /// 投注玩法
        /// </summary>
        enum BetType
        {
            [Description("单式投注")]
            OneBet = 1,
            [Description("复合投注")]
            CompoundBet = 2,
            [Description("胆拖投注")]
            DTBet = 3
        }

        /// <summary>
        /// 单式投注玩法
        /// </summary>
        enum OneBetPlayMode
        {
            [Description("幸运二")]
            OneBetLuckyTwo = 1,
            [Description("幸运三")]
            OneBetLuckyThree = 2,
            [Description("幸运四")]
            OneBetLuckyFour = 3,
            [Description("幸运五")]
            OneBetLuckyFive = 4,
            [Description("三全中")]
            OneBetThreeScore = 5,
            [Description("三连中")]
            OneBetThreeSeriesScore = 6
        }

        /// <summary>
        /// 复合式投注玩法
        /// </summary>
        /// 
        enum CompoundBetPlayMode
        {
            [Description("幸运二复式")]
            CompoundBetLuckyTwo = 1,
            [Description("幸运三复式")]
            CompoundBetLuckyThree = 2,
            [Description("幸运四复式")]
            CompoundBetLuckyFour = 3,
            [Description("幸运五复式")]
            CompoundBetLuckyFive = 4,
            [Description("三全中复式")]
            CompoundBetBetThreeScore = 5
        }

        enum DTBetPlayDesc
        {
            [Description("幸运二胆拖")]
            DTBetTwo = 1,
            [Description("幸运三胆拖")]
            DTBetThree = 2,
            [Description("幸运四胆拖")]
            DTBetFour = 3,
            [Description("幸运五胆拖")]
            DTBetFive = 4,
            [Description("三全中胆拖")]
            DTBetFuorSQZ = 5
        }

        /// <summary>
        /// 胆拖投注玩法
        /// </summary>
        enum DTBetPlayMode
        {
            [Description("胆拖幸运二(胆1中1)")]
            DTBetTwo = 1,
            [Description("胆拖幸运三(胆1中1)")]
            DTBetThreeD1Z1 = 2,
            [Description("胆拖幸运三(胆2中2)")]
            DTBetThreeD2Z2 = 3,
            [Description("胆拖幸运四(胆1中1)")]
            DTBetFuorD1Z1 = 4,
            [Description("胆拖幸运四(胆2中2)")]
            DTBetFuorD2Z12 = 5,
            [Description("胆拖幸运四(胆3中3)")]
            DTBetFuorD3Z3 = 6,
            [Description("胆拖幸运五(胆1中1)")]
            DTBetFiveD1Z1 = 7,
            [Description("胆拖幸运五(胆2中2)")]
            DTBetFiveD2Z2 = 8,
            [Description("胆拖幸运五(胆3中3)")]
            DTBetFiveD3Z3 = 9,
            [Description("胆拖幸运五(胆4中4)")]
            DTBetFiveD4Z4 = 10,
            [Description("胆拖三全中(胆1中1)")]
            DTBetThreeScoreD1Z1 = 11,
            [Description("胆拖三全中(胆2中2)")]
            DTBetThreeScoreD2Z2 = 12
        }

        /// <summary>
        /// 数字描述
        /// </summary>
        enum NumDescNumDesc
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

        public void BindOpenList()
        {
            if (dgv_openlist.IsHandleCreated)
            {
                dgv_openlist.Invoke(new Action(delegate ()
                {
                    if (openList.Count > 0)
                    {
                        dgv_openlist.DataSource = openList.ToDataTable();
                        dgv_openlist.Refresh();
                        dgv_openlist.Sort(dgv_openlist.Columns["ExpectNo"], ListSortDirection.Descending);
                    }
                }));
            }
        }

        public void BindOrderList()
        {
            if (dgv_orderlist.IsHandleCreated)
            {
                dgv_orderlist.Invoke(new Action(delegate ()
                {
                    if (orderList.Count > 0)
                    {
                        List<DataGridViewRow> dataGridViewRows = new List<DataGridViewRow>();
                        foreach (var order in orderList.OrderByDescending(m => m.CreateTime))
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.OrderNo });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.ExpectNo });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.buyModeStr });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.CreateTime.ToString("yyyy/MM/dd HH:mm:ss") });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.BetMode });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.Result });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.State });
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.price });
                            var printButton = new DataGridViewButtonCell() { Value = "打印" };
                            row.Cells.Add(printButton);
                            var returnButton = new DataGridViewButtonCell() { Value = string.IsNullOrEmpty(order.Result) || order.Result == "未开奖" ? "撤单" : "" };
                            row.Cells.Add(returnButton);
                            var drawButton = new DataGridViewButtonCell() { Value = order.State == "中" ? "打印兑奖票" : "" };
                            row.Cells.Add(drawButton);
                            dataGridViewRows.Add(row);
                        }
                        dgv_orderlist.Rows.Clear();
                        dgv_orderlist.Rows.AddRange(dataGridViewRows.ToArray());
                        dgv_orderlist.Refresh();
                    }
                }));
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void dgv_orderlist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgv_orderlist.CurrentRow.Cells.Count > 0
                        && dgv_orderlist.CurrentCell != null
                        && dgv_orderlist.CurrentRow.Cells["OrderNo"].Value != null)
                    {
                        string OrderNo = dgv_orderlist.CurrentRow.Cells["OrderNo"].Value.ToString();//订单号
                        string OpenPrice = dgv_orderlist.CurrentRow.Cells["OpenPrice"]?.Value?.ToString() ?? "";//开奖金额

                        if (dgv_orderlist.CurrentCell.ColumnIndex == 8)
                        {
                            string OpenNum = dgv_orderlist.CurrentRow.Cells["CN_OpenNum"].Value.ToString();//开奖号
                            string BuyCPNum = dgv_orderlist.CurrentRow.Cells["BuyCPNum"].Value.ToString();//购买号码
                            string OrderTime = dgv_orderlist.CurrentRow.Cells["OrderTime"].Value.ToString();//购买时间
                            string OpenState = dgv_orderlist.CurrentRow.Cells["OpenState"]?.Value?.ToString()??"";//开奖状态
                            string BuyPrice = dgv_orderlist.CurrentRow.Cells["BuyPrice"].Value.ToString();//开奖状态
                            string PlayMode = dgv_orderlist.CurrentRow.Cells["PlayMode"].Value.ToString();

                            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                            DialogResult dr = MessageBox.Show("确定打印?", "提示", mess);
                            if (dr == DialogResult.OK)
                            {
                                PrintHtml(OrderNo, OpenNum, BuyCPNum.Trim('+'), Convert.ToDateTime(OrderTime).ToString("yyyy/MM/dd-HH:mm:ss"), BuyPrice, OpenState, PlayMode);
                            }
                        }
                        else if (dgv_orderlist.CurrentCell.ColumnIndex == 9 && (dgv_orderlist.CurrentCell.Value?.ToString() ?? "") == "撤单")
                        {
                            if (!string.IsNullOrEmpty(OpenPrice))
                            {
                                MessageBox.Show("当前期数已开奖，不能撤单");
                                return;
                            }
                            orderList = orderList.FindAll(m => m.OrderNo != OrderNo);
                            
                            OrderListOperate.DeleteOrder(OrderNo);
                            //dgv_orderlist.CurrentRow.
                            dgv_orderlist.Rows.Remove(dgv_orderlist.CurrentRow);
                            MessageBox.Show("撤单成功");
                        }
                        else if (dgv_orderlist.CurrentCell.ColumnIndex == 10 && (dgv_orderlist.CurrentCell.Value?.ToString() ?? "") == "打印兑奖票")
                        {
                            var order = orderList.Find(m => m.OrderNo == OrderNo);
                            if (order.State == "中")
                            {
                                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                                DialogResult dr = MessageBox.Show("确定打印?", "提示", mess);
                                if (dr == DialogResult.OK)
                                {
                                    PrintDrawHtml(order.ExpectNo, order.StreamNum.ToString(), order.Result, DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss"));
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        public class OrderListOperate
        {
            static string orderFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\order.dat";
            static OrderListOperate()
            {
                if (!File.Exists(orderFilePath)) File.Create(orderFilePath);
            }

            public static List<Order> GetOrders()
            {
                List<Order> tempOrders = new List<Order>();
                try
                {
                    string str = File.ReadAllText(orderFilePath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        tempOrders = JsonConvert.DeserializeObject<List<Order>>(str);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取订单失败");
                }
                return tempOrders;
            }

            public static void InsertOrder(Order order)
            {
                List<Order> tempOrders = new List<Order>();
                try
                {
                    string str = File.ReadAllText(orderFilePath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        tempOrders = JsonConvert.DeserializeObject<List<Order>>(str);
                    }
                    tempOrders.Add(order);
                    File.WriteAllText(orderFilePath,JsonConvert.SerializeObject(tempOrders));
                }
                catch (Exception)
                {
                    MessageBox.Show("记录订单失败");
                }
            }

            public static void UpdateOrder(string orderNo,string State,int price,string DrawRecordStr)
            {
                List<Order> tempOrders = new List<Order>();
                orderList.Find(m => string.Equals(m.OrderNo, orderNo, StringComparison.OrdinalIgnoreCase)).Result = price.ToString();
                orderList.Find(m => string.Equals(m.OrderNo, orderNo, StringComparison.OrdinalIgnoreCase)).State = State;
                orderList.Find(m => string.Equals(m.OrderNo, orderNo, StringComparison.OrdinalIgnoreCase)).DrawRecordStr = DrawRecordStr;
                try
                {
                    File.WriteAllText(orderFilePath, JsonConvert.SerializeObject(orderList));
                }
                catch (Exception)
                {
                    MessageBox.Show("记录订单失败");
                }
            }

            public static void DeleteOrder(string orderNo)
            {
                List<Order> tempOrders = new List<Order>();
                try
                {
                    string str = File.ReadAllText(orderFilePath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        tempOrders = JsonConvert.DeserializeObject<List<Order>>(str);
                    }
                    tempOrders = tempOrders.FindAll(m => m.OrderNo != orderNo);
                    File.WriteAllText(orderFilePath, JsonConvert.SerializeObject(tempOrders));
                }
                catch (Exception)
                {
                    MessageBox.Show("记录订单失败");
                }
            }

        }

        private void btn_CreateOrder_Click(object sender, EventArgs e)
        {
            var b = "";
                b = lab_chooseNumDesc.Text;
            if (string.IsNullOrEmpty(lab_chooseNumDesc.Text))
            {
                MessageBox.Show("请选择号码");
                return;
            }
            if (betType == 3)
            {
                if (string.IsNullOrEmpty(lab_tuoNumDesc.Text))
                {
                    MessageBox.Show("请选择拖码");
                    return;
                }
                b += "|" + lab_tuoNumDesc.Text;
            }
            CreateOrder(b + "~" + times);
        }

        void CreateOrder(string num)
        {
            var a = GetCurrentNo();
            var list = num.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (list.Count == 0)
            {
                MessageBox.Show("没有选择号码");
                return;
            }
            if ((betType == 3 || betType == 2) && list.Count > 1)
            {
                MessageBox.Show("复式玩法和胆拖玩法每次只能下一注");
                return;
            }
            else if (betType == 1 && list.Count > 10)
            {
                MessageBox.Show("单式玩法每次最多下10注");
                return;
            }

            string[] betList = num.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 1;
            string errorMsg = "";
            string betNumStr = "";
            foreach (string bet in betList)
            {
                //1,2,3~1 前面是下注号码 后面是倍率
                string[] array = bet.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Count() > 2)
                {
                    errorMsg = $"第{i}注格式错误,注码为：{bet}。\n正确注码格式:1,2,4|1";
                    break;
                }
                string betTimes = array.Count() == 2 ? array[1] : "1";
                betTimes = betTimes == "0" ? "1" : betTimes;
                long t;
                if (!Int64.TryParse(betTimes, out t))
                {
                    errorMsg = $"第{i}注格式错误,注码为：{bet},倍数只能是数字";
                    break;
                }
                if (t > 1000)
                {
                    errorMsg = $"第{i}注格式错误,注码为：{bet},不能超过1000倍";
                    break;
                }
                //如果是胆拖玩法就有两个数组第一个是胆码第二个是拖码
                var numListArray=array[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                bool flag1 = true;
                string line;
                GetNumByNumList(numListArray[0], out line);
                if (!string.IsNullOrEmpty(line))
                {
                    errorMsg = $"第{line}注格式错误,注码为：{bet},数字只能输入1-20";
                    break;
                }

                if (betType == 3)
                {
                    if (numListArray.Count() < 2)
                    {
                        errorMsg = $"请输入托码第{line}注格式错误,注码为：{bet},格式 1 2 3|4";
                        break;
                    }
                    GetNumByNumList(numListArray[1], out line);
                    if (!string.IsNullOrEmpty(line))
                    {
                        errorMsg = $"第{line}注格式错误,注码为：{bet},数字只能输入1-20";
                        break;
                    }

                }

                if (!flag1) break;


                betNumStr += "~" + betTimes + ",";
                i++;
            }
            betNumStr = betNumStr.Trim(',');
            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                return;
            }

            bool flag = true;
            
            foreach (var item in betList)
            {
                var numObj = item.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (numObj.Count != 2)
                {
                    MessageBox.Show("下注格式错误");
                    flag = false;
                    break;
                }
                string n = numObj[0];
                if (!CheckNum(n))
                {
                    flag = false;
                    break;
                }
            }
            
            if (!flag) return;

            num = num.Trim();
            var orderNo = Guid.NewGuid().ToString("N").ToUpper();



            orderNo = orderNo.Substring(0, 4) + "-" + orderNo.Substring(4, 4) + "-" + orderNo.Substring(8, 4) + "-" + orderNo.Substring(12, 4)
            + "-" + orderNo.Substring(16, 4) + "-" + orderNo.Substring(20, 4) + "-" + orderNo.Substring(24, 4);

            int buyPrice = 0;
            foreach (var item in num.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var numStrList = item.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                int tempTimes = numStrList.Count() == 2 ? numStrList[1].ToInt32() : 1;
                var numList = numStrList[0];
                List<string> numberList = numList.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                int danCount = 0;
                if (betType == 3)
                {
                    List<string> numberCollection = numList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var danList = numberCollection[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    numberList = numberCollection[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList(); ;
                    danCount = danList.Count;
                }
                buyPrice += GetBuyPrice(numberList, danCount) * tempTimes;
            }

            var validateNo = Guid.NewGuid().ToString("N").ToUpper();
            validateNo = validateNo.Substring(0, 8) + "-" + validateNo.Substring(8, 8) + "-" + validateNo.Substring(16, 8) + "-" + validateNo.Substring(24, 8);
            var order = new Order()
            {
                times = times,
                BetContent = num,
                ExpectNo = a.Trim(),
                CreateTime = DateTime.Now,
                OrderNo = orderNo,
                BetMode = playModeTxt,
                BetModeType = modeType,
                BetType = betType,
                price = buyPrice,
                Validate = validateNo
            };

            order.StreamNum = getStream();
            orderList.Add(order);
            OrderListOperate.InsertOrder(order);
            DataGridViewRow row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.OrderNo });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.ExpectNo });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.buyModeStr });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.CreateTime.ToString("yyyy/MM/dd HH:mm:ss") });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.BetMode });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.Result });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.State });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = order.price });
            row.Cells.Add(new DataGridViewButtonCell() { Value = "打印" });
            dgv_orderlist.Rows.Insert(0, row);

            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("订单录入成功,是否需要打印?", "提示", mess);
            if (dr == DialogResult.OK)
            {
                PrintHtml(order.OrderNo, order.ExpectNo, order.buyModeStr, order.CreateTime.ToString("yyyy/MM/dd HH:mm:ss"), buyPrice.ToString(), "", order.BetMode);
            }
        }

        int getStream()
        {
            int num;
            var oList = orderList.FindAll(m => m.ExpectNo== GetCurrentNo());
            if (oList.Count > 0)
            {
                num = oList.Max(m => m.StreamNum) + 1;
            }
            else
            {
                num = 1;
            }
            return num;
        }

        /// <summary>
        /// 获取当前开奖期数
        /// </summary>
        /// <returns></returns>
        string GetCurrentNo()
        {
            int minute = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            int qishu = (minute % 20 == 0 ? 1 : 0) + Convert.ToInt32((Math.Ceiling(Decimal.Parse(minute.ToString()) / 20)));
            if (qishu == 60 || (qishu > 10 && qishu < 22)) qishu = 0;
            else if (qishu > 21) qishu -= 12;
            string str = DateTime.Now.ToString("yyyyMMdd") + qishu.ToString().PadLeft(3, '0');
            return str;
        }

        void LoadingKaiJiang()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkNum">格式：1 2 3 4 5</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        string GetNumByNumList(string checkNum,out string errorMsg)
        {
            string[] betNumList = checkNum.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            errorMsg = "";
            string betNumStr = "";
            int i = 1;
            foreach (var betNum in betNumList)
            {
                string newBetNum = betNum.PadLeft(2, '0');
                betNumStr += newBetNum + " ";
                if (!openNum.Contains(newBetNum))
                {
                    errorMsg = i.ToString();
                    break;
                }
                i++;
            }
            return betNumStr;
        }

        bool CheckNum(string num)
        {
            bool flag = false;
            if (betType == 1)
            {
                List<string> numList = num.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string errorNum;
                if (IsSameWithArrayContains(numList.ToArray(), out errorNum))
                {
                    MessageBox.Show($"{errorNum}重复");
                    return false;
                }

                BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType);
                string desc = GetEnumDescription((OneBetPlayMode)modeType);
                if (betModeModel.BetMaxCount == numList.Count) flag = true;
                else MessageBox.Show($"{desc}玩法只能选择{betModeModel.BetMaxCount}个数字");
            }
            else if (betType == 2)
            {
                List<string> numList = num.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string errorNum;
                if (IsSameWithArrayContains(numList.ToArray(), out errorNum))
                {
                    MessageBox.Show($"{errorNum}重复");
                    return false;
                }
                BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType);
                string desc = GetEnumDescription((CompoundBetPlayMode)modeType);
                int maxCount = betModeModel.BetMaxCount;
                int minCount = betModeModel.BetMinCount;
                int selectCount = numList.Count;
                if (selectCount >= minCount && selectCount <= maxCount) flag = true;
                else
                {
                    if (selectCount < minCount) MessageBox.Show($"{desc}玩法最少选择{minCount}个数字");
                    else MessageBox.Show($"{desc}玩法最多选择{maxCount}个数字");
                }
            }
            else if (betType == 3)
            {
                flag= CheckTuo(num);
            }
            return flag;
        }

        bool IsSameWithArrayContains(string[] arr,out string num)
        {
            num = "";
            var newArr = new string[arr.Length];
            var idx = 0;
            foreach (var i in arr)
            {
                if (false == newArr.Contains(i))
                {
                    newArr[idx] = i;
                    idx++;
                }
                else
                {
                    num = i;
                    return true;
                }
            }
            return false;
        }

        bool CheckTuo(string num)
        {
            List<string> numberCollection = num.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (numberCollection.Count != 2)
            {
                MessageBox.Show("请选择拖码");
                return false;
            }
            List<string> danMa = numberCollection[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> tuoMa = numberCollection[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int danMaCount = danMa.Count;
            int tuoMaCount = tuoMa.Count;
            if (danMaCount == 0)
            {
                MessageBox.Show("请选择胆码");
                return false;
            }
            if (tuoMaCount == 0)
            {
                MessageBox.Show("请选择拖码");
                return false;
            }
            string errorNum1;
            if (IsSameWithArrayContains(danMa.ToArray(), out errorNum1))
            {
                MessageBox.Show($"胆码{errorNum1}重复");
                return false;
            }
            string errorNum2;
            if (IsSameWithArrayContains(tuoMa.ToArray(), out errorNum2))
            {
                MessageBox.Show($"拖码{errorNum2}重复");
                return false;
            }
            string desc = GetEnumDescription((DTBetPlayDesc)modeType);
            int maxDanCount = modeType;
            maxDanCount = modeType == 5 ? 2 : maxDanCount;
            if (danMaCount > maxDanCount)
            {
                MessageBox.Show($"{desc}玩法最多只能选择{maxDanCount}个胆码");
                return false;
            }
            if (danMa.Intersect(tuoMa).Count() > 0)
            {
                MessageBox.Show("胆码和拖码不能一样！");
                return false;
            }
            BetModeModel betModeModel = playRuleList.Find(m => m.BetType == betType).BetMode.Find(m => m.BetMode == modeType && m.DanCount == danMaCount);
            int betMinCount = betModeModel.BetMinCount;
            int betMaxCount = betModeModel.BetMaxCount;
            if (tuoMaCount < betMinCount)
            {
                MessageBox.Show($"最少选择{betMinCount}个拖码！");
                return false;
            }
            if (tuoMaCount > betMaxCount)
            {
                MessageBox.Show($"最多选择{betMaxCount}个拖码！");
                return false;
            }
            return true;
        }
        
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            //快捷键选中复式
            if (key == Keys.E)
            {
                SetConfigStyle(2);
                tabShow.SelectTab(tabPage2);
            }
            //快捷键选中胆拖
            else if (key == Keys.C)
            {
                SetConfigStyle(3);
                tabShow.SelectTab(tabPage2);
            }
            //快捷键选中单式
            else if (key == Keys.B)
            {
                SetConfigStyle(1);
                tabShow.SelectTab(tabPage2);
            }
            //退出-
            else if (key == Keys.ProcessKey)
            {
                tabShow.SelectTab(tabPage3);
            }
            //机选（随机选择一个号码）
            else if (key == Keys.F3 || key == Keys.Delete)
            {
                string num;
                int count = 1;
                string randomNum = "";
                string strText;
                string times;
                string num1;
                string num2;
                DialogResult dResult = ChooseBetConfig(betType, modeType, out strText, out times, out num1, out num2);
                if (dResult != DialogResult.OK) return;

                if (betType == 1)
                {
                    if (Int32.TryParse(strText, out count))
                    {
                        if (count <= 0 || count > 10)
                        {
                            MessageBox.Show("注数必须必须小于10大于0");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("注数必须为整数");
                        return;
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    randomNum += GetRandomNumber(times, num1, num2) + "\n";
                }

                DialogResult dialogResult = ShowChooseBet(out num, randomNum);
                num = num.Replace("\n", ",");
                if (dialogResult == DialogResult.OK)
                {
                    CreateOrder(num);
                }
            }
            else if (Keys.Oemcomma == key)
            {
                string str;
                if (Show(out str) == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        int k;
                        if (Int32.TryParse(str, out k))
                        {
                            SetTimes(k);
                        }
                        else
                        {
                            MessageBox.Show("请输入数字");
                        }
                    }
                }
            }
            else if (key == Keys.D1 || key == Keys.NumPad1)
            {
                
                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)1))}", "提示", mess);
                if (dr != DialogResult.OK) return;
                modeType = 1;
                betType = chooseType;
                FrmMain_Load(null, null);
                tabShow.SelectTab(tabPage3);
            }
            else if (key == Keys.D2 || key == Keys.NumPad2)
            {
                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)2))}", "提示", mess);
                if (dr != DialogResult.OK) return;
                modeType = 2;
                betType = chooseType;
                FrmMain_Load(null, null);
                tabShow.SelectTab(tabPage3);
            }
            else if (key == Keys.D3 || key == Keys.NumPad3)
            {
                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)3))}", "提示", mess);
                if (dr != DialogResult.OK) return;
                modeType = 3;
                betType = chooseType;
                FrmMain_Load(null, null);
                tabShow.SelectTab(tabPage3);
            }
            else if (key == Keys.D4 || key == Keys.NumPad4)
            {
                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)4))}", "提示", mess);
                if (dr != DialogResult.OK) return;
                modeType = 4;
                betType = chooseType;
                FrmMain_Load(null, null);
                tabShow.SelectTab(tabPage3);
            }
            else if (key == Keys.D5 || key == Keys.NumPad5)
            {
                MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)5))}", "提示", mess);
                if (dr != DialogResult.OK) return;
                modeType = 5;
                betType = chooseType;
                FrmMain_Load(null, null);
                tabShow.SelectTab(tabPage3);
            }
            else if (key == Keys.D6 || key == Keys.NumPad6)
            {
                if (betType == 1)
                {
                    MessageBoxButtons mess = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show($"确定要切换到{(GetEnumDescription((BetType)chooseType) + " " + GetEnumDescription((OneBetPlayMode)6))}", "提示", mess);
                    if (dr != DialogResult.OK) return;
                    modeType = 6;
                    betType = chooseType;
                    FrmMain_Load(null, null);
                    tabShow.SelectTab(tabPage3);

                }
                
            }
        }

        public DialogResult ChooseBetConfig(int betType, int modeType, out string strText, out string times, out string num1, out string num2)
        {
            string strTemp = string.Empty;
            string timesTemp = string.Empty;
            string num1Temp = string.Empty;
            string num2Temp = string.Empty;
            JiXuan inputDialog = new JiXuan(betType, modeType);
            inputDialog.TextHandler = (str, strtimes, strnum1, strnum2) =>
            {
                strTemp = str;
                timesTemp = strtimes;
                num1Temp = strnum1;
                num2Temp = strnum2;
            };
            DialogResult result = inputDialog.ShowDialog();
            strText = strTemp;
            times = timesTemp;
            num1 = num1Temp;
            num2 = num2Temp;
            return result;
        }

        public DialogResult Show(out string strText,string formTitle="")
        {
            string strTemp = string.Empty;
            Form_InputDialog inputDialog = new Form_InputDialog(formTitle);
            inputDialog.TextHandler = (str) => { strTemp = str; };
            DialogResult result = inputDialog.ShowDialog();
            strText = strTemp;

            return result;
        }

        public DialogResult ShowChooseBet(out string strText,string defaultVal="")
        {
            string strTemp = string.Empty;

            ManyBet inputDialog = new ManyBet(defaultVal);
            inputDialog.TextHandler = (str) => 
            {
                strTemp = str;
            };
            DialogResult result = inputDialog.ShowDialog();
            strText = strTemp.Trim().Replace(" ", "");
            
            string[] rows = strText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string num = "";
            foreach (var item in rows)
            {
                var bei = item.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                if (bei.Count() > 0)
                {
                    //是否胆拖玩法
                    var numList = bei[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    num += string.Join(" ", Regex.Split(numList[0], "(?<=\\G.{2})")).Trim();

                    if (numList.Count() > 1)
                    {
                        string str2 = string.Join(" ", Regex.Split(numList[1], "(?<=\\G.{2})"));
                        num += "|" + str2.Trim();
                    }
                    if (bei.Count() > 1)
                    {
                        num += "~" + bei[1];
                    }
                }
                num += "\n";
            }
            strText = num.Trim(',');
            //var bei = strText.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
            //if (bei.Count() > 0)
            //{
            //    //是否胆拖玩法
            //    var numList = bei[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            //    strText = string.Join(" ", Regex.Split(numList[0], "(?<=\\G.{2})")).Trim(',');

            //    if (numList.Count() > 1)
            //    {
            //        string str2 = string.Join(" ", Regex.Split(numList[1], "(?<=\\G.{2})"));
            //        strText = strText + "|" + str2.Trim(',');
            //    }
            //}

            //string[] s = Regex.Split(strText, "(?<=\\G.{2})");
            return result;
        }

        string GetRandomNumber(string times,string num1,string num2="")
        {
            List<string> allNumber = new List<string>();
            for (int i = 1; i < 21; i++) allNumber.Add(i.ToString().PadLeft(2, '0'));

            List<string> num1List = string.Join(" ", Regex.Split(num1, "(?<=\\G.{2})")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> num2List = string.Join(" ", Regex.Split(num2, "(?<=\\G.{2})")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            string num = "";
            if (betType == 1)
            {
                int count = (modeType + 1);
                if (modeType == 5 || modeType == 6) count = 3;
                count -= num1List.Count;
                //取差集
                allNumber = allNumber.Except(num1List).ToList();
                num = num1;
                for (int i = 0; i < count; i++)
                {
                    string str = GetNumByList(allNumber);
                    num += str + "";
                    Thread.Sleep(10);
                }
            }
            else if (betType == 2)
            {
                int count = modeType + 2;
                count = new Random().Next(count, 21);
                count -= num1List.Count;
                //取差集
                allNumber = allNumber.Except(num1List).ToList();
                num = num1;
                for (int i = 0; i < count; i++)
                {
                    string str = GetNumByList(allNumber);
                    num += str + "";
                    Thread.Sleep(10);
                }
            }
            else if (betType == 3)
            {
                int maxDanNum = modeType == 5 ? 2 : modeType;
                maxDanNum = new Random().Next(1, maxDanNum+1);
                //取差集
                allNumber = allNumber.Except(num1List).Except(num2List).ToList();
                num = num1;
                int randomDanCount = maxDanNum - num1List.Count;
                for (int i = 0; i < randomDanCount; i++)
                {
                    string str = GetNumByList(allNumber);
                    num += str + "";
                    Thread.Sleep(10);
                }
                num += "/" + num2;
                int maxTuoNum = 20 - maxDanNum;
                maxTuoNum = new Random().Next(modeType - maxDanNum + 1, maxTuoNum + 1);
                int randomTuoCount = maxTuoNum - num2List.Count;
                for (int i = 0; i < randomTuoCount; i++)
                {
                    string str = GetNumByList(allNumber);
                    num += str + "";
                    Thread.Sleep(10);
                }
            }
            return num + "~" + times;
        }

        string GetNumByList(List<string> num)
        {
            string n = num[new Random().Next(0, num.Count)];
            num.Remove(n);
            return n.PadLeft(2, '0');
        }

        private void btn_normal_Click(object sender, EventArgs e)
        {
            SetConfigStyle(1);
        }

        private void btn_fs_Click(object sender, EventArgs e)
        {
            SetConfigStyle(2);
        }

        private void btn_dt_Click(object sender, EventArgs e)
        {
            SetConfigStyle(3);
        }

        int chooseType = 0;
        //设置配置样式
        void SetConfigStyle(int type)
        {
            chooseType = type;
            btn_normal.BackColor = Color.White;
            btn_fs.BackColor = Color.White;
            btn_dt.BackColor = Color.White;
            if (type == 1)
            {
                btn_normal.BackColor = Color.Orange;
                selectBetType = 1;
            }
            else if (type == 2)
            {
                btn_fs.BackColor = Color.Orange;
                selectBetType = 2;
            }
            else if (type == 3)
            {
                btn_dt.BackColor = Color.Orange;
                selectBetType = 3;
            }
            for (int i = 0; i < pnl_playMode.Controls.Count; i++)
            {

                if (pnl_playMode.Controls[i] is Button)
                {
                    var button = pnl_playMode.Controls[i] as Button;
                    int index = button.TabIndex + 1;
                    string desc = "";
                    if (type == 1)
                    {
                        desc = GetEnumDescription((OneBetPlayMode)index);
                        btn_SLZ.Show();
                    }
                    else if (type == 2&&index<6)
                    {
                        desc = GetEnumDescription((CompoundBetPlayMode)index);
                        btn_SLZ.Hide();
                    }
                    else if (type == 3 && index < 6)
                    {
                        desc = GetEnumDescription((DTBetPlayDesc)index);
                        btn_SLZ.Hide();
                    }
                    button.Text = $"【{index}】{desc}";
                }
            }
        }

        private void ClickMode(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int index = button.TabIndex + 1;
            betType = selectBetType;
            modeType = index;
            FrmMain_Load(null, null);
            tabShow.SelectTab(tabPage3);
        }

        private void clb_NumCheckList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string txt = lab_chooseNumDesc.Text;
            string selectCheckTxt = ((CheckedListBox)sender).Text;
            if (e.CurrentValue == CheckState.Checked)
            {
                e.ToString();
                lab_chooseNumDesc.Text = txt.Replace(selectCheckTxt + " ", "");
            }
            else
            {
                lab_chooseNumDesc.Text = txt + selectCheckTxt + " ";
            }
        }

        private void clb_TuoNumList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string txt = lab_tuoNumDesc.Text;
            string selectCheckTxt = ((CheckedListBox)sender).Text;
            if (e.CurrentValue == CheckState.Checked)
            {
                e.ToString();
                lab_tuoNumDesc.Text = txt.Replace(selectCheckTxt + " ", "");
            }
            else
            {
                lab_tuoNumDesc.Text = txt + selectCheckTxt + " ";
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            string str;
            if (ShowChooseBet(out str) == DialogResult.OK)
            {
                string[] betList = str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int i = 1;
                string errorMsg = "";
                string betNumStr = "";
                foreach (string bet in betList)
                {
                    //1,2,3~1 前面是下注号码 后面是倍率
                    string[] array = bet.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                    string betTimes = array.Count() == 2 ? array[1] : "1";
                    betTimes = betTimes == "0" ? "1" : betTimes;

                    string[] betNumList = array[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    bool flag = true;
                    foreach (var betNum in betNumList)
                    {
                        string newBetNum = betNum.PadLeft(2, '0');
                        betNumStr += newBetNum + " ";
                    }
                    if (!flag) break;
                    betNumStr += "~" + betTimes + ",";
                    i++;
                }
                betNumStr = betNumStr.Trim(',');
                if (string.IsNullOrEmpty(errorMsg))
                {
                    betNumStr = betNumStr.Replace("\n", ",");
                    CreateOrder(betNumStr);
                }
                else MessageBox.Show(errorMsg);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int minute = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            int qishu = (minute % 20 == 0 ? 1 : 0) + Convert.ToInt32((Math.Ceiling(Decimal.Parse(minute.ToString()) / 20)));
            int m = (qishu) * 20 * 60 - (minute * 60 + DateTime.Now.Second);
            string l = Math.Floor(Convert.ToDecimal(m) / 60).ToString() + "分" + (m % 60).ToString() + "秒";

            int qishu1 = Convert.ToInt32((Math.Ceiling(Decimal.Parse(minute.ToString()) / 20)));
            if (qishu1 == 60 || (qishu1 > 10 && qishu < 22)) qishu = 0;

            lab_opendate.Text = qishu1 == 0 ? "当前还没有开奖" : $"剩余开奖时间：{l}";
        }
    }
}