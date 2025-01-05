<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyResources.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyResources" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Modify Resources</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
            <h1>Modify Project Resources</h1>
            <div class="project-info">
                <h2><asp:Literal ID="litProjectName" runat="server" /></h2>
            </div>

            <div class="resource-modification">
                <h3>Current Resources</h3>
                <asp:GridView ID="ResourcesGrid" runat="server" AutoGenerateColumns="False" 
                            CssClass="grid-view" OnRowCommand="ResourcesGrid_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Resource Name" />
                        <asp:BoundField DataField="QuantityAllocated" HeaderText="Allocated Quantity" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="quantity-input" />
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                                          CommandName="UpdateResource" 
                                          CommandArgument='<%# Eval("ResourceId") %>'
                                          CssClass="action-button" />
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" 
                                          CommandName="RemoveResource" 
                                          CommandArgument='<%# Eval("ResourceId") %>'
                                          CssClass="action-button remove" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <h3>Add New Resource</h3>
                <div class="add-resource-form">
                    <asp:DropDownList ID="ddlResources" runat="server" CssClass="dropdown" />
                    <asp:TextBox ID="txtNewQuantity" runat="server" CssClass="quantity-input" placeholder="Quantity" />
                    <asp:Button ID="btnAddResource" runat="server" Text="Add Resource" 
                              OnClick="btnAddResource_Click" CssClass="action-button" />
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
