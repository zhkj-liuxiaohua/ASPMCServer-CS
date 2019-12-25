/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/12/25
 * 时间: 15:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Web;

namespace ASPMCServer
{
	/// <summary>
	/// Description of farcmd
	/// </summary>
	public class farcmd : IHttpHandler
	{	

		public static string PIPEMSGTAG = System.Web.Configuration.WebConfigurationManager.AppSettings["PIPEMSGTAG"];
		
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
		/// 发送远程消息
		/// </summary>
		/// <param name="msg">完整信息</param>
		/// <returns></returns>
		public static string sendFarMessage(string msg) {
			string cmd = TAG_CMD + "," + msg;
			try {
				NamedPipeClientStream pipeOut = new NamedPipeClientStream(".", PIPEMSGTAG, PipeDirection.InOut);
				pipeOut.Connect(2000);
				StreamWriter writer = new StreamWriter(pipeOut);
				writer.AutoFlush = true;
				writer.WriteLine(cmd);
				pipeOut.Close();
				return "消息" + msg + "已发送。";
			}catch{
			}
			return "未能发送至指定端口。";
		}
		
		private string decode(string p)
		{
			try {
			byte [] a = new byte[p.Length / 2];
			string sub = null;
			for (int i = 0, l = p.Length; i < l; i+=2) {
				sub = p.Substring(i, 2);
				a[i/2] = (byte)(Convert.ToByte(sub, 16) ^ 0x22);
			}
			return Encoding.Default.GetString(a);
			} catch (Exception e) {
				return e.Message;
			}
		}
		
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#region Process Request
		public void ProcessRequest(HttpContext context)
		{
			string user = context.Request.QueryString["user"];
			string password = context.Request.QueryString["password"];
			string cmd = context.Request.QueryString["cmd"];
			
			string result = "出错:" + "user=" + user + ",password=" + password + ",cmd=" + cmd;
			try {
				string DATA_DIR = System.Web.Configuration.WebConfigurationManager.AppSettings["DATA_DIR"];
				string fi = DATA_DIR + user + ".txt";
				String p = File.ReadAllText(fi);
				p = decode(p);
				if (!p.Equals(password)) {
					result = "用户名或密码错误";
				} else {
					result = sendFarMessage(cmd);
				}
			} catch{
			}
			
			context.Response.ContentType = "text/plain";
			context.Response.Write(result);
			context.Response.Flush();
			context.Response.Close();
		}
		#endregion
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		public bool IsReusable
		{
			get { return true; }
		}

	}
}
