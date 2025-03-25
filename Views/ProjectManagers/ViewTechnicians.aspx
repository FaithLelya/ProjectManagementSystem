<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTechnicians.aspx.cs" Inherits="ProjectManagementSystem.Views.ProjectManagers.ViewTechnicians" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Technicians</title>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        :root {
            --primary-color: #4361ee;
            --secondary-color: #3f37c9;
            --accent-color: #4895ef;
            --light-bg: #f8f9fa;
            --dark-text: #212529;
        }
        
        body {
            background-color: #f5f7fb;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        .container {
            max-width: 1200px;
            margin-top: 30px;
        }
        
        .card {
            border: none;
            border-radius: 10px;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.1);
            margin-bottom: 2rem;
        }
        
        .card-header {
            background-color: var(--primary-color);
            color: white;
            border-radius: 10px 10px 0 0 !important;
            padding: 1rem 1.5rem;
            font-weight: 600;
        }
        
        .table th {
            background-color: var(--primary-color);
            color: white;
            border-top: none;
        }
        
        .table td, .table th {
            vertical-align: middle;
            padding: 1rem;
        }
        
        .btn-outline-secondary {
            border-color: var(--secondary-color);
            color: var(--secondary-color);
        }
        
        .btn-outline-secondary:hover {
            background-color: var(--secondary-color);
            color: white;
        }
        
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
            padding-bottom: 15px;
            border-bottom: 1px solid #dee2e6;
        }
        
        .empty-message {
            text-align: center;
            padding: 2rem;
            color: #6c757d;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="page-header">
                <div>
                    <a href="/Views/Shared/Dashboard/Welcome.aspx" class="btn btn-outline-secondary">
                        <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
                    </a>
                </div>
                <h2 class="mb-0">
                    <i class="fas fa-user-cog me-2"></i>View Technicians
                </h2>
                <div></div> <!-- Empty div for alignment -->
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0"><i class="fas fa-list me-2"></i>All Registered Technicians</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:Repeater ID="TechniciansRepeater" runat="server" OnItemDataBound="TechniciansRepeater_ItemDataBound">
                            <HeaderTemplate>
                                <table class="table table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Username</th>
                                            <th>Email</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("UserId") %></td>
                                    <td><%# Eval("UserName") %></td>
                                    <td><%# Eval("Email") %></td>
                                    <td>
                                        <asp:LinkButton ID="btnViewDetails" runat="server" CssClass="btn btn-sm btn-outline-primary" CommandArgument='<%# Eval("UserId") %>' OnClick="btnViewDetails_Click">
                                            <i class="fas fa-eye me-1"></i>View
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        
                        <asp:Label ID="lblNoTechnicians" runat="server" CssClass="empty-message" Text="No technicians found." Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>