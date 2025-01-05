using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class Modifybudget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int projectId = Convert.ToInt32(Request.QueryString["id"]);
                LoadProjectDetails(projectId);
            }
        }

        private void LoadProjectDetails(int projectId)
        {
            ProjectController projectController = new ProjectController();
            Project project = projectController.GetProjectById(projectId);
            if (project != null)
            {
                litProjectName.Text = project.ProjectName;
                litCurrentBudget.Text = project.Budget.ToString("F2"); // Format to 2 decimal places
            }
            else
            {
                lblMessage.Text = "Project not found.";
            }
        }

        protected void btnUpdateBudget_Click(object sender, EventArgs e)
        {
            int projectId = Convert.ToInt32(Request.QueryString["id"]);
            decimal newBudget;

            if (decimal.TryParse(txtNewBudget.Text.Trim(), out newBudget))
            {
                ProjectController projectController = new ProjectController();
                Project project = projectController.GetProjectById(projectId);

                if (project != null)
                {
                    // Check if the user has permission to modify the budget
                    string userRole = Session["UserRole"]?.ToString();
                    if (userRole == "Admin")
                    {
                        projectController.UpdateProjectBudget(projectId, newBudget);
                        lblMessage.Text = "Budget updated successfully.";
                    }
                    else
                    {
                        lblMessage.Text = "You do not have permission to modify the budget.";
                    }
                }
                else
                {
                    lblMessage.Text = "Project not found.";
                }
            }
            else
            {
                lblMessage.Text = "Invalid budget value.";
            }
        }
    }
}