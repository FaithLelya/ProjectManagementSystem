<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- FontAwesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <style>
        :root {
            --primary: #007bff;
            --primary-light: #eef2f9;
            --secondary: #6c757d;
            --success: #28a745;
            --info: #17a2b8;
            --warning: #ffc107;
            --danger: #dc3545;
            --light: #f8f9fa;
            --dark: #343a40;
        }

        body {
            background-color: #f5f7fa;
            font-family: 'Segoe UI', Arial, sans-serif;
        }

        .page-header {
            background: linear-gradient(135deg, var(--primary), #2c4d82);
            color: white;
            padding: 20px 0;
            margin-bottom: 30px;
            border-radius: 0 0 15px 15px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }

        .status-badge {
            font-size: 0.75rem;
            font-weight: 600;
            padding: 6px 12px;
            border-radius: 20px;
            display: inline-block;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .completed { background-color: #e6f7ee; color: #146c43; border: 1px solid #28a745; }
        .in-progress { background-color: #e6f2ff; color: #0d6efd; border: 1px solid #0d6efd; }
        .on-hold { background-color: #fff8e6; color: #997404; border: 1px solid #ffc107; }
        .delayed { background-color: #ffebee; color: #b71c1c; border: 1px solid #dc3545; }
        .not-started { background-color: #f0f0f0; color: #495057; border: 1px solid #6c757d; }
        .past-due { background-color: #ffebee; color: #b71c1c; border: 1px solid #dc3545; }

        .card {
            border: none;
            border-radius: 12px;
            overflow: hidden;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(0,0,0,0.05);
            margin-bottom: 25px;
        }
        
        .card:hover {
            transform: translateY(-7px);
            box-shadow: 0 10px 25px rgba(0,0,0,0.1);
        }

        .card-header {
            background: linear-gradient(to right, var(--primary), #5482c9);
            color: white;
            font-size: 1.1rem;
            font-weight: 600;
            padding: 15px 20px;
            border-bottom: none;
        }

        .card-body {
            padding: 20px;
        }

        .list-group-item {
            border-left: none;
            border-right: none;
            padding: 12px 20px;
            color: #495057;
            background: transparent;
        }
        
        .list-group-item:first-child {
            border-top: none;
        }

        .section-heading {
            font-size: 1.1rem;
            font-weight: 600;
            color: var(--primary);
            margin-top: 20px;
            margin-bottom: 15px;
            padding-bottom: 8px;
            border-bottom: 2px solid var(--primary-light);
        }

        .btn-primary {
            background-color: var(--primary);
            border-color: var(--primary);
        }
        
        .btn-outline-primary {
            color: var(--primary);
            border-color: var(--primary);
        }
        
        .btn-outline-primary:hover {
            background-color: var(--primary);
            color: white;
        }
        
        .btn-success {
            background-color: var(--success);
            border-color: var(--success);
        }

        .action-button {
            border-radius: 6px;
            padding: 8px 16px;
            font-weight: 500;
            transition: all 0.2s;
            margin-top: 15px;
            display: block;
            width: 100%;
        }

        .empty-message {
            color: #97a0af;
            font-style: italic;
            text-align: center;
            padding: 15px;
            background-color: #f8f9fa;
            border-radius: 6px;
        }

        .technician-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 15px;
        }

        .senior-badge {
            background-color: #198754;
            color: white;
            font-size: 0.7rem;
            padding: 3px 8px;
            border-radius: 10px;
            margin-left: 5px;
            font-weight: 600;
        }
        
        .key-label {
            font-weight: 600;
            color: #495057;
        }
        
        .value-text {
            color: #212529;
        }
        
        .financial-highlight {
            background-color: #f8f9fa;
            border-radius: 8px;
            padding: 15px;
            margin-top: 10px;
            border-left: 4px solid var(--primary);
        }
        
        .back-button {
            display: inline-flex;
            align-items: center;
            padding: 8px 16px;
            font-weight: 500;
            border-radius: 6px;
            transition: all 0.2s;
        }
        
        .back-button i {
            margin-right: 8px;
        }
        
        .resource-list-item, .technician-list-item {
            background-color: #f8f9fa;
            margin-bottom: 8px;
            border-radius: 6px;
            transition: all 0.2s;
        }
        
        .resource-list-item:hover, .technician-list-item:hover {
            background-color: #eef2f9;
        }
        
        .past-due-alert {
            background-color: #fff8f8;
            border-left: 4px solid #dc3545;
            padding: 15px;
            margin-top: 15px;
            border-radius: 6px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        
        .past-due-alert i {
            color: #dc3545;
            margin-right: 10px;
            font-size: 1.2rem;
        }
        
        .completion-confirm-btn {
            margin-left: 15px;
            white-space: nowrap;
        }

        /* Modal styles */
        .modal-header {
            background: linear-gradient(to right, var(--primary), #5482c9);
            color: white;
        }
        
        .modal-title {
            font-weight: 600;
        }
        
        .modal-footer .btn-success {
            background-color: var(--success);
            border-color: var(--success);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Page Header -->
        <div class="page-header">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-auto">
                        <a href="/Views/Shared/Dashboard/Welcome.aspx" class="btn btn-light back-button">
                            <i class="fas fa-arrow-left"></i> Back to Dashboard
                        </a>
                    </div>
                    <div class="col text-center">
                        <h1 class="mb-0">Projects Dashboard</h1>
                    </div>
                    <div class="col-auto">
                        <!-- Empty div for alignment -->
                    </div>
                </div>
            </div>
        </div>
        
        <div class="container py-3">
            <!-- Success Alert for Completion -->
            <asp:Panel ID="CompletionSuccessPanel" runat="server" Visible="false" CssClass="alert alert-success mb-4" role="alert">
                <i class="fas fa-check-circle me-2"></i>
                <asp:Literal ID="CompletionMessage" runat="server"></asp:Literal>
                <button type="button" class="btn-close float-end" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>
            
            <div class="row row-cols-1 row-cols-lg-2 g-4">
                <asp:Repeater ID="ProjectRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100">
                                <div class="card-header d-flex justify-content-between align-items-center">
                                    <span><%# Eval("ProjectName") %></span>
                                    <span class="status-badge <%# Eval("Status").ToString().ToLower().Replace(" ", "-") %>">
                                        <%# Eval("Status") %>
                                    </span>
                                </div>
                                <div class="card-body">
                                    <!-- Project Info -->
                                    <div class="row">
                                        <div class="col-md-6">
                                            <p><span class="key-label">Project ID:</span> <span class="value-text"><%# Eval("ProjectId") %></span></p>
                                            <p><span class="key-label">Location:</span> <span class="value-text"><%# Eval("Location") %></span></p>
                                        </div>
                                        <div class="col-md-6">
                                            <p><span class="key-label">Start:</span> <span class="value-text"><%# Eval("StartDate", "{0:MMM dd, yyyy}") %></span></p>
                                            <p><span class="key-label">End:</span> <span class="value-text"><%# Eval("EndDate", "{0:MMM dd, yyyy}") %></span></p>
                                        </div>
                                    </div>
                                    
                                    <div class="mt-3">
                                        <p><span class="key-label">Description:</span></p>
                                        <p class="value-text"><%# Eval("Description") %></p>
                                    </div>
                                    
                                    <!-- Past Due Project Alert -->
                                    <asp:Panel ID="PastDuePanel" runat="server" 
                                              Visible='<%# IsProjectPastDue((DateTime)Eval("EndDate")) && Eval("Status").ToString() != "Completed" %>'>
                                        <div class="past-due-alert">
                                            <div>
                                                <i class="fas fa-exclamation-triangle"></i>
                                                Project end date has passed. Is this project complete?
                                            </div>
                                            <asp:Button ID="btnConfirmCompletion" runat="server" 
                                                      Text="Mark as Complete" 
                                                      CssClass="btn btn-success btn-sm completion-confirm-btn"
                                                      OnClick="btnConfirmCompletion_Click"
                                                      CommandArgument='<%# Eval("ProjectId") %>'
                                                      OnClientClick='<%# "return confirmProjectCompletion(" + Eval("ProjectId") + ", \"" + Eval("ProjectName") + "\");" %>' />
                                        </div>
                                    </asp:Panel>
                                    
                                    <div class="financial-highlight">
                                        <p class="mb-0"><span class="key-label">Budget:</span> <span class="value-text">KES <%# Eval("Budget", "{0:N2}") %></span></p>
                                    </div>

                                    <!-- Financial Details -->
                                    <asp:Panel ID="FinancialPanel" runat="server" CssClass="mt-4" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanViewFinancials() %>'>
                                        <h5 class="section-heading">
                                            <i class="fas fa-chart-pie me-2"></i>Financial Details
                                        </h5>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <p><span class="key-label">Resource Cost:</span> <span class="value-text">KES <%# Eval("TotalResourceCost", "{0:N2}") %></span></p>
                                                <p><span class="key-label">Materials Cost:</span> <span class="value-text">KES <%# Eval("MaterialsCost", "{0:N2}") %></span></p>
                                            </div>
                                            <div class="col-md-6">
                                                <p><span class="key-label">Technician Payments:</span> <span class="value-text">KES <%# Eval("TechnicianPayment", "{0:N2}") %></span></p>
                                                <p><span class="key-label">Total Expense:</span> <span class="value-text">KES <%# Eval("TotalExpense", "{0:N2}") %></span></p>
                                            </div>
                                        </div>
                                        
                                        <!-- Modify Budget Button (Only for Admins) -->
                                        <asp:Panel ID="BudgetButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyBudget() && !((ProjectManagementSystem.Views.Projects.Projects)Page).IsProjectCompleted(Eval("Status").ToString()) %>'>
                                            <asp:Button ID="btnModifyBudget" runat="server" Text="Modify Budget" 
                                                      CssClass="btn btn-outline-primary action-button" OnClick="btnModifyBudget_Click" 
                                                      CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </asp:Panel>
                                    <!-- Delete Project Button (Only for Admins) -->
                                     <asp:Panel ID="DeleteButtonPanel" runat="server" Visible='<%# Session["UserRole"]?.ToString() == "Admin" %>'>
                                          <asp:Button ID="btnDeleteProject" runat="server" Text="Delete Project" 
                                                    CssClass="btn btn-danger action-button mt-3" OnClick="btnDeleteProject_Click" 
                                                    CommandArgument='<%# Eval("ProjectId") %>'
                                                    OnClientClick='<%# "return confirm(\"Are you sure you want to delete project \\\"" + Eval("ProjectName") + "\\\"? This action cannot be undone.\");" %>' />
                                      </asp:Panel>

                                    <!-- Allocated Resources Section -->
                                    <div class="mt-4">
                                        <h5 class="section-heading">
                                            <i class="fas fa-box-open me-2"></i>Allocated Resources
                                        </h5>
                                        <!-- Resources Repeater -->
                                        <asp:Repeater ID="ResourceRepeater" runat="server" DataSource='<%# Eval("AllocatedResources") %>'>
                                            <ItemTemplate>
                                                <div class="resource-list-item p-3">
                                                    <div class="d-flex justify-content-between">
                                                        <span class="fw-medium"><%# Eval("ResourceName") %></span>
                                                        <span class="badge bg-light text-dark">Qty: <%# Eval("Quantity") %></span>
                                                    </div>
                                                    <div class="mt-1 text-muted small">
                                                        Unit Cost: KES <%# Eval("CostPerUnit", "{0:N2}") %>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        
                                        <!-- No Resources Message Panel -->
                                        <asp:Panel ID="NoResourcesPanel" runat="server" Visible='<%# ((System.Collections.ICollection)Eval("AllocatedResources")).Count == 0 %>'>
                                            <div class="empty-message">
                                                <i class="fas fa-info-circle me-2"></i>No resources allocated
                                            </div>
                                        </asp:Panel>
                                        
                                        <!-- Add Resources Button (Only for Project Managers) -->
                                        <asp:Panel ID="ResourceButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyResources() && !((ProjectManagementSystem.Views.Projects.Projects)Page).IsProjectCompleted(Eval("Status").ToString()) %>'>
                                            <asp:Button ID="btnModifyResources" runat="server" Text="Modify Resources" 
                                                        CssClass="btn btn-outline-primary action-button" OnClick="btnModifyResources_Click" 
                                                        CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </div>

                                    <!-- Assigned Technicians Section -->
                                    <div class="mt-4">
                                        <h5 class="section-heading">
                                            <i class="fas fa-users me-2"></i>Assigned Technicians
                                        </h5>
                                        <!-- Technicians Repeater -->
                                        <asp:Repeater ID="TechnicianRepeater" runat="server" DataSource='<%# Eval("AssignedTechnicians") %>'>
                                            <ItemTemplate>
                                                <div class="technician-list-item p-3">
                                                    <div class="d-flex justify-content-between align-items-center">
                                                        <div>
                                                            <i class="fas fa-user-gear me-2 text-secondary"></i>
                                                            <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                                                        </div>
                                                        <span class="senior-badge" style="<%# ((bool)DataBinder.Eval(Container.DataItem, "IsSenior")) ? "display:inline" : "display:none" %>">
                                                            <i class="fas fa-star me-1"></i>Senior
                                                        </span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        
                                        <!-- No Technicians Message Panel -->
                                        <asp:Panel ID="NoTechniciansPanel" runat="server" Visible='<%# ((System.Collections.ICollection)Eval("AssignedTechnicians")).Count == 0 %>'>
                                            <div class="empty-message">
                                                <i class="fas fa-info-circle me-2"></i>No technicians assigned
                                            </div>
                                        </asp:Panel>
                                        
                                        <!-- Modify Technicians Button (Only for Project Managers) -->
                                        <asp:Panel ID="TechnicianButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyTechnicians() && !((ProjectManagementSystem.Views.Projects.Projects)Page).IsProjectCompleted(Eval("Status").ToString()) %>'>
                                            <asp:Button ID="btnModifyTechnicians" runat="server" Text="Modify Technicians" 
                                                        CssClass="btn btn-outline-primary action-button" OnClick="btnModifyTechnicians_Click" 
                                                        CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        
        <!-- Bootstrap JS and dependencies -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        
        <script type="text/javascript">
            function confirmProjectCompletion(projectId, projectName) {
                return confirm("Are you sure you want to mark project '" + projectName + "' as complete?");
            }
        </script>
    </form>
</body>
</html>