<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.CreateProject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Project</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <style>
        body {
            background-color: #f4f6f9;
        }
        .form-container {
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            padding: 30px;
            margin-top: 30px;
        }
        .calendar-container {
            background-color: #f8f9fa;
            border-radius: 5px;
            padding: 15px;
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-8 form-container">
                    <h2 class="text-center mb-4">Create New Project</h2>
                    
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtProjectName">Project Name</label>
                                <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" ControlToValidate="txtProjectName" 
                                    ErrorMessage="Project Name is required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlProjectManager">Assign Project Manager</label>
                                <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control" required></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server" 
                                    ControlToValidate="ddlProjectManager" ErrorMessage="Please select a Project Manager." 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtDescription">Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                            ErrorMessage="Description is required." CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtLocation">Location</label>
                                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" 
                                    ErrorMessage="Location is required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtResources">Tools and Materials</label>
                                <asp:TextBox ID="txtResources" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvResources" runat="server" ControlToValidate="txtResources" 
                                    ErrorMessage="Resources are required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Start Time</label>
                                <div class="calendar-container">
                                    <asp:Calendar ID="calStartTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calStartTime_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>End Time</label>
                                <div class="calendar-container">
                                    <asp:Calendar ID="calEndTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calEndTime_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtTechnicianCost">Technician Payment Cost</label>
                                <asp:TextBox ID="txtTechnicianCost" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                    ErrorMessage="Technician Payment Cost is required." CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                    ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtMaterialsCost">Tools and Materials Cost</label>
                                <asp:TextBox ID="txtMaterialsCost" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                    ErrorMessage="Tools and Materials Cost is required." CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                    ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" />
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtBudget">Total Budget</label>
                        <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>

                    <!-- Task Creation Section -->
                    <div class="card mt-4">
                        <div class="card-header bg-primary text-white">
                            Add Initial Project Tasks
                        </div>
                        <div class="card-body" id="taskContainer">
                            <div class="task-item mb-3">
                                <div class="row">
                                    <div class="col-md-6">
                                        <label>Task Name</label>
                                        <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" placeholder="Enter task name"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label>Task Description</label>
                                        <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="form-control" placeholder="Enter task description"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-4">
                                        <label>Start Date</label>
                                        <asp:TextBox ID="txtTaskStartDate" runat="server" CssClass="form-control" placeholder="Select start date"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>End Date</label>
                                        <asp:TextBox ID="txtTaskEndDate" runat="server" CssClass="form-control" placeholder="Select end date"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Assigned To</label>
                                        <asp:DropDownList ID="ddlTaskAssignedTo" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer">
                            <button type="button" class="btn btn-success" onclick="addTask()">Add Another Task</button>
                        </div>
                    </div>

                    <div class="text-center mt-4">
                        <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-primary btn-lg" OnClick="btnCreateProject_Click" />
                    </div>

                    <div class="text-center mt-3">
                        <asp:Label ID="lblOutput" runat="server" CssClass="text-success"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        function calculateBudget() {
            var technicianCost = parseFloat($("#<%= txtTechnicianCost.ClientID %>").val()) || 0;
            var materialsCost = parseFloat($("#<%= txtMaterialsCost.ClientID %>").val()) || 0;
            var totalBudget = technicianCost + materialsCost;
            $("#<%= txtBudget.ClientID %>").val(totalBudget.toFixed(2));
        }

        function addTask() {
            // Clone the first task container
            var taskContainer = $('#taskContainer');
            var newTask = taskContainer.find('.task-item').first().clone();

            // Clear input values
            newTask.find('input, textarea, select').val('');

            // Append to container
            taskContainer.append(newTask);
        }

        $(function () {
            $("#<%= txtTechnicianCost.ClientID %>, #<%= txtMaterialsCost.ClientID %>").on("input", function () {
                calculateBudget();
            });
        });
    </script>
</body>
</html>