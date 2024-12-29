<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ProjectManagementSystem.Views.Shared.Dashboard.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: white;
        }
        .dashboard-container {
            padding: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <h2>Dashboard</h2>

            <h3>Current Projects</h3>
            <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Budget" HeaderText="Budget" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("Id") %>' OnCommand="EditProject_Command" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <h3>Create New Project</h3>
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
            <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" CssClass="btn btn-primary" OnClick="btnCreateProject_Click" />
        </div>
    </form>
</body>
</html>
