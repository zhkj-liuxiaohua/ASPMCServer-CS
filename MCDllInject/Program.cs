/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/6/3
 * 时间: 15:47
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MCDllInject
{

	[StructLayout(LayoutKind.Sequential)]
	public struct STARTUPINFO { public int cb; public string lpReserved; public string lpDesktop; public int lpTitle; public int dwX; public int dwY; public int dwXSize; public int dwYSize; public int dwXCountChars; public int dwYCountChars; public int dwFillAttribute; public int dwFlags; public int wShowWindow; public int cbReserved2; public byte lpReserved2; public IntPtr hStdInput; public IntPtr hStdOutput; public IntPtr hStdError; }

	[StructLayout(LayoutKind.Sequential)]
	public struct PROCESS_INFORMATION { public IntPtr hProcess; public IntPtr hThread; public int dwProcessId; public int dwThreadId; }
	[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
	public class SECURITY_ATTRIBUTES
	{
		public int nLength; public string lpSecurityDescriptor; public bool bInheritHandle;
	}

	class Program
	{
		public static void Main(string[] args)
		{
			if (args == null || args.Length < 2) {
				Console.WriteLine("参数配置错误");
				Console.WriteLine("用法：" + Assembly.GetExecutingAssembly().GetName().Name + " [exepath] [dlldir]");
				return;
			}
			dllsInject(args[0], args[1]);
		}

		static readonly IntPtr INTPTR_ZERO = (IntPtr)0;
		const int CREATE_SUSPENDED = 0x00000004;
		const int MEM_COMMIT = 0x00001000;
		const int MEM_RESERVE = 0x00002000;
		const int PAGE_READWRITE = 0x04;
		const uint INFINITE = 0xFFFFFFFF;  // Infinite timeout
		const int MEM_RELEASE = 0x00008000;
		
		[DllImport("kernel32.dll")] //声明API函数
		public static extern int VirtualAllocEx(IntPtr hwnd, int lpaddress, int size, int type, int tect);
		[DllImport("kernel32.dll")]
		public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);
		[DllImport("kernel32.dll")]
		public static extern bool GetExitCodeProcess(IntPtr hProcess, out int lpExitCode);
		[DllImport("kernel32.dll")]
		public static extern int WriteProcessMemory(IntPtr hwnd, int baseaddress, string buffer, int nsize, int filewriten );
		[DllImport("kernel32.dll")]
		public static extern int GetProcAddress(int hwnd, string lpname);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
		[DllImport("kernel32.dll")]
		public static extern int GetModuleHandleA(string name);
		[DllImport("kernel32.dll")]
		public static extern int CreateRemoteThread(IntPtr hwnd, int attrib, int size, int address, int par, int flags, int threadid);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		public static extern bool CreateProcess(StringBuilder lpApplicationName, StringBuilder lpCommandLine, SECURITY_ATTRIBUTES lpProcessAttributes, SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, StringBuilder lpEnvironment, StringBuilder lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, ref PROCESS_INFORMATION lpProcessInformation);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetModuleHandle(string lpModuleName);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress,
		                                        IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		public static extern uint WaitForSingleObject(IntPtr handle, uint dwMilliseconds);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		public static extern uint ResumeThread(IntPtr hThread);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int CloseHandle(IntPtr hObject);

		/// <summary>
		/// 单插件开服
		/// </summary>
		/// <param name="sExePath">服务端应用程序路径</param>
		/// <param name="sDllPath">插件dll所在路径</param>
		/// <returns></returns>
		public static bool start(string sExePath, string sDllPath)
		{
			STARTUPINFO sInfo = new STARTUPINFO();
			PROCESS_INFORMATION pInfo = new PROCESS_INFORMATION();
			bool ret = CreateProcess(null, new StringBuilder(sExePath), null, null, false, CREATE_SUSPENDED, null, null, ref sInfo, ref pInfo);
			if (!ret) {
				Console.WriteLine("CreateProcess ERROR");
				return false;
			}
			IntPtr hndProc = pInfo.hProcess;
			IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			if (lpLLAddress == INTPTR_ZERO)
			{
				Console.WriteLine("GetProcAddress LoadLibraryA ERROR");
				return false;
			}
			IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)sDllPath.Length, (0x1000 | 0x2000), 0X40);
			if (lpAddress == INTPTR_ZERO)
			{
				Console.WriteLine("VirtualAllocEx ERROR");
				return false;
			}
			byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);
			if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
			{
				Console.WriteLine("WriteProcessMemory ERROR");
				return false;
			}
			if (CreateRemoteThread(hndProc, (IntPtr)null, INTPTR_ZERO, lpLLAddress, lpAddress, 0, (IntPtr)null) == INTPTR_ZERO)
			{
				Console.WriteLine("CreateRemoteThread ERROR");
				return false;
			}
			ResumeThread(pInfo.hThread);
			CloseHandle(pInfo.hThread);
			WaitForSingleObject(hndProc, 0xFFFFFFFF);
			CloseHandle(hndProc);
			return true;
		}
		
		/// <summary>
		/// 多插件开服
		/// </summary>
		/// <param name="exepath">服务端应用程序所在路径</param>
		/// <param name="dlldir">插件目录（含斜杆）</param>
		/// <returns></returns>
		public static bool dllsInject(string exepath, string dlldir) {
			int exitcode = 0;
			if (!File.Exists(exepath)) {
				Console.WriteLine("无效的服务端路径，启动失败");
				return false;
			}
			string [] dlls = Directory.GetFiles(dlldir, "*.dll");
			if (dlls == null || dlls.Length < 1) {
				Console.WriteLine("无效的插件目录，启动失败");
				return false;
			}
			STARTUPINFO sInfo = new STARTUPINFO();
			PROCESS_INFORMATION pInfo = new PROCESS_INFORMATION();
			bool ret = CreateProcess(null, new StringBuilder(exepath), null, null, false, CREATE_SUSPENDED, null, null, ref sInfo, ref pInfo);
			if (!ret) {
				Console.WriteLine("CreateProcess ERROR");
				return false;
			}
			IntPtr hndProc = pInfo.hProcess;
			IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			if (lpLLAddress == INTPTR_ZERO)
			{
				Console.WriteLine("GetProcAddress LoadLibraryA ERROR");
				return false;
			}
			// 循环读取路径
			foreach (string sDllPath in dlls) {
				IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)sDllPath.Length, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
				if (lpAddress == INTPTR_ZERO)
				{
					Console.WriteLine("VirtualAllocEx ERROR");
					return false;
				}
				byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);
				if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
				{
					Console.WriteLine("WriteProcessMemory ERROR");
					VirtualFreeEx(hndProc, lpAddress, 0, MEM_RELEASE);
					return false;
				}
				var hRemoteThread = CreateRemoteThread(hndProc, (IntPtr)null, INTPTR_ZERO, lpLLAddress, lpAddress, 0, (IntPtr)null);
				if (hRemoteThread == INTPTR_ZERO)
				{
					Console.WriteLine("CreateRemoteThread ERROR");
					VirtualFreeEx(hndProc, lpAddress, 0, MEM_RELEASE);
					return false;
				}
				WaitForSingleObject(hRemoteThread, INFINITE);
				VirtualFreeEx(hndProc, lpAddress, 0, MEM_RELEASE);
			}
			ResumeThread(pInfo.hThread);
			CloseHandle(pInfo.hThread);
			WaitForSingleObject(hndProc, 0xFFFFFFFF);
			GetExitCodeProcess(hndProc, out exitcode);
			CloseHandle(hndProc);
			return true;
		}
	}
}
