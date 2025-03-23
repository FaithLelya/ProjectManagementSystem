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
        private ProjectController _projectController;
        private int _projectId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize controller
            _projectController = new ProjectController();

            if (!IsPostBack)
            {
                // First try to get project ID from query string
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _projectId))
                {
                    LoadProjectDetails(_projectId);
                }
                // If not in query string, try to get from session (user clicked from projects list)
                else if (Session["SelectedProjectId"] != null && int.TryParse(Session["SelectedProjectId"].ToString(), out _projectId))
                {
                    LoadProjectDetails(_projectId);
                }
                // If still not found, try to get the latest project (fallback)
                else
                {
                    try
                    {
                        // Get the most recent project as a fallback
                        Project latestProject = _projectController.GetLatestProject();
                        if (latestProject != null)
                        {
                            _projectId = latestProject.ProjectId;
                            LoadProjectDetails(_projectId);
                        }
                        else
                        {
                            lblMessage.Text = "No projects found in the system.";
                            DisableForm();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error retrieving project: " + ex.Message;
                        DisableForm();
                    }
                }
            }
        }

        private void LoadProjectDetails(int projectId)
        {
            try
            {
                Project project = _projectController.GetProjectById(projectId);
                if (project != null)
                {
                    litProjectName.Text = project.ProjectName;
                    litCurrentBudget.Text = project.Budget.ToString("F2"); // Format to 2 decimal places

                    // Store the project ID in ViewState for retrieval during postback
                    ViewState["ProjectId"] = projectId;

                    // Check permissions
                    string userRole = Session["UserRole"]?.ToString();
                    if (userRole != "Admin" && userRole != "ProjectManager")
                    {
                        lblMessage.Text = "You do not have permission to modify the budget.";
                        DisableForm();
                    }
                }
                else
                {
                    lblMessage.Text = "Project not found. Please check the project ID.";
                    DisableForm();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading project: " + ex.Message;
                DisableForm();
            }
        }

        private void DisableForm()
        {
            txtNewBudget.Enabled = false;
            btnUpdateBudget.Enabled = false;
        }

        protected void btnUpdateBudget_Click(object sender, EventArgs e)
        {
            // Get project ID from ViewState (more reliable than query string during postback)
            int projectId = 0;
            if (ViewState["ProjectId"] != null && int.TryParse(ViewState["ProjectId"].ToString(), out projectId))
            {
                // Verify user has permission
                string userRole = Session["UserRole"]?.ToString();
                if (userRole != "Admin" && userRole != "ProjectManager")
                {
                    lblMessage.Text = "You do not have permission to modify the budget.";
                    return;
                }

                // Verify budget value
                if (!decimal.TryParse(txtNewBudget.Text.Trim(), out decimal newBudget))
                {
                    lblMessage.Text = "Invalid budget value. Please enter a valid number.";
                    return;
                }

                // Additional validation for negative values
                if (newBudget < 0)
                {
                    lblMessage.Text = "Budget cannot be negative.";
                    return;
                }

                try
                {
                    // Get project and update budget
                    Project project = _projectController.GetProjectById(projectId);
                    if (project != null)
                    {
                        _projectController.UpdateProjectBudget(projectId, newBudget);
                        lblMessage.Text = "Budget updated successfully.";
                        lblMessage.CssClass = "message-success mt-3";
                        // Update displayed current budget
                        litCurrentBudget.Text = newBudget.ToString("F2");
                    }
                    else
                    {
                        lblMessage.Text = "Project not found.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error updating budget: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Invalid project ID.";
            }
        }
    }
}