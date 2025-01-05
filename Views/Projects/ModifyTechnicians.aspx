<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyTechnicians.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.TechnicianProjects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Modify Technicians</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Modify Project Technicians</h1>
            <div class="project-info">
                <h2><asp:Literal ID="litProjectName" runat="server" /></h2>
            </div>

            <div class="technician-modification">
                <h3>Assigned Technicians</h3>
                <asp:GridView ID="TechniciansGrid" runat="server" AutoGenerateColumns="False" 
                            CssClass="grid-view" OnRowCommand="TechniciansGrid_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Technician Name" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" 
                                          CommandName="RemoveTechnician" 
                                          CommandArgument='<%# Eval("UserId") %>'
                                          CssClass="action-button remove" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <h3>Add Technician</h3>
                <div class="add-technician-form">
                    <asp:DropDownList ID="ddlTechnicians" runat="server" CssClass="dropdown" />
                    <asp:Button ID="btnAddTechnician" runat="server" Text="Add Technician" 
                              OnClick="btnAddTechnician_Click" CssClass="action-button" />
                </div>
            </div>

            <div class="button-group">
                <asp:Button ID="btnSave" runat="server" Text="Save Changes" 
                          OnClick="btnSave_Click" CssClass="action-button save" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                          OnClick="btnCancel_Click" CssClass="action-button cancel" />
            </div>
        </div>
    </form>
</body>
</html>
