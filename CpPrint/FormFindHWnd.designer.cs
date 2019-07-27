namespace CpPrint
{
    partial class FormFindHWnd
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxFindWnd = new System.Windows.Forms.PictureBox();
            this.textBoxGetHwnd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFindWnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxFindWnd
            // 
            this.pictureBoxFindWnd.Location = new System.Drawing.Point(167, 50);
            this.pictureBoxFindWnd.Name = "pictureBoxFindWnd";
            this.pictureBoxFindWnd.Size = new System.Drawing.Size(31, 28);
            this.pictureBoxFindWnd.TabIndex = 34;
            this.pictureBoxFindWnd.TabStop = false;
            this.pictureBoxFindWnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFindWnd_MouseDown);
            this.pictureBoxFindWnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFindWnd_MouseMove);
            this.pictureBoxFindWnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFindWnd_MouseUp);
            // 
            // textBoxGetHwnd
            // 
            this.textBoxGetHwnd.Location = new System.Drawing.Point(55, 54);
            this.textBoxGetHwnd.Name = "textBoxGetHwnd";
            this.textBoxGetHwnd.Size = new System.Drawing.Size(106, 21);
            this.textBoxGetHwnd.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "句柄：";
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 44;
            this.label4.Text = "句柄：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(55, 91);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(106, 21);
            this.textBox1.TabIndex = 43;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(167, 88);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(31, 28);
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // FormFindHWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 447);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxGetHwnd);
            this.Controls.Add(this.pictureBoxFindWnd);
            this.Name = "FormFindHWnd";
            this.Text = "查找窗体及控件";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFindWnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxFindWnd;
        private System.Windows.Forms.TextBox textBoxGetHwnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

