<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ProjectManagementSystem.Views.Shared.Dashboard.Edit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Project</title>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
            <h2>Edit Project</h2>
            <div class="form-group">
                <label for="txtProjectName">Project Name:</label>
                <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="txtDescription">Description:</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="txtBudget">Budget:</label>
                <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="txtLocation">Location:</label>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="txtStartDate">Start Date:</label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="form-group">
                <label for="txtEndDate">End Date:</label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <asp:Button ID="btnUpdateProject" runat="server" Text="Update Project" CssClass="btn btn-primary" OnClick="btnUpdateProject_Click" />
        </div>
    </form>
</body>
</html>