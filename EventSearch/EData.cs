/*
 * 由SharpDevelop创建。
 * 用户： Admin
 * 日期: 2019/9/12
 * 时间: 10:30
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections;
using System.IO;

namespace EventSearch
{
	/// <summary>
	/// Description of EData.
	/// </summary>
	public class EData
	{
		public ArrayList datas;
		public ArrayList events;
		
		public delegate void OnInitFinish();
		public OnInitFinish f;
		
		public Hashtable oldPoint;
		
		public EData()
		{
		}
		
		/// <summary>
		/// 设置初始化结束监听
		/// </summary>
		/// <param name="onif">监听回调</param>
		public void setOnInitFinish(OnInitFinish onif){
			f = onif;
		}
		
		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="dir">目标数据所在目录</param>
		public void init(string dir) {
			datas = new ArrayList();
			events = new ArrayList();
			oldPoint = new Hashtable();
			string [] txts = null;
			try {
				txts = Directory.GetFiles(dir, "*.txt");
			} catch{
			}
			if (txts != null && txts.Length > 0) {
				foreach(string txt in txts) {
					string [] edata = File.ReadAllLines(txt);
					datas.AddRange(edata);
				}
			}
			if (datas.Count > 0) {
				for (int t = 0, l = datas.Count; t < l; t++) {
					string d = (string)datas[t];
					MGEvent ev = selectFromTxt(d);
					ev.tindex = t;
					events.Add(ev);
				}
			}
			if(f != null) {
				f();
			}
		}
		
		// 转换文本至索引
		private MGEvent selectFromTxt(string d) {
			MGEvent ev = new MGEvent();
            ev.x = int.MaxValue;
            ev.y = int.MaxValue;
            ev.z = int.MaxValue;
			int i = d.IndexOf('[');
			int j = d.IndexOf(']');
			if (i > -1) {
				if (j > i) {
					string dandt = d.Substring(i + 1, j - i - 1);
					string [] dtt = dandt.Split(' ');
					if (dtt.Length == 3) {
						ev.dt = DateTime.Parse(dtt[0] + " " + dtt[1]);
						ev.title = dtt[2];
					}
				}
			} else
				return ev;
			string nextNode = null;
			i = d.IndexOf("玩家");
			if (i > -1) {
				nextNode = d.Substring(i);
                int pnodeNums = 0;
				string [] pinfos = nextNode.Split(' ');
				bool oped = false, closed = false;
				if (pinfos.Length > 0) {
					if (pinfos[0] == "玩家") {
                        ev.player = "";
                        while (pinfos[pnodeNums + 1][0] < 127 && pnodeNums < pinfos.Length - 1) {
                            ev.player += (pinfos[1 + pnodeNums] + " ");
                            ++pnodeNums;
                        }
                        ev.player = ev.player.Trim();
					}
					if (pinfos[pnodeNums + 1] == "悬空地") {
						ev.isFloat = true;
					} else if (pinfos[pnodeNums + 1] == "改变维度至") {
						ev.dimension = pinfos[pnodeNums + 2];
					} else {
						string [] opi = pinfos[pnodeNums + 1].Split(new String [] {"在"}, StringSplitOptions.RemoveEmptyEntries);
						if (opi.Length > 1) {
							ev.dimension = opi[1];
							oped = opi[0].Equals("开启");
							closed = opi[0].Equals("关闭");
						}
					}
				}
				int points = nextNode.IndexOf('(');
				if (points > -1) {
					int pointe = nextNode.IndexOf(')');
					if (pointe > points) {
						string pointstr = nextNode.Substring(points + 1, pointe - points - 1);
						string [] ps = pointstr.Split(',');
						if (ps.Length > 2) {
							ev.x = int.Parse(ps[0]);
							ev.y = int.Parse(ps[1]);
							ev.z = int.Parse(ps[2]);
							if (oped) {
								oldPoint[ev.player] = new P3D(ev.x, ev.y, ev.z);
							}
							if (closed) {
								oldPoint[ev.player] = null;
							}
						}
					}
				} else {
					if (!string.IsNullOrEmpty(ev.player)) {
						Object ooldp = oldPoint[ev.player];
						if (null != ooldp) {
							P3D oldp = (P3D) ooldp;
							ev.x = oldp.x;
							ev.y = oldp.y;
							ev.z = oldp.z;
						}
					}
				}
			}
			return ev;
		}
		
		/// <summary>
		/// 查询接口
		/// </summary>
		/// <param name="dts">时间戳</param>
		/// <param name="xs">X间隔</param>
		/// <param name="ys">Y间隔</param>
		/// <param name="zs">Z间隔</param>
		/// <param name="titles">事件类型</param>
		/// <param name="xboxid">玩家xboxID</param>
		/// <returns>结果集</returns>
		public ArrayList search(DateTime [] dts, int [] xs, int [] ys, int [] zs, ArrayList titles, string xboxid) {
			if (titles.Count < 1) {
				return null;
			}
			ArrayList result = new ArrayList();
			foreach(MGEvent ev in events) {
				if (checkTitles(ev, titles))
					if (checkDT(ev, dts))
						if (checkXs(ev, xs))
							if (checkYs(ev, ys))
								if (checkZs(ev, zs))
									if (checkXboxID(ev, xboxid))
										result.Add(ev);
			}
			if (result != null && result.Count > 0) {
				ArrayList strs = new ArrayList();
				foreach (MGEvent ev in result) {
					strs.Add(datas[ev.tindex]);
				}
				return strs;
			}
			return null;
		}
		
		private bool checkDT(MGEvent ev, DateTime [] dts) {
			return (dts == null || (dts != null && (ev.dt >= dts[0] && ev.dt <= dts[1])));
		}
		private bool checkXs(MGEvent ev, int [] xs) {
			return (xs == null || (xs != null && (ev.x >= xs[0] && ev.x <= xs[1])));
		}
		private bool checkYs(MGEvent ev, int [] ys) {
			return (ys == null || (ys != null && (ev.y >= ys[0] && ev.y <= ys[1])));
		}
		private bool checkZs(MGEvent ev, int [] zs) {
			return (zs == null || (zs != null && (ev.z >= zs[0] && ev.z <= zs[1])));
		}
		private bool checkTitles(MGEvent ev, ArrayList titles) {
			if (titles != null && titles.Count > 0) {
				if (titles.Contains(ev.title)) {
					return true;
				}
			}
			return false;
		}
		private bool checkXboxID(MGEvent ev, string id) {
			return (id == null || (id != null && ev.player == id));
		}
	}
	
	public class MGEvent {
		public int tindex;
		public DateTime dt;
		public string title;
		public string player;
		public bool isFloat;
		public string dimension;
		public int x;
		public int y;
		public int z;
	}
	
	public struct P3D {
		public int x;
		public int y;
		public int z;
		public P3D(int x, int y, int z){
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
	
}
