using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetingSystem
{
    public partial class Form_InputDialog : Form
    {
        public Form_InputDialog(string Name="")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Name))
            {
                this.Text = Name;
            }
        }

        public delegate void TextEventHandler(string strText);

        public TextEventHandler TextHandler;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (null != TextHandler)
            {
                TextHandler.Invoke(txtString.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter == (Keys)e.KeyChar)
            {
                if (null != TextHandler)
                {
                    TextHandler.Invoke(txtString.Text);
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void Form_InputDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
