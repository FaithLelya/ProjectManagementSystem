<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
        }

        .status-badge {
            font-size: 0.9rem;
            font-weight: bold;
            padding: 5px 10px;
            border-radius: 15px;
        }

        .completed { background-color: #28a745; color: white; }
        .in-progress { background-color: #007bff; color: white; }
        .on-hold { background-color: #ffc107; color: black; }
        .delayed { background-color: #dc3545; color: white; }
        .not-started { background-color: #6c757d; color: white; }

        .card {
            border-radius: 12px;
            overflow: hidden;
            transition: transform 0.2s ease-in-out;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .card:hover {
            transform: translateY(-5px);
        }

        .card-header {
            background-color: #007bff;
            color: white;
            font-size: 1.2rem;
            font-weight: bold;
            text-align: center;
        }

        .list-group-item {
            font-size: 0.95rem;
        }

        .action-button {
            width: 100%;
            margin-top: 10px;
        }

        .empty-message {
            color: #6c757d;
            font-style: italic;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-4">
            <h1 class="mb-4 text-center text-primary">Projects Dashboard</h1>
            <div class="row row-cols-1 row-cols-md-2 g-4">
                <asp:Repeater ID="ProjectRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100">
                                <div class="card-header">
                                    <%# Eval("ProjectName") %>
                                </div>
                                <div class="card-body">
                                    <!-- Project Info -->
                                    <ul class="list-group list-group-flush">
                                        <li class="list-group-item"><strong>Project ID:</strong> <%# Eval("ProjectId") %></li>
                                        <li class="list-group-item"><strong>Description:</strong> <%# Eval("Description") %></li>
                                        <li class="list-group-item"><strong>Status:</strong> 
                                            <span class="status-badge <%# Eval("Status").ToString().ToLower().Replace(" ", "-") %>">
                                                <%# Eval("Status") %>
                                            </span>
                                        </li>
                                        <li class="list-group-item"><strong>Budget:</strong> KES <%# Eval("Budget", "{0:N2}") %></li>
                                        <li class="list-group-item"><strong>Location:</strong> <%# Eval("Location") %></li>
                                        <li class="list-group-item"><strong>Start Date:</strong> <%# Eval("StartDate", "{0:yyyy-MM-dd}") %></li>
                                        <li class="list-group-item"><strong>End Date:</strong> <%# Eval("EndDate", "{0:yyyy-MM-dd}") %></li>
                                    </ul>

                                    <!-- Financial Details -->
                                    <asp:Panel ID="FinancialPanel" runat="server" CssClass="mt-3" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanViewFinancials() %>'>
                                        <h5 class="text-primary">Financial Details</h5>
                                        <ul class="list-group list-group-flush">
                                            <li class="list-group-item"><strong>Resource Cost:</strong> KES <%# Eval("TotalResourceCost", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Materials Cost:</strong> KES <%# Eval("MaterialsCost", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Technician Payments:</strong> KES <%# Eval("TechnicianPayment", "{0:N2}") %></li>
                                            <li class="list-group-item"><strong>Total Expense:</strong> KES <%# Eval("TotalExpense", "{0:N2}") %></li>
                                        </ul>
                                        
                                        <!-- Modify Budget Button (Only for Admins) -->
                                        <asp:Panel ID="BudgetButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyBudget() %>'>
                                            <asp:Button ID="btnModifyBudget" runat="server" Text="Modify Budget" 
                                                      CssClass="btn btn-outline-primary action-button" OnClick="btnModifyBudget_Click" 
                                                      CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </asp:Panel>

                                    <!-- Allocated Resources Section -->
                                    <div class="mt-3">
                                        <h5 class="text-primary">Allocated Resources</h5>
                                        <div class="list-group">
                                            <asp:Repeater ID="ResourceRepeater" runat="server" DataSource='<%# Eval("AllocatedResources") %>'>
                                                <ItemTemplate>
                                                    <div class="list-group-item">
                                                        <%# Eval("ResourceName") %> - Quantity: <%# Eval("Quantity") %> - Unit Cost: KES <%# Eval("CostPerUnit", "{0:N2}") %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:Panel ID="NoResourcesPanel" runat="server" Visible='<%# ((System.Collections.ICollection)Eval("AllocatedResources")).Count == 0 %>'>
                                                <div class="list-group-item empty-message">No resources allocated</div>
                                            </asp:Panel>
                                        </div>
                                        <!-- Add Resources Button (Only for Project Managers) -->
                                        <asp:Panel ID="ResourceButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyResources() %>'>
                                            <asp:Button ID="btnModifyResources" runat="server" Text="Modify Resources" 
                                                        CssClass="btn btn-outline-primary action-button" OnClick="btnModifyResources_Click" 
                                                        CommandArgument='<%# Eval("ProjectId") %>' />
                                        </asp:Panel>
                                    </div>

                                    <!-- Assigned Technicians Section -->
                                    <div class="mt-3">
                                        <h5 class="text-primary">Assigned Technicians</h5>
                                        <div class="list-group">
                                            <asp:Repeater ID="TechnicianRepeater" runat="server" DataSource='<%# Eval("AssignedTechnicians") %>'>
                                                <ItemTemplate>
                                                    <div class="list-group-item">
                                                        <%# Eval("Username") %> 
                                                        <%# (bool)Eval("IsSenior") ? "<span class=\"badge bg-secondary\">Senior</span>" : "" %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:Panel ID="NoTechniciansPanel" runat="server" Visible='<%# ((System.Collections.ICollection)Eval("AssignedTechnicians")).Count == 0 %>'>
                                                <div class="list-group-item empty-message">No technicians assigned</div>
                                            </asp:Panel>
                                        </div>
                                        <!-- Modify Technicians Button (Only for Project Managers) -->
                                        <asp:Panel ID="TechnicianButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyTechnicians() %>'>
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
    </form>
</body>
</html>