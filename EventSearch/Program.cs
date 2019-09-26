/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/9/10
 * 时间: 23:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace EventSearch
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			if (args.Length > 0) {
				MainForm.setDir(args[0]);
			} else {
				MainForm.setDir(".");
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}
