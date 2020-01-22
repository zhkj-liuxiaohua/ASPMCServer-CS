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
using System.IO.Pipes;
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
	public class mccontrol : Page
	{
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region 预定义按钮处，此处不用开放

		protected HtmlInputButton logout, showbackup, clearbackup, cpmap;
		protected Button btwhite, btblack, btban, btunban, showmc, showlog, /*showevent,*/ btcmd, shutdown, StartServer;
		protected TextBox whitetext, blacktext, bantext, unbantext, cmdtext;
		protected HtmlTextArea msg;
		protected HtmlAnchor welcome;
		
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
			string	mt = msg.Value;
			mt += ("\n" + s);
			msg.Value = MCWinControl.FormatStrAsLine(mt, 20);
		}
		// 显示后台
		void ShowmcClick(object sender, EventArgs e)
		{
			secAddTxt(MCWinControl.getStrFromProc(LAUNCHERNAME));
		}
		// 显示往期log
		void ShowlogClick(object sender, EventArgs e) {
			msg.Value = MCWinControl.LOG_FILE_INFO;
		}
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
		// 添加黑名单
		void BtbanClick(object sender, EventArgs e)
		{
			string uname = bantext.Text;
			if (String.IsNullOrEmpty(uname)) {
				secAddTxt("空用户名");
				return;
			}
			bantext.Text = "";
			secAddTxt(MCWinControl.ban(LAUNCHERNAME, uname));
		}
		// 移除黑名单
		void BtunbanClick(object sender, EventArgs e)
		{
			string uname = unbantext.Text;
			if (String.IsNullOrEmpty(uname)) {
				secAddTxt("空用户名");
				return;
			}
			unbantext.Text = "";
			secAddTxt(MCWinControl.unban(LAUNCHERNAME, uname));
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
			btban.Click += BtbanClick;
			btunban.Click += BtunbanClick;
			showmc.Click += ShowmcClick;
			showlog.Click += ShowlogClick;
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
		public static string PIPEMSGTAG = System.Web.Configuration.WebConfigurationManager.AppSettings["PIPEMSGTAG"];
		public static string PEXEPATH = System.Web.Configuration.WebConfigurationManager.AppSettings["PEXEPATH"];
		public static string PDLLDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["PDLLDIR"];
		public static string LOG_FILE_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["LOGPATH"];
		public static string EVENT_FILE_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["EVENTPATH"];
		public static string BANLIST_PATH = System.Web.Configuration.WebConfigurationManager.AppSettings["BANLISTPATH"];
		public static string TMPDIR = System.Web.Configuration.WebConfigurationManager.AppSettings["TMPDIR"];
		
		public static string CLIENTNAME = System.Web.Configuration.WebConfigurationManager.AppSettings["CLIENTNAME"];
		public static string SERVERADDR = System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERADDR"];
		public static string SERVERPORT = System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERPORT"];
		public static string CLIENTPORT = System.Web.Configuration.WebConfigurationManager.AppSettings["CLIENTPORT"];
		
		public static string KEEPRUN_FILE = Path.GetDirectoryName(PROCPATH) + @"\MKEEPRUN.tmp";
		public const string PROCSTR_FILE = @"\MPROCSTR.tmp";
		public const string INPUT_FILE = @"\MINPUT.tmp";
		public const string INPUTFLAG_FILE = @"\MINPUTFLAG.tmp";

		// 过期参数
//		public const string KEEPRUN_TAG = "MKEEPRUN_TAG";
//		public const string PROCSTR_TAG = "MPROCSTR_TAG";
//		public const string INPUT_TAG = "MINPUT_TAG";
//		public const string INPUTFLAG_TAG = "MINPUTFLAG_TAG";
		
		public const int INPUTCMD_END = 0;
		public const int INPUTCMD_RUN = 1;
		public const int INPUTCMD_SEND = 2;
		
		// 消息类型
		
		/// <summary>
		/// 命令消息
		/// </summary>
		public const string TAG_CMD = "cmd";
		/// <summary>
		/// 停服消息
		/// </summary>
		public const string TAG_STOP = "stop";
		/// <summary>
		/// 取log消息
		/// </summary>
		public const string TAG_REFRESH = "refresh";

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
		
		// 文件存储运行状态
		public static bool KEEPRUN {
			get{
				return File.Exists(KEEPRUN_FILE);
			}
			set{
				try {
					if (value) {
						File.WriteAllText(KEEPRUN_FILE, "1");
					} else
						File.Delete(KEEPRUN_FILE);
				} catch{
				}
			}
		}

		// 管道读取log信息
		public static string PROCSTR {
			get{
				string str = "";
				try {
					string RECEIVE_TAG = "TAG" + (new Random(DateTime.Now.Millisecond).Next(0, int.MaxValue));
					if (null != sendFarMessage(TAG_REFRESH + "," + RECEIVE_TAG)) {
						NamedPipeServerStream pipeIn = new NamedPipeServerStream(RECEIVE_TAG, PipeDirection.InOut, 254);
						pipeIn.WaitForConnection();
						StreamReader reader = new StreamReader(pipeIn);
						while(true) {
						string line = reader.ReadLine();
						if (!string.IsNullOrEmpty(line)) {
						if (line != "FEOF")
							str += (line + "\n");
						else
							break;
							}
						}
						pipeIn.Disconnect();
					}
				} catch {
				}
				return str;
			}
		}
		
		
		// 硬存储外部发送信号信息
		public static string INPUT {
			set {
				sendFarMessage(value);
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
		/// 发送远程消息
		/// </summary>
		/// <param name="msg">完整信息</param>
		/// <returns></returns>
		public static string sendFarMessage(string msg) {
			try {
				NamedPipeClientStream pipeOut = new NamedPipeClientStream(".", PIPEMSGTAG, PipeDirection.InOut);
				pipeOut.Connect(2000);
				StreamWriter writer = new StreamWriter(pipeOut);
				writer.AutoFlush = true;
				writer.WriteLine(msg);
				pipeOut.Close();
				return "消息" + msg + "已发送";
			}catch{
			}
			return null;
		}
		
		
		/// <summary>
		/// 关闭指定进程
		/// </summary>
		/// <param name="procname">进程名</param>
		/// <returns>关闭信息</returns>
		public static string closeProc(string procname)
		{
			if (!findedProcName(procname))
				return "未能获取指定进程信息";
			keeprun = false;
			KEEPRUN = false;
			INPUT = TAG_STOP + ",true";
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
			string[] strs = mystr.Split(new string [] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			if (strs.Length < linecount) {
				return mystr;
			}
			mystr = "";
			for (int i = strs.Length - linecount; i < strs.Length; i++) {
				mystr += ("\n" + strs[i]);
			}
			return mystr.Substring(mystr.IndexOf("\n") + 1);
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
				myProcess.StartInfo.Arguments = PROCNAME + " " + PROCPATH + " " +  PIPEMSGTAG + " " +
					PEXEPATH + " " + PDLLDIR + " " +
					LOG_FILE_PATH + " " + EVENT_FILE_PATH + " " + BANLIST_PATH + ((SERVERPORT != "0" && CLIENTPORT != "0") ? 
					                                                              (" " + CLIENTNAME + " " + SERVERADDR + " " + SERVERPORT + " " + CLIENTPORT) : "");
				myProcess.Start();
				myProcess.WaitForExit();
				myProcess.Close();
				myProcess = null;
				//PROCSTR = "log end";
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
			// 共享文件自检，通道自检跳过
			KEEPRUN = true;
			bool krflag = KEEPRUN;
			if (!krflag) {
				return "运行状态标志设置已炸";
			}
			Thread tproc = new Thread(startProcThread);
			tproc.Start();
			return "尝试开服，请使用log查看信息"
				+ "\n" + "参数：" + PROCNAME + " " + PROCPATH + " " + PIPEMSGTAG + " " +
					PEXEPATH + " " + PDLLDIR + " " +
					LOG_FILE_PATH + " " + EVENT_FILE_PATH + " " + BANLIST_PATH + ((SERVERPORT != "0" && CLIENTPORT != "0") ? 
					                                                              (" " + CLIENTNAME + " " + SERVERADDR + " " + SERVERPORT + " " + CLIENTPORT) : "");
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
				INPUT = TAG_CMD + "," + cmd;
				return "远程命令" + cmd + "已发送";
			}
			return "未能找到对应接收进程";
		}

		// 重载黑名单
		private static string [] reloadBanlist() {
			string [] bl = new string[]{};
			try {
				bl = File.ReadAllLines(BANLIST_PATH);
			} catch {
			}
			return bl;
		}
		
		// 拉黑名单
		public static string ban(string pname, string name) {
			string [] bps = reloadBanlist();
			foreach (string ob in bps) {
				string [] pinfos = ob.Split(',');
				if (pinfos[0] == name) {
					// 不添加
					return "玩家" + name + "已存在于黑名单上。";
				}
			}
			File.AppendAllLines(BANLIST_PATH, new string[] {name});
			postLongCmd(pname, "BANUPDATE");
			return "玩家" + name + "已添加至黑名单。";
		}
		
		// 解除黑名单
		public static string unban(string pname, string name) {
			string [] bps = reloadBanlist();
			int i = 0;
			for(int l = bps.Length; i < l; i++) {
				string ob = bps[i];
				string [] pinfos = ob.Split(',');
				if (pinfos[0] == name) {
					// 即将移除
					break;
				}
			}
			if (i >= bps.Length)
				return "玩家" + name + "未在黑名单内。";
			string nstr = "";
			for (int j = 0; j < bps.Length; j++) {
				if (j != i)
					nstr += (bps[j] + "\n");
			}
			File.WriteAllText(BANLIST_PATH, nstr);
			postLongCmd(pname, "BANUPDATE");
			return "玩家" + name + "已从黑名单中移除。";
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
