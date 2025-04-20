<%@ Page Title="Member Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" Inherits="LibraryManagementSystem.Member.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome, <%: Context.User.Identity.Name %></h2>
    <p>This is your member dashboard.</p>
    <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="btn btn-danger" />
</asp:Content>