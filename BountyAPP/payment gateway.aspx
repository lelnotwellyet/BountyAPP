<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payment gateway.aspx.cs" Inherits="BountyAPP.payment_gateway" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet"href="StyleSheet.css"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div><p align="center"><b>Payment</b></p>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="Yes" OnClick="Button1_Click" /><asp:Button ID="Button2" runat="server" Text="No" /><br/>
            </div>
    </form>
</body>
</html>
