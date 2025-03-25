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
        .btn-mpesa {
            background-color: #4cd964;
            border-color: #4cd964;
            color: white;
            padding: 0.75rem 1.5rem;
            border-radius: 6px;
            font-weight: 600;
        }
        .btn-mpesa:hover {
            background-color: #3cba54;
            border-color: #3cba54;
            color: white;
        }
        .transaction-alert {
            border-radius: 8px;
            border-left: 5px solid #17a2b8;
        }
        .history-table th {
            background-color: #f1f6ff;
            color: #345;
        }
        .page-title {
            color: #345;
            margin-bottom: 1.5rem;
            font-weight: 600;
        }
        .amount-display {
            font-size: 1.2rem;
            font-weight: 600;
            color: #345;
        }
        .mpesa-logo {
            height: 30px;
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-10">
                    <h2 class="text-center page-title">
                        <i class="fas fa-credit-card me-2"></i>Technician Payment Information
                    </h2>
                    
                    <!-- Payment Details Card -->
                    <div class="card payment-card">
                        <div class="card-header bg-primary text-white">
                            <h3 class="card-title mb-0">
                                <i class="fas fa-file-invoice-dollar me-2"></i>Payment Details
                            </h3>
                        </div>
                        <div class="card-body">
                            <div class="row g-4">
                                <!-- Left Column -->
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Select Project:</label>
                                        <asp:DropDownList ID="ddlProjects" runat="server" CssClass="form-select" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Technician Name:</label>
                                        <asp:Label ID="lblTechnicianName" runat="server" CssClass="form-control" Text=""></asp:Label>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Payment Amount (KES):</label>
                                        <asp:Label ID="lblPaymentAmount" runat="server" CssClass="form-control amount-display" Text=""></asp:Label>
                                    </div>
                                </div>
                                
                                <!-- Right Column - M-Pesa Payment -->
                                <div class="col-md-6">
                                    <div class="card h-100">
                                        <div class="card-header bg-success text-white">
                                            <h4 class="card-title mb-0">
                                                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/M-PESA_LOGO-01.svg/320px-M-PESA_LOGO-01.svg.png" alt="M-Pesa" class="mpesa-logo" />
                                                M-Pesa Payment
                                            </h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="mb-3">
                                                <label for="txtMpesaNumber" class="form-label fw-bold">
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
                                            
                                            <div class="mb-4">
                                                <label for="txtDescription" class="form-label fw-bold">
                                                    <i class="fas fa-file-alt me-1"></i>Payment Description:
                                                </label>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" 
                                                    TextMode="MultiLine" Rows="2" placeholder="Enter payment description">
                                                </asp:TextBox>
                                            </div>
                                            
                                            <asp:Button ID="btnInitiatePayment" runat="server" Text="Initiate M-Pesa Payment" 
                                                CssClass="btn btn-mpesa w-100" OnClick="btnInitiatePayment_Click">
                                            </asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Transaction Status Panel -->
                            <div class="row mt-4">
                                <div class="col-12">
                                    <asp:Panel ID="pnlTransactionStatus" runat="server" Visible="false">
                                        <div class="alert alert-info transaction-alert p-4">
                                            <h4 class="alert-heading">
                                                <i class="fas fa-info-circle me-2"></i>
                                                <asp:Label ID="lblTransactionTitle" runat="server" Text="Transaction Status"></asp:Label>
                                            </h4>
                                            <p class="mb-2"><asp:Label ID="lblTransactionMessage" runat="server" Text=""></asp:Label></p>
                                            <hr />
                                            <p class="mb-0 small">
                                                <i class="fas fa-receipt me-1"></i>
                                                Transaction Reference: <asp:Label ID="lblTransactionRef" runat="server" Text="" CssClass="fw-bold"></asp:Label>
                                            </p>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            
                            <!-- Payment History -->
                            <div class="row mt-4">
                                <div class="col-12">
                                    <h4 class="mb-3">
                                        <i class="fas fa-history me-2"></i>Payment History
                                    </h4>
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvPaymentHistory" runat="server" CssClass="table table-striped table-hover history-table"
                                            AutoGenerateColumns="false" EmptyDataText="No payment records found." BorderWidth="0">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                                <asp:BoundField DataField="ProjectName" HeaderText="Project" />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount (KES)" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="PhoneNumber" HeaderText="M-Pesa Number" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetStatusBadgeClass(Eval("Status").ToString()) %>'>
                                                            <%# Eval("Status") %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransactionId" HeaderText="Transaction ID" />
                                            </Columns>
                                        </asp:GridView>
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