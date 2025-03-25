<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserController.aspx.cs" Inherits="ProjectManagementSystem.Controllers.UserController" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create User</title>
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
        }
        .card {
            max-width: 500px;
            margin: auto;
            box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
            border-radius: 10px;
        }
        .card-header {
            background-color: #0056b3;;
            color: white;
            text-align: center;
            font-size: 20px;
            font-weight: bold;
        }
        .btn-primary {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="card">
                <div class="card-header">Create New User</div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtUsername">Username:</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtEmail">Email:</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" required></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="txtPassword">Password:</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="ddlRole">Role:</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text ="Admin" Value="Admin"></asp:ListItem>
                            <asp:ListItem Text="Project Manager" Value="ProjectManager"></asp:ListItem>
                            <asp:ListItem Text="Technician" Value="Technician"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group" id="technicianLevelGroup" runat="server" style="display:none;">
                        <label for="ddlTechnicianLevel">Technician Level:</label>
                        <asp:DropDownList ID="ddlTechnicianLevel" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Select Level" Value=""></asp:ListItem>
                            <asp:ListItem Value="Junior">Junior</asp:ListItem>
                            <asp:ListItem Value="Senior">Senior</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <asp:Button ID="btnCreateUser" runat="server" Text="Create User" CssClass="btn btn mt-3 mb-2 text-white" style="background-color: #0056b3;" OnClick="btnCreateUser_Click" />
                    <br /><br />
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS & jQuery -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>
