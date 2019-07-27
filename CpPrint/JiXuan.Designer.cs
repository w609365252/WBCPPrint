namespace BetingSystem
{
    partial class JiXuan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_num2 = new System.Windows.Forms.TextBox();
            this.lab_num2 = new System.Windows.Forms.Label();
            this.txt_num1 = new System.Windows.Forms.TextBox();
            this.lab_num1 = new System.Windows.Forms.Label();
            this.lab_count = new System.Windows.Forms.Label();
            this.txt_times = new System.Windows.Forms.TextBox();
            this.lab_times = new System.Windows.Forms.Label();
            this.txt_count = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_num2
            // 
            this.txt_num2.Location = new System.Drawing.Point(82, 126);
            this.txt_num2.Name = "txt_num2";
            this.txt_num2.Size = new System.Drawing.Size(173, 21);
            this.txt_num2.TabIndex = 19;
            // 
            // lab_num2
            // 
            this.lab_num2.AutoSize = true;
            this.lab_num2.Location = new System.Drawing.Point(36, 130);
            this.lab_num2.Name = "lab_num2";
            this.lab_num2.Size = new System.Drawing.Size(47, 12);
            this.lab_num2.TabIndex = 18;
            this.lab_num2.Text = "数字2：";
            // 
            // txt_num1
            // 
            this.txt_num1.Location = new System.Drawing.Point(82, 98);
            this.txt_num1.Name = "txt_num1";
            this.txt_num1.Size = new System.Drawing.Size(173, 21);
            this.txt_num1.TabIndex = 17;
            // 
            // lab_num1
            // 
            this.lab_num1.AutoSize = true;
            this.lab_num1.Location = new System.Drawing.Point(36, 101);
            this.lab_num1.Name = "lab_num1";
            this.lab_num1.Size = new System.Drawing.Size(47, 12);
            this.lab_num1.TabIndex = 16;
            this.lab_num1.Text = "数字1：";
            // 
            // lab_count
            // 
            this.lab_count.AutoSize = true;
            this.lab_count.Location = new System.Drawing.Point(36, 46);
            this.lab_count.Name = "lab_count";
            this.lab_count.Size = new System.Drawing.Size(41, 12);
            this.lab_count.TabIndex = 15;
            this.lab_count.Text = "注数：";
            // 
            // txt_times
            // 
            this.txt_times.Location = new System.Drawing.Point(82, 70);
            this.txt_times.Name = "txt_times";
            this.txt_times.Size = new System.Drawing.Size(173, 21);
            this.txt_times.TabIndex = 14;
            // 
            // lab_times
            // 
            this.lab_times.AutoSize = true;
            this.lab_times.Location = new System.Drawing.Point(36, 74);
            this.lab_times.Name = "lab_times";
            this.lab_times.Size = new System.Drawing.Size(41, 12);
            this.lab_times.TabIndex = 13;
            this.lab_times.Text = "倍数：";
            // 
            // txt_count
            // 
            this.txt_count.Location = new System.Drawing.Point(82, 42);
            this.txt_count.Name = "txt_count";
            this.txt_count.Size = new System.Drawing.Size(173, 21);
            this.txt_count.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(82, 156);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // JiXuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 196);
            this.Controls.Add(this.txt_num2);
            this.Controls.Add(this.lab_num2);
            this.Controls.Add(this.txt_num1);
            this.Controls.Add(this.lab_num1);
            this.Controls.Add(this.lab_count);
            this.Controls.Add(this.txt_times);
            this.Controls.Add(this.lab_times);
            this.Controls.Add(this.txt_count);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "JiXuan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "机选配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_num2;
        private System.Windows.Forms.Label lab_num2;
        private System.Windows.Forms.TextBox txt_num1;
        private System.Windows.Forms.Label lab_num1;
        private System.Windows.Forms.Label lab_count;
        private System.Windows.Forms.TextBox txt_times;
        private System.Windows.Forms.Label lab_times;
        private System.Windows.Forms.TextBox txt_count;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}