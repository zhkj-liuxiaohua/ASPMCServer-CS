/*
 * 由SharpDevelop创建。
 * 用户： classmates
 * 日期: 2020/3/8
 * 时间: 8:59
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace ASPMCServer
{
	/// <summary>
	/// Description of LoginCheck.
	/// </summary>
	public class LoginCheck
	{
		public LoginCheck()
		{
		}
		
		static Mutex mban = new Mutex(), mlog = new Mutex();
		
		// 是否已处于黑名单列表上
		public static bool checkIsBan(string f, string ip) {
			bool ret = false;
			mban.WaitOne();
			try{
				string [] ips = File.ReadAllLines(f);
				foreach(string ipdata in ips) {
					string [] dat = ipdata.Split(',');
					if(dat[0].Equals(ip)) {
						if (int.Parse(dat[1]) > 4) {
							if (DateTime.Now.Date.ToString().Equals(dat[2])) {
								ret = true;
								break;
							}
						}
					}
				}
			} catch{
			}
			mban.ReleaseMutex();
			return ret;
		}

		// 添加一次失败验证次数
		public static void failOne(string f, string ip) {
			mban.WaitOne();
			Hashtable iplist = IPDATA.getAllData(f);
			Object o = iplist[ip];
			if (o != null) {
				IPDATA od = (IPDATA)o;
				DateTime ndt = DateTime.Now.Date;
				if (!od.dt.Equals(ndt))
					od.failTimes = 0;
				++od.failTimes;
				od.dt = ndt;
			} else {
				o = iplist[ip] = new IPDATA();
				IPDATA od = (IPDATA)o;
				od.ip = ip;
				++od.failTimes;
				od.dt = DateTime.Now.Date;
			}
			IPDATA.saveAllData(f, iplist);
			mban.ReleaseMutex();
		}
		
		// 获取已失败次数
		public static int getFailTimes(string f, string ip) {
			int times = 0;
			mban.WaitOne();
			Hashtable iplist = IPDATA.getAllData(f);
			Object o = iplist[ip];
			if (o != null) {
				IPDATA od = (IPDATA)o;
				times = od.failTimes;
			}
			mban.ReleaseMutex();
			return times;
		}
		
		// 解锁
		public static void clear(string f, string ip) {
			mban.WaitOne();
			Hashtable iplist = IPDATA.getAllData(f);
			Object o = iplist[ip];
			if (o != null) {
				IPDATA od = (IPDATA)o;
				od.failTimes = 0;
				od.dt = DateTime.Now.Date;
			}
			IPDATA.saveAllData(f, iplist);
			mban.ReleaseMutex();
		}
		
		// 写入一个登录信息log
		public static void addLoginLog(string f, string log) {
			mlog.WaitOne();
			try {
				File.AppendAllLines(f, new string[]{log});
			}catch{}
			mlog.ReleaseMutex();
		}
	}
	
	public class IPDATA {
		public string ip;
		public int failTimes;
		public DateTime dt;
		/// <summary>
		/// 初始化一个空值
		/// </summary>
		public IPDATA() {
		}
		/// <summary>
		/// 序列化一条信息
		/// </summary>
		/// <param name="ipdata">分割读值</param>
		public IPDATA(string ipdata) {
			string [] dat = ipdata.Split(',');
			if (dat.Length == 3) {
				ip = dat[0];
				failTimes = int.Parse(dat[1]);
				dt = DateTime.Parse(dat[2]);
			}
		}
		/// <summary>
		/// 格式化输出
		/// </summary>
		/// <returns></returns>
		public string toString() {
			return ip + ',' + failTimes + ',' + dt;
		}
		/// <summary>
		/// 返回一个表
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Hashtable getAllData(string f) {
			Hashtable t = new Hashtable();
			try{
				string [] ips = File.ReadAllLines(f);
				foreach(string data in ips) {
					IPDATA a = new IPDATA(data);
					t[a.ip] = a;
				}
			} catch{
			}
			return t;
		}
		/// <summary>
		/// 保存一个表
		/// </summary>
		/// <param name="f"></param>
		/// <param name="t"></param>
		public static void saveAllData(string f, Hashtable t) {
			string saveStr = "";
			foreach (IPDATA v in t.Values) {
				saveStr += (v.toString() + "\n");
			}
			try {
				File.WriteAllText(f, saveStr);
			}catch{}
		}
	}
}
