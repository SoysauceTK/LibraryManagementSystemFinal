<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookStorageTryIt.aspx.cs" Inherits="LibraryManagementSystem.Services.BookStorageTryIt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Try a Service</h2>
    <asp:DropDownList ID="ddlService" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlService_SelectedIndexChanged">
        <asp:ListItem Text="Select Service" Value="" />
        <asp:ListItem Text="GetAllBooks" Value="GetAllBooks" />
        <!-- Add more service ops here -->
    </asp:DropDownList>
    <asp:Button ID="btnCallService" runat="server" Text="Call Service" OnClick="btnCallService_Click" />
    <br /><br />
    <asp:Literal ID="litResult" runat="server" />
</asp:Content>
