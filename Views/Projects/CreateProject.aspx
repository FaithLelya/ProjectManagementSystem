<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.CreateProject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Project</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Arial, sans-serif;
        }
        /* Style the ASP.NET Calendar */
        .aspNetCalendar {
            border: 1px solid #007bff;
            border-radius: 5px;
            background-color: #ffffff;
            width: 300px; /* Adjust the width */
            font-size: 14px;
            padding: 10px;
            box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
            display: none;
            position: absolute;
            z-index: 1000;
        }

        /* Calendar Table Styling */
        .aspNetCalendar table {
            width: 100%;
            border-collapse: collapse;
        }

        .aspNetCalendar td {
            text-align: center;
            padding: 5px;
            cursor: pointer;
        }

        /* Highlight Selected Date */
        .aspNetCalendar .SelectedDate {
            background-color: #007bff !important;
            color: #fff !important;
            font-weight: bold;
            border-radius: 5px;
        }

        /* Calendar Header Styling */
        .aspNetCalendar .DayHeader {
            background-color: #007bff;
            color: white;
            font-weight: bold;
        }

        /* Style Navigation Buttons */
        .aspNetCalendar .NextPrev {
            font-weight: bold;
            color: #007bff;
            cursor: pointer;
        }

        /* Hover effect */
        .aspNetCalendar td:hover {
            background-color: #dfe8ff;
            border-radius: 5px;
        }
        
        .date-container {
            position: relative;
        }
        
        .task-item {
            background-color: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 5px;
            padding: 15px;
            margin-bottom: 10px;
        }
        
        .task-date-container {
            position: relative;
            margin-bottom: 10px;
        }
        
        .pointer {
            cursor: pointer;
        }
        
        .calendar-toggle {
            position: absolute;
            right: 10px;
            top: 10px;
            color: #007bff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2>Create New Project</h2>
            
            <!-- Project Details Section -->
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h4>Project Details</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtProjectName">Project Name</label>
                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" ControlToValidate="txtProjectName" ErrorMessage="Project Name is required." CssClass="text-danger" />
                    </div>
                    <div class="form-group">
                        <label for="txtDescription">Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description is required." CssClass="text-danger" />
                    </div>
                    <div class="form-group">
                        <label for="txtLocation">Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" ErrorMessage="Location is required." CssClass="text-danger" />
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group date-container">
                                <label for="txtStartTime">Start Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control date-input" ReadOnly="true"></asp:TextBox>
                                    <div class="input-group-append">
                                        <span class="input-group-text calendar-toggle-btn" data-calendar="calStartTime">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                </div>
                                <asp:Calendar ID="calStartTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calStartTime_SelectionChanged"></asp:Calendar>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group date-container">
                                <label for="txtEndTime">End Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control date-input" ReadOnly="true"></asp:TextBox>
                                    <div class="input-group-append">
                                        <span class="input-group-text calendar-toggle-btn" data-calendar="calEndTime">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                </div>
                                <asp:Calendar ID="calEndTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calEndTime_SelectionChanged"></asp:Calendar>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtTechnicianCost">Technician Payment Cost</label>
                                <asp:TextBox ID="txtTechnicianCost" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" ErrorMessage="Technician Payment Cost is required." CssClass="text-danger" />
                                <asp:RegularExpressionValidator ID="revTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMaterialsCost">Tools and Materials Cost</label>
                                <asp:TextBox ID="txtMaterialsCost" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" ErrorMessage="Tools and Materials Cost is required." CssClass="text-danger" />
                                <asp:RegularExpressionValidator ID="revMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtBudget">Total Budget</label>
                                <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlProjectManager">Assign Project Manager</label>
                                <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control" required></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server" ControlToValidate="ddlProjectManager" ErrorMessage="Please select a Project Manager." CssClass="text-danger" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtResources">Tools and Materials</label>
                                <asp:TextBox ID="txtResources" runat="server" CssClass="form-control" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvResources" runat="server" ControlToValidate="txtResources" ErrorMessage="Resources are required." CssClass="text-danger" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Project Tasks Section -->
            <div class="card mb-4">
                <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                    <h4 class="mb-0">Project Tasks</h4>
                    <button type="button" class="btn btn-light" id="addTaskBtn">
                        <i class="fas fa-plus"></i> Add Task
                    </button>
                </div>
                <div class="card-body">
                    <div id="tasksContainer">
                        <!-- Tasks will be added here dynamically -->
                        <div class="text-center text-muted" id="noTasksMessage">
                            <p>No tasks added yet. Click "Add Task" to create tasks for this project.</p>
                        </div>
                    </div>
                    
                    <!-- Hidden fields to store task data for server processing -->
                    <asp:HiddenField ID="hdnTasksData" runat="server" />
                </div>
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-primary btn-lg" OnClick="btnCreateProject_Click" OnClientClick="return prepareTasksData();" />
                <asp:Label ID="lblOutput" runat="server" CssClass="mt-3 text-success"></asp:Label>
            </div>
        </div>
    </form>

    <!-- Task Template Modal -->
    <div class="modal fade" id="taskModal" tabindex="-1" role="dialog" aria-labelledby="taskModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="taskModalLabel">Add New Task</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="taskName">Task Name</label>
                        <input type="text" class="form-control" id="taskName" required />
                    </div>
                    <div class="form-group">
                        <label for="taskDescription">Description</label>
                        <textarea class="form-control" id="taskDescription" rows="3"></textarea>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group task-date-container">
                                <label for="taskStartDate">Start Date</label>
                                <div class="input-group">
                                    <input type="text" class="form-control" id="taskStartDate" readonly />
                                    <div class="input-group-append">
                                        <span class="input-group-text pointer" id="taskStartDateToggle">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                </div>
                                <div id="taskStartDateCalendar" class="calendar-popup" style="display: none;"></div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group task-date-container">
                                <label for="taskEndDate">End Date</label>
                                <div class="input-group">
                                    <input type="text" class="form-control" id="taskEndDate" readonly />
                                    <div class="input-group-append">
                                        <span class="input-group-text pointer" id="taskEndDateToggle">
                                            <i class="fas fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                </div>
                                <div id="taskEndDateCalendar" class="calendar-popup" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="taskStatus">Status</label>
                        <select class="form-control" id="taskStatus">
                            <option value="Not Started">Not Started</option>
                            <option value="In Progress">In Progress</option>
                            <option value="Completed">Completed</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="saveTaskBtn">Save Task</button>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        $(function () {
            // Calculate budget on input change
            $("#<%= txtTechnicianCost.ClientID %>, #<%= txtMaterialsCost.ClientID %>").on("input", function () {
                calculateBudget();
            });

            // Initialize jQuery UI Datepickers for task dates
            $("#taskStartDate, #taskEndDate").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true
            });

            // Show/hide calendar for project dates
            $(".calendar-toggle-btn").click(function () {
                var calendarId = $(this).data("calendar");
                $("#" + calendarId).toggle();
            });

            // Hide calendars when clicking elsewhere
            $(document).click(function (e) {
                if (!$(e.target).closest('.date-container').length && !$(e.target).hasClass('calendar-toggle-btn') && !$(e.target).closest('.calendar-toggle-btn').length) {
                    $(".aspNetCalendar").hide();
                }
            });

            // Task modal actions
            $("#addTaskBtn").click(function () {
                clearTaskModal();
                $("#taskModal").modal('show');
            });

            // Task start/end date toggles
            $("#taskStartDateToggle").click(function () {
                $("#taskStartDate").datepicker("show");
            });

            $("#taskEndDateToggle").click(function () {
                $("#taskEndDate").datepicker("show");
            });

            // Save task button
            $("#saveTaskBtn").click(function () {
                // Validate task form
                if (!validateTaskForm()) {
                    return;
                }

                // Get task data
                var taskData = {
                    id: generateTaskId(),
                    name: $("#taskName").val(),
                    description: $("#taskDescription").val(),
                    startDate: $("#taskStartDate").val(),
                    endDate: $("#taskEndDate").val(),
                    status: $("#taskStatus").val()
                };

                // Add task to UI
                addTaskToUI(taskData);

                // Close modal
                $("#taskModal").modal('hide');
            });

            // Initialize tasks array
            window.projectTasks = [];
        });

        function calculateBudget() {
            var technicianCost = parseFloat($("#<%= txtTechnicianCost.ClientID %>").val()) || 0;
            var materialsCost = parseFloat($("#<%= txtMaterialsCost.ClientID %>").val()) || 0;
            var totalBudget = technicianCost + materialsCost;
            $("#<%= txtBudget.ClientID %>").val(totalBudget.toFixed(2));
        }
        
        function clearTaskModal() {
            $("#taskName").val("");
            $("#taskDescription").val("");
            $("#taskStartDate").val("");
            $("#taskEndDate").val("");
            $("#taskStatus").val("Not Started");
        }
        
        function validateTaskForm() {
            if (!$("#taskName").val()) {
                alert("Task name is required");
                return false;
            }
            
            if (!$("#taskStartDate").val()) {
                alert("Task start date is required");
                return false;
            }
            
            if (!$("#taskEndDate").val()) {
                alert("Task end date is required");
                return false;
            }
            
            // Check if end date is after start date
            var startDate = new Date($("#taskStartDate").val());
            var endDate = new Date($("#taskEndDate").val());
            
            if (endDate < startDate) {
                alert("End date must be after start date");
                return false;
            }
            
            return true;
        }
        
        function generateTaskId() {
            return 'task_' + Math.random().toString(36).substr(2, 9);
        }
        
        function addTaskToUI(taskData) {
            // Hide no tasks message
            $("#noTasksMessage").hide();
            
            // Create task item HTML
            var taskHtml = `
                <div class="task-item" data-task-id="${taskData.id}">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h5 class="mb-0">${taskData.name}</h5>
                        <div>
                            <span class="badge badge-${getStatusBadgeClass(taskData.status)}">${taskData.status}</span>
                            <button type="button" class="btn btn-sm btn-outline-primary ml-2 edit-task" data-task-id="${taskData.id}">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button type="button" class="btn btn-sm btn-outline-danger ml-1 delete-task" data-task-id="${taskData.id}">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                    <p class="text-muted">${taskData.description}</p>
                    <div class="row">
                        <div class="col-md-6">
                            <small><strong>Start Date:</strong> ${taskData.startDate}</small>
                        </div>
                        <div class="col-md-6">
                            <small><strong>End Date:</strong> ${taskData.endDate}</small>
                        </div>
                    </div>
                </div>
            `;
            
            // Add to tasks container
            $("#tasksContainer").append(taskHtml);
            
            // Add to tasks array
            window.projectTasks.push(taskData);
            
            // Bind edit/delete handlers
            bindTaskEventHandlers();
        }
        
        function getStatusBadgeClass(status) {
            switch(status) {
                case "Not Started":
                    return "secondary";
                case "In Progress":
                    return "primary";
                case "Completed":
                    return "success";
                default:
                    return "secondary";
            }
        }
        
        function bindTaskEventHandlers() {
            // Edit task button
            $(".edit-task").off("click").on("click", function() {
                var taskId = $(this).data("task-id");
                editTask(taskId);
            });
            
            // Delete task button
            $(".delete-task").off("click").on("click", function() {
                var taskId = $(this).data("task-id");
                deleteTask(taskId);
            });
        }
        
        function editTask(taskId) {
            // Find task data
            var taskData = window.projectTasks.find(t => t.id === taskId);
            if (!taskData) return;
            
            // Populate modal
            $("#taskName").val(taskData.name);
            $("#taskDescription").val(taskData.description);
            $("#taskStartDate").val(taskData.startDate);
            $("#taskEndDate").val(taskData.endDate);
            $("#taskStatus").val(taskData.status);
            
            // Show modal with edit title
            $("#taskModalLabel").text("Edit Task");
            
            // Change save button to update
            $("#saveTaskBtn").off("click").on("click", function() {
                // Validate form
                if (!validateTaskForm()) {
                    return;
                }
                
                // Update task data
                taskData.name = $("#taskName").val();
                taskData.description = $("#taskDescription").val();
                taskData.startDate = $("#taskStartDate").val();
                taskData.endDate = $("#taskEndDate").val();
                taskData.status = $("#taskStatus").val();
                
                // Update UI
                updateTaskUI(taskId, taskData);
                
                // Close modal
                $("#taskModal").modal('hide');
                
                // Reset save button
                resetSaveTaskButton();
            });
            
            $("#taskModal").modal('show');
        }
        
        function deleteTask(taskId) {
            if (confirm("Are you sure you want to delete this task?")) {
                // Remove from UI
                $(`.task-item[data-task-id="${taskId}"]`).remove();
                
                // Remove from array
                window.projectTasks = window.projectTasks.filter(t => t.id !== taskId);
                
                // Show no tasks message if no tasks left
                if (window.projectTasks.length === 0) {
                    $("#noTasksMessage").show();
                }
            }
        }
        
        function updateTaskUI(taskId, taskData) {
            var taskElement = $(`.task-item[data-task-id="${taskId}"]`);
            if (!taskElement.length) return;
            
            // Update task item UI
            taskElement.find("h5").text(taskData.name);
            taskElement.find("p").text(taskData.description);
            taskElement.find(".badge").removeClass("badge-secondary badge-primary badge-success")
                                      .addClass(`badge-${getStatusBadgeClass(taskData.status)}`)
                                      .text(taskData.status);
            taskElement.find("small:first").html(`<strong>Start Date:</strong> ${taskData.startDate}`);
            taskElement.find("small:last").html(`<strong>End Date:</strong> ${taskData.endDate}`);
        }
        
        function resetSaveTaskButton() {
            $("#taskModalLabel").text("Add New Task");
            $("#saveTaskBtn").off("click").on("click", function() {
                // Validate task form
                if (!validateTaskForm()) {
                    return;
                }
                
                // Get task data
                var taskData = {
                    id: generateTaskId(),
                    name: $("#taskName").val(),
                    description: $("#taskDescription").val(),
                    startDate: $("#taskStartDate").val(),
                    endDate: $("#taskEndDate").val(),
                    status: $("#taskStatus").val()
                };
                
                // Add task to UI
                addTaskToUI(taskData);
                
                // Close modal
                $("#taskModal").modal('hide');
            });
        }
        
        function prepareTasksData() {
            // Serialize tasks array to JSON string and store in hidden field
            $("#<%= hdnTasksData.ClientID %>").val(JSON.stringify(window.projectTasks));
            return true;
        }
    </script>
</body>
</html>