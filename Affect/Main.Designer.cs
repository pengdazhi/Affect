namespace Affect
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UpdateBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.AppendBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DelBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DelAllBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshTool = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.scanBtn = new System.Windows.Forms.ToolStripButton();
            this.subBtn = new System.Windows.Forms.ToolStripButton();
            this.setUpBtn = new System.Windows.Forms.ToolStripButton();
            this.freshUpBtn = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UpdateImgBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DelMainBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SubmitTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DelLiuNoTool = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddBarTool = new System.Windows.Forms.ToolStripMenuItem();
            this.DelImgTool = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.contextMenuStrip5.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateBarTool,
            this.AppendBarTool,
            this.DelBarTool,
            this.DelAllBarTool,
            this.RefreshTool});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 114);
            // 
            // UpdateBarTool
            // 
            this.UpdateBarTool.Name = "UpdateBarTool";
            this.UpdateBarTool.Size = new System.Drawing.Size(194, 22);
            this.UpdateBarTool.Text = "修改条码";
            this.UpdateBarTool.Click += new System.EventHandler(this.UpdateBarTool_Click);
            // 
            // AppendBarTool
            // 
            this.AppendBarTool.Name = "AppendBarTool";
            this.AppendBarTool.Size = new System.Drawing.Size(194, 22);
            this.AppendBarTool.Text = "追加扫描";
            this.AppendBarTool.Click += new System.EventHandler(this.AppendBarTool_Click);
            // 
            // DelBarTool
            // 
            this.DelBarTool.Name = "DelBarTool";
            this.DelBarTool.Size = new System.Drawing.Size(194, 22);
            this.DelBarTool.Text = "移除条码";
            this.DelBarTool.Click += new System.EventHandler(this.DelBarTool_Click);
            // 
            // DelAllBarTool
            // 
            this.DelAllBarTool.Name = "DelAllBarTool";
            this.DelAllBarTool.Size = new System.Drawing.Size(194, 22);
            this.DelAllBarTool.Text = "删除条码号下所有图片";
            this.DelAllBarTool.Click += new System.EventHandler(this.DelAllBarTool_Click);
            // 
            // RefreshTool
            // 
            this.RefreshTool.Name = "RefreshTool";
            this.RefreshTool.Size = new System.Drawing.Size(194, 22);
            this.RefreshTool.Text = "刷新";
            this.RefreshTool.Click += new System.EventHandler(this.RefreshTool_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Location = new System.Drawing.Point(2, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 655);
            this.panel1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(214, 655);
            this.treeView1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Location = new System.Drawing.Point(351, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 0);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Location = new System.Drawing.Point(223, 36);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1134, 655);
            this.panel3.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.Silver;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.RightToLeftLayout = true;
            this.listView1.Size = new System.Drawing.Size(1134, 655);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanBtn,
            this.subBtn,
            this.setUpBtn,
            this.freshUpBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1362, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // scanBtn
            // 
            this.scanBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scanBtn.Image = ((System.Drawing.Image)(resources.GetObject("scanBtn.Image")));
            this.scanBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.scanBtn.Size = new System.Drawing.Size(65, 22);
            this.scanBtn.Text = "扫描";
            this.scanBtn.Click += new System.EventHandler(this.scanBtn_Click);
            // 
            // subBtn
            // 
            this.subBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.subBtn.Image = ((System.Drawing.Image)(resources.GetObject("subBtn.Image")));
            this.subBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.subBtn.Name = "subBtn";
            this.subBtn.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.subBtn.Size = new System.Drawing.Size(89, 22);
            this.subBtn.Text = "提交识别";
            this.subBtn.Click += new System.EventHandler(this.subBtn_Click);
            // 
            // setUpBtn
            // 
            this.setUpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setUpBtn.Image = ((System.Drawing.Image)(resources.GetObject("setUpBtn.Image")));
            this.setUpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setUpBtn.Name = "setUpBtn";
            this.setUpBtn.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.setUpBtn.Size = new System.Drawing.Size(65, 22);
            this.setUpBtn.Text = "设置";
            this.setUpBtn.Click += new System.EventHandler(this.setUpBtn_Click);
            // 
            // freshUpBtn
            // 
            this.freshUpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.freshUpBtn.Image = ((System.Drawing.Image)(resources.GetObject("freshUpBtn.Image")));
            this.freshUpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.freshUpBtn.Name = "freshUpBtn";
            this.freshUpBtn.Size = new System.Drawing.Size(59, 22);
            this.freshUpBtn.Text = "重新提交";
            this.freshUpBtn.Click += new System.EventHandler(this.freshUpBtn_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateImgBarTool,
            this.DelMainBarTool,
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(123, 70);
            // 
            // UpdateImgBarTool
            // 
            this.UpdateImgBarTool.Name = "UpdateImgBarTool";
            this.UpdateImgBarTool.Size = new System.Drawing.Size(122, 22);
            this.UpdateImgBarTool.Text = "修改条码";
            this.UpdateImgBarTool.Click += new System.EventHandler(this.UpdateImgBarTool_Click);
            // 
            // DelMainBarTool
            // 
            this.DelMainBarTool.Name = "DelMainBarTool";
            this.DelMainBarTool.Size = new System.Drawing.Size(122, 22);
            this.DelMainBarTool.Text = "移除条码";
            this.DelMainBarTool.Click += new System.EventHandler(this.DelMainBarTool_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SubmitTool,
            this.DelLiuNoTool,
            this.刷新ToolStripMenuItem1});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(171, 70);
            // 
            // SubmitTool
            // 
            this.SubmitTool.Name = "SubmitTool";
            this.SubmitTool.Size = new System.Drawing.Size(170, 22);
            this.SubmitTool.Text = "提交识别";
            this.SubmitTool.Click += new System.EventHandler(this.subBtn_Click);
            // 
            // DelLiuNoTool
            // 
            this.DelLiuNoTool.Name = "DelLiuNoTool";
            this.DelLiuNoTool.Size = new System.Drawing.Size(170, 22);
            this.DelLiuNoTool.Text = "移除此扫描流水号";
            this.DelLiuNoTool.Click += new System.EventHandler(this.DelLiuNoTool_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBarTool,
            this.DelImgTool,
            this.刷新ToolStripMenuItem2});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(123, 70);
            // 
            // AddBarTool
            // 
            this.AddBarTool.Name = "AddBarTool";
            this.AddBarTool.Size = new System.Drawing.Size(122, 22);
            this.AddBarTool.Text = "新增条码";
            this.AddBarTool.Click += new System.EventHandler(this.AddBarTool_Click);
            // 
            // DelImgTool
            // 
            this.DelImgTool.Name = "DelImgTool";
            this.DelImgTool.Size = new System.Drawing.Size(122, 22);
            this.DelImgTool.Text = "删除图片";
            this.DelImgTool.Click += new System.EventHandler(this.DelImgTool_Click);
            // 
            // 刷新ToolStripMenuItem2
            // 
            this.刷新ToolStripMenuItem2.Name = "刷新ToolStripMenuItem2";
            this.刷新ToolStripMenuItem2.Size = new System.Drawing.Size(122, 22);
            this.刷新ToolStripMenuItem2.Text = "刷新";
            // 
            // pictureBox2
            // 
            this.pictureBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox2.Location = new System.Drawing.Point(2, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1920, 28);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 696);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 696);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(729, 696);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 8;
            // 
            // contextMenuStrip5
            // 
            this.contextMenuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStrip5.Name = "contextMenuStrip5";
            this.contextMenuStrip5.Size = new System.Drawing.Size(99, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.DelImgTool_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 741);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox2);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "影像系统V2.0.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip4.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.contextMenuStrip5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem UpdateBarTool;
        private System.Windows.Forms.ToolStripMenuItem AppendBarTool;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton scanBtn;
        private System.Windows.Forms.ToolStripButton subBtn;
        private System.Windows.Forms.ToolStripButton setUpBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem UpdateImgBarTool;
        private System.Windows.Forms.ToolStripMenuItem DelMainBarTool;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelBarTool;
        private System.Windows.Forms.ToolStripMenuItem DelAllBarTool;
        private System.Windows.Forms.ToolStripMenuItem RefreshTool;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem SubmitTool;
        private System.Windows.Forms.ToolStripMenuItem DelLiuNoTool;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem AddBarTool;
        private System.Windows.Forms.ToolStripMenuItem DelImgTool;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip5;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton freshUpBtn;
        private System.Windows.Forms.ImageList imageList;
    }
}

