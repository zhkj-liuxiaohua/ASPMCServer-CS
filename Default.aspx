<%@ Page
	Language           = "C#"
	AutoEventWireup    = "false"
	Inherits           = "ASPMCServer.Default"
	ValidateRequest    = "false"
	EnableSessionState = "true"
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>登录</title>

<style type="text/css">
.stitle {
				text-align: center;
				font-size:8vmin;
}

.smain {
				font-size:6vmin;
}

.sbox {
				font-size:6vmin;
				width:60vmin;
}

.smsg {
				text-align: center;
				color: #FF0000;
				font-size: 5vmin;
}
</style>

</head>

<body>
<div style="text-align: center; width: 100%" ><a class="stitle">梦之故里游戏服控制面板</a></div>
<form id="Form1" method="post" style="text-align: center" runat="server">
<p class="smain">用户名：<input id="fname" type="text" runat="server" class="sbox"/></p>
<p class="smain">密　码：<input id="fpass" type="password" runat="server" class="sbox"/></p>
<p><input id="submit1" type="submit" value="登录" class="smain" runat="server"/></p>
<a class="smain" href="editpass.aspx" >修改密码</a>
</form>
<p id="msg" class="smsg" runat="server">
</p>
</body>

</html>
