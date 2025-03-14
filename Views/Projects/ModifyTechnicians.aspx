<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyTechnicians.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyTechnicians" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Technicians</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css"/>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
            <h1>Modify Project Technicians</h1>
            <asp:Label ID="lblProjectName" runat="server" CssClass="h4"></asp:Label>
            <div class="project-info">
                <h2><asp:Literal ID="litProjectName" runat="server" /></h2>
            </div>

            <div class="technician-modification">
                <h3>Assign Technicians</h3>
                <div class="form-group">
                    <asp:DropDownList ID="ddlTechnicians" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select Technician" Value=""/>
                    </asp:DropDownList>
                </div>
                <div class="button-group">
                    <asp:Button ID="btnSaveChanges" runat="server" Text="Assign Technician" OnClick="btnSaveChanges_Click" />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>