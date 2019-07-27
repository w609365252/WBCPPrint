using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
namespace BetingSystem
{
    public partial class JiXuan : CCSkinMain
    {
        public JiXuan(int betType, int modeType)
        {
            InitializeComponent();
            if (betType == 1)
            {
                txt_num2.Hide();
                lab_num2.Hide();
                lab_num1.Text = "注码";
            }
            else if (betType == 2)
            {
                txt_num2.Hide();
                lab_num2.Hide();
                lab_count.Hide();
                txt_count.Hide();
                lab_num1.Text = "注码";
            }
            else if (betType == 3)
            {
                lab_count.Hide();
                txt_count.Hide();
                lab_num1.Text = "胆码";
                lab_num2.Text = "拖码";
            }
        }

        public delegate void TextEventHandler(string strText,string times,string num1,string num2);

        public TextEventHandler TextHandler;


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (null != TextHandler)
            {
                TextHandler.Invoke(txt_count.Text, txt_times.Text, txt_num1.Text, txt_num2.Text);
                DialogResult = DialogResult.OK;
            }

        }
        
    }
}
