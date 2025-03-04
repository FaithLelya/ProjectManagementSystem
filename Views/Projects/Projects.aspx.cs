using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Helpers;
using System.Data.SQLite;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class Projects : System.Web.UI.Page
    {
        //private List<Project> _projects;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Projects"] == null)
            {
                // Initialize projects if not already done
                ProjectController controller = new ProjectController();
                controller.initializeSampleProjects(); // method to initialize sampleprojects
                Session["Projects"] = controller.GetProjects(); // store projects in session
            }

            LoadProjects();
        }

        private void LoadProjects()
        {
            var userRole = Session["UserRole"]?.ToString();

            // Retrieve the list of projects from the session
            //var projects = Session["Projects"] as List<Project>;

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
                                StartDate = DateTime.Parse(reader.GetString(4)), // Parse date from string
                                EndDate = DateTime.Parse(reader.GetString(5)),   // Parse date from string
                                TechnicianPayment = reader.GetDecimal(6),
                                MaterialsCost = reader.GetDecimal(7),
                                Budget = reader.GetDecimal(8),
                                ProjectManagerId = reader.GetInt32(9),
                                Resources = reader.GetString(10),
                                Status = reader.GetString(11),
                                TotalExpense = reader.GetDecimal(12) // Get total expense
                            };
                            projects.Add(project);
                        }
                    }
                }
            }
                
            // Bind the projects to a repeater or panel
            ProjectRepeater.DataSource = projects;
            ProjectRepeater.DataBind();

        }
       
        public bool CanViewFinancials()
        {
            string userRole = Session["User Role"]?.ToString();
            return userRole == "Admin" || userRole == "ProjectManager";
        }

        public bool CanModifyResources()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Technician" || userRole == "ProjectManager";
        }
        public bool CanModifyTechnicians()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "ProjectManager";
        }

        public bool CanModifyBudget()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Admin";
        }

        protected void btnModifyResources_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyResources.aspx?projectId={projectId}");
        }
        protected void btnModifyTechnicians_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyTechnicians.aspx?projectId={projectId}");
        }
        protected void btnModifyBudget_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"~/Views/Projects/ModifyBudget.aspx?projectId={projectId}");
        }
    } 
 }


