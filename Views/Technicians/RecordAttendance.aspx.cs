using System;
using ProjectManagementSystem.Controllers;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Web.UI;
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
            int technicianId = int.Parse(ddlTechnician.SelectedValue);
            int ProjectId = int.Parse(ddlProject.SelectedValue);
            DateTime date = DateTime.Parse(txtDate.Text);
            decimal hoursWorked = decimal.Parse(txtHoursWorked.Text);
            decimal overtimeHours = string.IsNullOrEmpty(txtOvertimeHours.Text) ? 0 : decimal.Parse(txtOvertimeHours.Text);
            string notes = txtNotes.Text;

            // Insert attendance record into the database
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Attendance (TechnicianId, ProjectId, Date, HoursWorked, OvertimeHours, SubmittedBy, Notes) VALUES (@TechnicianId, @ProjectId, @Date, @HoursWorked, @OvertimeHours, @SubmittedBy, @Notes)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@TechnicianId", technicianId);
                    command.Parameters.AddWithValue("@ProjectId", ProjectId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                    command.Parameters.AddWithValue("@OvertimeHours", overtimeHours);
                    command.Parameters.AddWithValue("@SubmittedBy", GetCurrentUserId()); //  get the current user's ID
                    command.Parameters.AddWithValue("@Notes", notes);

                    command.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Attendance recorded successfully!";
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
       