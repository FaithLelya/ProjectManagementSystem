using System;
using ProjectManagementSystem.Controllers;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Web.UI;
using ProjectManagementSystem.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace ProjectManagementSystem.Views.Technicians
{
    public partial class RecordAttendance : System.Web.UI.Page
    {
        private ProjectController _projectController;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTechnicians();
                LoadProjects();
            }
        }

        private void LoadTechnicians()
        {
            // Load junior technicians from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianID, Username FROM Technician WHERE TechnicianLevel = 'Junior'";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlTechnician.Items.Clear(); // Clear existing items to avoid duplicates
                    ddlTechnician.Items.Add(new ListItem("-- Select Technician --", ""));

                    // Check if there are any rows returned
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlTechnician.Items.Add(new ListItem(reader["Username"].ToString(), reader["TechnicianID"].ToString()));
                        }
                    }
                    else
                    {
                        // Handle the case where no technicians are found
                        lblMessage.Text = "No technicians found.";
                        lblMessage.CssClass = "ms-3 text-danger";
                    }
                }
            }
        }

        private void LoadProjects()
        {
            // Load projects from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT ProjectId, ProjectName FROM Projects";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlProject.Items.Clear(); // Clear existing items to avoid duplicates
                    ddlProject.Items.Add(new ListItem("-- Select Project --", ""));

                    while (reader.Read())
                    {
                        ddlProject.Items.Add(new ListItem(reader["ProjectName"].ToString(), reader["ProjectId"].ToString()));
                    }
                }
            }
        }

        protected void ddlTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load technician rates when a technician is selected
            if (!string.IsNullOrEmpty(ddlTechnician.SelectedValue))
            {
                LoadTechnicianRates(int.Parse(ddlTechnician.SelectedValue));
            }
        }

        private void LoadTechnicianRates(int technicianId)
        {
            try
            {
                decimal hourlyRate = 0;
                decimal overtimeRate = 0;

                // Fetch technician hourly rate and overtime rate from database
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string query = "SELECT HourlyRate, OvertimeRate FROM Technician WHERE TechnicianID = @TechnicianId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TechnicianId", technicianId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hourlyRate = reader.IsDBNull(reader.GetOrdinal("HourlyRate")) ? 0 :
                                    Convert.ToDecimal(reader["HourlyRate"]);
                                overtimeRate = reader.IsDBNull(reader.GetOrdinal("OvertimeRate")) ? 0 :
                                    Convert.ToDecimal(reader["OvertimeRate"]);
                            }
                        }
                    }
                }

                // Create JavaScript object with technician rates
                var technicianRateData = new
                {
                    technicianId = technicianId,
                    hourlyRate = hourlyRate,
                    overtimeRate = overtimeRate
                };

                // Serialize to JSON
                string jsonRates = $"technicianRates[{technicianId}] = {{hourlyRate: {hourlyRate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, overtimeRate: {overtimeRate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}}};";

                // Register script to update the JavaScript object
                ScriptManager.RegisterStartupScript(this, this.GetType(), $"technicianRates_{technicianId}",
                    jsonRates + " calculateHours();", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading technician rates: " + ex.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset message
                lblMessage.Text = "";
                lblMessage.CssClass = "ms-3";

                // Validate that a technician and project are selected
                if (string.IsNullOrEmpty(ddlTechnician.SelectedValue) || string.IsNullOrEmpty(ddlProject.SelectedValue))
                {
                    lblMessage.Text = "Please select a technician and a project.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Validate the date - not in the future
                DateTime date;
                if (!DateTime.TryParse(txtDate.Text, out date))
                {
                    lblMessage.Text = "Please enter a valid date.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Ensure the date is not in the future
                if (date.Date > DateTime.Now.Date)
                {
                    lblMessage.Text = "Attendance can only be recorded for today or past dates.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Check if the technician is marked as absent
                bool isAbsent = rbtnAbsent.Checked;

                // Initialize hours variables
                decimal hoursWorked = 0;
                decimal overtimeHours = 0;
                decimal totalPayment = 0;

                // If not absent, calculate hours from start and end times
                if (!isAbsent)
                {
                    // Get time values from the time fields
                    TimeSpan startTime, endTime;
                    if (!TimeSpan.TryParse(txtStartTime.Text, out startTime) || !TimeSpan.TryParse(txtEndTime.Text, out endTime))
                    {
                        lblMessage.Text = "Please enter valid start and end times.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Calculate hours worked
                    double totalHours;

                    // If end time is earlier than start time, assume it spans to the next day
                    if (endTime < startTime)
                    {
                        totalHours = (24 - startTime.TotalHours) + endTime.TotalHours;
                    }
                    else
                    {
                        totalHours = endTime.TotalHours - startTime.TotalHours;
                    }

                    // Round to nearest half hour
                    totalHours = Math.Round(totalHours * 2) / 2;

                    // Validate calculated hours
                    if (totalHours <= 0 || totalHours > 24)
                    {
                        lblMessage.Text = "Total hours must be positive and cannot exceed 24 hours.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Calculate regular hours (capped at 8) and overtime
                    hoursWorked = Math.Min(8m, (decimal)totalHours);
                    overtimeHours = Math.Max(0m, (decimal)totalHours - 8m);

                    // Round to 1 decimal place
                    hoursWorked = Math.Round(hoursWorked, 1);
                    overtimeHours = Math.Round(overtimeHours, 1);

                    // Get payment from hidden field (calculated by JavaScript)
                    if (!string.IsNullOrEmpty(hiddenTotalPayment.Value) && decimal.TryParse(hiddenTotalPayment.Value, out totalPayment))
                    {
                        // Successfully parsed the total payment
                    }
                    else
                    {
                        // Calculate payment directly if JavaScript calculation failed
                        totalPayment = CalculateTotalPayment(int.Parse(ddlTechnician.SelectedValue), hoursWorked, overtimeHours);
                    }
                }
                else
                {
                    // When absent, explicitly set hours and payment to 0
                    hoursWorked = 0;
                    overtimeHours = 0;
                    totalPayment = 0;
                }

                // Check for duplicate attendance record
                if (IsAttendanceRecordExists(int.Parse(ddlTechnician.SelectedValue), date))
                {
                    lblMessage.Text = "Attendance record for this technician on this date already exists.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Debug output - log the values being saved
                System.Diagnostics.Debug.WriteLine($"Saving: Hours={hoursWorked}, Overtime={overtimeHours}, Absent={isAbsent}, Payment={totalPayment}");

                // Insert attendance record into the database
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Attendance (TechnicianId, ProjectId, Date, HoursWorked, OvertimeHours, TotalPayment, SubmittedBy, Notes, IsAbsent) VALUES (@TechnicianId, @ProjectId, @Date, @HoursWorked, @OvertimeHours, @TotalPayment, @SubmittedBy, @Notes, @IsAbsent)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TechnicianId", int.Parse(ddlTechnician.SelectedValue));
                        command.Parameters.AddWithValue("@ProjectId", int.Parse(ddlProject.SelectedValue));
                        command.Parameters.AddWithValue("@Date", date);
                        // Always save the calculated hours, don't use DBNull even when absent
                        command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                        command.Parameters.AddWithValue("@OvertimeHours", overtimeHours);
                        command.Parameters.AddWithValue("@TotalPayment", totalPayment);
                        command.Parameters.AddWithValue("@SubmittedBy", GetCurrentUserId()); // Get the current user's ID
                        command.Parameters.AddWithValue("@Notes", txtNotes.Text);
                        command.Parameters.AddWithValue("@IsAbsent", isAbsent ? 1 : 0); // Store absence status
                        command.ExecuteNonQuery();
                    }
                }

                // Success message with appropriate styling
                lblMessage.Text = isAbsent ? "Attendance recorded as absent successfully." : "Attendance recorded successfully.";
                lblMessage.CssClass = "ms-3 text-success";

                // Reset the form for a new entry
                ResetForm();
            }
            catch (Exception ex)
            {
                // Handle unexpected errors with more details
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "ms-3 text-danger";
                System.Diagnostics.Debug.WriteLine("Exception details: " + ex.ToString());
            }
        }

        private decimal CalculateTotalPayment(int technicianId, decimal regularHours, decimal overtimeHours)
        {
            decimal hourlyRate = 0;
            decimal overtimeRate = 0;

            // Get technician rates from database
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT HourlyRate, OvertimeRate FROM Technician WHERE TechnicianID = @TechnicianId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TechnicianId", technicianId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hourlyRate = reader.IsDBNull(reader.GetOrdinal("HourlyRate")) ? 0 :
                                Convert.ToDecimal(reader["HourlyRate"]);
                            overtimeRate = reader.IsDBNull(reader.GetOrdinal("OvertimeRate")) ? 0 :
                                Convert.ToDecimal(reader["OvertimeRate"]);
                        }
                    }
                }
            }

            // Calculate total payment
            decimal totalPayment = (regularHours * hourlyRate) + (overtimeHours * overtimeRate);
            return Math.Round(totalPayment, 2);
        }

        private void ResetForm()
        {
            // Reset form fields except for the technician and project dropdowns
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtStartTime.Text = "09:00";
            txtEndTime.Text = "17:00";

            // Re-enable time inputs and recalculate hours
            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetForm",
                "toggleTimeInputs(); calculateHours();", true);

            rbtnPositive.Checked = true;
            rbtnAbsent.Checked = false;
            txtNotes.Text = "";
        }

        private bool IsAttendanceRecordExists(int technicianId, DateTime date)
        {
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Attendance WHERE TechnicianId = @TechnicianId AND Date = @Date";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TechnicianId", technicianId);
                    command.Parameters.AddWithValue("@Date", date.Date); // Use only the date part for comparison
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0; // Return true if a record exists
                }
            }
        }

        private int GetCurrentUserId()
        {
            // Check if the session variable exists
            if (Session["UserID"] != null)
            {
                // Return the user ID stored in the session
                return (int)Session["UserID"];
            }
            else
            {
                Response.Redirect("~/Views/Account/Login.aspx");
                return -1; // This line won't execute due to the redirect
            }
        }
    }
}