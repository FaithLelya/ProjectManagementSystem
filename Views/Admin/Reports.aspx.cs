using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;
// Add these references for PDF export
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace ProjectManagementSystem.Views.Admin
{
    public partial class Reports : System.Web.UI.Page
    {
        private string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in and is an Admin
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Prevent redirect loop by checking if the current page is not the login page
                if (!Request.Url.AbsolutePath.EndsWith("Login.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/Views/Shared/Login.aspx");
                    Context.ApplicationInstance.CompleteRequest(); // Prevents the thread from aborting
                }
            }

            if (!IsPostBack)
            {
                LoadProjects();
                LoadReportData();
            }
        }

        private void LoadProjects()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ProjectId, ProjectName FROM Projects ORDER BY ProjectName";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlProjects.Items.Add(new System.Web.UI.WebControls.ListItem(
                                reader["ProjectName"].ToString(),
                                reader["ProjectId"].ToString()));
                        }
                    }
                }
            }
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportData();
        }

        protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDateRange.SelectedValue == "custom")
            {
                customDateRange.Visible = true;
            }
            else
            {
                customDateRange.Visible = false;
                LoadReportData();
            }
        }

        protected void btnApplyDates_Click(object sender, EventArgs e)
        {
            LoadReportData();
        }

        private void LoadReportData()
        {
            List<ProjectFinanceReport> reportData = new List<ProjectFinanceReport>();

            // Setup date filters
            DateTime? startDate = null;
            DateTime? endDate = null;

            switch (ddlDateRange.SelectedValue)
            {
                case "month":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDate = startDate.Value.AddMonths(1).AddDays(-1);
                    break;
                case "quarter":
                    int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
                    startDate = new DateTime(DateTime.Now.Year, (currentQuarter - 1) * 3 + 1, 1);
                    endDate = startDate.Value.AddMonths(3).AddDays(-1);
                    break;
                case "year":
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    endDate = new DateTime(DateTime.Now.Year, 12, 31);
                    break;
                case "custom":
                    if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                    {
                        startDate = DateTime.Parse(txtStartDate.Text);
                        endDate = DateTime.Parse(txtEndDate.Text);
                    }
                    break;
            }

            // Build query with filters - Modified to include Attendance payments
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"SELECT p.ProjectId, p.ProjectName, p.Budget, 
                               p.TechnicianPayment, p.MaterialsCost,
                               COALESCE((SELECT SUM(TotalPayment) FROM Attendance WHERE ProjectId = p.ProjectId), 0) as AttendancePayment,
                               p.TechnicianPayment + p.MaterialsCost + COALESCE((SELECT SUM(TotalPayment) FROM Attendance WHERE ProjectId = p.ProjectId), 0) as TotalExpense,
                               (p.Budget - (p.TechnicianPayment + p.MaterialsCost + COALESCE((SELECT SUM(TotalPayment) FROM Attendance WHERE ProjectId = p.ProjectId), 0))) as Variance
                               FROM Projects p
                               WHERE 1=1");

            if (!string.IsNullOrEmpty(ddlProjects.SelectedValue))
            {
                sqlBuilder.Append(" AND p.ProjectId = @ProjectId");
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                sqlBuilder.Append(" AND ((p.StartDate BETWEEN @StartDate AND @EndDate) OR (p.EndDate BETWEEN @StartDate AND @EndDate))");
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sqlBuilder.ToString(), conn))
                {
                    if (!string.IsNullOrEmpty(ddlProjects.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", ddlProjects.SelectedValue);
                    }

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
                    }

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectFinanceReport report = new ProjectFinanceReport
                            {
                                ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                Budget = reader.GetDecimal(reader.GetOrdinal("Budget")),
                                TechnicianPayment = reader.GetDecimal(reader.GetOrdinal("TechnicianPayment")),
                                MaterialsCost = reader.GetDecimal(reader.GetOrdinal("MaterialsCost")),
                                AttendancePayment = reader.GetDecimal(reader.GetOrdinal("AttendancePayment")),
                                TotalExpense = reader.GetDecimal(reader.GetOrdinal("TotalExpense")),
                                Variance = reader.GetDecimal(reader.GetOrdinal("Variance"))
                            };

                            reportData.Add(report);
                        }
                    }
                }
            }

            // Bind data to the grid
            gvProjectData.DataSource = reportData;
            gvProjectData.DataBind();

            // Update summary values
            decimal totalBudget = reportData.Sum(p => p.Budget);
            decimal totalExpenses = reportData.Sum(p => p.TotalExpense);

            litTotalBudget.Text = totalBudget.ToString("N2");
            litTotalExpenses.Text = totalExpenses.ToString("N2");

            // Update progress bar
            int percentage = totalBudget > 0 ? (int)Math.Min(100, (totalExpenses / totalBudget * 100)) : 0;
            progressBar.Attributes["style"] = $"width: {percentage}%";
            progressBar.Attributes["aria-valuenow"] = percentage.ToString();
            progressBar.InnerText = $"{percentage}%";

            // Set progress bar color based on budget status
            if (percentage < 75)
                progressBar.Attributes["class"] = "progress-bar bg-success";
            else if (percentage < 90)
                progressBar.Attributes["class"] = "progress-bar bg-warning";
            else
                progressBar.Attributes["class"] = "progress-bar bg-danger";

            // Initialize chart data
            if (reportData.Any())
            {
                List<string> projectNames = reportData.Select(p => p.ProjectName).ToList();
                List<decimal> budgetData = reportData.Select(p => p.Budget).ToList();
                List<decimal> expenseData = reportData.Select(p => p.TotalExpense).ToList();

                // Convert data to JavaScript arrays
                string projectNamesJson = "['" + string.Join("', '", projectNames) + "']";
                string budgetDataJson = "[" + string.Join(", ", budgetData) + "]";
                string expenseDataJson = "[" + string.Join(", ", expenseData) + "]";

                // Initialize chart
                string chartScript = $"$(document).ready(function() {{ initializeChart('budgetChart', {projectNamesJson}, {budgetDataJson}, {expenseDataJson}); }});";
                ScriptManager.RegisterStartupScript(this, GetType(), "ChartInit", chartScript, true);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Create DataTable with report data
            DataTable dt = new DataTable("Budget_vs_Actual");
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Project ID"),
                new DataColumn("Project Name"),
                new DataColumn("Budget"),
                new DataColumn("Technician Cost"),
                new DataColumn("Materials Cost"),
                new DataColumn("Attendance Payment"),
                new DataColumn("Total Expenses"),
                new DataColumn("Variance"),
                new DataColumn("Status")
            });

            foreach (GridViewRow row in gvProjectData.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Project ID"] = row.Cells[0].Text;
                dr["Project Name"] = row.Cells[1].Text;
                dr["Budget"] = row.Cells[2].Text;
                dr["Technician Cost"] = row.Cells[3].Text;
                dr["Materials Cost"] = row.Cells[4].Text;
                dr["Attendance Payment"] = row.Cells[5].Text;
                dr["Total Expenses"] = row.Cells[6].Text;
                dr["Variance"] = row.Cells[7].Text;
                dr["Status"] = (row.Cells[8].FindControl("lblStatus") as Label)?.Text ?? "";
                dt.Rows.Add(dr);
            }

            // Export the DataTable to Excel
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // Create table header row
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table border='1'>");
                    sb.Append("<tr>");
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append("<th>" + dc.ColumnName + "</th>");
                    }
                    sb.Append("</tr>");

                    // Add data rows
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn dc in dt.Columns)
                        {
                            sb.Append("<td>" + dr[dc].ToString() + "</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    // Export to Excel
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=BudgetReport.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new PDF document
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                // Create memory stream to write the PDF
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                // Open the document to enable writing
                pdfDoc.Open();

                // Add title to the PDF
                Paragraph title = new Paragraph("Budget vs Actual Finances Report",
                    new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(title);

                // Add report generation information
                Paragraph dateInfo = new Paragraph($"Report Generated: {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}",
                    new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC));
                dateInfo.Alignment = Element.ALIGN_RIGHT;
                pdfDoc.Add(dateInfo);

                // Add space after title
                pdfDoc.Add(new Paragraph(" "));

                // Add summary information
                PdfPTable summaryTable = new PdfPTable(2);
                summaryTable.WidthPercentage = 100;

                // Add cells for summary
                PdfPCell cell1 = new PdfPCell(new Phrase("Total Budget", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell cell2 = new PdfPCell(new Phrase($"${litTotalBudget.Text}", new Font(Font.FontFamily.HELVETICA, 12)));
                PdfPCell cell3 = new PdfPCell(new Phrase("Total Expenses", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                PdfPCell cell4 = new PdfPCell(new Phrase($"${litTotalExpenses.Text}", new Font(Font.FontFamily.HELVETICA, 12)));

                cell1.BackgroundColor = new BaseColor(240, 240, 240);
                cell3.BackgroundColor = new BaseColor(240, 240, 240);

                summaryTable.AddCell(cell1);
                summaryTable.AddCell(cell2);
                summaryTable.AddCell(cell3);
                summaryTable.AddCell(cell4);

                pdfDoc.Add(summaryTable);
                pdfDoc.Add(new Paragraph(" "));

                // Create table for the project data
                PdfPTable table = new PdfPTable(9);
                table.WidthPercentage = 100;
                float[] widths = new float[] { 5f, 18f, 9f, 9f, 9f, 9f, 9f, 9f, 13f };
                table.SetWidths(widths);

                // Add header row
                string[] headers = new string[] { "ID", "Project Name", "Budget", "Technician Cost", "Materials Cost", "Attendance Payment", "Total Expenses", "Variance", "Status" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    cell.BackgroundColor = new BaseColor(54, 162, 235);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                // Add data rows
                foreach (GridViewRow row in gvProjectData.Rows)
                {
                    // Add Project ID cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[0].Text)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    // Add Project Name cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[1].Text)) { HorizontalAlignment = Element.ALIGN_LEFT });

                    // Add Budget cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[2].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Add Technician Cost cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[3].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Add Materials Cost cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[4].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Add Attendance Payment cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[5].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Add Total Expenses cell
                    table.AddCell(new PdfPCell(new Phrase(row.Cells[6].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Add Variance cell with appropriate styling
                    PdfPCell varianceCell = new PdfPCell(new Phrase(row.Cells[7].Text)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                    table.AddCell(varianceCell);

                    // Safely parse the variance value to determine status
                    decimal variance = 0;
                    string varianceText = row.Cells[7].Text.Replace("$", "").Replace(",", "");

                    // Try to parse the variance, default to 0 if it fails
                    if (!decimal.TryParse(varianceText, out variance))
                    {
                        // Try to handle different formats or fallback to getting data directly
                        // Option 1: Get value from underlying data item
                        if (row.DataItem is ProjectFinanceReport reportItem)
                        {
                            variance = reportItem.Variance;
                        }
                        // Option 2: If there's a hidden field with the raw value, use that instead
                    }

                    // Add Status cell with appropriate styling
                    string status = variance < 0 ? "Over Budget" : "Under Budget";
                    PdfPCell statusCell = new PdfPCell(new Phrase(status)) { HorizontalAlignment = Element.ALIGN_CENTER };
                    if (variance < 0)
                    {
                        statusCell.BackgroundColor = new BaseColor(255, 99, 132, 100); // Light red
                    }
                    else
                    {
                        statusCell.BackgroundColor = new BaseColor(75, 192, 192, 100); // Light green
                    }
                    table.AddCell(statusCell);
                }

                pdfDoc.Add(table);

                // Add filter information
                pdfDoc.Add(new Paragraph(" "));
                pdfDoc.Add(new Paragraph("Report Filters:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                string projectFilter = ddlProjects.SelectedItem.Text != "-- All Projects --" ?
                    $"Project: {ddlProjects.SelectedItem.Text}" : "Project: All Projects";
                pdfDoc.Add(new Paragraph(projectFilter));

                string dateFilter = "Date Range: ";
                switch (ddlDateRange.SelectedValue)
                {
                    case "all":
                        dateFilter += "All Time";
                        break;
                    case "month":
                        dateFilter += "Current Month";
                        break;
                    case "quarter":
                        dateFilter += "Current Quarter";
                        break;
                    case "year":
                        dateFilter += "Current Year";
                        break;
                    case "custom":
                        dateFilter += $"Custom Range ({txtStartDate.Text} to {txtEndDate.Text})";
                        break;
                }
                pdfDoc.Add(new Paragraph(dateFilter));

                // Close the PDF document
                pdfDoc.Close();

                // Write the PDF to response
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=BudgetReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                // Log the error
                // In a production environment, you should implement proper error logging
                ScriptManager.RegisterStartupScript(this, GetType(), "PDFExportError",
                    $"alert('Error exporting to PDF: {ex.Message}');", true);
            }
        }
    }

    // Model class for the report data - Updated with AttendancePayment property
    public class ProjectFinanceReport
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal Budget { get; set; }
        public decimal TechnicianPayment { get; set; }
        public decimal MaterialsCost { get; set; }
        public decimal AttendancePayment { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Variance { get; set; }
    }
}