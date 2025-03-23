using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Technicians
{
    public partial class PaymentInfo : System.Web.UI.Page
    {
        private ProjectController _projectController;
        private MpesaPaymentController _paymentController;
        private int _currentTechnicianId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize controllers
            _projectController = new ProjectController();
            _paymentController = new MpesaPaymentController();

            // Get current user ID (assuming you have authentication implemented)
            // For now, using a placeholder value
            _currentTechnicianId = GetCurrentUserId();

            if (!IsPostBack)
            {
                // Load projects dropdown
                LoadProjects();

                // Load payment history
                LoadPaymentHistory();
            }
        }

        // Helper method for status badge styling
        protected string GetStatusBadgeClass(string status)
        {
            switch (status.ToLower())
            {
                case "completed":
                    return "bg-success";
                case "pending":
                    return "bg-warning text-dark";
                case "processing":
                    return "bg-info text-dark";
                case "failed":
                    return "bg-danger";
                default:
                    return "bg-secondary";
            }
        }

        private int GetCurrentUserId()
        {
            // In a real application, you would get this from authentication
            // For now, we'll return a placeholder value
            return 1; // Assuming user ID 1 for testing
        }

        private void LoadProjects()
        {
            // Get all projects this technician is assigned to
            List<Project> projects = _projectController.GetProjects()
                .Where(p => _projectController.IsTechnicianAssignedToProject(_currentTechnicianId, p.ProjectId))
                .ToList();

            ddlProjects.DataSource = projects;
            ddlProjects.DataTextField = "ProjectName";
            ddlProjects.DataValueField = "ProjectId";
            ddlProjects.DataBind();

            // Add a prompt item
            ddlProjects.Items.Insert(0, new ListItem("-- Select Project --", "0"));

            // If there are projects, select the first one and load its details
            if (ddlProjects.Items.Count > 1)
            {
                ddlProjects.SelectedIndex = 1;
                LoadProjectPaymentDetails();
            }
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectPaymentDetails();
        }

        private void LoadProjectPaymentDetails()
        {
            if (ddlProjects.SelectedIndex > 0)
            {
                int projectId = Convert.ToInt32(ddlProjects.SelectedValue);
                Project project = _projectController.GetProjects()
                    .FirstOrDefault(p => p.ProjectId == projectId);

                if (project != null)
                {
                    // Get technician details - in a real app, you'd get this from your user service
                    string technicianName = GetTechnicianName(_currentTechnicianId);
                    lblTechnicianName.Text = technicianName;

                    // Calculate payment amount - this could be based on your business logic
                    // For simplicity, we're using the TechnicianPayment field from the Project model
                    decimal paymentAmount = project.TechnicianPayment;
                    lblPaymentAmount.Text = paymentAmount.ToString("N2");

                    // Set default description
                    txtDescription.Text = $"Payment for {project.ProjectName}";
                }
            }
            else
            {
                // Clear the fields if no project is selected
                lblTechnicianName.Text = "";
                lblPaymentAmount.Text = "";
                txtDescription.Text = "";
            }
        }

        private string GetTechnicianName(int technicianId)
        {
            // In a real app, get this from your database
            // For now, returning a placeholder
            return "John Doe";
        }

        private void LoadPaymentHistory()
        {
            // In a real application, you would fetch this from your database
            // For now, we'll create sample data
            DataTable dtPayments = new DataTable();
            dtPayments.Columns.Add("PaymentDate", typeof(DateTime));
            dtPayments.Columns.Add("ProjectName", typeof(string));
            dtPayments.Columns.Add("Amount", typeof(decimal));
            dtPayments.Columns.Add("PhoneNumber", typeof(string));
            dtPayments.Columns.Add("Status", typeof(string));
            dtPayments.Columns.Add("TransactionId", typeof(string));

            // Add sample data
            dtPayments.Rows.Add(DateTime.Now.AddDays(-5), "Project Alpha: Karen SmartHome", 20000, "254712345678", "Completed", "MPJ78HK1L1");
            dtPayments.Rows.Add(DateTime.Now.AddDays(-12), "Project Beta: CCTV Installation", 30000, "254712345678", "Completed", "MPJ62VB4T9");
            dtPayments.Rows.Add(DateTime.Now.AddDays(-2), "Project Gamma: Office Security", 15000, "254712345678", "Pending", "MPJ89VP3R5");

            gvPaymentHistory.DataSource = dtPayments;
            gvPaymentHistory.DataBind();
        }

        protected void btnInitiatePayment_Click(object sender, EventArgs e)
        {
            // Validate form
            if (!IsValid)
                return;

            // Check if a project is selected
            if (ddlProjects.SelectedIndex <= 0)
            {
                ShowTransactionStatus("Error", "Please select a project first.", "");
                return;
            }

            // Get the selected project and payment amount
            int projectId = Convert.ToInt32(ddlProjects.SelectedValue);
            Project project = _projectController.GetProjects()
                .FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                ShowTransactionStatus("Error", "Project not found.", "");
                return;
            }

            decimal amount = project.TechnicianPayment;
            string mpesaNumber = txtMpesaNumber.Text.Trim();
            string description = txtDescription.Text.Trim();

            // Call the M-Pesa payment controller to initiate payment
            string transactionRef = _paymentController.InitiatePayment(mpesaNumber, amount, description);

            // Show transaction status
            ShowTransactionStatus("Payment Initiated",
                "An M-Pesa payment request has been sent to " + mpesaNumber +
                ". Please check your phone to complete the transaction.",
                transactionRef);

            // In a real application, you would save this transaction to your database
            // and implement a callback to handle the M-Pesa response
        }

        private void ShowTransactionStatus(string title, string message, string reference)
        {
            lblTransactionTitle.Text = title;
            lblTransactionMessage.Text = message;
            lblTransactionRef.Text = reference;
            pnlTransactionStatus.Visible = true;
        }
    }
}