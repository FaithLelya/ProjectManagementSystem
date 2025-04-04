<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentInfo.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.PaymentInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Technician Payment Information</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
            padding-top: 2rem;
        }
        .payment-card {
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            margin-bottom: 2rem;
        }
        .card-header {
            border-radius: 10px 10px 0 0 !important;
            padding: 1rem 1.5rem;
        }
        .card-body {
            padding: 1.5rem;
        }
        .form-control, .form-select {
            padding: 0.75rem;
            border-radius: 6px;
        }
        .payment-method-card {
            border: 1px solid #dee2e6;
            border-radius: 8px;
            padding: 1rem;
            margin-bottom: 1rem;
        }
        .payment-method-card.active {
            border-color: #0d6efd;
            background-color: #f8f9ff;
        }
        .payment-icon {
            font-size: 2rem;
            margin-right: 0.5rem;
            vertical-align: middle;
        }
        .method-title {
            font-weight: 600;
            color: #345;
            margin-bottom: 0.5rem;
        }
        .success-alert {
            border-radius: 8px;
            border-left: 5px solid #28a745;
        }
        .page-title {
            color: #345;
            margin-bottom: 1.5rem;
            font-weight: 600;
        }
        .btn-save {
            background-color: #0d6efd;
            color: white;
            padding: 0.75rem 1.5rem;
            border-radius: 6px;
            font-weight: 600;
        }
        .btn-save:hover {
            background-color: #0b5ed7;
            color: white;
        }
        .payment-method-logo {
            height: 40px;
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-10">
                 <div class="row">
                 <div class="col">
                     <a href="../Shared/Dashboard/Welcome.aspx" class="btn btn-outline-secondary back-button">
                         <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
                     </a>
                 </div>
             </div>
                    <h2 class="text-center page-title">
                        <i class="fas fa-credit-card me-2"></i>My Payment Information
                    </h2>

                    
                    <!-- Personal Info Card -->
                    <div class="card payment-card">
                        <div class="card-header bg-primary text-white">
                            <h3 class="card-title mb-0">
                                <i class="fas fa-user me-2"></i>Personal Information
                            </h3>
                        </div>
                        <div class="card-body">
                            <div class="row g-4">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Technician ID:</label>
                                        <asp:Label ID="lblTechnicianId" runat="server" CssClass="form-control" Text=""></asp:Label>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Full Name:</label>
                                        <asp:Label ID="lblTechnicianName" runat="server" CssClass="form-control" Text=""></asp:Label>
                                    </div>
                                </div>
                                
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Email:</label>
                                        <asp:Label ID="lblTechnicianEmail" runat="server" CssClass="form-control" Text=""></asp:Label>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Phone Number:</label>
                                        <asp:Label ID="lblTechnicianPhone" runat="server" CssClass="form-control" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <!-- Payment Methods Card -->
                    <div class="card payment-card">
                        <div class="card-header bg-primary text-white">
                            <h3 class="card-title mb-0">
                                <i class="fas fa-wallet me-2"></i>Payment Methods
                            </h3>
                        </div>
                        <div class="card-body">
                            <!-- Success Message Panel -->
                            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success success-alert mb-4">
                                <i class="fas fa-check-circle me-2"></i>
                                <asp:Label ID="lblSuccessMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                            
                            <!-- Payment Method Selection -->
                            <div class="mb-4">
                                <label class="form-label fw-bold">Preferred Payment Method:</label>
                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                                    <asp:ListItem Text="-- Select Payment Method --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="M-Pesa" Value="MPESA"></asp:ListItem>
                                    <asp:ListItem Text="Bank Transfer" Value="BANK"></asp:ListItem>
                                    <asp:ListItem Text="Credit/Debit Card" Value="CARD"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <!-- M-Pesa Payment Details Panel -->
                            <asp:Panel ID="pnlMpesa" runat="server" CssClass="payment-method-card" Visible="false">
                                <div class="d-flex align-items-center mb-3">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/M-PESA_LOGO-01.svg/320px-M-PESA_LOGO-01.svg.png" alt="M-Pesa" class="payment-method-logo" />
                                    <h4 class="method-title mb-0">M-Pesa Details</h4>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="txtMpesaNumber" class="form-label">
                                                <i class="fas fa-phone me-1"></i>M-Pesa Phone Number:
                                            </label>
                                            <div class="input-group">
                                                <span class="input-group-text">+</span>
                                                <asp:TextBox ID="txtMpesaNumber" runat="server" CssClass="form-control" 
                                                    placeholder="Enter M-Pesa number (2547XXXXXXXX)" MaxLength="12">
                                                </asp:TextBox>
                                            </div>
                                            <asp:RegularExpressionValidator ID="regexMpesa" runat="server" 
                                                ControlToValidate="txtMpesaNumber" 
                                                ValidationExpression="^2547\d{8}$" 
                                                ErrorMessage="Please enter a valid M-Pesa number starting with 2547 followed by 8 digits." 
                                                CssClass="text-danger small" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="txtMpesaName" class="form-label">
                                                <i class="fas fa-user me-1"></i>Registered Name:
                                            </label>
                                            <asp:TextBox ID="txtMpesaName" runat="server" CssClass="form-control" 
                                                placeholder="Name registered with M-Pesa">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <!-- Bank Transfer Details Panel -->
                            <asp:Panel ID="pnlBank" runat="server" CssClass="payment-method-card" Visible="false">
                                <div class="d-flex align-items-center mb-3">
                                    <i class="fas fa-university payment-icon text-primary"></i>
                                    <h4 class="method-title mb-0">Bank Account Details</h4>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="ddlBank" class="form-label">
                                                <i class="fas fa-landmark me-1"></i>Bank Name:
                                            </label>
                                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="-- Select Bank --" Value=""></asp:ListItem>
                                                <asp:ListItem Text="KCB Bank" Value="KCB"></asp:ListItem>
                                                <asp:ListItem Text="Equity Bank" Value="EQUITY"></asp:ListItem>
                                                <asp:ListItem Text="Co-operative Bank" Value="COOP"></asp:ListItem>
                                                <asp:ListItem Text="NCBA Bank" Value="NCBA"></asp:ListItem>
                                                <asp:ListItem Text="Standard Chartered" Value="SC"></asp:ListItem>
                                                <asp:ListItem Text="Absa Bank Kenya" Value="ABSA"></asp:ListItem>
                                                <asp:ListItem Text="Other" Value="OTHER"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <label for="txtAccountName" class="form-label">
                                                <i class="fas fa-user me-1"></i>Account Holder Name:
                                            </label>
                                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" 
                                                placeholder="Name on bank account">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="txtAccountNumber" class="form-label">
                                                <i class="fas fa-hashtag me-1"></i>Account Number:
                                            </label>
                                            <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" 
                                                placeholder="Enter account number">
                                            </asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label for="txtBranchCode" class="form-label">
                                                <i class="fas fa-code-branch me-1"></i>Branch Code:
                                            </label>
                                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="form-control" 
                                                placeholder="Enter branch code">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <!-- Credit/Debit Card Details Panel -->
                            <asp:Panel ID="pnlCard" runat="server" CssClass="payment-method-card" Visible="false">
                                <div class="d-flex align-items-center mb-3">
                                    <i class="fas fa-credit-card payment-icon text-primary"></i>
                                    <h4 class="method-title mb-0">Card Details</h4>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="txtCardholderName" class="form-label">
                                                <i class="fas fa-user me-1"></i>Cardholder Name:
                                            </label>
                                            <asp:TextBox ID="txtCardholderName" runat="server" CssClass="form-control" 
                                                placeholder="Name on card">
                                            </asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label for="ddlCardType" class="form-label">
                                                <i class="fas fa-credit-card me-1"></i>Card Type:
                                            </label>
                                            <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="-- Select Card Type --" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
                                                <asp:ListItem Text="MasterCard" Value="MASTERCARD"></asp:ListItem>
                                                <asp:ListItem Text="American Express" Value="AMEX"></asp:ListItem>
                                                <asp:ListItem Text="Other" Value="OTHER"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label for="txtCardNumber" class="form-label">
                                                <i class="fas fa-hashtag me-1"></i>Card Number (Last 4 digits only):
                                            </label>
                                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" 
                                                placeholder="Enter last 4 digits" MaxLength="4">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regexCard" runat="server" 
                                                ControlToValidate="txtCardNumber" 
                                                ValidationExpression="^\d{4}$" 
                                                ErrorMessage="Please enter only the last 4 digits of your card." 
                                                CssClass="text-danger small" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                            <span class="text-muted small">For security, only enter the last 4 digits</span>
                                        </div>
                                        <div class="mb-3">
                                            <label for="txtCardExpiry" class="form-label">
                                                <i class="fas fa-calendar me-1"></i>Expiry Date (MM/YY):
                                            </label>
                                            <asp:TextBox ID="txtCardExpiry" runat="server" CssClass="form-control" 
                                                placeholder="MM/YY" MaxLength="5">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regexExpiry" runat="server" 
                                                ControlToValidate="txtCardExpiry" 
                                                ValidationExpression="^(0[1-9]|1[0-2])\/([0-9]{2})$" 
                                                ErrorMessage="Please enter a valid expiry date (MM/YY)." 
                                                CssClass="text-danger small" Display="Dynamic">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <!-- Save Button -->
                            <div class="mt-4">
                                <asp:Button ID="btnSavePaymentInfo" runat="server" Text="Save Payment Information" 
                                    CssClass="btn btn-save" OnClick="btnSavePaymentInfo_Click">
                                </asp:Button>
                            </div>
                        </div>
                    </div>

                    <!-- Payment Summary Card -->
                    <div class="card payment-card">
                        <div class="card-header bg-primary text-white">
                            <h3 class="card-title mb-0">
                                <i class="fas fa-file-invoice-dollar me-2"></i>Payment Summary
                            </h3>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-4">
                                        <h5>Recent Projects</h5>
                                        <asp:GridView ID="gvRecentProjects" runat="server" CssClass="table table-striped table-hover"
                                            AutoGenerateColumns="false" EmptyDataText="No recent projects found." BorderWidth="0">
                                            <Columns>
                                                <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                                                <asp:BoundField DataField="ProjectAmount" HeaderText="Amount (KES)" DataFormatString="{0:N2}" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-4">
                                        <h5>Payment Statistics</h5>
                                        <div class="card-body bg-light rounded p-3">
                                            <div class="d-flex justify-content-between mb-2">
                                                <span>Total Projects:</span>
                                                <span class="fw-bold"><asp:Label ID="lblTotalProjects" runat="server"></asp:Label></span>
                                            </div>
                                            <div class="d-flex justify-content-between mb-2">
                                                <span>Total Earnings:</span>
                                                <span class="fw-bold">KES <asp:Label ID="lblTotalEarnings" runat="server"></asp:Label></span>
                                            </div>
                                            <div class="d-flex justify-content-between mb-2">
                                                <span>Current Payment Method:</span>
                                                <span class="fw-bold"><asp:Label ID="lblCurrentMethod" runat="server"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Bootstrap JS Bundle with Popper -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>