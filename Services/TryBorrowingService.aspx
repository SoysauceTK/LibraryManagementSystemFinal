<%@ Page Title="Try Borrowing Service" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TryBorrowingService.aspx.cs" Inherits="LibraryManagementSystem.Services.TryBorrowingService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><%: Title %></h2>
        <p class="lead">Test the Borrowing Service functionality</p>
        
        <div class="row">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h3>Borrow a Book</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <asp:Label ID="lblBookId" runat="server" AssociatedControlID="txtBookId" CssClass="control-label">Book ID:</asp:Label>
                            <asp:TextBox ID="txtBookId" runat="server" CssClass="form-control" placeholder="Enter book ID" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBookId" 
                                CssClass="text-danger" ErrorMessage="Book ID is required." 
                                Display="Dynamic" ValidationGroup="BorrowGroup" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblMemberId" runat="server" AssociatedControlID="txtMemberId" CssClass="control-label">Member ID:</asp:Label>
                            <asp:TextBox ID="txtMemberId" runat="server" CssClass="form-control" placeholder="Enter member ID" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMemberId" 
                                CssClass="text-danger" ErrorMessage="Member ID is required." 
                                Display="Dynamic" ValidationGroup="BorrowGroup" />
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnBorrow" runat="server" Text="Borrow Book" OnClick="btnBorrow_Click" 
                                CssClass="btn btn-primary" ValidationGroup="BorrowGroup" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h3>Return a Book</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <asp:Label ID="lblBorrowId" runat="server" AssociatedControlID="txtBorrowId" CssClass="control-label">Borrow ID:</asp:Label>
                            <asp:TextBox ID="txtBorrowId" runat="server" CssClass="form-control" placeholder="Enter borrow transaction ID" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBorrowId" 
                                CssClass="text-danger" ErrorMessage="Borrow ID is required." 
                                Display="Dynamic" ValidationGroup="ReturnGroup" />
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnReturn" runat="server" Text="Return Book" OnClick="btnReturn_Click" 
                                CssClass="btn btn-success" ValidationGroup="ReturnGroup" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <h3>View Borrowing History</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <asp:Label ID="lblHistoryMemberId" runat="server" AssociatedControlID="txtHistoryMemberId" CssClass="control-label">Member ID:</asp:Label>
                            <asp:TextBox ID="txtHistoryMemberId" runat="server" CssClass="form-control" placeholder="Enter member ID to view their borrowing history" />
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnViewHistory" runat="server" Text="View History" OnClick="btnViewHistory_Click" 
                                CssClass="btn btn-info" />
                            <asp:Button ID="btnViewAllBorrowing" runat="server" Text="View All Current Borrowings" 
                                OnClick="btnViewAllBorrowing_Click" CssClass="btn btn-secondary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <h3>Results</h3>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="alert alert-info">
                            <asp:Literal ID="litResults" runat="server"></asp:Literal>
                        </asp:Panel>
                        
                        <asp:GridView ID="gvBorrowingHistory" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-striped table-bordered" Visible="false">
                            <Columns>
                                <asp:BoundField DataField="BorrowId" HeaderText="Borrow ID" />
                                <asp:BoundField DataField="BookId" HeaderText="Book ID" />
                                <asp:BoundField DataField="BookTitle" HeaderText="Book Title" />
                                <asp:BoundField DataField="MemberId" HeaderText="Member ID" />
                                <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-warning">No borrowing records found.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 