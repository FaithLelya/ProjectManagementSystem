<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="ProjectManagementSystem.Views.Account.Profile" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Profile</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card shadow-lg">
                        <!-- Card Header -->
                        <div class="card-header text-white text-center" style="background-color: #0056b3;">
                            <h2>My Profile</h2>
                        </div>
                        <!-- Card Body -->
                        <div class="card-body">
                            <div class="form-group">
                                <label for="txtName" class="font-weight-bold">Name:</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="txtEmail" class="font-weight-bold">Email Address:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            </div>
                            <!-- Password Section (Collapsible) -->
                            <button class="btn btn-warning btn-sm mb-3" type="button" data-toggle="collapse" data-target="#passwordSection">
                                Toggle Password Section
                            </button>
                            <div id="passwordSection" class="collapse">
                                <h5 class="text-danger font-weight-bold">Change Password</h5>
                                <div class="form-group">
                                    <label for="txtOldPassword" class="font-weight-bold">Old Password:</label>
                                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <div class="form-group">
                                    <label for="txtNewPassword" class="font-weight-bold">New Password:</label>
                                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <div class="form-group">
                                    <label for="txtConfirmPassword" class="font-weight-bold">Confirm New Password:</label>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="chkChangePassword" runat="server" CssClass="form-check-input" />
                                    <label for="chkChangePassword" class="form-check-label ml-2">Confirm password change</label>
                                </div>
                            </div>
                            <!-- Alert Message -->
                            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3 d-none" role="alert"></asp:Label>
                            <!-- Buttons -->
                            <div class="d-flex justify-content-between mt-4">
                                <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary btn-lg" OnClick="btnSave_Click" />
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary btn-lg" OnClick="btnBack_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script>
        window.onload = function () {
            var messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
            if (messageLabel.innerHTML.trim() !== "") {
                messageLabel.classList.remove('d-none');
            }
        };
    </script>
</body>
</html>
