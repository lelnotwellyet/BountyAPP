<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="BountyAPP.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="StyleSheet.css" / >
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login page</h2>
            Email:</br><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></br>
            Password:</br><asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox></br>
            </br><asp:Button ID="Button1" runat="server" Text="Login" OnClick="btnLogin_Clicked" /></buton>
            </br> New User ? <a href="registration.aspx" >Register</a>

        </div>
    </form>
</body>
</html>
