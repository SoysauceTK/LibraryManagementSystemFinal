<%@ Page Title="Try Cookies" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TryCookies.aspx.cs" Inherits="LibraryManagementSystem.Controls.TryCookies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <h2><%: Title %></h2>
                <hr />
                
                <div class="card mb-4">
                    <div class="card-header">
                        <h3>Cookie Demo</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <asp:Label ID="lblCookieName" runat="server" AssociatedControlID="txtCookieName" CssClass="control-label">Cookie Name:</asp:Label>
                            <asp:TextBox ID="txtCookieName" runat="server" CssClass="form-control" placeholder="Enter cookie name" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblCookieValue" runat="server" AssociatedControlID="txtCookieValue" CssClass="control-label">Cookie Value:</asp:Label>
                            <asp:TextBox ID="txtCookieValue" runat="server" CssClass="form-control" placeholder="Enter cookie value" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblExpiration" runat="server" AssociatedControlID="ddlExpiration" CssClass="control-label">Expiration:</asp:Label>
                            <asp:DropDownList ID="ddlExpiration" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0" Text="Session Cookie (expires when browser closes)" />
                                <asp:ListItem Value="1" Text="1 Minute" />
                                <asp:ListItem Value="60" Text="1 Hour" />
                                <asp:ListItem Value="1440" Text="1 Day" />
                                <asp:ListItem Value="10080" Text="1 Week" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnSetCookie" runat="server" Text="Set Cookie" CssClass="btn btn-primary" OnClick="btnSetCookie_Click" />
                            <asp:Button ID="btnDeleteCookie" runat="server" Text="Delete Cookie" CssClass="btn btn-danger" OnClick="btnDeleteCookie_Click" />
                        </div>
                    </div>
                </div>

                <div class="card mb-4">
                    <div class="card-header">
                        <h3>Current Cookies</h3>
                    </div>
                    <div class="card-body">
                        <asp:Literal ID="litCookies" runat="server"></asp:Literal>
                    </div>
                </div>

                <div class="card mb-4">
                    <div class="card-header">
                        <h3>Session Information</h3>
                    </div>
                    <div class="card-body">
                        <p><strong>Session ID:</strong> <asp:Literal ID="litSessionId" runat="server"></asp:Literal></p>
                        <p><strong>Session Created:</strong> <asp:Literal ID="litSessionStart" runat="server"></asp:Literal></p>
                        <p><strong>Session Timeout:</strong> <asp:Literal ID="litSessionTimeout" runat="server"></asp:Literal> minutes</p>
                        
                        <div class="mt-3">
                            <h4>Session Variables</h4>
                            <div class="form-group">
                                <asp:Label ID="lblSessionKey" runat="server" AssociatedControlID="txtSessionKey" CssClass="control-label">Key:</asp:Label>
                                <asp:TextBox ID="txtSessionKey" runat="server" CssClass="form-control" placeholder="Session variable name" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblSessionValue" runat="server" AssociatedControlID="txtSessionValue" CssClass="control-label">Value:</asp:Label>
                                <asp:TextBox ID="txtSessionValue" runat="server" CssClass="form-control" placeholder="Session variable value" />
                            </div>
                            <div class="form-group mt-2">
                                <asp:Button ID="btnSetSession" runat="server" Text="Set Session Variable" CssClass="btn btn-primary" OnClick="btnSetSession_Click" />
                                <asp:Button ID="btnClearSession" runat="server" Text="Clear All Session Data" CssClass="btn btn-warning" OnClick="btnClearSession_Click" OnClientClick="return confirm('Are you sure you want to clear all session data?');" />
                            </div>
                        </div>
                        
                        <div class="mt-3">
                            <h4>Current Session Variables</h4>
                            <asp:Literal ID="litSessionVars" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>

                <div class="card mb-4">
                    <div class="card-header">
                        <h3>Application State</h3>
                    </div>
                    <div class="card-body">
                        <p><strong>Active Users:</strong> <asp:Literal ID="litActiveUsers" runat="server"></asp:Literal></p>
                        <p><strong>Total Visits:</strong> <asp:Literal ID="litTotalVisits" runat="server"></asp:Literal></p>
                        <p class="text-info"><small>These values are managed by the Global.asax Session_Start and Session_End events</small></p>
                    </div>
                </div>

                <div class="alert alert-info mt-4">
                    <h4>How This Works</h4>
                    <p>This page demonstrates that:</p>
                    <ol>
                        <li>HTTP cookies work properly in this application</li>
                        <li>Global.asax event handlers are functioning (tracking active sessions)</li>
                        <li>Application state is being maintained</li>
                        <li>Session state is properly managed</li>
                    </ol>
                    <p>The <code>Global.asax</code> file manages the <code>Application_Start</code>, <code>Session_Start</code>, <code>Session_End</code>, and <code>Application_Error</code> events.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 