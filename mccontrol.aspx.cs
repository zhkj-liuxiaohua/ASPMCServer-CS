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
using System.Text;
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
		
		public static string TMPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["TMPDIR"];
		
		public const string KEEPRUN_FILE = @"\MKEEPRUN.tmp";
		public const string PROCSTR_FILE = @"\MPROCSTR.tmp";
		public const string INPUT_FILE = @"\MINPUT.tmp";
		public const string INPUTFLAG_FILE = @"\MINPUTFLAG.tmp";

		public const int INPUTCMD_END = 0;
		public const int INPUTCMD_RUN = 1;
		public const int INPUTCMD_SEND = 2;
		
		/// <summary>
		/// 文件读取一个字符串
		/// </summary>
		/// <param name="path">文件名</param>
		/// <returns>取值</returns>
		public static string filegetString(string path)
		{
			if (string.IsNullOrEmpty(path)) {
				return null;
			}
			string s = null;
			try {
				s = File.ReadAllText(path);
			} catch {
			}
			return s;
		}
		
		/// <summary>
		/// 文件设置一个字符串
		/// </summary>
		/// <param name="path">文件名</param>
		/// <param name="value">字符串值</param>
		public static void filesetString(string path, string value) {
			if (string.IsNullOrEmpty(path)) {
				return;
			}
			try {
				File.WriteAllText(path, value);
			} catch {
			}
		}
		
		// 硬存储运行状态
		public static bool KEEPRUN {
			get {
				return filegetString(TMPDIR + KEEPRUN_FILE).Equals("1");
			}
			set{
				string s = (value ? "1" : "0");
				filesetString(TMPDIR + KEEPRUN_FILE, s);
			}
		}

		// 硬存储log信息
		public static string PROCSTR {
			get {
				return filegetString(TMPDIR + PROCSTR_FILE);
			}
			set {
				filesetString(TMPDIR + PROCSTR_FILE, value);
			}
		}
		
		// 硬存储外部发送信号信息
		public static int INPUTFLAG {
			get {
				return Convert.ToInt32(filegetString(TMPDIR + INPUTFLAG_FILE));
			}
			set {
				filesetString(TMPDIR + INPUTFLAG_FILE, "" + value);
			}
		}
		// 硬存储外部输入流详细信息
		public static string INPUT {
			get {
				return filegetString(TMPDIR + INPUT_FILE);
			}
			set {
				filesetString(TMPDIR + INPUT_FILE, value);
			}
		}
		
		/// <summary>
		/// 关闭指定进程
		/// </summary>
		/// <param name="procname">进程名</param>
		/// <returns>关闭信息</returns>
		public static string closeProc(string procname)
		{
			keeprun = false;
			KEEPRUN = false;
			INPUTFLAG = 0;
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
			return "未能获取指定进程信息";
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

		// 外部输入流监听服务
		private static void startProcReadMsg() {
			string s = null;
			int flag = INPUTCMD_END;
			while (keeprun) {
				flag = INPUTFLAG;
				if (flag == INPUTCMD_SEND) {
					s = INPUT;
					INPUTFLAG = INPUTCMD_RUN;
					if (!string.IsNullOrEmpty(s)) {
						sendCommand(procname, s);
					}
				} else if (flag == INPUTCMD_RUN) {
					Thread.Sleep(500);	// 监听频率：0.5s
				} else {
					// 非发送、非运行的情况，退出执行
					break;
				}
			}
		}
		
		private static void OnDataReceived(object sender, DataReceivedEventArgs e) {
			procstr = procstr + "<br>" + e.Data;
			procstr = FormatStrAsLine(procstr, 2000); // 最多保留2000行log文本
			PROCSTR = procstr;
		}
		// 自动重启服务
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
				INPUTFLAG = INPUTCMD_END;
				myProcess.Close();
				myProcess = null;
				procstr = "";
				PROCSTR = "log end";
				keeprun = KEEPRUN;
				if (keeprun)
					for (int i = 0; i < 10 && keeprun; i++)
						Thread.Sleep(1000);
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
			// 共享内存自检
			KEEPRUN = true;
			bool krflag = KEEPRUN;
			if (!krflag) {
				return "运行状态标志设置已炸";
			}
			INPUTFLAG = INPUTCMD_RUN;
			int msgflag = INPUTFLAG;
			if (msgflag != INPUTCMD_RUN) {
				return "消息标志设置已炸";
			}
			PROCSTR = "A";
			string aproc = PROCSTR;
			if (!aproc.Equals("A")) {
				return "LOG信息设置已炸";
			}
			INPUT = "A";
			string sinput = INPUT;
			if (!sinput.Equals("A")) {
				return "消息缓存设置已炸";
			}
			Thread tmsg = new Thread(startProcReadMsg);
			tmsg.Start();
			Thread tproc = new Thread(startProcThread);
			tproc.Start();
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
					s += PROCSTR;
					return s;
				}
			} else {
				return procstr;
			}
			return "未找到指定应用程序";
		}
		
		// 远端发送指令
		public static string postLongCmd(string pname, string cmd) {
			if (findedProcName(pname)) {
				INPUT = cmd;
				INPUTFLAG = INPUTCMD_SEND;
				return "远程命令" + cmd + "已发送";
			}
			return "未能找到对应接收进程";
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
				 // 远端进程发送消息
				 return postLongCmd(pname, cmd);
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
