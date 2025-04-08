<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordAttendance.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.RecordAttendance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record Attendance</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <style>
        :root {
            --primary-color: #4361ee;
            --secondary-color: #3f37c9;
        }
        
        body {
            background-color: white;
        }
        
        .attendance-card {
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border: none;
        }
        
        .card-header {
            background-color: var(--primary-color);
            color: white;
            border-radius: 10px 10px 0 0 !important;
        }
        
        .btn-primary {
            background-color: var(--primary-color);
            border-color: var(--primary-color);
        }
        
        .btn-primary:hover {
            background-color: var(--secondary-color);
            border-color: var(--secondary-color);
        }
        
        .btn-outline-primary {
            color: white;
            border-color: white;
        }
        
        .btn-outline-primary:hover {
            background-color: white;
            color: var(--primary-color);
        }
        
        .radio-group {
            display: flex;
            gap: 20px;
            margin-top: 8px;
        }
        
        .radio-option {
            display: flex;
            align-items: center;
            gap: 8px;
        }
        
        .form-label {
            font-weight: 500;
            margin-bottom: 8px;
        }
        
        .back-btn {
            margin-bottom: 20px;
        }
        
        .overtime-info {
            font-size: 0.85rem;
            color: #6c757d;
            margin-top: 4px;
        }
        
        .validation-error {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 4px;
            display: none;
        }
        
        .total-payment {
            font-weight: bold;
            font-size: 1.1rem;
            color: var(--primary-color);
        }
    </style>
    <script type="text/javascript">
        // Regular working hours constant
        const REGULAR_HOURS = 8;
        const MAX_HOURS = 24;

        // Store technician rates
        var technicianRates = {};

        function toggleTimeInputs() {
            var isAbsent = document.getElementById('<%= rbtnAbsent.ClientID %>').checked;
            document.getElementById('<%= txtStartTime.ClientID %>').disabled = isAbsent;
            document.getElementById('<%= txtEndTime.ClientID %>').disabled = isAbsent;
            document.getElementById('<%= txtHoursWorked.ClientID %>').disabled = true; // Always disabled as calculated automatically
            document.getElementById('<%= txtOvertimeHours.ClientID %>').disabled = true; // Always disabled as calculated automatically
            document.getElementById('<%= txtTotalPayment.ClientID %>').disabled = true; // Always disabled as calculated automatically

            if (isAbsent) {
                // When marked as absent, set hours to 0
                document.getElementById('<%= txtHoursWorked.ClientID %>').value = "0";
                document.getElementById('<%= txtOvertimeHours.ClientID %>').value = "0";
                document.getElementById('<%= hiddenTotalHours.ClientID %>').value = "0";
                document.getElementById('<%= txtTotalPayment.ClientID %>').value = "0";
            } else {
                // Recalculate hours when toggling back to present
                calculateHours();
            }
        }

        function calculateHours() {
            // Hide any previous validation messages
            document.getElementById('hoursValidationError').style.display = 'none';
            // Enable submit button by default
            document.getElementById('<%= btnSubmit.ClientID %>').disabled = false;

            if (document.getElementById('<%= rbtnAbsent.ClientID %>').checked) {
                document.getElementById('<%= txtHoursWorked.ClientID %>').value = "0";
                document.getElementById('<%= txtOvertimeHours.ClientID %>').value = "0";
                document.getElementById('<%= hiddenTotalHours.ClientID %>').value = "0";
                document.getElementById('<%= txtTotalPayment.ClientID %>').value = "0";
                return;
            }

            var startTimeStr = document.getElementById('<%= txtStartTime.ClientID %>').value;
            var endTimeStr = document.getElementById('<%= txtEndTime.ClientID %>').value;

            if (!startTimeStr || !endTimeStr) {
                return; // Exit if times are not set
            }

            // Parse time strings to Date objects for calculation
            var startDate = new Date("2000-01-01T" + startTimeStr + ":00");
            var endDate = new Date("2000-01-01T" + endTimeStr + ":00");

            // If end time is earlier than start time, assume it's the next day
            if (endDate < startDate) {
                endDate.setDate(endDate.getDate() + 1);
            }

            // Calculate time difference in hours
            var diffMs = endDate - startDate;
            var diffHrs = diffMs / (1000 * 60 * 60);

            // Round to nearest half hour
            var totalHours = Math.round(diffHrs * 2) / 2;

            // Validation: Check if total hours is valid
            if (totalHours <= 0 || totalHours > MAX_HOURS) {
                document.getElementById('hoursValidationError').style.display = 'block';
                document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
                return;
            }

            // Calculate regular and overtime hours
            var regularHours = Math.min(REGULAR_HOURS, totalHours);
            var overtimeHours = Math.max(0, totalHours - REGULAR_HOURS);

            // Update the form fields
            document.getElementById('<%= txtHoursWorked.ClientID %>').value = regularHours.toFixed(1);
            document.getElementById('<%= txtOvertimeHours.ClientID %>').value = overtimeHours.toFixed(1);
            document.getElementById('<%= hiddenTotalHours.ClientID %>').value = totalHours.toFixed(1);
            
            // Calculate total payment
            calculateTotalPayment(regularHours, overtimeHours);
            
            // Debug output to console
            console.log("Start Time: " + startTimeStr);
            console.log("End Time: " + endTimeStr);
            console.log("Regular Hours: " + regularHours);
            console.log("Overtime Hours: " + overtimeHours);
            console.log("Total Hours: " + totalHours);
        }
        
        function calculateTotalPayment(regularHours, overtimeHours) {
            var technicianId = document.getElementById('<%= ddlTechnician.ClientID %>').value;
            
            if (!technicianId || !technicianRates[technicianId]) {
                document.getElementById('<%= txtTotalPayment.ClientID %>').value = "0.00";
                return;
            }
            
            var hourlyRate = technicianRates[technicianId].hourlyRate;
            var overtimeRate = technicianRates[technicianId].overtimeRate;
            
            var totalPayment = (regularHours * hourlyRate) + (overtimeHours * overtimeRate);
            document.getElementById('<%= txtTotalPayment.ClientID %>').value = totalPayment.toFixed(2);
            document.getElementById('<%= hiddenTotalPayment.ClientID %>').value = totalPayment.toFixed(2);
            
            console.log("Hourly Rate: " + hourlyRate);
            console.log("Overtime Rate: " + overtimeRate);
            console.log("Total Payment: " + totalPayment.toFixed(2));
        }
    
        function validateForm() {
            // Perform final validation before form submission
            if (document.getElementById('<%= rbtnPositive.ClientID %>').checked) {
                var regularHours = parseFloat(document.getElementById('<%= txtHoursWorked.ClientID %>').value) || 0;
                var overtimeHours = parseFloat(document.getElementById('<%= txtOvertimeHours.ClientID %>').value) || 0;
                var totalHours = regularHours + overtimeHours;
                
                // Update hidden field with total hours
                document.getElementById('<%= hiddenTotalHours.ClientID %>').value = totalHours.toFixed(1);
                
                if (isNaN(totalHours) || totalHours <= 0 || totalHours > MAX_HOURS) {
                    document.getElementById('hoursValidationError').style.display = 'block';
                    return false;
                }
            }
            return true;
        }
        
        function updateTechnicianRates() {
            var technicianId = document.getElementById('<%= ddlTechnician.ClientID %>').value;
            if (technicianId) {
                calculateHours();
            } else {
                document.getElementById('<%= txtTotalPayment.ClientID %>').value = "0.00";
            }
        }
    
        // Set default date and times on page load and restrict future dates
        window.onload = function() {
            // Set today's date
            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0');
            var yyyy = today.getFullYear();
            today = yyyy + '-' + mm + '-' + dd;
            document.getElementById('<%= txtDate.ClientID %>').value = today;
            
            // Set max date attribute to prevent future date selection
            document.getElementById('<%= txtDate.ClientID %>').setAttribute("max", today);
            
            // Set default start time (9:00 AM)
            document.getElementById('<%= txtStartTime.ClientID %>').value = "09:00";
            
            // Set default end time (5:00 PM)
            document.getElementById('<%= txtEndTime.ClientID %>').value = "17:00";
            
            // Set up event listeners for time inputs
            document.getElementById('<%= txtStartTime.ClientID %>').addEventListener('change', calculateHours);
            document.getElementById('<%= txtEndTime.ClientID %>').addEventListener('change', calculateHours);
            document.getElementById('<%= ddlTechnician.ClientID %>').addEventListener('change', updateTechnicianRates);

            // Calculate initial hours
            calculateHours();
        };
    </script>
