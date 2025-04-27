<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryManagementSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Library Management System</h1>
        <p class="lead">Welcome to the Library Management System. This platform allows you to browse books, manage your borrowings, and much more.</p>
        <p>
            <asp:Button ID="btnMember" runat="server" CssClass="btn btn-primary btn-lg" Text="Member Area" OnClick="btnMember_Click" />
            <asp:Button ID="btnStaff" runat="server" CssClass="btn btn-secondary btn-lg" Text="Staff Area" OnClick="btnStaff_Click" />
        </p>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2>How to Get Started</h2>
            <p>
                To access the library services, please register for a member account or log in if you already have one.
                Staff members can access the staff area with credentials provided by the administrator.
            </p>
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
            <p>
                This application demonstrates various features required for CSE 445/598 Assignments 5 and 6, including:
            </p>
            <ul>
                <li>Member self-registration with CAPTCHA verification</li>
                <li>Secure password storage with hashing</li>
                <li>Role-based access control for Members and Staff</li>
                <li>XML-based data storage</li>
                <li>Web services integration</li>
                <li>Multiple component types (DLL, User Controls, Global Event Handlers)</li>
            </ul>
        </div>
    </div>

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
                            <td>Sawyer Kesti</td>
                            <td>User Control</td>
                            <td>CaptchaControl</td>
                            <td>N/A</td>
                            <td>N/A</td>
                            <td>Displays a CAPTCHA for form verification</td>
                            <td><a href="Controls/TryCaptcha.aspx" class="btn btn-sm btn-info">Try It</a></td>
                        </tr>
                        <tr>
                            <td>Both Members</td>
                            <td>Global Event Handler</td>
                            <td>Application_Start</td>
                            <td>object, EventArgs</td>
                            <td>void</td>
                            <td>Initializes application and logs startup</td>
                            <td>N/A</td>
                        </tr>
                        <tr>
                            <td>Both Members</td>
                            <td>Global Event Handler</td>
                            <td>Session_Start</td>
                            <td>object, EventArgs</td>
                            <td>void</td>
                            <td>Tracks active users and session stats</td>
                            <td>N/A</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>