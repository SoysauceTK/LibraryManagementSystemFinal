<%@ Page Title="Staff Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LibraryManagementSystem.Staff.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Staff Dashboard</h1>
        <p class="lead">Welcome, <asp:Literal ID="UserNameLiteral" runat="server"></asp:Literal>. Manage the library system here.</p>
    </div>

    <div class="row">
        <%-- Left Column: Quick Stats & Actions --%>
        <div class="col-md-4">
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h3>Quick Stats</h3>
                </div>
                <div class="card-body">
                    <ul class="list-group list-group-flush">
                        <li class="list-group-item">Total Books: <asp:Literal ID="TotalBooksLiteral" runat="server">Loading...</asp:Literal></li>
                        <li class="list-group-item">Books Borrowed: <asp:Literal ID="BorrowedBooksLiteral" runat="server">Loading...</asp:Literal></li>
                        <li class="list-group-item">Registered Members: <asp:Literal ID="MembersLiteral" runat="server">Loading...</asp:Literal></li>
                        <li class="list-group-item">Overdue Returns: <asp:Literal ID="OverdueLiteral" runat="server">Loading...</asp:Literal></li>
                    </ul>
                </div>
            </div>

            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h3>Quick Actions</h3>
                </div>
                <div class="card-body">
                    <div class="list-group">
                        <a href="BookManagement.aspx" class="list-group-item list-group-item-action">Book Management</a>
                        <a href="MemberManagement.aspx" class="list-group-item list-group-item-action">Member Management</a>
                        <a href="BorrowingManagement.aspx" class="list-group-item list-group-item-action">Borrowing Management</a>
                        <a href="ReportGenerator.aspx" class="list-group-item list-group-item-action">Generate Reports</a>
                    </div>
                </div>
            </div>
        </div>

        <%-- Right Column: Add Book, Search, Activities --%>
        <div class="col-md-8">
            <%-- Add New Book Card --%>
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h3>Add New Book</h3>
                </div>
                <div class="card-body">
                    <%-- Book fields... --%>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BookTitle" CssClass="control-label">Title</asp:Label>
                        <asp:TextBox runat="server" ID="BookTitle" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="BookTitle" CssClass="text-danger" ErrorMessage="Title is required." ValidationGroup="BookAdd" Display="Dynamic"/>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BookAuthor" CssClass="control-label">Author</asp:Label>
                        <asp:TextBox runat="server" ID="BookAuthor" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="BookAuthor" CssClass="text-danger" ErrorMessage="Author is required." ValidationGroup="BookAdd" Display="Dynamic"/>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <asp:Label runat="server" AssociatedControlID="BookISBN" CssClass="control-label">ISBN</asp:Label>
                            <asp:TextBox runat="server" ID="BookISBN" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="BookISBN" CssClass="text-danger" ErrorMessage="ISBN is required." ValidationGroup="BookAdd" Display="Dynamic"/>
                        </div>
                        <div class="form-group col-md-6">
                            <asp:Label runat="server" AssociatedControlID="BookCategory" CssClass="control-label">Category</asp:Label>
                            <asp:DropDownList runat="server" ID="BookCategory" CssClass="form-control">
                                <asp:ListItem Text="Fiction" Value="Fiction" />
                                <asp:ListItem Text="Non-Fiction" Value="Non-Fiction" />
                                <asp:ListItem Text="Science Fiction" Value="Science Fiction" />
                                <asp:ListItem Text="Mystery" Value="Mystery" />
                                <asp:ListItem Text="Biography" Value="Biography" />
                                <asp:ListItem Text="History" Value="History" />
                                <asp:ListItem Text="Self-Help" Value="Self-Help" />
                                <asp:ListItem Text="Reference" Value="Reference" />
                                <%-- Add more categories as needed --%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <asp:Label runat="server" AssociatedControlID="BookPublicationYear" CssClass="control-label">Publication Year</asp:Label>
                            <asp:TextBox runat="server" ID="BookPublicationYear" CssClass="form-control" TextMode="Number" />
                            <asp:RangeValidator runat="server" ControlToValidate="BookPublicationYear" CssClass="text-danger" ErrorMessage="Invalid year." MinimumValue="1000" MaximumValue="2030" Type="Integer" ValidationGroup="BookAdd" Display="Dynamic"/> <%-- Increased Max Year slightly --%>
                        </div>
                        <div class="form-group col-md-6">
                            <asp:Label runat="server" AssociatedControlID="BookCopies" CssClass="control-label">Copies Available</asp:Label>
                            <asp:TextBox runat="server" ID="BookCopies" CssClass="form-control" TextMode="Number" />
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="BookCopies" CssClass="text-danger" ErrorMessage="Copies is required." ValidationGroup="BookAdd" Display="Dynamic"/>
                            <asp:RangeValidator runat="server" ControlToValidate="BookCopies" CssClass="text-danger" ErrorMessage="Must be 0 or more." MinimumValue="0" MaximumValue="1000" Type="Integer" ValidationGroup="BookAdd" Display="Dynamic"/> <%-- Allow 0 copies --%>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BookDescription" CssClass="control-label">Description</asp:Label>
                        <asp:TextBox runat="server" ID="BookDescription" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" ID="AddBookButton" Text="Add Book" CssClass="btn btn-primary" OnClick="AddBookButton_Click" ValidationGroup="BookAdd" />
                    </div>
                    <div class="form-group">
                        <%-- Make this a div for consistent styling --%>
                        <asp:Panel runat="server" ID="BookAddStatusPanel" Visible="false" CssClass="mt-3">
                             <asp:Literal runat="server" ID="BookAddStatus" />
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <%-- NEW: Book Search Card --%>
            <div class="card mb-4">
                <div class="card-header bg-info text-white"> <%-- Changed color for distinction --%>
                    <h3>Search Books</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SearchTextBox" CssClass="control-label">Search Term (Title, Author, ISBN, Category, etc.)</asp:Label>
                        <asp:TextBox runat="server" ID="SearchTextBox" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" ID="SearchButton" Text="Search Books" CssClass="btn btn-info" OnClick="SearchButton_Click" />
                    </div>
                    <div class="form-group">
                         <asp:Panel runat="server" ID="SearchStatusPanel" Visible="false" CssClass="mt-3">
                            <asp:Literal runat="server" ID="SearchStatusLiteral" />
                         </asp:Panel>
                    </div>
                     <div class="form-group mt-3 table-responsive"> <%-- Added table-responsive --%>
                         <asp:GridView ID="SearchResultsGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"
                             EmptyDataText="No books found matching your search criteria.">
                             <Columns>
                                 <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                                 <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author" />
                                 <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                                 <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                 <asp:BoundField DataField="PublicationYear" HeaderText="Year" SortExpression="PublicationYear" />
                                 <asp:BoundField DataField="CopiesAvailable" HeaderText="Copies" SortExpression="CopiesAvailable" />
                             </Columns>
                              <HeaderStyle CssClass="bg-light" />
                         </asp:GridView>
                     </div>
                </div>
            </div>

            <%-- Recent Activities Card --%>
            <div class="card">
                <div class="card-header bg-secondary text-white"> <%-- Changed color --%>
                    <h3>Recent Activities</h3>
                </div>
                <div class="card-body table-responsive"> <%-- Added table-responsive --%>
                    <asp:GridView ID="ActivityLogGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover"
                        EmptyDataText="No recent activities recorded.">
                        <Columns>
                            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:g}" SortExpression="Date" />
                            <asp:BoundField DataField="Action" HeaderText="Action" SortExpression="Action" />
                            <asp:BoundField DataField="Details" HeaderText="Details" SortExpression="Details" />
                            <asp:BoundField DataField="User" HeaderText="User" SortExpression="User" />
                        </Columns>
                         <HeaderStyle CssClass="bg-light" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>