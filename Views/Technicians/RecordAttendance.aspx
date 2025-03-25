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
            background-color: #f8f9fa;
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
            color: var(--primary-color);
            border-color: var(--primary-color);
        }
        
        .btn-outline-primary:hover {
            background-color: var(--primary-color);
            color: white;
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
    </style>
    <script type="text/javascript">
        function toggleHoursInput() {
            var isAbsent = document.getElementById('<%= rbtnAbsent.ClientID %>').checked;
            document.getElementById('<%= txtHoursWorked.ClientID %>').disabled = isAbsent;
            document.getElementById('<%= txtOvertimeHours.ClientID %>').disabled = isAbsent;
        }
        
        // Set default date to today on page load
        window.onload = function() {
            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0');
            var yyyy = today.getFullYear();
            today = yyyy + '-' + mm + '-' + dd;
            document.getElementById('<%= txtDate.ClientID %>').value = today;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                                    <asp:DropDownList ID="ddlTechnician" runat="server" CssClass="form-select" required>
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
                                <div class="col-md-6">
                                    <label for="txtDate" class="form-label">Date</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <!-- Attendance Type -->
                                <div class="col-md-6">
                                    <label class="form-label">Attendance Type</label>
                                    <div class="radio-group">
                                        <div class="radio-option">
                                            <asp:RadioButton ID="rbtnPositive" runat="server" GroupName="AttendanceType" 
                                                Text="Present" Checked="true" AutoPostBack="false" OnClick="toggleHoursInput()" />
                                        </div>
                                        <div class="radio-option">
                                            <asp:RadioButton ID="rbtnAbsent" runat="server" GroupName="AttendanceType" 
                                                Text="Absent" AutoPostBack="false" OnClick="toggleHoursInput()" />
                                        </div>
                                    </div>
                                </div>
                                
                                <!-- Hours Worked -->
                                <div class="col-md-6">
                                    <label for="txtHoursWorked" class="form-label">Regular Hours Worked</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="far fa-clock"></i></span>
                                        <asp:TextBox ID="txtHoursWorked" runat="server" CssClass="form-control" 
                                            placeholder="8" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <!-- Overtime Hours -->
                                <div class="col-md-6">
                                    <label for="txtOvertimeHours" class="form-label">Overtime Hours Worked</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="fas fa-clock"></i></span>
                                        <asp:TextBox ID="txtOvertimeHours" runat="server" CssClass="form-control" 
                                            placeholder="0"></asp:TextBox>
                                    </div>
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