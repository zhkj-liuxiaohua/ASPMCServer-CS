<!DOCTYPE html>
<%@ Page Language="C#"
	AutoEventWireup    = "false"
	CodeFile           = "mccontrol.aspx.cs"
	Inherits           = "ASPMCServer.mccontrol"
	ValidateRequest    = "false"
	EnableSessionState = "true"
 %>
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>MC 后台主控</title>
</head>

<body>
<h1 style="text-align: center">梦故MC主控面板</h1>
<form runat="server">
<p>信息框：<a id="welcome" runat="server"></a>
<input runat="server" id="logout" type="button" value="退出登录" style="right: 2vmin; position: absolute;" /></p>
<div runat="server" id="msg" style="border-style: double; height: 48vh; overflow: scroll;"></div>

<p>添加白名单：<asp:TextBox runat="server" id="whitetext"></asp:TextBox>
<asp:Button runat="server" Text="添加" id="btwhite"></asp:Button>
<input id="showbackup" runat="server" type="button" value="显示备份目录所有文件" style="position: absolute; right: 2vmin"></p>
<p>移除白名单：<asp:TextBox runat="server" id="blacktext"></asp:TextBox>
<asp:Button runat="server" Text="移除" id="btblack"></asp:Button>
<input id="clearbackup" runat="server" type="button" value="清理备份目录至最近10个(慎重)" style="position: absolute; right: 2vmin"></p>
<p><asp:Button runat="server" Text="一键显示后台信息(最多20条)" id="showmc"></asp:Button>
　<asp:Button runat="server" Text="一键显示往期log信息(最多2000条)" id="showlog"></asp:Button>
　<a href="events.aspx">前往监控页</a>
<input id="cpmap" runat="server" type="button" value="复制地图至ftp" style="position: absolute; right: 2vmin"></p>

<p>发送后台指令：<asp:TextBox runat="server" id="cmdtext"></asp:TextBox>
<asp:Button runat="server" Text="发送" id="btcmd"></asp:Button>
<a style="position: absolute; right: 2vmin;" href="ftp://game.xiafox.com">连接ftp://game.xiafox.com</a>
</p>

<p style="text-align: center"><asp:Button runat="server" Text="关闭服务端(服务器需禁用配置的重启计划任务)" id="shutdown"></asp:Button></p>
<p style="text-align: center" ><asp:Button runat="server" Text="由我开服(自动重启时限为10秒)" id="StartServer"></asp:Button></p>
</form>

</body>

</html>
