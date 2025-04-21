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
                                <td>RESTful Service</td>
                                <td>Book Storage Service (e.g., GetAllBooks)</td>
                                <td>Varies (e.g., None for GetAllBooks)</td>
                                <td>Varies (e.g., List&lt;Book&gt;)</td>
                                <td>Manages CRUD operations for library books via REST API.</td>
                            </tr>
                            <tr>
                                <td>Aarya Baireddy</td>
                                <td>Application Lifecycle</td>
                                <td>Global.asax (e.g., Application_Start)</td>
                                <td>Varies (e.g., sender, e for events)</td>
                                <td>void</td>
                                <td>Manages application-level events and state.</td>
                            </tr>
                                <tr>
                                <td>Aarya Baireddy</td>
                                <td>Session State Management</td>
                                <td>SetSessionValue</td>
                                <td>key (string), value (object)</td>
                                <td>void</td>
                                <td>Handles server-side session state persistence.</td>
                            </tr>
                            <tr>
                                <td>Sawyer Kesti</td>
                                <td>RESTful Service</td>
                                <td>Book Search Service (e.g., SearchBooks)</td>
                                <td>query (string)</td>
                                <td>List&lt;Book&gt;</td>
                                <td>Provides book searching capabilities via REST API.</td>
                            </tr>
                                <tr>
                                <td>Sawyer Kesti</td>
                                <td>Captcha User Control</td>
                                <td>GenerateCaptchaImage</td>
                                <td>None</td>
                                <td>CaptchaResult (object)</td>
                                <td>ASP.NET control for generating and validating CAPTCHAs.</td>
                            </tr>
                                <tr>
                                <td>Sawyer Kesti</td>
                                <td>Cookie Storage Utility</td>
                                <td>SetClientCookie</td>
                                <td>name (string), value (string), expiry (DateTime)</td>
                                <td>void</td>
                                <td>Helper methods for managing client-side cookies.</td>
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