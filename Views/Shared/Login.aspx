<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectManagementSystem.Views.Shared.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Management System - Login</title>
    
    <%-- Bootstrap CSS for styling (optional but recommended) --%>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <%-- Custom styles for our login form --%>
    <style type="text/css">
        .login-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
         
        .error-message {
            color: #dc3545;
            margin-bottom: 15px;
            font-size: 0.9em;
        }

        .form-group {
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="loginForm" runat="server">
        <div class="container">
            <div class="login-container">
                <h2 class="text-center mb-4">Login</h2>
                
                <%-- Email field --%>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtEmail">Email:</asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Email is required"
                        CssClass="error-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
                
                <%-- Password field --%>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtPassword">Password:</asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Password is required"
                        CssClass="error-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>
                
                <%-- Login button --%>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" 
                        OnClick="btnLogin_Click" 
                        CssClass="btn btn-primary btn-block" />
                </div>

                <%-- Error message panel --%>
                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>