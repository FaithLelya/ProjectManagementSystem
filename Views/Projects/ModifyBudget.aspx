<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyBudget.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Modifybudget" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Budget</title>
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Modify Project Budget</h1>
            <div class="project-info">
                <h2><asp:Literal ID="litProjectName" runat="server" /></h2>
                <p>Current Budget: $<asp:Literal ID="litCurrentBudget" runat="server" /></p>
            </div>

            <div class="budget-modification">
                <div class="form-group">
                    <label for="txtNewBudget">New Budget:</label>
                    <asp:TextBox ID="txtNewBudget" runat="server" CssClass="budget-input" />
                    <asp:RequiredFieldValidator ID="rfvBudget" runat="server" 
                                              ControlToValidate="txtNewBudget"
                                              ErrorMessage="Budget is required"
                                              CssClass="validation-error" />
                    <asp:RegularExpressionValidator ID="revBudget" runat="server"
                                                  ControlToValidate="txtNewBudget"
                                                  ErrorMessage="Invalid budget format"
                                                  CssClass="validation-error"
                                                  ValidationExpression="^\d+(\.\d{1,2})?$" />
                </div>

                <div class="form-group">
                    <asp:Button ID="btnUpdateBudget" runat="server" Text="Update Budget" OnClick="btnUpdateBudget_Click" CssClass="btn btn-primary" />
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="message" />
            </div>
        </div>
    </form>
</body>
</html>