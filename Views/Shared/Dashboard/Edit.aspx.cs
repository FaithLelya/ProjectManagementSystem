using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models; 

namespace ProjectManagementSystem.Views.Shared.Dashboard
{
    public partial class Edit : Page
    {
        // Simulated data store for projects
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int projectId;
                if (int.TryParse(Request.QueryString["id"], out projectId))
                {
                    var projects = Session["Projects"] as List<Project>;
                    var project = projects?.Find(p => p.ProjectId == projectId);
                    if (project != null)
                    {
                        txtProjectName.Text = project.ProjectName;
                        txtDescription.Text = project.Description;
                        txtBudget.Text = project.Budget.ToString();
                    }
                    else
                    {
                        Response.Redirect("Index.aspx"); // Redirect if not found
                    }
                }
                else
                {
                    Response.Redirect("Index.aspx"); // Redirect if invalid ID
                }
            }
        }

        protected void btnUpdateProject_Click(object sender, EventArgs e)
        {
            int projectId;
            if (int.TryParse(Request.QueryString["id"], out projectId))
            {
                var projects = Session["Projects"] as List<Project>;
                var project = projects?.Find(p => p.ProjectId == projectId);
                if (project != null)
                {
                    project.ProjectName = txtProjectName.Text;
                    project.Description = txtDescription.Text;
                    project.Budget = decimal.TryParse(txtBudget.Text, out var budget) ? budget : 0;

                    Response.Redirect("Index.aspx");
                }
            }
        }
    }
}