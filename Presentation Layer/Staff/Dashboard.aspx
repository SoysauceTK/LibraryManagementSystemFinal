<%@ Page Title="Staff Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" Inherits="LibraryManagementSystem.Presentation_Layer.Staff.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Staff Dashboard <small class="text-muted">Welcome <%: Context.User.Identity.Name %></small></h2>
        <hr />
        
        <div class="row">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        Quick Actions
                    </div>
                    <div class="card-body">
                        <asp:HyperLink runat="server" CssClass="btn btn-secondary btn-block mb-2"
                            NavigateUrl="~/Service Layer/Book Storage Service/BookStorageTryIt.aspx">
                            <i class="fas fa-book"></i> Book Storage
                        </asp:HyperLink>

                        <asp:HyperLink runat="server" CssClass="btn btn-secondary btn-block mb-2"
                            NavigateUrl="~/Service Layer/Book Search Service/SearchTryIt.aspx">
                            <i class="fas fa-search"></i> Search Service
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        Service Directory
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Provider</th>
                                        <th>Component Type</th>
                                        <th>Operation Name</th>
                                        <th>Parameters</th>
                                        <th>Return Type</th>
                                        <th>Description</th>
                                        <th>Try It</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Aarya Baireddy</td>
                                        <td>WSDL Service</td>
                                        <td>GetAllBooks</td>
                                        <td>None</td>
                                        <td>List&lt;Book&gt;</td>
                                        <td>Retrieves all books in the library</td>
                                        <td><a href="~/Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=GetAllBooks" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                    <tr>
                                        <td>Aarya Baireddy</td>
                                        <td>WSDL Service</td>
                                        <td>GetBookById</td>
                                        <td>id (string)</td>
                                        <td>Book</td>
                                        <td>Retrieves a book by its ID</td>
                                        <td><a href="~/Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=GetBookById" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                    <tr>
                                        <td>Aarya Baireddy</td>
                                        <td>WSDL Service</td>
                                        <td>AddBook</td>
                                        <td>Book (object)</td>
                                        <td>Book</td>
                                        <td>Adds a new book to the library</td>
                                        <td><a href="~/Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=AddBook" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                    <tr>
                                        <td>Sawyer Kesti</td>
                                        <td>RESTful Service</td>
                                        <td>SearchBooks</td>
                                        <td>query (string)</td>
                                        <td>List&lt;Book&gt;</td>
                                        <td>Searches books by title, author, or keyword</td>
                                        <td><a href="~/Service Layer/Book Search Service/SearchTryIt.aspx?op=SearchBooks" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                    <tr>
                                        <td>Sawyer Kesti</td>
                                        <td>RESTful Service</td>
                                        <td>AdvancedSearch</td>
                                        <td>title, author, category, year (all string)</td>
                                        <td>List&lt;Book&gt;</td>
                                        <td>Performs advanced search with multiple criteria</td>
                                        <td><a href="~/Service Layer/Book Search Service/SearchTryIt.aspx?op=AdvancedSearch" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                    <tr>
                                        <td>Sawyer Kesti</td>
                                        <td>RESTful Service</td>
                                        <td>GetRecommendations</td>
                                        <td>bookId (string)</td>
                                        <td>List&lt;Book&gt;</td>
                                        <td>Gets book recommendations based on a book ID</td>
                                        <td><a href="~/Service Layer/Book Search Service/SearchTryIt.aspx?op=GetRecommendations" runat="server" class="btn btn-sm btn-info">Try It</a></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>