/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2019/3/16
 * 时间: 19:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
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
//		protected Button btwhite, btblack, showmc, showlog, /*showevent,*/ btcmd, shutdown, StartServer;
//		protected	TextBox		whitetext, blacktext, cmdtext;
//		protected HtmlGenericControl msg, welcome;
		
		public string BACKUPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["BACKUPDIR"];
		public string MAPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["MAPDIR"];
		public string FTPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPDIR"];
		public static string LAUNCHERNAME = System.Web.Configuration.WebConfigurationManager.AppSettings["LAUNCHERNAME"];
		public static string LAUNCHERPATH = System.Web.Configuration.WebConfigurationManager.AppSettings["LAUNCHERPATH"];

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
			string	mt = msg.InnerHtml;
			mt += ("<br>" + s);
			msg.InnerHtml = MCWinControl.FormatStrAsLine(mt, 20);
		}
		// 显示后台
		void ShowmcClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.getStrFromProc(LAUNCHERNAME));
		}
		// 显示往期log
		void ShowlogClick(object sender, EventArgs e) {
			msg.InnerHtml = MCWinControl.LOG_FILE_INFO;
		}
		// 显示监控
//		void ShoweventClick(object sender, EventArgs e) {
//			msg.InnerHtml = MCWinControl.EVENT_FILE_INFO;
//			base.Response.Redirect("events.aspx", true);
//		}
		// 关服
		void ShutdownClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.closeProc(LAUNCHERNAME));
		}
		// 启动服务端
		void StartServerClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.StartProc(LAUNCHERNAME, LAUNCHERPATH));
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
			secAddTxt(MCWinControl.postLongCmd(LAUNCHERNAME, cmd));
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
			secAddTxt(MCWinControl.postLongCmd(LAUNCHERNAME, "whitelist add \"" + uname + "\""));
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
			secAddTxt(MCWinControl.postLongCmd(LAUNCHERNAME, "whitelist remove \"" + uname + "\""));
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
			showlog.Click += ShowlogClick;
