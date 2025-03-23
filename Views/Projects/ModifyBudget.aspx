<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyBudget.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Modifybudget" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Project Budget</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <style>
        .validation-error {
            color: #dc3545;
            font-size: 80%;
            margin-top: .25rem;
            display: block;
        }
        .message-success {
            color: #28a745;
            font-weight: bold;
        }
        .message-error {
            color: #dc3545;
            font-weight: bold;
        }
        .card {
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .project-info {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-4">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h2 class="mb-0">Modify Project Budget</h2>
                </div>
                <div class="card-body">
                    <div class="project-info mb-4">
                        <h3 class="card-title"><asp:Literal ID="litProjectName" runat="server" /></h3>
                        <div class="row">
                            <div class="col-md-6">
                                <p class="mb-1"><strong>Current Budget:</strong></p>
                                <h4 class="text-success">$<asp:Literal ID="litCurrentBudget" runat="server" /></h4>
                            </div>
                        </div>
                    </div>
                    
                    <div class="budget-modification">
                        <div class="form-group">
                            <label for="txtNewBudget"><strong>New Budget:</strong></label>
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">$</span>
                                </div>
                                <asp:TextBox ID="txtNewBudget" runat="server" CssClass="form-control" placeholder="Enter new budget amount" />
                            </div>
                            <asp:RequiredFieldValidator ID="rfvBudget" runat="server" 
                                                      ControlToValidate="txtNewBudget"
                                                      ErrorMessage="Budget amount is required"
                                                      CssClass="validation-error" 
                                                      Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revBudget" runat="server"
                                                          ControlToValidate="txtNewBudget"
                                                          ErrorMessage="Please enter a valid monetary value (e.g., 1000.00)"
                                                          CssClass="validation-error"
                                                          ValidationExpression="^\d+(\.\d{1,2})?$" 
                                                          Display="Dynamic" />
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnUpdateBudget" runat="server" Text="Update Budget" OnClick="btnUpdateBudget_Click" 
                                       CssClass="btn btn-primary" />
                            <a href="ProjectDetails.aspx?id=<%=Request.QueryString["id"] %>" class="btn btn-outline-secondary ml-2">Cancel</a>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="message-error mt-3" />
                    </div>
                </div>
                <div class="card-footer text-muted">
                    <small>Last updated: <%= DateTime.Now.ToString("MMM dd, yyyy") %></small>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS and dependencies -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        // Add some client-side validation
        $(document).ready(function() {
            $("#<%= txtNewBudget.ClientID %>").on("input", function () {
                const value = $(this).val();
                if (value !== "" && !/^\d*\.?\d{0,2}$/.test(value)) {
                    $(this).addClass("is-invalid");
                } else {
                    $(this).removeClass("is-invalid");
                }
            });
        });
    </script>
</body>
</html>