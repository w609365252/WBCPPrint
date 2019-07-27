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

namespace CpPrint
{
    public partial class ManyBet : CCSkinMain
    {

        public delegate void TextEventHandler(string strText);

        public TextEventHandler TextHandler;
        public ManyBet(string defaultVal="")
        {
            InitializeComponent();
            lab_desc.Text = "幸运二玩法：\n010305~1\n020406~2\n表示01 03 05号1倍，\n02 04 06号下注2倍使用回车区分表示下多注\n复式玩法：\n01 02 03 04 05 06~4\n01 02 03 04 05 06号码下4倍\n胆拖玩法：\n010203/0506^1\n01 02 03为胆码05 06为拖码下注1倍";
            richTextBox1.Text = defaultVal;
        }

        private void ManyBet_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (null != TextHandler)
            {
                TextHandler.Invoke(richTextBox1.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
