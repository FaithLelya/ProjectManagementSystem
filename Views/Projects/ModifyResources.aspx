<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyResources.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyResources" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Resources</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container">
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
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ResourceName") %></td>
                                <td><%# Eval("QuantityUsed") %></td>
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
                        <label for="txtResourceName">Resource Name:</label>
                        <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-control"></asp:TextBox>
                        <small class="form-text text-muted">Enter a new resource or use an existing one.</small>
                    </div>
                    <div class="form-group">
                        <label for="txtQuantity">Quantity:</label>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
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