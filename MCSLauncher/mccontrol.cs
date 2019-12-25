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
using System.IO.Pipes;					  
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;
using MCSLauncher.net;

namespace MCSLauncher
{
	/// <summary>
	/// Description of mccontrol.
	/// </summary>
	public static class mccontrol
	{
		public static Hashtable memtable = new Hashtable();
		public static Mutex mutex = new Mutex();

		private static Process myProcess = null;
		private static string procname = null;
		private static string procpath = null;
		private static string ptag = null;
		private static string pargs = null;
		private static string procstr = "";
		private static string[] banliststr = null;
		private static bool keeprun = false;
		public static string log_file_path = "log.txt";
		public static string event_file_path = "event.txt";
		public static string ban_file_path = "banlist.txt";
		public static string KEEPRUN_FILE = System.AppDomain.CurrentDomain.BaseDirectory + "MKEEPRUN.tmp";
		// 过期mmf方法
//		public const string KEEPRUN_TAG = "MKEEPRUN_TAG";
//		public const string PROCSTR_TAG = "MPROCSTR_TAG";
//		public const string INPUT_TAG = "MINPUT_TAG";
//		public const string INPUTFLAG_TAG = "MINPUTFLAG_TAG";
		
		// 跨服聊天相关
		public static P2PLoader chatservice = null;
		public static string myname = null;
		public static int myport = 0;
		public static string cserverip = null;
		public static int cserverport = 0;
		public static string chatkey = null;
		
