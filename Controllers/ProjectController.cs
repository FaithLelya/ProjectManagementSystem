using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public partial class ProjectController : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) 
        { 
            if (!IsPostBack) 
            { 
                BindProjects(); 
            } 
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
            } 
        protected void UpdateProjectStatus_Click(object sender, EventArgs e) 
        { 
            // Update project status logic
        } 
    }
 }