<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PosterDashboard.aspx.cs" Inherits="BountyAPP.PosterDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2> Welcome , Problem Posters</h2>
            Problem Title:</br><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></br>
            Description:</br> <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine"></asp:TextBox></br>
            Bounty Amount($):</br><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></br>
            </br><asp:Button ID="Button1" runat="server" Text="Post" OnClick="btnPostProblem_Click" /></br>
            <h3>Your Problems</h3>
            </br><asp:GridView ID="GridView1" runat="server"></asp:GridView></br>
        </div>
    </form>
</body>
</html>
