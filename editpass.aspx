<!DOCTYPE html>
<%@ Page
	Language           = "C#"
	AutoEventWireup    = "false"
	Inherits           = "ASPMCServer.editpass"
	ValidateRequest    = "false"
	EnableSessionState = "false"
%>
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>修改密码</title>

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
<div style="text-align: center; width: 100%" ><a class="stitle">修改密码</a></div>
<a href="Default.aspx" class="smain">返回</a>
<form id="form1" runat="server" style="text-align: center">
<p class="smain">用户名：<input type="text" runat="server" id="username" class="sbox"/>
</p>
<p class="smain">旧密码：<input id="oldpass" runat="server" type="password" class="sbox" /></p>
<p class="smain">新密码：<input runat="server" type="password" id="newpass" class="sbox" /></p>
<p class="smain">再确认：<input runat="server" type="password" id="newpass2" class="sbox"/></p>
<div><input runat="server" type="submit" value="确认修改" id="submit" class="smain" /></div>
</form>
<div id="msg" runat="server" class="smsg"></div>
</body>

</html>
