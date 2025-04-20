<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LibraryManagementSystem.Presentation_Layer.Member.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6 mx-auto">
            <div class="card">
                <div class="card-header">
                    <h2>Register</h2>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Username">Username</asp:Label>
                        <asp:TextBox runat="server" ID="Username" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Username" CssClass="text-danger" ErrorMessage="Username is required." />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Password is required." />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email">Email</asp:Label>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" ID="btnRegister" Text="Register" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                    </div>
                    <asp:Literal ID="litMessage" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
