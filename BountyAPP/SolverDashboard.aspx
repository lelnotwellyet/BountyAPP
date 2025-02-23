<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolverDashboard.aspx.cs" Inherits="BountyAPP.SolverDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <title>Solver Dashboard</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .grid-view th, .grid-view td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }
        .grid-view th {
            background-color: #f4f4f4;
        }
        .btn-claim {
            background-color: #4CAF50;
            color: white;
            padding: 5px 10px;
            border: none;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Solver Dashboard</h2>
        <div>Logged in as: <asp:Label ID="lblUserEmail" runat="server"></asp:Label></div>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="grid-view" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ProblemID" HeaderText="Problem ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Bounty" HeaderText="Bounty" DataFormatString="£{0:N2}" />
                <asp:BoundField DataField="ClaimedBy" HeaderText="Claimed By" />
                <asp:BoundField DataField="Solved" HeaderText="Solved" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnClaim" runat="server" 
                            Text="Claim Problem" 
                            CommandArgument='<%# Eval("ProblemID") %>' 
                            OnClick="btnClaim_Click" 
                            Visible='<%# string.IsNullOrEmpty(Eval("ClaimedBy")?.ToString()) %>'
                            CssClass="btn-claim" />
                        <asp:Button ID="btnSubmitSolution" runat="server" 
            Text="Submit Solution" 
            CommandArgument='<%# Eval("ProblemID") %>' 
            OnClick="btnSubmitSolution_Click" 
            Visible='<%# Eval("ClaimedBy")?.ToString() == Session["UserEmail"]?.ToString() %>'
            CssClass="btn-claim" />


                    </ItemTemplate>

                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>