<%@ Page Title="Try Service" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="LibraryManagementSystem.Services.TryIt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Service Test Page</h1>
        <p class="lead">Test the <asp:Literal ID="ServiceNameLiteral" runat="server"></asp:Literal> operation: <asp:Literal ID="OperationNameLiteral" runat="server"></asp:Literal></p>
        <small class="text-muted">Service URL: <asp:Literal ID="ServiceUrlLiteral" runat="server"></asp:Literal></small>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3>Input Parameters</h3>
                </div>
                <div class="card-body">
                    <asp:PlaceHolder ID="ParametersPlaceholder" runat="server"></asp:PlaceHolder>
                    <div class="form-group mt-3">
                        <asp:Button ID="ExecuteButton" runat="server" Text="Execute" CssClass="btn btn-primary" OnClick="ExecuteButton_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h3>Result</h3>
                </div>
                <div class="card-body">
                    <pre id="resultJson" runat="server" class="bg-light p-3 rounded"></pre>
                </div>
            </div>
        </div>
    </div>
</asp:Content>