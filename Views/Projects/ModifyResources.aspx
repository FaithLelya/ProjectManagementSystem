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
            <asp:Label ID="lblProjectName" runat="server" CssClass="h4"></asp:Label>
            <h3>Current Resources</h3>
            <asp:Repeater ID="ResourceRepeater" runat="server">
                <ItemTemplate>
                    <div class="resource-item">
                        <p><%# Eval("ResourceName") %> - Quantity Used: <%# Eval("QuantityUsed") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

               <h3>Add Resources</h3>
            <div class="form-group">
                <label for="txtResourceName">Resource Name:</label>
                <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtQuantity">Quantity:</label>
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="button-group">
                <asp:Button ID="btnAddResource" runat="server" Text="Add Resource" 
                            OnClick="btnAddResource_Click" CssClass="btn btn-primary" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
        </div>
    </form>
</body>
</html>