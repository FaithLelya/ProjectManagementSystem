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
using ProjectManagementSystem.Helpers;
using System.Data.SQLite;

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

            // Get current user ID
            _currentTechnicianId = GetCurrentUserId();

            // Try to create the table, but don't let it block the page load if it fails
            try
            {
                CreateMpesaPaymentsTableIfNotExists();
            }
            catch (Exception ex)
            {
                // Log the error but continue
                System.Diagnostics.Debug.WriteLine("Error in Page_Load: " + ex.Message);
            }

            if (!IsPostBack)
            {
                try
                {
                    // Load projects dropdown
                    LoadProjects();

                    // Load payment history with modified method
                    LoadPaymentHistory();
                }
                catch (Exception ex)
                {
                    // Log the error and show a friendly message
                    System.Diagnostics.Debug.WriteLine("Error loading data: " + ex.Message);
                    // You could add a label to display a user-friendly error message
                    // lblError.Text = "Unable to load data. Please try again later.";
                }
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
            // In a real application, get this from the Session after authentication
            if (Session["UserID"] != null)
            {
                return Convert.ToInt32(Session["UserID"]);
            }

            // Default for testing - should be replaced with proper authentication
            return 1;
        }

        private void LoadProjects()
        {
            using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT p.ProjectId, p.ProjectName as ProjectName FROM Projects p
                      JOIN ProjectTechnicians pt ON p.ProjectId = pt.ProjectId
                      WHERE pt.TechnicianId = @TechnicianId
                      ORDER BY p.ProjectName";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                    // Create a DataTable to hold the results
                    DataTable projects = new DataTable();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(projects);
                    }

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

                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();
                    string sql = @"SELECT p.*, u.Username 
                          FROM Projects p
                          JOIN ProjectTechnicians pt ON p.ProjectId = pt.ProjectId
                          JOIN User u ON pt.TechnicianId = u.UserID
                          WHERE p.ProjectId = @ProjectId 
                          AND pt.TechnicianId = @TechnicianId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", projectId);
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Get technician name from the database
                                string technicianName = reader["Username"].ToString();
                                lblTechnicianName.Text = technicianName;

                                // Get the payment amount from the database
                                decimal paymentAmount = Convert.ToDecimal(reader["TechnicianPayment"]);
                                lblPaymentAmount.Text = paymentAmount.ToString("N2");

                                // Set default description
                                string projectName = reader["ProjectName"].ToString();
                                txtDescription.Text = $"Payment for {projectName}";
                            }
                        }
                    }
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

       
        private void LoadPaymentHistory()
        {
            DataTable dtPayments = new DataTable();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();
                    string sql = @"SELECT mp.PaymentDate, p.ProjectName as ProjectName, mp.Amount, mp.PhoneNumber, 
                  mp.Status, mp.TransactionId 
                  FROM MpesaPayments mp
                  JOIN Projects p ON mp.ProjectId = p.ProjectId
                  WHERE mp.TechnicianId = @TechnicianId
                  ORDER BY mp.PaymentDate DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dtPayments);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Table doesn't exist or other SQLite error
                System.Diagnostics.Debug.WriteLine("SQLite error: " + ex.Message);

                // Continue with empty data table that we'll populate with sample data
                dtPayments = new DataTable();
                dtPayments.Columns.Add("PaymentDate", typeof(DateTime));
                dtPayments.Columns.Add("ProjectName", typeof(string));
                dtPayments.Columns.Add("Amount", typeof(decimal));
                dtPayments.Columns.Add("PhoneNumber", typeof(string));
                dtPayments.Columns.Add("Status", typeof(string));
                dtPayments.Columns.Add("TransactionId", typeof(string));
            }

            // If no records found, add placeholder data for demo purposes
            if (dtPayments.Rows.Count == 0)
            {
                // Make sure the columns are defined if they weren't created from the query
                if (dtPayments.Columns.Count == 0)
                {
                    dtPayments.Columns.Add("PaymentDate", typeof(DateTime));
                    dtPayments.Columns.Add("ProjectName", typeof(string));
                    dtPayments.Columns.Add("Amount", typeof(decimal));
                    dtPayments.Columns.Add("PhoneNumber", typeof(string));
                    dtPayments.Columns.Add("Status", typeof(string));
                    dtPayments.Columns.Add("TransactionId", typeof(string));
                }

                // Add sample data
                dtPayments.Rows.Add(DateTime.Now.AddDays(-5), "Project Alpha: Karen SmartHome", 20000, "254712345678", "Completed", "MPJ78HK1L1");
                dtPayments.Rows.Add(DateTime.Now.AddDays(-12), "Project Beta: CCTV Installation", 30000, "254712345678", "Completed", "MPJ62VB4T9");
                dtPayments.Rows.Add(DateTime.Now.AddDays(-2), "Project Gamma: Office Security", 15000, "254712345678", "Pending", "MPJ89VP3R5");
            }

            gvPaymentHistory.DataSource = dtPayments;
            gvPaymentHistory.DataBind();
        }
        private void CreateMpesaPaymentsTableIfNotExists()
        {
            // Use a connection string with pooling enabled and higher timeouts
            string connString = SQLiteHelper.ConnectionString + ";Pooling=True;Default Timeout=30";

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                CREATE TABLE IF NOT EXISTS MpesaPayments (
                    PaymentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProjectId INTEGER NOT NULL,
                    TechnicianId INTEGER NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    Amount DECIMAL(10,2) NOT NULL,
                    Description TEXT,
                    TransactionId TEXT,
                    Status TEXT NOT NULL,
                    PaymentDate DATETIME NOT NULL,
                    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
                    FOREIGN KEY (TechnicianId) REFERENCES User(UserID)
                );";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        // Set command timeout to avoid locking issues
                        cmd.CommandTimeout = 30;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex)
                {
                    // Log the exception but don't throw - just continue with the application
                    System.Diagnostics.Debug.WriteLine("Error creating MpesaPayments table: " + ex.Message);
                }
                finally
                {
                    // Ensure connection is closed even if an exception occurs
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
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

            // Get the selected project ID and other details
            int projectId = Convert.ToInt32(ddlProjects.SelectedValue);
            string mpesaNumber = txtMpesaNumber.Text.Trim();
            string description = txtDescription.Text.Trim();

            // Get the payment amount from the database
            decimal amount = 0;
            using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TechnicianPayment FROM Projects WHERE ProjectId = @ProjectId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        amount = Convert.ToDecimal(result);
                    }
                    else
                    {
                        ShowTransactionStatus("Error", "Could not retrieve payment amount for this project.", "");
                        return;
                    }
                }
            }

            // Call the M-Pesa payment controller to initiate payment
            string transactionRef = _paymentController.InitiatePayment(mpesaNumber, amount, description);

            // Save the transaction to the database
            SaveTransaction(projectId, mpesaNumber, amount, description, transactionRef);

            // Show transaction status
            ShowTransactionStatus("Payment Initiated",
                "An M-Pesa payment request has been sent to " + mpesaNumber +
                ". Please check your phone to complete the transaction.",
                transactionRef);
        }

        private void SaveTransaction(int projectId, string phoneNumber, decimal amount, string description, string transactionRef)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO MpesaPayments 
                          (ProjectId, TechnicianId, PhoneNumber, Amount, Description, TransactionId, Status, PaymentDate) 
                          VALUES 
                          (@ProjectId, @TechnicianId, @PhoneNumber, @Amount, @Description, @TransactionId, @Status, @PaymentDate)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", projectId);
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@TransactionId", transactionRef);
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error saving transaction: " + ex.Message);
            }
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