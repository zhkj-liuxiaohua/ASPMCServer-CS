/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2018/9/16
 * 时间: 14:56
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MCSLauncher.net
{
	/// <summary>
	/// 网络连接器
	/// </summary>
	public class P2PLoader
	{
		/// <summary>
		/// 本地发信端口
		/// </summary>
		public static int localport = 0;
		
		private Socket socket = null;
		private bool stoped = true;
		
		public P2PLoader(int port)
		{
			//得到本机IP，设置TCP端口号
			localport = port;
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, localport);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定网络地址
            socket.Bind(ip);
		}
		
		/// <summary>
		/// 开启监听
		/// </summary>
		public void startListen() {
			if (stoped) {
				stoped = false;
				Thread t = new Thread(delegate () {
					while (!stoped) {
						try {
							EndPoint Remote = new IPEndPoint(IPAddress.Any, localport);
							byte[] data = new byte[1024];
							int recv = socket.ReceiveFrom(data, ref Remote);
							String msg = Encoding.Default.GetString(data, 0, recv);
							if (doOnReceiveMsg != null) {
								doOnReceiveMsg(Remote.ToString(), msg);
							}
						} catch {
						}
					}
				});
				t.Start();
			}
		}
		
		/// <summary>
		/// 停止监听
		/// </summary>
		public void stopListen() {
			stoped = true;
			try {
				socket.Close();
			} catch{
			}
			if (doOnStopListen != null) {
				doOnStopListen();
			}
		}
		
		/// <summary>
		/// 发送远程消息
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		/// <param name="msg"></param>
		public void sendMsg(string ip, int port, string msg) {
			byte [] data = System.Text.Encoding.Default.GetBytes(msg);
			EndPoint Remote = new IPEndPoint(IPAddress.Parse(ip), port);
			try {
				socket.SendTo(data, data.Length, SocketFlags.None, Remote);
			} catch(SocketException) {
				// do nothing
			}
		}
		
		/// <summary>
		/// 设置停止监听
		/// </summary>
		/// <param name="l"></param>
		public void setOnStopListen(DoOnStopListen l) {
			this.doOnStopListen = l;
		}
		
		/// <summary>
		/// 设置消息监听
		/// </summary>
		/// <param name="l"></param>
		public void setOnReceiveMsg(DoOnReceiveMsg l) {
			this.doOnReceiveMsg = l;
		}
		
		public delegate void DoOnStopListen();
		public delegate void DoOnReceiveMsg(string ipep, string msg);
		
		private DoOnStopListen doOnStopListen = null;
		private DoOnReceiveMsg doOnReceiveMsg = null;
	}
	
	
	
}
