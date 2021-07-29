
namespace SevenNiu
{
    partial class frmMain
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblMin = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblCannel = new System.Windows.Forms.Label();
            this.plTitle = new System.Windows.Forms.Panel();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flMain = new System.Windows.Forms.FlowLayoutPanel();
            this.plTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMin
            // 
            this.lblMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMin.BackColor = System.Drawing.Color.Transparent;
            this.lblMin.Location = new System.Drawing.Point(665, 0);
            this.lblMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(45, 30);
            this.lblMin.TabIndex = 4;
            this.lblMin.Text = "—";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblMin, "最小化");
            this.lblMin.Click += new System.EventHandler(this.lblMin_Click);
            this.lblMin.MouseEnter += new System.EventHandler(this.lblMin_MouseEnter);
            this.lblMin.MouseLeave += new System.EventHandler(this.lblMin_MouseLeave);
            // 
            // lblMax
            // 
            this.lblMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMax.BackColor = System.Drawing.Color.Transparent;
            this.lblMax.Location = new System.Drawing.Point(710, 0);
            this.lblMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(45, 30);
            this.lblMax.TabIndex = 5;
            this.lblMax.Text = "□";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblMax, "最大化");
            this.lblMax.Click += new System.EventHandler(this.lblMax_Click);
            this.lblMax.MouseEnter += new System.EventHandler(this.lblMax_MouseEnter);
            this.lblMax.MouseLeave += new System.EventHandler(this.lblMax_MouseLeave);
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Location = new System.Drawing.Point(755, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(45, 30);
            this.lblClose.TabIndex = 6;
            this.lblClose.Text = "×";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblClose, "关闭");
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseEnter += new System.EventHandler(this.lblClose_MouseEnter);
            this.lblClose.MouseLeave += new System.EventHandler(this.lblClose_MouseLeave);
            // 
            // lblCannel
            // 
            this.lblCannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCannel.BackColor = System.Drawing.Color.Transparent;
            this.lblCannel.Location = new System.Drawing.Point(620, 0);
            this.lblCannel.Margin = new System.Windows.Forms.Padding(0);
            this.lblCannel.Name = "lblCannel";
            this.lblCannel.Size = new System.Drawing.Size(45, 30);
            this.lblCannel.TabIndex = 7;
            this.lblCannel.Text = "←";
            this.lblCannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblCannel, "返回");
            this.lblCannel.Click += new System.EventHandler(this.lblCannel_Click);
            this.lblCannel.MouseEnter += new System.EventHandler(this.lblCannel_MouseEnter);
            this.lblCannel.MouseLeave += new System.EventHandler(this.lblCannel_MouseLeave);
            // 
            // plTitle
            // 
            this.plTitle.BackColor = System.Drawing.Color.Transparent;
            this.plTitle.Controls.Add(this.lblPath);
            this.plTitle.Controls.Add(this.lblCannel);
            this.plTitle.Controls.Add(this.lblClose);
            this.plTitle.Controls.Add(this.lblMax);
            this.plTitle.Controls.Add(this.lblMin);
            this.plTitle.Controls.Add(this.lblTitle);
            this.plTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plTitle.Location = new System.Drawing.Point(3, 0);
            this.plTitle.Margin = new System.Windows.Forms.Padding(0);
            this.plTitle.Name = "plTitle";
            this.plTitle.Size = new System.Drawing.Size(800, 523);
            this.plTitle.TabIndex = 2;
            this.plTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plTitle_MouseDown);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(122, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(0, 12);
            this.lblPath.TabIndex = 8;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 12);
            this.lblTitle.TabIndex = 0;
            // 
            // flMain
            // 
            this.flMain.AllowDrop = true;
            this.flMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flMain.Location = new System.Drawing.Point(3, 30);
            this.flMain.Margin = new System.Windows.Forms.Padding(0);
            this.flMain.Name = "flMain";
            this.flMain.Size = new System.Drawing.Size(800, 493);
            this.flMain.TabIndex = 3;
            this.flMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.flMain_DragDrop);
            this.flMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.flMain_DragEnter);
            this.flMain.DragLeave += new System.EventHandler(this.flMain_DragLeave);
            this.flMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.flMain_MouseDown);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(806, 526);
            this.Controls.Add(this.flMain);
            this.Controls.Add(this.plTitle);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(806, 523);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "七牛云客户端";
            this.TransparencyKey = System.Drawing.SystemColors.Desktop;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.TextChanged += new System.EventHandler(this.frmMain_TextChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.Move += new System.EventHandler(this.frmMain_Move);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.plTitle.ResumeLayout(false);
            this.plTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel plTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.FlowLayoutPanel flMain;
        private System.Windows.Forms.Label lblCannel;
        private System.Windows.Forms.Label lblPath;
    }
}

