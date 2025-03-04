<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserController.aspx.cs" Inherits="ProjectManagementSystem.Controllers.UserController" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create User</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Create New User</h2>
            
            <label>Username:</label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />

            <label>Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <br />

            <label>Role:</label>
            <asp:DropDownList ID="ddlRole" runat="server">
                <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                <asp:ListItem Text="Project Manager" Value="ProjectManager"></asp:ListItem>
                <asp:ListItem Text="Technician" Value="Technician"></asp:ListItem>
            </asp:DropDownList>
            <br />

            <asp:Button ID="btnCreateUser" runat="server" Text="Create User" OnClick="btnCreateUser_Click" />
            <br />

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
