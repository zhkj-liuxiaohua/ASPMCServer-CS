/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2019/3/17
 * 时间: 0:15
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ASPMCServer
{
	/// <summary>
	/// Description of editpass
	/// </summary>
	public partial class editpass : Page
	{	
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Data

//		protected	HtmlInputButton		submit;
//		protected	HtmlInputText		username;
//		protected HtmlInputPassword oldpass, newpass, newpass2;
//		protected HtmlGenericControl msg;
		
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Init & Exit (Open/Close DB connections here...)

		protected void PageInit(object sender, System.EventArgs e)
		{
		}
		//----------------------------------------------------------------------
		protected void PageExit(object sender, System.EventArgs e)
		{
		}

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Load
		private void Page_Load(object sender, System.EventArgs e)
		{
			//Response.Write(@"Hello #Develop<br>");
			//------------------------------------------------------------------
			//if(IsPostBack)
			//{
			//}
			//------------------------------------------------------------------
		}
		#endregion
		
		private void showTips(string s) {
			msg.InnerHtml = s;
		}

		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Click_Button_OK
		//----------------------------------------------------------------------
		protected void Click_Button_Ok(object sender, System.EventArgs e)
		{
			// 此处添加事件
			const string DATA_DIR = @"C:\Media\db\";
			string uname = username.Value;
			string oldp = oldpass.Value;
			string np = newpass.Value;
			string np2 = newpass2.Value;
			if (String.IsNullOrEmpty(uname) || String.IsNullOrEmpty(oldp)) {
				showTips("用户名或密码为空");
				return;
			}
			if (String.IsNullOrEmpty(np)) {
				showTips("新密码不能为空");
				return;
			}
			if (!np.Equals(np2)) {
				showTips("密码再确认不一致");
				return;
			}
			string fi = DATA_DIR + uname + ".txt";
			if (!File.Exists(fi)) {
				showTips("用户名不存在或密码错误");
				return;
			}
			string p = File.ReadAllText(fi);
			if (!oldp.Equals(p)) {
				showTips("用户名不存在或密码错误");
				return;
			}
			File.WriteAllText(fi, np);
			showTips("密码已修改，请返回<a href=\"Default.aspx\">主页</a>");
		}

		#endregion

		#region Initialize Component

		protected override void OnInit(EventArgs e)
		{	InitializeComponent();
			base.OnInit(e);
		}
		//----------------------------------------------------------------------
		private void InitializeComponent()
		{	//------------------------------------------------------------------
			this.Load	+= new System.EventHandler(Page_Load);
			this.Init   += new System.EventHandler(PageInit);
			this.Unload += new System.EventHandler(PageExit);
			//------------------------------------------------------------------
			submit.ServerClick	 += new EventHandler(Click_Button_Ok);
		}
		#endregion
	}
}
