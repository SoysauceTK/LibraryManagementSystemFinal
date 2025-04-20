<%@ Page Title="Try Book Storage Service" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookStorageTryIt.aspx.cs" Inherits="LibraryManagementSystem.Service_Layer.Book_Storage_Service.BookStorageTryIt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Book Storage Service Test Page</h2>

    <div class="row">
        <div class="col-md-6">
            <!-- Get Operations -->
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h3>Get Operations</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Button ID="btnGetAllBooks" runat="server" Text="Get All Books" OnClick="btnGetAllBooks_Click" CssClass="btn btn-info btn-block mb-2" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblBookId" runat="server" Text="Get Book By ID:" />
                        <div class="input-group">
                            <asp:TextBox ID="txtBookId" runat="server" CssClass="form-control" placeholder="Enter Book ID" />
                            <div class="input-group-append">
                                <asp:Button ID="btnGetBookById" runat="server" Text="Get" OnClick="btnGetBookById_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblCategory" runat="server" Text="Get Books By Category:" />
                        <div class="input-group">
                            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" placeholder="Enter Category" />
                            <div class="input-group-append">
                                <asp:Button ID="btnGetBooksByCategory" runat="server" Text="Get" OnClick="btnGetBooksByCategory_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="col-md-6">
            <!-- CRUD Operations -->
            <div class="card mb-4">
                <div class="card-header bg-success text-white">
                    <h3>CRUD Operations</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Label ID="lblAddBook" runat="server" Text="Add New Book:" />
                        <asp:Button ID="btnShowAddForm" runat="server" Text="Show Form" OnClick="btnShowAddForm_Click" CssClass="btn btn-success btn-block mb-2" />
                        <asp:Panel ID="pnlAddBook" runat="server" Visible="false">
                            <div class="form-group">
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control mb-2" placeholder="Title" />
                                <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control mb-2" placeholder="Author" />
                                <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control mb-2" placeholder="ISBN" />
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control mb-2" placeholder="Category" />
                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control mb-2" placeholder="Publication Year" TextMode="Number" />
                                <asp:TextBox ID="txtPublisher" runat="server" CssClass="form-control mb-2" placeholder="Publisher" />
                                <asp:TextBox ID="txtCopies" runat="server" CssClass="form-control mb-2" placeholder="Copies Available" TextMode="Number" />
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control mb-2" placeholder="Description" TextMode="MultiLine" Rows="3" />
                                <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="btnAddBook_Click" CssClass="btn btn-success" />
                                <asp:Button ID="btnCancelAdd" runat="server" Text="Cancel" OnClick="btnCancelAdd_Click" CssClass="btn btn-secondary" />
                            </div>
                        </asp:Panel>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblUpdateBook" runat="server" Text="Update Book:" />
                        <div class="input-group mb-2">
                            <asp:TextBox ID="txtUpdateId" runat="server" CssClass="form-control" placeholder="Book ID to Update" />
                            <div class="input-group-append">
                                <asp:Button ID="btnLoadForUpdate" runat="server" Text="Load" OnClick="btnLoadForUpdate_Click" CssClass="btn btn-warning" />
                            </div>
                        </div>
                        <asp:Panel ID="pnlUpdateBook" runat="server" Visible="false">
                            <div class="form-group">
                                <asp:HiddenField ID="hdnUpdateId" runat="server" />
                                <asp:TextBox ID="txtUpdateTitle" runat="server" CssClass="form-control mb-2" placeholder="Title" />
                                <asp:TextBox ID="txtUpdateAuthor" runat="server" CssClass="form-control mb-2" placeholder="Author" />
                                <asp:TextBox ID="txtUpdateISBN" runat="server" CssClass="form-control mb-2" placeholder="ISBN" />
                                <asp:TextBox ID="txtUpdateCategory" runat="server" CssClass="form-control mb-2" placeholder="Category" />
                                <asp:TextBox ID="txtUpdateYear" runat="server" CssClass="form-control mb-2" placeholder="Publication Year" TextMode="Number" />
                                <asp:TextBox ID="txtUpdatePublisher" runat="server" CssClass="form-control mb-2" placeholder="Publisher" />
                                <asp:TextBox ID="txtUpdateCopies" runat="server" CssClass="form-control mb-2" placeholder="Copies Available" TextMode="Number" />
                                <asp:TextBox ID="txtUpdateDescription" runat="server" CssClass="form-control mb-2" placeholder="Description" TextMode="MultiLine" Rows="3" />
                                <asp:Button ID="btnUpdateBook" runat="server" Text="Update Book" OnClick="btnUpdateBook_Click" CssClass="btn btn-warning" />
                                <asp:Button ID="btnCancelUpdate" runat="server" Text="Cancel" OnClick="btnCancelUpdate_Click" CssClass="btn btn-secondary" />
                            </div>
                        </asp:Panel>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblDeleteBook" runat="server" Text="Delete Book:" />
                        <div class="input-group">
                            <asp:TextBox ID="txtDeleteId" runat="server" CssClass="form-control" placeholder="Enter Book ID to Delete" />
                            <div class="input-group-append">
                                <asp:Button ID="btnDeleteBook" runat="server" Text="Delete" OnClick="btnDeleteBook_Click" CssClass="btn btn-danger" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-info text-white">
                    <h3>Results</h3>
                </div>
                <div class="card-body">
                    <asp:Literal ID="litResult" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>