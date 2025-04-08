using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using ProjectManagementSystem.Helpers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class CreateProject : System.Web.UI.Page
    {
        // Session key for storing tasks
        private const string TASKS_SESSION_KEY = "ProjectTasks";

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

            if (!IsPostBack)
            {
                // Initialize tasks list in session
                Session[TASKS_SESSION_KEY] = new List<ProjectTask>();

                // Load project managers
                LoadProjectManagers();

                // Set minimum dates for start date (7 days from today) and end date
                SetMinimumDates();

                // Bind tasks repeater
                BindTasksRepeater();
            }
        }

        private void SetMinimumDates()
        {
            // Set min date to 7 days from today
            DateTime minStartDate = DateTime.Today.AddDays(7);

            // Apply min date to calendar
            calStartTime.SelectedDate = minStartDate;
            txtStartTime.Text = minStartDate.ToString("yyyy-MM-dd");

            // Min end date should be at least one day after start date
            DateTime minEndDate = minStartDate.AddDays(1);
            calEndTime.SelectedDate = minEndDate;
            txtEndTime.Text = minEndDate.ToString("yyyy-MM-dd");
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

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (!Page.IsValid)
            {
                lblOutput.Text = "Please fix the validation errors before submitting.";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                return;
            }

            try
            {
                // Get form values
                string ProjectName = txtProjectName.Text.Trim();
                string Description = txtDescription.Text.Trim();
                string Location = txtLocation.Text.Trim();
                DateTime StartDate = DateTime.Parse(txtStartTime.Text);
                DateTime EndDate = DateTime.Parse(txtEndTime.Text);

                // Validate costs - ensure they are at least 10,000
                decimal TechnicianPayment;
                decimal MaterialsCost;
                if (!decimal.TryParse(txtTechnicianCost.Text, out TechnicianPayment) || TechnicianPayment < 10000 ||
                    !decimal.TryParse(txtMaterialsCost.Text, out MaterialsCost) || MaterialsCost < 10000)
                {
                    lblOutput.Text = "Both Technician Payment and Materials Cost must be at least ₦10,000.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }
                // Validate that location has sufficient detail
                if (Location.Split(' ').Length < 3) // Check if location has at least 3 words for detailed address
                {
                    lblOutput.Text = "Please provide a more detailed location for the automation project (building name, floor, room, etc.).";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }

                // Calculate total budget
                decimal Budget = TechnicianPayment + MaterialsCost;

                // Validate manager selection
                if (string.IsNullOrEmpty(ddlProjectManager.SelectedValue))
                {
                    lblOutput.Text = "Please select a Project Manager.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }
                int ProjectManagerId = int.Parse(ddlProjectManager.SelectedValue);

                // Validate start date (must be at least 7 days from today)
                DateTime minStartDate = DateTime.Today.AddDays(7);
                if (StartDate < minStartDate)
                {
                    lblOutput.Text = $"Project start date must be at least 7 days from today ({minStartDate.ToString("yyyy-MM-dd")}).";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }

                // Validate end date (must be after start date)
                if (EndDate <= StartDate)
                {
                    lblOutput.Text = "End Date must be after Start Date.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }
                // Validate project duration (must be at least 2 weeks)
                TimeSpan projectDuration = EndDate - StartDate;
                if (projectDuration.TotalDays < 14) // 14 days = 2 weeks
                {
                    lblOutput.Text = "Project duration must be at least 2 weeks.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }
                // Validate maximum project duration (e.g., 6 months)
                if (projectDuration.TotalDays > 180) // 180 days = ~6 months
                {
                    lblOutput.Text = "Project duration cannot exceed 6 months for standard automation projects.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                    return;
                }

                // Get resources
                string Resources = txtResources.Text.Trim();

                // Get tasks from session (now optional)
                var tasks = Session[TASKS_SESSION_KEY] as List<ProjectTask>;

                // Debug info
                System.Diagnostics.Debug.WriteLine($"Inserting Project: {ProjectName}, {Description}, {Location}, {StartDate}, {EndDate}, {TechnicianPayment}, {MaterialsCost}, {Budget}, {ProjectManagerId}, {Resources}");

                // Save project to database
                int projectId = SQLiteHelper.InsertProject(ProjectName, Description, Location, StartDate, EndDate, TechnicianPayment, MaterialsCost, Budget, ProjectManagerId, Resources);

                // Save tasks for this project (if any tasks exist)
                if (tasks != null && tasks.Count > 0)
                {
                    DateTime defaultStartDate = DateTime.Now;
                    DateTime defaultEndDate = DateTime.Now.AddDays(7);

                    foreach (var task in tasks)
                    {
                        SQLiteHelper.InsertProjectTask(projectId, task.Name, task.Description, defaultStartDate, defaultEndDate, task.AssignedToUserId);
                    }
                }

                // Clear tasks from session
                Session[TASKS_SESSION_KEY] = null;

                // Display success message
                lblSuccess.Text = "Project Created Successfully!";
                lblSuccess.CssClass = lblSuccess.CssClass.Replace("d-none", "").Trim();

                // Redirect to projects list with delay
                Response.AddHeader("REFRESH", "2;URL=Projects.aspx");
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error creating project: {ex.Message}");
                lblOutput.Text = "An error occurred while creating the project. Please try again.";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
            }
        }

        protected void calStartTime_SelectionChanged(object sender, EventArgs e)
        {
            DateTime minStartDate = DateTime.Today.AddDays(7);
            if (calStartTime.SelectedDate < minStartDate)
            {
                // If selected date is before min date, force min date
                calStartTime.SelectedDate = minStartDate;
                lblOutput.Text = $"Start date must be at least 7 days from today ({minStartDate.ToString("yyyy-MM-dd")}).";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
            }

            txtStartTime.Text = calStartTime.SelectedDate.ToString("yyyy-MM-dd");

            // Update end date if it's now before or equal to start date
            DateTime endDate;
            if (DateTime.TryParse(txtEndTime.Text, out endDate) && endDate <= calStartTime.SelectedDate)
            {
                calEndTime.SelectedDate = calStartTime.SelectedDate.AddDays(1);
                txtEndTime.Text = calEndTime.SelectedDate.ToString("yyyy-MM-dd");
            }

            DateTime startDate;
            if (DateTime.TryParse(txtStartTime.Text, out startDate))
            {
                if (calEndTime.SelectedDate <= startDate)
                {
                    // If selected end date is on or before start date, set it to start date + 1
                    calEndTime.SelectedDate = startDate.AddDays(1);
                    lblOutput.Text = "End date must be after the start date.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                }
                else if ((calEndTime.SelectedDate - startDate).TotalDays < 14)
                {
                    // If project duration is less than 2 weeks, set end date to start date + 14 days
                    calEndTime.SelectedDate = startDate.AddDays(14);
                    lblOutput.Text = "Project duration must be at least 2 weeks.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                }
            }

            txtEndTime.Text = calEndTime.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void calEndTime_SelectionChanged(object sender, EventArgs e)
        {
            DateTime startDate;
            if (DateTime.TryParse(txtStartTime.Text, out startDate))
            {
                if (calEndTime.SelectedDate <= startDate)
                {
                    // If selected end date is on or before start date, set it to start date + 1
                    calEndTime.SelectedDate = startDate.AddDays(1);
                    lblOutput.Text = "End date must be after the start date.";
                    lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                }
            }

            txtEndTime.Text = calEndTime.SelectedDate.ToString("yyyy-MM-dd");
        }

        // Task management methods
        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            string taskName = txtTaskName.Text.Trim();
            string taskDescription = txtTaskDescription.Text.Trim();
            DateTime taskDueDate;

            // Basic validation
            if (string.IsNullOrEmpty(taskName))
            {
                lblOutput.Text = "Task name is required.";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                return;
            }

            // Validate due date
            if (string.IsNullOrEmpty(txtTaskDueDate.Text) || !DateTime.TryParse(txtTaskDueDate.Text, out taskDueDate))
            {
                lblOutput.Text = "Please enter a valid due date for the task.";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                return;
            }

            // Ensure due date is not in the past
            if (taskDueDate < DateTime.Today)
            {
                lblOutput.Text = "Task due date cannot be in the past.";
                lblOutput.CssClass = lblOutput.CssClass.Replace("d-none", "").Trim();
                return;
            }

            // Create task object
            var task = new ProjectTask
            {
                Name = taskName,
                Description = taskDescription,
                EndDate = taskDueDate
            };

            // Add to session
            var tasks = Session[TASKS_SESSION_KEY] as List<ProjectTask>;
            if (tasks == null)
            {
                tasks = new List<ProjectTask>();
                Session[TASKS_SESSION_KEY] = tasks;
            }

            tasks.Add(task);

            // Clear form fields
            txtTaskName.Text = string.Empty;
            txtTaskDescription.Text = string.Empty;
            txtTaskDueDate.Text = string.Empty;

            // Rebind tasks
            BindTasksRepeater();

            // Show success message
            lblSuccess.Text = "Task added successfully.";
            lblSuccess.CssClass = lblSuccess.CssClass.Replace("d-none", "").Trim();
        }

        private void BindTasksRepeater()
        {
            var tasks = Session[TASKS_SESSION_KEY] as List<ProjectTask>;
            rptTasks.DataSource = tasks;
            rptTasks.DataBind();
        }

        protected void cvTasksRequired_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var tasks = Session[TASKS_SESSION_KEY] as List<ProjectTask>;
            args.IsValid = (tasks != null && tasks.Count > 0);
        }
    }

    
}