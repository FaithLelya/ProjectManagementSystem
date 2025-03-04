<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserController.aspx.cs" Inherits="ProjectManagementSystem.Controllers.UserController" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create User</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2>Create New User</h2>
            
            <div class="form-group">
                <label for="txtUsername">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>

           <div class="form-group">
            <label>Email:</label>
           <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="ddlRole">Role:</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                    <asp:ListItem Text="Project Manager" Value="ProjectManager"></asp:ListItem>
                    <asp:ListItem Text="Technician" Value="Technician"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:Button ID="btnCreateUser" runat="server" Text="Create User" CssClass="btn-primary" OnClick="btnCreateUser_Click" />
            <br /><br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
