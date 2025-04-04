<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ProjectManagementSystem.Views.Admin.Reports" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget vs Actual Finances Report</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/css/projects.css" rel="stylesheet" type="text/css" />
    <link href="~/Content/sidebar.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js"></script>
    <!-- Add jsPDF library -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf-autotable/3.5.25/jspdf.plugin.autotable.min.js"></script>
    <style>
        .export-buttons {
            margin-top: 20px;
        }
        .export-buttons .btn {
            margin-right: 10px;
        }
        .export-icon {
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="container">
             <div class="row align-items-center mb-3">
     <div class="col-auto">
         <a href="/Views/Shared/Dashboard/Welcome.aspx" class="btn btn-outline-secondary">
             <i class="fas fa-arrow-left mr-2"></i>Back to Dashboard
         </a>
     </div>
    
 </div>
            <h1>Budget vs Actual Finances Report</h1>
            
            <div class="row mb-4">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">Report Filters</div>
                        <div class="card-body">
                            <div class="form-group">
                                <label for="ddlProjects">Select Project:</label>
                                <asp:DropDownList ID="ddlProjects" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="-- All Projects --" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="ddlDateRange">Date Range:</label>
                                <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
                                    <asp:ListItem Value="all" Text="All Time" />
                                    <asp:ListItem Value="month" Text="Current Month" />
                                    <asp:ListItem Value="quarter" Text="Current Quarter" />
                                    <asp:ListItem Value="year" Text="Current Year" />
                                    <asp:ListItem Value="custom" Text="Custom Range" />
                                </asp:DropDownList>
                            </div>
                            <div id="customDateRange" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="txtStartDate">Start Date:</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtEndDate">End Date:</label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnApplyDates" runat="server" Text="Apply" CssClass="btn btn-primary" OnClick="btnApplyDates_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">Summary</div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="card bg-light mb-3">
                                        <div class="card-body">
                                            <h5 class="card-title">Total Budget</h5>
                                            <h3 class="card-text text-primary">KSH<asp:Literal ID="litTotalBudget" runat="server">0.00</asp:Literal></h3>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="card bg-light mb-3">
                                        <div class="card-body">
                                            <h5 class="card-title">Total Expenses</h5>
                                            <h3 class="card-text text-danger">KSH<asp:Literal ID="litTotalExpenses" runat="server">0.00</asp:Literal></h3>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="progress mt-3" style="height: 25px;">
                                <div id="progressBar" runat="server" class="progress-bar" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">0%</div>
                            </div>
                            <p class="mt-2"><small>Budget Utilization</small></p>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="card mb-4">
                <div class="card-header">
                    <h3>Budget vs Actual by Project</h3>
                </div>
                <div class="card-body">
                    <div style="height: 300px">
                        <canvas id="budgetChart" runat="server"></canvas>
                    </div>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h3 class="mb-0">Detailed Project Data</h3>
                    <div class="export-buttons">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <button type="button" id="btnExportPDFClient" class="btn btn-danger">Export to PDF</button>
                                <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CssClass="btn btn-danger d-none" OnClick="btnExportPDF_Click" OnClientClick="return false;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvProjectData" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ProjectId" HeaderText="ID" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="Budget" HeaderText="Budget" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="TechnicianPayment" HeaderText="Technician Cost" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="MaterialsCost" HeaderText="Materials Cost" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="TotalExpense" HeaderText="Total Expenses" DataFormatString="{0:C2}" />
                            <asp:TemplateField HeaderText="Variance">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToDecimal(Eval("Variance")) < 0 ? "text-danger" : "text-success" %>'>
                                        <%# Eval("Variance", "{0:C2}") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" CssClass='<%# Convert.ToDecimal(Eval("Variance")) < 0 ? "badge badge-danger" : "badge badge-success" %>' 
                                        Text='<%# Convert.ToDecimal(Eval("Variance")) < 0 ? "Over Budget" : "Under Budget" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No data found for the selected criteria.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function initializeChart(chartId, projectNames, budgetData, expenseData) {
            var ctx = document.getElementById(chartId).getContext('2d');
            var chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: projectNames,
                    datasets: [
                        {
                            label: 'Budget',
                            data: budgetData,
                            backgroundColor: 'rgba(54, 162, 235, 0.5)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        },
                        {
                            label: 'Actual Expenses',
                            data: expenseData,
                            backgroundColor: 'rgba(255, 99, 132, 0.5)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }

        // Wait for the document to be fully loaded
        document.addEventListener('DOMContentLoaded', function () {
            // Get reference to the export PDF button
            const exportPdfButton = document.getElementById('btnExportPDFClient');

            if (exportPdfButton) {
                exportPdfButton.addEventListener('click', exportToPDF);
            }
        });

        function exportToPDF() {
            // Using jsPDF
            const { jsPDF } = window.jspdf;
            const doc = new jsPDF();

            // Add title
            doc.setFontSize(18);
            doc.text('Budget vs Actual Finances Report', 14, 20);

            // Add date
            doc.setFontSize(12);
            doc.text('Generated: ' + new Date().toLocaleDateString(), 14, 30);

            // Add summary information
            doc.setFontSize(14);
            doc.text('Summary', 14, 45);

            const totalBudget = document.querySelector('.text-primary').textContent;
            const totalExpenses = document.querySelector('.text-danger').textContent;

            doc.setFontSize(12);
            doc.text('Total Budget: ' + totalBudget, 14, 55);
            doc.text('Total Expenses: ' + totalExpenses, 14, 65);

            // Create table for detailed project data
            const table = document.getElementById('gvProjectData');
            if (table) {
                // Extract table headers
                const headers = [];
                for (let i = 0; i < table.rows[0].cells.length; i++) {
                    headers.push(table.rows[0].cells[i].textContent.trim());
                }

                // Extract table data
                const data = [];
                for (let i = 1; i < table.rows.length; i++) {
                    const rowData = [];
                    for (let j = 0; j < table.rows[i].cells.length; j++) {
                        rowData.push(table.rows[i].cells[j].textContent.trim());
                    }
                    data.push(rowData);
                }

                // Add table to PDF
                doc.text('Detailed Project Data', 14, 80);

                doc.autoTable({
                    head: [headers],
                    body: data,
                    startY: 85,
                    theme: 'grid',
                    styles: { fontSize: 10 },
                    headStyles: { fillColor: [54, 162, 235] },
                    alternateRowStyles: { fillColor: [240, 240, 240] },
                    margin: { top: 90 }
                });
            }

            // Add current chart if possible
            try {
                const chartCanvas = document.getElementById('budgetChart');
                if (chartCanvas) {
                    const chartImage = chartCanvas.toDataURL('image/png');

                    // Add new page if needed
                    if (doc.lastAutoTable.finalY > 180) {
                        doc.addPage();
                        doc.text('Budget vs Actual by Project Chart', 14, 20);
                        doc.addImage(chartImage, 'PNG', 14, 30, 180, 100);
                    } else {
                        const yPosition = doc.lastAutoTable.finalY + 20;
                        doc.text('Budget vs Actual by Project Chart', 14, yPosition);
                        doc.addImage(chartImage, 'PNG', 14, yPosition + 10, 180, 100);
                    }
                }
            } catch (error) {
                console.log('Could not add chart to PDF: ' + error.message);
            }

            // Save the PDF
            doc.save('BudgetVsActualReport.pdf');
        }
    </script>
</body>
</html>