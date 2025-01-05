using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;

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
            var userRole = Session["User Role"]?.ToString();
            // Retrieve the list of projects from the session
            var projects = Session["Projects"] as List<Project>;

            if(projects != null & projects.Count > 0)
            {
                ProjectRepeater.DataSource = projects;
                ProjectRepeater.DataBind();
            }
            
        }
        public bool CanViewFinancials()
        {
            string userRole = Session["User Role"]?.ToString();
            return userRole == "Admin" || userRole == "ProjectManager";
        }

        public bool CanModifyResources()
        {
            string userRole = Session["User Role"]?.ToString();
            return userRole == "Technician" || userRole == "ProjectManager";
        }
        public bool CanModifyTechnicians()
        {
            string userRole = Session["User Role"]?.ToString();
            return userRole == "ProjectManager";
        }

        public bool CanModifyBudget()
        {
            string userRole = Session["User Role"]?.ToString();
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


