using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public partial class ProjectController : System.Web.UI.Page
    {
        //simulated database of projects
        private List<Project> _projects;
        protected void Page_Load(object sender, EventArgs e) 
        { 
            if (!IsPostBack) 
            {
                initializeSampleProjects();
                Session["Projects"] = _projects;
            } 
        }
        public void initializeSampleProjects()
        {
            _projects= new List<Project>
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
                    TotalTechnicianPayments = 54000,
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
                    TotalTechnicianPayments = 110000,
                }
            };
        }
        public List<Project> GetProjects()
        {
            //fetch projects from database
            return _projects;
        }
        public Project GetProjectById(int projectId)
        {
            return _projects.Find(p => p.ProjectId == projectId);
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

        /*public void SaveTimeEntries(int userId, int projectId, List<TimeEntry> timeEntries, string deliverables, double totalPayment)
        {
            // Save the time entries and payment details to the database
        }*/

        public void UpdateProjectBudget(int projectId, decimal newBudget)
        {
            var project = GetProjectById(projectId);
            if (project != null)
            {
                project.Budget = newBudget;
            }
        }
        protected bool CanViewFinancials()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Admin" || userRole == "ProjectManager";
        }
        protected bool CanModifyResources()
        {
            string userRole = Session["UserRole"].ToString();
            return userRole == "Technician" || userRole == "ProjectManager";
        }
        protected bool CanModifyTechnicians()
        {
            string userRole = Session["UserRole"].ToString();
            return userRole == "ProjectManager";
        }
        protected bool CanModifyBudget()
        {
            string userRole = Session["UserRole"]?.ToString();
            return userRole == "Admin";
        }
        protected bool CanCreateProject()
        {
            string userRole = Session["UserRole"].ToString();
            return userRole == "ProjectManager";
        }
        /*
        private void LoadProjects()
        {
            // Load projects into a view (e.g., a GridView or ListView)
            // This is where you would bind the projects to a control
        }
        private void BindProjects()
        { 
            // Retrieve and bind project data
        } 
        protected void CreateProject_Click(object sender, EventArgs e) 
        { 
            //create a new project
            /*var project = new Project { 
                ProjectName = txtProjectName.Text, 
                Description = txtDescription.Text, 
                Location = txtLocation.Text, 
                Budget = decimal.Parse(txtBudget.Text), 
                StartDate = DateTime.Parse(txtStartDate.Text), 
                EndDate = DateTime.Parse(txtEndDate.Text), 
                Status = "In Progress" 
            }; */
            // Save project to database
         //   } 
      /*  protected void UpdateProjectStatus_Click(object sender, EventArgs e) 
        { 
            // Update project status logic
        } */
    }
 }