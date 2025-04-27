<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="LibraryManagementSystem.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="error-template">
                    <h1>Oops!</h1>
                    <h2>Something went wrong</h2>
                    <div class="error-details mb-4">
                        We apologize for the inconvenience, but an error occurred while processing your request.
                    </div>
                    <div class="error-actions">
                        <a href="Default.aspx" class="btn btn-primary btn-lg">
                            <span class="glyphicon glyphicon-home"></span>
                            Take Me Home
                        </a>
                        <a href="mailto:support@example.com" class="btn btn-default btn-lg">
                            <span class="glyphicon glyphicon-envelope"></span>
                            Contact Support
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>