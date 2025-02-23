<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="BountyAPP.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="StyleSheet.css" / >
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Registration</h2>
            Name: </br> <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></br>
            Email:</br><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></br>
            Password:</br><asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox></br>
            Role:</br><asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Text="Problem Poster" Value="Poster"> </asp:ListItem>
                <asp:ListItem Text="Problem Solver" Value="Solver"> </asp:ListItem>
            </asp:DropDownList></br>
            </br>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label> <%-- Added lblMessage here --%>
            </br>
            <asp:Button ID="Button1" runat="server" Text="Register" OnClick="Button1_Click" />
            </br>
            </br> Already have an account ?? <a href="login.aspx">Login here </a>
        </div>
    </form>
</body>
</html>