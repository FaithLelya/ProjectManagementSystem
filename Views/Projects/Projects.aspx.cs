using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;
using System.Data.SQLite;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class Projects : System.Web.UI.Page
    {
        private List<Project> _projects;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if there's a completion message in the session
                if (Session["CompletionMessage"] != null)
                {
                    CompletionSuccessPanel.Visible = true;
                    CompletionMessage.Text = Session["CompletionMessage"].ToString();
                    Session.Remove("CompletionMessage"); // Clear the message
                }

                LoadProjects();
            }
        }

        private void LoadProjects()
        {
            var userRole = Session["UserRole"]?.ToString();

            var projects = new List<Project>();
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Projects";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                ProjectId = reader.GetInt32(0),
                                ProjectName = reader.GetString(1),
                                Description = reader.GetString(2),
                                Location = reader.GetString(3),
                                StartDate = DateTime.Parse(reader.GetString(4)),
                                EndDate = DateTime.Parse(reader.GetString(5)),
                                TechnicianPayment = reader.GetDecimal(6),
                                MaterialsCost = reader.GetDecimal(7),
                                Budget = reader.GetDecimal(8),
                                ProjectManagerId = reader.GetInt32(9),
                                Resources = reader.GetString(10),
                                Status = reader.GetString(11),
                                TotalExpense = reader.GetDecimal(12),
                                AssignedTechnicians = new List<Technician>(),
                                AllocatedResources = new List<Resource>()
                            };

                            LoadProjectTechnicians(conn, project);
                            LoadProjectResources(conn, project);
                            CalculateTotalResourceCost(project);

                            projects.Add(project);
                        }
                    }
                }
            }

            ProjectRepeater.DataSource = projects;
            ProjectRepeater.DataBind();
        }

        // Helper method to check if project end date has passed
        protected bool IsProjectPastDue(DateTime endDate)
        {
            return endDate < DateTime.Today;
        }

        private void LoadProjectTechnicians(SQLiteConnection conn, Project project)
        {
            string sql = @"SELECT t.TechnicianID, t.UserName, pt.IsSenior 
                           FROM ProjectTechnicians pt 
                           JOIN Technician t ON pt.TechnicianId = t.TechnicianID 
                           WHERE pt.ProjectId = @ProjectId";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        project.AssignedTechnicians.Add(new Models.Technician
                        {
                            TechnicianId = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            IsSenior = reader.GetInt32(2) == 1
                        });
                    }
                }
            }
        }

        private void LoadProjectResources(SQLiteConnection conn, Project project)
        {
            // Updated SQL query to include CostPerUnit from the Resources table
            string sql = @"SELECT pr.ResourceId, r.ResourceName, pr.Quantity, r.CostPerUnit 
                           FROM ProjectResources pr 
                           JOIN Resources r ON pr.ResourceId = r.ResourceId 
                           WHERE pr.ProjectId = @ProjectId";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        project.AllocatedResources.Add(new Resource
                        {
                            ResourceId = reader.GetInt32(0),
                            ResourceName = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                            CostPerunit = reader.GetDecimal(3)
                        });
                    }
                }
            }
        }

        // New method to calculate the total resource cost
        private void CalculateTotalResourceCost(Project project)
        {
            decimal totalResourceCost = 0;

            foreach (var resource in project.AllocatedResources)
            {
                totalResourceCost += resource.Quantity * resource.CostPerunit;
            }

            project.TotalResourceCost = totalResourceCost;

            // Update the total expense to include the calculated resource cost
            project.TotalExpense = project.TechnicianPayment + project.MaterialsCost + totalResourceCost;
        }
        protected void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"]?.ToString() != "Admin")
                return;

            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            string projectName = string.Empty;

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Get project name first for confirmation
                string nameSql = "SELECT ProjectName FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand nameCmd = new SQLiteCommand(nameSql, conn))
                {
                    nameCmd.Parameters.AddWithValue("@ProjectId", projectId);
                    projectName = (string)nameCmd.ExecuteScalar();
                }

                // First delete related records
                string deleteTechSql = "DELETE FROM ProjectTechnicians WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(deleteTechSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.ExecuteNonQuery();
                }

                string deleteResSql = "DELETE FROM ProjectResources WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(deleteResSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.ExecuteNonQuery();
                }

                // Then delete the project
                string deleteProjSql = "DELETE FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(deleteProjSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.ExecuteNonQuery();
                }

                // Log the deletion
                string logSql = @"INSERT INTO ProjectLogs (ProjectId, ActionType, ActionBy, ActionDate, Details)
                          VALUES (@ProjectId, 'Deletion', @UserId, @Date, 'Project deleted')";
                using (SQLiteCommand logCmd = new SQLiteCommand(logSql, conn))
                {
                    logCmd.Parameters.AddWithValue("@ProjectId", projectId);
                    logCmd.Parameters.AddWithValue("@UserId", Session["UserId"] ?? DBNull.Value);
                    logCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    logCmd.ExecuteNonQuery();
                }
            }

            // Refresh the page
            Response.Redirect(Request.RawUrl);
        }


        // New method to mark a project as completed
        protected void btnConfirmCompletion_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            string projectName = string.Empty;

            // Update the project status to "Completed" in the database
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Get project name first for messaging
                string nameSql = "SELECT ProjectName FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand nameCmd = new SQLiteCommand(nameSql, conn))
                {
                    nameCmd.Parameters.AddWithValue("@ProjectId", projectId);
                    projectName = (string)nameCmd.ExecuteScalar();
                }

                // Update project status
                string updateSql = "UPDATE Projects SET Status = 'Completed' WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(updateSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    cmd.ExecuteNonQuery();
                }

                // Log the completion (optional)
                string logSql = @"INSERT INTO ProjectLogs (ProjectId, ActionType, ActionBy, ActionDate, Details)
                                  VALUES (@ProjectId, 'Status Change', @UserId, @Date, 'Project marked as completed')";
                using (SQLiteCommand logCmd = new SQLiteCommand(logSql, conn))
                {
                    logCmd.Parameters.AddWithValue("@ProjectId", projectId);
                    logCmd.Parameters.AddWithValue("@UserId", Session["UserId"] ?? DBNull.Value);
                    logCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    try
                    {
                        logCmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        // If logging fails, we still want to continue with the process
                        // Consider implementing proper error handling here
                    }
                }
            }

            // Set a success message and redirect to refresh the page
            Session["CompletionMessage"] = $"Project '{projectName}' has been successfully marked as completed.";
            Response.Redirect(Request.RawUrl);
        }

        public bool IsProjectCompleted(string status)
        {
            return status == "Completed";
        }


        // Role-Based Access Control
        public bool CanViewFinancials()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Admin" || userRole == "ProjectManager";
        }

        public bool CanModifyResources()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "ProjectManager"; // Only Project Managers can modify resources
        }

        public bool CanModifyTechnicians()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "ProjectManager" ; // Only Project Managers can modify technicians
        }

        public bool CanModifyBudget()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Admin"; // Only Admins can modify budget
        }

        protected void btnModifyResources_Click(object sender, EventArgs e)
        {
            if (!CanModifyResources() || IsProjectCompleted(GetProjectStatus(Convert.ToInt32(((Button)sender).CommandArgument))))
                return;

            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyResources.aspx?projectId={projectId}");
        }

        protected void btnModifyTechnicians_Click(object sender, EventArgs e)
        {
            if (!CanModifyTechnicians() || IsProjectCompleted(GetProjectStatus(Convert.ToInt32(((Button)sender).CommandArgument))))
                return;

            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyTechnicians.aspx?projectId={projectId}");
        }

        protected void btnModifyBudget_Click(object sender, EventArgs e)
        {
            if (!CanModifyBudget() || IsProjectCompleted(GetProjectStatus(Convert.ToInt32(((Button)sender).CommandArgument))))
                return;

            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyBudget.aspx?projectId={projectId}");
        }

        // Helper method to get project status
        private string GetProjectStatus(int projectId)
        {
            string status = string.Empty;
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Status FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    status = (string)cmd.ExecuteScalar();
                }
            }
            return status;
        }
    }
}