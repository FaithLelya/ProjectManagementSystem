<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyResources.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyResources" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Resources</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function updateCostPerUnit() {
            var ddl = document.getElementById('<%= ddlResourceName.ClientID %>');
            var costField = document.getElementById('<%= txtCostPerUnit.ClientID %>');
            
            if (ddl.selectedIndex > 0) {
                // Get the selected option
                var selectedOption = ddl.options[ddl.selectedIndex];
                // Set the cost per unit value from the data attribute
                costField.value = selectedOption.getAttribute('data-cost');
            } else {
                costField.value = '';
            }
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
               <div>
       <a href="/Views/Shared/Dashboard/Welcome.aspx" class="btn btn-outline-secondary">
           <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
       </a>
   </div>
            <h1>Modify Resources for Project</h1>
            <asp:Label ID="lblProjectName" runat="server" CssClass="h4 mb-4"></asp:Label>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h3>Current Resources</h3>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="ResourceRepeater" runat="server">
    <HeaderTemplate>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Resource Name</th>
                    <th>Quantity Used</th>
                    <th>Cost Per Unit</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("ResourceName") %></td>
            <td><%# Eval("QuantityUsed") %></td>
            <td><%# string.Format("{0:C}", Eval("CostPerUnit")) %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
                    <asp:Panel ID="NoResourcesPanel" runat="server" Visible="false">
                        <div class="alert alert-info">
                            No resources have been assigned to this project yet.
                        </div>
                    </asp:Panel>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header">
                    <h3>Add Resource</h3>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="ddlResourceName">Resource Name:</label>
                        <asp:DropDownList ID="ddlResourceName" runat="server" CssClass="form-control" onchange="updateCostPerUnit()">
                        </asp:DropDownList>
                        <small class="form-text text-muted">Select an existing resource from the list.</small>
                    </div>
                    <div class="form-group">
                        <label for="txtQuantity">Quantity:</label>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtCostPerUnit">Cost Per Unit:</label>
                        <asp:TextBox ID="txtCostPerUnit" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <small class="form-text text-muted">This value is automatically populated based on the selected resource.</small>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnAddResource" runat="server" Text="Add Resource" 
                            CssClass="btn btn-primary" OnClick="btnAddResource_Click" />
                        <asp:Button ID="btnReturn" runat="server" Text="Return to Projects" 
                            CssClass="btn btn-secondary" PostBackUrl="~/Views/Projects/Projects.aspx" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="mt-3"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>