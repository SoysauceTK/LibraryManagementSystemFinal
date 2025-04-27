<%@ Page Title="Member Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LibraryManagementSystem.Member.Dashboard" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Welcome, <asp:Literal ID="UserNameLiteral" runat="server"></asp:Literal>!</h1>
        <p class="lead">This is your personal dashboard where you can manage your library activities.</p>
    </div>

    <asp:Panel ID="AlertPanel" runat="server" CssClass="alert alert-success alert-dismissible fade show" Visible="false" role="alert">
        <asp:Literal ID="AlertMessage" runat="server"></asp:Literal>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </asp:Panel>

    <div class="row">
        <div class="col-md-4">
            <div class="card mb-4">
                <div class="card-header">
                    <h3>My Account</h3>
                </div>
                <div class="card-body">
                    <p><strong>Username:</strong> <asp:Literal ID="UsernameLabel" runat="server"></asp:Literal></p>
                    <p><strong>Email:</strong> <asp:Literal ID="EmailLabel" runat="server"></asp:Literal></p>
                    <p><strong>Member Since:</strong> <asp:Literal ID="MemberSinceLabel" runat="server"></asp:Literal></p>
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h3>Borrow History</h3>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="BorrowHistoryUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="BorrowHistoryGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-sm"
                                EmptyDataText="No borrow history available." GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="Title" HeaderText="Title" />
                                    <asp:BoundField DataField="ActionDate" HeaderText="Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="ActionType" HeaderText="Action" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <div class="card mb-4">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h3>My Borrowed Books</h3>
                    <asp:UpdateProgress ID="BorrowedBooksUpdateProgress" runat="server" AssociatedUpdatePanelID="BorrowedBooksUpdatePanel">
                        <ProgressTemplate>
                            <div class="spinner-border spinner-border-sm text-primary" role="status">
                                <span class="sr-only">Loading...</span>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="BorrowedBooksUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="BorrowedBooksGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
                                EmptyDataText="You have no borrowed books." GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="Title" HeaderText="Title" />
                                    <asp:BoundField DataField="Author" HeaderText="Author" />
                                    <asp:BoundField DataField="BorrowDate" HeaderText="Borrowed On" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:d}" />
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <%# GetDueStatus((DateTime)Eval("DueDate")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:Button ID="RenewButton" runat="server" Text="Renew" CssClass="btn btn-sm btn-info"
                                                CommandName="Renew" CommandArgument='<%# Eval("Id") %>' OnCommand="BookAction_Command" />
                                            <asp:Button ID="ReturnButton" runat="server" Text="Return" CssClass="btn btn-sm btn-success"
                                                CommandName="Return" CommandArgument='<%# Eval("Id") %>' OnCommand="BookAction_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h3>Recommended Books</h3>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="RecommendedBooksRepeater" runat="server">
                        <ItemTemplate>
                            <div class="card mb-3">
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("Title") %></h5>
                                    <h6 class="card-subtitle mb-2 text-muted"><%# Eval("Author") %></h6>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                    <asp:Button ID="BorrowButton" runat="server" Text="Borrow" CssClass="btn btn-primary btn-sm"
                                               CommandName="Borrow" CommandArgument='<%# Eval("Id") %>' OnCommand="BookAction_Command" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>