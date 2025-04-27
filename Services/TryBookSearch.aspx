<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryBookSearch.aspx.cs" Inherits="LibraryManagementSystem.Services.TryBookSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Try Book Search Service</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2>Try Book Search Service</h2>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Basic Search</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtSearchQuery">Search Query:</label>
                        <asp:TextBox ID="txtSearchQuery" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Advanced Search</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtTitle">Title:</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtAuthor">Author:</label>
                        <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCategory">Category:</label>
                        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtYear">Publication Year:</label>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAdvancedSearch" runat="server" Text="Advanced Search" CssClass="btn btn-primary" OnClick="btnAdvancedSearch_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get Recommendations</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtBookId">Book ID:</label>
                        <asp:TextBox ID="txtBookId" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnGetRecommendations" runat="server" Text="Get Recommendations" CssClass="btn btn-primary" OnClick="btnGetRecommendations_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get Popular Books</h4>
                </div>
                <div class="card-body">
                    <asp:Button ID="btnGetPopularBooks" runat="server" Text="Get Popular Books" CssClass="btn btn-primary" OnClick="btnGetPopularBooks_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get All Categories</h4>
                </div>
                <div class="card-body">
                    <asp:Button ID="btnGetAllCategories" runat="server" Text="Get All Categories" CssClass="btn btn-primary" OnClick="btnGetAllCategories_Click" />
                </div>
            </div>
            
            <h3>Results:</h3>
            <asp:Panel ID="pnlResults" runat="server" CssClass="card">
                <div class="card-body">
                    <pre><asp:Literal ID="litResults" runat="server"></asp:Literal></pre>
                </div>
            </asp:Panel>
            
            <div class="mt-4">
                <a href="../Default.aspx" class="btn btn-secondary">Back to Home</a>
            </div>
        </div>
    </form>
</body>
</html> 