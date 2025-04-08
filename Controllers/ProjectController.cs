using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController
    {
        // Changed to static to ensure persistence between page loads
        private List<Project> _projects;
        private readonly string _connectionString;    


        public ProjectController()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteDB"].ConnectionString;

            _projects = HttpContext.Current.Session["Projects"] as List<Project>;

            if (_projects == null)
            {
                InitializeSampleProjects();
                HttpContext.Current.Session["Projects"] = _projects;
            }
        }

        public void InitializeSampleProjects()
        {
            _projects = new List<Project>
            {
                new Project
                {
                    ProjectId = 1,
                    ProjectName = "Project Alpha: Karen SmartHome",
                    Budget = 100000,
                    Status = "In Progress",
                    Location = "Karen",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddMonths(3),
                    Description = "Complete installation for Faith's smarthome mansion.",
                    BudgetRangeMin = 20000,
                    BudgetRangeMax = 120000,
                    TotalResourceCost = 24000,
                    TechnicianPayment = 54000,
                },
                new Project
                {
                    ProjectId = 2,
                    ProjectName = "Project Beta: CCTV Installation",
                    Budget = 200000,
                    Status = "Completed",
                    Location = "Runda",
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now.AddDays(-5),
                    Description = "Installing for Faith cameras at her rec center.",
                    BudgetRangeMin = 20000,
                    BudgetRangeMax = 240000,
                    TotalResourceCost = 90000,
                    TechnicianPayment = 110000,
                }
            };
        }

        public List<Project> GetProjects()
        {
            return _projects;
        }

        public Project GetProjectById(int projectId)
        {
            Project project = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Projects WHERE ProjectId = @ProjectId", connection);
                command.Parameters.AddWithValue("@ProjectId", projectId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    project = new Project
                    {
                        ProjectId = Convert.ToInt32(reader["ProjectId"]),
                        ProjectName = reader["ProjectName"].ToString(),
                        Description = reader["Description"].ToString(),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : (DateTime?)null,
                        Budget = Convert.ToDecimal(reader["Budget"]),
                        Status = reader["Status"].ToString()
                    };
                }
                reader.Close();
            }
            return project;
        }

        public bool IsTechnicianAssignedToProject(int userId, int projectId)
        {
            // Check if the technician is assigned to the project in the database
            // For now, let's assume we return true as a placeholder
            return true;
        }

        public double GetTechnicianHourlyRate(int userId)
        {
            // Fetch the technician's hourly rate from the database
            // For now, let's assume we return the following as a placeholder
            return 230.50;
        }

        public void UpdateProjectBudget(int projectId, decimal newBudget)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "UPDATE Projects SET Budget = @Budget, LastModified = @LastModified WHERE ProjectId = @ProjectId",
                    connection);

                command.Parameters.AddWithValue("@Budget", newBudget);
                command.Parameters.AddWithValue("@LastModified", DateTime.Now);
                command.Parameters.AddWithValue("@ProjectId", projectId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // New method to get the most recent project (fallback if no ID provided)
        public Project GetLatestProject()
        {
            Project project = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Get the most recently added or modified project
                SqlCommand command = new SqlCommand(
                    "SELECT TOP 1 * FROM Projects ORDER BY LastModified DESC, ProjectId DESC",
                    connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    project = new Project
                    {
                        ProjectId = Convert.ToInt32(reader["ProjectId"]),
                        ProjectName = reader["ProjectName"].ToString(),
                        Description = reader["Description"].ToString(),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = reader["EndDate"] != DBNull.Value ? Convert.ToDateTime(reader["EndDate"]) : (DateTime?)null,
                        Budget = Convert.ToDecimal(reader["Budget"]),
                        Status = reader["Status"].ToString()
                    };
                }
                reader.Close();
            }
            return project;
        }

        public bool CanModifyBudget(string userRole)
        {
            return userRole == "Admin";
        }
    }
}