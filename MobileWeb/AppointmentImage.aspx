﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppointmentImage.aspx.cs" Inherits="MobileWeb.AppointmentImage" %>
<%@ Import namespace="OpenDentBusiness.Mobile" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Appointment</title>
</head>
<body>
<div id="loggedin"><asp:Literal runat="server" ID="Message"></asp:Literal></div>
<div id="content">
<div class="styleError">  
				 <asp:Label ID="LabelError" runat="server" Text=""></asp:Label>
</div>

<img src="app.jpg" width="724px" height="662px" usemap="#immap" />
					<map name="immap" id="immap" style="position: relative;">
					<area shape="rect" coords="272, 84, 369, 172" onclick="areaClicked('AppointmentDetails.aspx?AptNum=22')" alt="box1__"  />
					<area shape="rect" coords="375, 557, 482, 622" onclick="areaClicked('AppointmentDetails.aspx?AptNum=23')" alt="box2__"  />
					</map>

</div>

</body>
</html>