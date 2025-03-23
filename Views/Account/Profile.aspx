<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="ProjectManagementSystem.Views.Account.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Profile</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="card shadow-lg">
                <div class="card-header bg-primary text-white text-center">
                    <h2>My Profile</h2>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtName">Name:</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtEmail">Email Address:</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-3 d-none" role="alert"></asp:Label>

                    <div class="d-flex justify-content-between mt-3">
                        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        window.onload = function () {
            var messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
            if (messageLabel.innerHTML.trim() !== "") {
                messageLabel.classList.remove('d-none');
                messageLabel.classList.add('alert-success');
            }
        };
    </script>
</body>
</html>
