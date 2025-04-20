<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="LibraryManagementSystem.Controls.CaptchaControl" %>

<div class="captcha-control">
    <asp:Image ID="imgCaptcha" runat="server" />
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" CssClass="btn btn-sm btn-outline-secondary" />
</div>
