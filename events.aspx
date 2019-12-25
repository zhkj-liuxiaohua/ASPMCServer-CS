<!DOCTYPE html>
<%@ Page
	Language           = "C#"
	AutoEventWireup    = "false"
	Inherits           = "ASPMCServer.Events"
	ValidateRequest    = "false"
	EnableSessionState = "true"
%>
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>监控信息</title>
</head>

<body>
<h1 align="center">梦故玩家监控信息</h1>
<p><a href="#" onclick="history.back()">返回上一页</a></p>
<div runat="server" id="msg" style="border-style: double; height: 55vh; overflow: scroll; white-space: pre;"></div>
<p><a href="http://game.xiafox.com/gamelogs/">查看往期log存放信息</a></p>
</body>

</html>
