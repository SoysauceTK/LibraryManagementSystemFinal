<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryManagementSystem.Presentation_Layer.Public._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron text-center">
        <h1>Library Management System</h1>
        <p class="lead">Welcome to the Library Management System. Please select your access type:</p>
        <div class="row">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <h3 class="card-title">Member Access</h3>
                        <p class="card-text">Browse books, manage your account, and check out materials.</p>
                        <asp:Button ID="btnMember" runat="server" CssClass="btn btn-primary btn-lg" Text="Member Login" OnClick="btnMember_Click" />
                        <asp:Button ID="btnMemberRegister" runat="server" CssClass="btn btn-outline-primary btn-lg" Text="Register" OnClick="btnMemberRegister_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <h3 class="card-title">Staff Access</h3>
                        <p class="card-text">Manage library inventory, members, and system administration.</p>
                        <asp:Button ID="btnStaff" runat="server" CssClass="btn btn-secondary btn-lg" Text="Staff Login" OnClick="btnStaff_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlLoggedInContent" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2>Service Directory</h2>
                <p>The following services are available in this application:</p>
                
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
                                <td><a href="Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=GetAllBooks" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>WSDL Service</td>
                                <td>GetBookById</td>
                                <td>id (string)</td>
                                <td>Book</td>
                                <td>Retrieves a book by its ID</td>
                                <td><a href="Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=GetBookById" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>WSDL Service</td>
                                <td>AddBook</td>
                                <td>Book (object)</td>
                                <td>Book</td>
                                <td>Adds a new book to the library</td>
                                <td><a href="Service Layer/Book Storage Service/BookStorageTryIt.aspx?op=AddBook" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>RESTful Service</td>
                                <td>SearchBooks</td>
                                <td>query (string)</td>
                                <td>List&lt;Book&gt;</td>
                                <td>Searches books by title, author, or keyword</td>
                                <td><a href="Service Layer/Book Search Service/SearchTryIt.aspx?op=SearchBooks" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>RESTful Service</td>
                                <td>AdvancedSearch</td>
                                <td>title, author, category, year (all string)</td>
                                <td>List&lt;Book&gt;</td>
                                <td>Performs advanced search with multiple criteria</td>
                                <td><a href="Service Layer/Book Search Service/SearchTryIt.aspx?op=AdvancedSearch" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>RESTful Service</td>
                                <td>GetRecommendations</td>
                                <td>bookId (string)</td>
                                <td>List&lt;Book&gt;</td>
                                <td>Gets book recommendations based on a book ID</td>
                                <td><a href="Service Layer/Book Search Service/SearchTryIt.aspx?op=GetRecommendations" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>DLL Library</td>
                                <td>HashPassword</td>
                                <td>password (string)</td>
                                <td>string</td>
                                <td>Generates a secure hash of a password</td>
                                <td><a href="Service Layer/SecurityServiceTryIt.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>User Control</td>
                                <td>CaptchaControl</td>
                                <td>N/A</td>
                                <td>N/A</td>
                                <td>Displays a CAPTCHA for form verification</td>
                                <td><a href="Controls/TryCaptcha.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2>Test Cases</h2>
                <p>To test this application, you can use the following credentials:</p>
                <ul>
                    <li><strong>Staff Login:</strong> Username: "TA", Password: "Cse445!"</li>
                    <li><strong>Member Login:</strong> Register a new account or use the test account: Username: "testuser", Password: "Test@123"</li>
                </ul>
            </div>
        </div>
    </asp:Panel>
</asp:Content>