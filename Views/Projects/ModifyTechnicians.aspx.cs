using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class ModifyTechnicians : System.Web.UI.Page
    {
        private int projectId;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize the projectId from the query string
            if (Request.QueryString["projectId"] != null)
            {
                if (!int.TryParse(Request.QueryString["projectId"], out projectId))
                {
                    // Handle invalid ID (redirect or show error)
                    Response.Redirect("~/Views/Projects/Projects.aspx");
                    return;
                }
            }
            else
            {
                // No project ID provided, redirect back
                Response.Redirect("~/Views/Projects/Projects.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadTechnicians();
                LoadProjectDetails();
            }
        }
        protected void LoadProjectDetails()
        {
            // Load project name
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ProjectName FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    lblProjectName.Text = cmd.ExecuteScalar()?.ToString();
                }
            }
        }
        private void LoadTechnicians()
        {
            // Load all available technicians
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianID, Username FROM Technician";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlTechnicians.Items.Clear();
                    while (reader.Read())
                    {
                        ddlTechnicians.Items.Add(new ListItem(reader["Username"].ToString(), reader["TechnicianID"].ToString()));
                    }
                }
            }
        }
        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Get selected technician ID
            string selectedTechnicianId = ddlTechnicians.SelectedValue;
            
            if (string.IsNullOrEmpty(selectedTechnicianId))
            {
                lblMessage.Text = "Please select a technician.";
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();

                // Check if a senior technician is already assigned
                bool seniorTechnicianExists = false;               
                // Check if a senior technician is already assigned
                string checkSeniorQuery = "SELECT COUNT(*) FROM ProjectTechnicians WHERE ProjectId = @ProjectId AND IsSenior = 1";                   
                using (var checkCommand = new SQLiteCommand(checkSeniorQuery, connection))
                {
                        checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                        seniorTechnicianExists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                }

                // If the technician is senior and one already exists, show an error
                if (IsSeniorTechnician(selectedTechnicianId) && seniorTechnicianExists)
                {
                    lblMessage.Text = "Only one senior technician can be assigned to a project.";
                    return;
                }

                // Add selected technicians to the project
                string insertTechnicianQuery = "INSERT INTO ProjectTechnicians (ProjectId, TechnicianId, IsSenior) VALUES (@ProjectId, @TechnicianId, @IsSenior)";
                using (var command = new SQLiteCommand(insertTechnicianQuery, connection))
                {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        command.Parameters.AddWithValue("@TechnicianId", selectedTechnicianId);
                        command.Parameters.AddWithValue("@IsSenior", IsSeniorTechnician(selectedTechnicianId));
                        command.ExecuteNonQuery();
                }
                
            }

            lblMessage.Text = "Technicians added successfully!";
        }
        private bool IsSeniorTechnician(string technicianId)
        {
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianLevel FROM Technician WHERE TechnicianID = @TechnicianId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TechnicianId", technicianId);
                    var result = command.ExecuteScalar();
                    return result != null && result.ToString() == "Senior";
                }
            }
        }
    }
}
