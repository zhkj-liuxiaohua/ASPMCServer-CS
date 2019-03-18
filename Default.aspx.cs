/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2019/3/16
 * 时间: 17:32
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ASPMCServer
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class Default : Page
	{	
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Data

//		protected HtmlInputText fname;
//		protected HtmlInputPassword fpass;
//		protected	HtmlInputButton		submit1;
//		protected HtmlGenericControl msg;
		
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Init & Exit (Open/Close DB connections here...)

		protected void PageInit(object sender, EventArgs e)
		{
		}
		//----------------------------------------------------------------------
		protected void PageExit(object sender, EventArgs e)
		{
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Load
		private void Page_Load(object sender, EventArgs e)
		{
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		void showTips(string s) {
			msg.InnerHtml = s;
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
		#region Click_Button_OK
		//----------------------------------------------------------------------
		protected void Click_Button_Ok(object sender, EventArgs e)
		{
			string DATA_DIR = System.Web.Configuration.WebConfigurationManager.AppSettings["DATA_DIR"];
			if (fname.Value == null || fname.Value.Equals("")) {
				showTips("账号密码不能为空");
				return;
			}
			string fi = DATA_DIR + fname.Value + ".txt";
			if (!File.Exists(fi)) {
				showTips("不存在此用户名或账号密码错误");
				return;
			}
			String p = File.ReadAllText(fi);
			p = decode(p);
			if (!p.Equals(fpass.Value)) {
				showTips("用户名或密码错误");
				return;
			}
			// 此处执行跳转
			Session.Add("user", fname.Value);
			Session.Add("password", fpass.Value);
			Session.Add("flag", "通过");
			base.Response.Redirect("mccontrol.aspx", true);
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Initialize Component

		protected override void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		//----------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.Load	+= new System.EventHandler(Page_Load);
			this.Init   += new System.EventHandler(PageInit);
			this.Unload += new System.EventHandler(PageExit);
			submit1.ServerClick	 += new EventHandler(Click_Button_Ok);
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	}
}