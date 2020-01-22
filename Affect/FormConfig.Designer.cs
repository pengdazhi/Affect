namespace Affect
{
    partial class FormConfig
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
            this.saveBtn = new System.Windows.Forms.Button();
            this.cancleBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.scanCmb = new System.Windows.Forms.ComboBox();
            this.IsAutoCbx = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.urlTxt = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dpiCmb = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.saveDayTxt = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(61, 337);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 9;
            this.saveBtn.Text = "确定";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancleBtn
            // 
            this.cancleBtn.Location = new System.Drawing.Point(191, 337);
            this.cancleBtn.Name = "cancleBtn";
            this.cancleBtn.Size = new System.Drawing.Size(75, 23);
            this.cancleBtn.TabIndex = 10;
            this.cancleBtn.Text = "取消";
            this.cancleBtn.UseVisualStyleBackColor = true;
            this.cancleBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(323, 330);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.scanCmb);
            this.tabPage1.Controls.Add(this.IsAutoCbx);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.urlTxt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 304);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " 基本设置 ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择扫描源：";
            // 
            // scanCmb
            // 
            this.scanCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scanCmb.FormattingEnabled = true;
            this.scanCmb.Location = new System.Drawing.Point(140, 31);
            this.scanCmb.Name = "scanCmb";
            this.scanCmb.Size = new System.Drawing.Size(121, 20);
            this.scanCmb.TabIndex = 1;
            this.scanCmb.SelectedIndexChanged += new System.EventHandler(this.scanCmb_SelectedIndexChanged);
            // 
            // IsAutoCbx
            // 
            this.IsAutoCbx.AutoSize = true;
            this.IsAutoCbx.Location = new System.Drawing.Point(56, 164);
            this.IsAutoCbx.Name = "IsAutoCbx";
            this.IsAutoCbx.Size = new System.Drawing.Size(72, 16);
            this.IsAutoCbx.TabIndex = 8;
            this.IsAutoCbx.Text = "自动登录";
            this.IsAutoCbx.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(57, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "管理网站：";
            // 
            // urlTxt
            // 
            this.urlTxt.Location = new System.Drawing.Point(56, 117);
            this.urlTxt.Name = "urlTxt";
            this.urlTxt.Size = new System.Drawing.Size(204, 21);
            this.urlTxt.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dpiCmb);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.saveDayTxt);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(315, 304);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " 高级设置 ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dpiCmb
            // 
            this.dpiCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dpiCmb.FormattingEnabled = true;
            this.dpiCmb.Items.AddRange(new object[] {
            "300",
            "200",
            "150",
            "96",
            "75"});
            this.dpiCmb.Location = new System.Drawing.Point(128, 20);
            this.dpiCmb.Name = "dpiCmb";
            this.dpiCmb.Size = new System.Drawing.Size(101, 20);
            this.dpiCmb.TabIndex = 20;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(235, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 12);
            this.label16.TabIndex = 19;
            this.label16.Text = "DPI";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(240, 68);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 18;
            this.label15.Text = "天";
            // 
            // saveDayTxt
            // 
            this.saveDayTxt.Location = new System.Drawing.Point(128, 61);
            this.saveDayTxt.Name = "saveDayTxt";
            this.saveDayTxt.Size = new System.Drawing.Size(101, 21);
            this.saveDayTxt.TabIndex = 17;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 68);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 12);
            this.label14.TabIndex = 16;
            this.label14.Text = "保留日志文件：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 11;
            this.label13.Text = "扫描图片清晰度：";
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 368);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancleBtn);
            this.Controls.Add(this.saveBtn);
            this.MaximizeBox = false;
            this.Name = "FormConfig";
            this.ShowIcon = false;
            this.Text = "设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormConfig_FormClosed);
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button cancleBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox saveDayTxt;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox dpiCmb;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox scanCmb;
        private System.Windows.Forms.CheckBox IsAutoCbx;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox urlTxt;
    }
}