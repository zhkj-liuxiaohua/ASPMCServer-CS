/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2019/3/19
 * 时间: 18:55
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace MCSLauncher
{
	/// <summary>
	/// MC开服用启动器
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			string pname = null, ppath = null, logpath = null;
			if (args.Length > 2) {
				pname = args[0];
				ppath = args[1];
				logpath = args[2];
			}
			if (!String.IsNullOrEmpty(pname) && !String.IsNullOrEmpty(ppath) && !String.IsNullOrEmpty(logpath))
				mccontrol.StartProc(pname, ppath, logpath);
		}
	}
}
