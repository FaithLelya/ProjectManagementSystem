using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models; 

namespace ProjectManagementSystem.Views.Shared.Dashboard
{
    public partial class Index : Page
    {
        // Simulated data store for projects
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProjects();
            }
        }
        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            var project = new Project
            {
                ProjectId = (Session["Projects"] as List<Project>)?.Count + 1 ?? 1, // Simple ID assignment
                ProjectName = txtProjectName.Text,
                Description = txtDescription.Text,
                Budget = decimal.TryParse(txtBudget.Text, out var budget) ? budget : 0,
                Status = "In Progress" // Default status
            };

            var projects = Session["Projects"] as List<Project> ?? new List<Project>();
            projects.Add(project);
            Session["Projects"] = projects;

            BindProjects();

            // Clear input fields
            txtProjectName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBudget.Text = string.Empty;
        }

        private void BindProjects()
        {
            gvProjects.DataSource = Session["Projects"] as List<Project>;
            gvProjects.DataBind();
        }

        protected void EditProject_Command(object sender, CommandEventArgs e)
        {
            int projectId = Convert.ToInt32(e.CommandArgument);

            // Retrieve the list of projects from session
            var projects = Session["Projects"] as List<Project>;

            // Check if the projects list is not null
            if (projects != null)
            {
                var project = projects.Find(p => p.ProjectId == projectId);
                if (project != null)
                {
                    // Redirect to the edit page with the project ID
                    Response.Redirect($"Edit.aspx?id={projectId}");
                }
            }
            else
            {
                // Handle the case where the projects list is null (e.g., redirect or show a message)
                Response.Redirect("Index.aspx"); // Redirect to the index if no projects are found
            }
        }
    }
}