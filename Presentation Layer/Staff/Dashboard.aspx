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
                        <%-- Links to the main service interaction pages --%>
                        <asp:HyperLink runat="server" CssClass="btn btn-secondary btn-block mb-2"
                            NavigateUrl="~/Service Layer/Book Storage Service/BookStorageTryIt.aspx">
                            <i class="fas fa-book"></i> Book Storage Service
                        </asp:HyperLink>

                        <asp:HyperLink runat="server" CssClass="btn btn-secondary btn-block mb-2"
                            NavigateUrl="~/Service Layer/Book Search Service/SearchTryIt.aspx">
                            <i class="fas fa-search"></i> Book Search Service
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        Component Contributions & Service Directory
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Contributor</th>
                                        <th>Component Type</th>
                                        <th>Primary Function/Example Operation</th>
                                        <th>Parameters</th>
                                        <th>Return Type</th>
                                        <th>Description</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Aarya Baireddy</td>
                                        <td>RESTful Service</td>
                                        <td>Book Search Service (e.g., SearchBooks)</td>
                                        <td>query (string)</td>
                                        <td>List&lt;Book&gt;</td>
                                        <td>Provides book searching capabilities via REST API.</td>
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
                                        <td>Book Storage Service (e.g., GetAllBooks)</td>
                                        <td>Varies (e.g., None for GetAllBooks)</td>
                                        <td>Varies (e.g., List&lt;Book&gt;)</td>
                                        <td>Manages CRUD operations for library books via REST API.</td>
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
            </div>
        </div>
    </div>
</asp:Content>