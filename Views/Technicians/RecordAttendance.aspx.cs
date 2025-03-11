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
                    while (reader.Read())
                    {
                        ddlProject.Items.Add(new ListItem(reader["ProjectName"].ToString(), reader["ProjectId"].ToString()));
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            

            // Validate that a technician and project are selected
            if (ddlTechnician.SelectedValue == "" || ddlProject.SelectedValue == "")
            {
                lblMessage.Text = "Please select a technician and a project.";
                return;
            }

            // Validate the date
            DateTime date;
            if (!DateTime.TryParse(txtDate.Text, out date) || date > DateTime.Now)
            {
                lblMessage.Text = "Please enter a valid date that is not in the future.";
                return;
            }
            // Check if the technician is marked as absent
            bool isAbsent = rbtnAbsent.Checked; 

            // Initialize hours variables
            decimal hoursWorked = 0;
            decimal overtimeHours = 0;

            // If not absent, validate hours worked
            if (!isAbsent)
            {
                if (!decimal.TryParse(txtHoursWorked.Text, out hoursWorked) || hoursWorked < 0 || hoursWorked > 24)
                {
                    lblMessage.Text = "Hours worked must be a positive number and cannot exceed 24.";
                    return;
                }

                if (!string.IsNullOrEmpty(txtOvertimeHours.Text) &&
                    (!decimal.TryParse(txtOvertimeHours.Text, out overtimeHours) || overtimeHours < 0 || overtimeHours > 24))
                {
                    lblMessage.Text = "Overtime hours must be a positive number and cannot exceed 24.";
                    return;
                }

                // Validate total hours
                if (hoursWorked + overtimeHours > 24)
                {
                    lblMessage.Text = "The total of hours worked and overtime hours cannot exceed 24.";
                    return;
                }
            }

            // Check for duplicate attendance record
            if (IsAttendanceRecordExists(int.Parse(ddlTechnician.SelectedValue), date))
            {
                lblMessage.Text = "Attendance record for this technician on this date already exists.";
                return;
            }

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
                        command.Parameters.AddWithValue("@HoursWorked", isAbsent ? (object)DBNull.Value : hoursWorked); // Set to DBNull if absent
                        command.Parameters.AddWithValue("@OvertimeHours", isAbsent ? (object)DBNull.Value : overtimeHours); // Set to DBNull if absent
                        command.Parameters.AddWithValue("@SubmittedBy", GetCurrentUserId()); // Get the current user's ID
                        command.Parameters.AddWithValue("@Notes", txtNotes.Text);
                        command.Parameters.AddWithValue("@IsAbsent", isAbsent ? 1 : 0); // Store absence status
                        command.ExecuteNonQuery();
                    }
            }
            lblMessage.Text = isAbsent ? "Attendance recorded as absent successfully." : "Attendance recorded successfully.";
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
                    command.Parameters.AddWithValue("@Date", date);
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
                // Handle the case where the user is not logged in
                throw new Exception("User is not logged in.");
            }
        }
    }
}         
       