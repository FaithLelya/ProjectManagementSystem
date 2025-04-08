using System;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class ModifyTechnicians : System.Web.UI.Page
    {
        private int projectId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["projectId"] != null && int.TryParse(Request.QueryString["projectId"], out projectId))
            {
                if (!IsPostBack)
                {
                    LoadTechnicians();
                    LoadProjectDetails();
                }
            }
            else
            {
                Response.Redirect("~/Views/Projects/Projects.aspx");
            }
        }

        protected void LoadProjectDetails()
        {
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ProjectName FROM Projects WHERE ProjectId = @ProjectId";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    litProjectName.Text = cmd.ExecuteScalar()?.ToString();
                }
            }
        }

        private void LoadTechnicians()
        {
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianID, Username FROM Technician";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlTechnicians.Items.Clear();
                    ddlTechnicians.Items.Add(new ListItem("Select Technician", ""));
                    while (reader.Read())
                    {
                        ddlTechnicians.Items.Add(new ListItem(reader["Username"].ToString(), reader["TechnicianID"].ToString()));
                    }
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string selectedTechnicianId = ddlTechnicians.SelectedValue;

            if (string.IsNullOrEmpty(selectedTechnicianId))
            {
                lblMessage.Text = "Please select a technician.";
                lblMessage.CssClass = "alert alert-danger mt-3";
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();

                // Check if a senior technician is already assigned
                string checkSeniorQuery = "SELECT COUNT(*) FROM ProjectTechnicians WHERE ProjectId = @ProjectId AND IsSenior = 1";
                using (var checkCommand = new SQLiteCommand(checkSeniorQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    bool seniorTechnicianExists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

                    if (IsSeniorTechnician(selectedTechnicianId) && seniorTechnicianExists)
                    {
                        lblMessage.Text = "Only one senior technician can be assigned to a project.";
                        lblMessage.CssClass = "alert alert-warning mt-3";
                        return;
                    }
                }

                // Get the username from the selected item in the dropdown
                string selectedUsername = ddlTechnicians.SelectedItem.Text;

                // Assign technician
                string insertQuery = "INSERT INTO ProjectTechnicians (ProjectId, TechnicianId, IsSenior, Username) VALUES (@ProjectId, @TechnicianId, @IsSenior, @Username)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.Parameters.AddWithValue("@TechnicianId", selectedTechnicianId);
                    command.Parameters.AddWithValue("@IsSenior", IsSeniorTechnician(selectedTechnicianId));
                    command.Parameters.AddWithValue("@Username", selectedUsername);
                    command.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Technician assigned successfully!";
            lblMessage.CssClass = "alert alert-success mt-3";
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

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }
    }
}
