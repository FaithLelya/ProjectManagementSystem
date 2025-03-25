using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Helpers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class CreateProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in and is an Admin
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Prevent redirect loop by checking if the current page is not the login page
                if (!Request.Url.AbsolutePath.EndsWith("Login.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/Views/Shared/Login.aspx");
                    Context.ApplicationInstance.CompleteRequest(); // Prevents the thread from aborting
                }
            }

            // Load project managers and users only on the first page load
            if (!IsPostBack)
            {
                LoadProjectManagers();
                LoadTaskAssignedToUsers();
            }
        }

        private void LoadProjectManagers()
        {
            // Load project managers from the database
            List<ProjectManager> projectManagers = SQLiteHelper.GetProjectManagers();
            ddlProjectManager.DataSource = projectManagers;
            ddlProjectManager.DataTextField = "Username"; // Displayed text in the dropdown
            ddlProjectManager.DataValueField = "UserId"; // Value sent to the server
            ddlProjectManager.DataBind();

            // default item
            ddlProjectManager.Items.Insert(0, new ListItem("Select a Project Manager", ""));
        }

        private void LoadTaskAssignedToUsers()
        {
            // Load all users who can be assigned tasks
            List<User> users = SQLiteHelper.GetAllUsers();
            ddlTaskAssignedTo.DataSource = users;
            ddlTaskAssignedTo.DataTextField = "Username";
            ddlTaskAssignedTo.DataValueField = "UserId";
            ddlTaskAssignedTo.DataBind();

            // default item
            ddlTaskAssignedTo.Items.Insert(0, new ListItem("Select Assignee", ""));
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            // Validate main project fields
            if (!ValidateProjectFields())
            {
                return;
            }

            // Create project details
            string projectName = txtProjectName.Text;
            string description = txtDescription.Text;
            string location = txtLocation.Text;
            DateTime startDate = DateTime.Parse(txtStartTime.Text);
            DateTime endDate = DateTime.Parse(txtEndTime.Text);
            decimal technicianPayment = decimal.Parse(txtTechnicianCost.Text);
            decimal materialsCost = decimal.Parse(txtMaterialsCost.Text);
            decimal budget = technicianPayment + materialsCost;
            int projectManagerId = int.Parse(ddlProjectManager.SelectedValue);
            string resources = txtResources.Text;

            // Insert project and get the new project ID
            int newProjectId = SQLiteHelper.InsertProject(projectName, description, location, startDate, endDate, 
                                                          technicianPayment, materialsCost, budget, 
                                                          projectManagerId, resources);

            // Create tasks for the project
            CreateProjectTasks(newProjectId);

            // Display success message
            lblOutput.Text = "Project Created Successfully with Initial Tasks!";

            // Redirect to projects page
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }

        private void CreateProjectTasks(int projectId)
        {
            // Get all task inputs from the form
            string[] taskNames = Request.Form.GetValues("ctl00$form1$txtTaskName");
            string[] taskDescriptions = Request.Form.GetValues("ctl00$form1$txtTaskDescription");
            string[] taskStartDates = Request.Form.GetValues("ctl00$form1$txtTaskStartDate");
            string[] taskEndDates = Request.Form.GetValues("ctl00$form1$txtTaskEndDate");
            string[] taskAssignedToValues = Request.Form.GetValues("ctl00$form1$ddlTaskAssignedTo");

            // Validate and insert tasks
            if (taskNames != null)
            {
                for (int i = 0; i < taskNames.Length; i++)
                {
                    // Skip empty tasks
                    if (string.IsNullOrWhiteSpace(taskNames[i]))
                        continue;

                    DateTime taskStartDate, taskEndDate;
                    int assignedToUserId = 0;

                    // Parse dates if provided
                    if (DateTime.TryParse(taskStartDates[i], out taskStartDate) &&
                        DateTime.TryParse(taskEndDates[i], out taskEndDate) &&
                        int.TryParse(taskAssignedToValues[i], out assignedToUserId) &&
                        assignedToUserId > 0)
                    {
                        // Insert task into database
                        SQLiteHelper.InsertProjectTask(
                            projectId, 
                            taskNames[i], 
                            taskDescriptions[i], 
                            taskStartDate, 
                            taskEndDate, 
                            assignedToUserId
                        );
                    }
                }
            }
        }

        private bool ValidateProjectFields()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtProjectName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtLocation.Text) ||
                string.IsNullOrWhiteSpace(txtStartTime.Text) ||
                string.IsNullOrWhiteSpace(txtEndTime.Text) ||
                string.IsNullOrWhiteSpace(txtTechnicianCost.Text) ||
                string.IsNullOrWhiteSpace(txtMaterialsCost.Text) ||
                ddlProjectManager.SelectedValue == "")
            {
                lblOutput.Text = "Please fill in all required project fields.";
                return false;
            }

            // Validate date range
            DateTime startDate, endDate;
            if (!DateTime.TryParse(txtStartTime.Text, out startDate) ||
                !DateTime.TryParse(txtEndTime.Text, out endDate) ||
                startDate >= endDate)
            {
                lblOutput.Text = "Start Date must be before End Date.";
                return false;
            }

            // Validate numeric fields
            decimal technicianPayment, materialsCost;
            if (!decimal.TryParse(txtTechnicianCost.Text, out technicianPayment) || technicianPayment < 0 ||
                !decimal.TryParse(txtMaterialsCost.Text, out materialsCost) || materialsCost < 0)
            {
                lblOutput.Text = "Technician Payment and Materials Cost must be valid non-negative numbers.";
                return false;
            }

            return true;
        }

        protected void calStartTime_SelectionChanged(object sender, EventArgs e)
        {
            txtStartTime.Text = calStartTime.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void calEndTime_SelectionChanged(object sender, EventArgs e)
        {
            txtEndTime.Text = calEndTime.SelectedDate.ToString("yyyy-MM-dd");
        }
    }
}