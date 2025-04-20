<%@ Page Title="Try Search Service" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchTryIt.aspx.cs" Inherits="LibraryManagementSystem.Service_Layer.Book_Search_Service.SearchTryIt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Try Search Service</h2>

    <div class="service-section">
        <h3>Basic Search</h3>
        <asp:TextBox ID="txtSearchQuery" runat="server" placeholder="Enter search term"></asp:TextBox>
        <asp:Button ID="btnBasicSearch" runat="server" Text="Search" OnClick="btnBasicSearch_Click" />
    </div>

    <div class="service-section">
        <h3>Advanced Search</h3>
        <div>
            <label>Title:</label>
            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Author:</label>
            <asp:TextBox ID="txtAuthor" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Category:</label>
            <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Year:</label>
            <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnAdvancedSearch" runat="server" Text="Advanced Search" OnClick="btnAdvancedSearch_Click" />
    </div>

    <div class="service-section">
        <h3>Other Features</h3>
        <asp:Button ID="btnGetPopular" runat="server" Text="Get Popular Books" OnClick="btnGetPopular_Click" />
        <asp:Button ID="btnGetCategories" runat="server" Text="Get All Categories" OnClick="btnGetCategories_Click" />
    </div>

    <div class="results">
        <h4>Results:</h4>
        <asp:Literal ID="litResults" runat="server" />
    </div>
</asp:Content>