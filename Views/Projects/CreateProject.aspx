<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.CreateProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Project</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome for icons -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        .form-group {
            margin-bottom: 1rem;
        }
        .tasks-container {
            max-height: 250px;
            overflow-y: auto;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            padding: 10px;
            margin-bottom: 15px;
        }
        .task-item {
            padding: 10px;
            border-bottom: 1px solid #eee;
        }
        .task-item:last-child {
            border-bottom: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <div class="row">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-primary text-white">
                            <h3><i class="fas fa-project-diagram me-2"></i>Create New Project</h3>
                        </div>
                        <div class="card-body">
                            <asp:Label ID="lblOutput" runat="server" CssClass="alert alert-danger d-none"></asp:Label>
                            <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success d-none"></asp:Label>
                            
                            <div class="row">
                                <!-- Project Basic Details -->
                                <div class="col-md-6">
                                    <h4 class="mb-3">Project Information</h4>
                                    
                                    <div class="form-group">
                                        <label for="txtProjectName" class="form-label">Project Name <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="Enter project name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" ControlToValidate="txtProjectName" 
                                            ErrorMessage="Project name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="txtDescription" class="form-label">Description <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                            Rows="3" placeholder="Project description"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                                            ErrorMessage="Description is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="txtLocation" class="form-label">Location <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="Project location"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" 
                                            ErrorMessage="Location is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="ddlProjectManager" class="form-label">Project Manager <span class="text-danger">*</span></label>
                                        <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-select"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server" ControlToValidate="ddlProjectManager" 
                                            ErrorMessage="Project manager selection is required" CssClass="text-danger" Display="Dynamic" 
                                            InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                
                                <!-- Project Timeline and Budget -->
                                <div class="col-md-6">
                                    <h4 class="mb-3">Timeline & Budget</h4>
                                    
                                    <div class="form-group">
                                        <label for="txtStartTime" class="form-label">Start Date <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" placeholder="YYYY-MM-DD" ReadOnly="true"></asp:TextBox>
                                            <button type="button" class="btn btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#calStartContainer">
                                                <i class="far fa-calendar-alt"></i>
                                            </button>
                                        </div>
                                        <div class="collapse mt-2" id="calStartContainer">
                                            <asp:Calendar ID="calStartTime" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
                                                DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                                                Width="100%" OnSelectionChanged="calStartTime_SelectionChanged" ShowGridLines="True">
                                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                <NextPrevStyle VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#808080" />
                                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                <SelectorStyle BackColor="#CCCCCC" />
                                                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <WeekendDayStyle BackColor="#FFFFCC" />
                                            </asp:Calendar>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvStartTime" runat="server" ControlToValidate="txtStartTime" 
                                            ErrorMessage="Start date is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="txtEndTime" class="form-label">End Date <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" placeholder="YYYY-MM-DD" ReadOnly="true"></asp:TextBox>
                                            <button type="button" class="btn btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#calEndContainer">
                                                <i class="far fa-calendar-alt"></i>
                                            </button>
                                        </div>
                                        <div class="collapse mt-2" id="calEndContainer">
                                            <asp:Calendar ID="calEndTime" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
                                                DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
                                                Width="100%" OnSelectionChanged="calEndTime_SelectionChanged" ShowGridLines="True">
                                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                <NextPrevStyle VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#808080" />
                                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                <SelectorStyle BackColor="#CCCCCC" />
                                                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <WeekendDayStyle BackColor="#FFFFCC" />
                                            </asp:Calendar>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvEndTime" runat="server" ControlToValidate="txtEndTime" 
                                            ErrorMessage="End date is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="txtTechnicianCost" class="form-label">Technician Payment (KES) <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">KES</span>
                                            <asp:TextBox ID="txtTechnicianCost" runat="server" CssClass="form-control" placeholder="Minimum KES10,000" TextMode="Number" min="10000"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                            ErrorMessage="Technician cost is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                            MinimumValue="10000" MaximumValue="9999999999" Type="Double" 
                                            ErrorMessage="Technician payment must be at least KES10,000" CssClass="text-danger" Display="Dynamic"></asp:RangeValidator>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label for="txtMaterialsCost" class="form-label">Materials Cost (KES) <span class="text-danger">*</span></label>
                                        <div class="input-group">
                                            <span class="input-group-text">KES</span>
                                            <asp:TextBox ID="txtMaterialsCost" runat="server" CssClass="form-control" placeholder="Minimum KES10,000" TextMode="Number" min="10000"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                            ErrorMessage="Materials cost is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                            MinimumValue="10000" MaximumValue="9999999999" Type="Double" 
                                            ErrorMessage="Materials cost must be at least KES10,000" CssClass="text-danger" Display="Dynamic"></asp:RangeValidator>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Project Resources -->
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <h4 class="mb-3">Resources & Requirements</h4>
                                    <div class="form-group">
                                        <label for="txtResources" class="form-label">Project Resources</label>
                                        <asp:TextBox ID="txtResources" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                            Rows="3" placeholder="List any equipment, personnel, or resources needed"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Project Tasks (Now Optional) -->
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <h4 class="mb-3">Project Tasks <small class="text-muted">(Optional)</small></h4>
                                    
                                    <div class="tasks-container mb-3" id="tasksList">
                                        <asp:Repeater ID="rptTasks" runat="server">
                                            <ItemTemplate>
                                                <div class="task-item">
                                                    <strong><%# Eval("Name") %></strong> - 
                                                    <span class="text-muted"><%# Eval("EndDate", "{0:yyyy-MM-dd}") %></span>
                                                    <p><%# Eval("Description") %></p>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        
                                        <!-- Empty state message - displayed using server-side code -->
                                        <asp:Panel ID="pnlNoTasks" runat="server" CssClass="alert alert-info">
                                            No tasks added yet. Adding tasks is optional.
                                        </asp:Panel>
                                    </div>
                                    
                                    <div class="card mb-3">
                                        <div class="card-header bg-light">
                                            <h5 class="mb-0">Add New Task</h5>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="txtTaskName" class="form-label">Task Name</label>
                                                        <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" placeholder="Task name"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="txtTaskDueDate" class="form-label">Due Date</label>
                                                        <asp:TextBox ID="txtTaskDueDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <label for="txtTaskDescription" class="form-label">Description</label>
                                                        <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="form-control" placeholder="Task details"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="text-end mt-2">
                                                <asp:Button ID="btnAddTask" runat="server" Text="Add Task" CssClass="btn btn-secondary" 
                                                    OnClick="btnAddTask_Click" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Removed the CustomValidator that was enforcing tasks requirement -->
                                </div>
                            </div>
                            
                            <!-- Submit Buttons -->
                            <div class="row mt-4">
                                <div class="col-md-12 text-end">
                                    <a href="Projects.aspx" class="btn btn-outline-secondary me-2">
                                        <i class="fas fa-times me-1"></i> Cancel
                                    </a>
                                    <!-- Fixed Button with icon - using HTML button with server-side event handling -->
                                    <button type="submit" class="btn btn-primary" id="btnCreateProject" runat="server" onserverclick="btnCreateProject_Click">
                                        <i class="fas fa-save me-1"></i> Create Project
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Bootstrap Bundle with Popper -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        <script>
            // Show error messages with proper styling
            function showAlert(elementId) {
                var element = document.getElementById(elementId);
                if (element) {
                    element.classList.remove('d-none');
                    setTimeout(function () {
                        element.classList.add('d-none');
                    }, 5000);
                }
            }

            // Check if error message exists and show it
            window.onload = function () {
                var outputLabel = document.getElementById('<%= lblOutput.ClientID %>');
                var successLabel = document.getElementById('<%= lblSuccess.ClientID %>');

                if (outputLabel && outputLabel.innerHTML.trim() !== '') {
                    outputLabel.classList.remove('d-none');
                }

                if (successLabel && successLabel.innerHTML.trim() !== '') {
                    successLabel.classList.remove('d-none');
                }
            };
        </script>
    </form>
</body>
</html>