/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2019/6/7
 * 时间: 0:27
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ASPMCServer
{
	/// <summary>
	/// Description of event
	/// </summary>
	public class Events : Page
	{	
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Data

		protected HtmlTextArea msg;
		
		public static string EVENT_FILE_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["EVENTPATH"];
		
		// 检测服务端是否保存了用户名
		private bool sessionCheck() {
			object u = Session["flag"];
			if (u != null) {
				if (u.ToString() != null && u.ToString().Equals("通过"))
					return true;
			}
			return false;
		}
		
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
			if (!sessionCheck()) {
				base.Response.Redirect("unlogin.html", true);
			}
			string str = "";
			try {
				str = File.ReadAllText(EVENT_FILE_PATH);
			}catch{
			}
			msg.Value = str;
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Add more events here...

		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
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
			// TODO 添加按钮信息
			//------------------------------------------------------------------
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	}
}
