<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resources.aspx.cs" Inherits="ProjectManagementSystem.Views.Resources.Resources" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resources Management</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
            <h1>Resources Management</h1>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h3>All Resources</h3>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="ResourcesRepeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Resource Name</th>
                                        <th>Description</th>
                                        <th>Available Quantity</th>
                                        <th>Cost Per Unit</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ResourceId") %></td>
                                <td><%# Eval("ResourceName") %></td>
                                <td><%# Eval("Description") %></td>
                                <td><%# Eval("Quantity") %></td>
                                <td>$<%# Eval("CostPerUnit", "{0:F2}") %></td>
                                <td>
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" 
                                        CommandName="Edit" 
                                        CommandArgument='<%# Eval("ResourceId") %>' 
                                        CssClass="btn btn-sm btn-primary" 
                                        OnClick="btnEdit_Click" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h3 id="formTitle" runat="server">Add New Resource</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtResourceName">Resource Name:</label>
                        <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvResourceName" runat="server" 
                            ControlToValidate="txtResourceName" 
                            ErrorMessage="Resource name is required." 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtDescription">Description:</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtQuantity">Quantity:</label>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" 
                            ControlToValidate="txtQuantity" 
                            ErrorMessage="Quantity is required." 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvQuantity" runat="server" 
                            ControlToValidate="txtQuantity" 
                            Type="Integer" MinimumValue="0" MaximumValue="99999" 
                            ErrorMessage="Quantity must be a positive number." 
                            CssClass="text-danger" Display="Dynamic"></asp:RangeValidator>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtCostPerUnit">Cost Per Unit:</label>
                        <asp:TextBox ID="txtCostPerUnit" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCostPerUnit" runat="server" 
                            ControlToValidate="txtCostPerUnit" 
                            ErrorMessage="Cost per unit is required." 
                            CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revCostPerUnit" runat="server" 
                            ControlToValidate="txtCostPerUnit" 
                            ValidationExpression="^\d+(\.\d{1,2})?$" 
                            ErrorMessage="Please enter a valid price (e.g., 10.99)." 
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnSave" runat="server" Text="Save Resource" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                    
                    <asp:HiddenField ID="hfResourceId" runat="server" Value="0" />
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>