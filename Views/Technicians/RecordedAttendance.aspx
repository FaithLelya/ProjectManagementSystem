<%@ Page Title="Recorded Attendance" Language="C#" AutoEventWireup="true" CodeBehind="RecordedAttendance.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.RecordedAttendance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recorded Attendance</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
        }
        .back-button {
            margin-bottom: 15px;
        }
        /* Improve date field display */
        .table .date-column {
            min-width: 120px;
            white-space: nowrap;
        }
        /* Improve table readability */
        .table th {
            background-color: #e9f0f8;
            color: #2c3e50;
            font-weight: 600;
        }
        .table-hover tbody tr:hover {
            background-color: #f1f8ff;
        }
        /* Badge styling */
        .status-badge {
            font-size: 0.85rem;
            padding: 0.35em 0.65em;
        }
        /* Card styling */
        .card {
            border-radius: 0.5rem;
            border: none;
            box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
        }
        .card-header {
            border-top-left-radius: 0.5rem !important;
            border-top-right-radius: 0.5rem !important;
            font-weight: 600;
        }
        /* Button styling */
        .btn-primary {
            background-color: #3268c1;
            border-color: #3268c1;
        }
        .btn-primary:hover {
            background-color: #2a58a5;
            border-color: #2a58a5;
        }
        /* Form control styling */
        .form-control:focus, .form-select:focus {
            border-color: #80bdff;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-4">
            <!-- Back button -->
            <div class="row">
                <div class="col">
                    <a href="../Shared/Dashboard/Welcome.aspx" class="btn btn-outline-secondary back-button">
                        <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
                    </a>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col">
                    <h2 class="text-primary"><i class="fas fa-clipboard-list me-2"></i>Attendance Records</h2>
                    <p class="lead">View and filter technician attendance records</p>
                </div>
            </div>

            <!-- Filter Section -->
            <div class="card mb-4 shadow-sm">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-filter me-2"></i>Filter Options</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <!-- First row of filters -->
                        <div class="col-md-4">
                            <label for="ddlTechnicianFilter" class="form-label">Technician</label>
                            <asp:DropDownList ID="ddlTechnicianFilter" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ddlProjectFilter" class="form-label">Project</label>
                            <asp:DropDownList ID="ddlProjectFilter" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="ddlAttendanceStatus" class="form-label">Status</label>
                            <asp:DropDownList ID="ddlAttendanceStatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Present" Value="Present"></asp:ListItem>
                                <asp:ListItem Text="Absent" Value="Absent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <!-- Second row of filters -->
                        <div class="col-md-4">
                            <label for="txtStartDate" class="form-label">Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="txtEndDate" class="form-label">End Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <asp:Button ID="btnFilter" runat="server" Text="Apply Filters" CssClass="btn btn-primary me-2" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnExport" runat="server" Text="Export to CSV" CssClass="btn btn-secondary" OnClick="btnExport_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Messages -->
            <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="alert alert-info d-block mb-4"></asp:Label>

            <!-- Results Grid -->
            <div class="card shadow-sm">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-table me-2"></i>Attendance Records</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-striped table-hover" OnRowDataBound="gvAttendance_RowDataBound"
                            EmptyDataText="No attendance records found." GridLines="None" 
                            OnRowCommand="gvAttendance_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="AttendanceId" HeaderText="ID" HeaderStyle-CssClass="d-none" ItemStyle-CssClass="d-none" />
                                <asp:BoundField DataField="Technician" HeaderText="Technician" />
                                <asp:BoundField DataField="ProjectName" HeaderText="Project" />
                                <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-CssClass="date-column" HeaderStyle-CssClass="date-column" />                                <asp:BoundField DataField="HoursWorked" HeaderText="Hours Worked" />
                                <asp:BoundField DataField="OvertimeHours" HeaderText="Overtime Hours" />
                                <asp:BoundField DataField="TotalPayment" HeaderText="Total Payment" DataFormatString="{0:C2}" />

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <!-- Status badge will be added in code-behind -->
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SubmittedBy" HeaderText="Submitted By" />
                                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="d-flex">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-primary me-1" 
                                                CommandName="EditItem" CommandArgument='<%# Eval("AttendanceId") %>'
                                                ToolTip="Edit">
                                                <i class="fas fa-edit"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger"
                                                CommandName="DeleteItem" CommandArgument='<%# Eval("AttendanceId") %>'
                                                ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this attendance record?');">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="table-primary" />
                            <PagerStyle CssClass="pagination justify-content-center" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            
            <!-- Edit Modal -->
            <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title" id="editModalLabel">Edit Attendance Record</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="hdnAttendanceId" runat="server" />
                            
                            <div class="mb-3">
                                <label for="txtEditDate" class="form-label">Date</label>
                                <asp:TextBox ID="txtEditDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            
                            <div class="mb-3">
                                <label for="ddlEditTechnician" class="form-label">Technician</label>
                                <asp:DropDownList ID="ddlEditTechnician" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            
                            <div class="mb-3">
                                <label for="ddlEditProject" class="form-label">Project</label>
                                <asp:DropDownList ID="ddlEditProject" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            
                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkAbsent" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="chkAbsent">Absent</label>
                                </div>
                            </div>
                            
                            <div id="hoursSection" runat="server">
                                <div class="mb-3">
                                    <label for="txtEditHoursWorked" class="form-label">Hours Worked</label>
                                    <asp:TextBox ID="txtEditHoursWorked" runat="server" CssClass="form-control" TextMode="Number" Step="0.5" Min="0"></asp:TextBox>
                                </div>
                                
                                <div class="mb-3">
                                    <label for="txtEditOvertimeHours" class="form-label">Overtime Hours</label>
                                    <asp:TextBox ID="txtEditOvertimeHours" runat="server" CssClass="form-control" TextMode="Number" Step="0.5" Min="0"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label for="txtEditTotalPayment" class="form-label">Total Payment</label>
                                    <div class="input-group">
                                        <span class="input-group-text">$</span>
                                        <asp:TextBox ID="txtEditTotalPayment" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" Min="0"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="mb-3">
                                <label for="txtEditNotes" class="form-label">Notes</label>
                                <asp:TextBox ID="txtEditNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSaveChanges_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    
    <script type="text/javascript">
        // Function to show the edit modal
        function showEditModal() {
            var editModal = new bootstrap.Modal(document.getElementById('editModal'));
            editModal.show();
        }

        // Function to toggle hours fields based on absence status
        function toggleHoursFields() {
            const absentCheckbox = document.getElementById('<%= chkAbsent.ClientID %>');
            const hoursSection = document.getElementById('<%= hoursSection.ClientID %>');
            
            if (absentCheckbox.checked) {
                hoursSection.style.display = 'none';
            } else {
                hoursSection.style.display = 'block';
            }
        }
        
        // Add event listener when document is ready
        document.addEventListener('DOMContentLoaded', function() {
            const absentCheckbox = document.getElementById('<%= chkAbsent.ClientID %>');
            if (absentCheckbox) {
                absentCheckbox.addEventListener('change', toggleHoursFields);
                // Initial toggle
                toggleHoursFields();
            }
        });
    </script>
</body>
</html>