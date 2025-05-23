﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="ProjectManagementSystem.Views.Shared.Dashboard.Welcome" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IT DOLLS - Project Management System</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" rel="stylesheet" />
    <style>
        :root {
            --primary-blue: #0056b3;
            --secondary-blue: #e3f2fd;
            --light-white: #f8f9fa;
            --sidebar-width: 250px;
            --topbar-height: 60px;
            --sidebar-collapsed-width: 70px;
        }

        body {
            background-color: var(--light-white);
            font-family: 'Segoe UI', Arial, sans-serif;
            overflow-x: hidden;
        }

        #wrapper {
            display: flex;
            width: 100%;
            align-items: stretch;
        }

          #sidebar {
            min-width: var(--sidebar-width);
            max-width: var(--sidebar-width);
            background: var(--primary-blue);
            color: #fff;
            transition: all 0.3s;
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            z-index: 999;
            box-shadow: 3px 0 10px rgba(0, 0, 0, 0.1);
        }

        #sidebar.collapsed {
            min-width: var(--sidebar-collapsed-width);
            max-width: var(--sidebar-collapsed-width);
        }

        #sidebar.collapsed .sidebar-header h3 {
            display: none;
        }

        #sidebar.collapsed .sidebar-header img {
            max-width: 40px;
        }

        #sidebar.collapsed .sidebar-item span {
            display: none;
        }

        #sidebar.collapsed .sidebar-item i {
            font-size: 1.5rem;
            margin-right: 0;
        }

        #sidebar.collapsed .sidebar-footer {
            justify-content: center;
        }

        #sidebar.collapsed .sidebar-footer span {
            display: none;
        }

        .sidebar-header {
            padding: 15px;
            background: rgba(0, 0, 0, 0.1);
            display: flex;
            align-items: center;
        }

        .sidebar-header img {
            max-width: 40px;
            margin-right: 10px;
            transition: all 0.3s;
        }

        .sidebar-header h3 {
            margin: 0;
            font-size: 1.2rem;
            font-weight: bold;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .sidebar-menu {
            padding: 20px 0;
            height: calc(100vh - var(--topbar-height) - 80px);
            overflow-y: auto;
        }

        .sidebar-item {
            padding: 12px 15px;
            display: flex;
            align-items: center;
            color: #fff;
            text-decoration: none;
            transition: all 0.3s;
            border-left: 3px solid transparent;
        }


        .sidebar-item:hover {
            background: white;
            color: var(--primary-blue);
            border-left: 3px solid var(--primary-blue);
        }

        .sidebar-item:hover i {
            color: var(--primary-blue);
        }
        .sidebar-item i {
            margin-right: 15px;
            font-size: 1.2rem;
            width: 20px;
            text-align: center;
        }

        .sidebar-item.active {
            background: white;
            color: var(--primary-blue);
            border-left: 3px solid var(--primary-blue);
        }

        .sidebar-item span {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .sidebar-footer {
            padding: 15px;
            display: flex;
            align-items: center;
            position: absolute;
            bottom: 0;
            width: 100%;
            background: rgba(0, 0, 0, 0.1);
        }

        #content {
            width: 100%;
            padding: 0;
            min-height: 100vh;
            transition: all 0.3s;
            margin-left: var(--sidebar-width);
        }

        #content.expanded {
            margin-left: var(--sidebar-collapsed-width);
        }

        .topbar {
            height: var(--topbar-height);
            background: #fff;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 20px;
            position: fixed;
            top: 0;
            right: 0;
            width: calc(100% - var(--sidebar-width));
            z-index: 998;
            transition: all 0.3s;
        }

        .topbar.expanded {
            width: calc(100% - var(--sidebar-collapsed-width));
        }

        .toggle-sidebar {
            background: none;
            border: none;
            color: #333;
            font-size: 1.2rem;
            cursor: pointer;
        }

        .user-profile {
            display: flex;
            align-items: center;
        }

        .user-profile-img {
            width: 35px;
            height: 35px;
            border-radius: 50%;
            background-color: var(--primary-blue);
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            margin-right: 10px;
        }

        .user-info {
            margin-right: 15px;
        }

        .user-info span {
            display: block;
        }

        .username {
            font-weight: bold;
        }

        .user-role {
            font-size: 0.8rem;
            color: #6c757d;
        }

        .btn-logout {
            color: var(--primary-blue);
            border: 1px solid var(--primary-blue);
            padding: 0.375rem 0.75rem;
            border-radius: 4px;
            text-decoration: none;
            background: none;
            transition: all 0.3s ease;
        }

        .btn-logout:hover {
            background-color: var(--primary-blue);
            color: white;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            transform: translateY(-2px);
        }

        .main-content {
            padding: 80px 20px 20px;
            transition: all 0.3s;
        }

        .welcome-banner {
            background-color: white;
            border-radius: 10px;
            padding: 30px;
            margin-bottom: 30px;
            box-shadow: 0 2px 15px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

        .welcome-title {
            color: var(--primary-blue);
            font-size: 2rem;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .role-message {
            color: var(--primary-blue);
            font-size: 1.1rem;
            margin-top: 15px;
        }

        .dashboard-cards {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
        }

        .dashboard-card {
            background-color: white;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 2px 15px rgba(0, 0, 0, 0.1);
            flex: 1 1 300px;
            min-height: 150px;
        }

        .category-header {
            color: var(--primary-blue);
            font-size: 1.3rem;
            margin-bottom: 15px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }

        .assigned-project {
            padding: 15px;
            border-bottom: 1px solid #eee;
        }

        .assigned-project:last-child {
            border-bottom: none;
        }

        .project-title {
            font-weight: 600;
            color: #333;
        }

        .project-status {
            padding: 3px 8px;
            border-radius: 12px;
            font-size: 0.8rem;
            font-weight: 500;
        }

        .status-ongoing {
            background-color: #e3f2fd;
            color: #0056b3;
        }

        .status-completed {
            background-color: #e8f5e9;
            color: #2e7d32;
        }

        .status-pending {
            background-color: #fff8e1;
            color: #f57f17;
        }

        .status-delayed {
            background-color: #ffebee;
            color: #c62828;
        }

        .action-icon {
            font-size: 1.5rem;
            margin-bottom: 10px;
            color: var(--primary-blue);
        }

        .quick-action-card {
            text-align: center;
            padding: 15px;
            transition: all 0.3s;
            border-radius: 8px;
            margin-bottom: 15px;
        }

        .quick-action-card:hover {
            background-color: var(--secondary-blue);
            transform: translateY(-3px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        /* Mobile responsive styles */
        @media (max-width: 768px) {
            #sidebar {
                margin-left: calc(-1 * var(--sidebar-width));
            }

            #sidebar.collapsed {
                margin-left: 0;
            }

            #content {
                margin-left: 0;
            }

            #content.expanded {
                margin-left: 0;
            }

            .topbar {
                width: 100%;
            }

            .topbar.expanded {
                width: 100%;
            }

            .sidebar-overlay {
                display: none;
                position: fixed;
                top: 0;
                left: 0;
                width: 100vw;
                height: 100vh;
                background: rgba(0, 0, 0, 0.5);
                z-index: 998;
            }

            .sidebar-overlay.active {
                display: block;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <!-- Sidebar -->
             <nav id="sidebar" class="bg-primary">
                <div class="sidebar-header p-3 text-center">
                    <img src="/Content/images/itdolls_logo.png" alt="IT DOLLS Logo" class="img-fluid mb-2" style="max-width: 100px;" />
                    <h3 class="text-white">IT DOLLS PMS</h3>
                </div>

                <div class="sidebar-menu">
                    <!-- Menu Items with Bootstrap and Hover Effects -->
                    <asp:LinkButton ID="btnDashboard" runat="server" CssClass="sidebar-item active d-flex align-items-center text-decoration-none" OnClick="btnDashboard_Click">
                        <i class="fas fa-home mr-3"></i>
                        <span>Dashboard</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnProjects" runat="server" CssClass="sidebar-item d-flex align-items-center text-decoration-none" OnClick="btnProjects_Click">
                        <i class="fas fa-tasks mr-3"></i>
                        <span>Projects</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnResources" runat="server" CssClass="sidebar-item" OnClick="btnResources_Click">
                        <i class="fas fa-tools"></i>
                        <span>Resources</span>
                    </asp:LinkButton>

                    <!-- Technician-specific Menu Items -->
                    <asp:LinkButton ID="btnAttendance" runat="server" CssClass="sidebar-item" OnClick="btnAttendance_Click" Visible='<%# IsUserTechnician() %>'>
                        <i class="fas fa-clock"></i>
                        <span>Attendance</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnPaymentInfo" runat="server" CssClass="sidebar-item" OnClick="btnPaymentInfo_Click" Visible='<%# IsUserTechnician() %>'>
                        <i class="fas fa-money-bill-wave"></i>
                        <span>Payment Details</span>
                    </asp:LinkButton>

                    <!-- Project Manager-specific Menu Items -->
                    <asp:LinkButton ID="btnAssignTechnicians" runat="server" CssClass="sidebar-item" OnClick="btnAssignTechnicians_Click" Visible='<%# IsProjectManager() %>'>
                        <i class="fas fa-users"></i>
                        <span>View Technicians</span>
                    </asp:LinkButton>

                    <!-- Admin-specific Menu Items -->
                    <asp:LinkButton ID="btnCreateProject" runat="server" CssClass="sidebar-item" OnClick="btnCreateProject_Click" Visible='<%# IsAdmin() %>'>
                        <i class="fas fa-plus-circle"></i>
                        <span>Create Project</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnReports" runat="server" CssClass="sidebar-item" OnClick="btnReports_Click" Visible='<%# IsAdmin() %>'>
                        <i class="fas fa-chart-bar"></i>
                        <span>Reports</span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnCreateUser" runat="server" CssClass="sidebar-item" OnClick="btnCreateUser_Click" Visible='<%# IsAdmin() %>'>
                        <i class="fas fa-user-plus"></i>
                        <span>Create User</span>
                    </asp:LinkButton>

                    <!-- Account Settings (Common) -->
                    <asp:LinkButton ID="btnAccount" runat="server" CssClass="sidebar-item" OnClick="btnAccount_Click">
                        <i class="fas fa-user-cog"></i>
                        <span>My Account</span>
                    </asp:LinkButton>
                </div>

                <div class="sidebar-footer mt-auto p-3">
                    <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-outline-light w-100" OnClick="btnLogout_Click">
                        <i class="fas fa-sign-out-alt mr-2"></i>Logout
                    </asp:LinkButton>
                </div>
            </nav>

            <div id="content" class="w-100">
                <!-- Top Navigation Bar -->
                <div class="topbar bg-white shadow-sm d-flex justify-content-between align-items-center p-3">
                    <button type="button" class="btn btn-outline-primary toggle-sidebar">
                        <i class="fas fa-bars"></i>
                    </button>
                    
                    <div class="user-profile d-flex align-items-center">
                        <div class="user-profile-img bg-primary text-white rounded-circle mr-3 d-flex align-items-center justify-content-center" style="width: 45px; height: 45px;">
                            <asp:Literal ID="ltUserInitial" runat="server"></asp:Literal>
                        </div>
                        <div class="user-info mr-3">
                            <div class="font-weight-bold"><asp:Label ID="lblUsername" runat="server"></asp:Label></div>
                            <small class="text-muted"><asp:Label ID="lblRole" runat="server"></asp:Label></small>
                        </div>
                        <asp:LinkButton ID="btnLogoutTop" runat="server" CssClass="btn btn-outline-primary" OnClick="btnLogout_Click">
                            <i class="fas fa-sign-out-alt mr-1"></i>Logout
                        </asp:LinkButton>
                    </div>
                </div>

                <!-- Main Content Area -->
                <div class="main-content container-fluid mt-5">
                    <!-- Welcome Banner with Bootstrap Card -->
                    <div class="card mb-4 shadow-sm">
                        <div class="card-body text-center">
                            <h1 class="card-title text-primary">Welcome to IT DOLLS</h1>
                            <p class="card-text text-muted">Project Management System</p>
                            <div class="alert alert-info" role="alert">
                                <asp:Label ID="lblRoleSpecificMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Dashboard Cards Section -->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card mb-4 shadow-sm">
                                <div class="card-header bg-primary text-white">
                                    Quick Actions
                                </div>
                                <div class="card-body">
                                  <!-- Admin Quick Actions -->
                                  <asp:Panel ID="pnlAdminQuickActions" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-plus-circle action-icon"></i>
                                                <asp:Button ID="btnNewProject" runat="server" 
                                                    Text="New Project" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnNewProject_Click" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-tasks action-icon"></i>
                                                <asp:Button ID="btnAssignTask" runat="server" 
                                                    Text="Assign Task" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnAssignTask_Click" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-chart-pie action-icon"></i>
                                                <asp:Button ID="btnGenerateReport" runat="server" 
                                                    Text="Generate Report" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnGenerateReport_Click" />
                                            </div>
                                        </div>
                                    </div>
                                  </asp:Panel>
                                    
                                  <!-- Project Manager Quick Actions -->
                                  <asp:Panel ID="pnlProjectManagerQuickActions" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-clipboard-list action-icon"></i>
                                                <asp:Button ID="btnViewProjects" runat="server" 
                                                    Text="View Projects" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnViewProjects_Click" />
                                            </div>                                            
                                            
                                        </div>
                                        <div class="col-md-4">
                                        <div class="quick-action-card">
                                             <i class="fas fa-users action-icon" style="font-size: 2rem; color: #0d6efd;"></i>
                                            <asp:Button ID="btnViewTechnicians" runat="server" 
                                                Text="View Technicians" 
                                                CssClass="btn btn-outline-primary btn-block" 
                                                OnClick="btnViewTechnicians_Click" />
                                        </div>
                                            </div>
                                      
                                    </div>
                                  </asp:Panel>
                                    
                                  <!-- Technician Quick Actions -->
                                  <asp:Panel ID="pnlTechnicianQuickActions" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-clock action-icon"></i>
                                                <asp:Button ID="btnRecordAttendance" runat="server" 
                                                    Text="Record Attendance" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnRecordAttendance_Click" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="quick-action-card">
                                                <i class="fas fa-list-alt action-icon"></i>
                                                <asp:Button ID="btnViewTasks" runat="server" 
                                                    Text="View Recorded attendance" 
                                                    CssClass="btn btn-outline-primary btn-block" 
                                                    OnClick="btnViewTasks_Click" />
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                           
                                        </div>
                                    </div>
                                  </asp:Panel>
                                </div>
                            </div>
                            
                            <!-- Assigned Projects Section -->
                            <div class="card mb-4 shadow-sm">
                                <div class="card-header bg-primary text-white">
                                    My Assigned Projects
                                </div>
                                <div class="card-body p-0">
                                    <asp:Panel ID="pnlAssignedProjects" runat="server">
                                        <asp:Repeater ID="rptAssignedProjects" runat="server">
                                            <ItemTemplate>
                                                <div class="assigned-project">
                                                    <div class="d-flex justify-content-between align-items-center">
                                                        <span class="project-title"><%# Eval("ProjectName") %></span>
                                                        <span class="project-status <%# GetStatusClass(Eval("Status").ToString()) %>">
                                                            <%# Eval("Status") %>
                                                        </span>
                                                    </div>
                                                    <div class="d-flex justify-content-between mt-2">
                                                        <small class="text-muted">Due: <%# Eval("DueDate", "{0:MMM dd, yyyy}") %></small>
                                                        <small class="text-muted">
                                                            Tasks: <%# Eval("CompletedTasks") %>/<%# Eval("TaskCount") %>
                                                            (<%# Eval("ProgressPercentage") %>%)
                                                        </small>
                                                    </div>
                                                    <div class="progress mt-2" style="height: 5px;">
                                                        <div class="progress-bar" role="progressbar" 
                                                             style="width: <%# Eval("ProgressPercentage") %>%" 
                                                             aria-valuenow="<%# Eval("ProgressPercentage") %>" 
                                                             aria-valuemin="0" 
                                                             aria-valuemax="100"></div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Label ID="lblNoProjects" runat="server" Text="No projects assigned" 
                                            CssClass="text-muted text-center d-block p-3" Visible="false"></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="card mb-4 shadow-sm">
                                <div class="card-header bg-primary text-white">
                                    Recent Activities
                                </div>
                                <div class="card-body">
                                    <asp:Repeater ID="rptRecentActivities" runat="server">
                                        <ItemTemplate>
                                            <div class="d-flex mb-3">
                                                <div class="flex-shrink-0 mr-3">
                                                    <i class="fas <%# GetActivityIcon(Eval("ActivityType").ToString()) %> text-primary"></i>
                                                </div>
                                                <div class="flex-grow-1">
                                                    <p class="mb-0"><%# Eval("Description") %></p>
                                                    <small class="text-muted"><%# Eval("Timestamp", "{0:MMM dd, yyyy hh:mm tt}") %></small>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNoActivities" runat="server" Text="No recent activities" 
                                                CssClass="text-muted text-center d-block" 
                                                Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            
                            <!-- Upcoming Deadlines Section -->
                            <div class="card mb-4 shadow-sm">
                                <div class="card-header bg-primary text-white">
                                    Upcoming Deadlines
                                </div>
                                <div class="card-body p-0">
                                    <asp:Repeater ID="rptUpcomingDeadlines" runat="server">
                                        <ItemTemplate>
                                            <div class="assigned-project">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <span class="project-title"><%# Eval("TaskName") %></span>
                                                    <span class="text-danger font-weight-bold">
                                                        <i class="fas fa-calendar-alt mr-1"></i>
                                                        <%# Eval("DueDate", "{0:MMM dd, yyyy}") %>
                                                    </span>
                                                </div>
                                                <small class="text-muted d-block mt-1">Project: <%# Eval("ProjectName") %></small>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNoDeadlines" runat="server" Text="No upcoming deadlines" 
                                                CssClass="text-muted text-center d-block p-3" 
                                                Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sidebar-overlay"></div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        $(document).ready(function () {
            // Toggle sidebar
            $('.toggle-sidebar').on('click', function () {
                $('#sidebar').toggleClass('collapsed');
                $('#content').toggleClass('expanded');
                $('.topbar').toggleClass('expanded');
                $('.sidebar-overlay').toggleClass('active');
            });

            // Close sidebar when clicking on overlay (mobile view)
            $('.sidebar-overlay').on('click', function () {
                $('#sidebar').addClass('collapsed');
                $('.sidebar-overlay').removeClass('active');
            });

            // Handle active menu item
            $('.sidebar-item').on('click', function () {
                $('.sidebar-item').removeClass('active');
                $(this).addClass('active');
            });

            // Adjust for mobile view on page load
            function checkWidth() {
                if ($(window).width() < 768) {
                    $('#sidebar').addClass('collapsed');
                    $('#content').removeClass('expanded');
                    $('.topbar').removeClass('expanded');
                }
            }

            // Initial check and resize handler
            checkWidth();
            $(window).resize(function () {
                checkWidth();
            });
        });
    </script>
</body>
</html>