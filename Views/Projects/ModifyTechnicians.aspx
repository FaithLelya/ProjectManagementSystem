<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyTechnicians.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyTechnicians" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Technicians</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
</head>
<body class="bg-light">
    <form id="form2" runat="server">
        <div class="container mt-5">
            <div class="card shadow-lg">
                <div class="card-header bg-primary text-white">
                    <h2 class="text-center">Modify Project Technicians</h2>
                </div>
                <div class="card-body">
                    <h4 class="text-muted">Project:</h4>
                    <h3 class="mb-4"><asp:Literal ID="litProjectName" runat="server" /></h3>

                    <div class="form-group">
                        <label for="ddlTechnicians">Select Technician:</label>
                        <asp:DropDownList ID="ddlTechnicians" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Select Technician" Value=""/>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="d-flex justify-content-between">
                        <asp:Button ID="btnSaveChanges" runat="server" Text="Assign Technician" CssClass="btn btn-primary" OnClick="btnSaveChanges_Click" />
                        <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="btn btn-secondary" OnClick="btnReturn_Click" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-3 d-none" role="alert"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <script>
        // Show message alert dynamically
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
