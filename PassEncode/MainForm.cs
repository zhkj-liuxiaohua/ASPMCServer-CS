/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2019/3/18
 * 时间: 9:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PassEncode
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private string decode(string p)
		{
			try {
			byte [] a = new byte[p.Length / 2];
			string sub = null;
			for (int i = 0, l = p.Length; i < l; i+=2) {
				sub = p.Substring(i, 2);
				a[i/2] = (byte)(Convert.ToByte(sub, 16) ^ 0x22);
			}
			return Encoding.Default.GetString(a);
			} catch (Exception e) {
				return e.Message;
			}
		}
		private string encode(string p) {
			byte [] a = Encoding.Default.GetBytes(p);
			string s = "";
			for (int i = 0, l = a.Length; i < l; i++) {
				a[i] ^= 0x22;
				s += a[i].ToString("X2");
			}
			return s;
		}
		void Label1Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
		}
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			filepathtxt.Text = openFileDialog1.FileName;
		}
		void BtshowfileClick(object sender, EventArgs e)
		{
			if (filepathtxt.Text != null && File.Exists(filepathtxt.Text)) {
				string s = File.ReadAllText(filepathtxt.Text);
				content.Text = s;
			}
		}
		void BtencodeClick(object sender, EventArgs e)
		{
			content.Text = encode(content.Text);
		}
		void BtdecodeClick(object sender, EventArgs e)
		{
			content.Text = decode(content.Text);
		}
		
	}
}