		public const string CK_CMD = "CMD";
		public const string CK_PORT = "PORT";
		public const string CK_KEY = "KEY";
		public const string CK_NAME = "NAME";
		public const string CK_MSG = "MSG";
		/// <summary>
		/// 注册客户端
		/// 格式：CMD=reg,PORT=client_port,KEY=client_key,NAME=client_name
		/// </summary>
		public const string CMD_REG = "reg";
		/// <summary>
		/// 注销客户端
		/// 格式：CMD=unreg,KEY=client_key
		/// </summary>
		public const string CMD_UNREG = "unreg";
		/// <summary>
		/// 广播消息
		/// 格式：CMD=msg,KEY=client_key,MSG=your_message
		/// </summary>
		public const string CMD_MSG = "msg";
		
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
				byte[] chs = null;
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
				byte[] chs = null;
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
		public static void filesetString(string path, string value)
		{
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
			get {
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

		// 文件存储外部输入信息
		public static string LOG_FILE_INFO {
			get {
				try {
					return File.ReadAllText(log_file_path);
				} catch {
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
		// 文件存储监控事件信息
		public static string EVENT_FILE_INFO {
			get {
				try {
					return File.ReadAllText(event_file_path);
				} catch {
				}
				return null;
			}
			set {
				try {
					File.WriteAllText(event_file_path, value);
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
			Process[] ps = Process.GetProcessesByName(procname);
			if (ps != null && ps.Length > 0) {
				foreach (Process p in ps) {
					try {
						KillTree(p.Id);
					} catch (Exception e) {
						return "失败：" + e.Message;
					}
				}
				return "已关闭指定进程";
			}
			return "未能获取指定进程信息";
		}

		#region PInvokes
		[DllImport("KERNEL32.DLL")]
		private static extern int OpenProcess(eDesiredAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId);
		[DllImport("KERNEL32.DLL")]
		private static extern int CloseHandle(int hObject);
		[DllImport("NTDLL.DLL")]
		private static extern int NtQueryInformationProcess(int hProcess, PROCESSINFOCLASS pic, ref PROCESS_BASIC_INFORMATION pbi, int cb, ref int pSize);
		private enum PROCESSINFOCLASS : int
		{
			ProcessBasicInformation = 0,
			ProcessQuotaLimits,
			ProcessIoCounters,
			ProcessVmCounters,
			ProcessTimes,
			ProcessBasePriority,
			ProcessRaisePriority,
			ProcessDebugPort,
			ProcessExceptionPort,
			ProcessAccessToken,
			ProcessLdtInformation,
			ProcessLdtSize,
			ProcessDefaultHardErrorMode,
			ProcessIoPortHandlers,
			// Note: this is kernel mode only
			ProcessPooledUsageAndLimits,
			ProcessWorkingSetWatch,
			ProcessUserModeIOPL,
			ProcessEnableAlignmentFaultFixup,
			ProcessPriorityClass,
			ProcessWx86Information,
			ProcessHandleCount,
			ProcessAffinityMask,
			ProcessPriorityBoost,
			MaxProcessInfoClass}

		;

		[StructLayout(LayoutKind.Sequential)]
		private struct PROCESS_BASIC_INFORMATION
		{
			public int ExitStatus;
			public int PebBaseAddress;
			public int AffinityMask;
			public int BasePriority;
			public int UniqueProcessId;
			public int InheritedFromUniqueProcessId;
			public int Size {
				get {
					return (6 * 4);
				}

			}

		};

		private enum eDesiredAccess : int
		{
			DELETE = 0x00010000,
			READ_CONTROL = 0x00020000,
			WRITE_DAC = 0x00040000,
			WRITE_OWNER = 0x00080000,
			SYNCHRONIZE = 0x00100000,
			STANDARD_RIGHTS_ALL = 0x001F0000,
			PROCESS_TERMINATE = 0x0001,
			PROCESS_CREATE_THREAD = 0x0002,
			PROCESS_SET_SESSIONID = 0x0004,
			PROCESS_VM_OPERATION = 0x0008,
			PROCESS_VM_READ = 0x0010,
			PROCESS_VM_WRITE = 0x0020,
			PROCESS_DUP_HANDLE = 0x0040,
			PROCESS_CREATE_PROCESS = 0x0080,
			PROCESS_SET_QUOTA = 0x0100,
			PROCESS_SET_INFORMATION = 0x0200,
			PROCESS_QUERY_INFORMATION = 0x0400,
			PROCESS_ALL_ACCESS = SYNCHRONIZE | 0xFFF

		}
		#endregion
		
		public static void KillTree(int processToKillId)
		{
			// Kill each child process
			foreach (int childProcessId in GetChildProcessIds(processToKillId)) {
				using (Process child = Process.GetProcessById(childProcessId)) {
					child.Kill();

				}

			}

			// Then kill this process
			using (Process thisProcess = Process.GetProcessById(processToKillId)) {
				thisProcess.Kill();
			}
		}

		public static int GetParentProcessId(int processId)
		{
			int ParentID = 0;
			int hProcess = OpenProcess(eDesiredAccess.PROCESS_QUERY_INFORMATION,
				               false, processId);
			if (hProcess != 0) {
				try {
					PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
					int pSize = 0;
					if (-1 != NtQueryInformationProcess(hProcess,
						    PROCESSINFOCLASS.ProcessBasicInformation, ref pbi, pbi.Size, ref
					                                    pSize)) {
						ParentID = pbi.InheritedFromUniqueProcessId;
					}
				} finally {
					CloseHandle(hProcess);
				}
			}
			return (ParentID);
		}

		public static int[] GetChildProcessIds(int parentProcessId)
		{
			ArrayList myChildren = new ArrayList();
			foreach (Process proc in Process.GetProcesses()) {
				int currentProcessId = proc.Id;
				proc.Dispose();
				if (parentProcessId == GetParentProcessId(currentProcessId)) {
					// Add this one
					myChildren.Add(currentProcessId);
					// Add any of its children
					myChildren.AddRange(GetChildProcessIds(currentProcessId));
				}

			}
			return (int[])myChildren.ToArray(typeof(int));
		}

		/// <summary>
		/// 保留指定行数的信息
		/// </summary>
		/// <param name="mystr">原始字符串</param>
		/// <param name="linecount">指定行数</param>
		/// <returns>修整后的行数</returns>
		public static string FormatStrAsLine(string mystr, int linecount)
		{
			if (linecount < 1) {
				return mystr;
			}
			string[] strs = mystr.Split(new string [] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
			if (strs.Length < linecount) {
				return mystr;
			}
			mystr = "";
			for (int i = strs.Length - linecount; i < strs.Length; i++) {
				mystr += ("<br>" + strs[i]);
			}
			return mystr.Substring(mystr.IndexOf("<br>") + 4);
		}

		/// <summary>
		/// 发信消息
		/// </summary>
		/// <param name="port">待发送的端口</param>
		/// <param name="msg">待发送的字符串</param>
		public static string sendFarMessage(string port, string msg) {
			// 创建管道连接
			try {
			NamedPipeClientStream pipeOut = new NamedPipeClientStream(".", port, PipeDirection.InOut);
			pipeOut.Connect(2000);
			StreamWriter writer = new StreamWriter(pipeOut);
			writer.AutoFlush = true;
			writer.WriteLine(msg);
			writer.WriteLine("FEOF");
			pipeOut.Close();
			return "远程专有消息已发送。";
			} catch{
			}
			return null;
		}					
		// 外部输入流监听服务
		private static void startProcReadMsg()
		{
			NamedPipeServerStream pipeIn = new NamedPipeServerStream(ptag, PipeDirection.InOut, 254);
			while(keeprun) {
				pipeIn.WaitForConnection();
				StreamReader reader = new StreamReader(pipeIn);
				string line = reader.ReadLine();
				if (!string.IsNullOrEmpty(line)) {
					// 消息处理
					string [] cmds = line.Split(',');
					if(cmds.Length > 1) {
						string curstrr = line.Substring(line.IndexOf(',') + 1);
						switch (cmds[0]) {
							case TAG_CMD:
								if (!string.IsNullOrEmpty(curstrr)) { 
									if (curstrr == "BANUPDATE") { 
									// 更新banlist 
									reloadBanlist(); 
								} else 
									sendCommand(procname, curstrr); 
								} 
								break;
							case TAG_REFRESH:
								sendFarMessage(curstrr, procstr);
								break;
							case TAG_STOP:
								keeprun = false;
								KEEPRUN = false;
								break;
							//... to do other case
						}
					}
				}
				pipeIn.Disconnect();
			}
			// 监听服务结束时，结束正在运行的应用程序
			if (chatservice != null) {
				chatunregister();
				chatservice.stopListen();
				chatservice = null;
			}
			closeProc(procname);
			
		}

		// 输出信息接收记录服务
		private static void OnDataReceived(object sender, DataReceivedEventArgs e)
		{
			string info = e.Data;
			if (String.IsNullOrEmpty(info))
				return;
			if (info[0] == '{') {
				// 添加事件监听
				string ev = info.Substring(1);
				eventAdd(ev);
				if(chatservice != null) {
					if (ev.IndexOf("Chat") > -1) {
						chatSendFarMsg(ev);
					}
				}
				return;
			}
			procstr = procstr + "<br>" + e.Data;
			procstr = FormatStrAsLine(procstr, 2000); // 最多保留2000行log文本
			//PROCSTR = procstr;
			logAdd(e.Data);
			checkBanlist(info);
			checkChatPlayerPlays(info);
		}

		// 检查黑名单是否符合指定信息
		private static void checkBanlist(string info)
		{
			int ci = info.IndexOf("Player connected");
			if (ci > -1) {
				// 解析连接数据
				string[] pinfo = info.Substring(ci).Split(',');
				if (pinfo.Length == 2) {
					string[] pids = pinfo[0].Split(':');
					string[] puids = pinfo[1].Split(':');
					string name = "", xuid = "";
					if (pids.Length == 2) {
						name = pids[1].Trim();
					}
					if (pinfo.Length == 2) {
						xuid = puids[1].Trim();
					}
					// 判断是否存在于黑名单中，并顺便补充完善黑名单xuid
					int i = 0, l = 0;
					for (l = banliststr.Length; i < l; i++) {
						string[] binfo = banliststr[i].Split(',');
						string bname = "", bxuid = "";
						if (binfo.Length > 0) {
							bname = binfo[0];
						}
						if (binfo.Length > 1) {
							bxuid = binfo[1];
						}
						if (bname == name || bxuid == xuid)
							break;
					}
					if (i < l) {
						// 发现存在于黑名单中
						if (banliststr[i].Split(',').Length < 2) {
							banliststr[i] += ("," + xuid);
							updateBanlist();
						}
						new Thread(delegate(){
						           	Thread.Sleep(1000);
						           	sendCommand(procname, "kick \"" + name + "\" 该玩家存在于本服黑名单列表中。");
						           }
						          ).Start();
						
					}
				}
			}
		}
		
		// 重载黑名单
		private static void reloadBanlist()
		{
			banliststr = new string[]{};
			try {
				banliststr = File.ReadAllLines(ban_file_path);
			} catch {
			}
		}
		// 更新黑名单
		private static void updateBanlist()
		{
			try {
				File.WriteAllLines(ban_file_path, banliststr);
			} catch {
			}
		}

		// 发信玩家登入登出记录
		private static void checkChatPlayerPlays(string info) {
			if (chatservice == null) {
				return;
			}
			int ci = info.IndexOf("Player connected");
			if (ci > -1) {
				// 解析连接数据
				string[] pinfo = info.Substring(ci).Split(',');
				if (pinfo.Length == 2) {
					string[] pids = pinfo[0].Split(':');
					string name = "";
					if (pids.Length == 2) {
						name = pids[1].Trim();
						chatSendFarMsg("玩家 " + name + " 进入了游戏");
					}
				}
				return;
			}
			ci = info.IndexOf("Player disconnected");
			if (ci > -1) {
				// 解析断开数据
				string[] pinfo = info.Substring(ci).Split(',');
				if (pinfo.Length == 2) {
					string[] pids = pinfo[0].Split(':');
					string name = "";
					if (pids.Length == 2) {
						name = pids[1].Trim();
						chatSendFarMsg("玩家 " + name + " 退出了游戏");
					}
				}
			}
		}
		
		// 自动重启服务
		private static void startProcThread()
		{
			while (keeprun) {
				procstr = "";
				reloadBanlist();
				if (findedProcName(procname)) {
					// 已存在实例
					return;
				}
				myProcess = new Process();
				myProcess.StartInfo.FileName = procpath;//控制台程序的路径
				myProcess.StartInfo.Arguments = pargs;
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardOutput = true;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.CreateNoWindow = true;
				myProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
				myProcess.OutputDataReceived += OnDataReceived;
				myProcess.Start();
				myProcess.BeginOutputReadLine();
				myProcess.WaitForExit();
				myProcess.Close();
				myProcess = null;
				procstr = "log end";
				//PROCSTR = "log end";
				keeprun = KEEPRUN;
				if (keeprun)
					for (int i = 0; i < 10 && KEEPRUN; i++)
						Thread.Sleep(1000);	// 自动重启时限：10s
			}
		}
		private static bool findedProcName(string pname)
		{
			Process[] ps = Process.GetProcessesByName(pname);
			return (ps != null && ps.Length > 0);
		}

		/// <summary>
		/// 聊天消息接收器
		/// </summary>
		/// <param name="ipep"></param>
		/// <param name="msg"></param>
		private static void onChatMsgReceive(string ipep, string msg) {
			string [] farmsgs = msg.Split(',');
			if (farmsgs.Length > 1) {
				string msgnode = msg.Substring(msg.IndexOf(',') + 1);
				string [] para1 = farmsgs[0].Split('=');
				string [] para2 = farmsgs[1].Split('=');
				if (para1.Length > 1 && para2.Length > 1 && para1[0] == CK_KEY && para2[0] == CK_MSG) {
					if (para1[1] == chatkey) {
						sendCommand(procname, "me " + msgnode.Substring(msgnode.IndexOf('=') + 1));
					}
				}
			}
		}

		/// <summary>
		/// 结束聊天回调
		/// </summary>
		private static void onChatStop() {
			logAdd("聊天会话已结束。");
		}
		
		/// <summary>
		/// 注册此客户端到远程聊天服务器
		/// </summary>
		public static void chatregister() {
			if (chatservice != null) {
				chatkey = Guid.NewGuid().ToString();
				chatservice.sendMsg(cserverip, cserverport, CK_CMD + "=" + CMD_REG + "," + CK_PORT + "=" + myport + "," + 
				                    CK_KEY + "=" + chatkey + "," + CK_NAME + "=" + myname);
			}
		}
		
		/// <summary>
		/// 结束聊天监听
		/// </summary>
		public static void chatunregister() {
			if (chatservice != null) {
				chatservice.sendMsg(cserverip, cserverport, CK_CMD + "=" + CMD_UNREG + "," + CK_KEY + "=" + chatkey);
			}
		}

		/// <summary>
		/// 发送远程聊天信息
		/// </summary>
		/// <param name="msg"></param>
		public static void chatSendFarMsg(string msg) {
			if (chatservice != null) {
				chatservice.sendMsg(cserverip, cserverport, CK_CMD + "=" + CMD_MSG + "," + CK_KEY + "=" + chatkey + "," + CK_MSG + "=" + msg);
			}
		}
		
		/// <summary>
		/// 通过域名取IP
		/// </summary>
		/// <param name="web">域名</param>
		/// <returns>IP地址</returns>
		public static string getIp(string web) {
			IPHostEntry host = Dns.GetHostByName(web);
			IPAddress ip = host.AddressList[0];
			return ip.ToString();
		}
		
		/// <summary>
		/// 我要开服（含聊天端口）
		/// </summary>
		/// <param name="pname"></param>
		/// <param name="fpath"></param>
		/// <param name="ptag"></param>
		/// <param name="pexe"></param>
		/// <param name="pmoddir"></param>
		/// <param name="logpath"></param>
		/// <param name="eventpath"></param>
		/// <param name="banlistpath"></param>
		/// <param name="clientname"></param>
		/// <param name="serveraddr"></param>
		/// <param name="serverport"></param>
		/// <param name="clientport"></param>
		/// <returns></returns>
		public static string StartProc(string pname, string fpath, string ptag, string pexe, string pmoddir, string logpath, string eventpath, string banlistpath,
		                               string clientname, string serveraddr, string serverport, string clientport) {
			if (clientname != null && serveraddr != null && serverport != null && clientport != null) {
				myport = int.Parse(clientport);
				if (myport != 0) {
					chatservice = new P2PLoader(int.Parse(clientport));
					myname = clientname;
					cserverip = getIp(serveraddr);
					cserverport = int.Parse(serverport);
					chatservice.setOnReceiveMsg(onChatMsgReceive);
					chatservice.setOnStopListen(onChatStop);
					chatservice.startListen();
					chatregister();
				}
			}
			return StartProc(pname, fpath, ptag, pexe, pmoddir, logpath, eventpath, banlistpath);
		}
		
		/// <summary>
		/// 我要开服
		/// </summary>
		/// <param name="pname">服务端应用名称</param>
		/// <param name="fpath">服务端应用实际完整路径</param>
		/// <param name="pexe">实际服务端应用程序路径</param>
		/// <param name="pmoddir">监控插件目录</param>
		/// <param name="logpath">服务端往期log完整路径</param>
		/// <param name="eventpath">服务端往期监控完整路径</param>
		/// <param name="banlistpath">黑名单完整路径</param>
		/// <returns>开服信息</returns>
		public static string StartProc(string pname, string fpath, string ptag, string pexe, string pmoddir, string logpath, string eventpath, string banlistpath)
		{
			if (myProcess != null) {
				if (!myProcess.HasExited)
					return "已启动一个实例";
			}
			if (findedProcName(pname)) {
				// 此处需要重新杀进程
				//return "已启动一个实例";
				closeProc(pname);
				Thread.Sleep(4000);	// 自动重启时限：4s
			}
			procname = pname;
			procpath = fpath;
			mccontrol.ptag = ptag;
			pargs = " " + pexe + " " + pmoddir;
			log_file_path = logpath;
			event_file_path = eventpath;
			ban_file_path = banlistpath;
			keeprun = true;
			// 共享文件自检
			KEEPRUN = true;
			bool krflag = KEEPRUN;
			if (!krflag) {
				return "运行状态标志设置已炸";
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
		public static string getStrFromProc(string procname)
		{
			return procstr;
		}
		
		// 远端发送指令
		public static string postLongCmd(string pname, string cmd)
		{
			return sendCommand(pname, cmd);
		}
		/// <summary>
		/// 发送指定消息
		/// </summary>
		/// <param name="pname">进程名</param>
		/// <param name="cmd">消息</param>
		/// <returns>消息反馈</returns>
		public static string sendCommand(string pname, string cmd)
		{
			if (myProcess != null) {
				if (!myProcess.HasExited) {
					byte [] bcmds = Encoding.UTF8.GetBytes(cmd);
					myProcess.StandardInput.BaseStream.Write(bcmds, 0, bcmds.Length);
					myProcess.StandardInput.WriteLine("");
					logAdd(cmd);
				}
			} else {
				// 远端进程发送消息
				// 出错
				return "由于进程被回收，命令发送失败。";
				//return postLongCmd(pname, cmd);
			}
			return "命令" + cmd + "已发送";
		}
		
		/// <summary>
		/// 截取前段指定行数的信息
		/// </summary>
		/// <param name="mystr">原始字符串</param>
		/// <param name="linecount">指定行数</param>
		/// <returns>修整后的行数</returns>
		public static string FormatStrAsLineResc(string mystr, int linecount)
		{
			if (linecount < 1) {
				return mystr;
			}
			string[] strs = mystr.Split(new string [] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
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
			mutex.WaitOne();
			string slog = LOG_FILE_INFO;
			slog = s + "<br>" + slog;			// 逆序存储所有log文本
			slog = FormatStrAsLineResc(slog, 2000); // 最多保留2000行log文本
			LOG_FILE_INFO = slog;
			mutex.ReleaseMutex();
		}
		
		/// <summary>
		/// 添加一条监控信息到文件
		/// </summary>
		/// <param name="s"></param>
		public static void eventAdd(string s)
		{
			mutex.WaitOne();
			try {
				File.AppendAllLines(event_file_path, new string[]{ s });	// 逐行存储，不设限制，通过计划任务进行每日日志调度
			} catch{
			}
			mutex.ReleaseMutex();
		}
	}
}
