<%@ Page Title="Try CAPTCHA" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TryCaptcha.aspx.cs" Inherits="LibraryManagementSystem.Controls.TryCaptcha" %>
<%@ Register TagPrefix="uc" TagName="Captcha" Src="~/Controls/CaptchaControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>CAPTCHA Test Page</h1>
        <p class="lead">This page demonstrates the CAPTCHA user control.</p>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3>Try the CAPTCHA</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label>CAPTCHA Image:</label>
                        <uc:Captcha ID="testCaptcha" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="CaptchaInput">Enter the code above:</asp:Label>
                        <asp:TextBox ID="CaptchaInput" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CaptchaInput"
                            CssClass="text-danger" ErrorMessage="CAPTCHA input is required." Display="Dynamic" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="VerifyButton" runat="server" Text="Verify" CssClass="btn btn-primary" OnClick="VerifyButton_Click" />
                    </div>
                    <div class="form-group">
                        <asp:Literal ID="ResultLiteral" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3>About This Component</h3>
                </div>
                <div class="card-body">
                    <p>This CAPTCHA user control is implemented with the following features:</p>
                    <ul>
                        <li>Random code generation with a mix of letters and numbers</li>
                        <li>Distortion and noise to prevent automated reading</li>
                        <li>Session-based verification</li>
                        <li>Refresh capability to generate new codes</li>
                    </ul>
                    <p>It's implemented as a reusable ASP.NET User Control that can be added to any form that requires CAPTCHA verification.</p>
                    <p><strong>Developer:</strong> Sawyer Kesti</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>