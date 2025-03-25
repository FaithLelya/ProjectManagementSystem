<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTechnicians.aspx.cs" Inherits="ProjectManagementSystem.Views.ProjectManagers.ViewTechnicians" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Technicians</title>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
        }
        .container {
            max-width: 900px;
            margin-top: 20px;
        }
        .card {
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2 class="text-center mb-4">View Technicians</h2>
            
            <div class="card">
                <div class="card-header text-white" style="background-color: #0056b3;">
                    <h5 class="mb-0">All Registered Technicians</h5>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="TechniciansRepeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped table-bordered">
                                <thead class="text-white" style="background-color: #0056b3;">
                                    <tr>
                                        <th>ID</th>
                                        <th>Username</th>
                                        <th>Email</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("UserId") %></td>
                                <td><%# Eval("UserName") %></td>
                                <td><%# Eval("Email") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <!-- Back Button -->
            <div class="text-center mt-3">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
            </div>

        </div>
    </form>
</body>
</html>
