using System;
using ProjectManagementSystem.Controllers;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Web.UI;
using ProjectManagementSystem.Models;
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

                // Validate the date
                DateTime date;
                if (!DateTime.TryParse(txtDate.Text, out date) || date > DateTime.Now)
                {
                    lblMessage.Text = "Please enter a valid date that is not in the future.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Check if the technician is marked as absent
                bool isAbsent = rbtnAbsent.Checked;

                // Initialize hours variables
                decimal hoursWorked = 0;
                decimal overtimeHours = 0;

                // If not absent, get the calculated hours from the hidden field and text boxes
                if (!isAbsent)
                {
                    // Try to get regular hours from the text box
                    if (!decimal.TryParse(txtHoursWorked.Text.Trim(), out hoursWorked))
                    {
                        lblMessage.Text = "Hours worked must be a valid number.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Try to get overtime hours from the text box
                    if (!decimal.TryParse(txtOvertimeHours.Text.Trim(), out overtimeHours))
                    {
                        lblMessage.Text = "Overtime hours must be a valid number.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Validate hours worked
                    if (hoursWorked < 0)
                    {
                        lblMessage.Text = "Hours worked must be a positive number.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Validate overtime hours
                    if (overtimeHours < 0)
                    {
                        lblMessage.Text = "Overtime hours must be a positive number.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Check total hours from hidden field for extra validation
                    decimal totalHours;
                    if (!decimal.TryParse(hiddenTotalHours.Value, out totalHours) || totalHours <= 0 || totalHours > 24)
                    {
                        lblMessage.Text = "Total hours must be positive and cannot exceed 24 hours.";
                        lblMessage.CssClass = "ms-3 text-danger";
                        return;
                    }

                    // Make sure the sum of hours worked and overtime matches the total
                    if (Math.Abs((hoursWorked + overtimeHours) - totalHours) > 0.01m)
                    {
                        // If there's a discrepancy, use the values from the total hours calculation
                        // This ensures the JavaScript calculation takes precedence
                        hoursWorked = Math.Min(totalHours, 8m);
                        overtimeHours = Math.Max(0m, totalHours - 8m);

                        // Round to 2 decimal places
                        hoursWorked = Math.Round(hoursWorked, 2);
                        overtimeHours = Math.Round(overtimeHours, 2);
                    }
                }
                else
                {
                    // When absent, explicitly set hours to 0
                    hoursWorked = 0;
                    overtimeHours = 0;
                }

                // Check for duplicate attendance record
                if (IsAttendanceRecordExists(int.Parse(ddlTechnician.SelectedValue), date))
                {
                    lblMessage.Text = "Attendance record for this technician on this date already exists.";
                    lblMessage.CssClass = "ms-3 text-danger";
                    return;
                }

                // Debug output - log the values being saved
                System.Diagnostics.Debug.WriteLine($"Saving: Hours={hoursWorked}, Overtime={overtimeHours}, Absent={isAbsent}");

                // Insert attendance record into the database
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Attendance (TechnicianId, ProjectId, Date, HoursWorked, OvertimeHours, SubmittedBy, Notes, IsAbsent) VALUES (@TechnicianId, @ProjectId, @Date, @HoursWorked, @OvertimeHours, @SubmittedBy, @Notes, @IsAbsent)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TechnicianId", int.Parse(ddlTechnician.SelectedValue));
                        command.Parameters.AddWithValue("@ProjectId", int.Parse(ddlProject.SelectedValue));
                        command.Parameters.AddWithValue("@Date", date);
                        // Always save the calculated hours, don't use DBNull even when absent
                        command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                        command.Parameters.AddWithValue("@OvertimeHours", overtimeHours);
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

        private void ResetForm()
        {
            // Reset form fields except for the technician and project dropdowns
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtStartTime.Text = "09:00";
            txtEndTime.Text = "17:00";
            txtHoursWorked.Text = "8";
            txtOvertimeHours.Text = "0";
            hiddenTotalHours.Value = "8";
            rbtnPositive.Checked = true;
            rbtnAbsent.Checked = false;
            txtNotes.Text = "";

            // Re-enable time inputs
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toggleTimeInputs", "toggleTimeInputs();", true);
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