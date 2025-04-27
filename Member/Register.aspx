<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LibraryManagementSystem.Member.Register" %>
<%@ Register TagPrefix="uc" TagName="Captcha" Src="~/Controls/CaptchaControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-8 mx-auto">
            <div class="card">
                <div class="card-header">
                    <h2><%: Title %></h2>
                    <p class="text-muted">Create a new account to access member features.</p>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Username" CssClass="control-label">Username</asp:Label>
                        <asp:TextBox runat="server" ID="Username" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                            CssClass="text-danger" ErrorMessage="Username is required." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="control-label">Email</asp:Label>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                            CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="Email"
                            CssClass="text-danger" ErrorMessage="Please enter a valid email address."
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                            CssClass="text-danger" ErrorMessage="Password is required." Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="Password"
                            CssClass="text-danger" ErrorMessage="Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$" Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="control-label">Confirm Password</asp:Label>
                        <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" ErrorMessage="Confirm password is required." Display="Dynamic" />
                        <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" ErrorMessage="The password and confirmation password do not match." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="control-label">Verification Code</asp:Label>
                        <uc:Captcha ID="RegisterCaptcha" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" ID="RegisterButton" Text="Register" CssClass="btn btn-primary" OnClick="RegisterButton_Click" />
                    </div>
                    <div class="form-group">
                        <asp:Literal runat="server" ID="StatusMessage" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>