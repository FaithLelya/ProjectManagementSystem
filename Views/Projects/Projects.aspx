<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom CSS for project-specific styling -->
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <style>
        .status-badge {
            display: inline-block;
            padding: 0.25rem 0.5rem;
            border-radius: 0.25rem;
            font-weight: 500;
        }
        .completed {
            background-color: #28a745;
            color: white;
        }
        .in-progress {
            background-color: #007bff;
            color: white;
        }
        .on-hold {
            background-color: #ffc107;
            color: black;
        }
        .delayed {
            background-color: #dc3545;
            color: white;
        }
        .not-started {
            background-color: #6c757d;
            color: white;
        }
        .card {
            margin-bottom: 20px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .card-header {
            background-color: #f8f9fa;
            font-weight: bold;
        }
        .action-button {
            margin-top: 10px;
            margin-right: 10px;
        }
        .empty-message {
            color: #6c757d;
            font-style: italic;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-4">
            <h1 class="mb-4 text-center">Projects Dashboard</h1>
            <div class="row">
                <asp:Repeater ID="ProjectRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-6 mb-4">
                            <div class="card h-100">
                                <div class="card-header">
                                    <h4 class="card-title"><%# Eval("ProjectName") %></h4>
                                </div>
                                <div class="card-body">
                                    <!-- Project Info Section -->
                                    <div class="mb-4">
                                        <h5 class="card-subtitle mb-3">Project Information</h5>
                                        <ul class="list-group list-group-flush">
                                            <li class="list-group-item"><strong>Project ID:</strong> <%# Eval("ProjectId") %></li>
                                            <li class="list-group-item"><strong>Description:</strong> <%# Eval("Description") %></li>
                                            <li class="list-group-item"><strong>Status:</strong> 
                                                <span class="status-badge <%# Eval("Status").ToString().ToLower().Replace(" ", "-") %>">
                                                    <%# Eval("Status") %>
                                                </span>
                                            </li>
                                            <li class="list-group-item"><strong>Budget:</strong> <%# Eval("Budget") %></li>

                                            <li class="list-group-item"><strong>Location:</strong> <%# Eval("Location") %></li>
                                            <li class="list-group-item"><strong>Start Date:</strong> <%# Eval("StartDate", "{0:yyyy-MM-dd}") %></li>
                                            <li class="list-group-item"><strong>End Date:</strong> <%# Eval("EndDate", "{0:yyyy-MM-dd}") %></li>
                                        </ul>
                                    </div>

                                    <!-- Financial Section -->
                                    <asp:Panel ID="FinancialPanel" runat="server" CssClass="mb-4" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanViewFinancials() %>'>
                                        <h5 class="card-subtitle mb-3">Financial Details</h5>
                                        <ul class="list-group list-group-flush">
                                            <li class="list-group-item"><strong>Budget:</strong> KES <%# Eval("Budget", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Resource Cost:</strong> KES <%# Eval("TotalResourceCost", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Materials Cost:</strong> KES <%# Eval("MaterialsCost", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Technician Payments:</strong> KES <%# Eval("TechnicianPayment", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Total Expense:</strong> KES <%# Eval("TotalExpense", "{0:N2}") %></li>
                                        </ul>
                                    </asp:Panel>

                                    <!-- Resources Section -->
                                    <div class="mb-4">
                                        <h5 class="card-subtitle mb-3">Allocated Resources</h5>
                                        <div class="list-group mb-3">
                                            <asp:Repeater ID="ResourceRepeater" runat="server" DataSource='<%# Eval("AllocatedResources") %>'>
                                                <ItemTemplate>
                                                    <div class="list-group-item">
                                                        <%# Eval("ResourceName") %> - Quantity: <%# Eval("Quantity") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            
                                            <asp:Panel ID="NoResourcesPanel" runat="server" Visible='<%# ((ICollection)Eval("AllocatedResources")).Count == 0 %>'>
                                                <div class="list-group-item empty-message">No resources allocated</div>
                                            </asp:Panel>
                                        </div>
                                        <asp:Panel ID="ResourceButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyResources() %>'>
                                            <asp:Button ID="btnModifyResources" runat="server" Text="Modify Resources" 
                                                        CssClass="btn btn-outline-primary action-button" OnClick="btnModifyResources_Click" 
                                                        CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </div>

                                    <!-- Technicians Section -->
                                    <div class="mb-4">
                                        <h5 class="card-subtitle mb-3">Assigned Technicians</h5>
                                        <div class="list-group mb-3">
                                            <asp:Repeater ID="TechnicianRepeater" runat="server" DataSource='<%# Eval("AssignedTechnicians") %>'>
                                                <ItemTemplate>
                                                    <div class="list-group-item">
                                                        <%# Eval("Username") %> <%# (bool)Eval("IsSenior") ? "<span class=\"badge bg-secondary\">Senior</span>" : "" %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            
                                            <asp:Panel ID="NoTechniciansPanel" runat="server" Visible='<%# ((ICollection)Eval("AssignedTechnicians")).Count == 0 %>'>
                                                <div class="list-group-item empty-message">No technicians assigned</div>
                                            </asp:Panel>
                                        </div>
                                        <asp:Panel ID="TechnicianButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyTechnicians() %>'>
                                            <asp:Button ID="btnModifyTechnicians" runat="server" Text="Modify Technicians" 
                                                        CssClass="btn btn-outline-primary action-button" OnClick="btnModifyTechnicians_Click" 
                                                        CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </div>

                                    <!-- Budget Modification Button -->
                                    <asp:Panel ID="BudgetButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyBudget() %>'>
                                        <asp:Button ID="btnModifyBudget" runat="server" Text="Modify Budget" 
                                                    CssClass="btn btn-outline-success action-button" OnClick="btnModifyBudget_Click" 
                                                    CommandArgument='<%# Eval("ProjectId") %>' />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        
        <!-- Bootstrap JS Bundle with Popper -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>