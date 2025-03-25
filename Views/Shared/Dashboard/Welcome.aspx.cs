using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Shared.Dashboard
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Views/Shared/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SetupDashboard();
                LoadAssignedProjects();
                LoadRecentActivities();
            }
        }

        protected bool IsUserTechnician()
        {
            return Session["UserRole"]?.ToString().Equals("Technician", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        protected bool IsProjectManager()
        {
            return Session["UserRole"]?.ToString().Equals("ProjectManager", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        protected bool IsAdmin()
        {
            return Session["UserRole"]?.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        private void SetupDashboard()
        {
            string userRole = Session["UserRole"]?.ToString() ?? "User";
            string username = Session["Username"]?.ToString() ?? "User";

            lblUsername.Text = username;
            lblRole.Text = userRole;

            // Set user initial for profile circle
            ltUserInitial.Text = !string.IsNullOrEmpty(username)
                ? username.Substring(0, 1).ToUpper()
                : "U";

            // Set role-specific welcome message
            switch (userRole.ToLower())
            {
                case "technician":
                    lblRoleSpecificMessage.Text = "Welcome! Access your work logs and payment information";
                    break;
                case "projectmanager":
                    lblRoleSpecificMessage.Text = "Welcome! Manage projects and team resources";
                    break;
                case "admin":
                    lblRoleSpecificMessage.Text = "Welcome! Monitor system performance and manage budgets";
                    break;
                default:
                    lblRoleSpecificMessage.Text = "Welcome to the Project Management System";
                    break;
            }

            // Configure UI elements based on role
            ConfigureRoleBasedUI(userRole);
        }

        private void ConfigureRoleBasedUI(string role)
        {
            // Show/hide quick action panels based on role
            pnlAdminQuickActions.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            pnlProjectManagerQuickActions.Visible = role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase);
            pnlTechnicianQuickActions.Visible = role.Equals("Technician", StringComparison.OrdinalIgnoreCase);

            // Show assigned projects for PM and Technician
            pnlAssignedProjects.Visible = role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase) ||
                                         role.Equals("Technician", StringComparison.OrdinalIgnoreCase);

            // Configure sidebar item visibility
            btnAttendance.Visible = role.Equals("Technician", StringComparison.OrdinalIgnoreCase);
            btnPaymentInfo.Visible = role.Equals("Technician", StringComparison.OrdinalIgnoreCase);
            btnAssignTechnicians.Visible = role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase);
            btnReports.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnCreateProject.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnCreateUser.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void LoadAssignedProjects()
        {
            string userRole = Session["UserRole"]?.ToString() ?? "";
            int userId = Convert.ToInt32(Session["UserId"]);

            if (userRole.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase) ||
                userRole.Equals("Technician", StringComparison.OrdinalIgnoreCase))
            {
                // Get projects assigned to the current user
                List<Project> projects = GetAssignedProjects(userId, userRole);

                if (projects.Count > 0)
                {
                    rptAssignedProjects.DataSource = projects;
                    rptAssignedProjects.DataBind();
                    lblNoProjects.Visible = false;
                }
                else
                {
                    rptAssignedProjects.Visible = false;
                    lblNoProjects.Visible = true;
                }
            }
        }

        private List<Project> GetAssignedProjects(int userId, string role)
        {
            List<Project> projects = new List<Project>();

            string query = "";
            SQLiteParameter[] parameters;

            if (role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase))
            {
                // Query for projects managed by this project manager
                query = @"
            SELECT 
                p.ProjectId,
                p.ProjectName,
                p.Status,
                p.EndDate AS DueDate,
                COUNT(t.TaskId) AS TaskCount,
                SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
            FROM Projects p
            LEFT JOIN Tasks t ON p.ProjectId = t.ProjectId
            WHERE p.ProjectManagerId = @UserId
            GROUP BY p.ProjectId, p.ProjectName, p.Status, p.EndDate
            ORDER BY p.EndDate ASC";

                parameters = new SQLiteParameter[] {
            new SQLiteParameter("@UserId", userId)
        };
            }
            else if (role.Equals("Technician", StringComparison.OrdinalIgnoreCase))
            {
                // Query for projects where this technician has assigned tasks
                query = @"
            SELECT DISTINCT
                p.ProjectId,
                p.ProjectName,
                p.Status,
                p.EndDate AS DueDate,
                COUNT(t.TaskId) AS TaskCount,
                SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
            FROM Projects p
            JOIN Tasks t ON p.ProjectId = t.ProjectId
            WHERE t.AssignedTo = @UserId
            GROUP BY p.ProjectId, p.ProjectName, p.Status, p.EndDate
            ORDER BY p.EndDate ASC";

                parameters = new SQLiteParameter[] {
            new SQLiteParameter("@UserId", userId)
        };
            }
            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admins see all active projects
                query = @"
            SELECT 
                p.ProjectId,
                p.ProjectName,
                p.Status,
                p.EndDate AS DueDate,
                COUNT(t.TaskId) AS TaskCount,
                SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
            FROM Projects p
            LEFT JOIN Tasks t ON p.ProjectId = t.ProjectId
            WHERE p.Status = 'Active'
            GROUP BY p.ProjectId, p.ProjectName, p.Status, p.EndDate
            ORDER BY p.EndDate ASC";

                parameters = new SQLiteParameter[0];
            }
            else
            {
                // Return empty list for other roles
                return projects;
            }

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(new Project
                            {
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                Status = reader["Status"].ToString(),
                                DueDate = Convert.ToDateTime(reader["DueDate"]),
                                TaskCount = Convert.ToInt32(reader["TaskCount"]),
                                CompletedTasks = Convert.ToInt32(reader["CompletedTasks"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Trace.TraceError($"Error fetching projects: {ex.Message}");
                    throw; // Or handle gracefully depending on your requirements
                }
            }

            return projects;
        }

        private void LoadRecentActivities()
        {
            try
            {
                // Get the current user's ID from session
                int userId = Convert.ToInt32(Session["UserId"]);

                // Get activities from database for this user
                List<Activity> activities = GetUserActivitiesFromDatabase(userId);

                if (activities.Count > 0)
                {
                    rptRecentActivities.DataSource = activities;
                    rptRecentActivities.DataBind();
                }
                else
                {
                    // Show message if no activities found
                    rptRecentActivities.DataSource = new List<Activity> {
                new Activity {
                    ActivityType = "info",
                    Description = "No recent activities found",
                    Timestamp = DateTime.Now
                }
            };
                    rptRecentActivities.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Log error and show friendly message
                System.Diagnostics.Trace.TraceError($"Error loading activities: {ex.Message}");

                rptRecentActivities.DataSource = new List<Activity> {
            new Activity {
                ActivityType = "error",
                Description = "Could not load activities. Please try again later.",
                Timestamp = DateTime.Now
            }
        };
                rptRecentActivities.DataBind();
            }
        }

        private List<Activity> GetUserActivitiesFromDatabase(int userId)
        {
            List<Activity> activities = new List<Activity>();

            // SQL query to get activities for the user
            string query = @"
        SELECT
            a.ActivityType, 
            a.Description, 
            a.Timestamp,
            p.ProjectName
        FROM UserActivities ua
        JOIN Activities a ON ua.ActivityId = a.ActivityId
        LEFT JOIN Projects p ON a.ProjectId = p.ProjectId
        WHERE ua.UserId = @UserId
        ORDER BY a.Timestamp DESC
        LIMIT 10"; // Get last 10 activities

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.Add(new SQLiteParameter("@UserId", userId));

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = reader["Description"].ToString();

                            // If there's a project name, include it in the description
                            if (!reader.IsDBNull(reader.GetOrdinal("ProjectName")))
                            {
                                description += $" (Project: {reader["ProjectName"]})";
                            }

                            activities.Add(new Activity
                            {
                                ActivityType = reader["ActivityType"].ToString(),
                                Description = description,
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log database error
                    System.Diagnostics.Trace.TraceError($"Database error: {ex.Message}");
                    throw; // Re-throw to be caught by calling method
                }
            }

            return activities;
        }
        public string GetStatusClass(string status)
        {
            string statusClass = "";
            switch (status.ToLower())
            {
                case "active":
                    statusClass = "status-active";
                    break;
                case "pending":
                    statusClass = "status-pending";
                    break;
                case "completed":
                    statusClass = "status-completed";
                    break;
            }
            return statusClass;
        }

        public string GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "active":
                    return "status-active";
                case "pending":
                    return "status-pending";
                case "completed":
                    return "status-completed";
                default:
                    return "primary";
            }
        }


        public string GetActivityIcon(string activityType)
        {
            string iconClass = "fa-bell"; // Default icon
            switch (activityType.ToLower())
            {
                case "project":
                    iconClass = "fa-project-diagram";
                    break;
                case "task":
                    iconClass = "fa-tasks";
                    break;
                case "report":
                    iconClass = "fa-chart-pie";
                    break;
            }
            return iconClass;
        }


        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/Shared/Login.aspx");
        }

        protected void btnAttendance_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/RecordAttendance.aspx");
        }

        protected void btnPaymentInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/PaymentInfo.aspx");
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/CreateProject.aspx");
        }

        protected void btnAssignTechnicians_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ProjectManagers/ViewTechnicians.aspx");
        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Admin/Reports.aspx");
        }

        protected void btnProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }

        protected void btnResources_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Resources/Resources.aspx");
        }

        protected void btnAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Account/Profile.aspx");
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Controllers/UserController.aspx");
        }

        protected void btnNewProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/CreateProject.aspx");
        }

        protected void btnAssignTask_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/CreateProject.aspx");
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Admin/Reports.aspx");
        }

        protected void btnProjectStatus_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/ProjectStatus.aspx");
        }

        protected void btnRecordAttendance_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/RecordAttendance.aspx");
        }

        protected void btnViewTasks_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/MyTasks.aspx");
        }
    }

    public class Activity
    {
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}