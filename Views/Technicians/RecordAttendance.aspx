<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordAttendance.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.RecordAttendance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record Attendance</title>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
     <script type="text/javascript">
        function toggleHoursInput() {
            var isAbsent = document.getElementById('<%= rbtnAbsent.ClientID %>').checked;
            document.getElementById('<%= txtHoursWorked.ClientID %>').disabled = isAbsent;
            document.getElementById('<%= txtOvertimeHours.ClientID %>').disabled = isAbsent;
        }
     </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2>Record Technician Attendance</h2>
            <div class="form-group">
                <label for="ddlTechnician">Select Technician</label>
                <asp:DropDownList ID="ddlTechnician" runat="server" CssClass="form-control" required></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="ddlProject">Select Project</label>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" required></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="txtDate">Date</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
            </div>
             <div class="form-group">
                <label>Attendance Type</label><br />
               <asp:RadioButton ID="rbtnAbsent" runat="server" GroupName="AttendanceType" Text="Absent" AutoPostBack="false" OnClick="toggleHoursInput()" />
               <asp:RadioButton ID="rbtnPositive" runat="server" GroupName="AttendanceType" Text="Present" AutoPostBack="false" OnClick="toggleHoursInput()" /></div>
            <div class="form-group">
                <label for="txtHoursWorked">Regular Hours Worked</label>
                <asp:TextBox ID="txtHoursWorked" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtOvertimeHours">Overtime Hours Worked</label>
                <asp:TextBox ID="txtOvertimeHours" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtNotes">Notes/Comments</label>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success mt-3"></asp:Label>
        </div>
    </form>
</body>
</html>