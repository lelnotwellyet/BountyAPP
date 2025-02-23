<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitSolution.aspx.cs" Inherits="BountyAPP.SubmitSolution" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <link rel="stylesheet" href="StyleSheet.css" / >
    <title>Submit Solution</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Submit Solution</h2>
            <asp:Label ID="lblProblemTitle" runat="server" Text="Problem Title: "></asp:Label><br /><br />
            
            <asp:Label ID="lblDescription" runat="server" Text="Problem Description: "></asp:Label><br /><br />

            <asp:TextBox ID="txtSolution" runat="server" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox><br /><br />
            
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Solution" OnClick="btnSubmit_Click" />
        </div>
    </form>
</body>
</html>
