<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryBookStorage.aspx.cs" Inherits="LibraryManagementSystem.Services.TryBookStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Try Book Storage Service</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2>Try Book Storage Service</h2>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get All Books</h4>
                </div>
                <div class="card-body">
                    <asp:Button ID="btnGetAllBooks" runat="server" Text="Get All Books" CssClass="btn btn-primary" OnClick="btnGetAllBooks_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get Book By ID</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtBookId">Book ID:</label>
                        <asp:TextBox ID="txtBookId" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnGetBookById" runat="server" Text="Get Book" CssClass="btn btn-primary" OnClick="btnGetBookById_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Add New Book</h4>
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
                        <label for="txtISBN">ISBN:</label>
                        <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCategory">Category:</label>
                        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtPublicationYear">Publication Year:</label>
                        <asp:TextBox ID="txtPublicationYear" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCopiesAvailable">Copies Available:</label>
                        <asp:TextBox ID="txtCopiesAvailable" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAddBook" runat="server" Text="Add Book" CssClass="btn btn-success" OnClick="btnAddBook_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Update Inventory</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtUpdateBookId">Book ID:</label>
                        <asp:TextBox ID="txtUpdateBookId" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtQuantityChange">Quantity Change:</label>
                        <asp:TextBox ID="txtQuantityChange" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnUpdateInventory" runat="server" Text="Update Inventory" CssClass="btn btn-warning" OnClick="btnUpdateInventory_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Get Books By Category</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtSearchCategory">Category:</label>
                        <asp:TextBox ID="txtSearchCategory" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnGetBooksByCategory" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnGetBooksByCategory_Click" />
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4>Delete Book</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtDeleteBookId">Book ID:</label>
                        <asp:TextBox ID="txtDeleteBookId" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnDeleteBook" runat="server" Text="Delete Book" CssClass="btn btn-danger" OnClick="btnDeleteBook_Click" />
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
