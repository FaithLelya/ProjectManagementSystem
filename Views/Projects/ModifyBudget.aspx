<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyBudget.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.ModifyBudget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Project Budget</title>
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
            padding: 20px;
        }
        .card {
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .card-header {
            background-color: #007bff;
            color: white;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="mb-0">Modify Project Budget</h3>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="AuthErrorPanel" runat="server" CssClass="alert alert-danger" Visible="false">
                                <strong>Access Denied:</strong> You don't have permission to modify project budgets.
                            </asp:Panel>

                            <asp:Panel ID="EditPanel" runat="server">
                                <div class="mb-3">
                                    <label class="form-label">Project Name:</label>
                                    <asp:Label ID="lblProjectName" runat="server" CssClass="form-control bg-light"></asp:Label>
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Current Budget:</label>
                                    <asp:Label ID="lblCurrentBudget" runat="server" CssClass="form-control bg-light"></asp:Label>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Current Total Expense:</label>
                                    <asp:Label ID="lblTotalExpense" runat="server" CssClass="form-control bg-light"></asp:Label>
                                </div>

                                <div class="mb-3">
                                    <label for="txtNewBudget" class="form-label">New Budget (KES):</label>
                                    <asp:TextBox ID="txtNewBudget" runat="server" CssClass="form-control" placeholder="Enter new budget amount"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBudget" runat="server" 
                                        ControlToValidate="txtNewBudget" 
                                        CssClass="text-danger" 
                                        ErrorMessage="Budget is required." 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revBudget" runat="server" 
                                        ControlToValidate="txtNewBudget" 
                                        ValidationExpression="^\d+(\.\d{1,2})?$" 
                                        CssClass="text-danger" 
                                        ErrorMessage="Please enter a valid amount (e.g. 1000.00)." 
                                        Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="txtJustification" class="form-label">Budget Change Justification:</label>
                                    <asp:TextBox ID="txtJustification" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Provide a reason for the budget change"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvJustification" runat="server" 
                                        ControlToValidate="txtJustification" 
                                        CssClass="text-danger" 
                                        ErrorMessage="Justification is required." 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="d-grid gap-2">
                                    <asp:Button ID="btnSave" runat="server" Text="Save Budget Changes" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="SuccessPanel" runat="server" CssClass="alert alert-success mt-3" Visible="false">
                                Budget has been successfully updated.
                                <div class="mt-3">
                                    <asp:Button ID="btnReturn" runat="server" Text="Return to Projects" CssClass="btn btn-success" OnClick="btnReturn_Click" />
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>