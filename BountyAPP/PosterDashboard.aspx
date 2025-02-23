<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PosterDashboard.aspx.cs" Inherits="BountyAPP.PosterDashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>Poster Dashboard</title>
    <link rel="stylesheet" href="StyleSheet.css" / >
</head>
<body>
    <h2>Welcome, Problem Posters</h2>
    <form runat="server">
        <asp:Label runat="server" Text="Problem Title:" /><br />
        <asp:TextBox ID="TextBox1" runat="server" /><br />
        <asp:Label runat="server" Text="Description:" /><br />
        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" /><br />
        <asp:Label runat="server" Text="Bounty Amount($):" /><br />
        <asp:TextBox ID="TextBox3" runat="server" /><br />
        <asp:Button ID="btnPostProblem" runat="server" Text="Post" OnClick="btnPostProblem_Click" /><br /><br />
        
        <h3>Your Problems</h3>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:BoundField DataField="ProblemID" HeaderText="ProblemID" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Bounty" HeaderText="Bounty" />
                <asp:BoundField DataField="SolverEmail" HeaderText="SolverEmail" />
                <asp:BoundField DataField="Solved" HeaderText="Solved" />
                <asp:BoundField DataField="Solution" HeaderText="Solution" />
                <asp:BoundField DataField="Paid" HeaderText="Paid" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnApprove" runat="server" Text="Approve"
                            CommandName="Approve" 
                            CommandArgument='<%# Eval("ProblemID") %>'
                            Visible='<%# Convert.ToBoolean(Eval("Solved")) == false %>' />
                        <asp:Button ID="btnReject" runat="server" Text="Reject"
                            CommandName="Reject" 
                            CommandArgument='<%# Eval("ProblemID") %>'
                            Visible='<%# Convert.ToBoolean(Eval("Solved")) == false %>' />
                        <asp:Button ID="btnPaySolver" runat="server" Text="Pay Solver"
                            CommandName="PaySolver" 
                            CommandArgument='<%# Eval("ProblemID") %>'
                            Visible='<%# Convert.ToBoolean(Eval("Solved")) == true && Convert.ToBoolean(Eval("Paid")) == false %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>