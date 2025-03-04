<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="ProjectManagementSystem.Views.Shared.Dashboard.Welcome" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IT DOLLS - Project Management System</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        :root {
            --primary-blue: #0056b3;
            --secondary-blue: #e3f2fd;
        }

        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Arial, sans-serif;
        }

        .welcome-container {
            padding: 2rem;
        }

        .logo-section {
            text-align: center;
            margin-bottom: 2rem;
            padding: 2rem;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 2px 15px rgba(0,0,0,0.1);
        }

        .company-logo {
            max-width: 200px;
            margin-bottom: 1rem;
        }

        .welcome-title {
            color: var(--primary-blue);
            font-size: 2.5rem;
            font-weight: bold;
            margin: 1rem 0;
        }

        .nav-card {
            background-color: white;
            border-radius: 10px;
            padding: 1.5rem;
            margin: 1rem 0;
            transition: transform 0.3s ease;
            border: none;
            box-shadow: 0 2px 15px rgba(0,0,0,0.1);
        }

        .nav-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0,0,0,0.15);
        }

        .nav-icon {
            font-size: 2rem;
            color: var(--primary-blue);
            margin-bottom: 1rem;
        }

        .header-bar {
            background-color: var(--primary-blue);
            padding: 1rem;
            color: white;
            margin-bottom: 2rem;
        }

        .user-profile {
            text-align: right;
        }

        .btn-logout {
            color: white;
            border: 1px solid white;
            padding: 0.375rem 0.75rem;
            border-radius: 4px;
            text-decoration: none;
        }

        .btn-logout:hover {
            background-color: white;
            color: var(--primary-blue);
        }

        .role-message {
            text-align: center;
            margin-bottom: 2rem;
            color: var(--primary-blue);
            font-size: 1.2rem;
        }

        .disabled-card {
            opacity: 0.6;
            cursor: not-allowed;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header Bar -->
        <div class="header-bar">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-md-6">
                        <h5 class="mb-0">IT DOLLS Project Management System</h5>
                    </div>
                    <div class="col-md-6">
                        <div class="user-profile">
                            Welcome, <asp:Label ID="lblUsername" runat="server" CssClass="mr-3"></asp:Label>
                            (<asp:Label ID="lblRole" runat="server" CssClass="mr-3"></asp:Label>)
                            <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn-logout" OnClick="btnLogout_Click">Logout</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container welcome-container">
            <!-- Logo and Welcome Section -->
            <div class="logo-section">
                <img src="/Content/images/itdolls_logo.png" alt="IT DOLLS Logo" class="company-logo" />
                <h1 class="welcome-title">Welcome to IT DOLLS</h1>
                <p class="lead text-muted">Project Management System</p>
                <div class="role-message">
                    <asp:Label ID="lblRoleSpecificMessage" runat="server"></asp:Label>
                </div>
            </div>

            <!-- Navigation Cards -->
            <div class="row">
                <!-- Technician-specific Panels -->
                <asp:Panel ID="pnlTimeLogging" runat="server" CssClass="col-md-3" Visible='<%# IsUserTechnician() %>'>
                    <asp:LinkButton ID="btnTimeLogging" runat="server" OnClick="btnTimeLogging_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">⏱️</div>
                            <h4 class="text-dark">Log Hours</h4>
                            <p class="text-muted">Track your work hours and view estimated payments</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

                <asp:Panel ID="pnlPaymentInfo" runat="server" CssClass="col-md-3" Visible='<%# IsUserTechnician() %>'>
                    <asp:LinkButton ID="btnPaymentInfo" runat="server" OnClick="btnPaymentInfo_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">💰</div>
                            <h4 class="text-dark">Payment Details</h4>
                            <p class="text-muted">View your payments and incentives</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

                <!-- Project Manager-specific Panels -->

                <asp:Panel ID="pnlAssignTechnicians" runat="server" CssClass="col-md-3" Visible='<%# IsProjectManager() %>'>
                    <asp:LinkButton ID="btnAssignTechnicians" runat="server" OnClick="btnAssignTechnicians_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">👥</div>
                            <h4 class="text-dark">Assign Technicians</h4>
                            <p class="text-muted">Manage team assignments</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

                <!-- Admin-specific Panels -->
                <asp:Panel ID="pnlCreateProject" runat="server" CssClass="col-md-3"  Visible='<%# IsAdmin() %>'>
                <asp:LinkButton ID="btnCreateProject" runat="server" OnClick="btnCreateProject_Click" CssClass="nav-card d-block text-decoration-none">
                    <div class="text-center">
                        <div class="nav-icon">➕</div>
                        <h4 class="text-dark">Create Project</h4>
                        <p class="text-muted">Start a new project</p>
                    </div>
                </asp:LinkButton>
            </asp:Panel>

                <asp:Panel ID="pnlReports" runat="server" CssClass="col-md-3" Visible='<%# IsAdmin() %>'>
                    <asp:LinkButton ID="btnReports" runat="server" OnClick="btnReports_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">📊</div>
                            <h4 class="text-dark">Reports</h4>
                            <p class="text-muted">View Project Reports</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>
                <asp:Panel ID="pnlCreateUser" runat="server" CssClass="col-md-3" Visible='<%# IsAdmin() %>'>
                    <asp:LinkButton ID="btnCreateUser" runat="server" OnClick="btnCreateUser_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">👤</div>
                            <h4 class="text-dark">Create User</h4>
                            <p class="text-muted">Add new users to the system</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>
                <!-- Common Panels -->
                <asp:Panel ID="pnlProjects" runat="server" CssClass="col-md-3">
                    <asp:LinkButton ID="btnProjects" runat="server" OnClick="btnProjects_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">📈</div>
                            <h4 class="text-dark">Projects</h4>
                            <p class="text-muted">View project details and status</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

                <asp:Panel ID="pnlResources" runat="server" CssClass="col-md-3">
                    <asp:LinkButton ID="btnResources" runat="server" OnClick="btnResources_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">🔧</div>
                            <h4 class="text-dark">Resources</h4>
                            <p class="text-muted">View available resources</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

                <asp:Panel ID="pnlAccount" runat="server" CssClass="col-md-3">
                    <asp:LinkButton ID="btnAccount" runat="server" OnClick="btnAccount_Click" CssClass="nav-card d-block text-decoration-none">
                        <div class="text-center">
                            <div class="nav-icon">👤</div>
                            <h4 class="text-dark">My Account</h4>
                            <p class="text-muted">Manage your profile</p>
                        </div>
                    </asp:LinkButton>
                </asp:Panel>

            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>