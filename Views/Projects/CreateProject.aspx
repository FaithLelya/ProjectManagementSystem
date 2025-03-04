<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.CreateProject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Project</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
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
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2>Create New Project</h2>
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
                 <div class="form-group">
                 <label for="calStartTime">Start Time</label>
                 <asp:Calendar ID="calStartTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calStartTime_SelectionChanged"></asp:Calendar>
                 <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

             <div class="form-group">
                 <label for="calEndTime">End Time</label>
                 <asp:Calendar ID="calEndTime" runat="server" CssClass="aspNetCalendar" OnSelectionChanged="calEndTime_SelectionChanged"></asp:Calendar>
                 <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
             </div> 
            
            <div class="form-group">
                <label for="txtTechnicianCost">Technician Payment Cost</label>
                <asp:TextBox ID="txtTechnicianCost" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" ErrorMessage="Technician Payment Cost is required." CssClass="text-danger" />
                <asp:RegularExpressionValidator ID="revTechnicianCost" runat="server" ControlToValidate="txtTechnicianCost" ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" />
            </div>
            <div class="form-group">
                <label for="txtMaterialsCost">Tools and Materials Cost</label>
                <asp:TextBox ID="txtMaterialsCost" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" ErrorMessage="Tools and Materials Cost is required." CssClass="text-danger" />
                <asp:RegularExpressionValidator ID="revMaterialsCost" runat="server" ControlToValidate="txtMaterialsCost" ErrorMessage="Invalid cost format." CssClass="text-danger" ValidationExpression="^\d+(\.\d{1,2})?$" />
            </div>
            <div class="form-group">
                <label for="txtBudget">Budget</label>
                <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ddlProjectManager">Assign Project Manager</label>
                <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control" required></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server" ControlToValidate="ddlProjectManager" ErrorMessage="Please select a Project Manager." CssClass="text-danger" />
            </div>
            <div class="form-group">
                <label for="txtResources">Tools and Materials</label>
                <asp:TextBox ID="txtResources" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvResources" runat="server" ControlToValidate="txtResources" ErrorMessage="Resources are required." CssClass="text-danger" />
            </div>
            <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-primary" OnClick="btnCreateProject_Click" />
              <asp:Label ID="lblOutput" runat="server" CssClass="mt-3 text-success"></asp:Label>
        </div>
    </form>

    <script>
        $(function () {
            // Calculate budget on input change
            $("#<%= txtTechnicianCost.ClientID %>, #<%= txtMaterialsCost.ClientID %>").on("input", function () {
                calculateBudget();
            });
        });

        function calculateBudget() {
            var technicianCost = parseFloat($("#<%= txtTechnicianCost.ClientID %>").val()) || 0;
            var materialsCost = parseFloat($("#<%= txtMaterialsCost.ClientID %>").val()) || 0;
            var totalBudget = technicianCost + materialsCost;
            $("#<%= txtBudget.ClientID %>").val(totalBudget.toFixed(2));
        }
    </script>
</body>
</html>