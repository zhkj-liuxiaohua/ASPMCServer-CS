/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/9/10
 * 时间: 23:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace EventSearch
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		public static string dir = null;
		public static void setDir(string d) {
			dir = d;
		}
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			CheckForIllegalCrossThreadCalls = false;
			Thread t = new Thread(initData);
			t.Start();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public static EData edatas = null;
		
		private void initData() {
			edatas = new EData();
			edatas.setOnInitFinish(onInitFinish);
			edatas.init(dir);
		}
		
		// 结束时调用
		void onInitFinish() {
			btsearch.Text = "开始查询";
			btsearch.Enabled = true;
		}
		
		void CbdtCheckedChanged(object sender, EventArgs e)
		{
			dtStart.Enabled = cbdt.Checked;
			dtEnd.Enabled = cbdt.Checked;
		}
		void CbxCheckedChanged(object sender, EventArgs e)
		{
			tbx1.Enabled = cbx.Checked;
			tbx2.Enabled = cbx.Checked;
		}
		void CbyCheckedChanged(object sender, EventArgs e)
		{
			tby1.Enabled = cby.Checked;
			tby2.Enabled = cby.Checked;
		}
		void CbzCheckedChanged(object sender, EventArgs e)
		{
			tbz1.Enabled = cbz.Checked;
			tbz2.Enabled = cbz.Checked;
		}
		void CbxboxCheckedChanged(object sender, EventArgs e)
		{
			tbxboxid.Enabled = cbxbox.Checked;
		}
		
		private void swapInt(int [] ints) {
			int t = ints[0];
			ints[0] = ints[1];
			ints[1] = t;
		}

        private int numtest(string s)
        {
            return int.Parse(string.IsNullOrEmpty(s) ? "0" : s);
        }

		// 开始检查查询项
		void BtsearchClick(object sender, EventArgs e)
		{
			DateTime [] dts = null;
			if (cbdt.Checked) {
				// 添加时间段查询
				dts = new DateTime[2];
				dts[0] = dtStart.Value;
				dts[1] = dtEnd.Value;
				if (dts[0] > dts[1]) {
					DateTime t = dts[0];
					dts[0] = dts[1];
					dts[1] = t;
				}
			}
			int [] xs = null;
			if (cbx.Checked) {
				// 添加x轴查询
				xs = new int[2];
				xs[0] = numtest(tbx1.Text);
				xs[1] = numtest(tbx2.Text);
				if (xs[0] > xs[1]) {
					swapInt(xs);
				}
			}
			int [] ys = null;
			if (cby.Checked) {
				// 添加y轴查询
				ys = new int[2];
				ys[0] = numtest(tby1.Text);
				ys[1] = numtest(tby2.Text);
				if (ys[0] > ys[1]) {
					swapInt(ys);
				}
			}
			int [] zs = null;
			if (cbz.Checked) {
				// 添加z轴查询
				zs = new int[2];
				zs[0] = numtest(tbz1.Text);
				zs[1] = numtest(tbz2.Text);
				if (zs[0] > zs[1]) {
					swapInt(zs);
				}
			}
			ArrayList titles = new ArrayList();
			if (cbevent.Checked) {
				// 添加Event事件查询
				titles.Add(cbevent.Text);
			}
			if (cbdeath.Checked) {
				// 添加DeathInfo事件查询
				titles.Add(cbdeath.Text);
			}
			if (cbdimen.Checked) {
				// 添加Dimension事件查询
				titles.Add(cbdimen.Text);
			}
			if (cbchat.Checked) {
				// 添加Chat事件查询
				titles.Add(cbchat.Text);
			}
			string xboxid = null;
			if (cbxbox.Checked) {
				// 添加玩家xboxID查询
				xboxid = tbxboxid.Text.Trim();
			}
			tbResult.Text = "正在查询，请稍后。。。";
			btsearch.Enabled = false;
			searchMsg(dts, xs, ys, zs, titles, xboxid);
		}
		
		// 开始一个查询
		void searchMsg(DateTime [] dts, int [] xs, int [] ys, int [] zs, ArrayList titles, string xboxid) {
			if (edatas != null) {
				ArrayList strs = edatas.search(dts, xs, ys, zs, titles, xboxid);
				string str = "";
				if (strs != null && strs.Count > 0) {
					int startLine = strs.Count - 10000;						// 仅显示最近10000行结果
					startLine = (startLine < 0 ? 0 : startLine);
					for (int i = startLine, l = strs.Count; i < l; i++) {
						str += (strs[i] + "\r\n");
					}
				}
				tbResult.Text = str;
				btsearch.Enabled = true;
			}
		}
	}
}
