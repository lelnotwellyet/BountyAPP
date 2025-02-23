<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmOTP.aspx.cs" Inherits="BountyAPP.ConfirmOTP" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Confirm OTP</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="d-flex justify-content-center align-items-center vh-100 bg-light">
    <form id="form1" runat="server" class="p-4 bg-white shadow rounded" style="max-width: 400px; width: 100%;">
        <h3 class="text-center">Confirm OTP</h3>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block text-center mb-3"></asp:Label>

        <div class="mb-3">
            <label for="txtOTP" class="form-label">Enter OTP</label>
            <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" MaxLength="6" placeholder="Enter OTP" required></asp:TextBox>
        </div>

        <asp:Button ID="btnConfirm" runat="server" Text="Verify OTP" CssClass="btn btn-primary w-100" OnClick="btnConfirm_Click" />

        <p class="text-center mt-3">
            Didn't receive OTP? <a href="ResendOTP.aspx">Resend</a>
        </p>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
