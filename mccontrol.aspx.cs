/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2019/3/16
 * 时间: 19:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ASPMCServer
{
	/// <summary>
	/// Description of mccontrol
	/// </summary>
	public partial class mccontrol : Page
	{
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region 预定义按钮处，此处不用开放

//		protected	HtmlInputButton		logout, showbackup, clearbackup, cpmap;
//		protected Button btwhite, btblack, showmc, btcmd, shutdown, StartServer;
//		protected	TextBox		whitetext, blacktext, cmdtext;
//		protected HtmlGenericControl msg, welcome;
		
		public string BACKUPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["BACKUPDIR"];
		public string MAPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["MAPDIR"];
		public string FTPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPDIR"];
		public string PROCNAME = System.Web.Configuration.WebConfigurationManager.AppSettings["PROCNAME"];
		public string PROCPATH = System.Web.Configuration.WebConfigurationManager.AppSettings["PROCPATH"];
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Page Init & Exit (Open/Close DB connections here...)

		// 检测服务端是否保存了用户名
		private bool sessionCheck() {
			object u = Session["flag"];
			if (u != null) {
				if (u.ToString() != null && u.ToString().Equals("通过"))
					return true;
			}
			return false;
		}
		
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
			welcome.InnerHtml = "欢迎用户 " + Session["user"] + " 使用本系统！";
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region 各类按钮事件处理

		//----------------------------------------------------------------------
		// 退出登录
		protected void logout_Ok(object sender, System.EventArgs e)
		{
			Session.RemoveAll();
			base.Response.Redirect("Default.aspx", true);
		}

		// 显示备份目录
		protected void showbackupClick(object sender, System.EventArgs e)
		{
			if (!Directory.Exists(BACKUPDIR)) {
				secAddTxt("备份目录不存在或无权访问。");
				return;
			}
			string x = "";
			string [] ds = Directory.GetDirectories(BACKUPDIR);
			if (ds != null && ds.Length > 0) {
				foreach (string s in ds) {
					x += ("\t" + s);
				}
			}
			string [] fs = Directory.GetFiles(BACKUPDIR);
			if (fs != null && fs.Length > 0) {
				foreach (string s in fs) {
					x += ("\t" + s);
				}
			}
			if (!String.IsNullOrEmpty(x))
				secAddTxt(x);
		}
		
		// 清理备份目录至最近10个
		protected void clearbackupClick(object sender, System.EventArgs e)
		{
			
			if (!Directory.Exists(BACKUPDIR)) {
				secAddTxt("备份目录不存在或无权访问。");
				return;
			}
			string [] ds = Directory.GetDirectories(BACKUPDIR);
			if (ds != null && ds.Length > 10) {
				for (int i = 0; i < ds.Length - 10; i++) {
					if (Directory.Exists(ds[i])) {
						Directory.Delete(ds[i], true);
					}
				}
				secAddTxt("已清理。");
				return;
			}
			secAddTxt("文件夹还未超出指定个数。");
		}
		
		// 备份当前地图至ftp目录
		protected void cpmapClick(object sender, System.EventArgs e)
		{
			if (Directory.Exists(MAPDIR) && Directory.Exists(FTPDIR)) {
				DirectoryInfo i = new DirectoryInfo(MAPDIR);
				string ddir = FTPDIR + @"\" + i.Name;
				if (Directory.Exists(ddir)) {
					Directory.Delete(ddir, true);
				}
				secAddTxt("开始复制地图到ftp，详情请查看ftp路径。");
				system("xcopy " + "\"" + MAPDIR + "\"" + " \"" +
				       ddir + "\" /e/q/h/i");
			}
		}
		
		private void secAddTxt(string s) {
			string mt = msg.InnerHtml;
			mt += ("<br>" + s);
			msg.InnerHtml = MCWinControl.FormatStrAsLine(mt, 20);
		}
		// 显示后台
		void ShowmcClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.getStrFromProc(PROCNAME));
		}
		// 关服
		void ShutdownClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.closeProc(PROCNAME));
		}
		// 启动服务端
		void StartServerClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.StartProc(PROCNAME, PROCPATH));
		}
		// 发送消息
		void BtcmdClick(object sender, EventArgs e)
		{
			string cmd = cmdtext.Text;
			if (String.IsNullOrEmpty(cmd)) {
				secAddTxt("空消息不予发送");
				return;
			}
			cmdtext.Text = "";
			secAddTxt(MCWinControl.sendCommand(PROCNAME, cmd));
		}
		// 添加白名单
		void BtwhiteClick(object sender, EventArgs e)
		{
			string uname = whitetext.Text;
			if (String.IsNullOrEmpty(uname)) {
				secAddTxt("空用户名");
				return;
			}
			whitetext.Text = "";
			secAddTxt(MCWinControl.sendCommand(PROCNAME, "whitelist add \"" + uname + "\""));
		}
		// 移除白名单
		void BtblackClick(object sender, EventArgs e)
		{
			string uname = blacktext.Text;
			if (String.IsNullOrEmpty(uname)) {
				secAddTxt("空用户名");
				return;
			}
			blacktext.Text = "";
			secAddTxt(MCWinControl.sendCommand(PROCNAME, "whitelist remove \"" + uname + "\""));
		}
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
			logout.ServerClick	 += new EventHandler(logout_Ok);
			showbackup.ServerClick += showbackupClick;
			clearbackup.ServerClick += clearbackupClick;
			cpmap.ServerClick += cpmapClick;
			btwhite.Click += BtwhiteClick;
			btblack.Click += BtblackClick;
			showmc.Click += ShowmcClick;
			btcmd.Click += BtcmdClick;
			shutdown.Click += ShutdownClick;
			StartServer.Click += StartServerClick;
			//------------------------------------------------------------------
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		[DllImport("msvcrt.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public extern static void system(string command); // longjmp
	}
	
	/// <summary>
	/// MC主控功能
	/// </summary>
	class MCWinControl
	{
		public MCWinControl()
		{
		}

		private static Process myProcess = null;
		private static string procname = null;
		private static string procpath = null;
		private static string procstr = "";
		private static bool keeprun = false;

		/// <summary>
		/// 关闭指定进程
		/// </summary>
		/// <param name="procname">进程名</param>
		/// <returns>关闭信息</returns>
		public static string closeProc(string procname)
		{
			keeprun = false;
			System.Web.Configuration.WebConfigurationManager.AppSettings.Set("KEEPRUN", "0");
			Process [] ps = Process.GetProcessesByName(procname);
			if (ps != null && ps.Length > 0) {
				foreach (Process p in ps) {
					try {
						p.Kill();
					} catch (Exception e) {
						return "失败：" + e.Message;
					}
				}
				return "已关闭指定进程";
			}
			return "关闭指定进程失败";
		}

		/// <summary>
		/// 保留指定行数的信息
		/// </summary>
		/// <param name="mystr">原始字符串</param>
		/// <param name="linecount">指定行数</param>
		/// <returns>修整后的行数</returns>
		public static string FormatStrAsLine(string mystr, int linecount) {
			if (linecount < 1) {
				return mystr;
			}
			string [] strs = mystr.Split(new string [] {"<br>"}, StringSplitOptions.RemoveEmptyEntries);
			if (strs.Length < linecount) {
				return mystr;
			}
			mystr = "";
			for (int i = strs.Length - linecount; i < strs.Length; i++) {
				mystr += ("<br>" + strs[i]);
			}
			return mystr.Substring(mystr.IndexOf("<br>") + 4);
		}
		
		private static void OnDataReceived(object sender, DataReceivedEventArgs e) {
			procstr = procstr + "<br>" + e.Data;
			procstr = FormatStrAsLine(procstr, 2000); // 最多保留2000行log文本
			System.Web.Configuration.WebConfigurationManager.AppSettings.Set("LOG", procstr);
		}
		private static void startProcThread()
		{
			while (keeprun) {
				procstr = "";
				if (findedProcName(procname)) {
					// 已存在实例
					return;
				}
				myProcess = new Process();
				myProcess.StartInfo.FileName = procpath;//控制台程序的路径
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardOutput = true;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);
				myProcess.Start();
				myProcess.BeginOutputReadLine();
				myProcess.WaitForExit();
				myProcess.Close();
				myProcess = null;
				procstr = "";
				System.Web.Configuration.WebConfigurationManager.AppSettings.Set("LOG", "");
				keeprun = (System.Web.Configuration.WebConfigurationManager.AppSettings["KEEPRUN"] == "1");
				if (keeprun)
					Thread.Sleep(10000);
			}
		}
		private static bool findedProcName(string pname)
		{
			Process [] ps = Process.GetProcessesByName(pname);
			return (ps != null && ps.Length > 0);
		}
		/// <summary>
		/// 我要开服
		/// </summary>
		/// <param name="pname">服务端应用名称</param>
		/// <param name="fpath">服务端应用实际完整路径</param>
		/// <returns>开服信息</returns>
		public static string StartProc(string pname, string fpath)
		{
			if (myProcess != null) {
				if (!myProcess.HasExited)
					return "已启动一个实例";
			}
			if (findedProcName(pname)) {
				return "已启动一个实例";
			}
			procname = pname;
			procpath = fpath;
			keeprun = true;
			System.Web.Configuration.WebConfigurationManager.AppSettings.Set("KEEPRUN", "1");
			Thread t = new Thread(startProcThread);
			t.Start();
            return "尝试开服，请使用log查看信息";
		}
		
		/// <summary>
		/// 获取后台信息
		/// </summary>
		/// <param name="procname">进程名</param>
		/// <returns>信息(含错误信息)</returns>
		public static string getStrFromProc(string procname) {
			if (myProcess == null) {
				Process [] ps = Process.GetProcessesByName(procname);
				if (ps != null && ps.Length > 0) {
					// 线程已被外部接管
					string s = "线程已托管，将尝试从log信息获取<br>";
					s += System.Web.Configuration.WebConfigurationManager.AppSettings["LOG"];
					return s;
				}
			} else {
				return procstr;
			}
			return "未找到指定应用程序";
		}
		
		/// <summary>
		/// 发送指定消息
		/// </summary>
		/// <param name="pname">进程名</param>
		/// <param name="cmd">消息</param>
		/// <returns>消息反馈</returns>
		public static string sendCommand(string pname, string cmd) {
			if (myProcess != null) {
				if (!myProcess.HasExited) {
					myProcess.StandardInput.WriteLine(cmd);
				}
			} else {
				// 进程发送消息
				if (cmd.IndexOf("\n") < 0) {
					cmd += "\n\r";
				}
				IntPtr hWnd = IntPtr.Zero;
				Process [] ps = Process.GetProcessesByName(pname);
				if (ps != null && ps.Length > 0) {
					hWnd = ps[0].MainWindowHandle;
				}
				if (hWnd != IntPtr.Zero) {
					char [] chs = cmd.ToCharArray();
					foreach(char c in chs) {
						SendMessage(hWnd, WM_CHAR, (int)c, 0);
					}
				} else {
					return "未找到对应进程";
				}
			}
			return "命令" + cmd + "已发送";
		}
		
		const int WM_KEYDOWN = 0x0100;
 		const int WM_KEYUP = 0x0101;
 		const int WM_CHAR = 0x0102;
 		
		[DllImport("user32.dll")]
 		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
 		[DllImport("user32.dll")]
 		public static extern IntPtr FindWindowEx(IntPtr hwndParent,IntPtr hwndChildAfter,string lpszClass,string lpszWindow);
		//消息发送API
         [DllImport("User32.dll", EntryPoint = "PostMessage")]
         public static extern int PostMessage(
             IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
             int wParam,         // 参数1
             int lParam            // 参数2
         );
         //消息发送API
         [DllImport("User32.dll", EntryPoint = "SendMessage")]
         public static extern int SendMessage(
             IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
             int wParam,         // 参数1
             int lParam          //参数2
         );
	}
	
}
