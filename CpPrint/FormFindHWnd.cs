using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CpPrint
{
    public partial class FormFindHWnd : Form
    {
        #region 窗体级变量及引用方法定义

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int MaxCount);
        [DllImport("user32")]
        public static extern uint RealGetWindowClass(IntPtr hWnd, StringBuilder pszType, int MaxCount);
        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        public delegate bool CallBack(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
         [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width
            {
                get
                {
                    return right - left;
                }
            }

            public int Height
            {
                get
                {
                    return bottom - top;
                }
            }
        }


        IntPtr hwdFinded = IntPtr.Zero;
        IntPtr hwdApp = IntPtr.Zero;
        IntPtr hwdTemp = IntPtr.Zero;
        Rect rect;
        Image imagePre;
        Image imagePre2;

        #endregion

        public FormFindHWnd()
        {
            InitializeComponent();
            pictureBoxFindWnd.Image = Image.FromFile("FindWndHome.bmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void pictureBoxFindWnd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //设置查找图标光标
                Cursor.Current = new Cursor("findwnd.cur");
                //变更背景图片
                imagePre = pictureBoxFindWnd.Image;
                pictureBoxFindWnd.Image = Image.FromFile( "FindWndGone.bmp");
                //设置本控件捕获鼠标，处理相应的鼠标事件
                pictureBoxFindWnd.Capture = true;

                hwdFinded = IntPtr.Zero;
            }
        }

        private void pictureBoxFindWnd_MouseUp(object sender, MouseEventArgs e)
        {
           List<IntPtr> sub= GetIntPtr(hwdFinded);
           foreach (IntPtr i in sub)
           {
               //输出标题
               StringBuilder strTemp = new StringBuilder(256);
               //GetWindowText(hwdFinded, strTemp, strTemp.Capacity);
               SendMessage(i, 0x000D, 256, strTemp);
           }
            //恢复初始状态
            Cursor.Current = Cursors.Default;
            pictureBoxFindWnd.Image = imagePre;
            pictureBoxFindWnd.Capture = false;
        }


        private void pictureBoxFindWnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.Bounds.Contains(Cursor.Position))
            {
                hwdFinded = WindowFromPoint(Cursor.Position);
                if (hwdFinded != IntPtr.Zero)
                {
                    //输出句柄
                    textBoxGetHwnd.Text = hwdFinded.ToString("D").PadLeft(8,'0');

                    //输出标题
                    StringBuilder strTemp = new StringBuilder(256);
                    //GetWindowText(hwdFinded, strTemp, strTemp.Capacity);
                    SendMessage(hwdFinded, 0x000D, 256, strTemp);                 
                    //输出类名
                    RealGetWindowClass(hwdFinded, strTemp, 256);
                 
                    //向上查找Windows窗体,应用程序的主窗体的父窗体句柄为IntPtr.Zero
                    IntPtr hWdParent = GetParent(hwdFinded);
                    
                    while (hWdParent != IntPtr.Zero)
                    {
                  
                        hwdTemp = hWdParent;
                        hWdParent = GetParent(hwdTemp);
                    }
                    
                    StringBuilder title = new StringBuilder(256);
                    GetWindowText(hwdTemp, title, title.Capacity);
                    if (hwdTemp != hwdFinded)
                    {
                        hwdApp = hwdTemp;
                    }
                    else
                    {
                        hwdApp = hwdFinded;
                    }

                    //输出相对位置
                    rect = new Rect();
                    GetWindowRect(hwdFinded, ref rect);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            rect = new Rect();
            GetWindowRect(hwdFinded, ref rect);
            Point pos = Cursor.Position;
            Rectangle r = new Rectangle(rect.left, rect.bottom - 100, rect.Width, 100);
            if(!r.Contains(Cursor.Position))
            {
                Cursor.Position = new Point((rect.left+r.Width)/2, rect.bottom - 50);
                //mouse_event(0x8000, rect.left + 30, rect.bottom - 50, 0, 0);  
                mouse_event(0x0002|0x0004, 0, 0, 1, 0); //模拟鼠标按下操作
            }
            if (WindowFromPoint(Cursor.Position) == hwdFinded)
            {
                //SendMessage(hwdFinded, 0x0C, 256,new StringBuilder("我是中国人"));
                SendKeys.Send("我是中国人");
                SendKeys.Send("{ENTER}");
            }
            System.Threading.Thread.Sleep(1000);
            Cursor.Position = pos;
            //SendMessage(hwdFinded,  0x0C, 256, new StringBuilder("abc\r\n"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            //SendMessage(hwdFinded, 0x000C, 256, new StringBuilder("abc\r\n"));
            //mouse_event(0x0002, rect.left + 20, rect.bottom - 20, 0, 0); //模拟鼠标按下操作
            //mouse_event(0x0004, rect.left + 20, rect.bottom - 20, 0, 0); //模拟鼠标放开操作
            // int h=rect.Height-50-(Cursor.Position.Y - rect.top );
            // label7.Text = h.ToString();
            // if ( h> 0)
           // mouse_event(0x0001, 0, 30, 0, 0);
            //  mouse_event(0x0001, rect.left + 20, rect.bottom - 20, 0, 0);
           // SendKeys.Send("我是中国人");   //模拟键盘输入游戏ID
            //SendKeys.Send("{TAB}"); //模拟键盘输入TAB
            //SendKeys.Send(_GamePass); //模拟键盘输入游戏密码
           // SendKeys.Send("{ENTER}"); //模拟键盘输入ENTER
        }

        public static List<IntPtr> GetIntPtr(IntPtr hwd)
        {
            List<IntPtr> listIntPtr = new List<IntPtr>();
            EnumChildWindows(hwd, delegate(IntPtr hWnd, int lParam)
            {
                listIntPtr.Add(hWnd);
                return true;
            }, 0);
            return listIntPtr;
        }
        public static IntPtr FindWindowEx(IntPtr hwnd, string lpszWindow, bool bChild)
        {
            IntPtr iResult = IntPtr.Zero;
            // 首先在父窗体上查找控件
            iResult = FindWindowEx(hwnd, (uint)IntPtr.Zero, null, lpszWindow);
            // 如果找到直接返回控件句柄
            if (iResult != IntPtr.Zero) return iResult;

            // 如果设定了不在子窗体中查找
            if (!bChild) return iResult;

            // 枚举子窗体，查找控件句柄
            int i = EnumChildWindows(
            hwnd,
            (h, l) =>
            {
                IntPtr f1 = FindWindowEx(h, (uint)IntPtr.Zero, null, lpszWindow);
                if (f1 == IntPtr.Zero)
                    return true;
                else
                {
                    iResult = f1;
                    return false;
                }
            },
            0);
            // 返回查找结果
            return iResult;
        }

        private void textBoxGetAppName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
