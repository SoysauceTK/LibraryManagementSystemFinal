<%@ Page Title="Staff Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LibraryManagementSystem.Presentation_Layer.Staff.Login" %>
<%@ Register Src="~/Local Component Layer/Controls/CaptchaControl.ascx" TagPrefix="uc" TagName="CaptchaControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6 mx-auto">
            <div class="card">
                <div class="card-header bg-secondary text-white">
                    <h2><%: Title %></h2>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Username" CssClass="control-label">Username</asp:Label>
                        <asp:TextBox runat="server" ID="Username" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                            CssClass="text-danger" ErrorMessage="Username is required." />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                            CssClass="text-danger" ErrorMessage="Password is required." />
                    </div>
                    <div class="form-group">
                        <uc:CaptchaControl runat="server" ID="StaffCaptcha" />
                    </div>
                    <div class="form-group">
                        <div class="checkbox">
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-secondary" />
                    </div>
                    <div class="form-group">
                        <asp:Literal runat="server" ID="FailureText" />
                    </div>
                    <div class="alert alert-info">
                        <p>For testing purposes, use: Username: "TA", Password: "Cse445!"</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>