<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryCaptcha.aspx.cs" Inherits="LibraryManagementSystem.Controls.TryCaptcha" %>
<%@ Register TagPrefix="lms" TagName="Captcha" Src="~/Controls/CaptchaControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Try Captcha Control</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2>Try Captcha Control</h2>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Captcha Verification</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <lms:Captcha ID="captchaControl" runat="server" />
                    </div>
                    <div class="form-group">
                        <label for="txtCaptchaInput">Enter Captcha Text:</label>
                        <asp:TextBox ID="txtCaptchaInput" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnVerifyCaptcha" runat="server" Text="Verify Captcha" CssClass="btn btn-primary" OnClick="btnVerifyCaptcha_Click" />
                    <asp:Button ID="btnRefreshCaptcha" runat="server" Text="Refresh Captcha" CssClass="btn btn-secondary" OnClick="btnRefreshCaptcha_Click" />
                </div>
            </div>
            
            <asp:Panel ID="pnlResults" runat="server" CssClass="alert" Visible="false">
                <asp:Literal ID="litResults" runat="server"></asp:Literal>
            </asp:Panel>
            
            <div class="mt-4">
                <a href="../Default.aspx" class="btn btn-secondary">Back to Home</a>
            </div>
        </div>
    </form>
</body>
</html>