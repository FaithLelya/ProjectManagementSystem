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

            //Load project managers only on the first page load
            if (!IsPostBack)
            {
                LoadProjectManagers();
            }
        }
        private void LoadProjectManagers()
        {
            // Load project managers from the database
            List<User> projectManagers = SQLiteHelper.GetProjectManagers();
            ddlProjectManager.DataSource = projectManagers;
            ddlProjectManager.DataTextField = "Username"; // Displayed text in the dropdown
            ddlProjectManager.DataValueField = "UserId"; // Value sent to the server
            ddlProjectManager.DataBind();

            // default item
            ddlProjectManager.Items.Insert(0, new ListItem("Select a Project Manager", ""));
        }
        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            string ProjectName = txtProjectName.Text;
            string Description = txtDescription.Text;
            string Location = txtLocation.Text;
            DateTime StartDate = DateTime.Parse(txtStartTime.Text);
            DateTime EndDate = DateTime.Parse(txtEndTime.Text);
            decimal TechnicianPayment = decimal.Parse(txtTechnicianCost.Text);
            decimal MaterialsCost = decimal.Parse(txtMaterialsCost.Text);
            decimal Budget = TechnicianPayment + MaterialsCost; // Calculate total budget
            int ProjectManagerId = int.Parse(ddlProjectManager.SelectedValue);
            string Resources = txtResources.Text;

            //debug
            System.Diagnostics.Debug.WriteLine($"Inserting Project: {ProjectName}, {Description}, {Location}, {StartDate}, {EndDate}, {TechnicianPayment}, {MaterialsCost}, {Budget}, {ProjectManagerId}, {Resources}");
            // Save to database
            SQLiteHelper.InsertProject(ProjectName, Description, Location, StartDate, EndDate, TechnicianPayment, MaterialsCost, Budget, ProjectManagerId, Resources);
            // Display success message
            lblOutput.Text = "Project Created Successfully!";

            // Optionally, redirect to the Projects page or refresh the project list
            Response.Redirect("~/Views/Projects/Projects.aspx");
        
        }
        protected void calStartTime_SelectionChanged(object sender, EventArgs e)
        {
            txtStartTime.Text = calStartTime.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void calEndTime_SelectionChanged(object sender, EventArgs e)
        {
            txtEndTime.Text = calEndTime.SelectedDate.ToString("yyyy-MM-dd");
        }

        decimal CalculateBudget()
        {
            // Implement logic to calculate the budget based on technician payment cost and cost of tools & materials
            return 0; // Placeholder
        }

    }
 }