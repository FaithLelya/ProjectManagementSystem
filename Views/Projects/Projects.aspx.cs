using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class Projects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
                BindGrid(); 
            }
        }

        private void BindGrid()
        { 
            // Sample data for demonstration
            var projects = new List<Project> { 
                new Project { ProjectId = 1, ProjectName = "Project 1", Budget = 1000M }, 
                new Project { ProjectId = 2, ProjectName = "Project 2", Budget = 2000M } 
            }; 
            GridView1.DataSource = projects; GridView1.DataBind(); 
        } 
    }

}
