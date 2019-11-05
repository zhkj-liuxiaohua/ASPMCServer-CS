/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/9/10
 * 时间: 23:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace EventSearch
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.DateTimePicker dtStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox grp1;
		private System.Windows.Forms.DateTimePicker dtEnd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox grp2;
		private System.Windows.Forms.TextBox tby2;
		private System.Windows.Forms.TextBox tby1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbx1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbdimen;
		private System.Windows.Forms.CheckBox cbdeath;
		private System.Windows.Forms.CheckBox cbevent;
		private System.Windows.Forms.Button btsearch;
		private System.Windows.Forms.TextBox tbResult;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox tbxboxid;
		private System.Windows.Forms.CheckBox cbdt;
		private System.Windows.Forms.CheckBox cbz;
		private System.Windows.Forms.CheckBox cby;
		private System.Windows.Forms.CheckBox cbx;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbz2;
		private System.Windows.Forms.TextBox tbz1;
		private System.Windows.Forms.TextBox tbx2;
		private System.Windows.Forms.CheckBox cbxbox;
		private System.Windows.Forms.CheckBox cbchat;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.dtStart = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.grp1 = new System.Windows.Forms.GroupBox();
			this.cbdt = new System.Windows.Forms.CheckBox();
			this.dtEnd = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.grp2 = new System.Windows.Forms.GroupBox();
			this.cbz = new System.Windows.Forms.CheckBox();
			this.cby = new System.Windows.Forms.CheckBox();
			this.cbx = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.tbz2 = new System.Windows.Forms.TextBox();
			this.tbz1 = new System.Windows.Forms.TextBox();
			this.tby2 = new System.Windows.Forms.TextBox();
			this.tby1 = new System.Windows.Forms.TextBox();
			this.tbx2 = new System.Windows.Forms.TextBox();
			this.tbx1 = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbdimen = new System.Windows.Forms.CheckBox();
			this.cbdeath = new System.Windows.Forms.CheckBox();
			this.cbevent = new System.Windows.Forms.CheckBox();
			this.btsearch = new System.Windows.Forms.Button();
			this.tbResult = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbxbox = new System.Windows.Forms.CheckBox();
			this.tbxboxid = new System.Windows.Forms.TextBox();
			this.cbchat = new System.Windows.Forms.CheckBox();
			this.grp1.SuspendLayout();
			this.grp2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dtStart
			// 
			this.dtStart.Enabled = false;
			this.dtStart.Location = new System.Drawing.Point(162, 23);
			this.dtStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.dtStart.Name = "dtStart";
			this.dtStart.Size = new System.Drawing.Size(239, 25);
			this.dtStart.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(527, 8);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(261, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = "梦故日志查询工具";
			// 
			// grp1
			// 
			this.grp1.Controls.Add(this.cbdt);
			this.grp1.Controls.Add(this.dtEnd);
			this.grp1.Controls.Add(this.label2);
			this.grp1.Controls.Add(this.dtStart);
			this.grp1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.grp1.Location = new System.Drawing.Point(22, 50);
			this.grp1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.grp1.Name = "grp1";
			this.grp1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.grp1.Size = new System.Drawing.Size(1292, 63);
			this.grp1.TabIndex = 4;
			this.grp1.TabStop = false;
			this.grp1.Text = "时间段查询范围";
			// 
			// cbdt
			// 
			this.cbdt.AutoSize = true;
			this.cbdt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbdt.Location = new System.Drawing.Point(37, 30);
			this.cbdt.Name = "cbdt";
			this.cbdt.Size = new System.Drawing.Size(109, 19);
			this.cbdt.TabIndex = 6;
			this.cbdt.Text = "限定时间段";
			this.cbdt.UseVisualStyleBackColor = true;
			this.cbdt.CheckedChanged += new System.EventHandler(this.CbdtCheckedChanged);
			// 
			// dtEnd
			// 
			this.dtEnd.Enabled = false;
			this.dtEnd.Location = new System.Drawing.Point(507, 23);
			this.dtEnd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.dtEnd.Name = "dtEnd";
			this.dtEnd.Size = new System.Drawing.Size(247, 25);
			this.dtEnd.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label2.Location = new System.Drawing.Point(448, 30);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "至";
			// 
			// grp2
			// 
			this.grp2.Controls.Add(this.cbz);
			this.grp2.Controls.Add(this.cby);
			this.grp2.Controls.Add(this.cbx);
			this.grp2.Controls.Add(this.label4);
			this.grp2.Controls.Add(this.label3);
			this.grp2.Controls.Add(this.label7);
			this.grp2.Controls.Add(this.tbz2);
			this.grp2.Controls.Add(this.tbz1);
			this.grp2.Controls.Add(this.tby2);
			this.grp2.Controls.Add(this.tby1);
			this.grp2.Controls.Add(this.tbx2);
			this.grp2.Controls.Add(this.tbx1);
			this.grp2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.grp2.Location = new System.Drawing.Point(22, 118);
			this.grp2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.grp2.Name = "grp2";
			this.grp2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.grp2.Size = new System.Drawing.Size(1292, 70);
			this.grp2.TabIndex = 5;
			this.grp2.TabStop = false;
			this.grp2.Text = "事件坐标范围";
			// 
			// cbz
			// 
			this.cbz.AutoSize = true;
			this.cbz.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbz.Location = new System.Drawing.Point(743, 37);
			this.cbz.Name = "cbz";
			this.cbz.Size = new System.Drawing.Size(54, 19);
			this.cbz.TabIndex = 3;
			this.cbz.Text = "Z轴";
			this.cbz.UseVisualStyleBackColor = true;
			this.cbz.CheckedChanged += new System.EventHandler(this.CbzCheckedChanged);
			// 
			// cby
			// 
			this.cby.AutoSize = true;
			this.cby.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cby.Location = new System.Drawing.Point(385, 37);
			this.cby.Name = "cby";
			this.cby.Size = new System.Drawing.Size(54, 19);
			this.cby.TabIndex = 3;
			this.cby.Text = "Y轴";
			this.cby.UseVisualStyleBackColor = true;
			this.cby.CheckedChanged += new System.EventHandler(this.CbyCheckedChanged);
			// 
			// cbx
			// 
			this.cbx.AutoSize = true;
			this.cbx.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbx.Location = new System.Drawing.Point(37, 37);
			this.cbx.Name = "cbx";
			this.cbx.Size = new System.Drawing.Size(54, 19);
			this.cbx.TabIndex = 3;
			this.cbx.Text = "X轴";
			this.cbx.UseVisualStyleBackColor = true;
			this.cbx.CheckedChanged += new System.EventHandler(this.CbxCheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(905, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(15, 15);
			this.label4.TabIndex = 2;
			this.label4.Text = "~";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(547, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(15, 15);
			this.label3.TabIndex = 2;
			this.label3.Text = "~";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(198, 42);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(15, 15);
			this.label7.TabIndex = 2;
			this.label7.Text = "~";
			// 
			// tbz2
			// 
			this.tbz2.Enabled = false;
			this.tbz2.Location = new System.Drawing.Point(925, 33);
			this.tbz2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbz2.Name = "tbz2";
			this.tbz2.Size = new System.Drawing.Size(91, 25);
			this.tbz2.TabIndex = 1;
			// 
			// tbz1
			// 
			this.tbz1.Enabled = false;
			this.tbz1.Location = new System.Drawing.Point(807, 33);
			this.tbz1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbz1.Name = "tbz1";
			this.tbz1.Size = new System.Drawing.Size(91, 25);
			this.tbz1.TabIndex = 1;
			// 
			// tby2
			// 
			this.tby2.Enabled = false;
			this.tby2.Location = new System.Drawing.Point(567, 33);
			this.tby2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tby2.Name = "tby2";
			this.tby2.Size = new System.Drawing.Size(91, 25);
			this.tby2.TabIndex = 1;
			// 
			// tby1
			// 
			this.tby1.Enabled = false;
			this.tby1.Location = new System.Drawing.Point(448, 33);
			this.tby1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tby1.Name = "tby1";
			this.tby1.Size = new System.Drawing.Size(91, 25);
			this.tby1.TabIndex = 1;
			// 
			// tbx2
			// 
			this.tbx2.Enabled = false;
			this.tbx2.Location = new System.Drawing.Point(218, 33);
			this.tbx2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbx2.Name = "tbx2";
			this.tbx2.Size = new System.Drawing.Size(91, 25);
			this.tbx2.TabIndex = 1;
			// 
			// tbx1
			// 
			this.tbx1.Enabled = false;
			this.tbx1.Location = new System.Drawing.Point(102, 33);
			this.tbx1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbx1.Name = "tbx1";
			this.tbx1.Size = new System.Drawing.Size(91, 25);
			this.tbx1.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbchat);
			this.groupBox1.Controls.Add(this.cbdimen);
			this.groupBox1.Controls.Add(this.cbdeath);
			this.groupBox1.Controls.Add(this.cbevent);
			this.groupBox1.Location = new System.Drawing.Point(22, 193);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Size = new System.Drawing.Size(1292, 62);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "事件类型";
			// 
			// cbdimen
			// 
			this.cbdimen.AutoSize = true;
			this.cbdimen.Checked = true;
			this.cbdimen.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbdimen.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbdimen.Location = new System.Drawing.Point(278, 33);
			this.cbdimen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.cbdimen.Name = "cbdimen";
			this.cbdimen.Size = new System.Drawing.Size(110, 19);
			this.cbdimen.TabIndex = 1;
			this.cbdimen.Text = "Dimension";
			this.cbdimen.UseVisualStyleBackColor = true;
			// 
			// cbdeath
			// 
			this.cbdeath.AutoSize = true;
			this.cbdeath.Checked = true;
			this.cbdeath.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbdeath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbdeath.Location = new System.Drawing.Point(140, 33);
			this.cbdeath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.cbdeath.Name = "cbdeath";
			this.cbdeath.Size = new System.Drawing.Size(110, 19);
			this.cbdeath.TabIndex = 1;
			this.cbdeath.Text = "DeathInfo";
			this.cbdeath.UseVisualStyleBackColor = true;
			// 
			// cbevent
			// 
			this.cbevent.AutoSize = true;
			this.cbevent.Checked = true;
			this.cbevent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbevent.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbevent.Location = new System.Drawing.Point(37, 33);
			this.cbevent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.cbevent.Name = "cbevent";
			this.cbevent.Size = new System.Drawing.Size(74, 19);
			this.cbevent.TabIndex = 0;
			this.cbevent.Text = "Event";
			this.cbevent.UseVisualStyleBackColor = true;
			// 
			// btsearch
			// 
			this.btsearch.Enabled = false;
			this.btsearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.btsearch.Location = new System.Drawing.Point(542, 332);
			this.btsearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btsearch.Name = "btsearch";
			this.btsearch.Size = new System.Drawing.Size(188, 32);
			this.btsearch.TabIndex = 7;
			this.btsearch.Text = "正在装入数据……";
			this.btsearch.UseVisualStyleBackColor = true;
			this.btsearch.Click += new System.EventHandler(this.BtsearchClick);
			// 
			// tbResult
			// 
			this.tbResult.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tbResult.Location = new System.Drawing.Point(0, 376);
			this.tbResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbResult.MaxLength = 999999;
			this.tbResult.Multiline = true;
			this.tbResult.Name = "tbResult";
			this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbResult.Size = new System.Drawing.Size(1347, 296);
			this.tbResult.TabIndex = 8;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbxbox);
			this.groupBox2.Controls.Add(this.tbxboxid);
			this.groupBox2.Location = new System.Drawing.Point(22, 260);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Size = new System.Drawing.Size(1292, 67);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "玩家信息";
			// 
			// cbxbox
			// 
			this.cbxbox.AutoSize = true;
			this.cbxbox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbxbox.Location = new System.Drawing.Point(37, 33);
			this.cbxbox.Name = "cbxbox";
			this.cbxbox.Size = new System.Drawing.Size(83, 19);
			this.cbxbox.TabIndex = 2;
			this.cbxbox.Text = "xboxID";
			this.cbxbox.UseVisualStyleBackColor = true;
			this.cbxbox.CheckedChanged += new System.EventHandler(this.CbxboxCheckedChanged);
			// 
			// tbxboxid
			// 
			this.tbxboxid.Enabled = false;
			this.tbxboxid.Location = new System.Drawing.Point(127, 32);
			this.tbxboxid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tbxboxid.Name = "tbxboxid";
			this.tbxboxid.Size = new System.Drawing.Size(1117, 25);
			this.tbxboxid.TabIndex = 1;
			// 
			// cbchat
			// 
			this.cbchat.AutoSize = true;
			this.cbchat.Checked = true;
			this.cbchat.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbchat.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbchat.Location = new System.Drawing.Point(417, 33);
			this.cbchat.Margin = new System.Windows.Forms.Padding(2);
			this.cbchat.Name = "cbchat";
			this.cbchat.Size = new System.Drawing.Size(65, 19);
			this.cbchat.TabIndex = 1;
			this.cbchat.Text = "Chat";
			this.cbchat.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1347, 672);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.tbResult);
			this.Controls.Add(this.btsearch);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grp2);
			this.Controls.Add(this.grp1);
			this.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "MainForm";
			this.Text = "日志查询";
			this.grp1.ResumeLayout(false);
			this.grp1.PerformLayout();
			this.grp2.ResumeLayout(false);
			this.grp2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
