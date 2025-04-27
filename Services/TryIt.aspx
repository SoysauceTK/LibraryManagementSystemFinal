<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="LibraryManagementSystem.Services.TryIt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Try Service</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2>Try <asp:Literal ID="litServiceName" runat="server"></asp:Literal> Service</h2>
            <h4>Operation: <asp:Literal ID="litOperationName" runat="server"></asp:Literal></h4>
            
            <asp:Panel ID="pnlHashPassword" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-header">
                    <h4>Hash Password</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtPassword">Password:</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnHashPassword" runat="server" Text="Hash Password" CssClass="btn btn-primary" OnClick="btnHashPassword_Click" />
                </div>
            </asp:Panel>
            
            <asp:Panel ID="pnlVerifyPassword" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-header">
                    <h4>Verify Password</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtVerifyPassword">Password:</label>
                        <asp:TextBox ID="txtVerifyPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtHashedPassword">Hashed Password:</label>
                        <asp:TextBox ID="txtHashedPassword" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnVerifyPassword" runat="server" Text="Verify Password" CssClass="btn btn-primary" OnClick="btnVerifyPassword_Click" />
                </div>
            </asp:Panel>
            
            <h3>Results:</h3>
            <asp:Panel ID="pnlResults" runat="server" CssClass="card">
                <div class="card-body">
                    <pre><asp:Literal ID="litResults" runat="server"></asp:Literal></pre>
                </div>
            </asp:Panel>
            
            <div class="mt-4">
                <a href="../Default.aspx" class="btn btn-secondary">Back to Home</a>
            </div>
        </div>
    </form>
</body>
</html> 