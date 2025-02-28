using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

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

            //Load project managers only on the first page load
            if (!IsPostBack)
            {
                LoadProjectManagers();
            }
        }
        private void LoadProjectManagers()
        {
            // Load project managers from the database
            // Example: ddlProjectManager.DataSource = GetProjectManagers();
            // ddlProjectManager.DataTextField = "Name";
            // ddlProjectManager.DataValueField = "Id";
            // ddlProjectManager.DataBind();

            // Simulate loading project managers
            ddlProjectManager.Items.Add(new ListItem("Select a Project Manager", ""));
            ddlProjectManager.Items.Add(new ListItem("Project Manager 1", "1"));
            ddlProjectManager.Items.Add(new ListItem("Project Manager 2", "2"));
            ddlProjectManager.Items.Add(new ListItem("Project Manager 3", "3"));
        }
        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            string projectName = txtProjectName.Text;
            string description = txtDescription.Text;
            string location = txtLocation.Text;
            DateTime startTime = DateTime.Parse(txtStartTime.Text);
            DateTime endTime = DateTime.Parse(txtEndTime.Text);
            decimal technicianCost = decimal.Parse(txtTechnicianCost.Text);
            decimal materialsCost = decimal.Parse(txtMaterialsCost.Text);
            decimal budget = technicianCost + materialsCost; // Calculate total budget
            int projectManagerId = int.Parse(ddlProjectManager.SelectedValue);
            string resources = txtResources.Text;

            // Display the project details instead of saving to the database
            lblOutput.Text = $"Project Created Successfully!<br/>" +
                             $"Name: {projectName}<br/>" +
                             $"Description: {description}<br/>" +
                             $"Location: {location}<br/>" +
                             $"Start Time: {startTime}<br/>" +
                             $"End Time: {endTime}<br/>" +
                             $"Technician Cost: {technicianCost}<br/>" +
                             $"Materials Cost: {materialsCost}<br/>" +
                             $"Total Budget: {budget}<br/>" +
                             $"Project Manager ID: {projectManagerId}<br/>" +
                             $"Resources: {resources}";

            /*
            // Save the project to the database
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString))
            {
                string query = "INSERT INTO Projects (Name, Description, Location, StartTime, EndTime, Budget, ProjectManagerId, Resources) VALUES (@Name, @Description, @Location, @StartTime, @EndTime, @Budget, @ProjectManagerId, @Resources)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", projectName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Location", location);
                    cmd.Parameters.AddWithValue("@StartTime", startTime);
                    cmd.Parameters.AddWithValue("@EndTime", endTime);
                    cmd.Parameters.AddWithValue("@Budget", budget);
                    cmd.Parameters.AddWithValue("@ProjectManagerId", projectManagerId);
                    cmd.Parameters.AddWithValue("@Resources", resources);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // Redirect to a confirmation page or back to the project list
            Response.Redirect("~/Views/Projects/Projects.aspx"); */
        }

        decimal CalculateBudget()
        {
            // Implement logic to calculate the budget based on technician payment cost and cost of tools & materials
            return 0; // Placeholder
        }

    }
 }