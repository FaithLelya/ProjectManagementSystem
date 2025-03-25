<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.CreateProject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Project</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
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
            color: var(--dark-text);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        .form-container {
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.1);
            padding: 2rem;
            margin-top: 2rem;
            margin-bottom: 2rem;
        }
        
        .card {
            border: none;
            border-radius: 10px;
            box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
            margin-bottom: 1.5rem;
        }
        
        .card-header {
            background-color: var(--primary-color);
            color: white;
            border-radius: 10px 10px 0 0 !important;
            padding: 1rem 1.5rem;
            font-weight: 600;
        }
        
        .btn-primary {
            background-color: var(--primary-color);
            border-color: var(--primary-color);
            padding: 0.5rem 1.5rem;
            font-weight: 500;
        }
        
        .btn-primary:hover {
            background-color: var(--secondary-color);
            border-color: var(--secondary-color);
        }
        
        .btn-outline-secondary {
            border-color: var(--secondary-color);
            color: var(--secondary-color);
        }
        
        .btn-outline-secondary:hover {
            background-color: var(--secondary-color);
            color: white;
        }
        
        .btn-success {
            background-color: #4cc9f0;
            border-color: #4cc9f0;
        }
        
        .btn-success:hover {
            background-color: #3a86ff;
            border-color: #3a86ff;
        }
        
        .form-control, .form-select {
            border-radius: 0.375rem;
            padding: 0.5rem 0.75rem;
            border: 1px solid #ced4da;
        }
        
        .form-control:focus, .form-select:focus {
            border-color: var(--accent-color);
            box-shadow: 0 0 0 0.25rem rgba(67, 97, 238, 0.25);
        }
        
        .calendar-container {
            background-color: white;
            border-radius: 0.5rem;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            padding: 1rem;
            margin-top: 0.5rem;
            display: none;
            position: absolute;
            z-index: 1000;
            width: auto;
        }
        
        .date-input-group {
            position: relative;
        }
        
        .input-group-text {
            cursor: pointer;
            background-color: var(--light-bg);
            transition: all 0.2s;
        }
        
        .input-group-text:hover {
            background-color: #e9ecef;
        }
        
        .aspNetCalendar {
            width: 100%;
            border: none;
        }
        
        .aspNetCalendar td, .aspNetCalendar th {
            padding: 0.5rem;
            text-align: center;
        }
        
        .aspNetCalendar .today {
            background-color: var(--accent-color);
            color: white;
            border-radius: 50%;
        }
        
        .aspNetCalendar a {
            text-decoration: none;
            color: var(--dark-text);
            display: block;
            border-radius: 50%;
            width: 2rem;
            height: 2rem;
            line-height: 2rem;
            margin: 0 auto;
        }
        
        .aspNetCalendar a:hover {
            background-color: var(--light-bg);
        }
        
        .aspNetCalendar .selected a {
            background-color: var(--primary-color);
            color: white;
        }
        
        .disabled-day a {
            color: #adb5bd !important;
            cursor: not-allowed;
        }
        
        .task-item {
            background-color: var(--light-bg);
            border-radius: 0.5rem;
            padding: 1rem;
            margin-bottom: 1rem;
        }
        
        h2 {
            color: var(--primary-color);
            font-weight: 600;
            margin-bottom: 1.5rem;
        }
        
        label {
            font-weight: 500;
            margin-bottom: 0.5rem;
        }
        
        .text-danger {
            font-size: 0.875rem;
        }
        
        .text-success {
            font-weight: 500;
        }
        
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
            padding-bottom: 15px;
            border-bottom: 1px solid #dee2e6;
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
                    <i class="fas fa-project-diagram me-2"></i>Create New Project
                </h2>
                <div></div> <!-- Empty div for alignment -->
            </div>
            
            <div class="row justify-content-center">
                <div class="col-lg-10 col-xl-8 form-container">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="Project Name" required></asp:TextBox>
                                <label for="txtProjectName">Project Name</label>
                                <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" ControlToValidate="txtProjectName" 
                                    ErrorMessage="Project Name is required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-select" required>
                                    <asp:ListItem Text="Select a Project Manager" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <label for="ddlProjectManager">Assign Project Manager</label>
                                <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server" 
                                    ControlToValidate="ddlProjectManager" ErrorMessage="Please select a Project Manager." 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        
                        <div class="col-12">
                            <div class="form-floating">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                    Rows="3" placeholder="Description" Style="height: 120px" required></asp:TextBox>
                                <label for="txtDescription">Description</label>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                                    ErrorMessage="Description is required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="Location" required></asp:TextBox>
                                <label for="txtLocation">Location</label>
                                <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" 
                                    ErrorMessage="Location is required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtResources" runat="server" CssClass="form-control" placeholder="Tools and Materials" required></asp:TextBox>
                                <label for="txtResources">Tools and Materials</label>
                                <asp:RequiredFieldValidator ID="rfvResources" runat="server" ControlToValidate="txtResources" 
                                    ErrorMessage="Resources are required." CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="form-floating">
                                <div class="input-group">
                                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="false"></asp:TextBox>
                                    <span class="input-group-text" onclick="toggleCalendar('start')">
                                        <i class="far fa-calendar-alt"></i>
                                    </span>
                                </div>
                                <label for="txtStartTime">Start Time</label>
                                <div class="calendar-container" id="startCalendarContainer">
                                    <asp:Calendar ID="calStartTime" runat="server" CssClass="aspNetCalendar" 
                                        OnDayRender="calStartTime_DayRender" OnSelectionChanged="calStartTime_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:Label ID="lblStartDateError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-floating">
                                <div class="input-group">
                                    <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="false"></asp:TextBox>
                                    <span class="input-group-text" onclick="toggleCalendar('end')">
                                        <i class="far fa-calendar-alt"></i>
                                    </span>
                                </div>
                                <label for="txtEndTime">End Time</label>
                                <div class="calendar-container" id="endCalendarContainer">
                                    <asp:Calendar ID="calEndTime" runat="server" CssClass="aspNetCalendar" 
                                        OnDayRender="calEndTime_DayRender" OnSelectionChanged="calEndTime_SelectionChanged"></asp:Calendar>
                                </div>
                                <asp:Label ID="lblEndDateError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtTechnicianCost" runat="server" CssClass="form-control" placeholder="Technician Payment Cost" required></asp:TextBox>
                                <label for="txtTechnicianCost">Technician Payment Cost</label>
                                <asp:RequiredFieldValidator ID="rfvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                    ErrorMessage="Technician Payment Cost is required." CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" 
                                    ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtMaterialsCost" runat="server" CssClass="form-control" placeholder="Tools and Materials Cost" required></asp:TextBox>
                                <label for="txtMaterialsCost">Tools and Materials Cost</label>
                                <asp:RequiredFieldValidator ID="rfvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                    ErrorMessage="Tools and Materials Cost is required." CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" 
                                    ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" />
                            </div>
                        </div>
                        
                        <div class="col-12">
                            <div class="form-floating">
                                <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" placeholder="Total Budget" ReadOnly="true"></asp:TextBox>
                                <label for="txtBudget">Total Budget</label>
                            </div>
                        </div>
                    </div>

                    <!-- Task Creation Section -->
                    <div class="card mt-4">
                        <div class="card-header">
                            <i class="fas fa-tasks me-2"></i>Add Initial Project Tasks
                        </div>
                        <div class="card-body" id="taskContainer">
                            <div class="task-item">
                                <div class="row g-3">
                                    <div class="col-md-6">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" placeholder="Task Name"></asp:TextBox>
                                            <label>Task Name</label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="form-control" placeholder="Task Description"></asp:TextBox>
                                            <label>Task Description</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtTaskStartDate" runat="server" CssClass="form-control" placeholder="Start Date" TextMode="Date"></asp:TextBox>
                                            <label>Start Date</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtTaskEndDate" runat="server" CssClass="form-control" placeholder="End Date" TextMode="Date"></asp:TextBox>
                                            <label>End Date</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-floating">
                                            <asp:DropDownList ID="ddlTaskAssignedTo" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="Select Assignee" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                            <label>Assigned To</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer text-end">
                            <button type="button" class="btn btn-success" onclick="addTask()">
                                <i class="fas fa-plus me-2"></i>Add Another Task
                            </button>
                        </div>
                    </div>

                    <div class="text-center mt-4">
                        <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" 
                            CssClass="btn btn-primary btn-lg px-4" OnClick="btnCreateProject_Click" />
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

            // Set minimum date for new date inputs
            var today = new Date().toISOString().split('T')[0];
            newTask.find('input[type="date"]').attr('min', today);
        }

        function toggleCalendar(type, event) {
            event.stopPropagation(); // Prevent event bubbling

            // Get the calendar container and button
            var calendar = type === 'start' ? $('#startCalendarContainer') : $('#endCalendarContainer');
            var button = type === 'start' ? $('#btnStartCalendar') : $('#btnEndCalendar');

            // Hide all calendars first
            $('.calendar-container').hide();

            // Toggle the selected calendar
            calendar.toggle();

            // Position it below the input
            var input = type === 'start' ? $('#<%= txtStartTime.ClientID %>') : $('#<%= txtEndTime.ClientID %>');
            var offset = input.offset();
            calendar.css({
                'top': offset.top + input.outerHeight() + 5,
                'left': offset.left,
                'width': input.outerWidth()
            });
        }

        // Initialize calendar functionality
        $(function () {
            // Set up calendar toggle buttons
            $('#btnStartCalendar').click(function (e) { toggleCalendar('start', e); });
            $('#btnEndCalendar').click(function (e) { toggleCalendar('end', e); });

            // Close calendar when clicking outside
            $(document).click(function () {
                $('.calendar-container').hide();
            });

            // Prevent calendar from closing when clicking inside it
            $('.calendar-container').click(function (e) {
                e.stopPropagation();
            });

            // Initialize dates
            var today = new Date();
            var tomorrow = new Date();
            tomorrow.setDate(today.getDate() + 1);

            // Set default dates in textboxes
            $('#<%= txtStartTime.ClientID %>').val(formatDate(today));
            $('#<%= txtEndTime.ClientID %>').val(formatDate(tomorrow));
            
            // Set minimum date for task date inputs
            $('input[type="date"]').attr('min', formatDate(today));
            
            // Initialize budget calculation
            $("#<%= txtTechnicianCost.ClientID %>, #<%= txtMaterialsCost.ClientID %>").on("input", calculateBudget);
        });

        function formatDate(date) {
            return date.getFullYear() + '-' +
                String(date.getMonth() + 1).padStart(2, '0') + '-' +
                String(date.getDate()).padStart(2, '0');
        }


        $(function () {
            // Initialize budget calculation
            $("#<%= txtTechnicianCost.ClientID %>, #<%= txtMaterialsCost.ClientID %>").on("input", calculateBudget);

            // Set minimum date for task date inputs
            var today = new Date().toISOString().split('T')[0];
            $('input[type="date"]').attr('min', today);

            // Initialize default dates
            var todayDate = new Date();
            var tomorrowDate = new Date();
            tomorrowDate.setDate(todayDate.getDate() + 1);
            
            // Format dates as yyyy-MM-dd
            var todayStr = todayDate.getFullYear() + '-' + 
                          String(todayDate.getMonth() + 1).padStart(2, '0') + '-' + 
                          String(todayDate.getDate()).padStart(2, '0');
            var tomorrowStr = tomorrowDate.getFullYear() + '-' + 
                             String(tomorrowDate.getMonth() + 1).padStart(2, '0') + '-' + 
                             String(tomorrowDate.getDate()).padStart(2, '0');
            
            // Set default dates
            $('#<%= txtStartTime.ClientID %>').val(todayStr);
            $('#<%= txtEndTime.ClientID %>').val(tomorrowStr);

            // Initialize calendar selections
            if (typeof calStartTime !== 'undefined') {
                calStartTime.selectedDate = todayDate;
            }
            if (typeof calEndTime !== 'undefined') {
                calEndTime.selectedDate = tomorrowDate;
            }
        });
    </script>
</body>
</html>