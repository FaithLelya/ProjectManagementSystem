using ProjectManagementSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Technicians
{
    public partial class TimeLogging : System.Web.UI.Page
    {
        private ProjectController _projectController;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _projectController = new ProjectController();
                LoadProjects();
            }
        }
        private void LoadProjects()
        {
            // Assuming GetProjects returns a list of projects
            var projects = _projectController.GetProjects();
            ddlProjects.DataSource = projects;
            ddlProjects.DataTextField = "ProjectName"; // Display name
            ddlProjects.DataValueField = "ProjectId"; // Value to use
            ddlProjects.DataBind();
        }
        /*protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkAgreeCompensation.Checked || !chkCorrectInfo.Checked)
                {
                    lblMessage.Text = "You must agree to HR's compensation rates and confirm the information is correct.";
                    return;
                }
                int userId = int.Parse(txt User Id.Text);
                int projectId = int.Parse(ddlProjects.SelectedValue);
                List<TimeEntry> timeEntries = new List<TimeEntry>();

                foreach (GridViewRow row in gvTimeEntries.Rows)
                {
                    var dateTextBox = (TextBox)row.Cells[0].FindControl("txtDate");
                    var hoursTextBox = (TextBox)row.Cells[1].FindControl("txtHours");

                    if (DateTime.TryParse(dateTextBox.Text, out DateTime date) &&
                        double.TryParse(hoursTextBox.Text, out double hours))
                    {
                        timeEntries.Add(new TimeEntry { Date = date, HoursWorked = hours });
                    }
                }

                string deliverables = txtDeliverables.Text;

                // Validate if the technician is assigned to the project
                if (!_projectController.IsTechnicianAssignedToProject(userId, projectId))
                {
                    lblMessage.Text = "You are not assigned to this project.";
                    return;
                }

                // Calculate payment
                double hourlyRate = _projectController.GetTechnicianHourlyRate(userId);
                double totalPayment = 0;

                foreach (var entry in timeEntries)
                {
                    totalPayment += entry.HoursWorked * hourlyRate;
                }

                // Save time entries and payment details to the database
                _projectController.SaveTimeEntries(userId, projectId, timeEntries, deliverables, totalPayment);

                lblMessage.Text = "Time entries submitted successfully!";
                //showSuccessPopup(); // Call the JavaScript function to show the popup
                btnViewCompensation.Visible = true; // Show the compensation button
            }
            catch (FormatException ex)
            {
                lblMessage.Text = "Please enter valid data. " + ex.Message;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
            }
        } 
        protected void btnViewCompensation_Click(object sender, EventArgs e)
        {
            // Redirect to the compensation details page
            Response.Redirect("CompensationDetails.aspx?userId=" + txtUser  Id.Text);
        }
        */
    }
}