</head>
<body>
    <form id="form1" runat="server" onsubmit="return validateForm();">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-lg-8">
                    <!-- Back Button -->
                    <asp:HyperLink ID="lnkBack" runat="server" 
                        NavigateUrl="~/Views/Shared/Dashboard/Welcome.aspx" 
                        CssClass="btn btn-outline-primary back-btn">
                        <i class="fas fa-arrow-left me-2"></i> Back to Dashboard
                    </asp:HyperLink>
                    
                    <!-- Attendance Card -->
                    <div class="card attendance-card">
                        <div class="card-header py-3">
                            <h3 class="mb-0"><i class="fas fa-calendar-check me-2"></i>Record Attendance</h3>
                        </div>
                        <div class="card-body p-4">
                            <div class="row g-3">
                                <!-- Technician Dropdown -->
                                <div class="col-md-6">
                                    <label for="ddlTechnician" class="form-label">Technician</label>
                                    <asp:DropDownList ID="ddlTechnician" runat="server" CssClass="form-select" required 
                                        OnSelectedIndexChanged="ddlTechnician_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="-- Select Technician --" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <!-- Project Dropdown -->
                                <div class="col-md-6">
                                    <label for="ddlProject" class="form-label">Project</label>
                                    <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-select" required>
                                        <asp:ListItem Text="-- Select Project --" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <!-- Date Picker -->
                                <div class="col-md-4">
                                    <label for="txtDate" class="form-label">Date</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                                    </div>
                                    <div class="overtime-info">Can only select today or past dates</div>
                                </div>
                                
                                <!-- Start Time -->
                                <div class="col-md-4">
                                    <label for="txtStartTime" class="form-label">Start Time</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="far fa-clock"></i></span>
                                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" 
                                            TextMode="Time" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <!-- End Time -->
                                <div class="col-md-4">
                                    <label for="txtEndTime" class="form-label">End Time</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="fas fa-clock"></i></span>
                                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" 
                                            TextMode="Time" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <!-- Hidden fields to store values -->
                                <asp:HiddenField ID="hiddenTotalHours" runat="server" />
                                <asp:HiddenField ID="hiddenTotalPayment" runat="server" />
                                
                                <!-- Attendance Type -->
                                <div class="col-md-4">
                                    <label class="form-label">Attendance Type</label>
                                    <div class="radio-group">
                                        <div class="radio-option">
                                            <asp:RadioButton ID="rbtnPositive" runat="server" GroupName="AttendanceType" 
                                                Text="Present" Checked="true" AutoPostBack="false" OnClick="toggleTimeInputs()" />
                                        </div>
                                        <div class="radio-option">
                                            <asp:RadioButton ID="rbtnAbsent" runat="server" GroupName="AttendanceType" 
                                                Text="Absent" AutoPostBack="false" OnClick="toggleTimeInputs()" />
                                        </div>
                                    </div>
                                </div>
                                
                                <!-- Hours Worked (Calculated Input) -->
                                <div class="col-md-4">
                                    <label for="txtHoursWorked" class="form-label">Regular Hours Worked</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="far fa-clock"></i></span>
                                        <asp:TextBox ID="txtHoursWorked" runat="server" CssClass="form-control" 
                                            TextMode="Number" min="0" max="24" step="0.5" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="overtime-info">Auto-calculated (max 8 hours)</div>
                                </div>
                                
                                <!-- Overtime Hours (Calculated Input) -->
                                <div class="col-md-4">
                                    <label for="txtOvertimeHours" class="form-label">Overtime Hours</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="fas fa-clock"></i></span>
                                        <asp:TextBox ID="txtOvertimeHours" runat="server" CssClass="form-control" 
                                            TextMode="Number" min="0" max="16" step="0.5" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="overtime-info">Auto-calculated (hours over 8)</div>
                                </div>
                                
                                <!-- Total Payment (Calculated Input) -->
                                <div class="col-12">
                                    <label for="txtTotalPayment" class="form-label">Total Payment</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i style="font-weight: bold; font-size: 1.2em;">KSh</i></span>
                                        <asp:TextBox ID="txtTotalPayment" runat="server" CssClass="form-control total-payment" 
                                            TextMode="Number" min="0" step="0.01" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="overtime-info">Auto-calculated based on hourly rates</div>
                                </div>
                                
                                <div id="hoursValidationError" class="validation-error col-12">
                                    Total hours worked must be positive and cannot exceed 24 hours.
                                </div>
                                
                                <!-- Notes -->
                                <div class="col-12">
                                    <label for="txtNotes" class="form-label">Notes/Comments</label>
                                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" 
                                        TextMode="MultiLine" Rows="3" placeholder="Any additional notes..."></asp:TextBox>
                                </div>
                                
                                <!-- Submit Button and Message -->
                                <div class="col-12 mt-4">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Attendance" 
                                        CssClass="btn btn-primary px-4 py-2" OnClick="btnSubmit_Click" />
                                    <asp:Label ID="lblMessage" runat="server" CssClass="ms-3"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>