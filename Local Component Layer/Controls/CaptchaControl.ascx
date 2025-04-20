<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="LibraryManagementSystem.Local_Component_Layer.Controls.CaptchaControl" %>

<div class="captcha-control form-group">
    <div class="d-flex align-items-center mb-2">
        <asp:Image ID="imgCaptcha" runat="server" CssClass="img-fluid" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" 
            CssClass="btn btn-sm btn-outline-secondary ml-2" CausesValidation="false" />
    </div>
    <asp:Label runat="server" AssociatedControlID="txtCaptcha" CssClass="control-label">Enter CAPTCHA code:</asp:Label>
    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCaptcha"
        CssClass="text-danger" ErrorMessage="CAPTCHA code is required." Display="Dynamic" />
</div>