//			showevent.Click += ShoweventClick;
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

		public static Hashtable memtable = new Hashtable();

		private static Process myProcess = null;
		private static string procname = null;
		private static string procpath = null;
		private static bool keeprun = false;
		
		public static string PROCNAME = System.Web.Configuration.WebConfigurationManager.AppSettings["PROCNAME"];
		public static string PROCPATH = System.Web.Configuration.WebConfigurationManager.AppSettings["PROCPATH"];
		public static string PEXEPATH = System.Web.Configuration.WebConfigurationManager.AppSettings["PEXEPATH"];
		public static string PDLLDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["PDLLDIR"];
		public static string LOG_FILE_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["LOGPATH"];
		public static string EVENT_FILE_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["EVENTPATH"];
		
		public const string KEEPRUN_FILE = @"\MKEEPRUN.tmp";
		public const string PROCSTR_FILE = @"\MPROCSTR.tmp";
		public const string INPUT_FILE = @"\MINPUT.tmp";
		public const string INPUTFLAG_FILE = @"\MINPUTFLAG.tmp";

		public const string KEEPRUN_TAG = "MKEEPRUN_TAG";
		public const string PROCSTR_TAG = "MPROCSTR_TAG";
		public const string INPUT_TAG = "MINPUT_TAG";
		public const string INPUTFLAG_TAG = "MINPUTFLAG_TAG";
		
		public const int INPUTCMD_END = 0;
		public const int INPUTCMD_RUN = 1;
		public const int INPUTCMD_SEND = 2;
		
		/// <summary>
		/// 共享内存读取一个整数
		/// </summary>
		/// <param name="tag">标签名</param>
		/// <returns>取值</returns>
		public static int memgetInt(string tag)
		{
			if (string.IsNullOrEmpty(tag)) {
				return 0;
			}
			int i = 0;
			try {
				MemoryMappedFile mmf = null;
				Object o = memtable[tag];
				if (o == null) {
					mmf = MemoryMappedFile.CreateOrOpen(tag, sizeof(int));
					memtable.Add(tag, mmf);
				} else {
					mmf = (MemoryMappedFile)o;
				}
				i = mmf.CreateViewAccessor().ReadInt32(0);
			} catch {
			}
			return i;
		}
		
		/// <summary>
		/// 共享内存设置一个整数
		/// </summary>
		/// <param name="tag">标签名</param>
		/// <param name="value">整数值</param>
		public static void memsetInt(string tag, int value)
		{
			if (string.IsNullOrEmpty(tag)) {
				return;
			}
			try {
				MemoryMappedFile mmf = null;
				Object o = memtable[tag];
				if (o == null) {
					mmf = MemoryMappedFile.CreateOrOpen(tag, sizeof(int));
					memtable.Add(tag, mmf);
				} else {
					mmf = (MemoryMappedFile)o;
				}
				mmf.CreateViewAccessor().Write(0, value);
			} catch {
			}
		}
		
		/// <summary>
		/// 共享内存读取一个字符串
		/// </summary>
		/// <param name="tag">标签名</param>
		/// <returns>取值</returns>
		public static string memgetString(string tag)
		{
			if (string.IsNullOrEmpty(tag)) {
				return null;
			}
			string s = null;
			try {
				MemoryMappedFile mmf = null;
				Object o = memtable[tag];
				if (o == null) {
					mmf = MemoryMappedFile.CreateOrOpen(tag, 1024 * 1024);
					memtable.Add(tag, mmf);
				} else {
					mmf = (MemoryMappedFile)o;
				}
				MemoryMappedViewAccessor vm = mmf.CreateViewAccessor();
				int size = vm.ReadInt32(0);
				byte [] chs = null;
				if (size > 0) {
					chs = new byte[size];
					for (int i = 0, l = sizeof(int); i < size; i++) {
						chs[i] = vm.ReadByte(i + l);
					}
					s = Encoding.UTF8.GetString(chs);
				}
			} catch {
			}
			return s;
		}
		
		/// <summary>
		/// 共享内存设置一个字符串
		/// </summary>
		/// <param name="tag">标签名</param>
		/// <param name="value">值</param>
		public static void memsetString(string tag, string value)
		{
			if (string.IsNullOrEmpty(tag)) {
				return;
			}
			try {
				int size = 0;
				byte [] chs = null;
				if (!string.IsNullOrEmpty(value)) {
					chs = Encoding.UTF8.GetBytes(value);
					size = chs.Length;
				}
				MemoryMappedFile mmf = null;
				Object o = memtable[tag];
				if (o == null) {
					mmf = MemoryMappedFile.CreateOrOpen(tag, 1024 * 1024);
					memtable.Add(tag, mmf);
				} else {
					mmf = (MemoryMappedFile)o;
				}
				MemoryMappedViewAccessor vm = mmf.CreateViewAccessor();
				vm.Write(0, size);
				for (int i = 0, l = sizeof(int); i < size; i++) {
					vm.Write(i + l, chs[i]);
				}
			} catch {
			}
		}
		
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
				return memgetInt(KEEPRUN_TAG) == 1;
			}
			set{
				memsetInt(KEEPRUN_TAG, value ? 1 : 0);
			}
		}

		// 硬存储log信息
		public static string PROCSTR {
			get {
				return memgetString(PROCSTR_TAG);
			}
			set {
				memsetString(PROCSTR_TAG, value);
			}
		}
		
		// 硬存储外部发送信号信息
		public static int INPUTFLAG {
			get {
				return memgetInt(INPUTFLAG_TAG);
			}
			set {
				memsetInt(INPUTFLAG_TAG, value);
			}
		}
		// 硬存储外部输入流详细信息
		public static string INPUT {
			get {
				return memgetString(INPUT_TAG);
			}
			set {
				memsetString(INPUT_TAG, value);
			}
		}
		// 文件存储外部输入信息
		public static string LOG_FILE_INFO {
			get {
				try {
					return File.ReadAllText(LOG_FILE_PATH);
				} catch{
				}
				return null;
			}
			set {
				try {
					File.WriteAllText(LOG_FILE_PATH, value);
				} catch {
				}
			}
		}
		// 文件存储监控事件信息
		public static string EVENT_FILE_INFO {
			get {
				try {
					return File.ReadAllText(EVENT_FILE_PATH);
				} catch{
				}
				return null;
			}
			set {
				try {
					File.WriteAllText(EVENT_FILE_PATH, value);
				} catch {
				}
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
			if (!findedProcName(procname))
				return "未能获取指定进程信息";
			return "已发送关闭指令";
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
			try {
				string[] strs = mystr.Split(new string [] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
				if (strs.Length < linecount) {
					return mystr;
				}
				mystr = "";
				for (int i = strs.Length - linecount; i < strs.Length; i++) {
					mystr += ("<br>" + strs[i]);
				}
				return mystr.Substring(mystr.IndexOf("<br>") + 4);
			} catch {
			}
			return mystr.Substring(mystr.IndexOf("<br>") + 4);
		}

		// 自动重启服务
		private static void startProcThread()
		{
			while (keeprun) {
				if (findedProcName(procname)) {
					// 已存在实例
					return;
				}
				myProcess = new Process();
				myProcess.StartInfo.FileName = procpath;//控制台程序的路径
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardOutput = true;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.StartInfo.Arguments = PROCNAME + " " + PROCPATH + " " +
					PEXEPATH + " " + PDLLDIR + " " +
					LOG_FILE_PATH + " " + EVENT_FILE_PATH;
				myProcess.Start();
				myProcess.WaitForExit();
				myProcess.Close();
				myProcess = null;
				PROCSTR = "log end";
				keeprun = KEEPRUN;
				if (keeprun)
					for (int i = 0; i < 10 && KEEPRUN; i++)
						Thread.Sleep(1000);
			}
		}
		// 返回是否发现指定进程
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
			Thread tproc = new Thread(startProcThread);
			tproc.Start();
			return "尝试开服，请使用log查看信息"
				+ "<br>" + "参数：" + PROCNAME + " " + PROCPATH + " " +
					PEXEPATH + " " + PDLLDIR + " " +
					LOG_FILE_PATH + " " + EVENT_FILE_PATH;
		}
		
		/// <summary>
		/// 获取后台信息
		/// </summary>
		/// <param name="procname">进程名</param>
		/// <returns>信息(含错误信息)</returns>
		public static string getStrFromProc(string procname) {
			// 直接接管
			if (findedProcName(procname)) {
				return PROCSTR;
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
