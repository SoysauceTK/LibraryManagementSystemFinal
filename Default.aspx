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
                <h2>Application and Components Summary</h2>
                <p>The following components are available in this application:</p>
                
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
                                <td>Book Search Service (e.g., SearchBooks)</td>
                                <td>query (string)</td>
                                <td>List&lt;Book&gt;</td>
                                <td>Provides book searching capabilities via WSDL API.</td>
                                <td><a href="Services/TryBookSearch.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>Global.asax</td>
                                <td>Application_Start</td>
                                <td>object sender, EventArgs e</td>
                                <td>void</td>
                                <td>Manages application-level events and initialization.</td>
                                <td><a href="Controls/TryCookies.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>DLL Library</td>
                                <td>HashPassword</td>
                                <td>password (string)</td>
                                <td>string</td>
                                <td>Generates a secure hash of a password</td>
                                <td><a href="Services/TryIt.aspx?service=SecurityService&op=HashPassword" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>DLL Library</td>
                                <td>VerifyPassword</td>
                                <td>password (string), hash (string)</td>
                                <td>bool</td>
                                <td>Verifies a password against a hash</td>
                                <td><a href="Services/TryIt.aspx?service=SecurityService&op=VerifyPassword" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>Session State</td>
                                <td>Session_Start</td>
                                <td>object sender, EventArgs e</td>
                                <td>void</td>
                                <td>Handles server-side session state initialization.</td>
                                <td><a href="Controls/TryCookies.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>UI Component</td>
                                <td>Member Dashboard</td>
                                <td>N/A</td>
                                <td>N/A</td>
                                <td>User interface for member account management and book browsing.</td>
                                <td><a href="Member/Dashboard.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy & Sawyer Kesti</td>
                                <td>WSDL Service</td>
                                <td>Borrowing Service</td>
                                <td>Varies (e.g., bookId, memberId)</td>
                                <td>Varies (e.g., string, BorrowRecord[])</td>
                                <td>Manages book borrowing and returns via WSDL API.</td>
                                <td><a href="Services/TryBorrowingService.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>WSDL Service</td>
                                <td>Book Storage Service (e.g., GetAllBooks)</td>
                                <td>Varies (e.g., None for GetAllBooks)</td>
                                <td>Varies (e.g., List&lt;Book&gt;)</td>
                                <td>Manages CRUD operations for library books via WSDL API.</td>
                                <td><a href="Services/TryBookStorage.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>User Control</td>
                                <td>CaptchaControl</td>
                                <td>None</td>
                                <td>CaptchaResult (object)</td>
                                <td>ASP.NET control for generating and validating CAPTCHAs.</td>
                                <td><a href="Controls/TryCaptcha.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>Cookie Storage</td>
                                <td>SetClientCookie</td>
                                <td>name (string), value (string), expiry (DateTime)</td>
                                <td>void</td>
                                <td>Helper methods for managing client-side cookies.</td>
                                <td><a href="Controls/TryCookies.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>UI Component</td>
                                <td>Member Registration</td>
                                <td>N/A</td>
                                <td>N/A</td>
                                <td>User interface for new member registration with CAPTCHA verification.</td>
                                <td><a href="Member/Register.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>UI Component</td>
                                <td>Staff Dashboard</td>
                                <td>N/A</td>
                                <td>N/A</td>
                                <td>Administrative interface for library staff to manage books and members.</td>
                                <td><a href="Staff/Dashboard.aspx" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                            <tr>
                                <td>Both Members</td>
                                <td>Data Storage</td>
                                <td>XML Database</td>
                                <td>N/A</td>
                                <td>N/A</td>
                                <td>XML-based persistent storage for books, members, and transactions.</td>
                                <td><a href="Services/TryIt.aspx?service=XMLDatabase" class="btn btn-sm btn-info">Try It</a></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2>Local Component Layer Summary</h2>
                <p>Each team member has implemented the following required component types:</p>
                <ul>
                    <li><strong>Global.asax</strong>: Application event handlers for initialization and session management</li>
                    <li><strong>DLL class libraries</strong>: Security services including password hashing and verification</li>
                    <li><strong>Cookie/Session state</strong>: User profile storage and session management</li>
                    <li><strong>User controls</strong>: CAPTCHA validation and login components</li>
                </ul>
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