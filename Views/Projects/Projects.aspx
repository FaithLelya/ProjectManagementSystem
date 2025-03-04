<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Projects Dashboard</h1>
            <asp:Repeater ID="ProjectRepeater" runat="server">
                <ItemTemplate>
                    <div class="project-card">
                        <h4><%# Eval("ProjectName") %></h4>
                        <div class="project-info">
                            <p><strong>Project ID:</strong> <%# Eval("ProjectId") %></p>
                            <p><strong>Description:</strong> <%# Eval("Description") %></p>
                            <p><strong>Status:</strong> <span class="status-badge <%# Eval("Status").ToString().ToLower().Replace(" ", "-") %>">
                                <%# Eval("Status") %></span></p>
                            <p><strong>Location:</strong> <%# Eval("Location") %></p>
                            <p><strong>Start Date:</strong> <%# Eval("StartDate", "{0:yyyy-MM-dd}") %></p>
                            <p><strong>End Date:</strong> <%# Eval("EndDate", "{0:yyyy-MM-dd}") %></p>

                        </div>

                        <asp:Panel ID="FinancialPanel" runat="server" CssClass="financial-details" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanViewFinancials() %>'>
                            <h3>Financial Details</h3>
                            <p><strong>Budget:</strong> KES $<%# Eval("Budget", "{0:N2}") %></p>
                            <p><strong>Resource Cost:</strong> KES $<%# Eval("TotalResourceCost", "{0:N2}") %></p>
                            <p><strong>Materials Cost:</strong> KES <%# Eval("MaterialsCost", "{0:N2S}") %></p>
                            <p><strong>Technician Payments:</strong> KES $<%# Eval("TechnicianPayment", "{0:N2}") %></p>
                            <p><strong>Total Expense:</strong> KES $<%# Eval("TotalExpense", "{0:N2}") %></p>
                        </asp:Panel>

                        <div class="resources-section">
                            <h3>Allocated Resources</h3>
                            <asp:Repeater ID="ResourceRepeater" runat="server" DataSource='<%# Eval("AllocatedResources") %>'>
                                <ItemTemplate>
                                    <div class="resource-item">
                                        <p><%# Eval("Name") %> - Quantity: <%# Eval("QuantityAllocated") %></p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Panel ID="ResourceButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyResources() %>'>
                                <asp:Button ID="btnModifyResources" runat="server" Text="Modify Resources" 
                                            CssClass="action-button" OnClick="btnModifyResources_Click" 
                                            CommandArgument='<%# Eval("ProjectId") %>' />
                            </asp:Panel>
                        </div>

                        <div class="technicians-section">
                            <h3>Assigned Technicians</h3>
                            <asp:Repeater ID="TechnicianRepeater" runat="server" DataSource='<%# Eval("AssignedTechnicians") %>'>
                                <ItemTemplate>
                                    <div class="technician-item">
                                        <p><%# Eval("Name") %></p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            
                            <asp:Panel ID="TechnicianButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem .Views.Projects.Projects)Page).CanModifyTechnicians() %>'>
                                <asp:Button ID="btnModifyTechnicians" runat="server" Text="Modify Technicians" 
                                            CssClass="action-button" OnClick="btnModifyTechnicians_Click" 
                                            CommandArgument='<%# Eval("ProjectId") %>' />
                            </asp:Panel>
                        </div>

                        <asp:Panel ID="BudgetButtonPanel" runat="server" Visible='<%# ((ProjectManagementSystem.Views.Projects.Projects)Page).CanModifyBudget() %>'>
                            <div class="budget-section">
                                <asp:Button ID="btnModifyBudget" runat="server" Text="Modify Budget" 
                                            CssClass="action-button" OnClick="btnModifyBudget_Click" 
                                            CommandArgument='<%# Eval("ProjectId") %>' />
                            </div>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html> 