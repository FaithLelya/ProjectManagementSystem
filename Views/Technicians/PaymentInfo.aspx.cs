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
        private int _currentTechnicianId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get current user ID
            _currentTechnicianId = GetCurrentUserId();

            // Try to create the table, but don't let it block the page load if it fails
            try
            {
                CreatePaymentInfoTableIfNotExists();
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
                    // Load technician's personal info
                    LoadTechnicianInfo();

                    // Load technician's payment info
                    LoadPaymentInfo();

                    // Load recent projects
                    LoadRecentProjects();

                    // Load payment statistics
                    LoadPaymentStatistics();
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine("Error loading data: " + ex.Message);
                }
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

        private void LoadTechnicianInfo()
        {
            using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT UserID, Username, Email
                  FROM User
                  WHERE UserID = @TechnicianId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate the personal info fields with data from the database
                            lblTechnicianId.Text = reader["UserID"].ToString();
                            lblTechnicianName.Text = reader["Username"].ToString();
                            lblTechnicianEmail.Text = reader["Email"].ToString();
                            //lblTechnicianPhone.Text = reader["PhoneNumber"].ToString();
                        }
                        else
                        {
                            // Display message if no user data found
                            lblTechnicianId.Text = _currentTechnicianId.ToString();
                            lblTechnicianName.Text = "Unknown User";
                            lblTechnicianEmail.Text = "User data not found";
                            lblTechnicianPhone.Text = "Please update your profile";

                            // Log the issue
                            System.Diagnostics.Debug.WriteLine($"No user data found for UserID: {_currentTechnicianId}");

                            // Optionally display a message to the user
                            ShowSuccessMessage("Your profile information could not be loaded. Please contact support.", false);
                        }
                    }
                }
            }
        }
        private void LoadPaymentInfo()
        {
            using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT PreferredPaymentMethod, MpesaNumber, MpesaName,
                          BankName, AccountNumber, AccountName, BranchCode,
                          CardType, CardLastFour, CardholderName, CardExpiry
                          FROM TechnicianPaymentInfo
                          WHERE TechnicianId = @TechnicianId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                    try
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Set the preferred payment method
                                string preferredMethod = reader["PreferredPaymentMethod"].ToString();
                                if (!string.IsNullOrEmpty(preferredMethod))
                                {
                                    ddlPaymentMethod.SelectedValue = preferredMethod;

                                    // Show the appropriate panel
                                    ShowSelectedPaymentMethodPanel();

                                    // Populate fields based on preferred method
                                    switch (preferredMethod)
                                    {
                                        case "MPESA":
                                            txtMpesaNumber.Text = reader["MpesaNumber"].ToString();
                                            txtMpesaName.Text = reader["MpesaName"].ToString();
                                            break;
                                        case "BANK":
                                            ddlBank.SelectedValue = reader["BankName"].ToString();
                                            txtAccountNumber.Text = reader["AccountNumber"].ToString();
                                            txtAccountName.Text = reader["AccountName"].ToString();
                                            txtBranchCode.Text = reader["BranchCode"].ToString();
                                            break;
                                        case "CARD":
                                            ddlCardType.SelectedValue = reader["CardType"].ToString();
                                            txtCardNumber.Text = reader["CardLastFour"].ToString();
                                            txtCardholderName.Text = reader["CardholderName"].ToString();
                                            txtCardExpiry.Text = reader["CardExpiry"].ToString();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        // Table probably doesn't exist yet
                        System.Diagnostics.Debug.WriteLine("SQLite error: " + ex.Message);
                    }
                }
            }
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedPaymentMethodPanel();
        }

        private void ShowSelectedPaymentMethodPanel()
        {
            // Hide all payment panels first
            pnlMpesa.Visible = false;
            pnlBank.Visible = false;
            pnlCard.Visible = false;

            // Show the selected panel
            switch (ddlPaymentMethod.SelectedValue)
            {
                case "MPESA":
                    pnlMpesa.Visible = true;
                    break;
                case "BANK":
                    pnlBank.Visible = true;
                    break;
                case "CARD":
                    pnlCard.Visible = true;
                    break;
            }
        }

        protected void btnSavePaymentInfo_Click(object sender, EventArgs e)
        {
            // Validate form
            if (!IsValid)
                return;

            // Check if payment method is selected
            if (string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
            {
                ShowSuccessMessage("Please select a payment method");
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();

                    // Check if the record already exists
                    bool recordExists = false;
                    string checkSql = "SELECT COUNT(*) FROM TechnicianPaymentInfo WHERE TechnicianId = @TechnicianId";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        recordExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                    }

                    // Create SQL for insert or update
                    string sql;
                    if (recordExists)
                    {
                        sql = @"UPDATE TechnicianPaymentInfo SET 
                              PreferredPaymentMethod = @PreferredPaymentMethod,
                              MpesaNumber = @MpesaNumber,
                              MpesaName = @MpesaName,
                              BankName = @BankName,
                              AccountNumber = @AccountNumber,
                              AccountName = @AccountName,
                              BranchCode = @BranchCode,
                              CardType = @CardType,
                              CardLastFour = @CardLastFour,
                              CardholderName = @CardholderName,
                              CardExpiry = @CardExpiry,
                              LastUpdated = @LastUpdated
                              WHERE TechnicianId = @TechnicianId";
                    }
                    else
                    {
                        sql = @"INSERT INTO TechnicianPaymentInfo (
                              TechnicianId, PreferredPaymentMethod,
                              MpesaNumber, MpesaName,
                              BankName, AccountNumber, AccountName, BranchCode,
                              CardType, CardLastFour, CardholderName, CardExpiry,
                              LastUpdated)
                              VALUES (
                              @TechnicianId, @PreferredPaymentMethod,
                              @MpesaNumber, @MpesaName,
                              @BankName, @AccountNumber, @AccountName, @BranchCode,
                              @CardType, @CardLastFour, @CardholderName, @CardExpiry,
                              @LastUpdated)";
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        cmd.Parameters.AddWithValue("@PreferredPaymentMethod", ddlPaymentMethod.SelectedValue);
                        cmd.Parameters.AddWithValue("@MpesaNumber", txtMpesaNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@MpesaName", txtMpesaName.Text.Trim());
                        cmd.Parameters.AddWithValue("@BankName", ddlBank.SelectedValue);
                        cmd.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccountName", txtAccountName.Text.Trim());
                        cmd.Parameters.AddWithValue("@BranchCode", txtBranchCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@CardType", ddlCardType.SelectedValue);
                        cmd.Parameters.AddWithValue("@CardLastFour", txtCardNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@CardholderName", txtCardholderName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CardExpiry", txtCardExpiry.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastUpdated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }
                }

                // Show success message
                ShowSuccessMessage("Your payment information has been successfully saved.");

                // Refresh payment statistics
                LoadPaymentStatistics();

            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error saving payment info: " + ex.Message);

                // Show error message to the user
                ShowSuccessMessage("Error saving payment information. Please try again later.", false);
            }
        }

        private void CreatePaymentInfoTableIfNotExists()
        {
            using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
            {
                conn.Open();

                string sql = @"
                CREATE TABLE IF NOT EXISTS TechnicianPaymentInfo (
                    TechnicianId INTEGER PRIMARY KEY,
                    PreferredPaymentMethod TEXT,
                    MpesaNumber TEXT,
                    MpesaName TEXT,
                    BankName TEXT,
                    AccountNumber TEXT,
                    AccountName TEXT,
                    BranchCode TEXT,
                    CardType TEXT,
                    CardLastFour TEXT,
                    CardholderName TEXT,
                    CardExpiry TEXT,
                    LastUpdated TEXT
                );";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadRecentProjects()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();

                    // Create a temporary projects table if needed for testing
                    CreateProjectsTableIfNotExists(conn);

                    string sql = @"
                        SELECT p.ProjectName, p.ProjectAmount 
                        FROM Projects p
                        JOIN ProjectAssignments pa ON p.ProjectId = pa.ProjectId
                        WHERE pa.TechnicianId = @TechnicianId
                        ORDER BY p.CreatedDate DESC
                        LIMIT 5";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);

                        DataTable dt = new DataTable();
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        // If no records found, add sample data for demonstration
                        if (dt.Rows.Count == 0)
                        {
                            AddSampleProjectData(dt);
                        }

                        gvRecentProjects.DataSource = dt;
                        gvRecentProjects.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading recent projects: " + ex.Message);

                // Use sample data as fallback
                DataTable dt = new DataTable();
                AddSampleProjectData(dt);

                gvRecentProjects.DataSource = dt;
                gvRecentProjects.DataBind();
            }
        }

        private void CreateProjectsTableIfNotExists(SQLiteConnection conn)
        {
            // Create the Projects table if it doesn't exist
            string createProjectsTable = @"
                CREATE TABLE IF NOT EXISTS Projects (
                    ProjectId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProjectName TEXT NOT NULL,
                    ProjectDescription TEXT,
                    ProjectAmount DECIMAL(10,2) NOT NULL,
                    CreatedDate TEXT NOT NULL
                );";

            using (SQLiteCommand cmd = new SQLiteCommand(createProjectsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Create the ProjectAssignments table if it doesn't exist
            string createAssignmentsTable = @"
                CREATE TABLE IF NOT EXISTS ProjectAssignments (
                    AssignmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProjectId INTEGER NOT NULL,
                    TechnicianId INTEGER NOT NULL,
                    AssignedDate TEXT NOT NULL,
                    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
                    FOREIGN KEY (TechnicianId) REFERENCES User(UserID)
                );";

            using (SQLiteCommand cmd = new SQLiteCommand(createAssignmentsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Check if we have any sample data and add if we don't
            string countQuery = "SELECT COUNT(*) FROM Projects";
            using (SQLiteCommand cmd = new SQLiteCommand(countQuery, conn))
            {
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    // Add some sample projects
                    AddSampleProjects(conn);
                }
            }
        }

        private void AddSampleProjects(SQLiteConnection conn)
        {
            // Sample project data
            string[] projectNames = {
                "Website Redesign",
                "Mobile App Development",
                "Network Configuration",
                "Database Optimization",
                "Security Audit"
            };

            string[] projectDescriptions = {
                "Redesign the company website with modern UI/UX",
                "Develop a mobile app for customer engagement",
                "Configure network infrastructure for new office",
                "Optimize database performance for high traffic",
                "Perform security audit and implement recommendations"
            };

            decimal[] projectAmounts = {
                45000.00M,
                75000.00M,
                30000.00M,
                25000.00M,
                35000.00M
            };

            // Insert sample projects
            for (int i = 0; i < projectNames.Length; i++)
            {
                string insertSql = @"
                    INSERT INTO Projects (ProjectName, ProjectDescription, ProjectAmount, CreatedDate)
                    VALUES (@ProjectName, @ProjectDescription, @ProjectAmount, @CreatedDate);
                    SELECT last_insert_rowid();";

                int projectId;
                using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectName", projectNames[i]);
                    cmd.Parameters.AddWithValue("@ProjectDescription", projectDescriptions[i]);
                    cmd.Parameters.AddWithValue("@ProjectAmount", projectAmounts[i]);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.AddDays(-i * 7).ToString("yyyy-MM-dd HH:mm:ss"));

                    projectId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Assign the project to current technician
                string assignSql = @"
                    INSERT INTO ProjectAssignments (ProjectId, TechnicianId, AssignedDate)
                    VALUES (@ProjectId, @TechnicianId, @AssignedDate);";

                using (SQLiteCommand cmd = new SQLiteCommand(assignSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                    cmd.Parameters.AddWithValue("@AssignedDate", DateTime.Now.AddDays(-i * 7).ToString("yyyy-MM-dd HH:mm:ss"));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddSampleProjectData(DataTable dt)
        {
            // Create columns if needed
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("ProjectName", typeof(string));
                dt.Columns.Add("ProjectAmount", typeof(decimal));
            }

            // Add sample rows
            dt.Rows.Add("Website Redesign", 45000.00M);
            dt.Rows.Add("Mobile App Development", 75000.00M);
            dt.Rows.Add("Network Configuration", 30000.00M);
            dt.Rows.Add("Database Optimization", 25000.00M);
            dt.Rows.Add("Security Audit", 35000.00M);
        }

        private void LoadPaymentStatistics()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(SQLiteHelper.ConnectionString))
                {
                    conn.Open();

                    // Get total project count
                    string countSql = @"
                        SELECT COUNT(*) 
                        FROM ProjectAssignments 
                        WHERE TechnicianId = @TechnicianId";

                    int projectCount = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(countSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            projectCount = Convert.ToInt32(result);
                        }
                    }

                    // Get total earnings
                    string earningSql = @"
                        SELECT SUM(p.ProjectAmount) 
                        FROM Projects p
                        JOIN ProjectAssignments pa ON p.ProjectId = pa.ProjectId 
                        WHERE pa.TechnicianId = @TechnicianId";

                    decimal totalEarnings = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(earningSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalEarnings = Convert.ToDecimal(result);
                        }
                    }

                    // Get current payment method
                    string methodSql = @"
                        SELECT PreferredPaymentMethod 
                        FROM TechnicianPaymentInfo 
                        WHERE TechnicianId = @TechnicianId";

                    string currentMethod = "Not Set";
                    using (SQLiteCommand cmd = new SQLiteCommand(methodSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TechnicianId", _currentTechnicianId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string methodCode = result.ToString();
                            switch (methodCode)
                            {
                                case "MPESA":
                                    currentMethod = "M-Pesa";
                                    break;
                                case "BANK":
                                    currentMethod = "Bank Transfer";
                                    break;
                                case "CARD":
                                    currentMethod = "Credit/Debit Card";
                                    break;
                                default:
                                    currentMethod = "Not Set";
                                    break;
                            }
                        }
                    }

                    // Update UI
                    lblTotalProjects.Text = projectCount.ToString();
                    lblTotalEarnings.Text = totalEarnings.ToString("N2");
                    lblCurrentMethod.Text = currentMethod;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading payment statistics: " + ex.Message);

                // Use sample statistics as fallback
                lblTotalProjects.Text = "5";
                lblTotalEarnings.Text = "210,000.00";
                lblCurrentMethod.Text = ddlPaymentMethod.SelectedItem?.Text ?? "Not Set";
            }
        }

        private void ShowSuccessMessage(string message, bool isSuccess = true)
        {
            lblSuccessMessage.Text = message;
            pnlSuccess.Visible = true;

            if (isSuccess)
            {
                pnlSuccess.CssClass = "alert alert-success success-alert mb-4";
            }
            else
            {
                pnlSuccess.CssClass = "alert alert-danger mb-4";
            }
        }
    }
}