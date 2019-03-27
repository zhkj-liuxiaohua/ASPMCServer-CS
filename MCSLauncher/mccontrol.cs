/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2019/3/19
 * 时间: 18:57
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;

namespace MCSLauncher
{
	/// <summary>
	/// Description of mccontrol.
	/// </summary>
	public class mccontrol
	{
		public static Hashtable memtable = new Hashtable();
		
		public mccontrol()
		{
		}
		
		private static Process myProcess = null;
		private static string procname = null;
		private static string procpath = null;
		private static string procstr = "";
		private static bool keeprun = false;
		public static string log_file_path = "log.txt";
		
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
					return File.ReadAllText(log_file_path);
				} catch{
				}
				return null;
			}
			set {
				try {
					File.WriteAllText(log_file_path, value);
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
					Thread.Sleep(100);	// 监听频率：0.1s
				} else {
					// 非发送、非运行的情况，退出执行
					break;
				}
			}
			// 监听服务结束时，结束正在运行的应用程序
			closeProc(procname);
		}
		
		private static void OnDataReceived(object sender, DataReceivedEventArgs e) {
			procstr = procstr + "<br>" + e.Data;
			procstr = FormatStrAsLine(procstr, 2000); // 最多保留2000行log文本
			PROCSTR = procstr;
			logAdd(e.Data);
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
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.OutputDataReceived += OnDataReceived;
				myProcess.Start();
				myProcess.BeginOutputReadLine();
				myProcess.WaitForExit();
				//INPUTFLAG = INPUTCMD_END;
				myProcess.Close();
				myProcess = null;
				procstr = "";
				PROCSTR = "log end";
				keeprun = KEEPRUN;
				if (keeprun)
					for (int i = 0; i < 10 && KEEPRUN; i++)
						Thread.Sleep(1000);	// 自动重启时限：10s
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
		/// <param name="logpath">服务端往期log完整路径</param>
		/// <returns>开服信息</returns>
		public static string StartProc(string pname, string fpath, string logpath)
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
			log_file_path = logpath;
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
					logAdd(cmd);
				}
			} else {
				 // 远端进程发送消息
				 return postLongCmd(pname, cmd);
			}
			return "命令" + cmd + "已发送";
		}
		
		/// <summary>
		/// 截取前段指定行数的信息
		/// </summary>
		/// <param name="mystr">原始字符串</param>
		/// <param name="linecount">指定行数</param>
		/// <returns>修整后的行数</returns>
		public static string FormatStrAsLineResc(string mystr, int linecount) {
			if (linecount < 1) {
				return mystr;
			}
			string [] strs = mystr.Split(new string [] {"<br>"}, StringSplitOptions.RemoveEmptyEntries);
			if (strs.Length < linecount) {
				return mystr;
			}
			mystr = strs[0];
			for (int i = 1; i < linecount; i++) {
				mystr += ("<br>" + strs[i]);
			}
			return mystr;
		}
		
		/// <summary>
		/// 添加一条log信息到文件
		/// </summary>
		/// <param name="s">待添加的字符串</param>
		/// <returns></returns>
		public static void logAdd(string s)
		{
			string slog = LOG_FILE_INFO;
			slog = s + "<br>" + slog;			// 逆序存储所有log文本
			slog = FormatStrAsLineResc(slog, 2000); // 最多保留2000行log文本
			LOG_FILE_INFO = slog;
		}
	}
}
