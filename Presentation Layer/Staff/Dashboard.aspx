<%@ Page Title="Staff Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LibraryManagementSystem.Staff.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome, <%: Context.User.Identity.Name %></h2>
    <p>This is your Staff dashboard.</p>
</asp:Content>
